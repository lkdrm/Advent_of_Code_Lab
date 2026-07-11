using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Application.Execution;

namespace Aoc.Application.Tests.Execution;

/// <summary>
/// Contains automated checks for <see cref="PuzzleRunResult"/>.
/// </summary>
public sealed class PuzzleRunResultTests
{
    private readonly PuzzleMetadata _metadata = new(
        id: new PuzzleId(2015, 3),
        title: "Test Puzzle");

    private readonly PuzzlePartResult _partOneResult = new(
        puzzlePart: PuzzlePart.PartOne,
        answer: "part-one-answer",
        duration: TimeSpan.FromMilliseconds(1));

    private readonly PuzzlePartResult _partTwoResult = new(
        puzzlePart: PuzzlePart.PartTwo,
        answer: "part-two-answer",
        duration: TimeSpan.FromMilliseconds(2));

    /// <summary>
    /// Verifies that valid run information is stored in its original order.
    /// </summary>
    [Fact]
    public void ConstructorWhenValuesAreValidSetsProperties()
    {
        // Arrange.
        PuzzlePartResult[] partResults =
        [
            _partOneResult,
            _partTwoResult,
        ];

        // Act.
        var result = new PuzzleRunResult(
            puzzleMetadata: _metadata,
            inputKind: PuzzleInputKind.Demo,
            partResults: partResults);

        // Assert.
        Assert.Same(_metadata, result.PuzzleMetadata);
        Assert.Equal(PuzzleInputKind.Demo, result.PuzzleInputKind);

        Assert.Collection(
            result.PartResults,
            partOne => Assert.Same(_partOneResult, partOne),
            partTwo => Assert.Same(_partTwoResult, partTwo));
    }

    /// <summary>
    /// Verifies that missing puzzle metadata is rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenMetadataIsNullThrowsArgumentNullException()
    {
        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => new PuzzleRunResult(
                puzzleMetadata: null!,
                inputKind: PuzzleInputKind.Demo,
                partResults: [_partOneResult]));

        // Assert.
        Assert.Equal("puzzleMetadata", exception.ParamName);
    }

    /// <summary>
    /// Verifies that unsupported input-kind values are rejected.
    /// </summary>
    /// <param name="inputKind">An unsupported input-kind value.</param>
    [Theory]
    [InlineData((PuzzleInputKind)0)]
    [InlineData((PuzzleInputKind)999)]
    public void ConstructorWhenInputKindIsInvalidThrowsArgumentOutOfRangeException(
        PuzzleInputKind inputKind)
    {
        // Act.
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new PuzzleRunResult(
                puzzleMetadata: _metadata,
                inputKind: inputKind,
                partResults: [_partOneResult]));

        // Assert.
        Assert.Equal("inputKind", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a missing part-results collection is rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenPartResultsAreNullThrowsArgumentNullException()
    {
        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => new PuzzleRunResult(
                puzzleMetadata: _metadata,
                inputKind: PuzzleInputKind.Demo,
                partResults: null!));

        // Assert.
        Assert.Equal("partResults", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a puzzle run must contain at least one part result.
    /// </summary>
    [Fact]
    public void ConstructorWhenPartResultsAreEmptyThrowsArgumentException()
    {
        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => new PuzzleRunResult(
                puzzleMetadata: _metadata,
                inputKind: PuzzleInputKind.Demo,
                partResults: []));

        // Assert.
        Assert.Equal("partResults", exception.ParamName);
    }

    /// <summary>
    /// Verifies that null entries inside the results collection are rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenPartResultsContainNullThrowsArgumentException()
    {
        // Arrange.
        PuzzlePartResult[] partResults =
        [
            _partOneResult,
            null!,
        ];

        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => new PuzzleRunResult(
                puzzleMetadata: _metadata,
                inputKind: PuzzleInputKind.Demo,
                partResults: partResults));

        // Assert.
        Assert.Equal("partResults", exception.ParamName);
    }

    /// <summary>
    /// Verifies that one run cannot contain duplicate puzzle parts.
    /// </summary>
    [Fact]
    public void ConstructorWhenPartResultsContainDuplicatesThrowsArgumentException()
    {
        // Arrange.
        var duplicateResult = new PuzzlePartResult(
            puzzlePart: PuzzlePart.PartOne,
            answer: "another-answer",
            duration: TimeSpan.Zero);

        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => new PuzzleRunResult(
                puzzleMetadata: _metadata,
                inputKind: PuzzleInputKind.Demo,
                partResults:
                [
                    _partOneResult,
                    duplicateResult,
                ]));

        // Assert.
        Assert.Equal("partResults", exception.ParamName);
    }

    /// <summary>
    /// Verifies that modifying the source collection does not modify
    /// an already-created run result.
    /// </summary>
    [Fact]
    public void ConstructorWhenSourceCollectionChangesKeepsOriginalSnapshot()
    {
        // Arrange.
        var sourceResults = new List<PuzzlePartResult>
        {
            _partOneResult,
        };

        var result = new PuzzleRunResult(
            puzzleMetadata: _metadata,
            inputKind: PuzzleInputKind.Demo,
            partResults: sourceResults);

        // Act.
        sourceResults.Add(_partTwoResult);

        // Assert.
        var storedResult = Assert.Single(result.PartResults);
        Assert.Same(_partOneResult, storedResult);
    }
}