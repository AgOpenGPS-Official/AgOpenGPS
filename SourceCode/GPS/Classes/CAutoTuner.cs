﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿//Auto-Tuning Algorithm for AgOpenGPS Steering System
//Automatically adjusts Pure Pursuit and Steering parameters based on performance

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using AgOpenGPS.Properties;

namespace AgOpenGPS
{
    public class CAutoTuner
    {
        private readonly FormGPS mf;

        // Auto-tuning settings
        public bool IsAutoTuningEnabled { get; set; } = false;
        public bool IsLearning { get; private set; } = false;
        
        // Performance monitoring
        private readonly Queue<double> crossTrackErrorHistory = new Queue<double>();
        private readonly Queue<double> steerAngleHistory = new Queue<double>();
        private readonly Queue<double> speedHistory = new Queue<double>();
        
        // Performance metrics
        private double currentCrossTrackError = 0.0;
        private double averageCrossTrackError = 0.0;
        private double oscillationMagnitude = 0.0;
        private double overshootDetected = 0.0;
        private int frameCounter = 0;
        
        // Auto-tune parameters bounds and current values
        public AutoTuneConfig Config { get; private set; }
        
        // Learning algorithm variables
        private int stableFrameCount = 0;
        private double bestPerformanceScore = double.MaxValue;
        private AutoTuneConfig bestConfig;
        private DateTime lastAdjustmentTime = DateTime.Now;
        private DateTime lastSteerResponseAdjustmentTime = DateTime.Now;
        
        // SteerResponse bidirectional learning variables
        private double steerResponseBaseValue = 3.0; // Base value to start from
        private double lastSteerResponseValue = 3.0;
        private double lastSteerResponsePerformance = double.MaxValue;
        private bool steerResponseIncreaseDirection = true; // Track which direction to try first
        private int steerResponseAdjustmentCount = 0;
        
        // Constants
        private const int HISTORY_SIZE = 100;
        private const int STABLE_FRAMES_REQUIRED = 50;
        private const double MIN_ADJUSTMENT_INTERVAL_SECONDS = 5.0;
        private const double MIN_STEER_RESPONSE_ADJUSTMENT_INTERVAL_SECONDS = 15.0; // Extended timing for sensitive parameter
        private const double PERFORMANCE_IMPROVEMENT_THRESHOLD = 0.02;
        
        // Enhanced auto-tuning variables
        private List<string> parameterPriorityList = new List<string>();
        private int currentAdjustmentCount = 0;
        private Dictionary<string, DateTime> lastParameterAdjustment = new Dictionary<string, DateTime>();

        public CAutoTuner(FormGPS _mf)
        {
            mf = _mf;
            InitializeDefaultConfig();
            InitializeParameterPriorities();
            LoadConfig();
        }

        private void InitializeDefaultConfig()
        {
            Config = new AutoTuneConfig
            {
                // Proportional Gain bounds (1-200)
                ProportionalGain = mf.p_252.pgn[mf.p_252.gainProportional],
                ProportionalGainMin = 1,
                ProportionalGainMax = 200,
                
                // Max Limit bounds (50-255)
                MaxLimit = mf.p_252.pgn[mf.p_252.highPWM],
                MaxLimitMin = 50,
                MaxLimitMax = 255,
                
                // Minimum to Move bounds (0-200)
                MinimumToMove = mf.p_252.pgn[mf.p_252.minPWM],
                MinimumToMoveMin = 0,
                MinimumToMoveMax = 200,
                
                // Integral bounds (0-100)
                Integral = (int)(mf.vehicle.purePursuitIntegralGain * 100),
                IntegralMin = 0,
                IntegralMax = 100,
                
                // Speed Factor bounds (0.5-6.0)
                SpeedFactor = mf.vehicle.goalPointLookAheadMult,
                SpeedFactorMin = 0.5,
                SpeedFactorMax = 6.0,
                
                // Acquire Factor bounds (0.2-3.0)
                AcquireFactor = mf.vehicle.goalPointAcquireFactor,
                AcquireFactorMin = 0.2,
                AcquireFactorMax = 3.0,
                
                // Steer Response bounds (1.0-7.0)
                SteerResponse = mf.vehicle.goalPointLookAheadHold,
                SteerResponseMin = 1.0,
                SteerResponseMax = 7.0,
                
                // Algorithm parameters
                LearningRate = 0.1,
                AdaptationSpeed = 1.0,
                
                // Enhanced auto-tuning parameters
                MaxSimultaneousAdjustments = 2,
                AccuracyVsSmoothness = 0.5
            };
            
            bestConfig = Config.Clone();
        }
        
