namespace Aoc.Abstractions.Puzzles;

/// <summary>
/// Contains human-readable information about an Advent of Code puzzle.
/// </summary>
/// <remarks>
/// This metadata is used by the CLI to display a puzzle in menus,
/// reports, and generated documentation.
/// </remarks>
public class PuzzleMetadata
{
    /// <summary>
    /// Gets the unique identifier of the puzzle.
    /// </summary>
    public PuzzleId Id { get; }

    /// <summary>
    /// Gets the title displayed in the CLI and Markdown reports.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets an optional short explanation shown before a puzzle is executed.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PuzzleMetadata"/> class.
    /// </summary>
    /// <param name="id">The unique year and day identifier of the puzzle.</param>
    /// <param name="title">The official or descriptive title of the puzzle.</param>
    /// <param name="description">An optional short explanation of the puzzle.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="title"/> is empty or contains only whitespace.
    /// </exception>
    public PuzzleMetadata(PuzzleId id, string title, string? description = default)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(nameof(title), "Puzzle title is empty!");
        }

        Id = id;
        Title = title;
        Description = description;
    }
}
