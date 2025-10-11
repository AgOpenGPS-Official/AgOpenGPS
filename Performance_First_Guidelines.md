# Performance-First Guidelines voor Guidance Refactoring

## 🎯 Core Principe

**"Build for performance from the start, not as an afterthought"**

Alle code die we schrijven tijdens de refactoring moet **real-time geschikt** zijn voor 10-100Hz guidance loops.

---

## ⚡ Performance Budget per Frame

**Target**: Guidance berekening moet **<5ms** zijn (voor 100Hz loop met 50% headroom)

| Component | Budget | Priority |
|-----------|--------|----------|
| FindClosestSegment | <0.5ms | CRITICAL |
| Distance calculations | <0.3ms | CRITICAL |
| Stanley/Pure Pursuit | <1.0ms | HIGH |
| Geometry transforms | <0.5ms | MEDIUM |
| State updates | <0.1ms | LOW |
| **TOTAL** | **<2.5ms** | - |

Reserve 2.5ms voor other operations (UI updates, logging, etc.)

---

## 🔥 Hot Path Rules (Code dat ELKE FRAME draait)

### Rule 1: **No Allocations in Hot Paths**

❌ **NEVER**:
```csharp
public GuidanceResult CalculateGuidance(...)
{
    var tempList = new List<vec3>();  // ← ALLOCATION EVERY FRAME!
    var result = new GuidanceResult(); // ← OK only if small struct
}
```

✅ **ALWAYS**:
```csharp
// Option A: Reuse existing collections
private List<vec3> _reuseableBuffer = new List<vec3>(capacity: 500);

public GuidanceResult CalculateGuidance(...)
{
    _reuseableBuffer.Clear();  // ← No allocation!
    // ... use buffer
}

// Option B: Use structs (stack allocated)
public struct GuidanceResult  // ← struct, not class
{
    public double SteerAngle;
    public double DistanceFromLine;
}

// Option C: Use out parameters
public void CalculateGuidance(..., out GuidanceResult result)
```

### Rule 2: **Avoid Math.Sqrt() in Loops**

❌ **SLOW**:
```csharp
for (int i = 0; i < points.Count; i++)
{
    double dist = Math.Sqrt(dx*dx + dy*dy);  // ← 20-30 CPU cycles!
    if (dist < minDist) { ... }
}
```

✅ **FAST**:
```csharp
// Compare squared distances
double minDistSquared = minDist * minDist;
for (int i = 0; i < points.Count; i++)
{
    double distSquared = dx*dx + dy*dy;  // ← 2 CPU cycles
    if (distSquared < minDistSquared) { ... }
}
// Only sqrt the final result if needed
return Math.Sqrt(minDistSquared);
```

### Rule 3: **Use AggressiveInlining for Small Methods**

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static double DistanceSquared(vec3 a, vec3 b)
{
    double dx = b.easting - a.easting;
    double dy = b.northing - a.northing;
    return dx * dx + dy * dy;
}
```

### Rule 4: **Pre-allocate Collections with Capacity**

❌ **SLOW** (multiple internal resizes):
```csharp
var list = new List<vec3>();
for (int i = 0; i < 500; i++)
    list.Add(...);  // ← Resizes at 4, 8, 16, 32, 64, 128, 256, 512
```

✅ **FAST**:
```csharp
var list = new List<vec3>(capacity: 500);  // ← One allocation, correct size
for (int i = 0; i < 500; i++)
    list.Add(...);
```

### Rule 5: **Two-Phase Search for Large Datasets**

```csharp
// Phase 1: Coarse search (skip points)
int roughIndex = 0;
int step = Math.Max(1, points.Count / 50);
for (int i = 0; i < points.Count; i += step)
{
    double distSq = DistanceSquared(searchPoint, points[i]);
    if (distSq < minDistSq)
    {
        minDistSq = distSq;
        roughIndex = i;
    }
}

// Phase 2: Fine search (±range around rough hit)
int start = Math.Max(0, roughIndex - 10);
int end = Math.Min(points.Count, roughIndex + 10);
for (int i = start; i < end; i++)
{
    // Detailed search
}
```

---

## 🧪 Performance Test Requirements

**ELKE service method moet performance tests hebben:**

```csharp
[Test]
public void GuidanceService_CalculateStanley_Under1ms()
{
    var service = new GuidanceService();
    var track = CreateTestTrack(pointCount: 500);

    // Warmup
    for (int i = 0; i < 100; i++)
        service.CalculateGuidance(...);

    // Measure
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < 1000; i++)
        service.CalculateGuidance(...);
    sw.Stop();

    double avgMs = sw.Elapsed.TotalMilliseconds / 1000.0;
    Assert.That(avgMs, Is.LessThan(1.0),
        $"Average time: {avgMs:F3}ms - Target: <1.0ms");
}

