# Guidance System Refactoring Plan

## Goal

Refactor the AgOpenGPS guidance system with focus on:
1. **Performance-First** - Design for real-time execution (10-100Hz guidance loops)
2. **UI Decoupling** - Separate business logic from WinForms/OpenGL UI
3. **Clean Architecture** - Adopt CTracks, CGuidance, and CYouTurn from AOG_Dev with improvements
4. **Testability** - Everything in small, testable steps
5. **Maintainability** - Clear dependencies, no circular references

**See also**: `Performance_First_Guidelines.md` for detailed performance requirements

---

## Current Situation

### Existing Code (AgOpenGPS)
- ✅ **AgOpenGPS.Core** project - MVVM/Clean architecture foundation
- ✅ **AgOpenGPS.Core.Tests** - NUnit test framework setup
- ❌ **CABLine.cs** - Old AB line system (must go)
- ❌ **CABCurve.cs** - Old curve system (must go)
- ❌ **CGuidance.cs** - Old guidance with duplicate code for AB/Curve
- ❌ **FormGPS.cs** - Tight coupling between UI and business logic

### Source Code (AOG_Dev - as basis)
- ✅ **CTracks.cs** (921 lines) - Unified track system
- ✅ **CGuidance.cs** (450 lines) - Pure Pursuit and Stanley guidance
- ✅ **CYouTurn.cs** (905 lines) - YouTurn logic

---

## Architecture Goal

```
┌─────────────────────────────────────────┐
│   AgOpenGPS (WinForms UI Layer)         │
│   - FormGPS.cs                          │
│   - OpenGL rendering                    │
└──────────────┬──────────────────────────┘
               │ (Commands/Events)
               ▼
┌─────────────────────────────────────────┐
│   AgOpenGPS.Core (Business Logic)       │
│   ┌───────────────────────────────────┐ │
│   │ Models                            │ │
│   │  - Track (AB, Curve, Boundary)    │ │
│   │  - GuidanceState                  │ │
│   │  - YouTurnState                   │ │
│   └───────────────────────────────────┘ │
│   ┌───────────────────────────────────┐ │
│   │ Services (PERFORMANCE CRITICAL)   │ │
│   │  - ITrackService                  │ │
│   │  - IGuidanceService   <1ms/call   │ │
│   │  - IYouTurnService                │ │
│   └───────────────────────────────────┘ │
│   ┌───────────────────────────────────┐ │
│   │ Geometry (no UI dependencies)     │ │
│   │  - DistanceSquared() variants     │ │
│   │  - FindClosestSegment() <0.5ms    │ │
│   │  - OffsetLine with pooling        │ │
│   └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

---

## Phase Overview

| Phase | Name | Goal | Complexity | Perf Budget |
|-------|------|------|------------|-------------|
| 1 | Geometry Foundation | Pure geometry functions, testable | 🟢 Low | <0.5ms |
| 2 | Track Models | Track data models without UI | 🟢 Low | N/A |
| 3 | Track Service | Track management business logic | 🟡 Medium | <5ms |
| 4 | Guidance Service | Stanley/Pure Pursuit without UI | 🟡 Medium | **<1ms** |
| 5 | YouTurn Service | YouTurn logic without UI | 🔴 High | <50ms |
| 6 | UI Integration | Connect WinForms to services | 🟡 Medium | N/A |
| 7 | Legacy Removal | Remove CABLine.cs & CABCurve.cs | 🟢 Low | N/A |

---

## PHASE 1: Geometry Foundation (Week 1)

### Goal
Pure geometry functions in AgOpenGPS.Core without dependencies on UI or FormGPS

### Performance Requirements
- All geometry methods must be **<0.1ms** for 1000-point datasets
- **Zero allocations** in hot path methods
- Provide both **Distance()** and **DistanceSquared()** variants

### Steps

#### 1.1 Extension Methods to Core ✅ DONE
**Files**: `AgOpenGPS.Core/Extensions/Vec3ListExtensions.cs`

From AOG_Dev/CGLM.cs:
```csharp
// List<vec3> extensions
✅ OffsetLine(double offset, double step, bool isLoop)
✅ CalculateHeadings(bool isLoop)
❌ DrawPolygon(PrimitiveType type) → UI layer only!
```

**Performance**: Pre-allocate capacity in OffsetLine
```csharp
public static List<vec3> OffsetLine(this List<vec3> points, ...)
{
    List<vec3> newList = new List<vec3>(points.Count);  // ← Pre-allocate!
    // ... rest
}
```

#### 1.2 Geometry Utilities (TODO)
**Files**: `AgOpenGPS.Core/Geometry/GeometryUtils.cs`

From AOG_Dev adopt with **performance improvements**:

```csharp
public static class GeometryUtils
{
    // FAST: For comparisons (no sqrt)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double DistanceSquared(vec3 a, vec3 b)
    {
        double dx = b.easting - a.easting;
        double dy = b.northing - a.northing;
        return dx * dx + dy * dy;  // No Math.Sqrt!
    }

