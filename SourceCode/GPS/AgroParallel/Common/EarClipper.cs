// ============================================================================
// EarClipper.cs - Triangulacion por ear-clipping de poligonos simples
// Ubicación: SourceCode/GPS/AgroParallel/Common/EarClipper.cs
// Target: net48 (C# 7.3)
//
// Triangulacion basica de poligonos simples (sin auto-interseccion, sin
// agujeros) usando el algoritmo de "orejas" de Meisters.
// Complejidad O(n²) que es suficiente para rings de shapefiles agricolas.
//
// El metodo normaliza winding (si el poligono es CW lo invierte a CCW) y
// devuelve el resultado como una lista plana de indices [a,b,c, a,b,c, ...]
// donde cada terna forma un triangulo en el array de puntos original.
//
// Retorna lista vacia si el poligono es degenerado (<3 puntos unicos) o si
// la triangulacion queda atascada (typicamente auto-interseccion).
// ============================================================================

using System.Collections.Generic;
using System.Drawing;

namespace AgroParallel.Common
{
    internal static class EarClipper
    {
        public static List<int> Triangulate(PointF[] points)
        {
            var result = new List<int>();
            if (points == null) return result;

            int n = points.Length;
            // Shapefile rings cierran el anillo repitiendo el primer punto;
            // ignoramos el ultimo si coincide con el primero.
            if (n >= 2 && points[0].X == points[n - 1].X && points[0].Y == points[n - 1].Y)
                n--;
            if (n < 3) return result;

            // Winding: positivo = CCW, negativo = CW. El algoritmo asume CCW.
            bool ccw = SignedArea(points, n) > 0;

            var ring = new LinkedList<int>();
            if (ccw)
            {
                for (int i = 0; i < n; i++) ring.AddLast(i);
            }
            else
            {
                for (int i = n - 1; i >= 0; i--) ring.AddLast(i);
            }

            int safety = n * n + 10;
            var node = ring.First;

            while (ring.Count > 3 && safety-- > 0)
            {
                var prev = node.Previous ?? ring.Last;
                var next = node.Next ?? ring.First;

                int ia = prev.Value, ib = node.Value, ic = next.Value;
                PointF a = points[ia], b = points[ib], c = points[ic];

                if (IsConvex(a, b, c) && !AnyPointInside(points, ring, ia, ib, ic, a, b, c))
                {
                    result.Add(ia);
                    result.Add(ib);
                    result.Add(ic);

                    var toRemove = node;
                    node = next;
                    ring.Remove(toRemove);
                }
                else
                {
                    node = next;
                }
            }

            if (ring.Count == 3)
            {
                var e = ring.First;
                result.Add(e.Value); e = e.Next;
                result.Add(e.Value); e = e.Next;
                result.Add(e.Value);
            }
            else if (ring.Count > 3)
            {
                // Triangulacion atascada (auto-interseccion o precision).
                // Devolvemos lo que tengamos; el caller puede fallback a outline.
            }

            return result;
        }

        private static double SignedArea(PointF[] p, int n)
        {
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                sum += (double)p[i].X * p[j].Y - (double)p[j].X * p[i].Y;
            }
            return sum * 0.5;
        }

        private static bool IsConvex(PointF a, PointF b, PointF c)
        {
            // Asumiendo winding CCW, una esquina convexa tiene cross > 0.
            double cross = (double)(b.X - a.X) * (c.Y - a.Y)
                         - (double)(b.Y - a.Y) * (c.X - a.X);
            return cross > 0;
        }

        private static bool AnyPointInside(PointF[] pts, LinkedList<int> ring,
            int ia, int ib, int ic, PointF a, PointF b, PointF c)
        {
            foreach (int i in ring)
            {
                if (i == ia || i == ib || i == ic) continue;
                if (PointInTriangle(pts[i], a, b, c)) return true;
            }
            return false;
        }

        private static bool PointInTriangle(PointF p, PointF a, PointF b, PointF c)
        {
            double s1 = Sign(p, a, b);
            double s2 = Sign(p, b, c);
            double s3 = Sign(p, c, a);
            bool hasNeg = s1 < 0 || s2 < 0 || s3 < 0;
            bool hasPos = s1 > 0 || s2 > 0 || s3 > 0;
            return !(hasNeg && hasPos);
        }

        private static double Sign(PointF p, PointF a, PointF b)
        {
            return (double)(p.X - b.X) * (a.Y - b.Y)
                 - (double)(a.X - b.X) * (p.Y - b.Y);
        }
    }
}
