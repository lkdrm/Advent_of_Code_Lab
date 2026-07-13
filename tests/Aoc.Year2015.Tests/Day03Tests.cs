using Aoc.Year2015.Puzzles;

namespace Aoc.Year2015.Tests;

/// <summary>
/// Contains automated checks for Advent of Code 2015, Day 03.
/// </summary>
public sealed class Day03Tests
{
    private readonly Day03 _puzzle = new();

    /// <summary>
    /// Verifies the number of unique houses visited by Santa
    /// for the official Part One examples.
    /// </summary>
    /// <param name="input">The movement directions followed by Santa.</param>
    /// <param name="expectedAnswer">
    /// The expected number of unique houses visited.
    /// </param>
    [Theory]
    [InlineData(">", "2")]
    [InlineData("^>v<", "4")]
    [InlineData("^v^v^v^v^v", "2")]
    public void SolvePartOneWhenDirectionsAreProvidedReturnsVisitedHouseCount(string input, string expectedAnswer)
    {
        // Act.
        var result = _puzzle.SolvePartOne(input);

        // Assert.
        Assert.Equal(expectedAnswer, result);
    }

    /// <summary>
    /// Verifies the number of unique houses visited by Santa and Robo-Santa
    /// for the official Part Two examples.
    /// </summary>
    /// <param name="input">
    /// The movement directions shared by Santa and Robo-Santa.
    /// </param>
    /// <param name="expectedAnswer">
    /// The expected number of unique houses visited.
    /// </param>
    [Theory]
    [InlineData("^v", "3")]
    [InlineData("^>v<", "3")]
    [InlineData("^v^v^v^v^v", "11")]
    public void SolvePartTwoWhenDirectionsAreProvidedReturnsVisitedHouseCount(string input, string expectedAnswer)
    {
        // Act.
        var result = _puzzle.SolvePartTwo(input);

        // Assert.
        Assert.Equal(expectedAnswer, result);
    }


    /// <summary>
    /// Verifies that Part One rejects a missing puzzle input.
    /// </summary>
    [Fact]
    public void SolvePartOneWhenInputIsNullThrowsArgumentNullException()
    {
        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => _puzzle.SolvePartOne(null!));

        // Assert.
        Assert.Equal("input", exception.ParamName);
    }

    /// <summary>
    /// Verifies that Part Two rejects a missing puzzle input.
    /// </summary>
    [Fact]
    public void SolvePartTwoWhenInputIsNullThrowsArgumentNullException()
    {
        var puzzle = new Day02();

        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => puzzle.SolvePartTwo(null!));

        // Assert.
        Assert.Equal("input", exception.ParamName);
    }

    /// <summary>
    /// Verifies that Part One rejects empty or whitespace-only puzzle input.
    /// </summary>
    /// <param name="input">An empty or whitespace-only puzzle input.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\r\n")]
    public void SolvePartOneWhenInputIsEmptyOrWhitespaceThrowsArgumentException(
        string input)
    {
        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => _puzzle.SolvePartOne(input));

        // Assert.
        Assert.Equal("input", exception.ParamName);
    }

    /// <summary>
    /// Verifies that Part Two rejects empty or whitespace-only puzzle input.
    /// </summary>
    /// <param name="input">An empty or whitespace-only puzzle input.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\r\n")]
    public void SolvePartTwoWhenInputIsEmptyOrWhitespaceThrowsArgumentException(
        string input)
    {
        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => _puzzle.SolvePartTwo(input));

        // Assert.
        Assert.Equal("input", exception.ParamName);
    }
}