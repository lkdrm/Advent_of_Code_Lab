using Aoc.Abstractions.Puzzles;

namespace Aoc.Application.Tests.Support;

/// <summary>
/// A controllable puzzle implementation used only in application-layer tests.
/// </summary>
/// <remarks>
/// The fake returns predefined answers and records which solver methods
/// were called. This lets tests verify PuzzleExecutionService behavior
/// without depending on a real Advent of Code algorithm.
/// </remarks>
public sealed class FakePuzzle : IPuzzle
{
    private readonly string _partOneAnswer;
    private readonly string _partTwoAnswer;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakePuzzle"/> class.
    /// </summary>
    /// <param name="metadata">Metadata used to identify this fake puzzle.</param>
    /// <param name="partOneAnswer">The answer returned by Part One.</param>
    /// <param name="partTwoAnswer">The answer returned by Part Two.</param>
    public FakePuzzle(PuzzleMetadata metadata, string partOneAnswer = "fake-part-one-answer", string partTwoAnswer = "fake-part-two-answer")
    {
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentException.ThrowIfNullOrWhiteSpace(partOneAnswer);
        ArgumentException.ThrowIfNullOrWhiteSpace(partTwoAnswer);

        Metadata = metadata;
        _partOneAnswer = partOneAnswer;
        _partTwoAnswer = partTwoAnswer;
    }

    /// <summary>
    /// Gets metadata that identifies the fake puzzle.
    /// </summary>
    public PuzzleMetadata Metadata { get; }

    /// <summary>
    /// Gets the number of times Part One was executed.
    /// </summary>
    public int PartOneCallCount { get; private set; }

    /// <summary>
    /// Gets the number of times Part Two was executed.
    /// </summary>
    public int PartTwoCallCount { get; private set; }

    /// <summary>
    /// Gets the input received by Part One during its latest execution.
    /// </summary>
    public string? PartOneInput { get; private set; }

    /// <summary>
    /// Gets the input received by Part Two during its latest execution.
    /// </summary>
    public string? PartTwoInput { get; private set; }

    /// <summary>
    /// Simulates solving Part One.
    /// </summary>
    /// <param name="input">The input passed by the execution service.</param>
    /// <returns>A predefined answer for Part One.</returns>
    public string SolvePartOne(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        PartOneCallCount++;
        PartOneInput = input;

        return _partOneAnswer;
    }

    /// <summary>
    /// Simulates solving Part Two.
    /// </summary>
    /// <param name="input">The input passed by the execution service.</param>
    /// <returns>A predefined answer for Part Two.</returns>
    public string SolvePartTwo(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        PartTwoCallCount++;
        PartTwoInput = input;

        return _partTwoAnswer;
    }
}