# Integration Tests TODO

## Known Issues

### Section Control Not Working in Headless Mode
**Status:** ROOT CAUSE IDENTIFIED
**Priority:** High

**Root Cause:**
Section control logic is embedded inside the `oglBack_Paint` event handler in `OpenGL.Designer.cs` (line 1025+).
In headless mode, Paint events don't fire, so section control code never executes. This means:
- `sectionOnRequest` never gets set to `true`
- `isMappingOn` stays `false`
- `TurnMappingOn()` is never called
- Coverage is never recorded (stays at 0 m²)

**Visual Test Confirmation:**
- ✅ Implement DOES turn on in Visual_Test_SectionControl (Paint events fire in visual mode)
- ❌ Other issues found: only 1 section, implement turns on then tractor teleports to other location

**Solution:**
Extract section control logic from Paint handler into a separate method (e.g., `UpdateSectionControl()`) that can be called from:
1. The Paint handler (for normal UI operation)
2. The simulation step (for headless testing)

**Implementation Task:**
- [ ] Extract section control logic (lines ~1025-1162 in OpenGL.Designer.cs) into new method
- [ ] Call new method from both `oglBack_Paint` and from a simulation update hook
- [ ] Test that headless mode now records coverage properly

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
