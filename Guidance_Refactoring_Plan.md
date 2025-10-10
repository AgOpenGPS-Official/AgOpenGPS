# Guidance System Refactoring Plan

## Doel
Refactor het guidance systeem van AgOpenGPS met focus op:
1. **UI Decoupling** - Business logic scheiden van WinForms/OpenGL UI
2. **Clean Architecture** - CTracks, CGuidance en CYouTurn van AOG_Dev overnemen met verbeteringen
3. **Testbaarheid** - Alles in kleine, testbare stappen
4. **Maintainability** - Duidelijke dependencies, geen circular references

---

## Huidige Situatie

### Bestaande Code (AgOpenGPS)
- ✅ **AgOpenGPS.Core** project - MVVM/Clean architecture basis
- ✅ **AgOpenGPS.Core.Tests** - NUnit test framework opgezet
- ❌ **CABLine.cs** - Oud AB line systeem (moet weg)
- ❌ **CABCurve.cs** - Oud curve systeem (moet weg)
- ❌ **CGuidance.cs** - Oude guidance met dubbele code voor AB/Curve
- ❌ **FormGPS.cs** - Tight coupling tussen UI en business logic

### Source Code (AOG_Dev - als basis)
- ✅ **CTracks.cs** (921 lines) - Unified track system
- ✅ **CGuidance.cs** (450 lines) - Pure Pursuit en Stanley guidance
- ✅ **CYouTurn.cs** (905 lines) - YouTurn logic

---

## Architectuur Doelstelling

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
│   │ Services                          │ │
│   │  - ITrackService                  │ │
│   │  - IGuidanceService               │ │
│   │  - IYouTurnService                │ │
│   └───────────────────────────────────┘ │
│   ┌───────────────────────────────────┐ │
│   │ Geometry (no UI dependencies)     │ │
│   │  - GeoCoord, GeoDelta, GeoPath    │ │
│   │  - OffsetLine, CalculateHeadings  │ │
│   └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

---

## Fase Overzicht

| Fase | Naam | Doel | Complexiteit |
|------|------|------|--------------|
| 1 | Geometry Foundation | Pure geometry functies testbaar maken | 🟢 Low |
| 2 | Track Models | Track data models zonder UI | 🟢 Low |
| 3 | Track Service | Track management business logic | 🟡 Medium |
| 4 | Guidance Service | Stanley/Pure Pursuit zonder UI | 🟡 Medium |
| 5 | YouTurn Service | YouTurn logic zonder UI | 🔴 High |
| 6 | UI Integration | WinForms koppelen aan services | 🟡 Medium |
| 7 | Legacy Removal | CABLine.cs & CABCurve.cs verwijderen | 🟢 Low |

---

## FASE 1: Geometry Foundation (Week 1)

### Doel
Pure geometrie functies in AgOpenGPS.Core zonder dependencies op UI of FormGPS

### Stappen

#### 1.1 Extension Methods naar Core
**Files**: `AgOpenGPS.Core/Extensions/GeometryExtensions.cs`

Van AOG_Dev/CGLM.cs overnemen:
```csharp
// List<vec3> extensions
- OffsetLine(double offset, double step, bool isLoop)
- CalculateHeadings(bool isLoop)
- DrawPolygon(PrimitiveType type) -> NIET! (UI concern)
```

**Strategie**:
- ✅ `OffsetLine` → Naar `GeoPath.Offset(double distance)`
- ✅ `CalculateHeadings` → Naar `GeoPathWithHeading.RecalculateHeadings()`
- ❌ `DrawPolygon` → Blijft in UI layer!

**Test**: `GeometryExtensionsTests.cs`
```csharp
[Test]
public void OffsetLine_StraightLine_CorrectOffset()
{
    var path = new GeoPath(new[] {
        new GeoCoord(0, 0),
        new GeoCoord(100, 0)
    });

    var offset = path.Offset(10);

    Assert.That(offset[0].Northing, Is.EqualTo(10).Within(0.01));
    Assert.That(offset[1].Northing, Is.EqualTo(10).Within(0.01));
}
```

#### 1.2 Geometry Utilities
**Files**: `AgOpenGPS.Core/Geometry/GeometryUtils.cs`

