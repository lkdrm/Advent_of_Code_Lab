namespace Aoc.Infrastructure.Inputs;

/// <summary>
/// Resolves the directory by searching the directory hierarchy.
/// </summary>
public static class RootPathResolver
{
    /// <summary>
    /// Resolves the Inputs directory starting from the specified path.
    /// </summary>
    /// <param name="startPath">The directory from which the search starts.</param>
    /// <returns>The absolute path to the Inputs directory.</returns>
    public static string Inputs(string startPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(startPath);

        DirectoryInfo? currentDirectory = new(Path.GetFullPath(startPath));

        while (currentDirectory != null)
        {
            var currentDirectoryPath = Path.Combine(currentDirectory.FullName, "Inputs");

            if (Directory.Exists(currentDirectoryPath))
            {
                return currentDirectoryPath;
            }

            currentDirectory = currentDirectory.Parent;
        }

        throw new DirectoryNotFoundException($"Could not find the Inputs directory starting from '{startPath}'.");
    }

    /// <summary>
    /// Resolves the Results directory starting from the specified path.
    /// </summary>
    /// <param name="startPath">The directory from which the search starts.</param>
    /// <returns>The absolute path to the Results directory.</returns>
    public static string Results(string startPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(startPath);

        DirectoryInfo? currentDirectory = new(Path.GetFullPath(startPath));

        var currentDirectoryPath = Path.Combine(currentDirectory.FullName, "Results");

        if (Directory.Exists(currentDirectoryPath))
        {
            return currentDirectoryPath;
        }
        else
        {
            Directory.CreateDirectory(currentDirectoryPath);
            return currentDirectoryPath;
        }
    }
}
