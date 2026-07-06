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
    public PuzzleExecutionService(IEnumerable<IPuzzle> puzzles, IPuzzleInputProvider inputProvider)
    {
        _puzzles = puzzles.ToDictionary(puzzle => puzzle.Metadata.Id, puzzle => puzzle);
        _inputProvider = inputProvider;
    }

    /// <inheritdoc />
    public async Task<PuzzleRunResult> ExecuteAsync(PuzzleId id, PuzzlePart puzzlePart, PuzzleInputKind inputKind, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

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