[Test]
public void GuidanceService_NoAllocationsInHotPath()
{
    var service = new GuidanceService();
    var track = CreateTestTrack(pointCount: 500);

    // Force GC
    GC.Collect(2, GCCollectionMode.Forced, blocking: true);
    GC.WaitForPendingFinalizers();

    long gen0Before = GC.CollectionCount(0);
    long gen1Before = GC.CollectionCount(1);

    // Run 10000 iterations
    for (int i = 0; i < 10000; i++)
        service.CalculateGuidance(...);

    long gen0After = GC.CollectionCount(0);
    long gen1After = GC.CollectionCount(1);

    Assert.That(gen0After - gen0Before, Is.LessThanOrEqualTo(1),
        "Hot path should cause max 1 Gen0 collection per 10k calls");
    Assert.That(gen1After - gen1Before, Is.EqualTo(0),
        "Hot path should never trigger Gen1 collection");
}
```

---

## 📐 Geometry Code Standards

### Distance Methods (AgOpenGPS.Core/Geometry/)

**Always provide BOTH versions:**

```csharp
public static class GeometryUtils
{
    // For comparisons (fast)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double DistanceSquared(vec3 a, vec3 b)
    {
        double dx = b.easting - a.easting;
        double dy = b.northing - a.northing;
        return dx * dx + dy * dy;
    }

    // For actual distance (only when needed)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Distance(vec3 a, vec3 b)
    {
        return Math.Sqrt(DistanceSquared(a, b));
    }

    // For line segment distance (fast)
    public static double DistanceToSegmentSquared(vec2 point, vec3 p1, vec3 p2)
    {
        // ... implementation
        return dx * dx + dy * dy;  // Return squared
    }

    // With full result
    public static double DistanceToSegment(vec2 point, vec3 p1, vec3 p2,
                                          out vec3 closestPoint, out double time)
    {
        double distSq = DistanceToSegmentSquared(point, p1, p2);
        // ... calculate closestPoint and time
        return Math.Sqrt(distSq);
    }
}
```

### FindClosestSegment() Template

```csharp
public static class PathUtils
{
    public static bool FindClosestSegment(
        this List<vec3> points,
        vec2 searchPoint,
        out int indexA,
        out int indexB,
        bool loop = false)
    {
        indexA = -1;
        indexB = -1;

        if (points.Count < 2) return false;

        // Phase 1: Coarse search (adaptive step size)
        int step = Math.Max(1, points.Count / 50);
        int roughIndex = 0;
        double minDistSq = double.MaxValue;

        for (int i = 0; i < points.Count; i += step)
        {
            double distSq = GeometryUtils.DistanceSquared(searchPoint, points[i]);
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

        for (int B = start; B < end; B++)
        {
            int A = B == 0 ? (loop ? points.Count - 1 : -1) : B - 1;
            if (A < 0) continue;

            double distSq = GeometryUtils.DistanceToSegmentSquared(
                searchPoint, points[A], points[B]);

            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                indexA = A;
                indexB = B;
            }
        }

        return indexA >= 0;
    }
}
```

---

## 🏗️ Service Design Patterns

### Pattern 1: Reusable Buffers

```csharp
public class GuidanceService : IGuidanceService
{
    // Reusable buffers (no per-frame allocation)
    private List<vec3> _workingBuffer = new List<vec3>(capacity: 1000);
    private readonly GuidanceState _state = new GuidanceState();

    // Struct for results (stack allocated)
    public struct GuidanceResult
    {
        public double SteerAngle;
        public double DistanceFromLine;
        public double HeadingError;
        public vec3 GoalPoint;
        public int SegmentIndexA;
        public int SegmentIndexB;
    }

    public GuidanceResult CalculateGuidance(...)
    {
        _workingBuffer.Clear();  // Reuse, no allocation

        GuidanceResult result;  // Stack allocated
        result.SteerAngle = 0;
        result.DistanceFromLine = 0;
        // ... calculate

        return result;
    }
}
```

### Pattern 2: Conditional Async

```csharp
public async Task<List<vec3>> BuildGuidanceTrackAsync(Track track, double offset)
{
    // Only use async for heavy operations
    if (track.CurvePts.Count > 1000 || track.Mode == TrackMode.BoundaryCurve)
    {
        // Heavy - worth the Task overhead
        return await Task.Run(() => BuildGuidanceTrack(track, offset));
    }
    else
    {
        // Light - run sync (no overhead)
        return BuildGuidanceTrack(track, offset);
    }
}

