namespace Aoc.Abstractions.Puzzles;

/// <summary>
/// Specifies which part of an Advent of Code puzzle should be executed.
/// </summary>
public enum PuzzlePart
{
    /// <summary>
    /// Runs the first part of the selected puzzle.
    /// </summary>
    PartOne,

    /// <summary>
    /// Runs the second part of the selected puzzle.
    /// </summary>
    PartTwo,

    /// <summary>
    /// Runs both parts of the selected puzzle in order.
    /// </summary>
    Both
}
