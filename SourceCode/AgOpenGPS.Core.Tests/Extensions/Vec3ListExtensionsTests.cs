using AgOpenGPS.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Tests.Extensions
{
    [TestFixture]
    public class Vec3ListExtensionsTests
    {
        [Test]
        public void CalculateHeadings_StraightLineNorthSouth_HeadingsAreZero()
        {
            // Arrange - straight line going north (0 degrees)
            var points = new List<vec3>
            {
                new vec3(0, 0, 0),
                new vec3(0, 10, 0),
                new vec3(0, 20, 0)
            };

            // Act
            points.CalculateHeadings(false);

            // Assert
            foreach (var point in points)
            {
                Assert.That(point.heading, Is.EqualTo(0).Within(0.001),
                    $"Heading for point at ({point.easting}, {point.northing}) should be 0 (north)");
            }
        }

        [Test]
        public void CalculateHeadings_StraightLineEastWest_HeadingsArePIBy2()
        {
            // Arrange - straight line going east (90 degrees = PIBy2)
            var points = new List<vec3>
            {
                new vec3(0, 0, 0),
                new vec3(10, 0, 0),
                new vec3(20, 0, 0)
            };

            // Act
            points.CalculateHeadings(false);

            // Assert
            foreach (var point in points)
            {
                Assert.That(point.heading, Is.EqualTo(GeoMath.PIBy2).Within(0.001),
                    $"Heading for point at ({point.easting}, {point.northing}) should be PIBy2 (east)");
            }
        }

        [Test]
        public void CalculateHeadings_LoopPath_FirstAndLastPointsUseLoopLogic()
        {
            // Arrange - square path forming a loop
            var points = new List<vec3>
            {
                new vec3(0, 0, 0),    // SW corner
                new vec3(10, 0, 0),   // SE corner
                new vec3(10, 10, 0),  // NE corner
                new vec3(0, 10, 0)    // NW corner
            };

            // Act
            points.CalculateHeadings(true); // loop = true

            // Assert
            Assert.That(points.Count, Is.EqualTo(4), "Should have 4 points after calculation");

            // First point heading should consider last and second point (average)
            Assert.That(points[0].heading, Is.GreaterThan(0), "First point should have heading considering loop");
        }

        [Test]
        public void OffsetLine_StraightLineNorth_OffsetToEast()
        {
            // Arrange - straight line going north
            var points = new List<vec3>
            {
                new vec3(0, 0, 0),
                new vec3(0, 10, 0),
                new vec3(0, 20, 0)
            };

            // Act - offset 5 meters to the right (east)
            var offsetPoints = points.OffsetLine(5.0, 0.1, false);

            // Assert
            Assert.That(offsetPoints.Count, Is.GreaterThan(0), "Should have offset points");

            // All offset points should be to the east (positive easting)
            foreach (var point in offsetPoints)
            {
                Assert.That(point.easting, Is.GreaterThan(4.9),
                    $"Offset point should be ~5m east, but was at {point.easting}");
            }
        }

        [Test]
        public void OffsetLine_StraightLineNorth_OffsetToWest()
        {
            // Arrange - straight line going north
            var points = new List<vec3>
            {
                new vec3(0, 0, 0),
                new vec3(0, 10, 0),
                new vec3(0, 20, 0)
            };

            // Act - offset 5 meters to the left (west)
            var offsetPoints = points.OffsetLine(-5.0, 0.1, false);

            // Assert
            Assert.That(offsetPoints.Count, Is.GreaterThan(0), "Should have offset points");

            // All offset points should be to the west (negative easting)
            foreach (var point in offsetPoints)
            {
                Assert.That(point.easting, Is.LessThan(-4.9),
                    $"Offset point should be ~-5m west, but was at {point.easting}");
            }
        }

        [Test]
        public void OffsetLine_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var points = new List<vec3>();

            // Act
            var offsetPoints = points.OffsetLine(5.0, 0.1, false);

            // Assert
            Assert.That(offsetPoints.Count, Is.EqualTo(0), "Empty input should return empty output");
        }

        [Test]
        public void OffsetLine_MinimumDistanceRespected()
        {
            // Arrange - dense points
            var points = new List<vec3>();
            for (int i = 0; i < 100; i++)
            {
                points.Add(new vec3(0, i * 0.1, 0)); // points every 0.1m
            }

            double minDist = 1.0; // require 1m minimum spacing

            // Act
            var offsetPoints = points.OffsetLine(2.0, minDist, false);

            // Assert
            // Check spacing between consecutive points
            for (int i = 1; i < offsetPoints.Count; i++)
            {
                double dist = GeoMath.Distance(offsetPoints[i - 1], offsetPoints[i]);
                Assert.That(dist, Is.GreaterThanOrEqualTo(Math.Sqrt(minDist) * 0.9),
                    $"Distance between points {i - 1} and {i} should respect minimum distance");
            }
        }

        [Test]
        public void GeoMath_Distance_CalculatesCorrectly()
        {
            // Arrange - 3-4-5 triangle
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(3, 4, 0);

            // Act
            double dist = GeoMath.Distance(p1, p2);

            // Assert
            Assert.That(dist, Is.EqualTo(5.0).Within(0.001), "Should calculate pythagorean distance correctly");
        }

        [Test]
        public void GeoMath_DistanceSquared_CalculatesCorrectly()
        {
            // Arrange
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(3, 4, 0);

            // Act
            double distSq = GeoMath.DistanceSquared(p1, p2);

            // Assert
            Assert.That(distSq, Is.EqualTo(25.0).Within(0.001), "Should calculate squared distance correctly");
        }
    }
}
