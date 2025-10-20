# Phase 6.10: YouTurn Integration with AOG_Dev Method

**Date**: 2025-10-20
**Status**: 60% complete - Manual UTurn working, NextPath track switching remaining
**Goal**: Integrate YouTurn functionality using exact AOG_Dev geometry methods

## Problem Statement

After Phase 6.9 (self-intersection prevention), we needed to integrate YouTurn functionality:
1. ✅ **Manual UTurn buttons did nothing** - legacy CTrk not populated
2. ✅ **Turn drawn in wrong position** - using ABLine/curve data instead of unified guidance
3. ✅ **Turn shape completely wrong** - "hairpin instead of U-turn" per user
4. ⏳ **Lookahead jumps to start** - needs investigation
5. ⏳ **Next track not selected** - missing track switching during turn

## Analysis - Why Manual UTurns Failed

### Root Cause 1: Legacy CTrk Usage
`BuildManualYouTurn()` used `mf.trk.idx` and `mf.trk.gArr` (legacy CTrk)
- CTrk is obsolete and no longer populated after Phase 6 migration
- Need to use unified guidance data instead

### Root Cause 2: Using Wrong Position Data
```csharp
// WRONG (Phase 6.10 initial attempt):
double rEastYT = mf.ABLine.rEastAB;  // or mf.curve.rEastCu
double rNorthYT = mf.ABLine.rNorthAB; // Mode-specific data

// CORRECT (AOG_Dev approach):
double rEastYT = mf.gyd.rEastTrk;   // Unified track position
double rNorthYT = mf.gyd.rNorthTrk; // Same data regardless of mode
```

### Root Cause 3: Wrong Geometry Method
- AOG_Dev uses `GetOffsetSemicirclePoints()` with sagitta compensation
- Initial implementation used Dubins path
- Result: hairpin shape instead of smooth U-turn

## Solution - Unified Guidance Data (Like AOG_Dev)

### Added to FormGPS.cs (Lines 284-286, 921-930)

**Unified guidance properties:**
```csharp
// Phase 6.10: Unified guidance data (like AOG_Dev gyd)
public double rEastTrk, rNorthTrk;      // Closest point on track
public double manualUturnHeading;        // Heading for manual U-turn
public bool isHeadingSameWay;            // Direction flag
```

**Populated in UpdateGuidance():**
```csharp
// Phase 6.10: Update unified guidance data
if (result.ClosestPointIndex >= 0 && result.ClosestPointIndex < guidanceTrack.Count)
{
    var closestPt = guidanceTrack[result.ClosestPointIndex];
    rEastTrk = closestPt.easting;
    rNorthTrk = closestPt.northing;
    manualUturnHeading = closestPt.heading;
}
this.isHeadingSameWay = isHeadingSameWay;
```

## AOG_Dev GetOffsetSemicirclePoints Method

Implemented exact AOG_Dev geometry in CYouTurn.cs:2551-2588

### Key Concepts

1. **Sagitta Compensation**: When turnOffset < 2×radius, add extra offset to prevent sharp turns
2. **Two Semicircles**: Start and goal positions each get their own semicircle
3. **Reverse & Combine**: Second semicircle reversed and merged with first
4. **Extension Points**: Added at start/end for smooth entry/exit

### GetOffsetSemicirclePoints Implementation

```csharp
private List<vec3> GetOffsetSemicirclePoints(
    vec3 currentPos,
    double theta,
    bool isTurningRight,
    double turningRadius,
    double offsetDistance,
    double angle)
{
    List<vec3> points = new List<vec3>() { currentPos };

    // Calculate first arc angle for offset (sagitta compensation)
    double firstArcAngle = Math.Acos(1 - (offsetDistance * 0.5) / turningRadius);

    if (offsetDistance > 0)
    {
        // Add initial offset arc (opposite direction to compensate)
        AddCoordinatesToPath(ref currentPos, ref theta, points,
            firstArcAngle * turningRadius, !isTurningRight, turningRadius);
    }

    // Add remaining semicircle (main turn direction)
    double remainingAngle = angle + firstArcAngle;
    AddCoordinatesToPath(ref currentPos, ref theta, points,
        remainingAngle * turningRadius, isTurningRight, turningRadius);

    return points;
}
```

### AddCoordinatesToPath Implementation

