namespace Aoc.Abstractions.Puzzles;

/// <summary>
/// Defines the common contract that every Advent of Code puzzle must implement.
/// </summary>
/// <remarks>
/// Implementations contain only puzzle-solving logic.
/// Reading input files, showing output in the CLI, measuring execution time,
/// and writing Markdown reports belong to other parts of the application.
/// </remarks>
public interface IPuzzle
{
    /// <summary>
    /// Gets the metadata used to identify and display the puzzle.
    /// </summary>
    PuzzleMetadata Metadata { get; }

    /// <summary>
    /// Solves the first part of the puzzle.
    /// </summary>
    /// <param name="input">
    /// The complete contents of the selected puzzle input file.
    /// </param>
    /// <returns>
    /// The first-part answer formatted for display in the CLI and Markdown reports.
    /// </returns>
    string SolvePartOne(string input);

    /// <summary>
    /// Solves the second part of the puzzle.
    /// </summary>
    /// <param name="input">
    /// The complete contents of the selected puzzle input file.
    /// </param>
    /// <returns>
    /// The second-part answer formatted for display in the CLI and Markdown reports.
    /// </returns>
    string SolvePartTwo(string input);
}