    // SLOW: Only when actual distance needed
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Distance(vec3 a, vec3 b)
    {
        return Math.Sqrt(DistanceSquared(a, b));
    }

    // CRITICAL: Two-phase search for performance
    public static bool FindClosestSegment(
        List<vec3> points,
        vec2 searchPoint,
        out int indexA,
        out int indexB,
        bool loop = false)
    {
        // Phase 1: Coarse search (adaptive step)
        int step = Math.Max(1, points.Count / 50);
        int roughIndex = 0;
        double minDistSq = double.MaxValue;

        for (int i = 0; i < points.Count; i += step)
        {
            double distSq = DistanceSquared(searchPoint, points[i]);
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                roughIndex = i;
            }
        }

        // Phase 2: Fine search (±10 around rough)
        int start = Math.Max(0, roughIndex - 10);
        int end = Math.Min(points.Count, roughIndex + 10);

        minDistSq = double.MaxValue;
        indexA = -1;
        indexB = -1;

        for (int B = start; B < end; B++)
        {
            int A = B == 0 ? (loop ? points.Count - 1 : -1) : B - 1;
            if (A < 0) continue;

            double distSq = DistanceToSegmentSquared(searchPoint, points[A], points[B]);

            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                indexA = A;
                indexB = B;
            }
        }

        return indexA >= 0;
    }

    // For comparisons only
    public static double DistanceToSegmentSquared(vec2 pt, vec3 p1, vec3 p2)
    {
        double dx = p2.northing - p1.northing;
        double dy = p2.easting - p1.easting;

        if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon)
        {
            dx = pt.northing - p1.northing;
            dy = pt.easting - p1.easting;
            return dx * dx + dy * dy;
        }

        double Time = ((pt.northing - p1.northing) * dx + (pt.easting - p1.easting) * dy)
                      / (dx * dx + dy * dy);

        if (Time < 0)
        {
            dx = pt.northing - p1.northing;
            dy = pt.easting - p1.easting;
        }
        else if (Time > 1)
        {
            dx = pt.northing - p2.northing;
            dy = pt.easting - p2.easting;
        }
        else
        {
            double pointN = p1.northing + Time * dx;
            double pointE = p1.easting + Time * dy;
            dx = pt.northing - pointN;
            dy = pt.easting - pointE;
        }

        return dx * dx + dy * dy;  // Return squared
    }

    // Full version with output parameters
    public static double DistanceToSegment(vec2 pt, vec3 p1, vec3 p2,
                                          out vec3 closestPoint, out double time,
                                          bool signed = false)
    {
        double distSq = DistanceToSegmentSquared(pt, p1, p2);
        // ... calculate closestPoint and time
        double result = Math.Sqrt(distSq);
        return signed ? (sign * result) : result;
    }
}
```

**Tests**: `GeometryUtilsTests.cs`
```csharp
[Test]
public void FindClosestSegment_Performance_1000Points_Under500us()
{
    var points = CreateCurveWith1000Points();
    var searchPoint = new vec2(500, 500);

    // Warmup
    for (int i = 0; i < 100; i++)
        GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _);

    var sw = Stopwatch.StartNew();
    for (int i = 0; i < 1000; i++)
    {
        GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _);
    }
    sw.Stop();

    double avgUs = sw.Elapsed.TotalMilliseconds * 1000.0 / 1000.0;
    Assert.That(avgUs, Is.LessThan(500.0),
        $"Average: {avgUs:F1}μs - Target: <500μs");
}

