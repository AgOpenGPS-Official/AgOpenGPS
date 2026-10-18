# Settings System (NewSettings Branch)

## Overview

Settings are split into three separate files for better organization:

| Settings | File | Purpose |
|----------|------|---------|
| Vehicle | `GPS/Properties/VehicleSettings.cs` | Steer settings, IMU, GPS config, brand presets |
| Tool | `GPS/Properties/ToolSettings.cs` | Sections, tramlines, relays, Arduino machines |
| Environment | `GPS/Properties/Settings.cs` | Display, sounds, window positions, colors |

## Save Pattern

```csharp
// Vehicle settings
VehicleSettings.Default.Save(RegistrySettings.vehicleFileName);

// Tool settings
ToolSettings.Default.Save(RegistrySettings.toolFileName);

// Environment settings
Settings.Default.Save(); // uses environmentFileName internally
```

## Registry Storage

All settings are stored in Windows Registry (not .config files).

## File Locations

| Type | Directory | File Pattern |
|------|-----------|--------------|
| Vehicle | `%AppData%\AgOpenGPS\VehicleProfiles\` | `{ProfileName}.xml` |
| Tool | `%AppData%\AgOpenGPS\ToolProfiles\` | `{ProfileName}.xml` |
| Environment | `%AppData%\AgOpenGPS\Environment\` | `environment.xml` or `DefaultEnvironment.xml` |

## Migration

`Classes/CSettingsMigration.cs` handles migration from the old single-file format.

---

# Vehicle Settings (VehicleSettings.cs)

## Vehicle Dimensions

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setVehicle_wheelbase` | double | 3.3 | Distance from front axle to rear axle (meters) |
| `setVehicle_antennaHeight` | double | 3.0 | GPS antenna height above ground (meters) |
| `setVehicle_antennaPivot` | double | 0.1 | Distance from antenna to pivot axle (meters) |
| `setVehicle_antennaOffset` | double | 0.0 | Lateral offset of antenna (meters) |
| `setVehicle_maxSteerAngle` | double | 30.0 | Maximum steering angle (degrees) |
| `setVehicle_maxAngularVelocity` | double | 0.64 | Maximum turn rate (degrees/sec) |
| `setVehicle_trackWidth` | double | 1.9 | Width of vehicle tracks (meters) |

## AutoSteer Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setAS_Kp` | byte | 50 | Proportional gain (0-100) |
| `setAS_countsPerDegree` | byte | 110 | WAS sensor counts per degree |
| `setAS_minSteerPWM` | byte | 25 | Absolute minimum PWM |
| `setAS_highSteerPWM` | byte | 180 | Maximum PWM output |
| `setAS_lowSteerPWM` | byte | 30 | Minimum PWM when driving |
| `setAS_wasOffset` | int | 3 | WAS sensor zero offset |
| `setAS_sideHillComp` | double | 0.0 | Side-hill compensation factor |
| `setAS_ackerman` | byte | 100 | Ackerman compensation (%) |
| `setAS_ModeXTE` | double | 0.1 | Cross-track error for mode switch |
| `setAS_ModeTime` | int | 1 | Time before mode hold activates |
| `setAS_functionSpeedLimit` | double | 12.0 | Speed limit for functions (km/h) |
| `setAS_maxSteerSpeed` | double | 15.0 | Max steering speed (km/h) |
| `setAS_minSteerSpeed` | double | 0.0 | Min steering speed (km/h) |
| `setAS_isSteerInReverse` | bool | false | Enable steering in reverse |

## IMU Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setIMU_rollZero` | double | 0.0 | Roll angle zero offset (degrees) |
| `setIMU_rollFilter` | double | 0.0 | Roll filter value (0-1) |
| `setIMU_invertRoll` | bool | false | Invert roll sensor |
| `setIMU_isDualAsIMU` | bool | false | Use dual antenna as IMU |
| `setIMU_fusionWeight2` | double | 0.06 | GPS/IMU fusion weight |

## GPS Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setGPS_headingFromWhichSource` | string | "Fix" | Heading source: "Fix", "Dual", "IMU" |
| `setGPS_forwardComp` | double | 0.15 | Forward compensation (meters) |
| `setGPS_reverseComp` | double | 0.3 | Reverse compensation (meters) |
| `setGPS_dualHeadingOffset` | double | 0.0 | Dual antenna heading offset (degrees) |
| `setGPS_dualReverseDetectionDistance` | double | 0.25 | Reverse detection distance (meters) |
| `setGPS_minimumStepLimit` | double | 0.05 | Minimum position step (meters) |

