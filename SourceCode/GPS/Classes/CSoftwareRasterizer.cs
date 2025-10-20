using System;

namespace AgOpenGPS
{
    /// <summary>
    /// Software triangle rasterizer for coverage tracking without OpenGL
    /// Renders triangles to a bitmap to calculate accurate overlap
    /// Mimics OpenGL rendering behavior without requiring GPU or graphics context
    /// </summary>
    public class CSoftwareRasterizer
    {
        private readonly FormGPS mf;
        private byte[] framebuffer;  // 1 byte per pixel (0-15 coverage levels, capped)
        private int width;
        private int height;
        private double pixelsPerMeter;
        private double fieldMinX, fieldMinY;
        private double fieldMaxX, fieldMaxY;

        public CSoftwareRasterizer(FormGPS _f)
        {
            mf = _f;
        }

        /// <summary>
        /// Initialize the framebuffer based on field size
        /// </summary>
        public void Initialize()
        {
            // Calculate field bounds
            if (mf.bnd.bndList.Count == 0)
            {
                fieldMinX = -50;
                fieldMinY = -100;
                fieldMaxX = 50;
                fieldMaxY = 100;
            }
            else
            {
                fieldMinX = double.MaxValue;
                fieldMinY = double.MaxValue;
                fieldMaxX = double.MinValue;
                fieldMaxY = double.MinValue;

                foreach (var point in mf.bnd.bndList[0].fenceLine)
                {
                    if (point.easting < fieldMinX) fieldMinX = point.easting;
                    if (point.easting > fieldMaxX) fieldMaxX = point.easting;
                    if (point.northing < fieldMinY) fieldMinY = point.northing;
                    if (point.northing > fieldMaxY) fieldMaxY = point.northing;
                }

                // Add padding
                fieldMinX -= 10;
                fieldMinY -= 10;
                fieldMaxX += 10;
                fieldMaxY += 10;
            }

            // Calculate framebuffer size
            // Use 10 pixels per meter (10cm resolution) - same as original OpenGL implementation
            pixelsPerMeter = 10.0;

            double fieldWidth = fieldMaxX - fieldMinX;
            double fieldHeight = fieldMaxY - fieldMinY;

            width = (int)Math.Ceiling(fieldWidth * pixelsPerMeter);
            height = (int)Math.Ceiling(fieldHeight * pixelsPerMeter);

            // Cap size to prevent excessive memory
            // At 10 pixels/meter with 1 byte per pixel:
            // 200m x 400m = 2,000 x 4,000 pixels = 8M pixels = 8MB
            if (width > 2000) width = 2000;
            if (height > 4000) height = 4000;

            // 1 byte per pixel for simplicity (0-255 coverage levels)
            framebuffer = new byte[width * height];
        }

        /// <summary>
        /// Render a triangle to the framebuffer
        /// Uses scanline rasterization for efficiency
        /// </summary>
        public void RenderTriangle(vec3 v0, vec3 v1, vec3 v2)
        {
            if (framebuffer == null) return;

            // Convert world coordinates to pixel coordinates
            int x0 = WorldToPixelX(v0.easting);
            int y0 = WorldToPixelY(v0.northing);
            int x1 = WorldToPixelX(v1.easting);
            int y1 = WorldToPixelY(v1.northing);
            int x2 = WorldToPixelX(v2.easting);
            int y2 = WorldToPixelY(v2.northing);

            // Sort vertices by Y coordinate (y0 <= y1 <= y2)
            if (y0 > y1) { Swap(ref x0, ref x1); Swap(ref y0, ref y1); }
            if (y0 > y2) { Swap(ref x0, ref x2); Swap(ref y0, ref y2); }
            if (y1 > y2) { Swap(ref x1, ref x2); Swap(ref y1, ref y2); }

            // Rasterize triangle using scanline algorithm
            RasterizeTriangleScanline(x0, y0, x1, y1, x2, y2);
        }