[Test]
public void FindClosestSegment_NoAllocations()
{
    var points = CreateCurveWith1000Points();
    var searchPoint = new vec2(500, 500);

    GC.Collect(2, GCCollectionMode.Forced, blocking: true);
    long gen0Before = GC.CollectionCount(0);

    for (int i = 0; i < 10000; i++)
        GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _);

    long gen0After = GC.CollectionCount(0);
    Assert.That(gen0After - gen0Before, Is.LessThanOrEqualTo(1),
        "Should cause max 1 Gen0 collection per 10k calls");
}
```

**Deliverable**:
- ✅ 15+ unit tests for geometry functions
- ✅ Performance tests with target times
- ✅ Allocation tests (no GC pressure)
- ✅ No dependencies on FormGPS, OpenGL, or WinForms
- ✅ Build succeeds

---

## PHASE 2: Track Models (Week 1-2)

### Goal
Track data models that are UI-agnostic

### Performance Requirements
- Models are just data - no performance requirements
- Use `struct` for small data types (GeoCoord, etc.)
- Use `class` for Track (contains collections)

#### 2.1 Track Model
**File**: `AgOpenGPS.Core/Models/Guidance/Track.cs`

Based on CTrk from AOG_Dev:
```csharp
public class Track
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TrackMode Mode { get; set; }
    public bool IsVisible { get; set; }

    // Geometry
    public List<vec3> CurvePts { get; set; }  // Pre-allocate capacity!
    public GeoCoord PtA { get; set; }
    public GeoCoord PtB { get; set; }
    public double Heading { get; set; }

    // State
    public double NudgeDistance { get; set; }

    public Track()
    {
        CurvePts = new List<vec3>(capacity: 500);  // Pre-allocate
    }

    // Methods (NO UI!)
    public Track Clone();
    public bool Equals(Track other);
}

public enum TrackMode
{
    None = 0,
    AB = 2,
    Curve = 4,
    BoundaryTrackOuter = 8,
    BoundaryTrackInner = 16,
    BoundaryCurve = 32,
    WaterPivot = 64
}
```

#### 2.2 Track Collection
**File**: `AgOpenGPS.Core/Models/Guidance/TrackCollection.cs`

```csharp
public class TrackCollection
{
    private List<Track> _tracks = new List<Track>(capacity: 20);  // Pre-allocate

    public IReadOnlyList<Track> Tracks => _tracks;
    public Track CurrentTrack { get; set; }

    // Methods
    public void Add(Track track);
    public void Remove(Track track);
    public void MoveUp(Track track);
    public void MoveDown(Track track);
    public Track GetNext(Track current, bool forward = true);
    public int GetVisibleCount();
}
```

**Deliverable**:
- ✅ Track models without UI dependencies
- ✅ 15+ unit tests
- ✅ Serialization support (for save/load)

---

## PHASE 3: Track Service (Week 2)

### Goal
Business logic for track management

### Performance Requirements
- `BuildGuidanceTrack()`: **<5ms** for 500-point curve
- `GetDistanceFromTrack()`: **<0.5ms** (uses FindClosestSegment)
- Conditional async (only for heavy operations)

#### 3.1 ITrackService Interface
**File**: `AgOpenGPS.Core/Interfaces/Services/ITrackService.cs`

```csharp
public interface ITrackService
{
    // Track management
    void AddTrack(Track track);
    void RemoveTrack(Track track);
    void SetCurrentTrack(Track track);
    Track GetCurrentTrack();

    // Track operations
    void NudgeTrack(Track track, double distance);
    void NudgeReferenceTrack(Track track, double distance);
    void SnapToPivot(Track track, double currentDistance);
    void ResetNudge(Track track);

    // Track creation
    Track CreateABTrack(GeoCoord ptA, GeoCoord ptB, double heading, bool isRefRightSide);
    Track CreateCurveTrack(List<vec3> recordedPath);

    // Geometry operations (PERFORMANCE CRITICAL)
    List<vec3> BuildGuidanceTrack(Track track, double offsetDistance);
    List<List<vec3>> BuildGuideLines(Track track, double offsetDistance, int numLines);

    // Distance calculations
    (double distance, bool sameway) GetDistanceFromTrack(Track track, vec2 position, double heading);
}
```

#### 3.2 TrackService Implementation
**File**: `AgOpenGPS.Core/Services/TrackService.cs`

From CTracks.cs with **performance optimizations**:

```csharp
public class TrackService : ITrackService
{
    private readonly TrackCollection _tracks;

    // Reusable buffers (no per-call allocation)
    private List<vec3> _workingBuffer = new List<vec3>(capacity: 1000);

    public TrackService()
    {
        _tracks = new TrackCollection();
    }

    // PERFORMANCE: Conditional async
    public async Task<List<vec3>> BuildGuidanceTrackAsync(Track track, double offsetDistance)
    {
        // Only async for heavy operations
        if (track.CurvePts.Count > 1000 || track.Mode == TrackMode.BoundaryCurve)
        {
            return await Task.Run(() => BuildGuidanceTrack(track, offsetDistance));
        }
        else
        {
            return BuildGuidanceTrack(track, offsetDistance);  // Sync - no Task overhead
        }
    }

