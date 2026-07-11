using Aoc.Abstractions.Puzzles;
using Aoc.Year2015.Puzzles;
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
    public static IServiceCollection AddYear2015Puzzles(this IServiceCollection services)
    {
        services.AddSingleton<IPuzzle, Day01>();
        services.AddSingleton<IPuzzle, Day02>();
        services.AddSingleton<IPuzzle, Day03>();
        return services;
    }
}
