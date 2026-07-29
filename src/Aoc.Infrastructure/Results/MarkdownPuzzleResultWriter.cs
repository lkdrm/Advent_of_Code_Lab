using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Application.Execution;
using Aoc.Application.Results;

namespace Aoc.Infrastructure.Results;

/// <summary>
/// Persists puzzle execution results as structured Markdown files.
/// </summary>
/// <remarks>
/// Demo and personal results are stored separately. The writer records
/// calculated answers but never receives or writes raw puzzle input.
/// </remarks>
public sealed class MarkdownPuzzleResultWriter : IPuzzleResultWriter
{
    private readonly string _resultRootPath;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MarkdownPuzzleResultWriter"/> class.
    /// </summary>
    /// <param name="resultsRootPath">
    /// The root directory in which generated puzzle results are stored.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="resultsRootPath"/> is empty
    /// or contains only whitespace.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="resultsRootPath"/> is
    /// <see langword="null"/>.
    /// </exception>
    public MarkdownPuzzleResultWriter(string resultsRootPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resultsRootPath, nameof(resultsRootPath));

        _resultRootPath = Path.GetFullPath(resultsRootPath);
    }

    /// <inheritdoc />
    public async Task WriteAsync(PuzzleRunResult result, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(result);

        cancellationToken.ThrowIfCancellationRequested();

        var resultFilePath = GetResultFilePath(result);
        var resultDirectoryPath = Path.GetDirectoryName(resultFilePath)!;

        Directory.CreateDirectory(resultDirectoryPath);

        string? existingMarkdown = null;

        if (File.Exists(resultFilePath))
        {
            existingMarkdown = await File.ReadAllTextAsync(resultFilePath, cancellationToken);
        }

        var updatedMarkdown = existingMarkdown is null ? BuildMarkdown(result) : MergeMarkdown(existingMarkdown, result);

        if (string.Equals(existingMarkdown, updatedMarkdown, StringComparison.Ordinal))
        {
            return;
        }

        await WriteFileSafelyAsync(resultFilePath, updatedMarkdown, cancellationToken);
    }

    /// <summary>
    /// Writes content through a temporary file and then replaces the destination file to avoid leaving partial Markdown content.
    /// </summary>
    private static async Task WriteFileSafelyAsync(string filePath, string markdownResult, CancellationToken cancellationToken)
    {
        var tempFilePath = $"{filePath}.{Guid.NewGuid():N}.tmp";

        try
        {
            await File.WriteAllTextAsync(tempFilePath, markdownResult, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            File.Move(tempFilePath, filePath, true);
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    /// <summary>
    /// Creates the absolute output path for the supplied puzzle result.
    /// </summary>
    /// <param name="result">
    /// The puzzle result whose output path should be created.
    /// </param>
    /// <returns>
    /// The absolute Markdown file path for the puzzle result.
    /// </returns>
    private string GetResultFilePath(PuzzleRunResult result)
    {
        var inputDirectoryName = GetInputDirectoryName(result.PuzzleInputKind);

        var puzzleId = result.PuzzleMetadata.Id;

        return Path.Combine(_resultRootPath, inputDirectoryName, puzzleId.Year.ToString(), $"day{puzzleId.Day:D2}.md");
    }

    /// <summary>
    /// Converts a supported input kind into its result-directory name.
    /// </summary>
    /// <param name="puzzleInputKind">
    /// The input kind used for the puzzle execution.
    /// </param>
    /// <returns>
    /// The directory name associated with the input kind.
    /// </returns>
    private static string GetInputDirectoryName(PuzzleInputKind puzzleInputKind) =>
        puzzleInputKind switch
        {
            PuzzleInputKind.Demo => "demo",
            PuzzleInputKind.Personal => "personal"
        };

    /// <summary>
    /// Builds a complete Markdown document for a new puzzle-result file.
    /// </summary>
    private static string BuildMarkdown(PuzzleRunResult result)
    {
        var partResults = result.PartResults.ToDictionary(partResult => partResult.PuzzlePart);

        partResults.TryGetValue(PuzzlePart.PartOne, out var partOneResult);
        partResults.TryGetValue(PuzzlePart.PartTwo, out var partTwoResult);

        var puzzleId = result.PuzzleMetadata.Id;

        string[] lines =
        [
            $"# Advent of Code {puzzleId.Year}",
            string.Empty,
            $"## Day {puzzleId.Day:D2}: {result.PuzzleMetadata.Title}",
            string.Empty,
            $"**Input:** {result.PuzzleInputKind}",
            string.Empty,
            "| Part | Result |",
            "| --- | --- |",
            BuildPartRow(PuzzlePart.PartOne, partOneResult),
            BuildPartRow(PuzzlePart.PartTwo, partTwoResult),
        ];

        return string.Join('\n', lines) + '\n';
    }

    /// <summary>
    /// Merges newly calculated part results into an existing Markdown document.
    /// </summary>
    private static string MergeMarkdown(string existingMarkdown, PuzzleRunResult result)
    {
        var lines = existingMarkdown
            .ReplaceLineEndings("\n")
            .TrimEnd('\n')
            .Split('\n')
            .ToList();

        foreach (var partResult in result.PartResults)
        {
            ReplacePartRow(lines: lines, partResult: partResult);
        }

        return string.Join('\n', lines) + '\n';
    }

    /// <summary>
    /// Builds one Markdown table row for a puzzle part.
    /// </summary>
    private static string BuildPartRow(PuzzlePart puzzlePart, PuzzlePartResult? partResult)
    {
        var (displayName, marker) = GetPartPresentation(puzzlePart);

        var resultCell = partResult is null ? "_Not recorded yet._" : FormatAnswer(partResult.Answer);

        return $"| {displayName} {marker} | {resultCell} |";
    }

    /// <summary>
    /// Formats a puzzle answer for use inside a Markdown table cell.
    /// </summary>
    /// <param name="answer">The calculated puzzle answer.</param>
    /// <returns>A normalized inline-code value.</returns>
    private static string FormatAnswer(string answer)
    {
        var normalizedAnswer = answer
            .ReplaceLineEndings(" ")
            .Replace("|", "\\|", StringComparison.Ordinal);

        return $"`{normalizedAnswer}`";
    }

    /// <summary>
    /// Replaces exactly one marked result row in the Markdown document.
    /// </summary>
    private static void ReplacePartRow(List<string> lines, PuzzlePartResult partResult)
    {
        var (_, marker) = GetPartPresentation(partResult.PuzzlePart);

        var matchingIndexes = Enumerable
            .Range(0, lines.Count)
            .Where(index => lines[index].Contains(
                marker,
                StringComparison.Ordinal))
            .ToArray();

        lines[matchingIndexes[0]] = BuildPartRow(puzzlePart: partResult.PuzzlePart, partResult: partResult);
    }

    /// <summary>
    /// Returns the display name and stable Markdown marker for a puzzle part.
    /// </summary>
    private static (string DisplayName, string Marker) GetPartPresentation(PuzzlePart puzzlePart)
    {
        return puzzlePart switch
        {
            PuzzlePart.PartOne => ("Part One", "<!-- result:part-one -->"),
            PuzzlePart.PartTwo => ("Part Two", "<!-- result:part-two -->"),
        };
    }
}
