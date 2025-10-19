# Phase 6: UI Integration Plan - Guidance Refactoring

**Date**: 2025-01-18
**Status**: Planning
**Goal**: Replace old CGuidance.cs with new GuidanceService from AgOpenGPS.Core

---

## 📋 Current Situation

### Old Code (to be replaced)
- **CGuidance.cs** (413 lines) - Old Stanley algorithm implementation
  - `StanleyGuidanceABLine()` - AB Line guidance
  - `StanleyGuidanceCurve()` - Curve guidance
  - `DoSteerAngleCalc()` - Final steer angle calculation
  - `sideHillCompFactor` - Roll compensation

### New Code (already built ✅)
- **AgOpenGPS.Core/Services/GuidanceService.cs** - Modern implementation
  - Stanley & Pure Pursuit algorithms
  - **Performance**: <1ms per call (tested!)
  - **Zero allocations** (verified!)
  - **Zero UI dependencies** - fully testable

---

## 🔍 Usage Analysis

### CGuidance Usage in Codebase

| File | Line | Method | Usage |
|------|------|--------|-------|
| **CABLine.cs** | 175 | `StanleyGuidanceABLine()` | Main AB Line guidance call |
| **CABCurve.cs** | 622 | `StanleyGuidanceCurve()` | Main Curve guidance call |
| **CContour.cs** | 540 | `gyd.sideHillCompFactor` | Roll compensation |
| **FormSteerWiz.cs** | 91, 1066 | `gyd.sideHillCompFactor` | Settings |
| **FormSteer.cs** | 200, 609 | `gyd.sideHillCompFactor` | Settings |

### Key Methods to Replace

#### 1. StanleyGuidanceABLine (CGuidance.cs:120-194)
```csharp
// OLD CODE:
public void StanleyGuidanceABLine(vec3 curPtA, vec3 curPtB, vec3 pivot, vec3 steer)
{
    // Calculate distance from line (pivot)
    distanceFromCurrentLinePivot = ...;

    // Calculate distance from line (steer)
    distanceFromCurrentLineSteer = ...;

    // Calculate heading error
    steerHeadingError = ...;

    // Call DoSteerAngleCalc()
    DoSteerAngleCalc();
}
```

**Replacement**: Use `GuidanceService.CalculateGuidance()` which internally handles both distance and steering calculations.

#### 2. StanleyGuidanceCurve (CGuidance.cs:202-409)
```csharp
// OLD CODE:
public void StanleyGuidanceCurve(vec3 pivot, vec3 steer, ref List<vec3> curList)
{
    // Two-phase search (coarse: every 10 points, fine: ±7)
    // Calculate distance from curve
    // Calculate heading error
    // Call DoSteerAngleCalc()
}
```

**Replacement**: Same - use `GuidanceService.CalculateGuidance()` with curve track.

#### 3. DoSteerAngleCalc (CGuidance.cs:42-109)
```csharp
// OLD CODE:
private void DoSteerAngleCalc()
{
    // Stanley formula
    // Integral term
    // Roll compensation (sideHillCompFactor)
    // Clamp to max steer angle
    // Update FormGPS properties
}
```

**Replacement**: GuidanceService already calculates steer angle. Need to:
1. Add roll compensation to GuidanceService
2. Map GuidanceResult to FormGPS properties

---

## 🎯 Integration Strategy

### Phase 6.1: Add GuidanceService to FormGPS ✅

**File**: `FormGPS.cs`

```csharp
public partial class FormGPS : Form
{
    // OLD (remove later)
    public CGuidance gyd;

    // NEW
    private readonly IGuidanceService _guidanceService;

    public FormGPS()
    {
        InitializeComponent();

        // OLD
        gyd = new CGuidance(this);

        // NEW
        _guidanceService = new GuidanceService();

        // Configure guidance service
        _guidanceService.Algorithm = GuidanceAlgorithm.Stanley; // or PurePursuit
        _guidanceService.StanleyGain = vehicle.stanleyDistanceErrorGain;
        _guidanceService.StanleyHeadingErrorGain = vehicle.stanleyHeadingErrorGain;
        _guidanceService.StanleyIntegralGain = vehicle.stanleyIntegralGainAB;
        _guidanceService.LookaheadDistance = vehicle.goalDistance;
        _guidanceService.MaxSteerAngle = vehicle.maxSteerAngle;
        _guidanceService.SideHillCompensation = gyd.sideHillCompFactor;
    }
}
```

### Phase 6.2: Replace CABLine.cs Usage

**File**: `CABLine.cs:175`

