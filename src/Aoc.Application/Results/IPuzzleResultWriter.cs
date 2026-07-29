using Aoc.Application.Execution;

namespace Aoc.Application.Results;

/// <summary>
/// Defines a contract for persisting the structured result
/// of an Advent of Code puzzle execution.
/// </summary>
/// <remarks>
/// Implementations decide how and where results are stored.
/// The application layer depends only on this contract and does not
/// perform file-system operations directly.
/// </remarks>
public interface IPuzzleResultWriter
{
    /// <summary>
    /// Writes the supplied puzzle execution result asynchronously.
    /// </summary>
    /// <param name="result">
    /// The complete structured result that should be persisted.
    /// </param>
    /// <param name="cancellationToken">
    /// Allows the write operation to be cancelled.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous write operation.
    /// </returns>
    Task WriteAsync(PuzzleRunResult result, CancellationToken cancellationToken);
}
