using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;

namespace Aoc.Application.Execution;

/// <summary>
/// Represents the complete result of running one Advent of Code puzzle.
/// </summary>
/// <remarks>
/// A run result contains puzzle metadata, the selected input kind,
/// and one or more results for executed puzzle parts.
/// </remarks>
public sealed record PuzzleRunResult
{
    /// <summary>
    /// Gets metadata that identifies the puzzle that was executed.
    /// </summary>
    public PuzzleMetadata PuzzleMetadata { get; }

    /// <summary>
    /// Gets the input source used for this puzzle execution.
    /// </summary>
    public PuzzleInputKind PuzzleInputKind { get; }

    /// <summary>
    /// Gets results for the executed puzzle parts.
    /// </summary>
    public IEnumerable<PuzzlePartResult> PartResults { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PuzzleRunResult"/> class.
    /// </summary>
    /// <param name="puzzleMetadata">
    /// Metadata that identifies the executed Advent of Code puzzle.
    /// </param>
    /// <param name="inputKind">
    /// Specifies whether the puzzle was run with demo or personal input.
    /// </param>
    /// <summary>
    /// Gets the ordered read-only results of the executed puzzle parts.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="puzzleMetadata"/> or
    /// <paramref name="partResults"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="inputKind"/> is not
    /// <see cref="PuzzleInputKind.Demo"/> or
    /// <see cref="PuzzleInputKind.Personal"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="partResults"/> is empty,
    /// contains a null result, or contains duplicate puzzle parts.
    /// </exception>
    public PuzzleRunResult(PuzzleMetadata puzzleMetadata, PuzzleInputKind inputKind, IReadOnlyList<PuzzlePartResult> partResults)
    {
        ArgumentNullException.ThrowIfNull(puzzleMetadata);
        ArgumentNullException.ThrowIfNull(partResults);

        if (inputKind is not (PuzzleInputKind.Demo or PuzzleInputKind.Personal))
        {
            throw new ArgumentOutOfRangeException(nameof(inputKind), inputKind, "Input kind must be Demo or Personal.");
        }

        var results = partResults.ToArray();

        if (results.Length == 0)
        {
            throw new ArgumentException("A puzzle run must contain at least one part result.", nameof(partResults));
        }

        if (results.Any(static result => result is null))
        {
            throw new ArgumentException("A puzzle run cannot contain null part results.", nameof(partResults));
        }

        var hasDuplicateParts = results.Select(result => result.PuzzlePart).Distinct().Count() != results.Length;

        if (hasDuplicateParts)
        {
            throw new ArgumentException(
                "A puzzle run cannot contain duplicate part results.",
                nameof(partResults));
        }

        PuzzleMetadata = puzzleMetadata;
        PuzzleInputKind = inputKind;
        PartResults = Array.AsReadOnly(results);
    }
}
