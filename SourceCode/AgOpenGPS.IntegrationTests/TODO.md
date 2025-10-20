# Integration Tests TODO

## Known Issues

### Overlap Calculation in Headless Mode
**Status:** FIXED
**Priority:** Medium

**Root Cause:**
Section control logic WORKS CORRECTLY in headless mode after extraction.
- ✅ Section control extracted to UpdateSectionControl() - WORKING
- ✅ Coverage is recorded in workedAreaTotal - WORKING
- ✅ actualAreaCovered now works using grid-based tracking - FIXED

Original overlap calculation (OpenGL.Designer.cs lines 1183-1226) reads pixels from OpenGL which doesn't work in headless mode.

**Solution Implemented:**
Created CCoverageTracker class with grid-based coverage tracking:
1. ✅ CCoverageTracker.cs created with 0.25m resolution grid
2. ✅ InitializeGrid() called in JobNew()
3. ✅ MarkCoverage() called from CPatches.AddMappingPoint()
4. ✅ CalculateOverlap() called from UpdateSectionControl()
5. ✅ Tests now show actualAreaCovered and overlapPercent working in headless mode

The grid-based tracker uses the same algorithm as the OpenGL version but operates on a coverage grid instead of reading pixels.

### Visual Test Issues
**Status:** Open
**Priority:** Medium

**Problems in Visual_Test_SectionControl:**
1. **Only 1 section configured:** Test sets `formGPS.tool.numOfSections = 1`
   - Should use multiple sections for realistic testing
2. **Tractor teleportation:** Implement turns on, then tractor teleports to different location
   - Need to investigate why position jumps
   - May be related to AB line setup or fix quality
3. **UI state not correct:** Buttons not visible during visual tests
   - UI may not be fully initialized when test starts
   - May need longer delay or explicit UI initialization

**Next Steps:**
- [ ] Increase numOfSections to 3-5 for more realistic section control testing
- [ ] Debug why tractor position jumps after implement is enabled
- [ ] Fix UI initialization issue for visual tests

## Planned Improvements

### FieldWorkTest Configuration
**Status:** Completed (but has issues - see below)

Updated settings:
- ✅ **Implement Width:** 5m
- ✅ **U-Turn Radius:** 5m
- ✅ **U-Turn Edge Distance:** 2.5m
- ✅ **Track Skipping:** Enabled with `skipMode = SkipMode.Alternative` and `rowSkipsWidth = 1`

**New Issue:** U-turns are not alternating correctly
- Test shows 6 left turns and 3 right turns instead of alternating
- `SkipMode.Alternative` may not be working as expected or may require additional configuration
- Need to investigate how to properly enable alternating U-turn directions

### Test Coverage
- [ ] Add test for implement/section control in isolation
- [ ] Add test for track skipping functionality
- [ ] Add test for U-turn radius configuration
- [ ] Verify coverage calculation works correctly

## Notes
- The Test_UTurnScenario test works correctly and triggers U-turns
- The FieldWorkTest U-turns work when starting at X=-10m but not at the field corner (X=-22m)
- Starting position affects U-turn triggering - needs investigation
