using Aoc.Abstractions.Puzzles;
using Aoc.Application.Execution;

namespace Aoc.Application.Tests.Execution;

/// <summary>
/// Contains automated checks for <see cref="PuzzlePartResult"/>.
/// </summary>
public sealed class PuzzlePartResultTests
{
    /// <summary>
    /// Verifies that valid result values are stored correctly.
    /// </summary>
    [Fact]
    public void ConstructorWhenValuesAreValidSetsProperties()
    {
        // Arrange.
        var duration = TimeSpan.FromMilliseconds(12.5);

        // Act.
        var result = new PuzzlePartResult(
            puzzlePart: PuzzlePart.PartOne,
            answer: "42",
            duration: duration);

        // Assert.
        Assert.Equal(PuzzlePart.PartOne, result.PuzzlePart);
        Assert.Equal("42", result.Answer);
        Assert.Equal(duration, result.Duration);
    }

    /// <summary>
    /// Verifies that an individual result cannot represent Both
    /// or an unsupported enum value.
    /// </summary>
    /// <param name="puzzlePart">An invalid individual puzzle part.</param>
    [Theory]
    [InlineData(PuzzlePart.Both)]
    [InlineData((PuzzlePart)999)]
    public void ConstructorWhenPuzzlePartIsNotConcreteThrowsArgumentOutOfRangeException(
        PuzzlePart puzzlePart)
    {
        // Act.
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new PuzzlePartResult(
                puzzlePart: puzzlePart,
                answer: "42",
                duration: TimeSpan.Zero));

        // Assert.
        Assert.Equal("puzzlePart", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a null answer is rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenAnswerIsNullThrowsArgumentNullException()
    {
        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => new PuzzlePartResult(
                puzzlePart: PuzzlePart.PartOne,
                answer: null!,
                duration: TimeSpan.Zero));

        // Assert.
        Assert.Equal("answer", exception.ParamName);
    }

    /// <summary>
    /// Verifies that empty or whitespace-only answers are rejected.
    /// </summary>
    /// <param name="answer">An invalid puzzle answer.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void ConstructorWhenAnswerIsEmptyOrWhitespaceThrowsArgumentException(
        string answer)
    {
        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => new PuzzlePartResult(
                puzzlePart: PuzzlePart.PartOne,
                answer: answer,
                duration: TimeSpan.Zero));

        // Assert.
        Assert.Equal("answer", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a negative execution duration is rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenDurationIsNegativeThrowsArgumentOutOfRangeException()
    {
        // Act.
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new PuzzlePartResult(
                puzzlePart: PuzzlePart.PartOne,
                answer: "42",
                duration: TimeSpan.FromMilliseconds(-1)));

        // Assert.
        Assert.Equal("duration", exception.ParamName);
    }
}