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
    public async Task GetInputAsyncWhenDemoInputExistsReturnsFileContent()
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
    public async Task GetInputAsyncWhenPersonalInputExistsReturnsFileContent()
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
    public async Task GetInputAsyncWhenInputFileDoesNotExistThrowsFileNotFoundException()
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

    /// <summary>
    /// Verifies that the resolver finds the Inputs directory in a parent directory.
    /// </summary>
    [Fact]
    public void ResolveWhenInputsDirectoryExistsInParentReturnsInputsPath()
    {
        // Arrange.
        var rootPath = Path.Combine(
            Path.GetTempPath(),
            "aoc-input-path-tests",
            Guid.NewGuid().ToString("N"));

        var expectedPath = Path.Combine(rootPath, "Inputs");
        var nestedPath = Path.Combine(
            rootPath,
            "src",
            "Aoc.Cli",
            "bin",
            "Debug",
            "net10.0");

        Directory.CreateDirectory(expectedPath);
        Directory.CreateDirectory(nestedPath);

        try
        {
            // Act.
            var result = InputsRootPathResolver.Resolve(nestedPath);

            // Assert.
            Assert.Equal(expectedPath, result);
        }
        finally
        {
            Directory.Delete(rootPath, recursive: true);
        }
    }

    /// <summary>
    /// Verifies that a missing Inputs directory produces a clear exception.
    /// </summary>
    [Fact]
    public void ResolveWhenInputsDirectoryDoesNotExistThrowsDirectoryNotFoundException()
    {
        // Arrange.
        var startPath = Path.Combine(
            Path.GetTempPath(),
            "aoc-input-path-tests",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(startPath);

        try
        {
            // Act.
            var exception = Assert.Throws<DirectoryNotFoundException>(
                () => InputsRootPathResolver.Resolve(startPath));

            // Assert.
            Assert.Contains(startPath, exception.Message);
        }
        finally
        {
            Directory.Delete(startPath, recursive: true);
        }
    }
}