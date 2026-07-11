using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;

namespace Aoc.Infrastructure.Inputs;

/// <summary>
/// Loads Advent of Code puzzle inputs from files stored on the local file system.
/// </summary>
/// <remarks>
/// The provider supports two input locations:
///
/// <list type="bullet">
/// <item>
/// <description>
/// Demo inputs committed to the repository:
/// <c>inputs/demo/{year}/day{day:00}.txt</c>.
/// </description>
/// </item>
/// <item>
/// <description>
/// Personal inputs stored locally and excluded from Git:
/// <c>inputs/local/{year}/day{day:00}.txt</c>.
/// </description>
/// </item>
/// </list>
/// </remarks>
public sealed class FilePuzzleInputProvider : IPuzzleInputProvider
{
    private readonly string _inputsRootPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilePuzzleInputProvider"/> class.
    /// </summary>
    /// <param name="inputsRootPath">
    /// The absolute or relative path to the root <c>inputs</c> directory.
    /// </param>
    public FilePuzzleInputProvider(string inputsRootPath) => _inputsRootPath = Path.GetFullPath(inputsRootPath);

    /// <inheritdoc />
    public async Task<string> GetInputAsync(PuzzleId id, PuzzleInputKind inputKind, CancellationToken cancellationToken)
    {
        var inputPath = GetInputPath(id, inputKind);

        if (!File.Exists(inputPath))
        {
            throw new FileNotFoundException($"Puzzle input was not found: '{inputPath}'. " +
                "Add a demo input or create your local personal input file.",
                inputPath);
        }

        return await File.ReadAllTextAsync(inputPath, cancellationToken);
    }

    /// <summary>
    /// Builds the expected file path for a puzzle input.
    /// </summary>
    /// <param name="id">The year and day of the requested puzzle.</param>
    /// <param name="inputKind">Specifies the demo or personal input location.</param>
    /// <returns>The full path to the requested input file.</returns>
    private string GetInputPath(PuzzleId id, PuzzleInputKind inputKind)
    {
        var inputFolderName = inputKind switch
        {
            PuzzleInputKind.Demo => "demo",
            PuzzleInputKind.Personal => "local"
        };

        return Path.Combine(_inputsRootPath, inputFolderName, id.Year.ToString(), $"day{id.Day:D2}.txt");
    }
}

