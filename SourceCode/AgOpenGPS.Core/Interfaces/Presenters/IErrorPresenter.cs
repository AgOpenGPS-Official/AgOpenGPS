using System;

namespace AgOpenGPS.Core.Interfaces
{
    public interface IErrorPresenter
    {
        void PresentTimedMessage(
            TimeSpan timeSpan,
            string titleString,
            string messageString);
    }
}
