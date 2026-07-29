using Aoc.Year2015.Puzzles;

namespace Aoc.Year2015.Tests;

/// <summary>
/// Contains automated checks for Advent of Code 2015, Day 04.
/// </summary>
public sealed class Day04Tests
{
    private readonly Day04 _puzzle = new();

    /// <summary>
    /// Verifies that Part One returns the lowest AdventCoin number
    /// for the official puzzle examples.
    /// </summary>
    /// <param name="input">The secret key used to generate hashes.</param>
    /// <param name="expectedAnswer">
    /// The expected lowest matching positive number.
    /// </param>
    [Theory]
    [InlineData("abcdef", "609043")]
    [InlineData("pqrstuv", "1048970")]
    public void SolvePartOneWhenSecretKeyIsProvidedReturnsLowestAdventCoinNumber(
        string input,
        string expectedAnswer)
    {
        // Act.
        var result = _puzzle.SolvePartOne(input);

        // Assert.
        Assert.Equal(expectedAnswer, result);
    }

    /// <summary>
    /// Verifies that Part Two returns the lowest AdventCoin number
    /// whose MD5 hash starts with six hexadecimal zeroes.
    /// </summary>
    [Fact]
    public void SolvePartTwoWhenSecretKeyIsProvidedReturnsLowestAdventCoinNumber()
    {
        // Act.
        var result = _puzzle.SolvePartTwo("abcdef");

        // Assert.
        Assert.Equal("6742839", result);
    }

    /// <summary>
    /// Verifies that Part One rejects a missing secret key.
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
    /// Verifies that Part Two rejects a missing secret key.
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
    /// Verifies that Part One rejects an empty or whitespace-only secret key.
    /// </summary>
    /// <param name="input">An invalid secret key.</param>
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
    /// Verifies that Part Two rejects an empty or whitespace-only secret key.
    /// </summary>
    /// <param name="input">An invalid secret key.</param>
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