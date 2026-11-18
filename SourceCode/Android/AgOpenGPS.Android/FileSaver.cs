using System;
using Android.App;
using Android.Content;
using Android.Widget;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// Saving a file where the user chooses (tablet, Downloads, USB stick, Drive...) through Android's
    /// save dialog: <see cref="Save"/> opens it, <see cref="HandleResult"/> (from OnActivityResult)
    /// writes the bytes.
    /// </summary>
    public static class FileSaver
    {
        public const int RequestCode = 3;

        private static byte[] _pending;
        private static string _doneMessage;

        public static void Save(Activity activity, string fileName, string mimeType, byte[] data, string doneMessage)
        {
            _pending = data;
            _doneMessage = doneMessage;
            var intent = new Intent(Intent.ActionCreateDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType(mimeType);
            intent.PutExtra(Intent.ExtraTitle, fileName);
            activity.StartActivityForResult(intent, RequestCode);
        }

        /// <summary>True when the result belonged to a save (handled here).</summary>
        public static bool HandleResult(Activity activity, int requestCode, Result resultCode, Intent data)
        {
            if (requestCode != RequestCode) return false;
            byte[] bytes = _pending;
            _pending = null;
            if (resultCode != Result.Ok || data?.Data == null || bytes == null) return true;
            try
            {
                using (var stream = activity.ContentResolver.OpenOutputStream(data.Data, "wt"))
                    stream.Write(bytes, 0, bytes.Length);
                Toast.MakeText(activity, _doneMessage, ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(activity, Tr.F("Could not save: {0}", ex.Message), ToastLength.Long).Show();
            }
            return true;
        }
    }
}
