using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;

namespace Aoc.Cli;

/// <summary>
/// Provides simple console-based menu helpers.
/// </summary>
public static class ConsoleMenu
{
    /// <summary>
    /// Converts an input kind into a readable CLI label.
    /// </summary>
    public static string GetInputKindDisplayName(PuzzleInputKind puzzleInputKind) =>
        puzzleInputKind switch
        {
            PuzzleInputKind.Demo => "Demo input",
            PuzzleInputKind.Personal => "Personal input"
        };

    /// <summary>
    /// Converts a puzzle part into a readable CLI label.
    /// </summary>
    public static string GetPartDisplayName(PuzzlePart puzzlePart) =>
        puzzlePart switch
        {
            PuzzlePart.PartOne => "Part one",
            PuzzlePart.PartTwo => "Part two",
            PuzzlePart.Both => "Both parts"
        };
}