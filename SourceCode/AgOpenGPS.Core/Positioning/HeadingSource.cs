// Purpose: Enum for heading source selection; avoids fragile strings.
namespace AgOpenGPS.Core.Positioning
{
    /// <summary>
    /// Indicates which source is used to determine heading for a given tick.
    /// </summary>
    public enum HeadingSource
    {
        Fix2Fix = 0,
        VTG = 1,
        Dual = 2
    }
}
