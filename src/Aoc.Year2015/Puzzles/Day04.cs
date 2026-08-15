using Aoc.Abstractions.Puzzles;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

namespace Aoc.Year2015.Puzzles;

/// <summary>
/// Solves Advent of Code 2015, Day 04:
/// The Ideal Stocking Stuffer.
/// </summary>
/// <remarks>
/// The puzzle searches for the lowest positive number that,
/// when appended to a secret key, produces an MD5 hash
/// with the required hexadecimal prefix.
/// </remarks>
public sealed class Day04 : IPuzzle
{
    public PuzzleMetadata Metadata => new(
        id: new(2015, 4),
        title: "The Ideal Stocking Stuffer",
        description: "Mines AdventCoins by searching for MD5 hashes with leading zeroes.");

    /// <summary>
    /// Finds the lowest positive number whose MD5 hash starts
    /// with five hexadecimal zeroes.
    /// </summary>
    /// <param name="input">The secret key used to generate hashes.</param>
    /// <returns>The lowest matching positive number.</returns>
    public string SolvePartOne(string input) => Solve(input, 5);

    /// <summary>
    /// Finds the lowest positive number whose MD5 hash satisfies
    /// the stricter Part Two requirement.
    /// </summary>
    /// <param name="input">The secret key used to generate hashes.</param>
    /// <returns>The lowest matching positive number.</returns>
    public string SolvePartTwo(string input) => Solve(input, 6);

    /// <summary>
    /// Validates and normalizes the secret key before searching
    /// for the lowest matching AdventCoin number.
    /// </summary>
    /// <param name="input">The raw puzzle input.</param>
    /// <param name="requiredLeadingHexZeroes">
    /// The required number of leading hexadecimal zeroes.
    /// </param>
    /// <returns>The lowest matching number formatted as text.</returns>
    private static string Solve(string input, int requiredLeadingHexZeroes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);
        return FindLowestNumber(input.Trim(), requiredLeadingHexZeroes).ToString();
    }

    /// <summary>
    /// Finds the lowest positive number whose MD5 hash contains
    /// the required number of leading hexadecimal zeroes.
    /// </summary>
    /// <param name="input">
    /// The normalized secret key used to generate hashes.
    /// </param>
    /// <param name="requiredHexZeroes">
    /// The required number of leading hexadecimal zeroes.
    /// </param>
    /// <returns>The lowest matching positive number.</returns>
    private static int FindLowestNumber(string input, int requiredHexZeroes)
    {
        var secretKeyBytes = Encoding.ASCII.GetBytes(input);

        var candidateBytes = new byte[secretKeyBytes.Length + 10];
        secretKeyBytes.AsSpan().CopyTo(candidateBytes);

        Span<byte> hash = stackalloc byte[16];

        for (var number = 1; number < int.MaxValue; number++)
        {
            var numberDestination = candidateBytes.AsSpan(secretKeyBytes.Length);

            Utf8Formatter.TryFormat(number, numberDestination, out var numberByteCount);

            var candidateLength = secretKeyBytes.Length + numberByteCount;

            MD5.HashData(candidateBytes.AsSpan(0, candidateLength), hash);
            if (HasRequiredLeadingZeroes(hash, requiredHexZeroes))
            {
                return number;
            }
        }

        throw new InvalidOperationException("No matching AdventCoin number was found.");
    }

    /// <summary>
    /// Determines whether an MD5 hash starts with the required
    /// number of hexadecimal zeroes.
    /// </summary>
    /// <param name="hash">The binary MD5 hash.</param>
    /// <param name="requiredLeadingHexZeroes">
    /// The required number of leading hexadecimal zeroes.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the required prefix is present;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static bool HasRequiredLeadingZeroes(ReadOnlySpan<byte> hash, int requiredLeadingHexZeroes)
    {
        var completeZeroBytes = requiredLeadingHexZeroes / 2;

        for (var index = 0; index < completeZeroBytes; index++)
        {
            if (hash[index] != 0)
            {
                return false;
            }
        }

        var requiresHalfByte = requiredLeadingHexZeroes % 2 != 0;

        return !requiresHalfByte || (hash[completeZeroBytes] & 0xF0) == 0;
    }
}
