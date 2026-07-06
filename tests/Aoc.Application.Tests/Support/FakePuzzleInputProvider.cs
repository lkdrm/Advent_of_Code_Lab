using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;

namespace Aoc.Application.Tests.Support;

/// <summary>
/// A controllable input provider used only in application-layer tests.
/// </summary>
/// <remarks>
/// The fake does not access the file system. It returns predefined input
/// and records the request received from PuzzleExecutionService.
/// </remarks>
public sealed class FakePuzzleInputProvider : IPuzzleInputProvider
{
    private readonly string _input;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakePuzzleInputProvider"/> class.
    /// </summary>
    /// <param name="input">The input text that should be returned to the caller.</param>
    public FakePuzzleInputProvider(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        _input = input;
    }

    /// <summary>
    /// Gets the number of times input was requested.
    /// </summary>
    public int CallCount { get; private set; }

    /// <summary>
    /// Gets the puzzle identifier from the latest request.
    /// </summary>
    public PuzzleId? RequestedPuzzleId { get; private set; }

    /// <summary>
    /// Gets the selected input kind from the latest request.
    /// </summary>
    public PuzzleInputKind? RequestedInputKind { get; private set; }

    /// <summary>
    /// Gets the cancellation token received during the latest request.
    /// </summary>
    public CancellationToken RequestedCancellationToken { get; private set; }

    /// <summary>
    /// Returns predefined input without reading any files.
    /// </summary>
    /// <param name="puzzleId">The requested puzzle identifier.</param>
    /// <param name="inputKind">The requested input kind.</param>
    /// <param name="cancellationToken">The cancellation token passed by the caller.</param>
    /// <returns>A completed task containing the predefined input.</returns>
    public Task<string> GetInputAsync(PuzzleId puzzleId, PuzzleInputKind inputKind, CancellationToken cancellationToken)
    {
        CallCount++;
        RequestedPuzzleId = puzzleId;
        RequestedInputKind = inputKind;
        RequestedCancellationToken = cancellationToken;

        return Task.FromResult(_input);
    }
}