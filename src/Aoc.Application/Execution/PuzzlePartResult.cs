using Aoc.Abstractions.Puzzles;

namespace Aoc.Application.Execution;

/// <summary>
/// Represents the result of running one individual part
/// of an Advent of Code puzzle.
/// </summary>
/// <remarks>
/// A single result contains the calculated answer and the time
/// required to execute that specific puzzle part.
/// </remarks>
public sealed record PuzzlePartResult
{
    /// <summary>
    /// Gets the puzzle part that was executed.
    /// </summary>
    public PuzzlePart PuzzlePart { get; }

    /// <summary>
    /// Gets the calculated answer returned by the puzzle solver.
    /// </summary>
    public string Answer { get; }

    /// <summary>
    /// Gets the execution duration of the puzzle part.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PuzzlePartResult"/> class.
    /// </summary>
    /// <param name="puzzlePart">
    /// The puzzle part that was executed. Only <see cref="PuzzlePart.PartOne"/>
    /// and <see cref="PuzzlePart.PartTwo"/> are valid here.
    /// </param>
    /// <param name="answer">The calculated answer formatted for display.</param>
    /// <param name="duration">The time required to execute the puzzle part.</param>
    public PuzzlePartResult(PuzzlePart puzzlePart, string answer, TimeSpan duration)
    {
        PuzzlePart = puzzlePart;
        Answer = answer;
        Duration = duration;
    }
}