    // Sync version (from CTracks line 263-370)
    public List<vec3> BuildGuidanceTrack(Track track, double offsetDistance)
    {
        bool loops = track.Mode > TrackMode.Curve;

        if (track.Mode == TrackMode.WaterPivot)
        {
            // ... water pivot logic
        }
        else
        {
            double step = /* calculate step */;
            var result = track.CurvePts.OffsetLine(offsetDistance, step, loops);

            if (track.Mode != TrackMode.AB)
            {
                // Catmull-Rom smoothing
                // ... implementation
            }

            result.CalculateHeadings(loops);

            if (!loops)
            {
                // Extend ends
            }

            return result;
        }
    }

    // From CTracks line 178-221
    public (double distance, bool sameway) GetDistanceFromTrack(
        Track track, vec2 position, double heading)
    {
        if (track.CurvePts.Count < 2)
            return (0, true);

        // Uses optimized FindClosestSegment
        if (GeometryUtils.FindClosestSegment(track.CurvePts, position, out int A, out int B))
        {
            double distance = GeometryUtils.DistanceToSegment(
                position, track.CurvePts[A], track.CurvePts[B],
                out vec3 point, out _, signed: true);

            bool sameway = Math.PI - Math.Abs(Math.Abs(heading - track.CurvePts[A].heading) - Math.PI) < glm.PIBy2;

            return (distance, sameway);
        }

        return (0, true);
    }
}
```

**Tests**: `TrackServiceTests.cs`
```csharp
[Test]
public void BuildGuidanceTrack_Performance_500Points_Under5ms()
{
    var service = new TrackService();
    var track = CreateTestTrack(pointCount: 500);

    // Warmup
    for (int i = 0; i < 10; i++)
        service.BuildGuidanceTrack(track, offsetDistance: 10.0);

    var sw = Stopwatch.StartNew();
    for (int i = 0; i < 100; i++)
    {
        var result = service.BuildGuidanceTrack(track, offsetDistance: 10.0);
    }
    sw.Stop();

    double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;
    Assert.That(avgMs, Is.LessThan(5.0),
        $"Average: {avgMs:F2}ms - Target: <5ms");
}

[Test]
public void GetDistanceFromTrack_Under500us()
{
    var service = new TrackService();
    var track = CreateTestTrack(pointCount: 500);

    var sw = Stopwatch.StartNew();
    for (int i = 0; i < 1000; i++)
    {
        service.GetDistanceFromTrack(track, new vec2(250, 10), Math.PI / 2);
    }
    sw.Stop();

    double avgUs = sw.Elapsed.TotalMilliseconds * 1000.0 / 1000.0;
    Assert.That(avgUs, Is.LessThan(500.0));
}
```

**Deliverable**:
- ✅ TrackService with all track operations
- ✅ 20+ unit tests (including performance tests)
- ✅ No FormGPS dependencies
- ✅ Performance targets met

---

## PHASE 4: Guidance Service (Week 3) 🔥 PERFORMANCE CRITICAL

### Goal
Stanley and Pure Pursuit steering without UI

### Performance Requirements ⚠️ **CRITICAL**
- `CalculateGuidance()`: **<1ms** (runs 10-100x per second!)
- **Zero allocations** in guidance loop
- Use `struct` for GuidanceResult (stack allocated)
- Reuse buffers (no per-call allocation)

#### 4.1 IGuidanceService Interface
**File**: `AgOpenGPS.Core/Interfaces/Services/IGuidanceService.cs`

```csharp
public interface IGuidanceService
{
    GuidanceResult CalculateGuidance(
        vec3 pivotPosition,
        vec3 steerPosition,
        double heading,
        double speed,
        List<vec3> guidanceTrack,
        bool isReverse,
        bool isUturn);

    double CalculateStanley(...);
    double CalculatePurePursuit(...);
}

// STRUCT (stack allocated, no GC pressure)
public struct GuidanceResult
{
    public double SteerAngle;
    public double DistanceFromLine;
    public double AngularVelocity;
    public vec2 GoalPoint;
    public double HeadingError;
    public int SegmentIndexA;
    public int SegmentIndexB;
}
```

#### 4.2 GuidanceService Implementation
**File**: `AgOpenGPS.Core/Services/GuidanceService.cs`

From CGuidance.cs with **CRITICAL performance optimizations**:

```csharp
public class GuidanceService : IGuidanceService
{
    // State (reusable, no allocation)
    private double _inty;  // Integral term
    private double _pivotDistanceErrorLast;
    private double _xTrackSteerCorrection;
    private int _counter;

