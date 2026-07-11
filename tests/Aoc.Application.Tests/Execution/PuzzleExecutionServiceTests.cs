using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Application.Execution;
using Aoc.Application.Tests.Support;

namespace Aoc.Application.Tests.Execution;

/// <summary>
/// Contains automated checks for <see cref="PuzzleExecutionService"/>.
/// </summary>
public sealed class PuzzleExecutionServiceTests
{
    /// <summary>
    /// Verifies that selecting Part One executes only Part One
    /// and returns one result with the expected answer.
    /// </summary>
    [Fact]
    public async Task ExecuteAsyncWhenPartOneIsSelectedExecutesOnlyPartOne()
    {
        // Arrange.
        var puzzleId = new PuzzleId(year: 2015, day: 1);

        var puzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: puzzleId,
                title: "Fake Puzzle"),
            partOneAnswer: "part-one-answer",
            partTwoAnswer: "part-two-answer");

        var inputProvider = new FakePuzzleInputProvider(
            input: "demo-input");

        var service = new PuzzleExecutionService(
            puzzles: [puzzle],
            inputProvider: inputProvider);

        // Act.
        var result = await service.ExecuteAsync(
            id: puzzleId,
            puzzlePart: PuzzlePart.PartOne,
            inputKind: PuzzleInputKind.Demo,
            cancellationToken: CancellationToken.None);

        // Assert: result data.
        var partResult = Assert.Single(result.PartResults);

        Assert.Equal(PuzzlePart.PartOne, partResult.PuzzlePart);
        Assert.Equal("part-one-answer", partResult.Answer);
        Assert.True(partResult.Duration >= TimeSpan.Zero);

        Assert.Equal(puzzle.Metadata, result.PuzzleMetadata);
        Assert.Equal(PuzzleInputKind.Demo, result.PuzzleInputKind);

        // Assert: behavior.
        Assert.Equal(1, puzzle.PartOneCallCount);
        Assert.Equal(0, puzzle.PartTwoCallCount);

        Assert.Equal("demo-input", puzzle.PartOneInput);

        Assert.Equal(1, inputProvider.CallCount);
        Assert.Equal(puzzleId, inputProvider.RequestedPuzzleId);
        Assert.Equal(PuzzleInputKind.Demo, inputProvider.RequestedInputKind);
    }

    /// <summary>
    /// Verifies that selecting Part Two executes only Part Two
    /// and returns one result with the expected answer.
    /// </summary>
    [Fact]
    public async Task ExecuteAsyncWhenPartTwoIsSelectedExecutesOnlyPartTwo()
    {
        // Arrange.
        var puzzleId = new PuzzleId(year: 2015, day: 1);

        var puzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: puzzleId,
                title: "Fake Puzzle"),
            partOneAnswer: "part-one-answer",
            partTwoAnswer: "part-two-answer");

        var inputProvider = new FakePuzzleInputProvider(
            input: "demo-input");

        var service = new PuzzleExecutionService(
            puzzles: [puzzle],
            inputProvider: inputProvider);

        // Act.
        var result = await service.ExecuteAsync(
            id: puzzleId,
            puzzlePart: PuzzlePart.PartTwo,
            inputKind: PuzzleInputKind.Personal,
            cancellationToken: CancellationToken.None);

        // Assert: result data.
        var partResult = Assert.Single(result.PartResults);

        Assert.Equal(PuzzlePart.PartTwo, partResult.PuzzlePart);
        Assert.Equal("part-two-answer", partResult.Answer);
        Assert.True(partResult.Duration >= TimeSpan.Zero);

        Assert.Equal(PuzzleInputKind.Personal, result.PuzzleInputKind);

        // Assert: behavior.
        Assert.Equal(0, puzzle.PartOneCallCount);
        Assert.Equal(1, puzzle.PartTwoCallCount);

        Assert.Equal("demo-input", puzzle.PartTwoInput);

        Assert.Equal(1, inputProvider.CallCount);
        Assert.Equal(puzzleId, inputProvider.RequestedPuzzleId);
        Assert.Equal(PuzzleInputKind.Personal, inputProvider.RequestedInputKind);
    }

    /// <summary>
    /// Verifies that selecting Both executes each puzzle part exactly once
    /// while loading the input only once.
    /// </summary>
    [Fact]
    public async Task ExecuteAsyncWhenBothPartsAreSelectedExecutesBothPartsAndLoadsInputOnce()
    {
        // Arrange.
        var puzzleId = new PuzzleId(year: 2015, day: 1);

        var puzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: puzzleId,
                title: "Fake Puzzle"),
            partOneAnswer: "part-one-answer",
            partTwoAnswer: "part-two-answer");

        var inputProvider = new FakePuzzleInputProvider(
            input: "shared-input");

        var service = new PuzzleExecutionService(
            puzzles: [puzzle],
            inputProvider: inputProvider);

        // Act.
        var result = await service.ExecuteAsync(
            id: puzzleId,
            puzzlePart: PuzzlePart.Both,
            inputKind: PuzzleInputKind.Demo,
            cancellationToken: CancellationToken.None);

        // Assert: result data.
        Assert.Equal(2, result.PartResults.Count());

        Assert.Collection(
            result.PartResults,
            partOneResult =>
            {
                Assert.Equal(PuzzlePart.PartOne, partOneResult.PuzzlePart);
                Assert.Equal("part-one-answer", partOneResult.Answer);
            },
            partTwoResult =>
            {
                Assert.Equal(PuzzlePart.PartTwo, partTwoResult.PuzzlePart);
                Assert.Equal("part-two-answer", partTwoResult.Answer);
            });

        // Assert: behavior.
        Assert.Equal(1, puzzle.PartOneCallCount);
        Assert.Equal(1, puzzle.PartTwoCallCount);

        Assert.Equal("shared-input", puzzle.PartOneInput);
        Assert.Equal("shared-input", puzzle.PartTwoInput);

        // Reading the same input twice would be unnecessary I/O.
        Assert.Equal(1, inputProvider.CallCount);
    }

    /// <summary>
    /// Verifies that an unknown puzzle id produces a useful exception
    /// without attempting to load an input file.
    /// </summary>
    [Fact]
    public async Task ExecuteAsyncWhenPuzzleIsNotRegisteredThrowsKeyNotFoundException()
    {
        // Arrange.
        var registeredPuzzleId = new PuzzleId(year: 2015, day: 1);
        var requestedPuzzleId = new PuzzleId(year: 2015, day: 2);

        var registeredPuzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: registeredPuzzleId,
                title: "Registered Fake Puzzle"));

        var inputProvider = new FakePuzzleInputProvider(
            input: "this-input-should-not-be-requested");

        var service = new PuzzleExecutionService(
            puzzles: [registeredPuzzle],
            inputProvider: inputProvider);

        // Act.
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.ExecuteAsync(
                id: requestedPuzzleId,
                puzzlePart: PuzzlePart.PartOne,
                inputKind: PuzzleInputKind.Demo,
                cancellationToken: CancellationToken.None));

        // Assert.
        Assert.Contains(requestedPuzzleId.ToString(), exception.Message);

        // We fail before I/O because there is no puzzle to run.
        Assert.Equal(0, inputProvider.CallCount);
    }

    /// <summary>
    /// Verifies that an unsupported puzzle part is rejected
    /// before input loading or puzzle execution begins.
    /// </summary>
    [Fact]
    public async Task ExecuteAsyncWhenPuzzlePartIsInvalidThrowsArgumentOutOfRangeException()
    {
        // Arrange.
        var puzzleId = new PuzzleId(year: 2015, day: 1);

        var puzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: puzzleId,
                title: "Fake Puzzle"));

        var inputProvider = new FakePuzzleInputProvider(
            input: "demo-input");

        var service = new PuzzleExecutionService(
            puzzles: [puzzle],
            inputProvider: inputProvider);

        // Act.
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => service.ExecuteAsync(
                id: puzzleId,
                puzzlePart: (PuzzlePart)999,
                inputKind: PuzzleInputKind.Demo,
                cancellationToken: CancellationToken.None));

        // Assert.
        Assert.Equal("puzzlePart", exception.ParamName);
        Assert.Equal(0, inputProvider.CallCount);
        Assert.Equal(0, puzzle.PartOneCallCount);
        Assert.Equal(0, puzzle.PartTwoCallCount);
    }

    /// <summary>
    /// Verifies that an unsupported input kind is rejected
    /// before input loading or puzzle execution begins.
    /// </summary>
    [Fact]
    public async Task ExecuteAsyncWhenInputKindIsInvalidThrowsArgumentOutOfRangeException()
    {
        // Arrange.
        var puzzleId = new PuzzleId(year: 2015, day: 1);

        var puzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: puzzleId,
                title: "Fake Puzzle"));

        var inputProvider = new FakePuzzleInputProvider(
            input: "demo-input");

        var service = new PuzzleExecutionService(
            puzzles: [puzzle],
            inputProvider: inputProvider);

        // Act.
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => service.ExecuteAsync(
                id: puzzleId,
                puzzlePart: PuzzlePart.PartOne,
                inputKind: (PuzzleInputKind)999,
                cancellationToken: CancellationToken.None));

        // Assert.
        Assert.Equal("inputKind", exception.ParamName);
        Assert.Equal(0, inputProvider.CallCount);
        Assert.Equal(0, puzzle.PartOneCallCount);

    }

    /// <summary>
    /// Verifies that a request with an already-cancelled token stops
    /// before input loading or puzzle execution begins.
    /// </summary>
    [Fact]
    public async Task ExecuteAsyncWhenCancellationIsAlreadyRequestedStopsImmediately()
    {
        // Arrange.
        var puzzleId = new PuzzleId(year: 2015, day: 1);

        var puzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: puzzleId,
                title: "Fake Puzzle"));

        var inputProvider = new FakePuzzleInputProvider(
            input: "demo-input");

        var service = new PuzzleExecutionService(
            puzzles: [puzzle],
            inputProvider: inputProvider);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        cancellationTokenSource.Cancel();

        // Act.
        var exception = await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => service.ExecuteAsync(
                id: puzzleId,
                puzzlePart: PuzzlePart.PartOne,
                inputKind: PuzzleInputKind.Demo,
                cancellationToken: cancellationTokenSource.Token));

        // Assert.
        Assert.Equal(
            cancellationTokenSource.Token,
            exception.CancellationToken);

        Assert.Equal(0, inputProvider.CallCount);
        Assert.Equal(0, puzzle.PartOneCallCount);
        Assert.Equal(0, puzzle.PartTwoCallCount);
    }

    /// <summary>
    /// Verifies that a missing puzzle collection is rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenPuzzlesAreNullThrowsArgumentNullException()
    {
        // Arrange.
        var inputProvider = new FakePuzzleInputProvider(
            input: "demo-input");

        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => new PuzzleExecutionService(
                puzzles: null!,
                inputProvider: inputProvider));

        // Assert.
        Assert.Equal("puzzles", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a missing input provider is rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenInputProviderIsNullThrowsArgumentNullException()
    {
        // Arrange.
        var puzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: new PuzzleId(2015, 1),
                title: "Fake Puzzle"));

        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => new PuzzleExecutionService(
                puzzles: [puzzle],
                inputProvider: null!));

        // Assert.
        Assert.Equal("inputProvider", exception.ParamName);
    }

    /// <summary>
    /// Verifies that at least one puzzle must be registered.
    /// </summary>
    [Fact]
    public void ConstructorWhenPuzzlesAreEmptyThrowsArgumentException()
    {
        // Arrange.
        var inputProvider = new FakePuzzleInputProvider(
            input: "demo-input");

        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => new PuzzleExecutionService(
                puzzles: [],
                inputProvider: inputProvider));

        // Assert.
        Assert.Equal("puzzles", exception.ParamName);
    }

    /// <summary>
    /// Verifies that null entries inside the puzzle collection are rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenPuzzlesContainNullThrowsArgumentException()
    {
        // Arrange.
        var inputProvider = new FakePuzzleInputProvider(
            input: "demo-input");

        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => new PuzzleExecutionService(
                puzzles: [null!],
                inputProvider: inputProvider));

        // Assert.
        Assert.Equal("puzzles", exception.ParamName);
    }

    /// <summary>
    /// Verifies that two puzzles cannot use the same puzzle identifier.
    /// </summary>
    [Fact]
    public void ConstructorWhenPuzzleIdsAreDuplicatedThrowsArgumentException()
    {
        // Arrange.
        var duplicateId = new PuzzleId(year: 2015, day: 1);

        var firstPuzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: duplicateId,
                title: "First Fake Puzzle"));

        var secondPuzzle = new FakePuzzle(
            metadata: new PuzzleMetadata(
                id: duplicateId,
                title: "Second Fake Puzzle"));

        var inputProvider = new FakePuzzleInputProvider(
            input: "demo-input");

        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => new PuzzleExecutionService(
                puzzles:
                [
                    firstPuzzle,
                secondPuzzle,
                ],
                inputProvider: inputProvider));

        // Assert.
        Assert.Equal("puzzles", exception.ParamName);
        Assert.Contains(duplicateId.ToString(), exception.Message);
    }
}