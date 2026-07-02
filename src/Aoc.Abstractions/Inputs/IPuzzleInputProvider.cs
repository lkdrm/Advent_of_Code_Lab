using Aoc.Abstractions.Puzzles;

namespace Aoc.Abstractions.Inputs;

/// <summary>
/// Defines a contract for asynchronously retrieving puzzle input content.
/// </summary>
/// <remarks>
/// Implementations decide where the input comes from:
/// a committed demo file, a local personal file, or later an HTTP source.
/// </remarks>
public interface IPuzzleInputProvider
{
    /// <summary>
    /// Gets the complete input content for the specified puzzle.
    /// </summary>
    /// <param name="id">
    /// Identifies the Advent of Code year and day whose input should be loaded.
    /// </param>
    /// <param name="inputKind">
    /// Specifies whether to load a committed demo input or a local personal input.
    /// </param>
    /// <param name="cancellationToken">
    /// Allows the caller to cancel the asynchronous file-reading operation.
    /// </param>
    /// <returns>
    /// A task containing the complete text from the selected input file.
    /// </returns>
    Task<string> GetInputAsync(PuzzleId id, PuzzleInputKind inputKind, CancellationToken cancellationToken);
}