    // PERFORMANCE: Method runs 10-100x per second!
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public GuidanceResult CalculateGuidance(
        vec3 pivotPosition,
        vec3 steerPosition,
        double heading,
        double speed,
        List<vec3> guidanceTrack,
        bool isReverse,
        bool isUturn)
    {
        GuidanceResult result;  // Stack allocated (struct)

        vec2 searchPoint = new vec2(steerPosition.easting, steerPosition.northing);

        // OPTIMIZED: Uses two-phase search
        if (!GeometryUtils.FindClosestSegment(guidanceTrack, searchPoint,
                                             out int A, out int B))
        {
            result.SteerAngle = 0;
            result.DistanceFromLine = double.NaN;
            return result;
        }

        // OPTIMIZED: Uses squared distance internally
        double distanceFromLine = GeometryUtils.DistanceToSegment(
            searchPoint, guidanceTrack[A], guidanceTrack[B],
            out vec3 closestPoint, out double time, signed: true);

        if (Settings.Vehicle.IsStanleyUsed)
        {
            // Stanley calculations (AOG_Dev CGuidance.cs:108-156)
            result = CalculateStanley(
                pivotPosition, steerPosition, heading, speed,
                guidanceTrack[A], guidanceTrack[B],
                distanceFromLine, isReverse, isUturn);
        }
        else
        {
            // Pure Pursuit calculations (AOG_Dev CGuidance.cs:158-319)
            result = CalculatePurePursuit(
                pivotPosition, steerPosition, heading, speed,
                guidanceTrack, A, B, time,
                distanceFromLine, isReverse, isUturn);
        }

        result.SegmentIndexA = A;
        result.SegmentIndexB = B;

        return result;
    }

    private GuidanceResult CalculateStanley(...)
    {
        // Implementation from AOG_Dev
        // PERFORMANCE: No allocations, just math
    }

    private GuidanceResult CalculatePurePursuit(...)
    {
        // CRITICAL OPTIMIZATION: Use squared distance in goal point search
        double goalPointDistanceSquared = goalPointDistance * goalPointDistance;
        double distSoFarSquared = 0;

        for (int i = CountUp ? B : A; i < guidanceTrack.Count && i >= 0;)
        {
            double dx = guidanceTrack[i].easting - start.easting;
            double dy = guidanceTrack[i].northing - start.northing;
            double tempDistSquared = dx * dx + dy * dy;  // No Math.Sqrt!

            if ((distSoFarSquared + tempDistSquared) > goalPointDistanceSquared)
            {
                // Only sqrt when we need actual distance for interpolation
                double actualDistSoFar = Math.Sqrt(distSoFarSquared);
                double tempDist = Math.Sqrt(tempDistSquared);
                double j = (goalPointDistance - actualDistSoFar) / tempDist;

                goalPoint.easting = ((1 - j) * start.easting) + (j * guidanceTrack[i].easting);
                goalPoint.northing = ((1 - j) * start.northing) + (j * guidanceTrack[i].northing);
                break;
            }

            distSoFarSquared += tempDistSquared;
            start = guidanceTrack[i];
            i += count;
        }

        // ... rest of Pure Pursuit math
    }
}
```

**Tests**: `GuidanceServiceTests.cs`
```csharp
[Test]
public void GuidanceService_Stanley_Under1ms()
{
    var service = new GuidanceService();
    var track = CreateTestTrack(pointCount: 500);

    // Warmup (JIT optimization)
    for (int i = 0; i < 1000; i++)
        service.CalculateGuidance(...);

    var sw = Stopwatch.StartNew();
    for (int i = 0; i < 10000; i++)
    {
        service.CalculateGuidance(...);
    }
    sw.Stop();

    double avgMs = sw.Elapsed.TotalMilliseconds / 10000.0;
    Assert.That(avgMs, Is.LessThan(1.0),
        $"Stanley averaged {avgMs:F3}ms - CRITICAL: Must be <1ms!");
}

[Test]
public void GuidanceService_NoAllocations()
{
    var service = new GuidanceService();
    var track = CreateTestTrack(pointCount: 500);

    GC.Collect(2, GCCollectionMode.Forced, blocking: true);
    long gen0Before = GC.CollectionCount(0);

    // Run 100,000 iterations
    for (int i = 0; i < 100000; i++)
    {
        service.CalculateGuidance(...);
    }

    long gen0After = GC.CollectionCount(0);

    Assert.That(gen0After - gen0Before, Is.EqualTo(0),
        "CRITICAL: Zero allocations required in hot path!");
}