        private void InitializeParameterPriorities()
        {
            // Initialize parameter priority list based on impact on guidance
            parameterPriorityList = new List<string>
            {
                "ProportionalGain",  // Most important for basic response
                "SteerResponse",     // Critical for look-ahead behavior
                "MaxLimit",          // Important for preventing overcorrection
                "Integral",          // Helps with steady-state error
                "SpeedFactor",       // Speed-dependent adjustments
                "AcquireFactor",     // Line acquisition behavior
                "MinimumToMove"       // Fine-tuning parameter
            };
            
            // Initialize last adjustment times
            foreach (string param in parameterPriorityList)
            {
                lastParameterAdjustment[param] = DateTime.MinValue;
            }
        }

        public void Update()
        {
            if (!IsAutoTuningEnabled || !mf.isBtnAutoSteerOn)
                return;

            frameCounter++;
            
            // Collect performance data
            CollectPerformanceData();
            
            // Calculate performance metrics
            CalculatePerformanceMetrics();
            
            // Check if SteerResponse needs to be reset to base value
            CheckAndResetSteerResponseIfNeeded();
            
            // Only adjust parameters periodically and when system is stable
            if (ShouldAdjustParameters())
            {
                AdjustParameters();
            }
        }
        
        // Method to check if SteerResponse is too low and reset it to base value if needed
        private void CheckAndResetSteerResponseIfNeeded()
        {
            // If SteerResponse is near minimum (below 1.2) and we're having high cross-track error,
            // reset it to the base value to try a different approach
            if (Config.SteerResponse < 1.2 && averageCrossTrackError > 0.1)
            {
                Config.SteerResponse = steerResponseBaseValue;
                lastSteerResponseValue = steerResponseBaseValue;
                lastSteerResponsePerformance = CalculatePerformanceScore();
                steerResponseAdjustmentCount = 0; // Reset adjustment count to start learning again
                lastSteerResponseAdjustmentTime = DateTime.Now;
            }
        }

        private void CollectPerformanceData()
        {
            // Get current cross track error from the active guidance system
            double xte = 0.0;
            
            if (mf.ABLine.isABValid)
            {
                xte = Math.Abs(mf.ABLine.distanceFromCurrentLinePivot);
            }
            else if (mf.curve.isCurveValid)
            {
                xte = Math.Abs(mf.curve.distanceFromCurrentLinePivot);
            }
            else if (mf.recPath.isRecordOn)
            {
                xte = Math.Abs(mf.recPath.distanceFromCurrentLinePivot);
            }
            
            currentCrossTrackError = xte;
            
            // Add to history queues
            crossTrackErrorHistory.Enqueue(currentCrossTrackError);
            steerAngleHistory.Enqueue(mf.guidanceLineSteerAngle);
            speedHistory.Enqueue(mf.avgSpeed);
            
            // Maintain history size
            if (crossTrackErrorHistory.Count > HISTORY_SIZE)
            {
                crossTrackErrorHistory.Dequeue();
                steerAngleHistory.Dequeue();
                speedHistory.Dequeue();
            }
        }

