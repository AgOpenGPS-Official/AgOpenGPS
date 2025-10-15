#!/usr/bin/env python3
"""
Visualize U-turn test data from JSON export
Plots the tractor path, field boundary, turn lines, AB line, and turn pattern

Usage:
    python visualize_uturn.py                    # Display plot interactively
    python visualize_uturn.py --no-display       # Save only, no UI
    python visualize_uturn.py path/to/data.json  # Visualize specific file
"""

import json
import matplotlib.pyplot as plt
import numpy as np
import sys
import argparse
from pathlib import Path

def load_json(filepath):
    """Load JSON data from file"""
    with open(filepath, 'r') as f:
        return json.load(f)

def plot_uturn_data(data, title="U-Turn Test Visualization"):
    """Create visualization of U-turn test data"""

    fig, ax = plt.subplots(figsize=(14, 10))

    # Extract data
    tractor_path = data.get('tractorPath', [])
    field_boundary = data.get('fieldBoundary', [])
    turn_lines = data.get('turnLines', [])
    ab_line = data.get('abLine', [])
    turn_pattern = data.get('turnPattern', [])
    metadata = data.get('metadata', {})

    # Plot field boundary (light gray)
    if field_boundary:
        boundary_e = [pt['e'] for pt in field_boundary]
        boundary_n = [pt['n'] for pt in field_boundary]
        # Close the boundary
        boundary_e.append(boundary_e[0])
        boundary_n.append(boundary_n[0])
        ax.plot(boundary_e, boundary_n, 'gray', linewidth=2, label='Field Boundary', alpha=0.5)

    # Plot turn lines (red dashed)
    if turn_lines:
        turn_e = [pt['e'] for pt in turn_lines]
        turn_n = [pt['n'] for pt in turn_lines]
        ax.plot(turn_e, turn_n, 'r--', linewidth=2, label='Turn Line', alpha=0.7)

        # Mark turn line points
        ax.scatter(turn_e, turn_n, c='red', s=30, alpha=0.5, zorder=3)

    # Plot AB line (blue) - clip to reasonable range
    if ab_line:
        ab_e = [pt['e'] for pt in ab_line]
        ab_n = [pt['n'] for pt in ab_line]

        # Determine clipping bounds
        if field_boundary and len(field_boundary) > 0:
            # Use field boundary for clipping
            boundary_n = [pt['n'] for pt in field_boundary]
            min_n = min(boundary_n)
            max_n = max(boundary_n)
            margin = (max_n - min_n) * 0.3  # 30% margin
            clip_n_min = min_n - margin
            clip_n_max = max_n + margin
        elif tractor_path and len(tractor_path) > 0:
            # Fallback: use tractor path extent for clipping
            path_n = [pt['n'] for pt in tractor_path]
            min_n = min(path_n)
            max_n = max(path_n)
            margin = (max_n - min_n) * 0.5  # 50% margin for safety
            clip_n_min = min_n - margin
            clip_n_max = max_n + margin
        else:
            # No reference, clip to reasonable default (±200m)
            clip_n_min = -200
            clip_n_max = 200

        # Clip AB line to calculated extent
        if len(ab_n) >= 2 and ab_n[0] != ab_n[1]:
            # Linear interpolation for clipped line
            slope = (ab_e[1] - ab_e[0]) / (ab_n[1] - ab_n[0])
            intercept = ab_e[0] - slope * ab_n[0]

            clipped_e = [slope * clip_n_min + intercept, slope * clip_n_max + intercept]
            clipped_n = [clip_n_min, clip_n_max]

            ax.plot(clipped_e, clipped_n, 'b-', linewidth=2, label='AB Line (Guidance)', alpha=0.7)
        else:
            # Vertical or single point - plot as-is
            ax.plot(ab_e, ab_n, 'b-', linewidth=2, label='AB Line (Guidance)', alpha=0.7)

    # Plot turn pattern (orange)
    if turn_pattern:
        pattern_e = [pt['e'] for pt in turn_pattern]
        pattern_n = [pt['n'] for pt in turn_pattern]
        ax.plot(pattern_e, pattern_n, 'orange', linewidth=3, label=f'Turn Pattern ({len(turn_pattern)} pts)', zorder=4)

        # Mark start point of turn pattern (where trigger should happen)
        if len(turn_pattern) > 2:
            start_pt = turn_pattern[2]  # Point 2 is the start point
            ax.scatter([start_pt['e']], [start_pt['n']], c='orange', s=200,
                      marker='*', label='Turn Pattern Start', zorder=5, edgecolors='black', linewidths=2)

            # Draw 1m trigger circle around start point
            circle = plt.Circle((start_pt['e'], start_pt['n']), 1.0,
                               color='orange', fill=False, linestyle=':', linewidth=2,
                               label='1m Trigger Distance')
            ax.add_patch(circle)

    # Plot tractor path with time progression (color gradient)
    if tractor_path:
        path_e = [pt['e'] for pt in tractor_path]
        path_n = [pt['n'] for pt in tractor_path]
        path_t = [pt['t'] for pt in tractor_path]
        uturn_active = [pt['uturn'] for pt in tractor_path]

        # Split path into segments: before U-turn, during U-turn, after U-turn
        before_uturn = [(e, n) for e, n, u in zip(path_e, path_n, uturn_active) if not u]
        during_uturn = [(e, n) for e, n, u in zip(path_e, path_n, uturn_active) if u]

        # Plot path before U-turn (green)
        if before_uturn:
            before_e, before_n = zip(*before_uturn)
            ax.plot(before_e, before_n, 'g-', linewidth=2, label='Path (Straight)', alpha=0.8)

        # Plot path during U-turn (magenta)
        if during_uturn:
            during_e, during_n = zip(*during_uturn)
            ax.plot(during_e, during_n, 'm-', linewidth=3, label='Path (U-Turn)', alpha=0.9, zorder=4)

        # Mark start and end points
        ax.scatter([path_e[0]], [path_n[0]], c='green', s=150,
                  marker='o', label='Start', zorder=5, edgecolors='black', linewidths=2)
        ax.scatter([path_e[-1]], [path_n[-1]], c='red', s=150,
                  marker='s', label='End', zorder=5, edgecolors='black', linewidths=2)

        # Mark every 5 seconds with a small dot and label
        for i, (e, n, t) in enumerate(zip(path_e, path_n, path_t)):
            if i % 100 == 0:  # Every 5 seconds (100 samples * 0.05s)
                ax.scatter([e], [n], c='black', s=20, zorder=6, alpha=0.5)
                ax.text(e + 1, n + 1, f'{t:.0f}s', fontsize=8, alpha=0.7)

    # Set labels and title
    ax.set_xlabel('Easting (m)', fontsize=12)
    ax.set_ylabel('Northing (m)', fontsize=12)
    ax.set_title(f'{title}\\nPhase: {metadata.get("youTurnPhase", "?")} | ' +
                f'YT Btn: {metadata.get("isYouTurnBtnOn", "?")} | ' +
                f'Turn Area Width: {metadata.get("turnAreaWidth", "?"):.2f}m',
                fontsize=14, fontweight='bold')
    ax.grid(True, alpha=0.3)
    ax.legend(loc='upper right', fontsize=10)

    # Set axis limits to focus on field area
    if field_boundary:
        boundary_e = [pt['e'] for pt in field_boundary]
        boundary_n = [pt['n'] for pt in field_boundary]
        min_e, max_e = min(boundary_e), max(boundary_e)
        min_n, max_n = min(boundary_n), max(boundary_n)
        margin_e = (max_e - min_e) * 0.2
        margin_n = (max_n - min_n) * 0.15
        ax.set_xlim(min_e - margin_e, max_e + margin_e)
        ax.set_ylim(min_n - margin_n, max_n + margin_n)

    # Use equal aspect for accurate geometry representation
    ax.set_aspect('equal', adjustable='box')

    # Add info text
    info_text = f"Total path points: {len(tractor_path)}\\n"
    if tractor_path:
        info_text += f"Duration: {path_t[-1]:.1f}s\\n"
        info_text += f"U-turn active: {any(uturn_active)}"

    ax.text(0.02, 0.02, info_text, transform=ax.transAxes,
           verticalalignment='bottom', fontsize=9,
           bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.7))

    return fig