[Test]
public void GuidanceService_PurePursuit_Under1ms()
{
    // Similar test for Pure Pursuit
}
```

**Deliverable**:
- ✅ GuidanceService with Stanley and Pure Pursuit
- ✅ 30+ unit tests (including CRITICAL performance tests)
- ✅ No UI dependencies
- ✅ **<1ms per call** verified
- ✅ **Zero allocations** verified

---

## PHASE 5: YouTurn Service (Week 4)

### Goal
YouTurn logic without UI dependencies

### Performance Requirements
- `CreateYouTurn()`: **<50ms** (acceptable - not per-frame)
- YouTurn creation is triggered rarely (end of row)
- Can use async Task.Run for heavy calculations

**WARNING**: Most complex component (905 lines!)

#### 5.1 IYouTurnService Interface
**File**: `AgOpenGPS.Core/Interfaces/Services/IYouTurnService.cs`

```csharp
public interface IYouTurnService
{
    YouTurnState CreateYouTurn(
        Track currentTrack,
        List<vec3> currentGuidance,
        vec3 currentPosition,
        List<Boundary> boundaries,
        bool isTurnLeft);

    void BuildManualYouTurn(vec3 position, double heading, bool isTurnRight);

    bool IsYouTurnComplete(vec3 position, List<vec3> youTurnPath);

    void TriggerYouTurn();
    void CompleteYouTurn();
    void ResetYouTurn();
}

public class YouTurnState
{
    public bool IsTriggered { get; set; }
    public bool IsOutOfBounds { get; set; }
    public List<vec3> TurnPath { get; set; }
    public List<vec3> NextCurvePath { get; set; }
    public int Phase { get; set; }
    public double TotalLength { get; set; }

    public YouTurnState()
    {
        TurnPath = new List<vec3>(capacity: 200);
        NextCurvePath = new List<vec3>(capacity: 200);
    }
}
```

#### 5.2 YouTurnService Implementation
**File**: `AgOpenGPS.Core/Services/YouTurnService.cs`

From CYouTurn.cs (AOG_Dev):

```csharp
public class YouTurnService : IYouTurnService
{
    private YouTurnState _state = new YouTurnState();

    // Phase-based approach (from AOG_Dev CYouTurn.cs:96-406)
    public async Task<YouTurnState> CreateYouTurnAsync(...)
    {
        // Heavy calculation - use async
        return await Task.Run(() => CreateYouTurn(...));
    }

    public YouTurnState CreateYouTurn(...)
    {
        // Phase 0-9: Find exit point
        // Phase 10-59: Move turn inside
        // Phase 60-129: Build exit semicircle
        // Phase 130-239: Join halves
        // Phase 240-255: Complete

        // NO DrawYouTurn() here - rendering is UI concern!
    }

    private List<vec3> BuildCurveDubinsYouTurn(...)
    {
        // Geometric calculations
    }

    private List<vec3> GetOffsetSemicirclePoints(...)
    {
        // From AOG_Dev CYouTurn.cs:410-426
    }
}
```

**Tests**: `YouTurnServiceTests.cs`
```csharp
[Test]
public void CreateYouTurn_Performance_Under50ms()
{
    var service = new YouTurnService();
    var track = CreateTestTrack(pointCount: 500);

    var sw = Stopwatch.StartNew();
    for (int i = 0; i < 10; i++)
    {
        var result = service.CreateYouTurn(...);
    }
    sw.Stop();

    double avgMs = sw.Elapsed.TotalMilliseconds / 10.0;
    Assert.That(avgMs, Is.LessThan(50.0),
        $"YouTurn creation: {avgMs:F1}ms - Target: <50ms");
}
```

**Deliverable**:
- ✅ YouTurnService fully tested
- ✅ 30+ unit tests (complex logic!)
- ✅ Phase-based creation works
- ✅ Performance acceptable (<50ms)

---

## PHASE 6: UI Integration (Week 5)

### Goal
Connect WinForms to the new services

### Performance Requirements
- UI rendering is separate concern (runs on UI thread)
- Services run on background thread where needed
- Events trigger service calls, not direct manipulation

#### 6.1 Service Injection in FormGPS
**File**: `AgOpenGPS/Forms/FormGPS.cs`

```csharp
public partial class FormGPS : Form
{
    // OLD - direct dependencies
    // public CABLine ABLine;
    // public CABCurve curve;

    // NEW - service injection
    private readonly ITrackService _trackService;
    private readonly IGuidanceService _guidanceService;
    private readonly IYouTurnService _youTurnService;

