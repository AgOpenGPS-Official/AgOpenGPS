# Form Launcher - Consolidated Forms Test Application

Test application for FormYes and FormTimedMessage consolidated from GPS/AgIO/ModSim into AgLibrary.

## Consolidated Forms

**FormYes** - Dialog with dynamic width, optional Cancel button
**FormTimedMessage** - Auto-closing notification with timer

## How to Run

Build and run from SourceCode directory:
```powershell
dotnet run --project TestFormLauncher/TestFormLauncher.csproj
```

## Test Scenarios

Five test buttons verify:
1. FormYes (No Cancel) - Basic yes/no dialog
2. FormYes (With Cancel) - Yes/No/Cancel dialog
3. FormYes (Long Message) - Dynamic width calculation
4. FormTimedMessage (3s) - Auto-close notification
5. FormTimedMessage (Long, 5s) - Dynamic width + timer

## Unit Tests

Run automated tests:
```powershell
dotnet test AgLibrary.Tests/AgLibrary.Tests.csproj --filter Category=ConsolidatedForms
```
