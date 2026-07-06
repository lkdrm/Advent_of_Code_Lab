using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;

namespace Aoc.Application.Execution;

/// <summary>
/// Defines the application use case for executing Advent of Code puzzles.
/// </summary>
/// <remarks>
/// Implementations coordinate puzzle discovery, input loading,
/// puzzle-part execution, and result creation.
///
/// This contract does not define how results are displayed or persisted.
/// Those responsibilities belong to the CLI and infrastructure layers.
/// </remarks>
public interface IPuzzleExecutionService
{
    /// <summary>
    /// Executes the requested part or parts of an Advent of Code puzzle.
    /// </summary>
    /// <param name="id">
    /// Identifies the year and day of the puzzle to execute.
    /// </param>
    /// <param name="puzzlePart">
    /// Specifies whether to execute Part One, Part Two, or both parts.
    /// </param>
    /// <param name="inputKind">
    /// Specifies whether to load demo or personal input.
    /// </param>
    /// <param name="cancellationToken">
    /// Allows the caller to cancel asynchronous input loading
    /// or future longer-running operations.
    /// </param>
    /// <returns>
    /// A task containing the complete structured result of the puzzle execution.
    /// </returns>
    Task<PuzzleRunResult> ExecuteAsync(PuzzleId id, PuzzlePart puzzlePart, PuzzleInputKind inputKind, CancellationToken cancellationToken);
}