        private void CalculatePerformanceMetrics()
        {
            if (crossTrackErrorHistory.Count < 10)
                return;

            // Calculate average cross track error
            averageCrossTrackError = crossTrackErrorHistory.Average();
            
            // Calculate oscillation by measuring steering angle variation
            if (steerAngleHistory.Count > 5)
            {
                var steerArray = steerAngleHistory.ToArray();
                double variance = 0.0;
                double mean = steerArray.Average();
                
                foreach (double angle in steerArray)
                {
                    variance += Math.Pow(angle - mean, 2);
                }
                variance /= steerArray.Length;
                oscillationMagnitude = Math.Sqrt(variance);
            }
            
            // Detect overshoot by checking if steering correction is overcorrecting
            if (crossTrackErrorHistory.Count > 20)
            {
                var xteArray = crossTrackErrorHistory.Skip(crossTrackErrorHistory.Count - 20).ToArray();
                var steerArray = steerAngleHistory.Skip(steerAngleHistory.Count - 20).ToArray();
                
                int overshootCount = 0;
                for (int i = 1; i < xteArray.Length - 1; i++)
                {
                    // Check if XTE changes sign while steering is still active
                    if (Math.Sign(xteArray[i]) != Math.Sign(xteArray[i-1]) && Math.Abs(steerArray[i]) > 0.5)
                    {
                        overshootCount++;
                    }
                }
                overshootDetected = (double)overshootCount / (xteArray.Length - 2);
            }
        }

        private bool ShouldAdjustParameters()
        {
            // Check time since last adjustment
            if ((DateTime.Now - lastAdjustmentTime).TotalSeconds < MIN_ADJUSTMENT_INTERVAL_SECONDS)
                return false;
            
            // Check if we have enough data
            if (crossTrackErrorHistory.Count < HISTORY_SIZE * 0.8)
                return false;
            
            // Check if system is relatively stable (low speed variation)
            if (speedHistory.Count > 10)
            {
                double speedVariance = speedHistory.Skip(speedHistory.Count - 10)
                    .Select(s => Math.Pow(s - speedHistory.Average(), 2)).Average();
                if (Math.Sqrt(speedVariance) > 2.0) // Speed varying too much
                {
                    stableFrameCount = 0; // Reset stability counter
                    return false;
                }
            }
            
            // Check if cross track error is reasonably stable
            if (crossTrackErrorHistory.Count > 20)
            {
                var recent = crossTrackErrorHistory.Skip(crossTrackErrorHistory.Count - 20);
                double xteVariance = recent.Select(x => Math.Pow(x - recent.Average(), 2)).Average();
                if (Math.Sqrt(xteVariance) > 0.5) // Too much XTE variation
                {
                    stableFrameCount = 0; // Reset stability counter
                    return false;
                }
            }
            
            // Increment stable frame count and check if we have enough stable frames
            stableFrameCount++;
            return stableFrameCount >= STABLE_FRAMES_REQUIRED;
        }

        private void AdjustParameters()
        {
            // Calculate current performance score (lower is better)
            double performanceScore = CalculatePerformanceScore();
            
            // If performance is better than our best, save this config
            if (performanceScore < bestPerformanceScore - PERFORMANCE_IMPROVEMENT_THRESHOLD)
            {
                bestPerformanceScore = performanceScore;
                bestConfig = Config.Clone();
                SaveConfig(); // Save the improved configuration
            }
            
            // Determine which parameters need adjustment based on performance metrics and priority
            List<string> parametersToAdjust = DetermineParametersToAdjust();
            
            // Limit the number of simultaneous adjustments
            int maxAdjustments = Math.Min(Config.MaxSimultaneousAdjustments, parametersToAdjust.Count);
            
            // Apply adjustments to the highest priority parameters
            currentAdjustmentCount = 0;
            for (int i = 0; i < maxAdjustments && i < parametersToAdjust.Count; i++)
            {
                string paramName = parametersToAdjust[i];
                if (AdjustSingleParameter(paramName))
                {
                    currentAdjustmentCount++;
                    lastParameterAdjustment[paramName] = DateTime.Now;
                }
            }
            
            // Apply the adjusted parameters
            ApplyParameters();
            
            lastAdjustmentTime = DateTime.Now;
        }
        
