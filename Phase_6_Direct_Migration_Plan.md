# Phase 6: Direct Migration Plan - CTrk → Track

**Date**: 2025-01-19
**Approach**: Direct replacement (no adapter!)
**Goal**: Replace CTrk/CTrack/CABLine/CABCurve/CGuidance completely with Track/TrackService/GuidanceService

---

## 🎯 Migration Strategy: DIRECT REPLACEMENT

**No Migration Adapter** - We replace everything in one clean sweep:
- `CTrk` → `Track` (AgOpenGPS.Core.Models.Track)
- `CTrack` → `TrackService` (AgOpenGPS.Core.Services.TrackService)
- `CABLine/CABCurve guidance` → `GuidanceService`
- Delete old code completely

---

## 📋 Phase 6 Steps (Revised)

### ✅ Phase 6.1: Service Initialization (DONE)
- Services added to FormGPS constructor ✅

### ✅ Phase 6.2: UpdateGuidance() Method (DONE)
- Unified guidance method created ✅

### Phase 6.3: Replace CTrk → Track in TrackFiles.cs

**File**: `SourceCode/GPS/IO/TrackFiles.cs`

**Current**:
```csharp
public static List<CTrk> Load(string fieldDirectory)
{
    var result = new List<CTrk>();
    // ... load from file
    var track = new CTrk
    {
        name = name,
        heading = heading,
        ptA = ptA,
        ptB = ptB,
        // ...
    };
    result.Add(track);
    return result;
}

public static void Save(string fieldDirectory, List<CTrk> tracks)
{
    // ... save to file
}
```

**NEW**:
```csharp
public static List<Track> Load(string fieldDirectory)
{
    var result = new List<Track>();
    // ... load from file
    var track = new Track
    {
        Id = Guid.NewGuid(),  // NEW
        Name = name,
        Heading = heading,
        PtA = new GeoCoord(ptA.easting, ptA.northing),  // Convert vec2 → GeoCoord
        PtB = new GeoCoord(ptB.easting, ptB.northing),
        Mode = mode,
        CurvePts = curvePts,
        // ...
    };
    result.Add(track);
    return result;
}

public static void Save(string fieldDirectory, List<Track> tracks)
{
    // ... save to file (same format, just different types)
}
```

**Changes**:
1. Change return type `List<CTrk>` → `List<Track>`
2. Change parameter type in Save
3. Convert vec2 → GeoCoord for PtA/PtB
4. Generate Guid for Track.Id
5. Update property names (lowercase → PascalCase)

### Phase 6.4: Replace CTrack → TrackService in FormGPS

**File**: `SourceCode/GPS/Forms/FormGPS.cs`

**OLD Declaration** (line ~356):
```csharp
//new track instance
trk = new CTrack(this);
```

**NEW - REMOVE OLD, USE SERVICE**:
```csharp
// OLD CTrack removed - now using _trackService (initialized in Phase 6.1)
// trk = new CTrack(this);  // DELETE THIS
```

**Field Declaration** (line ~196):
```csharp
// OLD
public CTrack trk;

// NEW - REMOVE, already have _trackService
// Just delete the field entirely
```

**Update Field Load** (SaveOpen.Designer.cs):
```csharp
// OLD
trk.gArr?.Clear();
trk.gArr.AddRange(tracks);
trk.idx = -1;

// NEW
_trackService.ClearTracks();
foreach (var track in tracks)
{
    _trackService.AddTrack(track);
}
_trackService.SetCurrentTrackIndex(-1);  // No track selected
```

**Update Field Save** (SaveOpen.Designer.cs):
```csharp
// OLD
TrackFiles.Save(GetFieldDir(true), trk.gArr);

// NEW
var tracks = _trackService.GetAllTracks();
TrackFiles.Save(GetFieldDir(true), tracks);
```

### Phase 6.5: Replace Guidance Calls in Position.designer.cs

**File**: `SourceCode/GPS/Forms/Position.designer.cs` (line ~896-908)

**OLD**:
```csharp
if (trk.gArr != null && trk.gArr.Count > 0 && trk.idx >= 0 && trk.idx < trk.gArr.Count)
{
    if (trk.gArr[trk.idx].mode == TrackMode.AB)
    {
        ABLine.BuildCurrentABLineList(pivotAxlePos);
        ABLine.GetCurrentABLine(pivotAxlePos, steerAxlePos);
    }
    else
    {
        curve.BuildCurveCurrentList(pivotAxlePos);
        curve.GetCurrentCurveLine(pivotAxlePos, steerAxlePos);
    }
}
```

