# Phase 6: Detailed Migration Plan - UI Integration

**Date**: 2025-01-19
**Status**: Planning
**Goal**: Complete migration from CABLine/CABCurve/CTrack/CGuidance to TrackService/GuidanceService

---

## 📋 Current Status

### ✅ What We Have Built (Phases 1-5)
- **Phase 1-2**: GeometryUtils, Track models, TrackCollection ✅
- **Phase 3**: TrackService (track management, BuildGuidanceTrack, GetDistance) ✅
- **Phase 4**: GuidanceService (Stanley + Pure Pursuit unified) ✅
- **Phase 5**: YouTurnService (turn generation) ✅

### ✅ What We Started (Phase 6)
- **Phase 6.1**: Added services to FormGPS constructor ✅
- **Phase 6.2**: Created UpdateGuidance() method in FormGPS ✅

### ❌ What We Need to Do
- Replace OLD track system (CTrack/CTrk/CABLine/CABCurve) with NEW (TrackService/Track)
- Replace OLD guidance calls with NEW UpdateGuidance()
- Remove OLD code files

---

## 🔍 Current Code Architecture

### OLD System (What exists now)
```
FormGPS
├─ CTrack trk                  // Track list management
│  └─ List<CTrk> gArr          // Array of tracks
│     └─ CTrk (old track data)
│        ├─ mode (TrackMode)
│        ├─ curList (List<vec3>)
│        └─ nudgeDistance
│
├─ CABLine ABLine              // AB Line track
│  ├─ GetCurrentABLine()       // Guidance calculation
│  └─ BuildCurrentABLineList() // Build current track
│
├─ CABCurve curve              // Curve track
│  ├─ GetCurrentCurveLine()    // Guidance calculation
│  └─ BuildCurveCurrentList()  // Build current track
│
└─ CGuidance gyd               // Guidance algorithms
   ├─ StanleyGuidanceABLine()
   └─ StanleyGuidanceCurve()
```

### NEW System (What we want)
```
FormGPS
├─ ITrackService _trackService      // NEW
│  ├─ TrackCollection               // Unified track list
│  ├─ GetCurrentTrack()             // Active track
│  └─ BuildGuidanceTrack()          // Build for any type
│
├─ IGuidanceService _guidanceService // NEW
│  ├─ CalculateGuidance()           // Unified (AB + Curve!)
│  ├─ Stanley algorithm
│  └─ Pure Pursuit algorithm
│
└─ IYouTurnService _youTurnService   // NEW
   └─ CreateYouTurn()
```

---

## 🎯 Migration Strategy

### Key Insight: CTrack/CTrk IS the Data Source!

The current code uses:
```csharp
// Position.designer.cs line 896-908
if (trk.gArr != null && trk.gArr.Count > 0 && trk.idx >= 0)
{
    if (trk.gArr[trk.idx].mode == TrackMode.AB)
    {
        ABLine.BuildCurrentABLineList(pivotAxlePos);      // Build track geometry
        ABLine.GetCurrentABLine(pivotAxlePos, steerAxlePos); // Calculate guidance
    }
    else
    {
        curve.BuildCurveCurrentList(pivotAxlePos);        // Build track geometry
        curve.GetCurrentCurveLine(pivotAxlePos, steerAxlePos); // Calculate guidance
    }
}
```

**Problem**:
- `trk.gArr[trk.idx]` = OLD CTrk track data
- CABLine/CABCurve = OLD guidance logic + OLD track building

**Solution**:
We need to migrate in TWO parts:
1. **Track Data Migration**: CTrk → Track (TrackService)
2. **Guidance Migration**: CABLine/CABCurve guidance → GuidanceService

---

## 📊 Phase 6 Detailed Breakdown

### Phase 6.1: Service Initialization ✅ DONE
**Status**: Complete
**What**: Initialize services in FormGPS constructor
```csharp
_trackService = new TrackService();
_guidanceService = new GuidanceService();
_youTurnService = new YouTurnService();
```

### Phase 6.2: UpdateGuidance() Method ✅ DONE
**Status**: Complete
**What**: Unified guidance calculation method in FormGPS
```csharp
public GuidanceResult UpdateGuidance(vec3 steerPosition, List<vec3> guidanceTrack)
```

### Phase 6.3: Analyze OLD Track System 🔄 IN PROGRESS
**Status**: In Progress
**Goal**: Understand how CTrack/CTrk works and where it's used

**Key Questions**:
1. Where is CTrack.gArr populated? (field load, track creation)
2. How does trk.idx get set? (track selection)
3. What properties does CTrk have that Track needs?
4. Where is CABLine/CABCurve used besides guidance?

**Files to Analyze**:
- `CTrack.cs` - Track list management
- `CABLine.cs` - AB line logic
- `CABCurve.cs` - Curve logic
- `Position.designer.cs` - Guidance calls
- Field load/save code

**Deliverable**: Document with:
- CTrk → Track property mapping
- CTrack.gArr → TrackService.TrackCollection migration path
- List of all CABLine/CABCurve usage sites

