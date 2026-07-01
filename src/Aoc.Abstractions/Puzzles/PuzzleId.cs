namespace Aoc.Abstractions.Puzzles;

/// <summary>
/// Identifies a single Advent of Code puzzle by its year and day number.
/// </summary>
/// <remarks>
/// For example: <c>2015 / Day 01</c>.
/// This type prevents passing year and day as separate values throughout the application.
/// </remarks>
public readonly record struct PuzzleId
{
    /// <summary>
    /// Gets the Advent of Code event year.
    /// </summary>
    public int Year { get; }

    /// <summary>
    /// Gets the puzzle day number within the event year.
    /// </summary>
    public int Day { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PuzzleId"/> struct.
    /// </summary>
    /// <param name="year">The Advent of Code event year. Must be 2015 or later.</param>
    /// <param name="day">The puzzle day number. Must be between 1 and 25.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="year"/> is earlier than 2015
    /// or when <paramref name="day"/> is outside the range from 1 to 25.
    /// </exception>
    public PuzzleId(int year, int day)
    {
        if (year < 2015)
        {
            throw new ArgumentOutOfRangeException(nameof(year), year, "Well Advent of Code started in 2015");
        }

        if (day < 1 || day > 25)
        {
            throw new ArgumentOutOfRangeException(nameof(day), day, "A puzzle day must be between 1 or 25 days!");
        }

        Year = year;
        Day = day;
    }

    public override string ToString() => $"{Year}/Day {Day:D2}";
}