## Arduino Steer Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setArdSteer_setting0` | byte | 56 | Steer config byte 0 |
| `setArdSteer_setting1` | byte | 0 | Steer config byte 1 |
| `setArdSteer_maxPulseCounts` | byte | 3 | Maximum pulse counts |
| `setArdMac_isDanfoss` | bool | false | Use Danfoss valve |

## Brand Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setBrand_TBrand` | TractorBrand | AGOpenGPS | Tractor brand preset |
| `setBrand_HBrand` | HarvesterBrand | AgOpenGPS | Harvester brand preset |
| `setBrand_WDBrand` | ArticulatedBrand | AgOpenGPS | Articulated brand preset |

## Vehicle Type

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setVehicle_vehicleType` | int | 0 | 0=Tractor, 1=Harvester, 2=4WD, 3=Articulated |
| `setVehicle_panicStopSpeed` | double | 0.0 | Speed threshold for panic stop |

---

# Tool Settings (ToolSettings.cs)

## Tool Dimensions

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setVehicle_toolWidth` | double | 4.0 | Total tool width (meters) |
| `setVehicle_toolOverlap` | double | 0.0 | Section overlap (meters) |
| `setVehicle_toolOffset` | double | 0.0 | Tool lateral offset (meters) |
| `setVehicle_tankTrailingHitchLength` | double | 3.0 | Tank hitch length (meters) |
| `setVehicle_toolTrailingHitchLength` | double | -2.5 | Tool hitch length (meters) |
| `setVehicle_toolLookAheadOn` | double | 1.0 | Lookahead when section ON (meters) |
| `setVehicle_toolLookAheadOff` | double | 0.5 | Lookahead when section OFF (meters) |
| `setVehicle_hitchLength` | double | -1.0 | Hitch length (meters) |

## Tool Configuration

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setTool_isToolTrailing` | bool | true | Tool is trailing |
| `setTool_isToolRearFixed` | bool | false | Tool is rear fixed |
| `setTool_isToolTBT` | bool | false | Tool is TBT (Tram Between Tool) |
| `setTool_isToolFront` | bool | false | Tool is front mounted |
| `setTool_isSectionsNotZones` | bool | true | Use sections instead of zones |
| `setTool_isSectionOffWhenOut` | bool | true | Turn off sections out of boundary |
| `setTool_isDisplayTramControl` | bool | true | Show tram control |
| `setTool_isTramOuterInverted` | bool | false | Invert outer tramlines |
| `setTool_isDirectionMarkers` | bool | true | Show direction markers |
| `setTool_trailingToolToPivotLength` | double | 0.0 | Distance to pivot (meters) |

## Section Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setVehicle_numSections` | int | 3 | Number of sections (1-16) |
| `setTool_numSectionsMulti` | int | 20 | Number of sections for multi (1-64) |
| `setSection_isFast` | bool | true | Fast section control |
| `setTool_defaultSectionWidth` | double | 2.0 | Default section width (meters) |
| `setTool_sectionWidthMulti` | double | 0.5 | Section width for multi (meters) |
| `setTool_zones` | string | "2,10,20,..." | Zone configuration string |

## Section Positions (setSection_position1-16)

Position of each section relative to vehicle center (meters, negative = left, positive = right).

| Property | Type | Default |
|----------|------|---------|
| `setSection_position1` | decimal | -2.0 |
| `setSection_position2` | decimal | -1.0 |
| `setSection_position3` | decimal | 1.0 |
| `setSection_position4-16` | decimal | 0.0 |

## Section Colors (setColor_sec01-16)

RGB color for each section display.

| Property | Type | Default |
|----------|------|---------|
| `setColor_sec01` | Color | RGB(249, 22, 10) - Red |
| `setColor_sec02` | Color | RGB(68, 84, 254) - Blue |
| `setColor_sec03` | Color | RGB(8, 243, 8) - Green |
| `setColor_sec04-16` | Color | Various |

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setColor_isMultiColorSections` | bool | false | Use unique colors per section |

## DeadZone Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setAS_deadZoneDistance` | int | 1 | Distance dead zone (cm) |
| `setAS_deadZoneHeading` | int | 10 | Heading dead zone (degrees) |
| `setAS_deadZoneDelay` | int | 5 | Dead zone delay (seconds) |

## Headland Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setHeadland_isSectionControlled` | bool | true | Section control in headland |

