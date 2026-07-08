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

var avaiblePuzzles = serviceProvider
    .GetServices<IPuzzle>()
    .OrderBy(puzzle => puzzle.Metadata.Id.Year)
    .ThenBy(puzzle => puzzle.Metadata.Id.Day)
    .ToArray();

var selectedPuzzle = SelectOption(title: "Select puzzle",
    options: avaiblePuzzles,
    getDisplayText: puzzle => $"{puzzle.Metadata.Id} - {puzzle.Metadata.Title}");

var selectedInputKind = SelectOption(title: "Select input kind",
    options: [PuzzleInputKind.Demo, PuzzleInputKind.Personal],
    getDisplayText: GetInputKindDisplayName);

var selectedPuzzlePart = SelectOption(title: "Select puzzle part",
    options: [PuzzlePart.PartOne, PuzzlePart.PartTwo, PuzzlePart.Both],
    getDisplayText: GetPartDisplayName);

var executionService = serviceProvider.GetRequiredService<IPuzzleExecutionService>();

var result = await executionService.ExecuteAsync(
    id: selectedPuzzle.Metadata.Id,
    puzzlePart: selectedPuzzlePart,
    inputKind: selectedInputKind,
    cancellationToken: cts.Token);

Console.WriteLine();
Console.WriteLine($"{result.PuzzleMetadata.Id} - {result.PuzzleMetadata.Title}");
Console.WriteLine(new string('-', $"{result.PuzzleMetadata.Id} - {result.PuzzleMetadata.Title}".Length));

foreach (var partResult in result.PartResults)
{
    Console.WriteLine($"{GetPartDisplayName(partResult.PuzzlePart)} : {partResult.Answer}");
    Console.WriteLine($"Execution time: {partResult.Duration.TotalMilliseconds:F3} ms");
}

/// <summary>
/// Displays a numbered list of options and asks the user to select one.
/// </summary>
/// <typeparam name="T">The type of option being selected.</typeparam>
/// <param name="title">The title displayed above the options.</param>
/// <param name="options">The available options.</param>
/// <param name="getDisplayText">A function that converts an option to readable text.</param>
/// <returns>The selected option.</returns>
static T SelectOption<T>(string title, IReadOnlyList<T> options, Func<T, string> getDisplayText)
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
static string GetInputKindDisplayName(PuzzleInputKind puzzleInputKind) =>
    puzzleInputKind switch
    {
        PuzzleInputKind.Demo => "Demo input",
        PuzzleInputKind.Personal => "Personal input"
    };

/// <summary>
/// Converts a puzzle part into a readable CLI label.
/// </summary>
static string GetPartDisplayName(PuzzlePart puzzlePart) =>
    puzzlePart switch
    {
        PuzzlePart.PartOne => "Part one",
        PuzzlePart.PartTwo => "Part two",
        PuzzlePart.Both => "Both parts"
    };


Console.ReadLine();