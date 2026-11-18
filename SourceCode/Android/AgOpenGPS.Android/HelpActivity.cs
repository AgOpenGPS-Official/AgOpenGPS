using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Widget;
using AgOpenGPS.Mobile.Core;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// The user guide in English or Croatian: docs/user-guide.*.md, packed into the APK, so help
    /// works without internet. Starts in the app's language; the buttons switch.
    /// </summary>
    [Activity(Label = "Help", Theme = "@android:style/Theme.Material.Light.NoActionBar",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout
            | ConfigChanges.SmallestScreenSize | ConfigChanges.KeyboardHidden | ConfigChanges.Keyboard)]
    public class HelpActivity : Activity
    {
        private TextView _text;
        private ScrollView _scroll;
        private Button _english, _croatian;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AppState.Init(this);
            SetContentView(BuildLayout());
            Show(Tr.IsCroatian ? "hr" : "en");
        }

        private void Show(string language)
        {
            string markdown;
            try
            {
                using (Stream s = Assets.Open("user-guide." + language + ".md"))
                using (var r = new StreamReader(s))
                    markdown = r.ReadToEnd();
            }
            catch (Exception ex)
            {
                markdown = "Help text missing: " + ex.Message;
            }

            _text.SetText(Html.FromHtml(HelpText.MarkdownToHtml(markdown), FromHtmlOptions.ModeLegacy), TextView.BufferType.Spannable);
            _scroll.ScrollTo(0, 0);
            _english.Alpha = language == "en" ? 1f : 0.5f;
            _croatian.Alpha = language == "hr" ? 1f : 0.5f;
        }

        private View BuildLayout()
        {
            var root = new LinearLayout(this) { Orientation = Orientation.Vertical };
            root.SetBackgroundColor(Color.ParseColor("#F2F2F2"));

            var bar = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            bar.SetBackgroundColor(Color.ParseColor("#37474F"));
            bar.SetPadding(Ui.Dp(this, 8), Ui.Dp(this, 4), Ui.Dp(this, 8), Ui.Dp(this, 4));
            bar.SetGravity(GravityFlags.CenterVertical);
            bar.AddView(Ui.Button(this, Tr.T("< Back"), Finish, 48));
            TextView title = Ui.Text(this, 22, true, "#FFFFFF");
            title.Text = "  " + Tr.T("Help");
            bar.AddView(title, Ui.Weight(1));
            _english = Ui.Button(this, "English", () => Show("en"), 48);
            _croatian = Ui.Button(this, "Hrvatski", () => Show("hr"), 48);
            bar.AddView(_english);
            bar.AddView(_croatian);
            root.AddView(bar);

            _text = new TextView(this);
            _text.SetTextSize(ComplexUnitType.Sp, 17);
            _text.SetTextColor(Color.ParseColor("#212121"));
            _text.SetLineSpacing(Ui.Dp(this, 3), 1f);
            _text.SetTextIsSelectable(true);
            int pad = Ui.Dp(this, 24);
            _text.SetPadding(pad, Ui.Dp(this, 8), pad, pad);

            // A readable column on a wide tablet screen.
            var card = new LinearLayout(this) { Orientation = Orientation.Vertical };
            card.SetBackgroundColor(Color.White);
            card.AddView(_text);
            var centre = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            centre.SetGravity(GravityFlags.CenterHorizontal);
            centre.SetPadding(Ui.Dp(this, 12), Ui.Dp(this, 12), Ui.Dp(this, 12), Ui.Dp(this, 12));
            int width = Math.Min(Resources.DisplayMetrics.WidthPixels - Ui.Dp(this, 24), Ui.Dp(this, 860));
            centre.AddView(card, new LinearLayout.LayoutParams(width, ViewGroup.LayoutParams.WrapContent));

            _scroll = new ScrollView(this);
            _scroll.AddView(centre);
            root.AddView(_scroll, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, 1));
            return root;
        }
    }
}