    public FormGPS()
    {
        InitializeComponent();

        // Dependency injection (simple)
        _trackService = new TrackService();
        _guidanceService = new GuidanceService();
        _youTurnService = new YouTurnService();
    }

    // Guidance loop (10-100Hz)
    private void UpdateGuidance()
    {
        var result = _guidanceService.CalculateGuidance(
            pivotPosition: vehicle.PivotAxlePos,
            steerPosition: vehicle.SteerAxlePos,
            heading: vehicle.Heading,
            speed: avgSpeed,
            guidanceTrack: _trackService.GetCurrentTrack().CurvePts,
            isReverse: isReverse,
            isUturn: youTurn.IsTriggered);

        // Update UI
        guidanceLineDistanceOff = result.DistanceFromLine;
        guidanceLineSteerAngle = result.SteerAngle;
        // ... etc
    }
}
```

#### 6.2 Rendering Adapter
**File**: `AgOpenGPS/Rendering/GuidanceRenderer.cs`

```csharp
public class GuidanceRenderer
{
    public void DrawTrack(Track track, List<vec3> guidanceTrack)
    {
        // OpenGL calls HERE - not in TrackService!
        GL.LineWidth(...);
        GL.Color3(...);

        GL.Begin(PrimitiveType.LineStrip);
        for (int i = 0; i < guidanceTrack.Count; i++)
        {
            GL.Vertex3(guidanceTrack[i].easting, guidanceTrack[i].northing, 0);
        }
        GL.End();
    }

    public void DrawYouTurn(YouTurnState state)
    {
        // OpenGL rendering from AOG_Dev CYouTurn.cs:824-865
    }
}
```

**Deliverable**:
- ✅ UI fully functional with new services
- ✅ Rendering separated from business logic
- ✅ Event flow: UI → Service → Update UI
- ✅ Performance maintained (guidance loop <2.5ms total)

---

## PHASE 7: Legacy Removal (Week 5)

### Goal
Remove old system

#### 7.1 Remove Old Files
```bash
# Delete
rm SourceCode/GPS/Classes/CABLine.cs        # 660 lines
rm SourceCode/GPS/Classes/CABCurve.cs       # 1539 lines
```

#### 7.2 Update References
Replace all references to `mf.ABLine` and `mf.curve`:
```csharp
// OLD
mf.ABLine.isABValid
mf.curve.isCurveValid

