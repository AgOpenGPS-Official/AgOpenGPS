# CereaBridge

CereaBridge is a small bridge process for AgOpenGPS with Cerea hardware:

- Phidgets DC motor controller as steering motor output
- Phidgets encoder as wheel angle feedback
- Tinkerforge IMU Brick 2.0 as heading/roll source

## Current state

CereaBridge listens for AgIO/AgOpenGPS steer packets and sends steer telemetry back as a virtual steer module.

## Simple setup

CereaBridge now uses:

- built-in default values
- optional saved profile values from `CereaBridge.profile.ini`

On first run, CereaBridge asks for the basic settings it needs and saves them into:

`CereaBridge.profile.ini`

You can run setup again any time with:

`CereaBridge.exe --setup`

An example profile is included as:

`CereaBridge.profile.example.ini`

Windows helper files are included as:

- `CereaBridge.setup.bat`
- `CereaBridge.run.bat`
- `CereaBridge.publish.bat`

## CI build

The repository includes a GitHub Actions workflow for CereaBridge that runs:

- restore
- build
- publish
- artifact upload

If the workflow passes, you can download the published `CereaBridge-publish` artifact from Actions.

## Defaults

- listens on UDP 18888 and 8888
- sends module data back to AgIO on 127.0.0.1:9999
- starts from built-in defaults and then applies saved profile values if present
- optional IMU Brick is enabled only when `ImuUid` is set

## Before running

1. Install Phidgets drivers / control panel.
2. Install Tinkerforge Brick Daemon.
3. Start CereaBridge once and enter your device settings.
4. Verify AgIO UDP is enabled.
5. Verify counts-per-degree and WAS offset in AgOpenGPS settings.

## What you see on startup

CereaBridge prints:

- the profile path in use
- the active settings summary
- startup warnings for empty or risky values
- device status for motor, encoder and IMU Brick

## Notes

- AgIO has been updated to mirror module packets to localhost so the bridge can run on the same PC.
- This bridge is intentionally simple: it emulates the steer module first and carries heading/roll in PGN 253.
