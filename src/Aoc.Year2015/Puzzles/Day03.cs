using Aoc.Abstractions.Puzzles;

namespace Aoc.Year2015.Puzzles;

/// <summary>
/// Solves Advent of Code 2015, Day 03:
/// Perfectly Spherical Houses in a Vacuum.
/// </summary>
/// <remarks>
/// The puzzle input contains directions that move Santa or Robo-Santa
/// across a two-dimensional grid of houses.
/// </remarks>
public sealed class Day03 : IPuzzle
{
    /// <inheritdoc />
    public PuzzleMetadata Metadata { get; } = new(
        id: new(2015, 3),
        title: "Perfectly Spherical Houses in a Vacuum",
        description: "Tracks houses visited while Santa follows movement directions.");

    /// <summary>
    /// Calculates how many unique houses Santa visits while following
    /// all movement directions.
    /// </summary>
    /// <param name="input">
    /// Movement directions containing north, south, east, and west commands.
    /// </param>
    /// <returns>
    /// The number of unique houses that receive at least one present.
    /// </returns>
    public string SolvePartOne(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        var currentPosition = (X: 0, Y: 0);

        var visitedHouses = new HashSet<(int X, int Y)> { currentPosition };

        foreach (var direction in input.Trim())
        {
            currentPosition = Move(currentPosition, direction);
            visitedHouses.Add(currentPosition);
        }

        return visitedHouses.Count.ToString();
    }

    /// <summary>
    /// Calculates how many unique houses Santa and Robo-Santa visit
    /// while taking turns following movement directions.
    /// </summary>
    /// <param name="input">
    /// Movement directions shared alternately between Santa and Robo-Santa.
    /// </param>
    /// <returns>
    /// The number of unique houses that receive at least one present.
    /// </returns>
    public string SolvePartTwo(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        var santaPosition = (X: 0, Y: 0);
        var roboSantaPosition = (X: 0, Y: 0);

        var visitedHouses = new HashSet<(int X, int Y)> { santaPosition };

        var directions = input.Trim();

        for (int i = 0; i < directions.Length; i++)
        {
            var direction = directions[i];

            if (i % 2 == 0)
            {
                santaPosition = Move(santaPosition, direction);
                visitedHouses.Add(santaPosition);
            }
            else
            {
                roboSantaPosition = Move(roboSantaPosition, direction);
                visitedHouses.Add(roboSantaPosition);
            }
        }

        return visitedHouses.Count.ToString();
    }

    /// <summary>
    /// Moves the supplied position by one house in the requested direction.
    /// </summary>
    /// <param name="currentPosition">The position before the movement.</param>
    /// <param name="direction">The direction character to process.</param>
    /// <returns>The position after applying the movement.</returns>
    private static (int X, int Y) Move((int X, int Y) currentPosition, char direction) =>
        direction switch
        {
            '^' => (currentPosition.X, currentPosition.Y + 1),
            'v' => (currentPosition.X, currentPosition.Y - 1),
            '>' => (currentPosition.X + 1, currentPosition.Y),
            '<' => (currentPosition.X - 1, currentPosition.Y),
            _ => currentPosition,
        };
}