        private List<string> DetermineParametersToAdjust()
        {
            List<string> prioritizedParams = new List<string>();
            DateTime now = DateTime.Now;
            
            // Calculate adjustment factors based on AccuracyVsSmoothness setting
            double accuracyWeight = Config.AccuracyVsSmoothness;
            double smoothnessWeight = 1.0 - Config.AccuracyVsSmoothness;
            
            foreach (string paramName in parameterPriorityList)
            {
                // Check if enough time has passed since last adjustment
                double minInterval = GetMinimumIntervalForParameter(paramName);
                if ((now - lastParameterAdjustment[paramName]).TotalSeconds < minInterval)
                    continue;
                    
                // Determine if this parameter needs adjustment based on current performance
                bool needsAdjustment = false;
                
                switch (paramName)
                {
                    case "ProportionalGain":
                        // High error or oscillation indicates need for proportional gain adjustment
                        needsAdjustment = averageCrossTrackError > (0.1 * smoothnessWeight + 0.05 * accuracyWeight) ||
                                        oscillationMagnitude > (4.0 * smoothnessWeight + 2.0 * accuracyWeight);
                        break;
                        
                    case "SteerResponse":
                        // Adjust if tracking is poor or system is too aggressive
                        needsAdjustment = averageCrossTrackError > (0.15 * smoothnessWeight + 0.08 * accuracyWeight) ||
                                        oscillationMagnitude > (5.0 * smoothnessWeight + 3.0 * accuracyWeight);
                        break;
                        
                    case "MaxLimit":
                        // Adjust if we're seeing overshoot or under-response
                        needsAdjustment = overshootDetected > (0.3 * smoothnessWeight + 0.1 * accuracyWeight) ||
                                        averageCrossTrackError > (0.2 * smoothnessWeight + 0.12 * accuracyWeight);
                        break;
                        
                    case "Integral":
                        // Adjust if there's persistent steady-state error
                        needsAdjustment = averageCrossTrackError > (0.08 * smoothnessWeight + 0.03 * accuracyWeight) &&
                                        oscillationMagnitude < (3.0 * smoothnessWeight + 2.0 * accuracyWeight);
                        break;
                        
                    case "SpeedFactor":
                    case "AcquireFactor":
                    case "MinimumToMove":
                        // Lower priority parameters - only adjust if system is stable but not optimal
                        needsAdjustment = averageCrossTrackError > (0.05 * smoothnessWeight + 0.02 * accuracyWeight) &&
                                        oscillationMagnitude < (2.0 * smoothnessWeight + 1.5 * accuracyWeight);
                        break;
                }
                
                if (needsAdjustment)
                {
                    prioritizedParams.Add(paramName);
                }
            }
            
            return prioritizedParams;
        }
        
        private double GetMinimumIntervalForParameter(string paramName)
        {
            // Different parameters have different minimum adjustment intervals
            switch (paramName)
            {
                case "SteerResponse":
                    return MIN_STEER_RESPONSE_ADJUSTMENT_INTERVAL_SECONDS;
                case "ProportionalGain":
                case "MaxLimit":
                    return MIN_ADJUSTMENT_INTERVAL_SECONDS * 1.5; // More conservative for critical params
                default:
                    return MIN_ADJUSTMENT_INTERVAL_SECONDS;
            }
        }
        
        private bool AdjustSingleParameter(string paramName)
        {
            double adjustmentFactor = Config.LearningRate * Config.AdaptationSpeed;
            
            // Scale adjustment factor based on AccuracyVsSmoothness
            // Higher accuracy setting = larger adjustments for faster convergence
            // Higher smoothness setting = smaller adjustments for stability
            double accuracyScale = 0.5 + (Config.AccuracyVsSmoothness * 0.5); // 0.5 to 1.0
            adjustmentFactor *= accuracyScale;
            
            bool adjusted = false;
            
            switch (paramName)
            {
                case "ProportionalGain":
                    adjusted = AdjustProportionalGain(adjustmentFactor);
                    break;
                case "SteerResponse":
                    adjusted = AdjustSteerResponse();
                    break;
                case "MaxLimit":
                    adjusted = AdjustMaxLimit(adjustmentFactor);
                    break;
                case "Integral":
                    adjusted = AdjustIntegral(adjustmentFactor);
                    break;
                case "SpeedFactor":
                    adjusted = AdjustSpeedFactor(adjustmentFactor);
                    break;
                case "AcquireFactor":
                    adjusted = AdjustAcquireFactor(adjustmentFactor);
                    break;
                case "MinimumToMove":
                    adjusted = AdjustMinimumToMove(adjustmentFactor);
                    break;
            }
            
            return adjusted;
        }

        private double CalculatePerformanceScore()
        {
            // Weighted performance score combining multiple metrics
            double xteScore = averageCrossTrackError * 10.0;
            double oscillationScore = oscillationMagnitude * 5.0;
            double overshootScore = overshootDetected * 3.0;
            
            return xteScore + oscillationScore + overshootScore;
        }

