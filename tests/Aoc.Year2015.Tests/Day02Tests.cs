using Aoc.Year2015.Puzzles;

namespace Aoc.Year2015.Tests;

/// <summary>
/// Contains automated checks for Advent of Code 2015, Day 02.
/// </summary>
public sealed class Day02Tests
{
    private readonly Day02 _puzzle = new();

    /// <summary>
    /// Verifies the wrapping-paper calculation for the official single-present examples.
    /// </summary>
    /// <param name="input">One present's dimensions in length-width-height format.</param>
    /// <param name="expectedAnswer">The expected total amount of wrapping paper.</param>
    [Theory]
    [InlineData("2x3x4", "58")]
    [InlineData("1x1x10", "43")]
    public void SolvePartOneWhenInputContainsOnePresentReturnsRequiredWrappingPaper(string input, string expectedAnswer)
    {
        // Act.
        var result = _puzzle.SolvePartOne(input);

        // Assert.
        Assert.Equal(expectedAnswer, result);
    }

    /// <summary>
    /// Verifies that Part One sums wrapping-paper requirements for multiple presents.
    /// </summary>
    [Fact]
    public void SolvePartOneWhenInputContainsMultiplePresentsReturnsTotalWrappingPaper()
    {
        // Arrange.
        const string input = "2x3x4\r\n1x1x10\r\n";

        // Act.
        var result = _puzzle.SolvePartOne(input);

        // Assert.
        Assert.Equal("101", result);
    }

    /// <summary>
    /// Verifies the ribbon calculation for the official single-present examples.
    /// </summary>
    /// <param name="input">One present's dimensions in length-width-height format.</param>
    /// <param name="expectedAnswer">The expected total ribbon length.</param>
    [Theory]
    [InlineData("2x3x4", "34")]
    [InlineData("1x1x10", "14")]
    public void SolvePartTwoWhenInputContainsOnePresentReturnsRequiredRibbon(string input, string expectedAnswer)
    {
        // Act.
        var result = _puzzle.SolvePartTwo(input);

        // Assert.
        Assert.Equal(expectedAnswer, result);
    }

    /// <summary>
    /// Verifies that Part Two sums ribbon requirements for multiple presents.
    /// </summary>
    [Fact]
    public void SolvePartTwoWhenInputContainsMultiplePresentsReturnsTotalRibbon()
    {
        // Arrange.
        const string input = "2x3x4\r\n1x1x10\r\n";

        // Act.
        var result = _puzzle.SolvePartTwo(input);

        // Assert.
        Assert.Equal("48", result);
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