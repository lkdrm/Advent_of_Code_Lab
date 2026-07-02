using Aoc.Abstractions.Puzzles;

namespace Aoc.Year2015.Puzzles;

/// <summary>
/// Solves Advent of Code 2015, Day 01: Not Quite Lisp.
/// </summary>
/// <remarks>
/// The puzzle input contains opening and closing parentheses.
/// Each character changes Santa's current floor.
/// </remarks>
public class Day01 : IPuzzle
{
    /// <summary>
    /// Gets the metadata displayed by the CLI and used to identify this puzzle.
    /// </summary>
    public PuzzleMetadata Metadata { get; } = new(
        id: new PuzzleId(2015, 1),
        title: "Not Quite Lisp",
        description: "Tracks Santa's floor while processing parentheses.");

    /// <summary>
    /// Calculates Santa's final floor after processing the complete input.
    /// </summary>
    /// <param name="input">The complete puzzle input.</param>
    /// <returns>The final floor number.</returns>
    public string SolvePartOne(string input)
    {
        var floor = 0;

        foreach (var item in input)
        {
            floor += GetFloor(item);
        }

        return floor.ToString();
    }

    /// <summary>
    /// Finds the first input position where Santa enters the basement.
    /// </summary>
    /// <param name="input">The complete puzzle input.</param>
    /// <returns>The one-based position that first reaches the basement.</returns>
    public string SolvePartTwo(string input)
    {
        var floor = 0;

        for (int index = 0; index < input.Length; index++)
        {
            floor += GetFloor(input[index]);

            if (floor == -1)
            {
                return (index + 1).ToString();
            }
        }

        return floor.ToString();
    }

    /// <summary>
    /// Converts one input character into its floor change.
    /// </summary>
    /// <param name="character">A character from the puzzle input.</param>
    /// <returns>
    private static int GetFloor(char item) =>
        item switch
        {
            '(' => 1,
            ')' => -1,
            _ => -0,
        };
}
