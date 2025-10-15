# TODO: PR 969 - Consolidate duplicate forms into AgLibrary

Based on unresolved feedback from PR #969:

## Completed ✓

### 1. Remove CONSOLIDATED_FORMS_TEST_SUMMARY.md
- [x] Remove CONSOLIDATED_FORMS_TEST_SUMMARY.md file from repository
- [x] Remove orphaned FormtimedMessage.resx from AgIO

### 2. Remove Remaining .resx Files
- [x] Remove remaining *.resx files from AgOpenGPS project
- [x] Remove remaining *.resx files from ModSim project
- No orphaned files found, only the one in AgIO

### 3. TestFormLauncher Structure
- [x] Move Main method from TestFormLauncher/MainForm.cs to Program.cs
- [x] Add Designer.cs file for MainForm
- [x] Make TestFormLauncher/README.md more concise

### 4. Split ConsolidatedFormsTests.cs
- [x] Split into separate test classes for FormTimedMessage
- [x] Split into separate test classes for FormYes

### 5. GeometryExtensionMethodsFormTests.cs
- [x] Removed - tests only checked for compilation errors, not useful

### 6. Bug Fixes
- [x] Fix designer compatibility (string.Empty → "")
- [x] Fix FormYes text centering (removed AutoSize that conflicted with TextAlign)

## Needs Team Discussion

### 7. Solution Platform Configurations
- [x] Reviewed x64 and x86 configurations in AgOpenGPS.sln
- **Finding**: All x64/x86 configurations map to "Any CPU" - they're not actually used
- **Recommendation**: Remove x64/x86 platform configs, keep only Debug|Any CPU and Release|Any CPU
- **Impact**: Would remove 4 of 6 solution configurations (78 project config entries)
- **Action**: Needs team discussion before changing - affects all developers

### 8. Form Color Scheme
- [x] Colors already changed in commit b4e734d9 to AgOpenGPS style
- Current: Orange/peach (255, 192, 128) with red borders
- No further action needed