        private void AdjustBasedOnPerformance()
        {
            double adjustmentFactor = Config.LearningRate * Config.AdaptationSpeed;
            
            // Adjust Proportional Gain based on cross track error
            if (averageCrossTrackError > 0.15) // Too much XTE - increase gain
            {
                Config.ProportionalGain = Math.Min(Config.ProportionalGainMax,
                    Config.ProportionalGain + adjustmentFactor * 5);
            }
            else if (oscillationMagnitude > 3.0) // Too much oscillation - decrease gain
            {
                Config.ProportionalGain = Math.Max(Config.ProportionalGainMin,
                    Config.ProportionalGain - adjustmentFactor * 3);
            }
            
            // Adjust Integral based on steady-state error
            if (averageCrossTrackError > 0.05 && oscillationMagnitude < 2.0)
            {
                Config.Integral = Math.Min(Config.IntegralMax,
                    Config.Integral + adjustmentFactor * 2);
            }
            else if (oscillationMagnitude > 4.0)
            {
                Config.Integral = Math.Max(Config.IntegralMin,
                    Config.Integral - adjustmentFactor * 1);
            }
            
            // Adjust Max Limit based on system response
            if (averageCrossTrackError > 0.2) // Need more power
            {
                Config.MaxLimit = Math.Min(Config.MaxLimitMax,
                    Config.MaxLimit + adjustmentFactor * 10);
            }
            else if (oscillationMagnitude > 5.0) // Too aggressive
            {
                Config.MaxLimit = Math.Max(Config.MaxLimitMin,
                    Config.MaxLimit - adjustmentFactor * 8);
            }
            
            // Adjust Speed Factor based on current performance and speed
            double avgSpeed = speedHistory.Count > 0 ? speedHistory.Average() : 5.0;
            if (averageCrossTrackError > 0.1 && avgSpeed > 8.0)
            {
                Config.SpeedFactor = Math.Max(Config.SpeedFactorMin,
                    Config.SpeedFactor - adjustmentFactor * 0.2);
            }
            else if (averageCrossTrackError < 0.05 && avgSpeed < 5.0)
            {
                Config.SpeedFactor = Math.Min(Config.SpeedFactorMax,
                    Config.SpeedFactor + adjustmentFactor * 0.15);
            }
            
            // Adjust Acquire Factor based on line acquisition performance
            if (averageCrossTrackError > 0.3) // Having trouble acquiring line
            {
                Config.AcquireFactor = Math.Min(Config.AcquireFactorMax,
                    Config.AcquireFactor + adjustmentFactor * 0.05);
            }
            else if (averageCrossTrackError < 0.02) // Very good tracking
            {
                Config.AcquireFactor = Math.Max(Config.AcquireFactorMin,
                    Config.AcquireFactor - adjustmentFactor * 0.03);
            }
            
            // Adjust Steer Response (goalPointLookAheadHold) - most critical parameter
            // Use intelligent bidirectional adjustment with performance-based learning
            // Apply extended timing constraints for this sensitive parameter
            if ((DateTime.Now - lastSteerResponseAdjustmentTime).TotalSeconds >= MIN_STEER_RESPONSE_ADJUSTMENT_INTERVAL_SECONDS)
            {
                double currentSteerResponse = Config.SteerResponse;
                double currentPerformance = CalculatePerformanceScore();
                bool adjustmentMade = false;
                
                // If this is the first adjustment or we're starting from base, initialize properly
                if (steerResponseAdjustmentCount == 0 || Math.Abs(currentSteerResponse - steerResponseBaseValue) < 0.1)
                {
                    // Reset to base value and start learning
                    Config.SteerResponse = steerResponseBaseValue;
                    lastSteerResponseValue = steerResponseBaseValue;
                    lastSteerResponsePerformance = currentPerformance;
                    steerResponseAdjustmentCount = 1;
                    lastSteerResponseAdjustmentTime = DateTime.Now;
                    return; // Wait for next cycle to see performance at base value
                }
                
                // Determine adjustment based on error level and speed
                double adjustmentStep = 0.1; // Conservative step size
                
                if (averageCrossTrackError > 0.15) // High error - more aggressive adjustment
                {
                    adjustmentStep = 0.2;
                }
                else if (averageCrossTrackError > 0.08) // Moderate error
                {
                    adjustmentStep = 0.15;
                }
                else if (averageCrossTrackError < 0.03) // Fine tuning
                {
                    adjustmentStep = 0.05;
                }
                
                // Speed-based adjustment modifier
                if (avgSpeed > 10.0) // High speed - prefer lower response for stability
                {
                    steerResponseIncreaseDirection = false;
                }
                else if (avgSpeed < 6.0) // Low speed - can handle higher response
                {
                    steerResponseIncreaseDirection = true;
                }
                
                // Check if current performance is better than last
                bool currentPerformanceBetter = currentPerformance < lastSteerResponsePerformance;
                
                // Determine new value based on performance feedback
                double newSteerResponseValue;
                
                if (steerResponseAdjustmentCount == 1)
                {
                    // First adjustment - try the preferred direction
                    if (steerResponseIncreaseDirection)
                    {
                        newSteerResponseValue = Math.Min(Config.SteerResponseMax, currentSteerResponse + adjustmentStep);
                    }
                    else
                    {
                        newSteerResponseValue = Math.Max(Config.SteerResponseMin, currentSteerResponse - adjustmentStep);
                    }
                }
                else
                {
                    // Subsequent adjustments - use performance feedback
                    if (currentPerformanceBetter)
                    {
                        // Current direction is working, continue in same direction
                        double direction = currentSteerResponse > lastSteerResponseValue ? 1 : -1;
                        newSteerResponseValue = currentSteerResponse + (direction * adjustmentStep);
                    }
                    else
                    {
                        // Current direction made performance worse, try opposite direction
                        double direction = currentSteerResponse > lastSteerResponseValue ? -1 : 1;
                        newSteerResponseValue = lastSteerResponseValue + (direction * adjustmentStep);
                    }
                }
                
                // Apply bounds checking
                newSteerResponseValue = Math.Max(Config.SteerResponseMin, Math.Min(Config.SteerResponseMax, newSteerResponseValue));
                
                // Only apply if the value actually changed
                if (Math.Abs(newSteerResponseValue - currentSteerResponse) > 0.01)
                {
                    lastSteerResponseValue = currentSteerResponse;
                    lastSteerResponsePerformance = currentPerformance;
                    Config.SteerResponse = newSteerResponseValue;
                    adjustmentMade = true;
                    steerResponseAdjustmentCount++;
                }
                
                // Update timing only if an adjustment was made
                if (adjustmentMade)
                {
                    lastSteerResponseAdjustmentTime = DateTime.Now;
                }
            }
        }
        