**NEW**:
```csharp
// Get current track from service
var currentTrack = _trackService.GetCurrentTrack();
if (currentTrack != null && currentTrack.CurvePts != null && currentTrack.CurvePts.Count >= 2)
{
    // Check for YouTurn override first
    if (yt.isYouTurnTriggered && yt.DistanceFromYouTurnLine())
    {
        // Use YouTurn guidance (preserve existing logic)
        // This will be handled separately
    }
    else
    {
        // Calculate offset for current pass
        double offset = CalculateTrackOffset(currentTrack, pivotAxlePos);

        // Build guidance track with offset
        var guidanceTrack = _trackService.BuildGuidanceTrack(currentTrack, offset);

        // Calculate guidance using NEW service
        var result = UpdateGuidance(steerAxlePos, guidanceTrack);

        // Result already sets guidanceLineDistanceOff and guidanceLineSteerAngle
    }
}
```

**Add Helper Method to FormGPS**:
```csharp
private double CalculateTrackOffset(Track track, vec3 pivotPosition)
{
    // Extract offset calculation from CABLine.BuildCurrentABLineList()
    // This calculates which pass we're on based on distance from reference line

    double widthMinusOverlap = tool.width - tool.overlap;

    // Calculate distance from reference line
    double distanceFromRefLine;
    if (track.Mode == TrackMode.AB)
    {
        // AB line distance calculation
        double dx = track.PtB.Easting - track.PtA.Easting;
        double dy = track.PtB.Northing - track.PtA.Northing;

        distanceFromRefLine = ((dy * pivotPosition.easting) - (dx * pivotPosition.northing)
            + (track.PtB.Easting * track.PtA.Northing)
            - (track.PtB.Northing * track.PtA.Easting))
            / Math.Sqrt((dy * dy) + (dx * dx));
    }
    else
    {
        // Curve - use TrackService to get distance
        var (distance, sameway) = _trackService.GetDistanceFromTrack(
            track,
            new vec2(pivotPosition.easting, pivotPosition.northing),
            pivotPosition.heading);
        distanceFromRefLine = distance;
    }

    distanceFromRefLine -= (0.5 * widthMinusOverlap);

    // Determine heading direction
    bool isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivotPosition.heading - track.Heading) - Math.PI) < glm.PIBy2;

    // Calculate which pass
    double RefDist = (distanceFromRefLine + (isHeadingSameWay ? tool.offset : -tool.offset) - track.NudgeDistance) / widthMinusOverlap;

    int howManyPathsAway;
    if (RefDist < 0) howManyPathsAway = (int)(RefDist - 0.5);
    else howManyPathsAway = (int)(RefDist + 0.5);

    // Calculate offset distance
    double distAway = widthMinusOverlap * howManyPathsAway
        + (isHeadingSameWay ? -tool.offset : tool.offset)
        + track.NudgeDistance;

    distAway += (0.5 * widthMinusOverlap);

    return distAway;
}
```

### Phase 6.6: Update All UI Forms Using trk.gArr

**Files to Update** (~27 files):

Pattern to find:
```bash
grep -r "trk\.gArr\|trk\.idx" --include="*.cs" SourceCode/GPS/Forms/
```

**Replacement Pattern**:
```csharp
// OLD
trk.gArr[i]
trk.gArr.Count
trk.idx

// NEW
_trackService.GetTrackAt(i)  // or GetAllTracks()[i]
_trackService.GetTrackCount()
_trackService.GetCurrentTrackIndex()
```

**Common Patterns**:

1. **Track List Display** (FormBuildTracks, etc.):
```csharp
// OLD
for (int i = 0; i < trk.gArr.Count; i++)
{
    var track = trk.gArr[i];
    // ...
}

// NEW
var tracks = _trackService.GetAllTracks();
for (int i = 0; i < tracks.Count; i++)
{
    var track = tracks[i];
    // ...
}
```

2. **Current Track Selection**:
```csharp
// OLD
if (trk.idx >= 0 && trk.idx < trk.gArr.Count)
{
    var current = trk.gArr[trk.idx];
}

// NEW
var current = _trackService.GetCurrentTrack();
if (current != null)
{
    // ...
}
```

3. **Track Creation**:
```csharp
// OLD
var newTrack = new CTrk { name = "AB1", mode = TrackMode.AB, ... };
trk.gArr.Add(newTrack);
trk.idx = trk.gArr.Count - 1;

// NEW
var newTrack = new Track
{
    Id = Guid.NewGuid(),
    Name = "AB1",
    Mode = TrackMode.AB,
    ...
};
_trackService.AddTrack(newTrack);
_trackService.SetCurrentTrack(newTrack);
```

**TrackService Methods Needed**:
```csharp
// Add these to ITrackService if not present:
List<Track> GetAllTracks();
Track GetTrackAt(int index);
int GetTrackCount();
int GetCurrentTrackIndex();
void SetCurrentTrackIndex(int index);
void ClearTracks();
void RemoveTrack(Track track);
void RemoveTrackAt(int index);
```

### Phase 6.7: Test Build and Fix Errors

**Process**:
1. Build after each file update
2. Fix compilation errors immediately
3. Common errors:
   - Type mismatches (CTrk vs Track)
   - Property name changes (lowercase vs PascalCase)
   - Missing TrackService methods

**Build Command**:
```bash
dotnet build SourceCode/AgOpenGPS.sln --configuration Debug
```

