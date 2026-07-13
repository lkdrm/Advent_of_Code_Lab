namespace Aoc.Infrastructure.Inputs;

/// <summary>
/// Resolves the Inputs directory by searching the directory hierarchy.
/// </summary>
public static class InputsRootPathResolver
{
    /// <summary>
    /// Resolves the Inputs directory starting from the specified path.
    /// </summary>
    /// <param name="startPath">The directory from which the search starts.</param>
    /// <returns>The absolute path to the Inputs directory.</returns>
    public static string Resolve(string startPath)
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
}
