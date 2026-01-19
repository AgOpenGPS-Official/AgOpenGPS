using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;
using System.Collections.Generic;

namespace AgOpenGPS.Visuals
{
    public class SectionsVisual
    {
        private static ColorRgba sectionsColor = new ColorRgba(0.9f, 0.9f, 0.8f);
        public static void DrawSections(List<CPatches> triStrip)
        {
            const int mipmap = 8;
            GLW.SetColor(sectionsColor);

            foreach (CPatches patches in triStrip)
            {
                foreach (List<vec3> triList in patches.patchList)
                {
                    GLW.BeginTriangleStripPrimitive();
                    int cnt = triList.Count;

                    //if large enough patch and camera zoomed out, fake mipmap the patches, skip triangles
                    if (cnt >= (mipmap))
                    {
                        int step = mipmap;
                        for (int i = 1; i < cnt; i += step)
                        {
                            GLW.Vertex2(triList[i++].ToGeoCoord());
                            GLW.Vertex2(triList[i++].ToGeoCoord());

                            //too small to mipmap it
                            if (cnt - i <= (mipmap + 2))
                                step = 0;
                        }
                    }
                    else
                    {
                        for (int i = 1; i < cnt; i++)
                        {
                            GLW.Vertex2(triList[i].ToGeoCoord());
                        }
                    }
                    GLW.EndPrimitive();
                }
            }
        }
    }
}
