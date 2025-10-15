# AgOpenGPS Integration Tests

This project contains integration tests for AgOpenGPS that enable automated testing of field operations, autosteer, and U-turn functionality.

## Status

The testing infrastructure has been implemented with the following components:

### Completed
- Controller interfaces in `AgOpenGPS.Core/Testing/`
- Controller implementations in `GPS/Testing/Controllers/`
  - `FieldController` - Create and manage fields, boundaries, tracks
  - `SimulatorController` - Control simulator position, heading, speed
  - `AutosteerController` - Enable/disable autosteer and monitor state
  - `UTurnController` - Control U-turn behavior
  - `PathLogger` - Log path data during simulation
- Test project structure
- Example test cases (currently ignored pending headless mode)

### Pending
- **Headless mode support in FormGPS** - Required to run tests without UI
- TestOrchestrator full implementation - Connect controllers to FormGPS
- Test fixtures and helpers
- Additional test scenarios

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

Once headless mode is implemented:

```bash
# Run all integration tests
dotnet test AgOpenGPS.IntegrationTests/AgOpenGPS.IntegrationTests.csproj

# Run specific test
dotnet test --filter "FullyQualifiedName~UTurnIntegrationTests.Test_UTurn_CompletesSuccessfully"
```

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

## Next Steps

1. Implement headless mode in FormGPS
   - Skip UI initialization when headless flag is set
   - Extract core initialization to separate method
   - Ensure all components work without UI

2. Complete TestOrchestrator implementation
   - Wire up controllers to FormGPS instance
   - Implement StepSimulation method
   - Handle initialization and shutdown

3. Create test helpers
   - GeometryHelpers for coordinate calculations
   - PathVerifier for asserting path quality
   - TestDataBuilder for reusable test scenarios

4. Write comprehensive test suite
   - Field creation and boundary tests
   - Simulator movement tests
   - Autosteer accuracy tests
   - U-turn execution tests
   - Edge case scenarios

## Documentation

See `AgOpenGPS-Testing-Plan.local.md` for the complete testing plan and design decisions.
