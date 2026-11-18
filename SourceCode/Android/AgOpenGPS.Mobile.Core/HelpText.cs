using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>
    /// Turns the simple Markdown of the user guide (docs/user-guide.*.md) into the small HTML subset
    /// Android's Html.FromHtml shows: headings, bullet and numbered lines, bold, italic, code.
    /// The guide in the repository and the help screen thereby share one text.
    /// </summary>
    public static class HelpText
    {
        private static readonly Regex Bold = new Regex(@"\*\*(.+?)\*\*");
        private static readonly Regex Italic = new Regex(@"(?<![\*\w])\*(?!\s)(.+?)(?<!\s)\*(?![\*\w])");
        private static readonly Regex Code = new Regex(@"`([^`]+)`");
        private static readonly Regex Numbered = new Regex(@"^(\d+)\.\s+(.*)$");

        public static string MarkdownToHtml(string markdown)
        {
            var html = new StringBuilder();
            string[] lines = (markdown ?? "").Replace("\r\n", "\n").Split('\n');
            bool lastWasText = false; // a blank line after text is a paragraph break...
            for (int li = 0; li < lines.Length; li++)
            {
                string line = lines[li].TrimEnd();
                if (line.Length == 0)
                {
                    // ...but not after or before a heading: headings bring their own space.
                    if (lastWasText && !NextIsHeading(lines, li)) html.Append("<br>");
                    lastWasText = false;
                    continue;
                }
                lastWasText = !line.TrimStart().StartsWith("#");

                int indent = line.Length - line.TrimStart().Length;
                string t = line.TrimStart();

                if (t.StartsWith("### ")) html.Append("<h4>").Append(Inline(t.Substring(4))).Append("</h4>");
                else if (t.StartsWith("## ")) html.Append("<h3>").Append(Inline(t.Substring(3))).Append("</h3>");
                else if (t.StartsWith("# ")) html.Append("<h2>").Append(Inline(t.Substring(2))).Append("</h2>");
                else if (t.StartsWith("- ") || t.StartsWith("* "))
                {
                    // Nested bullets (indented) get a smaller mark and extra space in front.
                    html.Append(indent >= 2 ? "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;◦ " : "&nbsp;&nbsp;• ")
                        .Append(Inline(t.Substring(2))).Append("<br>");
                }
                else if (Numbered.Match(t) is Match m && m.Success)
                {
                    html.Append("&nbsp;&nbsp;").Append(m.Groups[1].Value).Append(".&nbsp;").Append(Inline(m.Groups[2].Value)).Append("<br>");
                }
                else
                {
                    html.Append(Inline(t)).Append("<br>");
                }
            }
            return html.ToString();
        }

        private static bool NextIsHeading(string[] lines, int from)
        {
            for (int i = from + 1; i < lines.Length; i++)
            {
                string t = lines[i].Trim();
                if (t.Length > 0) return t.StartsWith("#");
            }
            return true; // end of text: no break needed
        }

        private static string Inline(string text)
        {
            string s = WebUtility.HtmlEncode(text);
            s = Code.Replace(s, "<tt>$1</tt>");
            s = Bold.Replace(s, "<b>$1</b>");
            s = Italic.Replace(s, "<i>$1</i>");
            return s;
        }
    }
}
