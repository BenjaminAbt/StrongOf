// Copyright © Benjamin Abt 2025. All rights reserved.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Geography;

/// <summary>
/// Represents a geographic coordinate composed of a <see cref="Latitude"/> and a <see cref="Longitude"/>.
/// </summary>
/// <remarks>
/// Use this type when you need both coordinates together (e.g. a location on a map).
/// <see cref="Latitude"/> and <see cref="Longitude"/> may still be used independently when
/// only one axis is needed (e.g. sorting by latitude).
/// </remarks>
/// <example>
/// <code>
/// var berlin = new GeoCoordinate(new Latitude(52.52m), new Longitude(13.405m));
/// bool valid = berlin.IsValidRange();
/// double dist = berlin.DistanceTo(other);
/// </code>
/// </example>
/// <param name="Latitude">The latitude component (–90 to +90).</param>
/// <param name="Longitude">The longitude component (–180 to +180).</param>
[DebuggerDisplay("({Latitude.Value}, {Longitude.Value})")]
public sealed record GeoCoordinate(Latitude Latitude, Longitude Longitude)
{
    /// <summary>
    /// Creates a <see cref="GeoCoordinate"/> from raw decimal values.
    /// </summary>
    /// <param name="latitude">Latitude in decimal degrees.</param>
    /// <param name="longitude">Longitude in decimal degrees.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static GeoCoordinate From(decimal latitude, decimal longitude)
        => new(new Latitude(latitude), new Longitude(longitude));

    /// <summary>
    /// Determines whether both components are within their valid ranges.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Latitude.IsValidRange() && Longitude.IsValidRange();

    /// <summary>
    /// Calculates the approximate great-circle distance (in kilometres) to another coordinate
    /// using the Haversine formula.
    /// </summary>
    /// <param name="other">The target coordinate.</param>
    /// <returns>Distance in kilometres.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public double DistanceTo(GeoCoordinate other)
    {
        const double earthRadiusKm = 6371.0;

        double dLat = DegreesToRadians((double)(other.Latitude.Value - Latitude.Value));
        double dLon = DegreesToRadians((double)(other.Longitude.Value - Longitude.Value));

        double lat1 = DegreesToRadians((double)Latitude.Value);
        double lat2 = DegreesToRadians((double)other.Latitude.Value);

        double a = (Math.Sin(dLat / 2) * Math.Sin(dLat / 2))
                   + (Math.Cos(lat1) * Math.Cos(lat2)
                      * Math.Sin(dLon / 2) * Math.Sin(dLon / 2));

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadiusKm * c;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;

    /// <summary>
    /// Returns the coordinate in standard decimal-degrees notation: <c>lat,lon</c>.
    /// </summary>
    public override string ToString() => $"{Latitude.Value},{Longitude.Value}";
}
