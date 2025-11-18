#!/usr/bin/env python3
"""
Analyze section control state from test JSON output
Shows why sections are not recording coverage
"""

import json
import sys
from pathlib import Path

def analyze_section_control(json_path):
    """Analyze section control state over time"""

    with open(json_path, 'r') as f:
        data = json.load(f)

    path = data['tractorPath']

    print(f"\n{'='*80}")
    print(f"Section Control Analysis: {Path(json_path).name}")
    print(f"{'='*80}\n")

    print(f"Total path points: {len(path)}\n")

    # Check first few points to see initial state
    print("=" * 80)
    print("FIRST 5 POINTS - Initial State:")
    print("=" * 80)
    for i in range(min(5, len(path))):
        pt = path[i]
        print(f"\n[{pt['t']:5.1f}s] N={pt['n']:6.1f}m, Speed={pt['speed']:4.1f} km/h")
        print(f"  jobStarted={pt['jobStarted']}, autoBtnState={pt['autoBtnState']}, patchCounter={pt['patchCounter']}")
        print(f"  sec0_btnState={pt['sec0_btnState']}, sec0_on={pt['sec0_on']}, sec0_mapping={pt['sec0_mapping']}")
        print(f"  sec0_onReq={pt['sec0_onReq']}, sec0_offReq={pt['sec0_offReq']}")
        print(f"  avgSpeed={pt['avgSpeed']:5.2f}, slowCutoff={pt['slowCutoff']:5.2f}, sec0_speedPx={pt['sec0_speedPx']:6.2f}")

    # Sample middle points
    mid_idx = len(path) // 2
    print("\n" + "=" * 80)
    print(f"MIDDLE 3 POINTS (around {path[mid_idx]['t']:.1f}s):")
    print("=" * 80)
    for i in range(mid_idx - 1, min(mid_idx + 2, len(path))):
        pt = path[i]
        print(f"\n[{pt['t']:5.1f}s] N={pt['n']:6.1f}m, Speed={pt['speed']:4.1f} km/h")
        print(f"  jobStarted={pt['jobStarted']}, autoBtnState={pt['autoBtnState']}, patchCounter={pt['patchCounter']}")
        print(f"  sec0_btnState={pt['sec0_btnState']}, sec0_on={pt['sec0_on']}, sec0_mapping={pt['sec0_mapping']}")
        print(f"  sec0_onReq={pt['sec0_onReq']}, sec0_offReq={pt['sec0_offReq']}")
        print(f"  avgSpeed={pt['avgSpeed']:5.2f}, slowCutoff={pt['slowCutoff']:5.2f}, sec0_speedPx={pt['sec0_speedPx']:6.2f}")

    # Sample last points
    print("\n" + "=" * 80)
    print("LAST 3 POINTS:")
    print("=" * 80)
    for i in range(max(0, len(path) - 3), len(path)):
        pt = path[i]
        print(f"\n[{pt['t']:5.1f}s] N={pt['n']:6.1f}m, Speed={pt['speed']:4.1f} km/h")
        print(f"  jobStarted={pt['jobStarted']}, autoBtnState={pt['autoBtnState']}, patchCounter={pt['patchCounter']}")
        print(f"  sec0_btnState={pt['sec0_btnState']}, sec0_on={pt['sec0_on']}, sec0_mapping={pt['sec0_mapping']}")
        print(f"  sec0_onReq={pt['sec0_onReq']}, sec0_offReq={pt['sec0_offReq']}")
        print(f"  avgSpeed={pt['avgSpeed']:5.2f}, slowCutoff={pt['slowCutoff']:5.2f}, sec0_speedPx={pt['sec0_speedPx']:6.2f}")

    # Summary statistics
    print("\n" + "=" * 80)
    print("SUMMARY:")
    print("=" * 80)

    job_started_count = sum(1 for pt in path if pt['jobStarted'])
    sec0_on_count = sum(1 for pt in path if pt['sec0_on'])
    sec0_mapping_count = sum(1 for pt in path if pt['sec0_mapping'])
    sec0_onReq_count = sum(1 for pt in path if pt['sec0_onReq'])
    patch_counter_nonzero = sum(1 for pt in path if pt['patchCounter'] > 0)

    print(f"Points where jobStarted=true:     {job_started_count:3d} / {len(path)} ({100*job_started_count/len(path):.1f}%)")
    print(f"Points where sec0_on=true:        {sec0_on_count:3d} / {len(path)} ({100*sec0_on_count/len(path):.1f}%)")
    print(f"Points where sec0_mapping=true:   {sec0_mapping_count:3d} / {len(path)} ({100*sec0_mapping_count/len(path):.1f}%)")
    print(f"Points where sec0_onReq=true:     {sec0_onReq_count:3d} / {len(path)} ({100*sec0_onReq_count/len(path):.1f}%)")
    print(f"Points where patchCounter > 0:    {patch_counter_nonzero:3d} / {len(path)} ({100*patch_counter_nonzero/len(path):.1f}%)")

    # Diagnostic conclusion
    print("\n" + "=" * 80)
    print("DIAGNOSTIC CONCLUSION:")
    print("=" * 80)

    if job_started_count == 0:
        print("PROBLEM: Job not started (isJobStarted=false)")
        print("   Solution: Call formGPS.JobNew() before enabling sections")
    elif sec0_mapping_count == 0:
        print("PROBLEM: Section mapping never turned on (sec0_mapping=false)")
        if sec0_onReq_count == 0:
            print("   Root cause: sec0_onReq=false (section not requesting to turn on)")
            print("   This means autosection logic determined section should NOT be on")
            print("   Possible reasons:")
            print("     - Section button state not set correctly")
            print("     - Speed too slow")
            print("     - Auto-section logic thinks area is already covered")
            print("     - Section look-ahead not seeing uncovered ground")
        else:
            print("   Root cause: sec0_onReq=true but sec0_mapping still false")
            print("   Mapping should turn on after a delay when onReq=true")
    elif patch_counter_nonzero == 0:
        print("PROBLEM: patchCounter always 0 (no patches being drawn)")
        print("   Section mapping is on, but patches aren't being created")
    else:
        print("SUCCESS: Sections appear to be working correctly!")

    print("")

if __name__ == '__main__':
    if len(sys.argv) < 2:
        # Find most recent SectionControl JSON
        test_output = Path(__file__).parent.parent / 'bin' / 'Debug' / 'net48' / 'TestOutput'
        if test_output.exists():
            jsons = sorted(test_output.glob('SectionControl_*.json'), key=lambda p: p.stat().st_mtime, reverse=True)
            if jsons:
                analyze_section_control(jsons[0])
            else:
                print("No SectionControl_*.json files found in TestOutput")
                sys.exit(1)
        else:
            print(f"TestOutput directory not found: {test_output}")
            sys.exit(1)
    else:
        analyze_section_control(sys.argv[1])
