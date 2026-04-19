# CereaBridge

CereaBridge is a small bridge process for testing AgOpenGPS with Cerea hardware:

- Phidgets DC motor controller as steering motor output
- Phidgets encoder as wheel angle feedback
- Tinkerforge IMU Brick 2.0 as heading/roll source

## Current state

This is a first-pass test bridge intended for same-day bench and field testing.
It listens for AgIO/AgOpenGPS steer packets and sends steer telemetry back as a virtual steer module.

## Defaults

- listens on UDP 18888 and 8888
- sends module data back to AgIO on 127.0.0.1:9999
- uses fallback configuration values from `BridgeConfig.cs`
- optional IMU Brick is enabled only when `ImuUid` is set in `App.config`

## Before running

1. Install Phidgets drivers / control panel.
2. Install Tinkerforge Brick Daemon.
3. Put your IMU Brick 2.0 UID into `App.config`.
4. Verify AgIO UDP is enabled.
5. Verify counts-per-degree and WAS offset in AgOpenGPS settings.

## Notes

- If AgIO does not reach the bridge through subnet broadcast, add a localhost mirror path in AgIO or test on the active UDP subnet.
- This bridge is intentionally simple: it emulates the steer module first and carries heading/roll in PGN 253.
