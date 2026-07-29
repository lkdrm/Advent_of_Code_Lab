using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Application.Execution;
using Aoc.Infrastructure.Results;

namespace Aoc.Infrastructure.Tests.Results;

/// <summary>
/// Contains automated checks for
/// <see cref="MarkdownPuzzleResultWriter"/>.
/// </summary>
public sealed class MarkdownPuzzleResultWriterTests : IDisposable
{
    private readonly string _resultsRootPath;

    /// <summary>
    /// Creates an isolated temporary results directory
    /// for each test instance.
    /// </summary>
    public MarkdownPuzzleResultWriterTests()
    {
        _resultsRootPath = Path.Combine(
            Path.GetTempPath(),
            "aoc-result-writer-tests",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(_resultsRootPath);
    }

    /// <summary>
    /// Verifies that a Demo Part One result creates a Markdown file
    /// in the expected year and day directory.
    /// </summary>
    [Fact]
    public async Task WriteAsyncWhenDemoPartOneResultIsProvidedCreatesExpectedMarkdownFile()
    {
        // Arrange.
        var puzzleMetadata = new PuzzleMetadata(
            id: new PuzzleId(year: 2015, day: 3),
            title: "Perfectly Spherical Houses in a Vacuum");

        var partOneResult = new PuzzlePartResult(
            puzzlePart: PuzzlePart.PartOne,
            answer: "4",
            duration: TimeSpan.FromMilliseconds(12));

        var runResult = new PuzzleRunResult(
            puzzleMetadata: puzzleMetadata,
            inputKind: PuzzleInputKind.Demo,
            partResults: [partOneResult]);

        var writer = new MarkdownPuzzleResultWriter(
            resultsRootPath: _resultsRootPath);

        var expectedPath = Path.Combine(
            _resultsRootPath,
            "demo",
            "2015",
            "day03.md");

        var expectedMarkdown = """
                # Advent of Code 2015

                ## Day 03: Perfectly Spherical Houses in a Vacuum

                **Input:** Demo

                | Part | Result |
                | --- | --- |
                | Part One <!-- result:part-one --> | `4` |
                | Part Two <!-- result:part-two --> | _Not recorded yet._ |
                """
            .ReplaceLineEndings("\n") + "\n";

        // Act.
        await writer.WriteAsync(
            result: runResult,
            cancellationToken: CancellationToken.None);

        // Assert.
        Assert.True(File.Exists(expectedPath));

        var actualMarkdown = await File.ReadAllTextAsync(expectedPath);

        Assert.Equal(expectedMarkdown, actualMarkdown);
        Assert.DoesNotContain("Duration", actualMarkdown);
        Assert.DoesNotContain("12", actualMarkdown);
    }

    /// <summary>
    /// Deletes the temporary directory created for this test instance.
    /// </summary>
    public void Dispose()
    {
        if (Directory.Exists(_resultsRootPath))
        {
            Directory.Delete(
                _resultsRootPath,
                recursive: true);
        }
    }

    /// <summary>
    /// Verifies that writing Part Two after Part One preserves
    /// both previously and newly calculated answers.
    /// </summary>
    [Fact]
    public async Task WriteAsyncWhenPartTwoIsWrittenAfterPartOnePreservesBothAnswers()
    {
        // Arrange.
        var puzzleMetadata = new PuzzleMetadata(
            id: new PuzzleId(year: 2015, day: 3),
            title: "Perfectly Spherical Houses in a Vacuum");

        var partOneRunResult = new PuzzleRunResult(
            puzzleMetadata: puzzleMetadata,
            inputKind: PuzzleInputKind.Demo,
            partResults:
            [
                new PuzzlePartResult(
                puzzlePart: PuzzlePart.PartOne,
                answer: "4",
                duration: TimeSpan.FromMilliseconds(10)),
            ]);

        var partTwoRunResult = new PuzzleRunResult(
            puzzleMetadata: puzzleMetadata,
            inputKind: PuzzleInputKind.Demo,
            partResults:
            [
                new PuzzlePartResult(
                puzzlePart: PuzzlePart.PartTwo,
                answer: "3",
                duration: TimeSpan.FromMilliseconds(20)),
            ]);

        var writer = new MarkdownPuzzleResultWriter(
            resultsRootPath: _resultsRootPath);

        var resultPath = Path.Combine(
            _resultsRootPath,
            "demo",
            "2015",
            "day03.md");

        // Act.
        await writer.WriteAsync(
            result: partOneRunResult,
            cancellationToken: CancellationToken.None);

        await writer.WriteAsync(
            result: partTwoRunResult,
            cancellationToken: CancellationToken.None);

        // Assert.
        var markdown = await File.ReadAllTextAsync(resultPath);

        Assert.Contains(
            "| Part One <!-- result:part-one --> | `4` |",
            markdown);

        Assert.Contains(
            "| Part Two <!-- result:part-two --> | `3` |",
            markdown);

        Assert.DoesNotContain(
            "_Not recorded yet._",
            markdown);
    }
}