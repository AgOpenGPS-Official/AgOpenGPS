# AgOpenGPS Integration Tests

This project contains integration tests for AgOpenGPS that enable automated testing of field operations, autosteer, and U-turn functionality.

## Quick Start - Field Work Test with Visualization

**Windows (easiest):**
```cmd
run-fieldwork-test.bat
```

or

```powershell
.\run-fieldwork-test.ps1
```

This will run the complete field work test and automatically open the visualization.

## Status

### Completed
- ✅ **Headless mode support in FormGPS** - Tests run without UI
- ✅ **TestOrchestrator** - Fully implemented and operational
- ✅ Controller interfaces in `AgOpenGPS.Core/Testing/`
- ✅ Controller implementations in `GPS/Testing/Controllers/`
  - `FieldController` - Create and manage fields, boundaries, tracks
  - `SimulatorController` - Control simulator position, heading, speed
  - `AutosteerController` - Enable/disable autosteer and monitor state
  - `UTurnController` - Control U-turn behavior
  - `PathLogger` - Log path data and export to JSON
- ✅ Working test cases with path visualization
- ✅ Automated test runner scripts

### Known Issues
- ⚠️ U-turn not triggering in field work test (under investigation)
- ⚠️ Coverage data shows 0% (section mapping in headless mode)

## Architecture

```
TestOrchestrator
├── FieldController (IFieldController)
├── SimulatorController (ISimulatorController)
├── AutosteerController (IAutosteerController)
├── UTurnController (IUTurnController)
└── PathLogger (IPathLogger)
     └── FormGPS (Headless Mode)
```

## Running Tests

### Run All Tests

```bash
dotnet test
```

### Run Specific Tests

```bash
# Field work test (complete field operation)
dotnet test --filter "Test_CompleteFieldWork_FromEdgeToEdge_WithUTurns"

# U-turn scenario test
dotnet test --filter "Test_UTurnScenario"

# Basic workflow test
dotnet test --filter "Test_BasicFieldWorkflow_CreateFieldAndLogPath"

# Manual steering test
dotnet test --filter "Test_ManualSteering"
```

### Quick Test Run (Skip Build)

```bash
# Skip rebuilding for faster test runs
dotnet test --no-build --filter "Test_CompleteFieldWork"

# Skip restore and build (fastest)
dotnet test --no-restore --no-build --filter "Test_CompleteFieldWork"
```

## Test Output and Visualization

### Automated Visualization

The easiest way to run tests and visualize results:

```cmd
run-fieldwork-test.bat
```

This script:
1. Runs the `Test_CompleteFieldWork_FromEdgeToEdge_WithUTurns` test
2. Finds the most recent JSON output
3. Automatically opens it in Python visualization tool

### Manual Visualization

Test outputs are saved to:
```
bin/Debug/net48/TestOutput/
```

Output files:
- `FieldWork_*.json` - Complete field work test results
- `UTurn_*.json` - U-turn scenario results
- Other test-specific JSON files

To manually visualize:
```bash
python ../../Tools/visualize_path.py "bin/Debug/net48/TestOutput/FieldWork_YYYYMMDD_HHMMSS.json"
```

### What's in the JSON

The exported JSON contains:
- **Tractor path** - Full state at each timestep (position, heading, speed, XTE, etc.)
- **Field boundary** - Outer boundary polygon
- **Turn lines** - Headland turn trigger lines
- **AB reference line** - Original track where user drove
- **Current AB line** - Offset line for implement edge tracking
- **Turn pattern** - U-turn waypoints (ytList)
- **Metadata** - Test parameters and configuration
- **Debug data** - Complete state for troubleshooting

### Visualization Features

The Python visualizer shows:
- Field boundary outline
- Tractor path (colored by autosteer on/off)
- AB lines (reference and current)
- Turn pattern visualization
- Cross-track error (XTE) over time graph
- Speed profile graph

## What the Field Work Test Does

`Test_CompleteFieldWork_FromEdgeToEdge_WithUTurns`:

1. Creates a test field (30m x 100m)
2. Adds rectangular boundary
3. Creates AB track line on left edge
4. Positions tractor at start
5. Enables autosteer
6. Enables U-turn (5m trigger distance)
7. Turns on all implement sections
8. Runs simulation until tractor crosses boundary
9. Tracks:
   - Field coverage percentage
   - Overlap percentage
   - U-turn count and directions
   - Cross-track error
10. Exports detailed JSON with full path data

## Writing Tests

Example test structure:

```csharp
[Test]
public void MyTest()
{
    // Setup orchestrator
    var orchestrator = new TestOrchestrator();
    orchestrator.Initialize(headless: true);

    // Configure field
    var field = orchestrator.FieldController;
    field.CreateNewField("MyField", 39.0, -94.0);

    // Configure simulator
    var sim = orchestrator.SimulatorController;
    sim.Enable();
    sim.SetPosition(39.0, -94.0);

    // Run simulation
    for (int i = 0; i < 100; i++)
    {
        orchestrator.StepSimulation(0.1);
    }

    // Assert results
    var state = sim.GetState();
    Assert.That(state.SpeedKph, Is.GreaterThan(0));

    // Cleanup
    orchestrator.Shutdown();
}
```

## Requirements

- .NET Framework 4.8 or later
- Python 3.x with matplotlib and numpy (for visualization)
- PowerShell (for automated test runner on Windows)

### Install Python Dependencies

```bash
pip install matplotlib numpy
```

## Troubleshooting

### U-turn Not Triggering

Currently a known issue under investigation. The U-turn system may require additional configuration or turn line setup in headless mode.

### No Coverage Data (0%)

Section mapping appears not to be working in headless mode. Under investigation.

### Python Visualization Doesn't Open

Verify Python is installed:
```bash
python --version
```

Ensure dependencies are installed:
```bash
pip install matplotlib numpy
```

### PowerShell Script Won't Run

If you get an execution policy error:
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

## Documentation

- See `AgOpenGPS-Testing-Plan.local.md` for the complete testing plan
- See individual test files for specific test documentation
