using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Pdf;
using AgOpenGPS.Mobile.Core;

namespace AgOpenGPS.Droid
{
    /// <summary>The job log as a printable PDF (A4 landscape table), in the app's language.</summary>
    public static class JobReport
    {
        private const int PageW = 842, PageH = 595, Margin = 36, RowH = 18;

        public static byte[] Pdf(IEnumerable<Job> jobsNewestFirst)
        {
            List<Job> jobs = jobsNewestFirst.OrderBy(j => j.StartUtc).ToList();
            var ci = CultureInfo.InvariantCulture;
            string[] head =
            {
                Tr.T("Date"), Tr.T("Field"), Tr.T("Start"), Tr.T("End"), Tr.T("Working time (h:mm)"),
                Tr.T("Worked (ha)"), Tr.T("Distance (km)"), Tr.T("Average speed (km/h)"), Tr.T("Implement width (m)"),
            };
            float[] colX = { 0, 70, 250, 300, 350, 460, 540, 625, 735 };

            var title = new Paint { Color = Color.Black, TextSize = 18, FakeBoldText = true, AntiAlias = true };
            var bold = new Paint { Color = Color.Black, TextSize = 10, FakeBoldText = true, AntiAlias = true };
            var text = new Paint { Color = Color.Black, TextSize = 10, AntiAlias = true };
            var line = new Paint { Color = Color.Gray, StrokeWidth = 0.5f };

            using (var doc = new PdfDocument())
            {
                int pageNo = 0, row = 0;
                PdfDocument.Page page = null;
                Canvas c = null;
                float y = 0;

                void NewPage()
                {
                    if (page != null) doc.FinishPage(page);
                    pageNo++;
                    page = doc.StartPage(new PdfDocument.PageInfo.Builder(PageW, PageH, pageNo).Create());
                    c = page.Canvas;
                    y = Margin + 18;
                    c.DrawText(Tr.T("Job log") + "  –  AgOpenGPS", Margin, y, title);
                    y += 16;
                    c.DrawText(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm", ci) + "   " + Tr.F("Page {0}", pageNo), Margin, y, text);
                    y += 24;
                    for (int i = 0; i < head.Length; i++) c.DrawText(head[i], Margin + colX[i], y, bold);
                    y += 6;
                    c.DrawLine(Margin, y, PageW - Margin, y, line);
                    y += RowH - 4;
                }

                NewPage();
                double totalHa = 0, totalSec = 0;
                foreach (Job j in jobs)
                {
                    if (y > PageH - Margin - RowH * 2) NewPage();
                    System.DateTime s = j.StartUtc.ToLocalTime(), e = j.EndUtc.ToLocalTime();
                    string[] cells =
                    {
                        s.ToString("yyyy-MM-dd", ci), Fit(j.FieldName, 30), s.ToString("HH:mm", ci), e.ToString("HH:mm", ci),
                        JobLog.Duration(j.WorkingSeconds), j.Hectares.ToString("0.00", ci), (j.WorkingMeters / 1000).ToString("0.00", ci),
                        double.IsNaN(j.AverageSpeedKmh) ? "" : j.AverageSpeedKmh.ToString("0.0", ci), j.ImplementWidth.ToString("0.00", ci),
                    };
                    for (int i = 0; i < cells.Length; i++) c.DrawText(cells[i], Margin + colX[i], y, text);
                    y += RowH;
                    row++;
                    totalHa += j.Hectares;
                    totalSec += j.WorkingSeconds;
                }
                y += 4;
                c.DrawLine(Margin, y - RowH + 6, PageW - Margin, y - RowH + 6, line);
                c.DrawText(Tr.F("Total: {0} jobs, {1:0.00} ha, {2} h:mm", row, totalHa, JobLog.Duration(totalSec)), Margin, y + 4, bold);
                doc.FinishPage(page);

                using (var ms = new MemoryStream())
                {
                    doc.WriteTo(ms);
                    return ms.ToArray();
                }
            }
        }

        private static string Fit(string s, int max) => string.IsNullOrEmpty(s) || s.Length <= max ? s ?? "" : s.Substring(0, max - 1) + "…";
    }
}
