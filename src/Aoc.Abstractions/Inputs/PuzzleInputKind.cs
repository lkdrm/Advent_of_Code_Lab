namespace Aoc.Abstractions.Inputs;

/// <summary>
/// Specifies which input source should be used when running a puzzle.
/// </summary>
public enum PuzzleInputKind
{
    /// <summary>
    /// Uses a small custom demo input committed to the repository.
    /// This option allows anyone to clone and run the application immediately.
    /// </summary>
    Demo = 1,

    /// <summary>
    /// Uses a personal Advent of Code input stored locally and excluded from Git.
    /// </summary>
    Personal = 2
}
