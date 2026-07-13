using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Application;
using Aoc.Application.Execution;
using Aoc.Cli;
using Aoc.Infrastructure.Inputs;
using Aoc.Year2015;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cts.Cancel();
};

var inputsRootPath = InputsRootPathResolver.Resolve(AppContext.BaseDirectory);
var logsRootPath = Path.Combine(AppContext.BaseDirectory, "Logs");

Directory.CreateDirectory(logsRootPath);

using var diagnosticLogger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.File(
        formatter: new RenderedCompactJsonFormatter(),
        path: Path.Combine(logsRootPath, "aoc-.json"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 14,
        fileSizeLimitBytes: 5 * 1024 * 1024,
        rollOnFileSizeLimit: true)
    .CreateLogger();

var services = new ServiceCollection();
services.AddSingleton<IPuzzleInputProvider>(_ => new FilePuzzleInputProvider(inputsRootPath));
services.AddYear2015Puzzles();
services.AddApplication();
services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddSerilog(diagnosticLogger);
});

using var serviceProvider = services.BuildServiceProvider();

var availablePuzzles = serviceProvider
    .GetServices<IPuzzle>()
    .OrderBy(puzzle => puzzle.Metadata.Id.Year)
    .ThenBy(puzzle => puzzle.Metadata.Id.Day)
    .ToArray();

var executionService = serviceProvider.GetRequiredService<IPuzzleExecutionService>();

await MainConsoleMenu.ExecuteMenuAsync(executionService, availablePuzzles, cts);
