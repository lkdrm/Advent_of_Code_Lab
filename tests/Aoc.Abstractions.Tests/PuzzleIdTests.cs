using Aoc.Abstractions.Puzzles;

namespace Aoc.Abstractions.Tests;

/// <summary>
/// Contains automated checks for <see cref="PuzzleId"/>.
/// </summary>
public sealed class PuzzleIdTests
{
    /// <summary>
    /// Verifies that valid year and day values are stored correctly.
    /// </summary>
    [Fact]
    public void ConstructorWhenValuesAreValidSetsYearAndDay()
    {
        // Act.
        var puzzleId = new PuzzleId(year: 2015, day: 3);

        // Assert.
        Assert.Equal(2015, puzzleId.Year);
        Assert.Equal(3, puzzleId.Day);
    }

    /// <summary>
    /// Verifies that years before the first Advent of Code event are rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenYearIsBefore2015ThrowsArgumentOutOfRangeException()
    {
        // Act.
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new PuzzleId(year: 2014, day: 1));

        // Assert.
        Assert.Equal("year", exception.ParamName);
    }

    /// <summary>
    /// Verifies that puzzle days outside the supported range are rejected.
    /// </summary>
    /// <param name="day">An invalid Advent of Code day number.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(26)]
    public void ConstructorWhenDayIsOutsideSupportedRangeThrowsArgumentOutOfRangeException(
        int day)
    {
        // Act.
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new PuzzleId(year: 2015, day: day));

        // Assert.
        Assert.Equal("day", exception.ParamName);
    }

    /// <summary>
    /// Verifies that identifiers with the same year and day are value-equal.
    /// </summary>
    [Fact]
    public void EqualityWhenYearAndDayAreEqualReturnsTrue()
    {
        // Arrange.
        var first = new PuzzleId(year: 2015, day: 3);
        var second = new PuzzleId(year: 2015, day: 3);

        // Assert.
        Assert.Equal(first, second);
        Assert.True(first == second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    /// <summary>
    /// Verifies that identifiers with different values are not equal.
    /// </summary>
    /// <param name="year">The year of the second identifier.</param>
    /// <param name="day">The day of the second identifier.</param>
    [Theory]
    [InlineData(2016, 3)]
    [InlineData(2015, 4)]
    public void EqualityWhenYearOrDayIsDifferentReturnsFalse(
        int year,
        int day)
    {
        // Arrange.
        var first = new PuzzleId(year: 2015, day: 3);
        var second = new PuzzleId(year: year, day: day);

        // Assert.
        Assert.NotEqual(first, second);
        Assert.True(first != second);
    }

    /// <summary>
    /// Verifies the human-readable identifier format used by the CLI.
    /// </summary>
    /// <param name="year">The Advent of Code event year.</param>
    /// <param name="day">The puzzle day number.</param>
    /// <param name="expected">The expected formatted identifier.</param>
    [Theory]
    [InlineData(2015, 1, "2015/Day 01")]
    [InlineData(2015, 9, "2015/Day 09")]
    [InlineData(2026, 25, "2026/Day 25")]
    public void ToStringReturnsYearAndZeroPaddedDay(
        int year,
        int day,
        string expected)
    {
        // Arrange.
        var puzzleId = new PuzzleId(year, day);

        // Act.
        var result = puzzleId.ToString();

        // Assert.
        Assert.Equal(expected, result);
    }
}