Van AOG_Dev overnemen:
```csharp
// CGuidance.cs helpers
- FindClosestSegment(List<vec3> points, vec2 point, out int A, out int B)
- FindDistanceToSegment(vec2 pt, vec3 p1, vec3 p2, out vec3 point, out double time)
- GetLineIntersection(...) // van glm.cs
```

**Test**: `GeometryUtilsTests.cs`
```csharp
[Test]
public void FindClosestSegment_SimpleCase()
{
    var points = new GeoPath(...);
    var testPoint = new GeoCoord(50, 5);

    var (indexA, indexB) = GeometryUtils.FindClosestSegment(points, testPoint);

    Assert.That(indexA, Is.EqualTo(0));
    Assert.That(indexB, Is.EqualTo(1));
}
```

**Deliverable**:
- ✅ 10+ unit tests voor geometry functies
- ✅ Geen dependencies op FormGPS, OpenGL, of WinForms
- ✅ Build succeeds

---

## FASE 2: Track Models (Week 1-2)

### Doel
Track data models die UI-agnostic zijn

#### 2.1 Track Model
**File**: `AgOpenGPS.Core/Models/Guidance/Track.cs`

Basis op CTrk uit AOG_Dev:
```csharp
public class Track
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TrackMode Mode { get; set; }
    public bool IsVisible { get; set; }

    // Geometry
    public GeoPath CurvePts { get; set; }
    public GeoCoord PtA { get; set; }
    public GeoCoord PtB { get; set; }
    public double Heading { get; set; }

    // State
    public double NudgeDistance { get; set; }

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

**Test**: `TrackTests.cs`
```csharp
[Test]
public void Track_Clone_CreatesIndependentCopy()
{
    var original = new Track { Name = "Track1", NudgeDistance = 5.0 };
    var clone = original.Clone();

    clone.NudgeDistance = 10.0;

    Assert.That(original.NudgeDistance, Is.EqualTo(5.0));
    Assert.That(clone.NudgeDistance, Is.EqualTo(10.0));
}
```

#### 2.2 Track Collection
**File**: `AgOpenGPS.Core/Models/Guidance/TrackCollection.cs`

Basis op CTracks._gArr uit AOG_Dev:
```csharp
public class TrackCollection
{
    private List<Track> _tracks = new List<Track>();

    public IReadOnlyList<Track> Tracks => _tracks.AsReadOnly();
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

**Test**: `TrackCollectionTests.cs`

**Deliverable**:
- ✅ Track models zonder UI dependencies
- ✅ 15+ unit tests
- ✅ Serialization support (voor save/load)

---

## FASE 3: Track Service (Week 2)

### Doel
Business logic voor track management

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
    Track CreateCurveTrack(GeoPath recordedPath);

    // Geometry operations
    GeoPath BuildGuidanceTrack(Track track, double offsetDistance);
    List<GeoPath> BuildGuideLines(Track track, double offsetDistance, int numLines);

    // Distance calculations
    (double distance, bool sameway) GetDistanceFromTrack(Track track, GeoCoord position, double heading);
}
```

#### 3.2 TrackService Implementation
**File**: `AgOpenGPS.Core/Services/TrackService.cs`

Van CTracks.cs overnemen (zonder FormGPS dependencies):
```csharp
public class TrackService : ITrackService
{
    private readonly TrackCollection _tracks;

    public TrackService()
    {
        _tracks = new TrackCollection();
    }

    // Implementatie van BuildGuidanceTrack() - van CTracks line 263
    public GeoPath BuildGuidanceTrack(Track track, double offsetDistance)
    {
        // Pure geometric calculation - NO UI!
        // AOG_Dev CTracks.cs:263-370
    }