### Phase 6.4: Create Migration Adapter Pattern
**Status**: Pending
**Goal**: Create temporary adapter to bridge OLD and NEW systems

**Why**: We can't migrate everything at once - field save/load, UI, etc. all use CTrack.

**Approach**: Two-way sync adapter
```csharp
public class TrackMigrationAdapter
{
    private readonly ITrackService _trackService;
    private readonly CTrack _oldTrack;

    // Sync OLD → NEW
    public void SyncFromOldToNew()
    {
        _trackService.ClearTracks();
        foreach (var oldTrack in _oldTrack.gArr)
        {
            var newTrack = ConvertCTrkToTrack(oldTrack);
            _trackService.AddTrack(newTrack);
        }

        if (_oldTrack.idx >= 0 && _oldTrack.idx < _oldTrack.gArr.Count)
        {
            var currentOldTrack = _oldTrack.gArr[_oldTrack.idx];
            var currentNewTrack = _trackService.FindByName(currentOldTrack.name);
            _trackService.SetCurrentTrack(currentNewTrack);
        }
    }

    // Sync NEW → OLD (for field save, etc.)
    public void SyncFromNewToOld()
    {
        // TODO: Later when we migrate field save/load
    }

    private Track ConvertCTrkToTrack(CTrk oldTrack)
    {
        var track = new Track
        {
            Id = Guid.NewGuid(),
            Name = oldTrack.name,
            Mode = oldTrack.mode,
            IsVisible = oldTrack.isVisible,
            NudgeDistance = oldTrack.nudgeDistance,
            // ... copy other properties
        };

        // Copy curve points
        if (oldTrack.curList != null)
        {
            track.CurvePts.AddRange(oldTrack.curList);
        }

        // Copy AB line points if AB mode
        if (oldTrack.mode == TrackMode.AB)
        {
            track.PtA = oldTrack.ptA;
            track.PtB = oldTrack.ptB;
            track.Heading = oldTrack.heading;
        }

        return track;
    }
}
```

**Deliverable**: TrackMigrationAdapter class in FormGPS

### Phase 6.5: Replace Guidance Calls in Position.designer.cs
**Status**: Pending
**Goal**: Replace ABLine/Curve guidance calls with UpdateGuidance()

**Current Code** (Position.designer.cs ~line 896-908):
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

**NEW Code**:
```csharp
// Sync OLD tracks to NEW system (temporary during migration)
_trackAdapter.SyncFromOldToNew();

// Get current track from NEW system
var currentTrack = _trackService.GetCurrentTrack();
if (currentTrack != null && currentTrack.CurvePts != null && currentTrack.CurvePts.Count >= 2)
{
    // Build guidance track with offset (tool width, nudge, etc.)
    double offset = CalculateTrackOffset(currentTrack);
    var guidanceTrack = _trackService.BuildGuidanceTrack(currentTrack, offset);

    // Calculate guidance (unified for AB and Curve!)
    var result = UpdateGuidance(steerAxlePos, guidanceTrack);

    // Use result for autosteer
    // guidanceLineDistanceOff and guidanceLineSteerAngle are already set by UpdateGuidance()
}
```

**Helper Method**:
```csharp
private double CalculateTrackOffset(Track track)
{
    // Calculate offset based on:
    // - Which pass we're on (howManyPathsAway)
    // - Tool width - overlap
    // - Tool offset (left/right)
    // - Track nudge distance

    // This logic is currently in CABLine.BuildCurrentABLineList() and curve.BuildCurveCurrentList()
    // We need to extract it
}
```

**Deliverable**: Modified Position.designer.cs with new guidance calls

### Phase 6.6: Extract Offset Calculation Logic
**Status**: Pending
**Goal**: Move offset calculation logic from CABLine/CABCurve to FormGPS or TrackService

**Current Situation**:
- CABLine.BuildCurrentABLineList() calculates which pass, applies tool width/overlap/offset
- CABCurve.BuildCurveCurrentList() does similar
- This logic is complex (100+ lines)

**Options**:
1. **Move to FormGPS**: Keep in UI layer temporarily
2. **Move to TrackService**: Clean, but needs more parameters
3. **Create GuidanceContext**: Encapsulate all guidance state

**Recommended**: Option 3 - GuidanceContext
```csharp
public class GuidanceContext
{
    public Track CurrentTrack { get; set; }
    public double ToolWidth { get; set; }
    public double ToolOverlap { get; set; }
    public double ToolOffset { get; set; }
    public bool IsToolOffsetRight { get; set; }
    public vec3 PivotPosition { get; set; }
    public vec3 SteerPosition { get; set; }
    public double Heading { get; set; }
    public double Speed { get; set; }

    public double CalculateTrackOffset()
    {
        // Extract logic from CABLine/CABCurve
    }
}
```

**Deliverable**: GuidanceContext class with offset calculation

### Phase 6.7: Handle Special Cases
**Status**: Pending
**Goal**: Ensure YouTurn, Contour, etc. still work

