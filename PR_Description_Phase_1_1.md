# [Phase 1.1] Move vec2/vec3 to Core + Geometry Extension Methods

## Summary

This PR completes **Phase 1.1** of the Guidance refactoring plan: moving core geometry types (`vec2`, `vec3`) to `AgOpenGPS.Core` and extracting geometry extension methods for UI decoupling and improved testability.

## Motivation

The current codebase has geometry logic tightly coupled with UI code (WinForms, OpenGL), making it:
- Hard to test in isolation
- Difficult to reuse in non-UI contexts (e.g., headless mode, different UI frameworks)
- Prone to mixing business logic with rendering code

This PR establishes the foundation for a clean architecture with **pure business logic in Core** and UI adapters in the GPS project.

## Changes

### 1. Core Types Migration ✅

**Moved to `AgOpenGPS.Core/Models/Base/`:**
- `Vec2.cs` - 2D coordinate struct with utility methods
- `Vec3.cs` - 3D coordinate struct with heading
- `vecFix2Fix` - GPS fix tracking struct

**Benefits:**
- ✅ Fully testable in Core without UI dependencies
- ✅ Can be used in other projects (e.g., AgIO, future Avalonia UI)
- ✅ Backward compatible via global usings

### 2. Geometry Helper Class ✅

**New: `AgOpenGPS.Core/Models/Base/GeoMath.cs`**
```csharp
public static class GeoMath
{
    public const double twoPI = 6.28318530717958647692;
    public const double PIBy2 = 1.57079632679489661923;

    public static double Distance(vec3 first, vec3 second)
    public static double DistanceSquared(vec3 first, vec3 second)
    public static vec3 Catmull(double t, vec3 p0, vec3 p1, vec3 p2, vec3 p3)
}
```

Pure geometry math - no UI dependencies.

### 3. Extension Methods from AOG_Dev ✅

**New: `AgOpenGPS.Core/Extensions/Vec3ListExtensions.cs`**

Migrated from AOG_Dev's improved implementation:

```csharp
public static class Vec3ListExtensions
{
    // Creates parallel offset lines for guidance tracks
    public static List<vec3> OffsetLine(
        this List<vec3> points,
        double distance,
        double minDist,
        bool loop)

    // Calculates headings based on neighboring points
    public static void CalculateHeadings(
        this List<vec3> points,
        bool loop)
}
```

**Used by:**
- `CTracks.BuildCurrentGuidanceTrack()` - Creates offset guidance lines
- `CTracks.NudgeRefCurve()` - Adjusts curve positions
- Track smoothing and Catmull-Rom spline interpolation

### 4. Backward Compatibility ✅

**New: `GPS/GlobalUsings.cs`**
```csharp
global using AgOpenGPS.Core.Models;
global using vec2 = AgOpenGPS.Core.Models.vec2;
global using vec3 = AgOpenGPS.Core.Models.vec3;
global using vecFix2Fix = AgOpenGPS.Core.Models.vecFix2Fix;
```

- All existing code continues to work without changes
- Removed old `GPS/Classes/vec3.cs` (now in Core)
- Added `<LangVersion>10.0</LangVersion>` to GPS project for global usings support

### 5. Unit Tests ✅

**New: `AgOpenGPS.Core.Tests/Extensions/Vec3ListExtensionsTests.cs`**

9 comprehensive tests covering:
- ✅ `CalculateHeadings()` - Straight lines (north, east)
- ✅ `CalculateHeadings()` - Closed loop paths
- ✅ `OffsetLine()` - East/West offsets
- ✅ `OffsetLine()` - Minimum distance filtering
- ✅ `OffsetLine()` - Empty list handling
- ✅ `GeoMath.Distance()` - Pythagorean distance
- ✅ `GeoMath.DistanceSquared()` - Squared distance

**Test Results:**
```
Test Run Successful.
Total tests: 9
     Passed: 9
 Total time: 3.14 seconds
```

## Breaking Changes

**None.** This is a pure refactoring with full backward compatibility.

## Build Status

✅ **AgOpenGPS.Core** - Builds successfully
✅ **AgOpenGPS (GPS)** - Builds successfully
✅ **All tests pass** - 9/9

## Files Changed

```
Added:
  AgOpenGPS.Core/Models/Base/Vec2.cs
  AgOpenGPS.Core/Models/Base/Vec3.cs
  AgOpenGPS.Core/Models/Base/GeoMath.cs
  AgOpenGPS.Core/Extensions/Vec3ListExtensions.cs
  AgOpenGPS.Core.Tests/Extensions/Vec3ListExtensionsTests.cs
  GPS/GlobalUsings.cs

Modified:
  GPS/AgOpenGPS.csproj (added LangVersion 10.0)

Deleted:
  GPS/Classes/vec3.cs (moved to Core)
```

## Testing Checklist

- [x] Unit tests pass (9/9)
- [x] AgOpenGPS.Core builds
- [x] AgOpenGPS (GPS) builds
- [x] No compiler warnings (except formatting suggestions)
- [x] Backward compatibility verified

## Review Notes

### Key Points to Review:

1. **`OffsetLine()` Algorithm** - Verify offset calculation logic is correct
   - Uses perpendicular heading offset: `Math.Cos(heading)` and `Math.Sin(heading)`
   - Filters points too close to original line
   - Respects minimum distance between points

2. **`CalculateHeadings()` Logic** - Verify heading calculations
   - First point: uses next point (or last→next for loops)
   - Middle points: average of previous→next
   - Last point: uses previous point (or prev→first for loops)

3. **Global Usings** - Confirm approach is acceptable
   - Alternative: Explicit `using AgOpenGPS.Core.Models;` in every file
   - Current approach: Cleaner, less code changes

4. **Test Coverage** - Suggestions for additional tests welcome
   - Current: Basic happy paths + edge cases
   - Future: More complex curve scenarios, large datasets

## Next Steps (Future PRs)

**Phase 1.2:** Migrate `FindClosestSegment()` and `FindDistanceToSegment()` from CGuidance
**Phase 2:** Extract unified Track models
**Phase 3:** Create ITrackService interface

## Documentation

All progress tracked in `Refactoring_Progress.md`

---

**Related Issues:** Part of Guidance system refactoring for UI decoupling
**Migration Source:** Based on improved AOG_Dev implementation (61% code reduction achieved there)
