using Aoc.Abstractions.Puzzles;

namespace Aoc.Year2015.Puzzles;

/// <summary>
/// Solves Advent of Code 2015, Day 02: I Was Told There Would Be No Math.
/// </summary>
/// <remarks>
/// Each input line represents one present in
/// <c>length x width x height</c> format.
/// </remarks>
public class Day02 : IPuzzle
{
    /// <inheritdoc />
    public PuzzleMetadata Metadata { get; } = new(
        id: new(2015, 2),
        title: "I Was Told There Would Be No Math",
        description: "Calculates wrapping paper and ribbon required for presents.");

    /// <summary>
    /// Calculates the total amount of wrapping paper required for all presents.
    /// </summary>
    /// <param name="input">
    /// Present dimensions, with one <c>length x width x height</c> value per line.
    /// </param>
    /// <returns>The total amount of wrapping paper required.</returns>
    public string SolvePartOne(string input)
    {
        var totalWrappingPaper = 0;

        foreach (var present in ParsePresents(input))
        {
            totalWrappingPaper += present.GetWrappingPaperRequired();
        }

        return totalWrappingPaper.ToString();
    }

    /// <summary>
    /// Calculates the total ribbon length required for all presents.
    /// </summary>
    /// <param name="input">
    /// Present dimensions, with one <c>length x width x height</c> value per line.
    /// </param>
    /// <returns>The total ribbon length required.</returns>
    public string SolvePartTwo(string input)
    {
        var totalRibbon = 0;
        foreach (var present in ParsePresents(input))
        {
            totalRibbon += present.GetRibbonRequired();
        }

        return totalRibbon.ToString();
    }

    /// <summary>
    /// Parses every non-empty input line into dimensions of a single present.
    /// </summary>
    /// <param name="input">The complete puzzle input.</param>
    /// <returns>A sequence of validated present dimensions.</returns>
    private static IEnumerable<PresentsDimensions> ParsePresents(string input)
    {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var line in lines)
        {
            yield return ParsePresentDimensions(line);
        }
    }

    /// <summary>
    /// Parses one input line in <c>length x width x height</c> format.
    /// </summary>
    /// <param name="line">A single present dimension line.</param>
    /// <returns>Dimensions of one present.</returns>
    private static PresentsDimensions ParsePresentDimensions(string line)
    {
        var values = line.Split('x', StringSplitOptions.TrimEntries);
        return new PresentsDimensions(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
    }

    /// <summary>
    /// Represents dimensions of one rectangular present.
    /// </summary>
    private readonly record struct PresentsDimensions(int Length, int Width, int Height)
    {
        /// <summary>
        /// Calculates the wrapping paper required for this present.
        /// </summary>
        /// <returns>
        /// Surface area of the present plus the area of its smallest face.
        /// </returns>
        public int GetWrappingPaperRequired()
        {
            var lengthWidthArea = Length * Width;
            var widthHeightArea = Width * Height;
            var heightLengthArea = Height * Length;

            var surfaceArea = 2 * (lengthWidthArea + widthHeightArea + heightLengthArea);

            var slack = Math.Min(lengthWidthArea, Math.Min(widthHeightArea, heightLengthArea));

            return surfaceArea + slack;
        }

        /// <summary>
        /// Calculates the ribbon required for this present.
        /// </summary>
        /// <returns>
        /// The smallest perimeter around the present plus ribbon for the bow.
        /// </returns>
        public int GetRibbonRequired()
        {
            var lengthWidthPerimeter = 2 * (Length + Width);
            var widthHeightPerimeter = 2 * (Width + Height);
            var heightLengthPerimeter = 2 * (Height + Length);

            var smallestPerimeter = Math.Min(lengthWidthPerimeter, Math.Min(widthHeightPerimeter, heightLengthPerimeter));

            var bow = Length * Width * Height;

            return smallestPerimeter + bow;
        }
    }
}