## U-Turn Settings (per Tool)

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `set_youTurnExtensionLength` | int | 20 | U-turn extension (meters) |
| `set_youTurnDistanceFromBoundary` | double | 2.0 | Distance from boundary (meters) |
| `set_youSkipWidth` | int | 1 | Skip width (passes) |
| `set_youTurnRadius` | double | 8.1 | Turn radius (meters) |
| `set_uTurnStyle` | int | 0 | U-turn style (0-2) |
| `setAS_uTurnSmoothing` | double | 14.0 | U-turn smoothing |
| `setAS_uTurnCompensation` | double | 1.0 | U-turn compensation |

## Arduino Machine Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setArdMac_setting0` | byte | 0 | Config byte 0 |
| `setArdMac_isHydEnabled` | byte | 0 | Hydraulics enabled |
| `setArdMac_hydRaiseTime` | byte | 3 | Hydraulic raise time (sec) |
| `setArdMac_hydLowerTime` | byte | 4 | Hydraulic lower time (sec) |
| `setArdMac_user1-4` | byte | 1-4 | User defined bytes |

## Relay Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setRelay_pinConfig` | string | "1,2,3,0,..." | 24 relay pin config (comma-separated) |

## Guidance Algorithm Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `purePursuitIntegralGainAB` | double | 0.0 | Pure Pursuit integral gain |
| `stanleyDistanceErrorGain` | double | 1.0 | Stanley distance gain |
| `stanleyHeadingErrorGain` | double | 1.0 | Stanley heading gain |
| `stanleyIntegralGainAB` | double | 0.0 | Stanley integral gain |
| `setVehicle_isStanleyUsed` | bool | false | Use Stanley algorithm |

## Tool Specific Steer Parameters

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setVehicle_goalPointLookAheadHold` | double | 3.0 | Lookahead when holding (meters) |
| `setVehicle_toolOffDelay` | double | 0.0 | Tool off delay (sec) |
| `setVehicle_goalPointLookAheadMult` | double | 1.5 | Lookahead multiplier |
| `setVehicle_goalPointAcquireFactor` | double | 0.9 | Acquire factor |
| `setVehicle_slowSpeedCutoff` | double | 0.5 | Slow speed threshold (km/h) |
| `setVehicle_minCoverage` | double | 100 | Minimum coverage (%) |
| `setVehicle_hydraulicLiftLookAhead` | double | 2.0 | Hyd lift lookahead (meters) |
| `setF_isSteerWorkSwitchEnabled` | bool | false | Steer work switch enabled |

---

# Environment Settings (Settings.cs)

## Window Positions

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setWindow_Location` | Point | (30, 30) | Main window position |
| `setWindow_Size` | Size | (1005, 730) | Main window size |
| `setWindow_Maximized` | bool | false | Start maximized |
| `setWindow_Minimized` | bool | false | Start minimized |
| `setJobMenu_location` | Point | (200, 200) | Job menu position |
| `setJobMenu_size` | Size | (640, 530) | Job menu size |
| `setWindow_steerSettingsLocation` | Point | (40, 40) | Steer settings position |
| `setWindow_buildTracksLocation` | Point | (40, 40) | Build tracks position |
| `setWindow_formNudgeLocation` | Point | (200, 200) | Nudge form position |
| `setWindow_formNudgeSize` | Size | (200, 400) | Nudge form size |
| `setWindow_abDrawSize` | Size | (1022, 742) | AB draw size |
| `setWindow_HeadlineSize` | Size | (1022, 742) | Headline window size |
| `setWindow_HeadAcheSize` | Size | (1022, 742) | Headache window size |
| `setWindow_MapBndSize` | Size | (1022, 742) | Boundary window size |
| `setWindow_BingMapSize` | Size | (965, 700) | Bing map window size |
| `setWindow_BingZoom` | int | 15 | Bing map zoom level |
| `setWindow_QuickABLocation` | Point | (100, 100) | Quick AB position |
| `setWindow_gridSize` | Size | (400, 400) | Grid window size |
| `setWindow_gridLocation` | Point | (20, 20) | Grid window position |
| `setWindow_tramLineSize` | Size | (921, 676) | Tramline window size |

