using System;

namespace AgOpenGPS
{
    public class WinFormsPgnErrorPresenter : IPgnErrorPresenter
    {
        private void TimedMessageBox(int timeout, string title, string message)
        {
            var form = new FormTimedMessage(timeout, title, message);
            form.Show();
        }

        void IPgnErrorPresenter.PresentOverflowErrorDoubleToUInt16(string className, int index, double inputValue, ushort errorValue)
        {
            string msg = "Class name:" + className + " Index:" + index + " Input value:" + inputValue + " Output value:" + errorValue;
            TimedMessageBox(5000, "Pgn overflow bug found", msg);
        }

        void IPgnErrorPresenter.PresentOverflowErrorIntToByte(string className, int index, int inputValue, byte errorValue)
        {
            string msg = "Class name:" + className + " Index:" + index + " Input value:" + inputValue + " Output value:" + errorValue;
            TimedMessageBox(5000, "Pgn overflow bug found", msg);
        }

        void IPgnErrorPresenter.PresentOverflowErrorIntToUInt16(string className, int index, int inputValue, ushort errorValue)
        {
            string msg = "Class name:" + className + " Index:" + index + " Input value:" + inputValue + " Output value:" + errorValue;
            TimedMessageBox(5000, "Pgn overflow bug found", msg);
        }

        void IPgnErrorPresenter.PresentRefactoringBug(string className, int index, byte orgVal, byte refactoredVal)
        {
            string msg = "Class name: " + className + "Index: " + index + "Original value: " + orgVal + "Refactored value: " + refactoredVal;
            TimedMessageBox(5000, "Pgn refactoring bug found", msg);
        }
    }
}
