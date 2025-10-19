using AgOpenGPS.Core.Models;
using AgOpenGPS.Core;
using AgOpenGPS.Protocols.ISOBUS;
using AgOpenGPS;
using System.Collections.Generic;
using System.Xml;
using AgOpenGPS.Core.Models.Guidance; // BIG BANG Step 2: For Track

/// <summary>
/// This class is responsible for importing field data from an ISO XML file.
/// It uses the `IsoXmlParserHelpers` to extract various components such as origin, boundaries, headland, and guidance lines.
/// For Curves, it also uses CurveCABTools to match CABCurve functionality.
/// This class is stateless and does not hold any references to the application state.
/// We keep that CurveCABTools class until we decoupled the CABCurve functionality from the UI.
/// </summary>
public class IsoXmlFieldImporter
{
    private readonly XmlNodeList _fieldParts;
    private readonly ApplicationModel _appModel;

    public IsoXmlFieldImporter(XmlNodeList fieldParts, ApplicationModel appModel)
    {
        _fieldParts = fieldParts;
        _appModel = appModel;
    }

    public bool TryGetOrigin(out Wgs84 origin) =>
        IsoXmlParserHelpers.TryExtractOrigin(_fieldParts, out origin);

    public List<CBoundaryList> GetBoundaries() =>
        IsoXmlParserHelpers.ParseBoundaries(_fieldParts, _appModel);

    public List<vec3> GetHeadland() =>
        IsoXmlParserHelpers.ParseHeadland(_fieldParts, _appModel);

    public List<Track> GetGuidanceLines() => // BIG BANG Step 2: CTrk → Track
        IsoXmlParserHelpers.ParseAllGuidanceLines(_fieldParts, _appModel);
}
