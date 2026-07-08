using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;

namespace Aoc.Cli;

/// <summary>
/// Provides simple console-based menu helpers.
/// </summary>
public static class ConsoleMenu
{
    /// <summary>
    /// Displays a numbered list of options and asks the user to select one.
    /// </summary>
    /// <typeparam name="T">The type of option being selected.</typeparam>
    /// <param name="title">The title displayed above the options.</param>
    /// <param name="options">The available options.</param>
    /// <param name="getDisplayText">A function that converts an option to readable text.</param>
    /// <returns>The selected option.</returns>
    [Obsolete("Use Spectre.Console SelectionPrompt instead.")]
    public static T SelectOption<T>(string title, IReadOnlyList<T> options, Func<T, string> getDisplayText)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length));

            for (var index = 0; index < options.Count; index++)
            {
                Console.WriteLine($"{index + 1}. {getDisplayText(options[index])}");
            }

            Console.WriteLine("Select an option: ");

            var input = Console.ReadLine();
            return options[int.Parse(input) - 1];
        }
    }

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