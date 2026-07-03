using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Infrastructure.Inputs;
using Aoc.Year2015;
using Microsoft.Extensions.DependencyInjection;

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cts.Cancel();
};

var rootRepositoryPath = Directory.GetCurrentDirectory();
var inputsRootPath = Path.Combine(rootRepositoryPath, "Inputs");

var services = new ServiceCollection();
services.AddSingleton<IPuzzleInputProvider>(_ => new FilePuzzleInputProvider(inputsRootPath));
services.AddYear2015Puzzles();

using var serviceProvider = services.BuildServiceProvider();

var puzzles = serviceProvider.GetRequiredService<IEnumerable<IPuzzle>>()
    .OrderBy(puzzle => puzzle.Metadata.Id.Year)
    .ThenBy(puzzle => puzzle.Metadata.Id.Day);


var selectedPuzzleId = new PuzzleId(2015, 1);

var selectedPuzzle = serviceProvider
    .GetServices<IPuzzle>()
    .Single(puzzle => puzzle.Metadata.Id == selectedPuzzleId);

var inputProvider = serviceProvider.GetRequiredService<IPuzzleInputProvider>();

var input = inputProvider.GetInputAsync(selectedPuzzleId, PuzzleInputKind.Demo, cts.Token);

var partOneAnswer = selectedPuzzle.SolvePartOne(await input);
var partTwoAnswer = selectedPuzzle.SolvePartTwo(await input);

Console.WriteLine($"{selectedPuzzle.Metadata.Id} — {selectedPuzzle.Metadata.Title}");
Console.WriteLine(new string('-', 45));
Console.WriteLine($"Part One: {partOneAnswer}");
Console.WriteLine($"Part Two: {partTwoAnswer}");