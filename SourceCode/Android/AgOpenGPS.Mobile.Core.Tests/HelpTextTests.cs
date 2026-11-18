using System.IO;
using AgOpenGPS.Mobile.Core;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class HelpTextTests
    {
        [Test]
        public void HeadingsListsAndInline()
        {
            string html = HelpText.MarkdownToHtml("# Title\n\n## Part\n\n- **Bold** and *italic* & `code`\n   - nested\n1. First step\n\nText < 5 cm");
            Assert.That(html, Does.Contain("<h2>Title</h2>"));
            Assert.That(html, Does.Contain("<h3>Part</h3>"));
            Assert.That(html, Does.Contain("• <b>Bold</b> and <i>italic</i> &amp; <tt>code</tt><br>"));
            Assert.That(html, Does.Contain("◦ nested<br>"));
            Assert.That(html, Does.Contain("1.&nbsp;First step<br>"));
            Assert.That(html, Does.Contain("Text &lt; 5 cm<br>"), "text is escaped");
        }

        [Test]
        public void StarsInsideWordsAreNotItalic()
        {
            Assert.That(HelpText.MarkdownToHtml("a*b and 2 * 3"), Does.Not.Contain("<i>"));
        }

        [TestCase("user-guide.en.md", "<h2>AgOpenGPS for Android")]
        [TestCase("user-guide.hr.md", "<h2>AgOpenGPS za Android")]
        public void TheRealGuides_Convert(string file, string start)
        {
            // The guides live in docs/ at the repository root; the app embeds the same files.
            string dir = TestContext.CurrentContext.TestDirectory;
            while (dir != null && !Directory.Exists(Path.Combine(dir, "docs"))) dir = Path.GetDirectoryName(dir);
            Assume.That(dir, Is.Not.Null, "docs/ not found");
            string html = HelpText.MarkdownToHtml(File.ReadAllText(Path.Combine(dir, "docs", file)));
            Assert.That(html, Does.StartWith(start));
            Assert.That(html, Does.Not.Contain("**"), "every bold marker was converted");
            Assert.That(html.Length, Is.GreaterThan(3000));
        }
    }
}

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class HelpTextSpacingTests
    {
        [Test]
        public void NoExtraBreaksAroundHeadings_ButParagraphsStaySeparate()
        {
            string html = HelpText.MarkdownToHtml("# T\n\n## A\n\nOne.\n\nTwo.\n\n## B\n\n- x");
            Assert.That(html, Is.EqualTo("<h2>T</h2><h3>A</h3>One.<br><br>Two.<br><h3>B</h3>&nbsp;&nbsp;• x<br>"));
        }
    }
}
