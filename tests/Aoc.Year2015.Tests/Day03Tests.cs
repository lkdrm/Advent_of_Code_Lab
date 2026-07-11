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
}