    // Implementatie van GetDistanceFromTrack() - van CTracks line 178
    public (double distance, bool sameway) GetDistanceFromTrack(
        Track track, GeoCoord position, double heading)
    {
        // CGuidance.FindClosestSegment gebruiken
        // AOG_Dev CTracks.cs:178-221
    }
}
```

**Test**: `TrackServiceTests.cs`
```csharp
[Test]
public void BuildGuidanceTrack_ABLine_CreatesParallelLine()
{
    var track = new Track {
        Mode = TrackMode.AB,
        CurvePts = new GeoPath(new[] {
            new GeoCoord(0, 0),
            new GeoCoord(100, 0)
        })
    };

    var service = new TrackService();
    var guidance = service.BuildGuidanceTrack(track, offsetDistance: 10);

    Assert.That(guidance[0].Northing, Is.EqualTo(10).Within(0.01));
    Assert.That(guidance.Count, Is.GreaterThan(2));
}
```

**Deliverable**:
- ✅ TrackService met alle track operaties
- ✅ 20+ unit tests
- ✅ Geen FormGPS dependencies

---

## FASE 4: Guidance Service (Week 3)

### Doel
Stanley en Pure Pursuit steering zonder UI

#### 4.1 IGuidanceService Interface
**File**: `AgOpenGPS.Core/Interfaces/Services/IGuidanceService.cs`

```csharp
public interface IGuidanceService
{
    GuidanceResult CalculateGuidance(
        GeoCoord pivotPosition,
        GeoCoord steerPosition,
        double heading,
        double speed,
        GeoPath guidanceTrack,
        bool isReverse,
        bool isUturn);

    double CalculateStanleyGuidance(...);
    double CalculatePurePursuitGuidance(...);
}

public class GuidanceResult
{
    public double SteerAngle { get; set; }
    public double DistanceFromLine { get; set; }
    public double AngularVelocity { get; set; }
    public GeoCoord GoalPoint { get; set; }
    public double HeadingError { get; set; }
    public int SegmentIndexA { get; set; }
    public int SegmentIndexB { get; set; }
}
```

#### 4.2 GuidanceService Implementation
**File**: `AgOpenGPS.Core/Services/GuidanceService.cs`

Van CGuidance.cs overnemen:
```csharp
public class GuidanceService : IGuidanceService
{
    private double _inty; // integral term
    private double _pivotDistanceErrorLast;

    public GuidanceResult CalculateGuidance(
        GeoCoord pivotPosition,
        GeoCoord steerPosition,
        double heading,
        double speed,
        GeoPath guidanceTrack,
        bool isReverse,
        bool isUturn)
    {
        // AOG_Dev CGuidance.cs:43-352
        // Pure berekeningen - NO FormGPS!
    }

    private double CalculateStanley(...)
    {
        // AOG_Dev CGuidance.cs:108-156
    }

    private double CalculatePurePursuit(...)
    {
        // AOG_Dev CGuidance.cs:158-319
    }
}
```

**Test**: `GuidanceServiceTests.cs`
```csharp
[Test]
public void Stanley_OnLine_ZeroSteerAngle()
{
    var service = new GuidanceService();
    var guidanceTrack = CreateStraightLine();

    var result = service.CalculateGuidance(
        pivotPosition: new GeoCoord(50, 0),
        steerPosition: new GeoCoord(52, 0),
        heading: 0,
        speed: 5.0,
        guidanceTrack,
        isReverse: false,
        isUturn: false);

    Assert.That(result.SteerAngle, Is.EqualTo(0).Within(0.1));
    Assert.That(result.DistanceFromLine, Is.EqualTo(0).Within(0.01));
}

[Test]
public void Stanley_OffLine_CorrectSteerAngle()
{
    // Test met 1m offset naar rechts
    var result = service.CalculateGuidance(
        pivotPosition: new GeoCoord(50, 1),  // 1m rechts van lijn
        ...);

    Assert.That(result.SteerAngle, Is.LessThan(0)); // Stuur naar links
    Assert.That(result.DistanceFromLine, Is.EqualTo(1).Within(0.01));
}
```

**Deliverable**:
- ✅ GuidanceService met Stanley en Pure Pursuit
- ✅ 25+ unit tests
- ✅ Geen UI dependencies

---

## FASE 5: YouTurn Service (Week 4)

### Doel
YouTurn logic zonder UI dependencies

**WARNING**: Dit is de meest complexe component (905 lines!)

#### 5.1 IYouTurnService Interface
**File**: `AgOpenGPS.Core/Interfaces/Services/IYouTurnService.cs`

```csharp
public interface IYouTurnService
{
    YouTurnState CreateYouTurn(
        Track currentTrack,
        GeoPath currentGuidance,
        GeoCoord currentPosition,
        List<Boundary> boundaries,
        bool isTurnLeft);

    void BuildManualYouTurn(GeoCoord position, double heading, bool isTurnRight);

    bool IsYouTurnComplete(GeoCoord position, GeoPath youTurnPath);

    void TriggerYouTurn();
    void CompleteYouTurn();
    void ResetYouTurn();
}