// Sync version (no Task overhead)
private List<vec3> BuildGuidanceTrack(Track track, double offset)
{
    var result = new List<vec3>(track.CurvePts.Count);
    // ... build
    return result;
}
```

### Pattern 3: Object Pooling (Advanced)

```csharp
// AgOpenGPS.Core/Pooling/Vec3ListPool.cs
public static class Vec3ListPool
{
    private static readonly ConcurrentBag<List<vec3>> _pool =
        new ConcurrentBag<List<vec3>>();

    public static List<vec3> Rent(int minCapacity = 0)
    {
        if (_pool.TryTake(out var list))
        {
            list.Clear();
            if (list.Capacity < minCapacity)
                list.Capacity = minCapacity;
            return list;
        }
        return new List<vec3>(minCapacity);
    }

    public static void Return(List<vec3> list)
    {
        if (list != null && list.Capacity < 5000)  // Don't pool huge lists
            _pool.Add(list);
    }
}

// Usage
public List<vec3> OffsetLine(...)
{
    var result = Vec3ListPool.Rent(points.Count);
    // ... build list
    return result;
}

// Caller responsible for returning
var offsetLine = service.OffsetLine(...);
try
{
    // ... use offsetLine
}
finally
{
    Vec3ListPool.Return(offsetLine);
}
```

---

## 📋 Code Review Checklist

Voor elke PR, check:

### Performance Checklist

- [ ] **No allocations in hot paths** (loops that run per-frame)
- [ ] **Collections pre-allocated with capacity**
- [ ] **Squared distance used for comparisons** (no Math.Sqrt in loops)
- [ ] **AggressiveInlining on small utility methods** (<10 lines)
- [ ] **Two-phase search for large datasets** (>100 points)
- [ ] **Async only for heavy operations** (not in <1ms methods)
- [ ] **Structs for small data** (results, coordinates)
- [ ] **Reusable buffers in services** (no per-call allocation)

### Testing Checklist

- [ ] **Performance test exists** (target <Xms specified)
- [ ] **Allocation test exists** (max Gen0 collections specified)
- [ ] **Large dataset test** (1000+ points)
- [ ] **Warmup iterations before measurement** (JIT optimization)
- [ ] **Multiple runs for average** (min 1000 iterations)

---

## 🎯 Performance Targets per Phase

### Phase 1: Geometry Foundation

**Target**: All geometry methods <0.1ms for 1000-point datasets

```csharp
[Test]
public void OffsetLine_Performance_1000Points_Under100us()
{
    var points = CreateCurveWith1000Points();

    var sw = Stopwatch.StartNew();
    for (int i = 0; i < 1000; i++)
    {
        var result = points.OffsetLine(distance: 5.0, minDist: 0.5, loop: false);
        Vec3ListPool.Return(result);  // Clean up
    }
    sw.Stop();

    double avgUs = sw.Elapsed.TotalMilliseconds * 1000.0 / 1000.0;
    Assert.That(avgUs, Is.LessThan(100.0));
}
```

### Phase 3: Track Service

**Target**: BuildGuidanceTrack <5ms for 500-point curve

```csharp
[Test]
public void TrackService_BuildGuidanceTrack_Under5ms()
{
    var service = new TrackService();
    var track = CreateTestTrack(pointCount: 500);

    var sw = Stopwatch.StartNew();
    for (int i = 0; i < 100; i++)
    {
        var result = service.BuildGuidanceTrack(track, offset: 10.0);
    }
    sw.Stop();

    double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;
    Assert.That(avgMs, Is.LessThan(5.0));
}
```

### Phase 4: Guidance Service

**Target**: CalculateGuidance <1ms (Stanley or Pure Pursuit)

```csharp
[Test]
public void GuidanceService_Stanley_Under1ms()
{
    var service = new GuidanceService();
    var track = CreateTestTrack(pointCount: 500);

    // Warmup
    for (int i = 0; i < 100; i++)
        service.CalculateStanley(...);

    var sw = Stopwatch.StartNew();
    for (int i = 0; i < 1000; i++)
    {
        service.CalculateStanley(...);
    }
    sw.Stop();

    double avgMs = sw.Elapsed.TotalMilliseconds / 1000.0;
    Assert.That(avgMs, Is.LessThan(1.0),
        $"Stanley guidance averaged {avgMs:F3}ms");
}
```

### Phase 5: YouTurn Service

**Target**: CreateYouTurn <50ms (acceptable latency - not per-frame)

```csharp
[Test]
public void YouTurnService_CreateYouTurn_Under50ms()
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
        "YouTurn creation is not per-frame, but should be <50ms");
}
```

---

## 🔬 Profiling Strategy

### Step 1: Baseline Measurement

```csharp
// Add to FormGPS.cs or test harness
private void MeasureGuidanceLoop()
{
    var stats = new List<double>();

    for (int i = 0; i < 10000; i++)
    {
        var sw = Stopwatch.StartNew();

        // Full guidance calculation
        _guidanceService.CalculateGuidance(...);

        sw.Stop();
        stats.Add(sw.Elapsed.TotalMilliseconds);
    }

    Console.WriteLine($"Guidance Stats:");
    Console.WriteLine($"  Average: {stats.Average():F3}ms");
    Console.WriteLine($"  Median:  {stats.OrderBy(x => x).ElementAt(stats.Count/2):F3}ms");
    Console.WriteLine($"  P95:     {stats.OrderBy(x => x).ElementAt((int)(stats.Count*0.95)):F3}ms");
    Console.WriteLine($"  P99:     {stats.OrderBy(x => x).ElementAt((int)(stats.Count*0.99)):F3}ms");
    Console.WriteLine($"  Max:     {stats.Max():F3}ms");
}
```

### Step 2: Allocation Profiling

```csharp
[Test]
public void MeasureAllocations()
{
    var service = new GuidanceService();

    // Force full GC
    GC.Collect(2, GCCollectionMode.Forced, blocking: true);
    GC.WaitForPendingFinalizers();

    long memBefore = GC.GetTotalMemory(forceFullCollection: true);

    // Run 10000 iterations
    for (int i = 0; i < 10000; i++)
    {
        service.CalculateGuidance(...);
    }

    long memAfter = GC.GetTotalMemory(forceFullCollection: false);

    double avgAllocation = (memAfter - memBefore) / 10000.0;

    Console.WriteLine($"Average allocation per call: {avgAllocation:F1} bytes");
    Console.WriteLine($"Target: <100 bytes per call");

    Assert.That(avgAllocation, Is.LessThan(100.0),
        "Hot path should allocate <100 bytes per call");
}
```

---

## 🚀 Quick Reference Card

**Print this and keep near your screen while coding:**

```
╔══════════════════════════════════════════════════════════╗
║         PERFORMANCE-FIRST QUICK REFERENCE                ║
╠══════════════════════════════════════════════════════════╣
║                                                          ║
║  🔥 HOT PATH RULES (runs every frame):                  ║
║                                                          ║
║  ❌ NO:  new List<T>()                                  ║
║  ✅ YES: _reuseableBuffer.Clear()                       ║
║                                                          ║
║  ❌ NO:  Math.Sqrt() in loops                           ║
║  ✅ YES: Compare squared distances                      ║
║                                                          ║
║  ❌ NO:  Linear search (O(n))                           ║
║  ✅ YES: Two-phase search (coarse + fine)               ║
║                                                          ║
║  ❌ NO:  await Task.Run() for <1ms work                 ║
║  ✅ YES: Sync execution or conditional async            ║
║                                                          ║
║ ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ ║
║                                                          ║
║  🎯 PERFORMANCE TARGETS:                                 ║
║                                                          ║
║  • Geometry utils:     <0.1ms                           ║
║  • FindClosestSegment: <0.5ms                           ║
║  • Stanley/PP:         <1.0ms                           ║
║  • BuildGuidanceTrack: <5.0ms                           ║
║  • Total per frame:    <2.5ms                           ║
║                                                          ║
║ ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ ║
║                                                          ║
║  🧪 REQUIRED TESTS:                                      ║
║                                                          ║
║  1. Performance test with target time                   ║
║  2. Allocation test (max Gen0 collections)              ║
║  3. Large dataset test (1000+ points)                   ║
║                                                          ║
║ ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ ║
║                                                          ║
║  💡 REMEMBER:                                            ║
║                                                          ║
║  "Premature optimization is the root of all evil"       ║
║  BUT...                                                  ║
║  "Designing for performance from the start is wisdom"   ║
║                                                          ║
╚══════════════════════════════════════════════════════════╝
```

---

## 📚 Additional Resources

**Must-read before starting each phase:**

1. [C# Performance Tips](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/performance/)
2. [Allocation-free C#](https://www.jacksondunstan.com/articles/3805)
3. [BenchmarkDotNet](https://benchmarkdotnet.org/) - Voor accurate benchmarks
4. [dotTrace Profiler](https://www.jetbrains.com/profiler/) - Voor profiling

**Before merging any PR:**

- Run BenchmarkDotNet suite
- Check allocations with dotMemory
- Profile with dotTrace
- Test on target hardware (e.g., tractor PC, not dev machine!)

---

## ✅ Summary

**Golden Rule**: Als het in een loop zit die per-frame draait, moet het **<0.5ms** zijn en **zero allocations**.

**Success Criteria**: Hele guidance loop (FindClosestSegment + Stanley/PP + state update) moet **<2.5ms** zijn voor 500-punt curve.

**Testing**: Elke service method heeft performance test met target tijd + allocation test.

**Mindset**: "Would this code run at 100Hz for 8 hours straight without GC pressure?" → If no, optimize!