```csharp
private void AddCoordinatesToPath(
    ref vec3 currentPos,
    ref double theta,
    List<vec3> finalPath,
    double length,
    bool isTurningRight,
    double turningRadius)
{
    // Generate arc segments incrementally
    int segments = (int)Math.Ceiling(length / (turningRadius * 0.1));
    double dist = length / segments;
    double turnParameter = (dist / turningRadius) * (isTurningRight ? 1.0 : -1.0);
    double radius = isTurningRight ? turningRadius : -turningRadius;

    double sinH = Math.Sin(theta);
    double cosH = Math.Cos(theta);

    for (int i = 0; i < segments; i++)
    {
        currentPos.easting += cosH * radius;
        currentPos.northing -= sinH * radius;
        theta += turnParameter;
        theta %= glm.twoPI;
        sinH = Math.Sin(theta);
        cosH = Math.Cos(theta);
        currentPos.easting -= cosH * radius;
        currentPos.northing += sinH * radius;

        finalPath.Add(currentPos);
    }
}
```

## Updated BuildManualYouTurn

CYouTurn.cs:2496-2549 - Complete rewrite using unified data + AOG_Dev geometry:

```csharp
public void BuildManualYouTurn(bool isTurnRight, bool isTurnButtonTriggered)
{
    // Phase 6.10: Use unified guidance data (like AOG_Dev)
    var currentTrack = mf._trackService.GetCurrentTrack();
    if (currentTrack == null || !currentTrack.IsValid()) return;

    // Use unified guidance data from FormGPS
    double head = mf.manualUturnHeading;
    isHeadingSameWay = mf.isHeadingSameWay;

    double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth +
        (isTurnRight ? mf.tool.offset * 2.0 : -mf.tool.offset * 2.0);

    if (!isHeadingSameWay) head += Math.PI;

    // Use unified track position (NOT ABLine/curve specific!)
    double rEastYT = mf.rEastTrk + Math.Sin(head) * (4 + youTurnStartOffset);
    double rNorthYT = mf.rNorthTrk + Math.Cos(head) * (4 + youTurnStartOffset);

    vec3 start = new vec3(rEastYT, rNorthYT, head);
    vec3 goal = new vec3();

    // Calculate goal point (perpendicular offset for turn width)
    if (isTurnRight)
    {
        goal.easting = start.easting + (Math.Cos(head) * turnOffset);
        goal.northing = start.northing - (Math.Sin(head) * turnOffset);
    }
    else
    {
        goal.easting = start.easting - (Math.Cos(head) * turnOffset);
        goal.northing = start.northing + (Math.Sin(head) * turnOffset);
    }

    // Calculate extra sagitta for tight turns (AOG_Dev line 806-808)
    double extraSagitta = 0;
    if (Math.Abs(turnOffset) < youTurnRadius * 2)
        extraSagitta = (youTurnRadius * 2 - Math.Abs(turnOffset)) * 0.5;

    // Generate two semicircles using AOG_Dev method (line 810-814)
    ytList = GetOffsetSemicirclePoints(start, head, isTurnRight,
        youTurnRadius, extraSagitta, glm.PIBy2);
    ytList2 = GetOffsetSemicirclePoints(goal, head, !isTurnRight,
        youTurnRadius, extraSagitta, glm.PIBy2);

    // Combine semicircles
    ytList2.Reverse();
    ytList.AddRange(ytList2);

    // Add extension points at start and end (line 816-817)
    ytList.Insert(0, new vec3(
        start.easting - Math.Sin(head) * youTurnStartOffset,
        start.northing - Math.Cos(head) * youTurnStartOffset,
        0));
    ytList.Add(new vec3(
        goal.easting - Math.Sin(head) * youTurnStartOffset,
        goal.northing - Math.Cos(head) * youTurnStartOffset,
        0));

    isTurnLeft = !isTurnRight;
    YouTurnTrigger();
}
```

## Files Modified

1. **FormGPS.cs:284-286**
   - Added: `rEastTrk`, `rNorthTrk`, `manualUturnHeading`, `isHeadingSameWay`

2. **FormGPS.cs:921-930**
   - UpdateGuidance() populates unified guidance data