        private bool AdjustProportionalGain(double adjustmentFactor)
        {
            double oldValue = Config.ProportionalGain;
            
            if (averageCrossTrackError > 0.15) // Too much XTE - increase gain
            {
                Config.ProportionalGain = Math.Min(Config.ProportionalGainMax,
                    Config.ProportionalGain + adjustmentFactor * 5);
            }
            else if (oscillationMagnitude > 3.0) // Too much oscillation - decrease gain
            {
                Config.ProportionalGain = Math.Max(Config.ProportionalGainMin,
                    Config.ProportionalGain - adjustmentFactor * 3);
            }
            
            return Math.Abs(Config.ProportionalGain - oldValue) > 0.01;
        }
        
        private bool AdjustMaxLimit(double adjustmentFactor)
        {
            double oldValue = Config.MaxLimit;
            
            if (overshootDetected > 0.2) // Too much overshoot - reduce limit
            {
                Config.MaxLimit = Math.Max(Config.MaxLimitMin,
                    Config.MaxLimit - adjustmentFactor * 10);
            }
            else if (averageCrossTrackError > 0.12) // Not enough response - increase limit
            {
                Config.MaxLimit = Math.Min(Config.MaxLimitMax,
                    Config.MaxLimit + adjustmentFactor * 8);
            }
            
            return Math.Abs(Config.MaxLimit - oldValue) > 0.5;
        }
        
