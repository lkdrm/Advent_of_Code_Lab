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
    /// <param name="puzzleMetaData">
    /// Metadata that identifies the executed Advent of Code puzzle.
    /// </param>
    /// <param name="inputKind">
    /// Specifies whether the puzzle was run with demo or personal input.
    /// </param>
    /// <param name="partResults">
    /// Results of the puzzle parts that were executed.
    /// </param>
    public PuzzleRunResult(PuzzleMetadata puzzleMetaData, PuzzleInputKind inputKind, IEnumerable<PuzzlePartResult> partResults)
    {
        var results = partResults.ToArray();

        var hasDuplicateParts = results.Select(result => result.PuzzlePart).Distinct().Count() != results.Length;

        if (hasDuplicateParts)
        {
            throw new ArgumentException(
                "A puzzle run cannot contain duplicate part results.",
                nameof(partResults));
        }

        PuzzleMetadata = puzzleMetaData;
        PuzzleInputKind = inputKind;
        PartResults = Array.AsReadOnly(results);
    }
}