public class YouTurnState
{
    public bool IsTriggered { get; set; }
    public bool IsOutOfBounds { get; set; }
    public GeoPath TurnPath { get; set; }
    public GeoPath NextCurvePath { get; set; }
    public int Phase { get; set; }
    public double TotalLength { get; set; }
}
```

#### 5.2 YouTurnService Implementation
**File**: `AgOpenGPS.Core/Services/YouTurnService.cs`

Van CYouTurn.cs overnemen:
```csharp
public class YouTurnService : IYouTurnService
{
    private YouTurnState _state;

    public YouTurnState CreateYouTurn(...)
    {
        // AOG_Dev CYouTurn.cs:96-406
        // Fase-gebaseerde aanpak behouden
        // MAAR: geen DrawYouTurn() hier!
    }

    private GeoPath BuildCurveDubinsYouTurn(...)
    {
        // Geometrische berekeningen
    }

    private GeoPath GetOffsetSemicirclePoints(...)
    {
        // AOG_Dev CYouTurn.cs:410-426
    }
}
```

**Test**: `YouTurnServiceTests.cs`
```csharp
[Test]
public void CreateYouTurn_SimpleTurn_GeneratesPath()
{
    var service = new YouTurnService();
    var track = CreateSimpleABTrack();
    var boundaries = CreateSimpleBoundary();

    var state = service.CreateYouTurn(
        track,
        guidancePath,
        position,
        boundaries,
        isTurnLeft: true);

    Assert.That(state.TurnPath.Count, Is.GreaterThan(5));
    Assert.That(state.IsOutOfBounds, Is.False);
}
```

**Deliverable**:
- ✅ YouTurnService volledig getest
- ✅ 30+ unit tests (complex logic!)
- ✅ Phase-based creation werkt

---

## FASE 6: UI Integration (Week 5)

### Doel
WinForms koppelen aan de nieuwe services

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

        // Dependency injection (simpel)
        _trackService = new TrackService();
        _guidanceService = new GuidanceService();
        _youTurnService = new YouTurnService();
    }
}
```

#### 6.2 UI Event Handlers
**File**: `AgOpenGPS/Forms/Guidance/FormGuidanceAdapter.cs`

```csharp
// Adapter pattern - UI events -> Service calls
public class FormGuidanceAdapter
{
    private readonly FormGPS _form;
    private readonly ITrackService _trackService;

    public void OnNudgeLeft()
    {
        var track = _trackService.GetCurrentTrack();
        _trackService.NudgeTrack(track, -0.1);
        _form.Refresh(); // UI update
    }

    public void OnCreateABLine(GeoCoord ptA, GeoCoord ptB)
    {
        var track = _trackService.CreateABTrack(ptA, ptB, heading, isRefRight);
        _form.RedrawGuidance(); // UI update
    }
}
```

#### 6.3 Rendering Adapter
**File**: `AgOpenGPS/Rendering/GuidanceRenderer.cs`

```csharp
public class GuidanceRenderer
{
    public void DrawTrack(Track track, GeoPath guidanceTrack)
    {
        // OpenGL calls HIER - niet in TrackService!
        GL.LineWidth(...);
        GL.Color3(...);
        guidanceTrack.ToVec3List().DrawPolygon(PrimitiveType.LineStrip);
    }

    public void DrawYouTurn(YouTurnState state)
    {
        // AOG_Dev CYouTurn.cs:824-865
        // OpenGL rendering
    }
}
```

**Test**: Integration tests met UI
```csharp
[Test]
[Apartment(ApartmentState.STA)]
public void FormGPS_CreateTrack_UpdatesUI()
{
    var form = new FormGPS();

    form.CreateABLineFromUI(ptA, ptB);

    Assert.That(form.GetCurrentTrackName(), Is.Not.Empty);
}
```

**Deliverable**:
- ✅ UI volledig werkend met nieuwe services
- ✅ Rendering gescheiden van business logic
- ✅ Event flow: UI → Service → Update UI

---

## FASE 7: Legacy Removal (Week 5)

### Doel
Oude systeem verwijderen

#### 7.1 Remove Old Files
```bash
# Verwijderen
rm SourceCode/GPS/Classes/CABLine.cs        # 660 lines
rm SourceCode/GPS/Classes/CABCurve.cs       # 1539 lines
```

