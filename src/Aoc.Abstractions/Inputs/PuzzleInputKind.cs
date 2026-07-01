namespace Aoc.Abstractions.Inputs;

/// <summary>
/// Specifies which input source should be used when running a puzzle.
/// </summary>
public enum PuzzleInputKind
{
    /// <summary>
    /// Uses the personal puzzle input assigned to the Advent of Code account.
    /// </summary>
    Personal,

    /// <summary>
    /// Uses a sample input taken from the puzzle description.
    /// </summary>
    Sample
}
