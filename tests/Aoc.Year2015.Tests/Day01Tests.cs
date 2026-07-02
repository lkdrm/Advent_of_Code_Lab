using Aoc.Year2015.Puzzles;

namespace Aoc.Year2015.Tests;

/// <summary>
/// Contains automated checks for Advent of Code 2015, Day 01.
/// </summary>
public sealed class Day01Tests
{
    /// <summary>
    /// Verifies that Part One calculates Santa's final floor correctly
    /// for the examples provided in the Advent of Code puzzle description.
    /// </summary>
    /// <param name="input">A sequence of opening and closing parentheses.</param>
    /// <param name="expectedFloor">The expected final floor.</param>
    [Theory]
    [InlineData("(())", "0")]
    [InlineData("()()", "0")]
    [InlineData("(((", "3")]
    [InlineData("(()(()(", "3")]
    [InlineData("))(((((", "3")]
    [InlineData("())", "-1")]
    [InlineData("))(", "-1")]
    [InlineData(")))", "-3")]
    [InlineData(")())())", "-3")]
    public void SolvePartOne_ReturnsExpectedFinalFloor(string input, string expectedFloor)
    {
        // Arrange: create the puzzle implementation we want to test.
        var puzzle = new Day01();

        // Act: execute only Part One with a known input.
        var actualFloor = puzzle.SolvePartOne(input);

        // Assert: compare the actual result with the expected AoC answer.
        Assert.Equal(expectedFloor, actualFloor);
    }

    /// <summary>
    /// Verifies that Part Two returns the first one-based position
    /// where Santa enters the basement.
    /// </summary>
    /// <param name="input">A sequence of opening and closing parentheses.</param>
    /// <param name="expectedPosition">
    /// The expected first position where the floor reaches -1.
    /// </param>
    [Theory]
    [InlineData(")", "1")]
    [InlineData("()())", "5")]
    public void SolvePartTwo_ReturnsFirstBasementPosition(string input, string expectedPosition)
    {
        // Arrange.
        var puzzle = new Day01();

        // Act.
        var actualPosition = puzzle.SolvePartTwo(input);

        // Assert.
        Assert.Equal(expectedPosition, actualPosition);
    }
}