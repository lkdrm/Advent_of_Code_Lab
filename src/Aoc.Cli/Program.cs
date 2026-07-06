using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Application;
using Aoc.Application.Execution;
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
services.AddApplication();

using var serviceProvider = services.BuildServiceProvider();

var executionService = serviceProvider.GetRequiredService<IPuzzleExecutionService>();

var result = await executionService.ExecuteAsync(
    id: new PuzzleId(year: 2015, day: 1),
    puzzlePart: PuzzlePart.Both,
    inputKind: PuzzleInputKind.Demo,
    cancellationToken: cts.Token);

Console.WriteLine($"{result.PuzzleMetadata.Id} - {result.PuzzleMetadata.Title}");
Console.WriteLine(new string('-', 45));

foreach (var partResult in result.PartResults)
{
    Console.WriteLine(
        $"{GetPartDisplayName(partResult.PuzzlePart)}: {partResult.Answer}");

    Console.WriteLine(
        $"Execution time: {partResult.Duration.TotalMilliseconds:F3} ms");
}

/// <summary>
/// Converts a concrete puzzle part into a readable CLI label.
/// </summary>
static string GetPartDisplayName(PuzzlePart puzzlePart) =>
    puzzlePart switch
    {
        PuzzlePart.PartOne => "Part One",
        PuzzlePart.PartTwo => "Part Two",
    };

Console.ReadLine();