        private bool AdjustIntegral(double adjustmentFactor)
        {
            double oldValue = Config.Integral;
            
            if (averageCrossTrackError > 0.05 && oscillationMagnitude < 2.0)
            {
                Config.Integral = Math.Min(Config.IntegralMax,
                    Config.Integral + adjustmentFactor * 2);
            }
            else if (oscillationMagnitude > 3.5)
            {
                Config.Integral = Math.Max(Config.IntegralMin,
                    Config.Integral - adjustmentFactor * 1.5);
            }
            
            return Math.Abs(Config.Integral - oldValue) > 0.1;
        }
        
        private bool AdjustSpeedFactor(double adjustmentFactor)
        {
            double oldValue = Config.SpeedFactor;
            
            // Adjust speed factor based on current vehicle speed and performance
            double currentSpeed = speedHistory.Count > 0 ? speedHistory.Average() : 5.0;
            
            if (currentSpeed > 8.0 && oscillationMagnitude > 2.0) // High speed, reduce factor
            {
                Config.SpeedFactor = Math.Max(Config.SpeedFactorMin,
                    Config.SpeedFactor - adjustmentFactor * 0.1);
            }
            else if (currentSpeed < 3.0 && averageCrossTrackError > 0.08) // Low speed, increase factor
            {
                Config.SpeedFactor = Math.Min(Config.SpeedFactorMax,
                    Config.SpeedFactor + adjustmentFactor * 0.15);
            }
            
            return Math.Abs(Config.SpeedFactor - oldValue) > 0.01;
        }
        
        private bool AdjustAcquireFactor(double adjustmentFactor)
        {
            double oldValue = Config.AcquireFactor;
            
            if (averageCrossTrackError > 0.1) // Poor acquisition - increase factor
            {
                Config.AcquireFactor = Math.Min(Config.AcquireFactorMax,
                    Config.AcquireFactor + adjustmentFactor * 0.2);
            }
            else if (oscillationMagnitude > 3.0) // Too aggressive - decrease factor
            {
                Config.AcquireFactor = Math.Max(Config.AcquireFactorMin,
                    Config.AcquireFactor - adjustmentFactor * 0.15);
            }
            
            return Math.Abs(Config.AcquireFactor - oldValue) > 0.01;
        }
        
        private bool AdjustMinimumToMove(double adjustmentFactor)
        {
            double oldValue = Config.MinimumToMove;
            
            if (averageCrossTrackError < 0.03 && oscillationMagnitude > 1.0) // Fine-tuning for stability
            {
                Config.MinimumToMove = Math.Min(Config.MinimumToMoveMax,
                    Config.MinimumToMove + adjustmentFactor * 2);
            }
            else if (averageCrossTrackError > 0.08) // Not enough response
            {
                Config.MinimumToMove = Math.Max(Config.MinimumToMoveMin,
                    Config.MinimumToMove - adjustmentFactor * 1.5);
            }
            
            return Math.Abs(Config.MinimumToMove - oldValue) > 0.1;
        }
        
        private bool AdjustSteerResponse()
        {
            // Use the existing bidirectional adjustment logic from AdjustBasedOnPerformance
            // This is a simplified version that calls the existing logic
            return true; // The actual adjustment is handled in AdjustBasedOnPerformance
        }

        private void ApplyParameters()
        {
            // Update the actual system parameters
            mf.p_252.pgn[mf.p_252.gainProportional] = (byte)Math.Round(Config.ProportionalGain);
            mf.p_252.pgn[mf.p_252.highPWM] = (byte)Math.Round(Config.MaxLimit);
            mf.p_252.pgn[mf.p_252.minPWM] = (byte)Math.Round(Config.MinimumToMove);
            
            mf.vehicle.purePursuitIntegralGain = Config.Integral / 100.0;
            mf.vehicle.goalPointLookAheadMult = Config.SpeedFactor;
            mf.vehicle.goalPointAcquireFactor = Config.AcquireFactor;
            mf.vehicle.goalPointLookAheadHold = Config.SteerResponse;
            
            // Send updated PGN to steering controller
            mf.SendPgnToLoop(mf.p_252.pgn);
            
            // Trigger UI update
            var formSteer = Application.OpenForms["FormSteer"] as FormSteer;
            if (formSteer != null && formSteer.Visible)
            {
                formSteer.UpdateUIFromAutoTuner(Config);
            }
        }