```csharp
// OLD CODE (line 175):
mf.gyd.StanleyGuidanceABLine(currentLinePtA, currentLinePtB, pivot, steer);

// NEW CODE:
// Build AB track points (should already exist in ABLine.curvePts)
if (curvePts == null || curvePts.Count < 2)
{
    // Build AB track if needed
    // This should happen when AB line is created, not per-frame!
}

// Call new guidance service
var result = mf._guidanceService.CalculateGuidance(
    pivotPosition: pivot,
    steerPosition: steer,
    heading: steer.heading,
    speed: mf.avgSpeed,
    guidanceTrack: curvePts,  // AB line curve points
    isReverse: mf.isReverse,
    isUturn: mf.youTurn.isYouTurnActive
);

// Map result to FormGPS properties
mf.guidanceLineDistanceOff = (short)(result.CrossTrackError * 1000.0);
mf.guidanceLineSteerAngle = (short)(result.SteerAngleRad * glm.toDegrees(1.0) * 100.0);
distanceFromCurrentLinePivot = result.CrossTrackError;

// Update display properties
rEastAB = result.ClosestPoint.easting;
rNorthAB = result.ClosestPoint.northing;
```

**Issue**: `curvePts` moet al bestaan in CABLine. Checken of die er is!

### Phase 6.3: Replace CABCurve.cs Usage

**File**: `CABCurve.cs:622`

```csharp
// OLD CODE (line 622):
mf.gyd.StanleyGuidanceCurve(pivot, steer, ref curList);

// NEW CODE:
var result = mf._guidanceService.CalculateGuidance(
    pivotPosition: pivot,
    steerPosition: steer,
    heading: steer.heading,
    speed: mf.avgSpeed,
    guidanceTrack: curList,  // Curve points
    isReverse: mf.isReverse,
    isUturn: mf.youTurn.isYouTurnActive
);

// Map result to FormGPS properties
mf.guidanceLineDistanceOff = (short)(result.CrossTrackError * 1000.0);
mf.guidanceLineSteerAngle = (short)(result.SteerAngleRad * glm.toDegrees(1.0) * 100.0);
distanceFromCurrentLinePivot = result.CrossTrackError;

// Update display properties
rEastCu = result.ClosestPoint.easting;
rNorthCu = result.ClosestPoint.northing;
currentLocationIndex = result.ClosestSegmentIndex;
manualUturnHeading = curList[result.ClosestSegmentIndex].heading;
```

### Phase 6.4: Migrate sideHillCompFactor

**Current Usage**: `gyd.sideHillCompFactor` (read/write from settings)

**Solution**: Add to GuidanceService interface

```csharp
// AgOpenGPS.Core/Interfaces/Services/IGuidanceService.cs
public interface IGuidanceService
{
    // ... existing properties
    double SideHillCompensation { get; set; }
}

// AgOpenGPS.Core/Services/GuidanceService.cs
public double SideHillCompensation { get; set; } = 0.0;

public GuidanceResult CalculateGuidance(...)
{
    // ... calculate steerAngle

    // Apply roll compensation (if IMU roll available)
    if (imuRoll != 88888)  // Magic value for "no IMU"
        steerAngle += imuRoll * -SideHillCompensation;

    // ... clamp and return
}
```

**Update usage sites**:
```csharp
// FormSteerWiz.cs, FormSteer.cs, etc.
// OLD:
mf.gyd.sideHillCompFactor = deg;

// NEW:
mf._guidanceService.SideHillCompensation = deg;
```

---

## 🔧 Implementation Steps

### Step 1: Extend GuidanceService Interface ✅

**File**: `AgOpenGPS.Core/Interfaces/Services/IGuidanceService.cs`

Add missing properties:
```csharp
public interface IGuidanceService
{
    // Add these properties:
    double SideHillCompensation { get; set; }
    double ImuRoll { get; set; }  // Or pass via CalculateGuidance parameter
}
```

### Step 2: Update GuidanceService Implementation ✅

**File**: `AgOpenGPS.Core/Services/GuidanceService.cs`

```csharp
public class GuidanceService : IGuidanceService
{
    public double SideHillCompensation { get; set; } = 0.0;

    public GuidanceResult CalculateGuidance(
        vec3 pivotPosition,
        vec3 steerPosition,
        double heading,
        double speed,
        List<vec3> guidanceTrack,
        bool isReverse,
        bool isUturn,
        double imuRoll = 88888)  // NEW parameter
    {
        // ... existing code

        // Apply roll compensation (AFTER Stanley/PurePursuit calculation)
        if (imuRoll != 88888)
            result.SteerAngleRad += imuRoll * -SideHillCompensation;

        // Clamp steer angle
        if (result.SteerAngleRad < -MaxSteerAngle)
            result.SteerAngleRad = -MaxSteerAngle;
        else if (result.SteerAngleRad > MaxSteerAngle)
            result.SteerAngleRad = MaxSteerAngle;

        return result;
    }
}
```

### Step 3: Add GuidanceService to FormGPS

