using Aoc.Year2015.Puzzles;

namespace Aoc.Year2015.Tests;

/// <summary>
/// Contains automated checks for Advent of Code 2015, Day 01.
/// </summary>
public sealed class Day01Tests
{
    private readonly Day01 _puzzle = new();

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
    public void SolvePartOneReturnsExpectedFinalFloor(string input, string expectedFloor)
    {
        // Act: execute only Part One with a known input.
        var actualFloor = _puzzle.SolvePartOne(input);

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
    public void SolvePartTwoReturnsFirstBasementPosition(string input, string expectedPosition)
    {
        // Act.
        var actualPosition = _puzzle.SolvePartTwo(input);

        // Assert.
        Assert.Equal(expectedPosition, actualPosition);
    }

    /// <summary>
    /// Verifies that Part One rejects a missing input.
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
    /// Verifies that Part Two rejects a missing input.
    /// </summary>
    [Fact]
    public void SolvePartTwoWhenInputIsNullThrowsArgumentNullException()
    {
        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => _puzzle.SolvePartTwo(null!));

        // Assert.
        Assert.Equal("input", exception.ParamName);
    }

    /// <summary>
    /// Verifies that trailing file whitespace does not change
    /// the first basement position.
    /// </summary>
    [Fact]
    public void SolvePartTwoWhenInputHasTrailingWhitespaceReturnsCorrectPosition()
    {
        // Act.
        var result = _puzzle.SolvePartTwo(")\r\n");

        // Assert.
        Assert.Equal("1", result);
    }

    /// <summary>
    /// Verifies that Part One rejects empty or whitespace-only input.
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
    /// Verifies that Part One rejects empty or whitespace-only input.
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