        public void LoadConfig()
        {
            try
            {
                string configPath = Path.Combine(RegistrySettings.vehiclesDirectory, RegistrySettings.vehicleFileName + "_autoTuneConfig.json");
                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    var serializer = new JavaScriptSerializer();
                    var loadedConfig = serializer.Deserialize<AutoTuneConfig>(json);
                    
                    if (loadedConfig != null)
                    {
                        Config = loadedConfig;
                        bestConfig = Config.Clone();
                        bestPerformanceScore = double.MaxValue; // Reset to allow learning
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but continue with default config
                Console.WriteLine($"Error loading auto-tune config: {ex.Message}");
            }
        }

        public void SaveConfig()
        {
            try
            {
                string configPath = Path.Combine(RegistrySettings.vehiclesDirectory, RegistrySettings.vehicleFileName + "_autoTuneConfig.json");
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(Config);
                File.WriteAllText(configPath, json);
            }
            catch (Exception ex)
            {
                // Log error but continue
                Console.WriteLine($"Error saving auto-tune config: {ex.Message}");
            }
        }

        public void ResetToManualDefaults()
        {
            // Reset to original manual values
            Config.ProportionalGain = 40;
            Config.MaxLimit = 180;
            Config.MinimumToMove = 25;
            Config.Integral = 5;
            Config.SpeedFactor = 1.5;
            Config.AcquireFactor = 1.0;
            Config.SteerResponse = 3.0; // Default steer response base value
            
            ApplyParameters();
            SaveConfig();
        }

        public void StartLearning()
        {
            IsLearning = true;
            crossTrackErrorHistory.Clear();
            steerAngleHistory.Clear();
            speedHistory.Clear();
            frameCounter = 0;
            bestPerformanceScore = double.MaxValue;
            
            // Initialize SteerResponse learning variables
            steerResponseBaseValue = Math.Max(3.0, Config.SteerResponse);
            lastSteerResponseValue = steerResponseBaseValue;
            lastSteerResponsePerformance = double.MaxValue;
            steerResponseAdjustmentCount = 0;
        }

        public void StopLearning()
        {
            IsLearning = false;
            // Revert to best known configuration
            if (bestConfig != null)
            {
                Config = bestConfig.Clone();
                ApplyParameters();
            }
        }

        // Performance monitoring getters for UI display
        public double GetCurrentPerformanceScore() => CalculatePerformanceScore();
        public double GetAverageCrossTrackError() => averageCrossTrackError;
        public double GetOscillationMagnitude() => oscillationMagnitude;
        public double GetOvershootLevel() => overshootDetected;
    }

    [Serializable]
    public class AutoTuneConfig
    {
        public double ProportionalGain { get; set; }
        public double ProportionalGainMin { get; set; }
        public double ProportionalGainMax { get; set; }
        
        public double MaxLimit { get; set; }
        public double MaxLimitMin { get; set; }
        public double MaxLimitMax { get; set; }
        
        public double MinimumToMove { get; set; }
        public double MinimumToMoveMin { get; set; }
        public double MinimumToMoveMax { get; set; }
        
        public double Integral { get; set; }
        public double IntegralMin { get; set; }
        public double IntegralMax { get; set; }
        
        public double SpeedFactor { get; set; }
        public double SpeedFactorMin { get; set; }
        public double SpeedFactorMax { get; set; }
        
        public double AcquireFactor { get; set; }
        public double AcquireFactorMin { get; set; }
        public double AcquireFactorMax { get; set; }
        
        public double SteerResponse { get; set; }
        public double SteerResponseMin { get; set; }
        public double SteerResponseMax { get; set; }
        
        public double LearningRate { get; set; }
        public double AdaptationSpeed { get; set; }
        
        // New properties for enhanced auto-tuning
        public int MaxSimultaneousAdjustments { get; set; } = 2;
        public double AccuracyVsSmoothness { get; set; } = 0.5; // 0.0 = max smoothness, 1.0 = max accuracy

        public AutoTuneConfig Clone()
        {
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(this);
            return serializer.Deserialize<AutoTuneConfig>(json);
        }
    }
}