**File**: `FormGPS.cs`

```csharp
public partial class FormGPS : Form
{
    // Keep old for now (parallel migration)
    public CGuidance gyd;

    // Add new service
    private GuidanceService _guidanceService;

    private void FormGPS_Load(object sender, EventArgs e)
    {
        // ... existing code

        // Initialize new guidance service
        _guidanceService = new GuidanceService();

        // Configure from vehicle settings
        UpdateGuidanceServiceSettings();
    }

    private void UpdateGuidanceServiceSettings()
    {
        _guidanceService.Algorithm = vehicle.isStanleyUsed
            ? GuidanceAlgorithm.Stanley
            : GuidanceAlgorithm.PurePursuit;

        _guidanceService.StanleyGain = vehicle.stanleyDistanceErrorGain;
        _guidanceService.StanleyHeadingErrorGain = vehicle.stanleyHeadingErrorGain;
        _guidanceService.StanleyIntegralGain = vehicle.stanleyIntegralGainAB;
        _guidanceService.LookaheadDistance = vehicle.goalDistance;
        _guidanceService.MaxSteerAngle = vehicle.maxSteerAngle;
        _guidanceService.SideHillCompensation = gyd.sideHillCompFactor;
    }
}
```

### Step 4: Create Adapter Method in FormGPS

**File**: `FormGPS.cs`

Add helper method to make transition easier:

```csharp
/// <summary>
/// Calculate guidance using new GuidanceService
/// </summary>
private GuidanceResult CalculateGuidanceNew(
    vec3 pivot,
    vec3 steer,
    List<vec3> guidanceTrack)
{
    // Update lookahead distance (changes dynamically)
    _guidanceService.LookaheadDistance = vehicle.UpdateGoalPointDistance();

    // Call new service
    var result = _guidanceService.CalculateGuidance(
        pivotPosition: pivot,
        steerPosition: steer,
        heading: steer.heading,
        speed: avgSpeed,
        guidanceTrack: guidanceTrack,
        isReverse: isReverse,
        isUturn: youTurn.isYouTurnActive,
        imuRoll: ahrs.imuRoll
    );

    return result;
}
```

### Step 5: Replace CABLine Call

**File**: `CABLine.cs` (around line 175)

```csharp
// BEFORE:
if (mf.isStanleyUsed)
{
    mf.gyd.StanleyGuidanceABLine(currentLinePtA, currentLinePtB, pivot, steer);
}

// AFTER:
if (mf.isStanleyUsed)
{
    // Use new guidance service
    var result = mf.CalculateGuidanceNew(pivot, steer, curvePts);

    // Map to existing properties
    distanceFromCurrentLinePivot = result.CrossTrackError;
    mf.guidanceLineDistanceOff = (short)(result.CrossTrackError * 1000.0);
    mf.guidanceLineSteerAngle = (short)(glm.toDegrees(result.SteerAngleRad) * 100.0);

    // Update closest point for display
    rEastAB = result.ClosestPoint.easting;
    rNorthAB = result.ClosestPoint.northing;
}
```

### Step 6: Replace CABCurve Call

**File**: `CABCurve.cs` (around line 622)

```csharp
// BEFORE:
if (mf.isStanleyUsed)
{
    mf.gyd.StanleyGuidanceCurve(pivot, steer, ref curList);
}

// AFTER:
if (mf.isStanleyUsed)
{
    // Use new guidance service
    var result = mf.CalculateGuidanceNew(pivot, steer, curList);

    // Map to existing properties
    distanceFromCurrentLinePivot = result.CrossTrackError;
    mf.guidanceLineDistanceOff = (short)(result.CrossTrackError * 1000.0);
    mf.guidanceLineSteerAngle = (short)(glm.toDegrees(result.SteerAngleRad) * 100.0);

    // Update closest point for display
    rEastCu = result.ClosestPoint.easting;
    rNorthCu = result.ClosestPoint.northing;
    currentLocationIndex = result.ClosestSegmentIndex;
    manualUturnHeading = curList[result.ClosestSegmentIndex].heading;
}
```

### Step 7: Update Settings Forms

**Files**: `FormSteerWiz.cs`, `FormSteer.cs`, `CContour.cs`

```csharp
// BEFORE:
mf.gyd.sideHillCompFactor = deg;

// AFTER:
mf._guidanceService.SideHillCompensation = deg;
// Also keep old for backward compatibility during migration:
mf.gyd.sideHillCompFactor = deg;
```

### Step 8: Testing

1. **Test AB Line Guidance**
   - Create AB line
   - Drive vehicle
   - Verify steering commands match old system
   - Check performance (<1ms per call)

2. **Test Curve Guidance**
   - Record curve
   - Drive vehicle
   - Verify steering commands
   - Check performance (<1ms per call)

