using System;
namespace AgOpenGPS
{
    public interface IPgnErrorPresenter
    {
        void PresentRefactoringBug(string className, int index, byte orgValue, byte refactoredVal);

        void PresentOverflowErrorIntToByte(string className, int index, int inputValue, byte errorValue);
        void PresentOverflowErrorIntToUInt16(string className, int index, int inputValue, UInt16 errorValue);

        void PresentOverflowErrorDoubleToUInt16(string className, byte index, double inputValue, UInt16 errorValue);
    }
}
