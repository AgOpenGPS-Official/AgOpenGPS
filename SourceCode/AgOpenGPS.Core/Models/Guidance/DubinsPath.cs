using AgOpenGPS.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgOpenGPS.Core.Models
{
    public abstract class DubinsPath
    {
        protected DubinsPathConstraints _constraints;

        protected GeoCircle _startCircle;
        protected GeoCircle _middleCircle;
        protected GeoCircle _goalCircle;

        protected GeoCoord _startTangent;
        protected GeoCoord _goalTangent;
        private bool _isComputed = false;

        protected double _length1, _length2, _length3;
        protected double _totalLength;

        protected abstract void ComputeTangents(
            GeoCircle startCircle,
            TurnType startTurnType,
            GeoCircle goalCircle,
            out GeoCoord startTangent,
            out GeoCoord goalTangent);

        // Technical note
        // DubinsPath does not call Compute() in the constructor, because it relies on the virtual function 
        // ComputeTangents. It delays computation to the moment that somebody asks for the Length
        public DubinsPath(
            DubinsPathConstraints constraints,
            TurnType startTurnType,
            TurnType middleTurnType,
            TurnType goalTurnType)
        {
            _constraints = constraints;
            StartTurnType = startTurnType;
            MiddleTurnType = middleTurnType;
            GoalTurnType = goalTurnType;
            _isComputed = false;
        }

        public TurnType StartTurnType { get; }
        public TurnType MiddleTurnType { get; }
        public TurnType GoalTurnType { get; }

        public double TotalLength
        {
            get
            {
                Compute();
                return _totalLength;
            }
        }

        public double Length1
        {
            get
            {
                Compute();
                return _length1;
            }
        }

        public double Length2
        {
            get
            {
                Compute();
                return _length2;
            }
        }

        public double Length3
        {
            get
            {
                Compute();
                return _length3;
            }
        }

        private void Compute()
        {
            if (_isComputed) return;
            _isComputed = true;
            _startCircle = ComputeCircle(_constraints.StartConstraint, _constraints.RadiusConstraint, StartTurnType);
            _goalCircle = ComputeCircle(_constraints.GoalConstraint, _constraints.RadiusConstraint, GoalTurnType);
            ComputeTangents(_startCircle, StartTurnType, _goalCircle, out _startTangent, out _goalTangent);
            _length1 = _startCircle.GetArcLength(_constraints.StartConstraint.Coord, _startTangent, StartTurnType);
            _length2 = MiddleTurnType == TurnType.Straight ?
                (_goalTangent - _startTangent).Length :
                _middleCircle.GetArcLength(_startTangent, _goalTangent, MiddleTurnType);
            _length3 = _goalCircle.GetArcLength(_goalTangent, _constraints.GoalConstraint.Coord, GoalTurnType);
            _totalLength = _length1 + _length2 + _length3;
        }

        public static GeoCircle ComputeCircle(GeoCoordDir cdOnCircle, double radius, TurnType turnType)
        {
            Debug.Assert(turnType != TurnType.Straight);
            GeoDir dir = turnType == TurnType.Right ? cdOnCircle.Direction.PerpendicularRight : cdOnCircle.Direction.PerpendicularLeft;
            return new GeoCircle(cdOnCircle.Coord + radius * dir, radius);
        }
    }

    // Base class of LslDubinsPath and RsrDubinsPath
    public abstract class OuterDubinsPath : DubinsPath
    {
        public OuterDubinsPath(
            DubinsPathConstraints constraints,
            TurnType startTurnType, TurnType goalTurnType) : base(constraints, startTurnType, TurnType.Straight, goalTurnType)
        {
            Debug.Assert(startTurnType != TurnType.Straight);
            Debug.Assert(goalTurnType != TurnType.Straight);
        }

        static public bool PathIsPossible(GeoCircle startCircle, GeoCircle goalCircle)
        {
            bool isPossible =
                startCircle.Center.Easting != goalCircle.Center.Easting ||
                startCircle.Center.Northing != goalCircle.Center.Northing;
            return isPossible;
        }

        protected override void ComputeTangents(
            GeoCircle startCircle,
            TurnType startTurnType,
            GeoCircle goalCircle,
            out GeoCoord startTangent,
            out GeoCoord goalTangent)
        {
            Debug.Assert(TurnType.Straight != startTurnType);
            GeoDelta delta = (goalCircle.Center - startCircle.Center);
            GeoDir direction = new GeoDir(delta);
            GeoDir tangentDir = (TurnType.Right == startTurnType) ? direction.PerpendicularLeft : direction.PerpendicularRight;

            startTangent = startCircle.PointOnCircle(tangentDir);
            goalTangent = goalCircle.PointOnCircle(tangentDir);
        }
    }

    // Base class of RslDubinsPath and LsrDubinsPath
    public abstract class InnerDubinsPath : DubinsPath
    {
        public InnerDubinsPath(
            DubinsPathConstraints constraints,
            TurnType startTurnType,
            TurnType goalTurnType) : base(constraints, startTurnType, TurnType.Straight, goalTurnType)
        {
            Debug.Assert(startTurnType != TurnType.Straight);
            Debug.Assert(goalTurnType != TurnType.Straight);
        }

        static public bool PathIsPossible(GeoCircle startCircle, GeoCircle goalCircle)
        {
            //RSL and LSR is only working of the circles don't intersect
            return startCircle.Center.Distance(goalCircle.Center) > startCircle.Radius + goalCircle.Radius;
        }

        protected override void ComputeTangents(
            GeoCircle startCircle,
            TurnType startTurnType,
            GeoCircle goalCircle,
            out GeoCoord startTangent,
            out GeoCoord goalTangent)
        {
            Debug.Assert(TurnType.Straight != startTurnType);
            GeoDelta delta = goalCircle.Center - startCircle.Center;
            GeoDir direction = new GeoDir(delta);

            //If the circles have the same radius we can use cosine and not the law of cosines
            //to calculate the angle to the first tangent coordinate
            double theta = Math.Acos((2 * startCircle.Radius) / delta.Length);

            GeoDir startTangentDir = (TurnType.Right == startTurnType) ? direction - theta : direction + theta;
            startTangent = startCircle.PointOnCircle(startTangentDir);
            goalTangent = goalCircle.PointOnCircle(startTangentDir.Inverted);
        }
    }

    // Base class of RlrDubinsPath and LrlDubinsPath
    public abstract class CurvedDubinsPath : DubinsPath
    {
        public CurvedDubinsPath(
            DubinsPathConstraints constraints,
            TurnType startTurnType,
            TurnType middleTurnType,
            TurnType goalTurnType) : base(constraints, startTurnType, middleTurnType, goalTurnType)
        {
            Debug.Assert(middleTurnType != TurnType.Straight);
        }

        static public bool PathIsPossible(GeoCircle startCircle, GeoCircle goalCircle)
        {
            // With the LRL and RLR paths, the distance between the circles has to be less than 4 * r
            return
                startCircle.Center.Distance(goalCircle.Center) < 2.0 * (startCircle.Radius + goalCircle.Radius);
        }

        protected override void ComputeTangents(
            GeoCircle startCircle,
            TurnType startTurnType,
            GeoCircle goalCircle,
            out GeoCoord startTangent,
            out GeoCoord goalTangent)
        {
            Debug.Assert(TurnType.Straight != startTurnType);
            GeoDelta delta = goalCircle.Center - startCircle.Center;
            GeoDir direction = new GeoDir(delta);
            double D = delta.Length;

            //The angle between the goal and the new 3rd circle we create with the law of cosines
            double theta = Math.Acos(D / (4f * _constraints.RadiusConstraint));

            GeoDir startTangentDir = (TurnType.Left == startTurnType) ? direction + theta : direction - theta;
            _middleCircle = new GeoCircle(startCircle.Center + 2 * startCircle.Radius * startTangentDir, startCircle.Radius);

            startTangent = _middleCircle.PointOnCircle(new GeoDir(startCircle.Center - _middleCircle.Center));
            goalTangent = _middleCircle.PointOnCircle(new GeoDir(goalCircle.Center - _middleCircle.Center));
        }
    }

    public class LslDubinsPath : OuterDubinsPath
    {
        public LslDubinsPath(DubinsPathConstraints constraints) : base(constraints, TurnType.Left, TurnType.Left)
        {
        }
    }

    public class RsrDubinsPath : OuterDubinsPath
    {
        public RsrDubinsPath(DubinsPathConstraints constraints) : base(constraints, TurnType.Right, TurnType.Right)
        {
        }
    }

    public class RslDubinsPath : InnerDubinsPath
    {
        public RslDubinsPath(DubinsPathConstraints constraints) : base(constraints, TurnType.Right, TurnType.Left)
        {
        }
    }

    public class LsrDubinsPath : InnerDubinsPath
    {
        public LsrDubinsPath(DubinsPathConstraints constraints) : base(constraints, TurnType.Left, TurnType.Right)
        {
        }
    }

    public class RlrDubinsPath : CurvedDubinsPath
    {
        public RlrDubinsPath(DubinsPathConstraints constraints) : base(constraints, TurnType.Right, TurnType.Left, TurnType.Right)
        {
        }
    }

    public class LrlDubinsPath : CurvedDubinsPath
    {
        public LrlDubinsPath(DubinsPathConstraints constraints) : base(constraints, TurnType.Left, TurnType.Right, TurnType.Left)
        {
        }
    }
}