3. **Test Roll Compensation**
   - Enable IMU roll
   - Set sideHillCompFactor
   - Verify correction applies

4. **Performance Verification**
   - Run profiler
   - Verify <1ms guidance calculation
   - Verify zero allocations

### Step 9: Cleanup (After Testing)

Once verified working:

1. **Remove CGuidance.cs**
2. **Remove `gyd` from FormGPS**
3. **Update all references to use `_guidanceService`**
4. **Remove compatibility code**

---

## ⚠️ Potential Issues

### Issue 1: GuidanceResult Missing Properties

**Problem**: Old code sets multiple properties:
- `distanceFromCurrentLinePivot`
- `distanceFromCurrentLineSteer`
- `rEastPivot`, `rNorthPivot`
- `rEastSteer`, `rNorthSteer`
- `steerHeadingError`

**Solution**: Ensure `GuidanceResult` struct contains all needed data:
```csharp
public struct GuidanceResult
{
    public double SteerAngleRad { get; set; }
    public double CrossTrackError { get; set; }        // Was: distanceFromCurrentLinePivot
    public double HeadingError { get; set; }           // Was: steerHeadingError
    public vec3 ClosestPoint { get; set; }             // Was: rEastPivot, rNorthPivot
    public int ClosestSegmentIndex { get; set; }       // Was: currentLocationIndex
    public bool IsValid { get; set; }
}
```

**Check**: Does current GuidanceResult have all these? Review and extend if needed.

### Issue 2: AB Line curvePts Not Built

**Problem**: CABLine may not have `curvePts` populated

**Solution**: Check CABLine initialization - ensure curvePts is built when AB line is set.

### Issue 3: Settings Synchronization

**Problem**: Settings (stanleyGain, etc.) may change at runtime

**Solution**: Call `UpdateGuidanceServiceSettings()` when settings change:
```csharp
// In settings forms:
private void btnSave_Click(object sender, EventArgs e)
{
    // Save settings
    Properties.Settings.Default.Save();

    // Update guidance service
    mf.UpdateGuidanceServiceSettings();
}
```

### Issue 4: Algorithm Switching

**Problem**: User can switch between Stanley and Pure Pursuit at runtime

**Solution**: Already handled - `_guidanceService.Algorithm` property allows runtime switching.

---

## 📊 Migration Checklist

### Phase 6.1: Preparation
- [ ] Extend IGuidanceService with SideHillCompensation
- [ ] Add imuRoll parameter to CalculateGuidance
- [ ] Verify GuidanceResult has all needed properties
- [ ] Add unit tests for roll compensation

### Phase 6.2: FormGPS Integration
- [ ] Add `_guidanceService` field to FormGPS
- [ ] Initialize in FormGPS_Load
- [ ] Add `UpdateGuidanceServiceSettings()` method
- [ ] Add `CalculateGuidanceNew()` adapter method
- [ ] Make `_guidanceService` accessible (public or internal method)

### Phase 6.3: CABLine Migration
- [ ] Verify CABLine.curvePts exists and is populated
- [ ] Replace `gyd.StanleyGuidanceABLine()` call
- [ ] Map GuidanceResult to CABLine properties
- [ ] Test AB line guidance

### Phase 6.4: CABCurve Migration
- [ ] Replace `gyd.StanleyGuidanceCurve()` call
- [ ] Map GuidanceResult to CABCurve properties
- [ ] Test curve guidance

### Phase 6.5: Settings Migration
- [ ] Update FormSteerWiz.cs references
- [ ] Update FormSteer.cs references
- [ ] Update CContour.cs references
- [ ] Add UpdateGuidanceServiceSettings() calls in settings forms

### Phase 6.6: Testing
- [ ] Test AB line guidance (straight line)
- [ ] Test curve guidance (recorded path)
- [ ] Test roll compensation (IMU)
- [ ] Test algorithm switching (Stanley ↔ Pure Pursuit)
- [ ] Performance test (<1ms verified)
- [ ] Allocation test (zero allocations verified)

### Phase 6.7: Cleanup
- [ ] Remove CGuidance.cs
- [ ] Remove `gyd` field from FormGPS
- [ ] Remove compatibility code
- [ ] Update documentation

---

## 🎯 Success Criteria

Phase 6 is complete when:

1. ✅ All guidance calculations use GuidanceService
2. ✅ CGuidance.cs deleted
3. ✅ All tests pass
4. ✅ UI fully functional
5. ✅ Performance <1ms verified in production
6. ✅ Zero allocations verified
7. ✅ No regressions in steering behavior

---

## 📝 Notes

- **Keep old code parallel** during migration (dual implementation)
- **Test extensively** before removing old code
- **Monitor performance** in real-world usage
- **Document any behavior differences**

---

*Next: Begin implementation with Step 1 - Extend GuidanceService Interface*
