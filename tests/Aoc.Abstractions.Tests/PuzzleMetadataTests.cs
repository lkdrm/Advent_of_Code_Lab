using Aoc.Abstractions.Puzzles;

namespace Aoc.Abstractions.Tests;

/// <summary>
/// Contains automated checks for <see cref="PuzzleMetadata"/>.
/// </summary>
public sealed class PuzzleMetadataTests
{
    /// <summary>
    /// Verifies that valid puzzle metadata is stored correctly.
    /// </summary>
    [Fact]
    public void ConstructorWhenValuesAreValidSetsProperties()
    {
        // Arrange.
        var puzzleId = new PuzzleId(year: 2015, day: 3);

        // Act.
        var metadata = new PuzzleMetadata(
            id: puzzleId,
            title: "Perfectly Spherical Houses in a Vacuum",
            description: "Tracks houses visited by Santa.");

        // Assert.
        Assert.Equal(puzzleId, metadata.Id);
        Assert.Equal(
            "Perfectly Spherical Houses in a Vacuum",
            metadata.Title);
        Assert.Equal(
            "Tracks houses visited by Santa.",
            metadata.Description);
    }

    /// <summary>
    /// Verifies that the description is optional.
    /// </summary>
    [Fact]
    public void ConstructorWhenDescriptionIsNotProvidedSetsDescriptionToNull()
    {
        // Act.
        var metadata = new PuzzleMetadata(
            id: new PuzzleId(2015, 1),
            title: "Not Quite Lisp");

        // Assert.
        Assert.Null(metadata.Description);
    }

    /// <summary>
    /// Verifies that a null title is rejected.
    /// </summary>
    [Fact]
    public void ConstructorWhenTitleIsNullThrowsArgumentNullException()
    {
        // Act.
        var exception = Assert.Throws<ArgumentNullException>(
            () => new PuzzleMetadata(
                id: new PuzzleId(2015, 1),
                title: null!));

        // Assert.
        Assert.Equal("title", exception.ParamName);
    }

    /// <summary>
    /// Verifies that empty or whitespace-only titles are rejected.
    /// </summary>
    /// <param name="title">An invalid puzzle title.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void ConstructorWhenTitleIsEmptyOrWhitespaceThrowsArgumentException(
        string title)
    {
        // Act.
        var exception = Assert.Throws<ArgumentException>(
            () => new PuzzleMetadata(
                id: new PuzzleId(2015, 1),
                title: title));

        // Assert.
        Assert.Equal("title", exception.ParamName);
    }
}