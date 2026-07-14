using Aoc.Abstractions.Puzzles;
using Microsoft.Extensions.DependencyInjection;

namespace Aoc.Year2015;

/// <summary>
/// Contains dependency-injection registrations for Advent of Code 2015 puzzles.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all implemented Advent of Code 2015 puzzles.
    /// </summary>
    /// <param name="services">The dependency-injection service collection.</param>
    /// <returns>The same service collection so registrations can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> is <see langword="null"/>.
    /// </exception>
    public static IServiceCollection AddYear2015Puzzles(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.Scan(scan => scan
            .FromAssemblyOf<Year2015AssemblyMarker>()
            .AddClasses(classes => classes.AssignableTo<IPuzzle>())
            .As<IPuzzle>()
            .WithSingletonLifetime());

        return services;
    }
}
