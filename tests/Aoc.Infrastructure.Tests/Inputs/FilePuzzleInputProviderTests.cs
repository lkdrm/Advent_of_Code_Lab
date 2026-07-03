using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Aoc.Infrastructure.Inputs;

namespace Aoc.Infrastructure.Tests.Inputs;

/// <summary>
/// Contains automated checks for <see cref="FilePuzzleInputProvider"/>.
/// </summary>
public sealed class FilePuzzleInputProviderTests : IDisposable
{
    private readonly string _inputsRootPath;

    /// <summary>
    /// Creates an isolated temporary inputs directory for each test instance.
    /// </summary>
    public FilePuzzleInputProviderTests()
    {
        _inputsRootPath = Path.Combine(
            Path.GetTempPath(),
            "aoc-infrastructure-tests",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(_inputsRootPath);
    }

    /// <summary>
    /// Verifies that a committed demo input can be read asynchronously.
    /// </summary>
    [Fact]
    public async Task GetInputAsync_WhenDemoInputExists_ReturnsFileContent()
    {
        // Arrange.
        var puzzleId = new PuzzleId(year: 2015, day: 1);
        const string expectedInput = "()())";

        var inputPath = CreateInputPath(
            inputFolderName: "demo",
            puzzleId: puzzleId);

        await File.WriteAllTextAsync(inputPath, expectedInput);

        var provider = new FilePuzzleInputProvider(_inputsRootPath);

        // Act.
        var actualInput = await provider.GetInputAsync(
            id: puzzleId,
            inputKind: PuzzleInputKind.Demo,
            cancellationToken: CancellationToken.None);

        // Assert.
        Assert.Equal(expectedInput, actualInput);
    }

    /// <summary>
    /// Verifies that a local personal input can be read asynchronously.
    /// </summary>
    [Fact]
    public async Task GetInputAsync_WhenPersonalInputExists_ReturnsFileContent()
    {
        // Arrange.
        var puzzleId = new PuzzleId(year: 2015, day: 1);
        const string expectedInput = "((())";

        var inputPath = CreateInputPath(
            inputFolderName: "local",
            puzzleId: puzzleId);

        await File.WriteAllTextAsync(inputPath, expectedInput);

        var provider = new FilePuzzleInputProvider(_inputsRootPath);

        // Act.
        var actualInput = await provider.GetInputAsync(
            id: puzzleId,
            inputKind: PuzzleInputKind.Personal,
            cancellationToken: CancellationToken.None);

        // Assert.
        Assert.Equal(expectedInput, actualInput);
    }

    /// <summary>
    /// Verifies that a useful exception is thrown when the requested input file is missing.
    /// </summary>
    [Fact]
    public async Task GetInputAsync_WhenInputFileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Arrange.
        var provider = new FilePuzzleInputProvider(_inputsRootPath);
        var puzzleId = new PuzzleId(year: 2015, day: 1);

        // Act.
        var exception = await Assert.ThrowsAsync<FileNotFoundException>(
            () => provider.GetInputAsync(
                id: puzzleId,
                inputKind: PuzzleInputKind.Demo,
                cancellationToken: CancellationToken.None));

        // Assert.
        Assert.Contains("Puzzle input was not found", exception.Message);
    }

    /// <summary>
    /// Creates the expected folder structure and returns a file path
    /// compatible with <see cref="FilePuzzleInputProvider"/>.
    /// </summary>
    private string CreateInputPath(
        string inputFolderName,
        PuzzleId puzzleId)
    {
        var inputDirectoryPath = Path.Combine(
            _inputsRootPath,
            inputFolderName,
            puzzleId.Year.ToString());

        Directory.CreateDirectory(inputDirectoryPath);

        return Path.Combine(
            inputDirectoryPath,
            $"day{puzzleId.Day:D2}.txt");
    }

    /// <summary>
    /// Deletes the temporary directory created for this test instance.
    /// </summary>
    public void Dispose()
    {
        if (Directory.Exists(_inputsRootPath))
        {
            Directory.Delete(_inputsRootPath, recursive: true);
        }
    }
}