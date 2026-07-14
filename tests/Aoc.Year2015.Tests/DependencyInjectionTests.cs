using Aoc.Abstractions.Puzzles;
using Aoc.Year2015.Puzzles;
using Microsoft.Extensions.DependencyInjection;

namespace Aoc.Year2015.Tests;

/// <summary>
/// Verifies dependency-injection registration for Advent of Code 2015 puzzles.
/// </summary>
public sealed class DependencyInjectionTests
{
    /// <summary>
    /// Verifies that every public puzzle implementation is registered exactly once.
    /// </summary>
    [Fact]
    public void AddYear2015PuzzlesWhenCalledRegistersEveryPuzzleExactlyOnce()
    {
        // Arrange.
        var services = new ServiceCollection();
        var expectedPuzzleTypes = GetPuzzleTypes();

        // Act.
        services.AddYear2015Puzzles();

        // Assert.
        var registrations = GetPuzzleRegistrations(services);

        Assert.Equal(expectedPuzzleTypes.Length, registrations.Length);
        Assert.All(
            registrations,
            registration => Assert.NotNull(registration.ImplementationType));

        var registeredPuzzleTypes = registrations
            .Select(registration => registration.ImplementationType!)
            .OrderBy(type => type.FullName)
            .ToArray();

        Assert.Equal(expectedPuzzleTypes, registeredPuzzleTypes);
    }

    /// <summary>
    /// Verifies that puzzle implementations use the singleton lifetime.
    /// </summary>
    [Fact]
    public void AddYear2015PuzzlesWhenCalledRegistersPuzzlesAsSingletons()
    {
        // Arrange.
        var services = new ServiceCollection();

        // Act.
        services.AddYear2015Puzzles();

        // Assert.
        var registrations = GetPuzzleRegistrations(services);

        Assert.All(
            registrations,
            registration => Assert.Equal(
                ServiceLifetime.Singleton,
                registration.Lifetime));
    }

    /// <summary>
    /// Verifies that registered puzzle implementations have unique identifiers.
    /// </summary>
    [Fact]
    public void AddYear2015PuzzlesWhenCalledRegistersPuzzlesWithUniqueIdentifiers()
    {
        // Arrange.
        var services = new ServiceCollection();

        // Act.
        services.AddYear2015Puzzles();

        // Assert.
        var puzzleIds = GetPuzzleRegistrations(services)
            .Select(registration => registration.ImplementationType!)
            .Select(type => (IPuzzle)Activator.CreateInstance(type)!)
            .Select(puzzle => puzzle.Metadata.Id)
            .ToArray();

        Assert.Equal(puzzleIds.Length, puzzleIds.Distinct().Count());
    }

    /// <summary>
    /// Verifies that a missing service collection is rejected.
    /// </summary>
    [Fact]
    public void AddYear2015PuzzlesWhenServicesIsNullThrowsArgumentNullException()
    {
        // Arrange.
        IServiceCollection services = null!;

        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => services.AddYear2015Puzzles());

        // Assert.
        Assert.Equal("services", exception.ParamName);
    }

    private static Type[] GetPuzzleTypes()
    {
        return typeof(Day01).Assembly
            .GetTypes()
            .Where(type =>
                type is { IsClass: true, IsAbstract: false, IsPublic: true }
                && typeof(IPuzzle).IsAssignableFrom(type))
            .OrderBy(type => type.FullName)
            .ToArray();
    }

    private static ServiceDescriptor[] GetPuzzleRegistrations(
        IServiceCollection services)
    {
        return services
            .Where(registration => registration.ServiceType == typeof(IPuzzle))
            .ToArray();
    }
}