## Display Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setMenu_isMetric` | bool | true | Use metric units |
| `setMenu_isGridOn` | bool | true | Show grid |
| `setMenu_isLightbarOn` | bool | true | Show lightbar |
| `setMenu_isSideGuideLines` | bool | false | Show side guidelines |
| `setMenu_isPureOn` | bool | true | Pure pursuit mode |
| `setMenu_isSimulatorOn` | bool | true | Enable simulator |
| `setMenu_isSpeedoOn` | bool | false | Show speedometer |
| `setMenu_isLightbarNotSteerBar` | bool | false | Lightbar instead of steer bar |
| `setDisplay_isDayMode` | bool | true | Day color mode |
| `setDisplay_isStartFullScreen` | bool | false | Start full screen |
| `setDisplay_isKeyboardOn` | bool | true | Enable keyboard shortcuts |
| `setDisplay_isVehicleImage` | bool | true | Show vehicle image |
| `setDisplay_isTextureOn` | bool | true | Use textures |
| `setDisplay_isBrightnessOn` | bool | false | Show brightness control |
| `setDisplay_isLogElevation` | bool | false | Log elevation data |
| `setDisplay_isSvennArrowOn` | bool | false | Show Svenn arrow |
| `setDisplay_isSectionLinesOn` | bool | true | Show section lines |
| `setDisplay_isLineSmooth` | bool | false | Smooth lines |
| `setDisplay_isTermsAccepted` | bool | false | Terms accepted |
| `setDisplay_isHardwareMessages` | bool | false | Show hardware messages |
| `setDisplay_isAutoStartAgIO` | bool | true | Auto-start AgIO |
| `setDisplay_isAutoOffAgIO` | bool | true | Auto-close AgIO |
| `setWindow_isKioskMode` | bool | false | Kiosk mode |
| `setWindow_isShutdownComputer` | bool | false | Shutdown on exit |
| `setDisplay_isShutdownWhenNoPower` | bool | false | Shutdown on power loss |

## Display Values

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setDisplay_lightbarCmPerPixel` | int | 5 | Lightbar scale |
| `setDisplay_lineWidth` | int | 2 | Line width |
| `setDisplay_brightness` | int | 40 | Screen brightness |
| `setDisplay_brightnessSystem` | int | 40 | System brightness |
| `setDisplay_camSmooth` | int | 50 | Camera smoothing |
| `setDisplay_camZoom` | double | 9.0 | Camera zoom |
| `setDisplay_camPitch` | double | -62.0 | Camera pitch |
| `setDisplay_vehicleOpacity` | int | 100 | Vehicle opacity (%) |

## Display Colors

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setDisplay_colorDayFrame` | Color | RGB(210, 210, 230) | Day frame color |
| `setDisplay_colorNightFrame` | Color | RGB(50, 50, 65) | Night frame color |
| `setDisplay_colorSectionsDay` | Color | RGB(27, 151, 160) | Day section color |
| `setDisplay_colorFieldDay` | Color | RGB(100, 100, 125) | Day field color |
| `setDisplay_colorFieldNight` | Color | RGB(60, 60, 60) | Night field color |
| `setDisplay_colorTextNight` | Color | RGB(230, 230, 230) | Night text color |
| `setDisplay_colorTextDay` | Color | RGB(10, 10, 20) | Day text color |
| `setDisplay_colorVehicle` | Color | White | Vehicle color |
| `setDisplay_customColors` | string | "-62208,..." | Custom color list |
| `setDisplay_customSectionColors` | string | "-62208,..." | Section color list |
| `setDisplay_buttonOrder` | string | "0,1,2,..." | Button order |

## Sound Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setSound_isUturnOn` | bool | true | U-turn sounds |
| `setSound_isHydLiftOn` | bool | true | Hyd lift sounds |
| `setSound_isAutoSteerOn` | bool | true | Autosteer sounds |
| `setSound_isSectionsOn` | bool | true | Section sounds |

## Keyboard Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setKey_hotkeys` | string | "ACFGMNPTYVW12345678" | Hotkey assignments |

## Field Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setF_CurrentDir` | string | "" | Current field directory |
| `setF_isWorkSwitchEnabled` | bool | false | Work switch enabled |
| `setF_isWorkSwitchManualSections` | bool | false | Work switch = manual sections |
| `setF_isWorkSwitchActiveLow` | bool | true | Work switch active low |
| `setF_isSteerWorkSwitchManualSections` | bool | false | Steer work switch = manual |
| `setF_minHeadingStepDistance` | double | 0.5 | Min heading step (meters) |
| `setF_UserTotalArea` | double | 0.0 | User total area (hectares) |
| `setF_isRemoteWorkSystemOn` | bool | false | Remote work system |

## Global Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setFeatures` | CFeatureSettings | - | Feature flags |
| `setIMU_isReverseOn` | bool | true | Reverse mode uses IMU |
| `setAutoSwitchDualFixOn` | bool | false | Auto-switch dual/fix |
| `setAutoSwitchDualFixSpeed` | double | 2.0 | Switch speed threshold (km/h) |
| `setBnd_isDrawPivot` | bool | true | Draw pivot point |
| `isHeadlandDistanceOn` | bool | false | Show headland distance |
| `bndToolSpacing` | int | 1 | Boundary tool spacing |
| `bndToolSmooth` | int | 1 | Boundary smoothing |