### Phase 6.8: Delete Old Files

**Files to DELETE**:
1. `SourceCode/GPS/Classes/CGuidance.cs` (413 lines)
2. `SourceCode/GPS/Classes/CTrack.cs` (entire file)
3. `SourceCode/GPS/Classes/CABLine.cs` (660 lines)
4. `SourceCode/GPS/Classes/CABCurve.cs` (1539 lines)

**Total**: ~2600 lines deleted!

**Verify**:
- No references to CGuidance, CTrack, CABLine, CABCurve
- Build succeeds
- No warnings about unused using statements

### Phase 6.9: Final Verification

**Checklist**:
- [ ] Code compiles without errors
- [ ] Can load existing field
- [ ] Can create new AB line
- [ ] Can create new Curve
- [ ] AB line guidance works
- [ ] Curve guidance works
- [ ] Can switch between tracks
- [ ] Field save works
- [ ] Track list displays correctly
- [ ] Nudge works
- [ ] YouTurn works

---

## 🔧 TrackService Extensions Needed

Add these methods to `TrackService.cs` if missing:

```csharp
public List<Track> GetAllTracks()
{
    return _tracks.GetAllTracks();
}

public Track GetTrackAt(int index)
{
    var tracks = _tracks.GetAllTracks();
    if (index < 0 || index >= tracks.Count) return null;
    return tracks[index];
}

public int GetTrackCount()
{
    return _tracks.GetAllTracks().Count;
}

public int GetCurrentTrackIndex()
{
    var current = GetCurrentTrack();
    if (current == null) return -1;

    var tracks = GetAllTracks();
    for (int i = 0; i < tracks.Count; i++)
    {
        if (tracks[i].Id == current.Id) return i;
    }
    return -1;
}

public void SetCurrentTrackIndex(int index)
{
    var track = GetTrackAt(index);
    SetCurrentTrack(track);
}

public void ClearTracks()
{
    _tracks = new TrackCollection();
    SetCurrentTrack(null);
}

public void RemoveTrack(Track track)
{
    if (track == null) return;
    var tracks = GetAllTracks();
    tracks.Remove(track);
    if (GetCurrentTrack()?.Id == track.Id)
    {
        SetCurrentTrack(null);
    }
}

public void RemoveTrackAt(int index)
{
    var track = GetTrackAt(index);
    if (track != null) RemoveTrack(track);
}
```

---

## 📊 Migration Checklist

### Phase 6.3: TrackFiles.cs
- [ ] Change Load() return type to List<Track>
- [ ] Change Save() parameter to List<Track>
- [ ] Convert vec2 → GeoCoord
- [ ] Generate Track.Id
- [ ] Update property names
- [ ] Test file load/save

### Phase 6.4: FormGPS CTrack Replacement
- [ ] Remove `public CTrack trk` field
- [ ] Remove `trk = new CTrack(this)` initialization
- [ ] Update field load to use _trackService
- [ ] Update field save to use _trackService
- [ ] Test build

### Phase 6.5: Position.designer.cs Guidance
- [ ] Add CalculateTrackOffset() helper
- [ ] Replace ABLine.GetCurrentABLine() with UpdateGuidance()
- [ ] Replace curve.GetCurrentCurveLine() with UpdateGuidance()
- [ ] Preserve YouTurn logic
- [ ] Test build

### Phase 6.6: UI Forms Update
- [ ] SaveOpen.Designer.cs
- [ ] GUI.Designer.cs
- [ ] Controls.Designer.cs
- [ ] OpenGL.Designer.cs
- [ ] FormBuildTracks.cs
- [ ] FormNudge.cs
- [ ] FormQuickAB.cs
- [ ] FormGrid.cs
- [ ] FormTram.cs
- [ ] FormTramLine.cs
- [ ] Other forms (as needed)

### Phase 6.7: Build Testing
- [ ] Build succeeds
- [ ] Zero errors
- [ ] Zero warnings (or acceptable)

### Phase 6.8: Delete Old Code
- [ ] Delete CGuidance.cs
- [ ] Delete CTrack.cs
- [ ] Delete CABLine.cs
- [ ] Delete CABCurve.cs
- [ ] Build succeeds

### Phase 6.9: Smoke Test
- [ ] Load field
- [ ] Create AB line
- [ ] Drive with AB guidance
- [ ] Create Curve
- [ ] Drive with Curve guidance
- [ ] Save field
- [ ] All features work

---

## 🎯 Success Criteria

Phase 6 Complete when:
1. ✅ All CTrk → Track
2. ✅ All CTrack → TrackService
3. ✅ All guidance uses GuidanceService
4. ✅ Build succeeds with 0 errors
5. ✅ Field load/save works
6. ✅ Guidance works (AB + Curve)
7. ✅ Old files deleted (CGuidance, CTrack, CABLine, CABCurve)
8. ✅ ~2600 lines of old code removed

---

*Let's execute this plan step by step!*
