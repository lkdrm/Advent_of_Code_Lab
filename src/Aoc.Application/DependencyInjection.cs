using Aoc.Application.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace Aoc.Application;

/// <summary>
/// Contains dependency-injection registrations for application-layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers application services required to execute Advent of Code puzzles.
    /// </summary>
    /// <param name="services">
    /// The dependency-injection service collection.
    /// </param>
    /// <returns>
    /// The same service collection so registrations can be chained fluently.
    /// </returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPuzzleExecutionService, PuzzleExecutionService>();

        return services;
    }
}
