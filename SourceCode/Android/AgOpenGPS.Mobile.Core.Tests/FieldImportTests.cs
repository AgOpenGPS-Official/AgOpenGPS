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