**Special Cases**:
1. **YouTurn active**: Don't use regular guidance
   ```csharp
   if (yt.isYouTurnTriggered && yt.DistanceFromYouTurnLine())
   {
       // Use YouTurn guidance instead
       // (Already handled in CABLine/CABCurve - need to preserve)
   }
   ```

2. **Contour mode**: Different guidance mode
3. **Recorded path driving**: Uses different guidance

**Deliverable**: Special case handling preserved in new system

### Phase 6.8: Test Build and Smoke Test
**Status**: Pending
**Goal**: Verify code compiles and basic functionality works

**Test Checklist**:
- [ ] Code compiles without errors
- [ ] Can create AB line
- [ ] Can create Curve
- [ ] AB line guidance works (drive vehicle, see steering)
- [ ] Curve guidance works
- [ ] Can switch between Stanley and Pure Pursuit
- [ ] YouTurn still works
- [ ] Field save/load works (OLD system still there)

**Deliverable**: Working build with new guidance active

### Phase 6.9: Remove OLD Guidance Code
**Status**: Pending
**Goal**: Delete CGuidance.cs and guidance logic from CABLine/CABCurve

**Files to Modify**:
1. **Delete**: `CGuidance.cs` (413 lines)
2. **CABLine.cs**: Remove guidance logic, keep data/drawing
3. **CABCurve.cs**: Remove guidance logic, keep data/drawing

**What to Keep in CABLine/CABCurve** (temporary):
- Track data (ptA, ptB, heading, curList, etc.)
- Drawing code (DrawABLine, DrawCurve)
- Field save/load logic (if any)

**What to Remove**:
- GetCurrentABLine() guidance logic
- GetCurrentCurveLine() guidance logic
- Pure Pursuit calculations
- Distance calculations (now in TrackService)

**Deliverable**: Leaner CABLine/CABCurve without guidance logic

---

## 🗺️ Phase 7: Full Track System Migration (Future)

**Note**: This is OPTIONAL and can be done later. For now, we keep CTrack/CTrk for data.

### Phase 7.1: Migrate Field Save/Load
- Convert field file format to use new Track model
- Or: keep OLD format, convert on load/save

### Phase 7.2: Migrate Track UI
- Track list display
- Track selection
- Track creation dialogs

### Phase 7.3: Delete CABLine.cs and CABCurve.cs
- Remove entire files
- Remove all references

### Phase 7.4: Delete CTrack.cs and CTrk.cs
- Remove entire files
- Replace with TrackService throughout

---

## 📝 Property Mapping: CTrk → Track

| CTrk Property | Track Property | Notes |
|---------------|----------------|-------|
| `name` | `Name` | Direct copy |
| `mode` | `Mode` | Direct copy (same enum) |
| `curList` | `CurvePts` | Direct copy (List<vec3>) |
| `ptA` | `PtA` | AB line start (GeoCoord) |
| `ptB` | `PtB` | AB line end (GeoCoord) |
| `heading` | `Heading` | AB line heading |
| `nudgeDistance` | `NudgeDistance` | Direct copy |
| `isVisible` | `IsVisible` | Direct copy |
| (none) | `Id` | NEW - generate Guid |
| (various) | Track has more properties for future |

---

## ⚠️ Risk Mitigation

### Risk 1: Breaking Field Save/Load
**Impact**: High - users can't load existing fields
**Mitigation**: Keep CTrack system intact, sync to new TrackService only for guidance
**Timeline**: Migrate field save/load in Phase 7

### Risk 2: Complex Offset Calculation
**Impact**: Medium - guidance might be inaccurate
**Mitigation**: Extract carefully, test thoroughly, compare with old behavior
**Timeline**: Phase 6.6

### Risk 3: Special Cases (YouTurn, Contour)
**Impact**: Medium - features might break
**Mitigation**: Preserve special case logic, test each mode
**Timeline**: Phase 6.7

### Risk 4: Performance Regression
**Impact**: Low - we have <1ms targets
**Mitigation**: Performance tests already exist, monitor in testing
**Timeline**: Ongoing

---

## 📊 Success Criteria

### Phase 6 Complete When:
1. ✅ Services initialized in FormGPS
2. ✅ UpdateGuidance() method created
3. ✅ TrackMigrationAdapter working
4. ✅ Position.designer.cs uses new guidance calls
5. ✅ Offset calculation extracted and working
6. ✅ Special cases preserved
7. ✅ Build succeeds
8. ✅ AB line guidance works
9. ✅ Curve guidance works
10. ✅ CGuidance.cs deleted
11. ✅ Guidance logic removed from CABLine/CABCurve
12. ✅ Field save/load still works (via OLD CTrack system)

### Phase 7 Complete When:
1. Field save/load uses new Track model
2. CABLine.cs deleted
3. CABCurve.cs deleted
4. CTrack.cs deleted
5. All references removed
6. Full regression test passes

---

## 🎯 Current Priority: Phase 6.3

**Next Steps**:
1. Analyze CTrack/CTrk code
2. Document property mapping
3. Find all CABLine/CABCurve usage sites
4. Create detailed migration checklist

---

*Last Updated: 2025-01-19*
*Status: Phase 6.3 - Analysis Phase*
