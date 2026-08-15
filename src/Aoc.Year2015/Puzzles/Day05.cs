using Aoc.Abstractions.Puzzles;

namespace Aoc.Year2015.Puzzles;

/// <summary>
/// Solves Advent of Code 2015, Day 05:
/// Doesn't He Have Intern-Elves For This?.
/// </summary>
public sealed class Day05 : IPuzzle
{
    /// <inheritdoc />
    public PuzzleMetadata Metadata => new(
        id: new(2015, 5),
        title: "Doesn't He Have Intern-Elves For This?",
        description: "Counts strings that are nice according to Santa's rules.");

    /// <summary>
    /// Solves Part One by counting candidate strings that satisfy the original
    /// nice-string rules.
    /// </summary>
    /// <param name="input">
    /// The puzzle input containing one candidate string per line.
    /// </param>
    /// <returns>
    /// The number of strings that contain at least three vowels, contain
    /// consecutive duplicate characters, and contain no forbidden pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="input"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is empty or contains only whitespace.
    /// </exception>
    public string SolvePartOne(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        var lines = ParseCandidates(input);
        var result = 0;

        foreach (var line in lines)
        {
            if (HasAtLeastThreeVowels(line) && HasConsecutiveDuplicate(line) && DoesNotContainForbiddenPair(line))
            {
                result++;
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// Solves Part Two by counting candidate strings that satisfy the revised
    /// nice-string rules.
    /// </summary>
    /// <param name="input">
    /// The puzzle input containing one candidate string per line.
    /// </param>
    /// <returns>
    /// The number of strings that contain a repeated non-overlapping
    /// two-character pair and a character repeated with exactly one character
    /// between its occurrences.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="input"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is empty or contains only whitespace.
    /// </exception>
    public string SolvePartTwo(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        var lines = ParseCandidates(input);
        var result = 0;

        foreach (var line in lines)
        {
            if (HasRepeatedPairWithoutOverlap(line) && HasRepeatingLetterWithOneBetween(line))
            {
                result++;
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// Splits the puzzle input into individual candidate strings.
    /// </summary>
    /// <param name="input">The puzzle input containing one candidate per line.</param>
    /// <returns>The non-empty, trimmed candidate strings.</returns>
    private static string[] ParseCandidates(string input) => input.Split(
            ['\r', '\n'],
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

    /// <summary>
    /// Determines whether the input contains at least three vowels.
    /// </summary>
    /// <param name="input">The input string to inspect.</param>
    /// <returns>
    /// <see langword="true"/> when the candidate contains at least three vowels;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static bool HasAtLeastThreeVowels(string input)
    {
        var vowelCount = 0;

        foreach (var character in input)
        {
            if (character is 'a' or 'e' or 'i' or 'o' or 'u')
            {
                vowelCount++;
            }

            if (vowelCount >= 3)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the input contains two identical consecutive characters.
    /// </summary>
    /// <param name="input">The input string to inspect.</param>
    /// <returns>
    /// <see langword="true"/> when a consecutive duplicate exists;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static bool HasConsecutiveDuplicate(string input)
    {
        for (int i = 0; i < input.Length - 1; i++)
        {
            if (input[i] == input[i + 1])
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the input contains none of the forbidden character pairs.
    /// </summary>
    /// <param name="input">The input string to inspect.</param>
    /// <returns>
    /// <see langword="true"/> when the candidate contains none of
    /// <c>ab</c>, <c>cd</c>, <c>pq</c>, or <c>xy</c>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static bool DoesNotContainForbiddenPair(string input)
    {
        for (int i = 0; i < input.Length - 1; i++)
        {
            if ($"{input[i]}{input[i + 1]}" is "ab" or "cd" or "pq" or "xy")
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Determines whether a two-character pair appears at least twice without overlapping.
    /// </summary>
    /// <param name="input">The input string to inspect.</param>
    /// <returns>
    /// <see langword="true"/> when a non-overlapping repeated pair exists;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static bool HasRepeatedPairWithoutOverlap(string input)
    {
        for (int i = 0; i < input.Length - 1; i++)
        {
            if (input.IndexOf(input.Substring(i, 2), i + 2, StringComparison.Ordinal) >= 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the same character appears twice with one character between.
    /// </summary>
    /// <param name="input">The input string to inspect.</param>
    /// <returns>
    /// <see langword="true"/> when such a repeating character exists;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static bool HasRepeatingLetterWithOneBetween(string input)
    {
        for (int i = 0; i < input.Length - 2; i++)
        {
            if (input[i] == input[i + 2])
            {
                return true;
            }
        }
        return false;
    }
}
