using System;

namespace AgOpenGPS
{
    /// <summary>
    /// Tracks field coverage in a grid to calculate overlap without requiring OpenGL
    /// Used in headless mode where GL.ReadPixels is not available
    /// </summary>
    public class CCoverageTracker
    {
        private readonly FormGPS mf;
        private byte[,] coverageGrid;
        private int gridWidth;
        private int gridHeight;
        private double gridResolution; // meters per grid cell
        private double fieldMinX, fieldMinY;

        public CCoverageTracker(FormGPS _f)
        {
            mf = _f;
            gridResolution = 0.10; // 10cm per grid cell for better accuracy
        }

        /// <summary>
        /// Initialize the coverage grid based on field boundary
        /// </summary>
        public void InitializeGrid()
        {
            if (mf.bnd.bndList.Count == 0)
            {
                // No boundary, use a default large area
                gridWidth = 400;
                gridHeight = 800;
                fieldMinX = -50;
                fieldMinY = -100;
            }
            else
            {
                // Calculate bounding box from boundary
                double minX = double.MaxValue, maxX = double.MinValue;
                double minY = double.MaxValue, maxY = double.MinValue;

                foreach (var point in mf.bnd.bndList[0].fenceLine)
                {
                    if (point.easting < minX) minX = point.easting;
                    if (point.easting > maxX) maxX = point.easting;
                    if (point.northing < minY) minY = point.northing;
                    if (point.northing > maxY) maxY = point.northing;
                }

                fieldMinX = minX - 10; // 10m padding
                fieldMinY = minY - 10;
                double fieldMaxX = maxX + 10;
                double fieldMaxY = maxY + 10;

                gridWidth = (int)Math.Ceiling((fieldMaxX - fieldMinX) / gridResolution);
                gridHeight = (int)Math.Ceiling((fieldMaxY - fieldMinY) / gridResolution);

                // Cap grid size to prevent excessive memory use
                // At 0.1m resolution: 100m x 200m field = 1000 x 2000 grid = 2MB
                if (gridWidth > 2000) gridWidth = 2000;
                if (gridHeight > 4000) gridHeight = 4000;
            }

            // Initialize grid (0 = not covered, 1 = once, 2 = twice, 3+ = more)
            coverageGrid = new byte[gridWidth, gridHeight];
        }

        /// <summary>
        /// Mark coverage for a section strip defined by left and right points
        /// </summary>
        public void MarkCoverage(vec2 leftPoint, vec2 rightPoint, vec2 prevLeftPoint, vec2 prevRightPoint)
        {
            if (coverageGrid == null) return;

            // Convert world coordinates to grid coordinates
            int x1Left = WorldToGridX(leftPoint.easting);
            int y1Left = WorldToGridY(leftPoint.northing);
            int x1Right = WorldToGridX(rightPoint.easting);
            int y1Right = WorldToGridY(rightPoint.northing);

            int x2Left = WorldToGridX(prevLeftPoint.easting);
            int y2Left = WorldToGridY(prevLeftPoint.northing);
            int x2Right = WorldToGridX(prevRightPoint.easting);
            int y2Right = WorldToGridY(prevRightPoint.northing);

            // Fill the quadrilateral defined by the four points
            FillQuadrilateral(x1Left, y1Left, x1Right, y1Right, x2Left, y2Left, x2Right, y2Right);
        }

        /// <summary>
        /// Fill a quadrilateral in the grid by scanning lines
        /// </summary>
        private void FillQuadrilateral(int x1L, int y1L, int x1R, int y1R, int x2L, int y2L, int x2R, int y2R)
        {
            // Find bounding box
            int minY = Math.Max(0, Math.Min(Math.Min(y1L, y1R), Math.Min(y2L, y2R)));
            int maxY = Math.Min(gridHeight - 1, Math.Max(Math.Max(y1L, y1R), Math.Max(y2L, y2R)));

            // For each scanline
            for (int y = minY; y <= maxY; y++)
            {
                // Find intersection points with quad edges
                int minX = gridWidth;
                int maxX = -1;

                // Check all 4 edges
                UpdateScanline(x1L, y1L, x2L, y2L, y, ref minX, ref maxX); // Left edge
                UpdateScanline(x1R, y1R, x2R, y2R, y, ref minX, ref maxX); // Right edge
                UpdateScanline(x1L, y1L, x1R, y1R, y, ref minX, ref maxX); // Top edge
                UpdateScanline(x2L, y2L, x2R, y2R, y, ref minX, ref maxX); // Bottom edge

                // Fill the scanline
                if (minX <= maxX)
                {
                    minX = Math.Max(0, minX);
                    maxX = Math.Min(gridWidth - 1, maxX);

                    for (int x = minX; x <= maxX; x++)
                    {
                        if (coverageGrid[x, y] < 255)
                            coverageGrid[x, y]++;
                    }
                }
            }
        }

        /// <summary>
        /// Update scanline min/max with intersection of line segment
        /// </summary>
        private void UpdateScanline(int x1, int y1, int x2, int y2, int y, ref int minX, ref int maxX)
        {
            if ((y1 <= y && y <= y2) || (y2 <= y && y <= y1))
            {
                if (y1 == y2)
                {
                    // Horizontal line
                    if (x1 < minX) minX = x1;
                    if (x2 < minX) minX = x2;
                    if (x1 > maxX) maxX = x1;
                    if (x2 > maxX) maxX = x2;
                }
                else
                {
                    // Calculate intersection x
                    int x = x1 + (x2 - x1) * (y - y1) / (y2 - y1);
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                }
            }
        }

        /// <summary>
        /// Calculate actual area covered and overlap from the grid
        /// </summary>
        public void CalculateOverlap()
        {
            if (coverageGrid == null || mf.fd.workedAreaTotal < 0.1) return;

            int once = 0;
            int twice = 0;
            int more = 0;

            // Count coverage levels
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    byte coverage = coverageGrid[x, y];
                    if (coverage == 1) once++;
                    else if (coverage == 2) twice++;
                    else if (coverage >= 3) more++;
                }
            }

            double total = once + twice + more;
            double total2 = total + twice + more + more;

            if (total2 > 0)
            {
                mf.fd.actualAreaCovered = (total / total2 * mf.fd.workedAreaTotal);
                mf.fd.overlapPercent = Math.Round(((1 - total / total2) * 100), 2);
            }
            else
            {
                mf.fd.actualAreaCovered = mf.fd.overlapPercent = 0;
            }
        }

        /// <summary>
        /// Reset the coverage grid
        /// </summary>
        public void Reset()
        {
            if (coverageGrid != null)
            {
                Array.Clear(coverageGrid, 0, coverageGrid.Length);
            }
        }

        private int WorldToGridX(double worldX)
        {
            return (int)((worldX - fieldMinX) / gridResolution);
        }

        private int WorldToGridY(double worldY)
        {
            return (int)((worldY - fieldMinY) / gridResolution);
        }
    }
}
