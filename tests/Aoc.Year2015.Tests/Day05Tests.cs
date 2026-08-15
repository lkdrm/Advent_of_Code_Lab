using Aoc.Year2015.Puzzles;

namespace Aoc.Year2015.Tests;

/// <summary>
/// Contains automated checks for Advent of Code 2015, Day 05.
/// </summary>
public sealed class Day05Tests
{
    private readonly Day05 _puzzle = new();

    /// <summary>
    /// Verifies that a string satisfying every Part One rule is counted as nice.
    /// </summary>
    [Fact]
    public void SolvePartOneWhenStringMeetsAllRulesReturnsOne()
    {
        // Arrange.
        const string input = "aaa";

        // Act.
        var result = _puzzle.SolvePartOne(input);

        // Assert.
        Assert.Equal("1", result);
    }

    /// <summary>
    /// Verifies that the letter U is treated as a vowel.
    /// </summary>
    [Fact]
    public void SolvePartOneWhenVowelsIncludeUReturnsOne()
    {
        // Arrange.
        const string input = "aeuux";

        // Act.
        var result = _puzzle.SolvePartOne(input);

        // Assert.
        Assert.Equal("1", result);
    }

    /// <summary>
    /// Verifies that a string containing a forbidden pair is not counted as nice.
    /// </summary>
    [Fact]
    public void SolvePartOneWhenInputContainsForbiddenPairReturnsZero()
    {
        // Arrange.
        const string input = "haegwjzuvuyypxyu";

        // Act.
        var result = _puzzle.SolvePartOne(input);

        // Assert.
        Assert.Equal("0", result);
    }

    /// <summary>
    /// Verifies that a string without consecutive duplicate letters is not counted as nice.
    /// </summary>
    [Fact]
    public void SolvePartOneWhenInputHasNoConsecutiveDuplicateReturnsZero()
    {
        // Arrange.
        const string input = "jchzalrnumimnmhp";

        // Act.
        var result = _puzzle.SolvePartOne(input);

        // Assert.
        Assert.Equal("0", result);
    }

    /// <summary>
    /// Verifies that Part Two classifies strings according to both niceness rules.
    /// </summary>
    /// <param name="input">The candidate string.</param>
    /// <param name="expected">The expected number of nice strings.</param>
    [Theory]
    [InlineData("qjhvhtzxzqqjkmpb", "1")]
    [InlineData("xxyxx", "1")]
    [InlineData("uurcxstgmygtbstg", "0")]
    [InlineData("ieodomkazucvgmuy", "0")]
    public void SolvePartTwoWhenCandidateIsProvidedReturnsExpectedNiceStringCount(string input, string expected)
    {
        // Act.
        var result = _puzzle.SolvePartTwo(input);

        // Assert.
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that Part One counts nice strings across multiple input lines.
    /// </summary>
    [Fact]
    public void SolvePartOneWhenMultipleCandidatesAreProvidedReturnsNiceStringCount()
    {
        // Arrange.
        
        var input = string.Join(
            Environment.NewLine,
            "ugknbfddgicrmopn",
            "aaa",
            "jchzalrnumimnmhp",
            "haegwjzuvuyypxyu",
            "dvszwmarrgswjxmb");

        // Act.
        var result = _puzzle.SolvePartOne(input);

        // Assert.
        Assert.Equal("2", result);
    }

    /// <summary>
    /// Verifies that Part Two counts nice strings across multiple input lines.
    /// </summary>
    [Fact]
    public void SolvePartTwoWhenMultipleCandidatesAreProvidedReturnsNiceStringCount()
    {
        // Arrange.

        var input = string.Join(
            Environment.NewLine,
            "qjhvhtzxzqqjkmpb",
            "xxyxx",
            "uurcxstgmygtbstg",
            "ieodomkazucvgmuy");

        // Act.
        var result = _puzzle.SolvePartTwo(input);

        // Assert.
        Assert.Equal("2", result);
    }
}