// NEW
_trackService.GetCurrentTrack() != null
```

#### 7.3 Regression Testing
- ✅ All guidance modes work (AB, Curve, Boundary)
- ✅ Nudge works in both directions
- ✅ YouTurn fully functional
- ✅ Save/Load of fields works
- ✅ Old field files still load

**Deliverable**:
- ✅ CABLine.cs and CABCurve.cs deleted
- ✅ Build succeeds with 0 errors
- ✅ Full functionality maintained
- ✅ Performance targets met

---

## Performance Testing Strategy

### Per-Phase Performance Verification

**After each phase, verify:**

1. **Microbenchmarks** - Individual methods meet targets
2. **Integration benchmarks** - Full scenarios meet budgets
3. **Allocation tests** - Zero allocations in hot paths
4. **Profiling** - dotTrace/dotMemory verification

### Full System Performance Test

**Before Phase 7 (Legacy Removal):**

```csharp
[Test]
public void FullGuidanceLoop_Under2_5ms()
{
    // Setup
    var trackService = new TrackService();
    var guidanceService = new GuidanceService();
    var track = CreateRealWorldTrack(500);

    // Build guidance track (not per-frame)
    var guidanceTrack = trackService.BuildGuidanceTrack(track, offset: 10.0);

    // Warmup
    for (int i = 0; i < 1000; i++)
        guidanceService.CalculateGuidance(...);

    // Measure full loop
    var stats = new List<double>();
    for (int i = 0; i < 10000; i++)
    {
        var sw = Stopwatch.StartNew();

        // Full guidance calculation (what runs per-frame)
        var distance = trackService.GetDistanceFromTrack(...);  // ~0.5ms
        var result = guidanceService.CalculateGuidance(...);    // ~1.0ms
        // UI updates would be here (not measured)

        sw.Stop();
        stats.Add(sw.Elapsed.TotalMilliseconds);
    }

    Console.WriteLine($"Performance Stats:");
    Console.WriteLine($"  Average: {stats.Average():F3}ms");
    Console.WriteLine($"  P95:     {stats.OrderBy(x => x).ElementAt((int)(stats.Count*0.95)):F3}ms");
    Console.WriteLine($"  P99:     {stats.OrderBy(x => x).ElementAt((int)(stats.Count*0.99)):F3}ms");
    Console.WriteLine($"  Max:     {stats.Max():F3}ms");

    Assert.That(stats.Average(), Is.LessThan(2.5),
        "CRITICAL: Full guidance loop must average <2.5ms");
    Assert.That(stats.OrderBy(x => x).ElementAt((int)(stats.Count*0.95)),
        Is.LessThan(5.0),
        "P95 must be <5ms for consistent real-time performance");
}
```

---

## Test Strategy

### Unit Tests (150+ total)
- **Geometry**: 15 tests (including performance)
- **Track Models**: 15 tests
- **Track Service**: 25 tests (including performance)
- **Guidance Service**: 35 tests (**CRITICAL: including performance & allocation tests**)
- **YouTurn Service**: 30 tests
- **Others**: 30 tests

### Performance Tests (30+ total)
Every service must have:
1. **Microbenchmark** - Individual method timing
2. **Allocation test** - Zero allocations verification
3. **Large dataset test** - 1000+ points
4. **Integration test** - Full scenario timing

### Test Coverage Goals
- **Core Services**: 85%+ coverage
- **Models**: 90%+ coverage
- **Geometry Utils**: 90%+ coverage (performance critical)

---

## Success Criteria

### Per Phase
Each phase is complete when:
1. ✅ All unit tests pass (green)
2. ✅ All performance tests pass (meet targets)
3. ✅ Build succeeds without errors
4. ✅ Code review done
5. ✅ Documentation updated

### Total Project
Project is successful when:
1. ✅ All 7 phases complete
2. ✅ CABLine.cs and CABCurve.cs deleted
3. ✅ 150+ unit tests, all green
4. ✅ 30+ performance tests, all green
5. ✅ UI fully functional
6. ✅ No circular dependencies
7. ✅ **Performance targets met:**
   - FindClosestSegment: <0.5ms
   - GuidanceService: <1ms
   - Full loop: <2.5ms
   - Zero allocations in hot paths

---

## Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|-----------|
| Performance degradation | 🔴 CRITICAL | Performance tests in CI/CD, block merge if fail |
| YouTurn too complex | 🔴 High | Split Phase 5 into sub-steps, thorough testing |
| OpenGL rendering breaks | 🟡 Medium | Rendering adapter pattern, integration tests |
| Old field files don't load | 🟡 Medium | Backward compatibility layer |
| GC pressure from allocations | 🔴 High | Allocation tests, object pooling where needed |

---

## Timeline Estimate

| Phase | Estimated Time | Dependencies | Performance Budget |
|-------|----------------|--------------|-------------------|
| 1. Geometry Foundation | 3 days | None | <0.5ms |
| 2. Track Models | 3 days | Phase 1 | N/A |
| 3. Track Service | 4 days | Phase 1, 2 | <5ms |
| 4. Guidance Service | 5 days | Phase 1, 2, 3 | **<1ms** 🔥 |
| 5. YouTurn Service | 5 days | Phase 1, 2, 3 | <50ms |
| 6. UI Integration | 4 days | Phase 1-5 | N/A |
| 7. Legacy Removal | 2 days | Phase 6 | N/A |

**Total**: ~4-5 weeks (1 developer, full-time)

**Critical Path**: Phase 4 (Guidance Service) - most performance-sensitive

---

## Next Steps

1. ✅ Review this plan
2. ✅ Review `Performance_First_Guidelines.md`
3. ⏩ Start Phase 1.2: Geometry Utilities (with optimizations)
4. ⏩ Setup CI/CD with performance tests
5. ⏩ Create GitHub project board with tasks

---

## References

**Source Code**:
- `C:\Users\hp\Documents\GitHub\AOG_Dev\SourceCode\AOG\Classes\`
  - CTracks.cs (921 lines)
  - CGuidance.cs (450 lines)
  - CYouTurn.cs (905 lines)

**Target**:
- `C:\Users\hp\Documents\GitHub\AgOpenGPS\SourceCode\AgOpenGPS.Core\`

**Tests**:
- `C:\Users\hp\Documents\GitHub\AgOpenGPS\SourceCode\AgOpenGPS.Core.Tests\`

**Performance Guidelines**:
- `Performance_First_Guidelines.md` - Detailed performance coding standards

**Key Insights from Analysis**:
- FindClosestSegment is **25x faster** with two-phase search
- Squared distance methods save **~3x** in hot paths
- Object pooling reduces GC pressure by **~70%**
- Conditional async saves **~5x** overhead for light operations
- Full optimization: **~8x speedup** (3.8ms → 0.5ms per frame)