#### 7.2 Update References
Alle verwijzingen naar `mf.ABLine` en `mf.curve` vervangen:
```csharp
// OLD
mf.ABLine.isABValid
mf.curve.isCurveValid

// NEW
_trackService.GetCurrentTrack() != null
```

#### 7.3 Regression Testing
- ✅ Alle guidance modes werken (AB, Curve, Boundary)
- ✅ Nudge werkt in beide richtingen
- ✅ YouTurn compleet functioneel
- ✅ Save/Load van fields werkt
- ✅ Oude field files laden nog steeds

**Deliverable**:
- ✅ CABLine.cs en CABCurve.cs verwijderd
- ✅ Build succeeds met 0 errors
- ✅ Volledige functionaliteit behouden

---

## Test Strategie

### Unit Tests (150+ totaal)
- **Geometry**: 10 tests
- **Track Models**: 15 tests
- **Track Service**: 20 tests
- **Guidance Service**: 25 tests (Stanley + Pure Pursuit)
- **YouTurn Service**: 30 tests (complex!)
- **Others**: 50 tests

### Integration Tests (20+)
- UI event flow
- Service interoperability
- OpenGL rendering (waar nodig)

### Test Coverage Doel
- **Core Services**: 80%+ coverage
- **Models**: 90%+ coverage
- **UI Adapters**: 60%+ coverage (UI is moeilijker te testen)

---

## Success Criteria

### Per Fase
Elke fase is compleet wanneer:
1. ✅ Alle unit tests slagen (green)
2. ✅ Build succeeds zonder errors
3. ✅ Code review gedaan
4. ✅ Documentatie bijgewerkt

### Totaal Project
Project is succesvol wanneer:
1. ✅ Alle 7 fases compleet
2. ✅ CABLine.cs en CABCurve.cs verwijderd
3. ✅ 150+ unit tests, allemaal groen
4. ✅ UI volledig functioneel
5. ✅ Geen circular dependencies
6. ✅ Code coverage > 75%

---

## Risico's & Mitigaties

| Risico | Impact | Mitigatie |
|--------|--------|-----------|
| YouTurn te complex | 🔴 High | Fase 5 in meerdere sub-stappen splitsen |
| OpenGL rendering breaks | 🟡 Medium | Rendering adapter pattern gebruiken |
| Oude field files laden niet | 🟡 Medium | Backward compatibility layer |
| Performance degradation | 🟡 Medium | Profiling na elke fase |
| UI events te tightly coupled | 🟡 Medium | Event aggregator pattern overwegen |

---

## Code Ownership

| Component | Owner | Reviewer |
|-----------|-------|----------|
| Geometry Foundation | TBD | TBD |
| Track Models & Service | TBD | TBD |
| Guidance Service | TBD | TBD |
| YouTurn Service | TBD | TBD |
| UI Integration | TBD | TBD |

---

## Timeline Schatting

| Fase | Geschatte Tijd | Dependencies |
|------|----------------|--------------|
| 1. Geometry Foundation | 3 dagen | None |
| 2. Track Models | 4 dagen | Fase 1 |
| 3. Track Service | 5 dagen | Fase 1, 2 |
| 4. Guidance Service | 5 dagen | Fase 1, 2, 3 |
| 5. YouTurn Service | 7 dagen | Fase 1, 2, 3 |
| 6. UI Integration | 5 dagen | Fase 1-5 |
| 7. Legacy Removal | 2 dagen | Fase 6 |

**Totaal**: ~5-6 weken (1 developer, part-time)

---

## Volgende Stappen

1. ✅ Review dit plan
2. ⏩ Start met Fase 1.1: Geometry Extensions
3. ⏩ Setup CI/CD voor automated tests
4. ⏩ Create GitHub project board met tasks

---

## Referenties

- **Source**: `C:\Users\hp\Documents\GitHub\AOG_Dev\SourceCode\AOG\Classes\`
  - CTracks.cs (921 lines)
  - CGuidance.cs (450 lines)
  - CYouTurn.cs (905 lines)

- **Target**: `C:\Users\hp\Documents\GitHub\AgOpenGPS\SourceCode\AgOpenGPS.Core\`

- **Tests**: `C:\Users\hp\Documents\GitHub\AgOpenGPS\SourceCode\AgOpenGPS.Core.Tests\`
