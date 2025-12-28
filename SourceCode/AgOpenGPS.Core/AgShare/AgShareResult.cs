using System;

namespace AgOpenGPS.Core.AgShare
{
    public class AgShareResult
    {
        private static readonly AgShareResult _success = new AgShareResult();

        private AgShareResult()
        {
        }

        private AgShareResult(AgShareError error)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public bool IsSuccessful => Error == null;
        public AgShareError Error { get; }

        public static AgShareResult Success() => _success;

        public static AgShareResult Failure(AgShareError error) => new AgShareResult(error);
    }
}
