using System;
using AgOpenGPS.Mobile.Core.Guidance;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class FieldImportTests
    {
        // A made-up 100 x 100 m square (not a real parcel).
        private const string Square = "[[15.0000,45.0000],[15.0012716,45.0000],[15.0012716,45.0008993],[15.0000,45.0008993],[15.0000,45.0000]]";

        [Test]
        public void GeoJson_FeatureCollection_WithName()
        {
            string json = "{\"type\":\"FeatureCollection\",\"features\":[{\"type\":\"Feature\",\"properties\":{\"name\":\"Test square\"}," +
                          "\"geometry\":{\"type\":\"Polygon\",\"coordinates\":[" + Square + "]}}]}";
            var fields = FieldImport.Parse(json, "file");
            Assert.That(fields, Has.Count.EqualTo(1));
            Assert.That(fields[0].Name, Is.EqualTo("Test square"));
            Assert.That(fields[0].Boundary, Has.Count.EqualTo(4), "closing point dropped");
            Assert.That(fields[0].Boundary[1].Longitude, Is.EqualTo(15.0012716), "GeoJSON is longitude first");

            var f = new Field { Name = "x", Boundary = fields[0].Boundary };
            Assert.That(f.AreaHectares, Is.EqualTo(1.0).Within(0.01));
        }

        [Test]
        public void GeoJson_BareMultiPolygon_UsesFallbackNames()
        {
            string json = "{\"type\":\"MultiPolygon\",\"coordinates\":[[" + Square + "],[" + Square + "]]}";
            var fields = FieldImport.Parse(json, "parcels");
            Assert.That(fields, Has.Count.EqualTo(2));
            Assert.That(fields[0].Name, Is.EqualTo("parcels 1"));
        }

        [Test]
        public void Kml_PlacemarkPolygon()
        {
            string kml = "<?xml version=\"1.0\"?><kml xmlns=\"http://www.opengis.net/kml/2.2\"><Document><Placemark><name><![CDATA[North & field]]></name>" +
                         "<Polygon><outerBoundaryIs><LinearRing><coordinates>\n 15,45,0 15.0012716,45,0\n15.0012716,45.0008993,0 15,45.0008993,0 15,45,0\n" +
                         "</coordinates></LinearRing></outerBoundaryIs><innerBoundaryIs><LinearRing><coordinates>15.0005,45.0004 15.0006,45.0004 15.0006,45.0005</coordinates></LinearRing></innerBoundaryIs>" +
                         "</Polygon></Placemark></Document></kml>";
            var fields = FieldImport.Parse(kml, "file");
            Assert.That(fields, Has.Count.EqualTo(1), "the hole is not a field");
            Assert.That(fields[0].Name, Is.EqualTo("North & field"));
            Assert.That(fields[0].Boundary, Has.Count.EqualTo(4));
            Assert.That(fields[0].Boundary[2].Latitude, Is.EqualTo(45.0008993));
        }

        [Test]
        public void ProjectedCoordinates_GiveAClearError()
        {
            string json = "{\"type\":\"Polygon\",\"coordinates\":[[[500000.0,5000000.0],[500100.0,5000000.0],[500100.0,5000100.0],[500000.0,5000000.0]]]}";
            var ex = Assert.Throws<FormatException>(() => FieldImport.Parse(json, "x"));
            Assert.That(ex.Message, Does.Contain("WGS84"));
        }

        [Test]
        public void NotAFieldFile()
        {
            Assert.Throws<FormatException>(() => FieldImport.Parse("hello", "x"));
            Assert.Throws<FormatException>(() => FieldImport.Parse("{\"type\":\"Point\",\"coordinates\":[15,45]}", "x"));
            Assert.Throws<FormatException>(() => FieldImport.Parse("{ broken", "x"));
        }
    }
}

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class FieldExportTests
    {
        private static readonly LocalPlane Plane = new LocalPlane(45.0, 15.0);

        private static TrackPoint P(double n, double e)
        {
            Plane.ToWgs84(new GeoPoint(n, e), out double lat, out double lon);
            return new TrackPoint(lat, lon);
        }

        [Test]
        public void ExportThenImport_RestoresFieldsBoundariesAndLines()
        {
            var north = new Field { Name = "North / big", Boundary = { P(0, 0), P(0, 200), P(150, 200), P(150, 0) } };
            var empty = new Field { Name = "No boundary yet" };
            var straight = new AbLine { Name = "Edge", FieldId = north.Id, NudgeMeters = 0.05 };
            TrackPoint a = P(5, 5), b = P(145, 5);
            straight.LatitudeA = a.Latitude; straight.LongitudeA = a.Longitude; straight.LatitudeB = b.Latitude; straight.LongitudeB = b.Longitude;
            var curve = new AbLine { Name = "Ditch", FieldId = north.Id, Kind = LineKind.Curve, Points = { P(0, 10), P(50, 15), P(100, 25), P(150, 40) } };
            curve.LatitudeA = curve.Points[0].Latitude; curve.LongitudeA = curve.Points[0].Longitude;
            curve.LatitudeB = curve.Points[3].Latitude; curve.LongitudeB = curve.Points[3].Longitude;
            var lonely = new AbLine { Name = "Headland", FieldId = empty.Id };
            lonely.LatitudeA = a.Latitude; lonely.LongitudeA = a.Longitude; lonely.LatitudeB = b.Latitude; lonely.LongitudeB = b.Longitude;

            string json = FieldExport.ToGeoJson(new[] { north, empty },
                id => id == north.Id ? new[] { straight, curve } : new[] { lonely });
            Assert.That(json, Does.Contain("\"type\": \"FeatureCollection\""));

            var back = FieldImport.Parse(json, "file");
            Assert.That(back, Has.Count.EqualTo(2));

            ImportedField n = back.Find(f => f.Name == "North / big");
            Assert.That(n.Boundary, Has.Count.EqualTo(4), "closing point dropped again");
            Assert.That(new Field { Boundary = n.Boundary }.AreaHectares, Is.EqualTo(north.AreaHectares).Within(1e-4));
            Assert.That(n.Lines, Has.Count.EqualTo(2));
            AbLine s = n.Lines.Find(l => l.Name == "Edge");
            Assert.That(s.Kind, Is.EqualTo(LineKind.Straight));
            Assert.That(s.NudgeMeters, Is.EqualTo(0.05));
            Assert.That(s.LengthMeters, Is.EqualTo(straight.LengthMeters).Within(0.01));
            AbLine c = n.Lines.Find(l => l.Name == "Ditch");
            Assert.That(c.Kind, Is.EqualTo(LineKind.Curve));
            Assert.That(c.Points, Has.Count.EqualTo(4));

            ImportedField e = back.Find(f => f.Name == "No boundary yet");
            Assert.That(e.Boundary, Is.Empty);
            Assert.That(e.Lines, Has.Count.EqualTo(1));
            Assert.That(e.Lines[0].Name, Is.EqualTo("Headland"));
        }

        [Test]
        public void OtherLineStrings_AreIgnored_AndFileNamesAreSafe()
        {
            string json = "{\"type\":\"FeatureCollection\",\"features\":[" +
                "{\"type\":\"Feature\",\"properties\":{\"name\":\"F\"},\"geometry\":{\"type\":\"Polygon\",\"coordinates\":[[[15,45],[15.001,45],[15.001,45.001],[15,45]]]}}," +
                "{\"type\":\"Feature\",\"properties\":{\"name\":\"Road\"},\"geometry\":{\"type\":\"LineString\",\"coordinates\":[[15,45],[15.001,45.001]]}}]}";
            var back = FieldImport.Parse(json, "x");
            Assert.That(back[0].Lines, Is.Empty, "a road is not an A-B line");
            Assert.That(FieldExport.FileName("Polje: sjever/1"), Is.EqualTo("Polje_ sjever_1.geojson"));
            Assert.That(FieldExport.FileName(""), Is.EqualTo("fields.geojson"));
        }
    }
}