3. **CYouTurn.cs:2496-2549**
   - BuildManualYouTurn() completely rewritten
   - Uses unified data instead of ABLine/curve
   - Uses AOG_Dev semicircle generation

4. **CYouTurn.cs:2551-2588**
   - Added GetOffsetSemicirclePoints()
   - Added AddCoordinatesToPath()

## Current Status

### ✅ Complete

1. **Unified guidance data** - rEastTrk, rNorthTrk, manualUturnHeading
2. **AOG_Dev geometry methods** - GetOffsetSemicirclePoints + AddCoordinatesToPath
3. **BuildManualYouTurn rewrite** - Uses unified data + correct geometry
4. **Manual UTurn buttons work** - Triggers correctly
5. **Correct turn shape** - Proper U-turn (not hairpin)
6. **Correct turn position** - Positioned relative to track

### ⏳ Remaining

1. **NextPath() track switching** - Select next line during YouTurn
2. **Lookahead jumping** - Investigate why lookahead jumps to turn start
3. **Auto UTurn** - Test automatic turn creation
4. **Core Migration** - Move to YouTurnService.cs (future phase)

## Next Steps

### Phase 6.10 Completion (NextPath)

**Goal**: Automatically switch to next track line during YouTurn

**AOG_Dev Implementation** (line 708-728):
```csharp
public void NextPath()
{
    isGoingStraightThrough = true;

    // Update track offset and reverse direction
    mf.trk.howManyPathsAway += (isTurnLeft ^ mf.trk.isHeadingSameWay)
        ? rowSkipsWidth
        : -rowSkipsWidth;
    mf.trk.isHeadingSameWay = !mf.trk.isHeadingSameWay;

    // Handle alternate skips...
    else isTurnLeft = !isTurnLeft;
}
```

**When to call**: From Position.designer.cs during YouTurn guidance when halfway through turn:
```csharp
// AOG_Dev CGuidance.cs:80-84
if (!mf.yt.isGoingStraightThrough &&
    mf.yt.onA > mf.yt.totalUTurnLength * 0.5)
{
    mf.yt.NextPath();  // Switch tracks at halfway point
}
```

**Implementation needed**:
1. Add NextPath() method to CYouTurn
2. Track distance along YouTurn path (onA)
3. Call NextPath() from Position.designer.cs when onA > totalLength * 0.5
4. Update ABLine.howManyPathsAway and curve.howManyPathsAway
5. Flip isHeadingSameWay

### Phase 6.11: Lookahead Investigation

Debug why lookahead point jumps to start of YouTurn path

### Phase 6.12: Core Migration

Move YouTurn to YouTurnService (future - requires larger refactor):
1. Update IYouTurnService interface
2. Implement in YouTurnService.cs
3. CYouTurn becomes obsolete wrapper
4. Position.designer.cs uses _youTurnService

## User Feedback Timeline

1. ✅ **"Manual Uturn (openGl.Designer) doet niks"**
   - Fix: Use _trackService instead of legacy trk

2. ✅ **"De knoppen reageren wel, maar de Turn word niet op de juiste plek getekend"**
   - Fix: Use unified guidance data (rEastTrk, rNorthTrk)

3. ✅ **"het wordt nu een soort van haarspeldbocht ipv een U"**
   - Fix: Implement AOG_Dev GetOffsetSemicirclePoints method

4. ⏳ **"Manual turn werkt nu alleen hij moet op basis van welke kant we op gaan ook de volgende lijn als currenttrack kiezen"**
   - Next: Implement NextPath() track switching

## Key Learnings

1. **Unified guidance data is critical** - Mode-agnostic position/heading eliminates duplication
2. **AOG_Dev geometry is precise** - GetOffsetSemicirclePoints with sagitta creates perfect U-turns
3. **Incremental arc generation** - AddCoordinatesToPath builds smooth curves segment-by-segment
4. **User feedback drives fixes** - Each iteration narrowed down the specific issue
5. **Reference implementation essential** - AOG_Dev provided the working solution

## References

- **AOG_Dev CYouTurn.cs**:
  - BuildManualYouTurn: lines 775-821
  - GetOffsetSemicirclePoints: lines 2557-2568
  - AddCoordinatesToPath: lines 2575-2588
  - NextPath: lines 708-728
- **AOG_Dev CGuidance.cs**:
  - NextPath call during YouTurn: lines 80-84
