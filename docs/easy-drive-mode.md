# Easy Drive Mode

## Concept

A quick-start mode that lets the user drive with GPS guidance without loading a field.
The user sets a working width and pivot distance, then creates AB lines or Curves.
Everything stays in memory - nothing is saved to disk.

## Requirements

### Entry
- New button in FormJob (extra row in TableLayoutPanel grid)
- Button is **disabled** when a field is already open (`isJobStarted`)
- Opens FormEasyDrive for configuration

### FormEasyDrive (new form, touch-friendly)
- **NudlessNumericUpDown** for working width (respects metric/imperial)
- **NudlessNumericUpDown** for pivot distance (default 100cm)
- Large touch-friendly Start and Cancel buttons
- Configures a Rigid Tool with 1 section, in-memory only

### Active Mode Behavior
- Global flag: `public bool isEasyDriveMode` on FormGPS
- Field label shows "Easy Drive" (or "Temporary")
- User can create AB lines/Curves via the guidance flowout panel (flp1)
- Multiple lines/curves can be created in the same session
- Flowout panel is limited: no ABDraw (boundary-dependent features hidden)
- FormQuickAB opens normally, user chooses when

### Disabled Features (gated by `isEasyDriveMode`)
- Config screen (blocked)
- Load Vehicle/Tool profiles (blocked)
- Headland, Boundary, Tram tools (blocked)
- All other FormJob buttons disabled when Easy Drive is active
- Steer setting changes are NOT saved

### Saves Blocked
- `FileSaveEverythingBeforeClosingField` skips all file saves
- `FormQuickAB.btnAdd` skips `FileSaveTracks()`
- `Settings.Default.setF_CurrentDir` not updated
- No field directory created on disk

### Exit
- Only via FormJob: click the same button (or Close)
- Confirmation dialog via `FormDialog.ShowQuestion`
- Tool settings restored from `ToolSettings.Default` via `SectionSetPosition()` + `SectionCalcWidths()`
- `isEasyDriveMode = false`, `JobClose()`

## Implementation

### New Files

| File | Purpose |
|------|---------|
| `Forms/Field/FormEasyDrive.cs` | Configuration form (width + pivot distance) |
| `Forms/Field/FormEasyDrive.Designer.cs` | Designer layout |

### Modified Files

| File | Change |
|------|--------|
| `Forms/FormGPS.cs` | Add `isEasyDriveMode` and `isEasyDriveRequested` flags |
| `Forms/Field/FormJob.cs` | Add btnEasyDrive, disable logic, exit confirmation |
| `Forms/Field/FormJob.Designer.cs` | Grid 5->6 rows, new button + separator |
| `Forms/Controls.Designer.cs` | Route Easy Drive from btnJobMenu_Click, gate saves, gate features |
| `Forms/Guidance/FormQuickAB.cs` | Gate `FileSaveTracks()` call |
| `Forms/GUI.Designer.cs` | Show "Easy Drive" in lblCurrentField |
| `Forms/Sections.Designer.cs` | Called programmatically for tool setup/restore |

### Step-by-Step Build Order

#### 1. Global flags (FormGPS.cs)
```csharp
public bool isEasyDriveMode = false;
public bool isEasyDriveRequested = false;
```

#### 2. FormEasyDrive (new form)
Touch-friendly form with:
- NudlessNumericUpDown for working width
- NudlessNumericUpDown for pivot distance (default 1.0m)
- Large Start + Cancel buttons

On Start:
```
tool.numOfSections = 1
tool.isSectionsNotZones = true
tool.isToolRearFixed = true (rigid)
tool.hitchLength = [pivot distance input]
section[0].positionLeft = -width/2
section[0].positionRight = width/2
SectionCalcWidths()
currentFieldDirectory = "Temporary"
pn.DefineLocalPlane(AppModel.CurrentLatLon, false)
JobNew()
isEasyDriveMode = true
```

#### 3. FormJob changes
- Designer: RowCount 5 -> 6, add btnEasyDrive + label5 separator
- FormJob_Load: `btnEasyDrive.Enabled = !mf.isJobStarted`
- When `isEasyDriveMode`: all buttons disabled except Close
- Close in Easy Drive: confirm via FormDialog, skip saves, restore tool, reset flag
- btnEasyDrive_Click: set `mf.isEasyDriveRequested = true`, DialogResult.OK, Close()

#### 4. btnJobMenu_Click routing (Controls.Designer.cs)
After FormJob closes, before other DialogResult checks:
```csharp
if (isEasyDriveRequested)
{
    isEasyDriveRequested = false;
    using (var form2 = new FormEasyDrive(this)) { form2.ShowDialog(this); }
}
```

#### 5. FileSaveEverythingBeforeClosingField gate (Controls.Designer.cs)
At top of method:
```csharp
if (isEasyDriveMode)
{
    this.Invoke((MethodInvoker)(() => {
        panelRight.Enabled = false;
        FieldMenuButtonEnableDisable(false);
        JobClose();
        isEasyDriveMode = false;
        Text = "AgOpenGPS";
    }));
    return;
}
```

#### 6. FormQuickAB save gate
In btnAdd_Click: `if (!mf.isEasyDriveMode) mf.FileSaveTracks();`

#### 7. Feature gates
Block with `TimedMessageBox` + return when `isEasyDriveMode`:
- Config screen opening
- Load Vehicle/Tool profiles
- Headland, Boundary, Tram menu items

#### 8. Flowout panel (flp1) adjustment
In btnTrack_Click, when building flp1 visibility:
- Hide ABDraw button (flp1.Controls[1]) when `isEasyDriveMode` (no boundary)
- Keep visible: PlusAB (create track), nudge, off, snap to pivot

#### 9. Label display
In GUI timer: if `isEasyDriveMode`, show "Easy Drive" in lblCurrentField

#### 10. Tool restore on exit
```csharp
SectionSetPosition();   // reload from ToolSettings.Default
SectionCalcWidths();    // recalculate
```

## UI Design Notes

- All controls must be touch-friendly (large buttons, NudlessNumericUpDown with keypad)
- FormEasyDrive follows existing button style: Tahoma 16pt Bold, FlatStyle.Flat, blue borders
- Minimum target size for touch: 75px height per button
- FormJob Easy Drive button should have a distinctive color to stand out