        /// <summary>
        /// Scanline triangle rasterization
        /// </summary>
        private void RasterizeTriangleScanline(int x0, int y0, int x1, int y1, int x2, int y2)
        {
            // Handle degenerate triangles
            if (y0 == y2) return;

            // Rasterize top half (y0 to y1)
            if (y1 > y0)
            {
                for (int y = y0; y <= y1; y++)
                {
                    if (y < 0 || y >= height) continue;

                    float t = (float)(y - y0) / (y1 - y0);
                    int xLeft = (int)(x0 + t * (x1 - x0));
                    int xRight = (int)(x0 + ((float)(y - y0) / (y2 - y0)) * (x2 - x0));

                    if (xLeft > xRight) Swap(ref xLeft, ref xRight);

                    DrawHorizontalLine(y, xLeft, xRight);
                }
            }

            // Rasterize bottom half (y1 to y2)
            if (y2 > y1)
            {
                for (int y = y1; y <= y2; y++)
                {
                    if (y < 0 || y >= height) continue;

                    float t = (float)(y - y1) / (y2 - y1);
                    int xLeft = (int)(x1 + t * (x2 - x1));
                    int xRight = (int)(x0 + ((float)(y - y0) / (y2 - y0)) * (x2 - x0));

                    if (xLeft > xRight) Swap(ref xLeft, ref xRight);

                    DrawHorizontalLine(y, xLeft, xRight);
                }
            }
        }

        /// <summary>
        /// Draw a horizontal line in the framebuffer
        /// </summary>
        private void DrawHorizontalLine(int y, int xStart, int xEnd)
        {
            // Bounds check for y
            if (y < 0 || y >= height) return;

            // Clamp x coordinates to valid range
            xStart = Math.Max(0, Math.Min(width - 1, xStart));
            xEnd = Math.Max(0, Math.Min(width - 1, xEnd));

            int offset = y * width;

            // Additional safety check for array bounds
            if (offset + xEnd >= framebuffer.Length) return;

            for (int x = xStart; x <= xEnd; x++)
            {
                int index = offset + x;
                if (index >= 0 && index < framebuffer.Length)
                {
                    // Increment coverage counter (similar to OpenGL blending)
                    // Cap at 255 to prevent overflow
                    if (framebuffer[index] < 255)
                        framebuffer[index]++;
                }
            }
        }

        /// <summary>
        /// Calculate overlap from the framebuffer
        /// Uses same algorithm as OpenGL pixel reading
        /// </summary>
        public void CalculateOverlap()
        {
            if (framebuffer == null || mf.fd.workedAreaTotal < 0.1) return;

            int once = 0;
            int twice = 0;
            int more = 0;
            int maxCoverage = 0;
            int nonZeroPixels = 0;

            // Count coverage levels - adjusted thresholds for software rasterizer
            // Triangles within a quad overlap, so single-pass pixels can have value 1-3
            // Multiple passes will have higher values due to additional triangle overlaps
            for (int i = 0; i < framebuffer.Length; i++)
            {
                byte coverage = framebuffer[i];
                if (coverage > 0)
                {
                    nonZeroPixels++;
                    if (coverage > maxCoverage) maxCoverage = coverage;
                }

                // Adjusted thresholds accounting for intra-quad triangle overlap
                // Empirically determined: pixels in overlap regions have higher coverage counts
                // once: 1-2 (single pass, minimal intra-quad overlap)
                // twice: 3-5 (two passes)
                // more: 6+ (three+ passes)
                if (coverage >= 6) more++;           // Covered 3+ times
                else if (coverage >= 3) twice++;     // Covered twice
                else if (coverage >= 1) once++;      // Covered once
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
        /// Clear the framebuffer
        /// </summary>
        public void Clear()
        {
            if (framebuffer != null)
            {
                Array.Clear(framebuffer, 0, framebuffer.Length);
            }
        }

        // Helper methods
        private int WorldToPixelX(double worldX)
        {
            return (int)((worldX - fieldMinX) * pixelsPerMeter);
        }

        private int WorldToPixelY(double worldY)
        {
            return (int)((worldY - fieldMinY) * pixelsPerMeter);
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
