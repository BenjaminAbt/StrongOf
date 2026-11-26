// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

public class GeoCoordinateTests
{
    [Fact]
    public void DistanceOnEquatorMatchesOneDegreeOfLongitude()
    {
        GeoCoordinate origin = GeoCoordinate.From(0, 0);
        GeoCoordinate east = GeoCoordinate.From(0, 1);
        Assert.Equal(0, origin.DistanceTo(origin));
        Assert.InRange(origin.DistanceTo(east), 111.19, 111.20);
        Assert.Equal(east.DistanceTo(origin), origin.DistanceTo(east), 8);
        Assert.Equal("0,1", east.ToString());
    }

    [Theory]
    [InlineData(90, 180, true)]
    [InlineData(-90, -180, true)]
    [InlineData(91, 0, false)]
    [InlineData(0, -181, false)]
    public void BothComponentsMustBeWithinRange(int latitude, int longitude, bool expected)
    {
        GeoCoordinate coordinate = GeoCoordinate.From(latitude, longitude);
        Assert.Equal(expected, coordinate.IsValidRange());
        Assert.True(coordinate.Latitude.Clamp().IsValidFormat());
        Assert.True(coordinate.Longitude.Clamp().IsValidFormat());
    }
}
