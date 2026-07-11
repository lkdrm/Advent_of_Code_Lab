using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using System.Diagnostics;

namespace Aoc.Application.Execution;

/// <summary>
/// Coordinates the execution of Advent of Code puzzles.
/// </summary>
/// <remarks>
/// This service finds a registered puzzle, loads its input asynchronously,
/// executes the requested puzzle part or parts, measures execution time,
/// and returns structured execution data.
///
/// It does not know how input is stored or how results are displayed.
/// Those responsibilities belong to infrastructure and UI layers.
/// </remarks>
public sealed class PuzzleExecutionService : IPuzzleExecutionService
{
    private readonly IReadOnlyDictionary<PuzzleId, IPuzzle> _puzzles;
    private readonly IPuzzleInputProvider _inputProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="PuzzleExecutionService"/> class.
    /// </summary>
    /// <param name="puzzles">
    /// All puzzle implementations registered in the application.
    /// </param>
    /// <param name="inputProvider">
    /// The service used to retrieve puzzle input content.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="puzzles"/> or
    /// <paramref name="inputProvider"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="puzzles"/> is empty,
    /// contains a null entry, or contains duplicate puzzle identifiers.
    /// </exception>
    public PuzzleExecutionService(IEnumerable<IPuzzle> puzzles, IPuzzleInputProvider inputProvider)
    {
        ArgumentNullException.ThrowIfNull(puzzles);
        ArgumentNullException.ThrowIfNull(inputProvider);

        var puzzleDictionary = new Dictionary<PuzzleId, IPuzzle>();

        foreach (var puzzle in puzzles)
        {
            if (puzzle is null)
            {
                throw new ArgumentException("The puzzle collection cannot contain null entries.", nameof(puzzles));
            }

            if (!puzzleDictionary.TryAdd(puzzle.Metadata.Id, puzzle))
            {
                throw new ArgumentException($"A puzzle with id '{puzzle.Metadata.Id}' is already registered.", nameof(puzzles));
            }
        }

        if (puzzleDictionary.Count == 0)
        {
            throw new ArgumentException("At least one puzzle must be registered.", nameof(puzzles));
        }

        _puzzles = puzzles.ToDictionary(puzzle => puzzle.Metadata.Id, puzzle => puzzle);
        _inputProvider = inputProvider;
    }

    /// <inheritdoc />
    public async Task<PuzzleRunResult> ExecuteAsync(PuzzleId id, PuzzlePart puzzlePart, PuzzleInputKind inputKind, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (puzzlePart is not (PuzzlePart.PartOne or PuzzlePart.PartTwo or PuzzlePart.Both))
        {
            throw new ArgumentOutOfRangeException(nameof(puzzlePart), puzzlePart, "Puzzle part must be Part One, Part Two, or Both.");
        }

        if (inputKind is not (PuzzleInputKind.Demo or PuzzleInputKind.Personal))
        {
            throw new ArgumentOutOfRangeException(nameof(inputKind), inputKind, "Input kind must be Demo or Personal.");
        }

        if (!_puzzles.TryGetValue(id, out var puzzle))
        {
            throw new KeyNotFoundException($"No puzzle is registered for '{id}'.");
        }

        var input = await _inputProvider.GetInputAsync(id, inputKind, cancellationToken);

        var partResults = new List<PuzzlePartResult>();

        switch (puzzlePart)
        {
            case PuzzlePart.PartOne:
                partResults.Add(ExecutePart(PuzzlePart.PartOne, input, puzzle.SolvePartOne));
                break;
            case PuzzlePart.PartTwo:
                partResults.Add(ExecutePart(PuzzlePart.PartTwo, input, puzzle.SolvePartTwo));
                break;
            case PuzzlePart.Both:
                partResults.Add(ExecutePart(PuzzlePart.PartOne, input, puzzle.SolvePartOne));
                partResults.Add(ExecutePart(PuzzlePart.PartTwo, input, puzzle.SolvePartTwo));
                break;
        }

        return new PuzzleRunResult(puzzle.Metadata, inputKind, partResults);
    }

    /// <summary>
    /// Executes one synchronous puzzle-solving method and measures its duration.
    /// </summary>
    /// <param name="part">The concrete puzzle part being executed.</param>
    /// <param name="input">The already loaded puzzle input.</param>
    /// <param name="solve">
    /// The synchronous method that solves the selected puzzle part.
    /// </param>
    /// <returns>The answer and execution duration for the selected puzzle part.</returns>
    private static PuzzlePartResult ExecutePart(PuzzlePart part, string input, Func<string, string> solve)
    {
        var stopWatch = Stopwatch.StartNew();
        var answer = solve(input);
        stopWatch.Stop();

        return new PuzzlePartResult(part, answer, stopWatch.Elapsed);
    }
}
