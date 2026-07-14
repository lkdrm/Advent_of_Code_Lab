using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Application.Execution;
using Spectre.Console;

namespace Aoc.Cli;

/// <summary>
/// Handles the main interactive console menu for the Advent of Code CLI.
/// </summary>
/// <remarks>
/// This class owns only user interaction:
/// displaying menus, reading user choices, running the selected puzzle,
/// and showing the result.
///
/// Application setup, dependency injection, and service registration remain
/// in <c>Program.cs</c>.
/// </remarks>
public static class MainConsoleMenu
{
    /// <summary>
    /// Starts the interactive Advent of Code menu loop.
    /// </summary>
    /// <param name="executionService">
    /// The application service responsible for loading input and executing puzzles.
    /// </param>
    /// <param name="availablePuzzles">
    /// All puzzles registered in dependency injection and available for execution.
    /// </param>
    /// <param name="cancellationTokenSource">
    /// The cancellation source used to stop the menu when the user presses Ctrl+C.
    /// </param>
    /// <returns>
    /// A task that completes when the user chooses to exit the menu
    /// or when cancellation is requested.
    /// </returns>
    public static async Task ExecuteMenuAsync(IPuzzleExecutionService executionService, IPuzzle[] availablePuzzles, CancellationTokenSource cancellationTokenSource)
    {
        bool shouldContinue = true;

        while (shouldContinue && !cancellationTokenSource.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Advent of Code Laboratory").Centered());
            AnsiConsole.WriteLine();

            var selectedPuzzle = AnsiConsole.Prompt(
                new SelectionPrompt<IPuzzle>()
                .Title("[yellow]Choose a puzzle[/]")
                .PageSize(10)
                .MoreChoicesText("[blue](Move up and down to reveal more puzzles)[/]")
                .UseConverter(puzzles => Markup.Escape($"{puzzles.Metadata.Id} - {puzzles.Metadata.Title}"))
                .AddChoices(availablePuzzles));

            var selectedInputKind = AnsiConsole.Prompt(
                new SelectionPrompt<PuzzleInputKind>()
                    .Title("[yellow]Choose input kind[/]")
                    .UseConverter(ConsoleMenu.GetInputKindDisplayName)
                    .AddChoices(
                        PuzzleInputKind.Demo,
                        PuzzleInputKind.Personal));

            var selectedPuzzlePart = AnsiConsole.Prompt(
                new SelectionPrompt<PuzzlePart>()
                    .Title("[yellow]Choose puzzle part[/]")
                    .UseConverter(ConsoleMenu.GetPartDisplayName)
                    .AddChoices(
                        PuzzlePart.PartOne,
                        PuzzlePart.PartTwo,
                        PuzzlePart.Both));
            try
            {
                var results = await executionService.ExecuteAsync(
                    id: selectedPuzzle.Metadata.Id,
                    puzzlePart: selectedPuzzlePart,
                    inputKind: selectedInputKind,
                    cancellationToken: cancellationTokenSource.Token);

                ShowResults(results);
            }
            catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
            {
                break;
            }
            catch (FileNotFoundException ex)
            {
                ShowError($"Input file was not found. {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError($"Input file cannot be accessed. {ex.Message}");
            }
            catch (IOException ex)
            {
                ShowError($"Input file could not be read. {ex.Message}");
            }
            catch (Exception)
            {
                ShowError("An unexpected error occurred. Check the Logs directory for details.");
            }

            if (!cancellationTokenSource.IsCancellationRequested)
            {
                shouldContinue = AnsiConsole.Confirm("Run another puzzle?", defaultValue: true);
            }
        }
    }

    /// <summary>
    /// Displays puzzle execution results as a formatted Spectre.Console table.
    /// </summary>
    /// <param name="results">
    /// The result returned by the puzzle execution service.
    /// </param>
    private static void ShowResults(PuzzleRunResult results)
    {
        AnsiConsole.WriteLine();

        AnsiConsole.Write(new Rule(Markup.Escape($"{results.PuzzleMetadata.Id} - {results.PuzzleMetadata.Title}")).RuleStyle("blue"));

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Part")
            .AddColumn("Answer")
            .AddColumn("Duration");

        foreach (var result in results.PartResults)
        {
            table.AddRow(
                Markup.Escape(ConsoleMenu.GetPartDisplayName(result.PuzzlePart)),
                Markup.Escape(result.Answer),
                Markup.Escape($"{result.Duration.TotalMilliseconds:F3} ms"));
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Displays a user-friendly error message.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    private static void ShowError(string message)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(
            $"[red]Error:[/] {Markup.Escape(message)}");
        AnsiConsole.WriteLine();
    }
}