def main():
    """Main function"""
    # Parse command line arguments
    parser = argparse.ArgumentParser(description='Visualize U-turn test data from JSON export')
    parser.add_argument('json_file', nargs='?', help='Path to JSON file (optional)')
    parser.add_argument('--no-display', action='store_true', help='Save only, do not display plot')
    args = parser.parse_args()

    # Default to TestOutput directory in the project
    script_dir = Path(__file__).parent
    test_output_dir = script_dir.parent / "TestOutput"

    # Determine which JSON files to visualize
    if args.json_file:
        json_files = [Path(args.json_file)]
    else:
        # Check for test output files
        headless_file = test_output_dir / "uturn_test_headless.json"
        visual_file = test_output_dir / "uturn_test_visual.json"

        json_files = []
        if headless_file.exists():
            json_files.append(headless_file)
        if visual_file.exists():
            json_files.append(visual_file)

        if not json_files:
            print(f"No test data found in {test_output_dir}")
            print("Expected: uturn_test_headless.json or uturn_test_visual.json")
            print("Run the integration tests first to generate data.")
            return

    # Process each file
    figures = []
    for json_file in json_files:
        if not json_file.exists():
            print(f"File not found: {json_file}")
            continue

        print(f"Loading: {json_file}")
        data = load_json(json_file)

        # Create plot
        mode = "Visual" if "visual" in json_file.name else "Headless"
        fig = plot_uturn_data(data, f"{mode} Mode Test")
        figures.append(fig)

        # Save
        output_file = json_file.parent / json_file.name.replace('.json', '.png')
        fig.savefig(output_file, dpi=150, bbox_inches='tight')
        print(f"Saved: {output_file}")

    # Display if requested
    if not args.no_display and figures:
        plt.show()
    else:
        for fig in figures:
            plt.close(fig)

if __name__ == '__main__':
    main()