## AutoSteer Settings (Environment)

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setAS_snapDistance` | double | 20.0 | Snap distance (cm) |
| `setAS_snapDistanceRef` | double | 5.0 | Ref snap distance (cm) |
| `setAS_isAutoSteerAutoOn` | bool | false | AutoSteer auto mode |
| `setAS_guidanceLookAheadTime` | double | 1.5 | Lookahead time (sec) |
| `setAS_isConstantContourOn` | bool | false | Constant contour |

## GPS Settings (Environment)

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setGPS_SimLatitude` | double | 53.4360564 | Simulator latitude |
| `setGPS_SimLongitude` | double | -111.160047 | Simulator longitude |
| `setGPS_isRTK` | bool | false | RTK mode |
| `setGPS_isRTK_KillAutoSteer` | bool | false | Kill autosteer on RTK loss |
| `setGPS_ageAlarm` | int | 20 | GPS age alarm |
| `setGPS_jumpFixAlarmDistance` | int | 0 | Jump fix alarm distance |

## Guide Lines

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setAS_numGuideLines` | int | 10 | Number of guide lines |

The U-turn settings moved to the tool profile. Their old entries are still read from the environment file, but no longer written: a tool profile saved before the move takes its U-turn values from there on load.

## Tram Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `setTram_tramWidth` | double | 24.0 | Tram width (meters) |
| `setTram_passes` | int | 1 | Number of passes |
| `setTram_alpha` | double | 0.8 | Tram transparency |

## AgShare Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `AgShareServer` | string | "https://agshare.agopengps.com" | AgShare server URL |
| `AgShareApiKey` | string | "" | API key |
| `PublicField` | bool | false | Public field sharing |
| `AgShareEnabled` | bool | false | AgShare enabled |
| `AgShareUploadActive` | bool | false | Upload active |

## UDP Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `SetGPS_udpWatchMsec` | int | 50 | UDP watch interval (ms) |

---

# Feature Settings (CFeatureSettings.cs)

Feature flags control which modules are enabled in the application.

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `isHeadlandOn` | bool | true | Headland detection |
| `isTramOn` | bool | false | Tramlines |
| `isBoundaryOn` | bool | true | Boundaries |
| `isBndContourOn` | bool | false | Boundary contours |
| `isRecPathOn` | bool | false | Recorded paths |
| `isABSmoothOn` | bool | false | Smooth AB lines |
| `isHideContourOn` | bool | false | Hide contour lines |
| `isWebCamOn` | bool | false | Webcam support |
| `isOffsetFixOn` | bool | false | Offset fix |
| `isAgIOOn` | bool | true | AgIO integration |
| `isContourOn` | bool | true | Contour guidance |
| `isYouTurnOn` | bool | true | U-turns |
| `isSteerModeOn` | bool | true | Steering modes |
| `isManualSectionOn` | bool | true | Manual sections |
| `isAutoSectionOn` | bool | true | Auto sections |
| `isCycleLinesOn` | bool | true | Cycle lines |
| `isABLineOn` | bool | true | AB lines |
| `isCurveOn` | bool | true | Curved lines |
| `isAutoSteerOn` | bool | true | AutoSteer |
| `isUTurnOn` | bool | true | U-turns (duplicate) |
| `isLateralOn` | bool | true | Lateral guidance |

## Migration Examples

### Old Combined Profile → New Split Profiles

```csharp
// Old format (single file)
Vehicles/MyTractor.xml

// New format (split)
VehicleProfiles/MyTractor.xml (vehicle settings)
ToolProfiles/MyTool.xml (tool settings)
Environment/environment.xml (environment settings)
```

### Migration Code Pattern

```csharp
// Load old profile
SettingsLegacy oldSettings = new SettingsLegacy();
XmlSettingsHandler.LoadXMLFile(oldPath, oldSettings);

// Create new profiles
VehicleSettings vehicle = new VehicleSettings();
ToolSettings tool = new ToolSettings();
Settings environment = new Settings();

// Copy properties
CSettingsMigration.CopyVehicleProperties(oldSettings, vehicle);
CSettingsMigration.CopyToolProperties(oldSettings, tool);
CSettingsMigration.CopyEnvironmentProperties(oldSettings, environment);

// Save new profiles
vehicle.Save(newProfileName);
tool.Save(newToolName);
environment.Save();
```

---

## Related Files

- [Architecture](architecture.md) - System architecture and dataflow
- [Classes](classes.md) - Core class documentation
- [PGN Protocol](pgn-protocol.md) - Communication protocol
