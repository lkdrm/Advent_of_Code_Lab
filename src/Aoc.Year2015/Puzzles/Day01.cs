using Aoc.Abstractions.Puzzles;

namespace Aoc.Year2015.Puzzles;

/// <summary>
/// Solves Advent of Code 2015, Day 01: Not Quite Lisp.
/// </summary>
/// <remarks>
/// The puzzle input contains opening and closing parentheses.
/// Each character changes Santa's current floor.
/// </remarks>
public sealed class Day01 : IPuzzle
{
    /// <inheritdoc />
    public PuzzleMetadata Metadata { get; } = new(
        id: new(2015, 1),
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
            floor += GetFloorChange(item);
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

        for (var index = 0; index < input.Length; index++)
        {
            floor += GetFloorChange(input[index]);

            if (floor == -1)
            {
                return (index + 1).ToString();
            }
        }

        return floor.ToString();
    }

    /// <summary>
    /// Converts one input character into its corresponding floor change.
    /// </summary>
    /// <param name="character">A character from the puzzle input.</param>
    /// <returns>
    /// `1` for an opening parenthesis, `-1` for a closing parenthesis,
    /// or `0` for any other character.
    /// </returns>
    private static int GetFloorChange(char character) =>
        character switch
        {
            '(' => 1,
            ')' => -1,
            _ => 0,
        };
}
