using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Application;
using Aoc.Application.Execution;
using Aoc.Cli;
using Aoc.Infrastructure.Inputs;
using Aoc.Year2015;
using Microsoft.Extensions.DependencyInjection;

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cts.Cancel();
};

var rootRepositoryPath = InputsRootPathResolver.Resolve(AppContext.BaseDirectory);

var services = new ServiceCollection();
services.AddSingleton<IPuzzleInputProvider>(_ => new FilePuzzleInputProvider(rootRepositoryPath));
services.AddYear2015Puzzles();
services.AddApplication();

using var serviceProvider = services.BuildServiceProvider();

var availablePuzzles = serviceProvider
    .GetServices<IPuzzle>()
    .OrderBy(puzzle => puzzle.Metadata.Id.Year)
    .ThenBy(puzzle => puzzle.Metadata.Id.Day)
    .ToArray();

var executionService = serviceProvider.GetRequiredService<IPuzzleExecutionService>();

await MainConsoleMenu.ExecuteMenuAsync(executionService, availablePuzzles, cts);
