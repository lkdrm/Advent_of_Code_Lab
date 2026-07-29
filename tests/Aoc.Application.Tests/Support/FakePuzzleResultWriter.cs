using Aoc.Application.Execution;
using Aoc.Application.Results;

namespace Aoc.Application.Tests.Support;

/// <summary>
/// A controllable puzzle-result writer used by application-layer tests.
/// </summary>
/// <remarks>
/// The fake does not access the file system. It records the result received
/// from PuzzleExecutionService and can optionally simulate a write failure.
/// </remarks>
public sealed class FakePuzzleResultWriter : IPuzzleResultWriter
{
    private readonly Exception? _exceptionToThrow;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="FakePuzzleResultWriter"/> class.
    /// </summary>
    /// <param name="exceptionToThrow">
    /// An optional exception that should be returned by the write operation.
    /// </param>
    public FakePuzzleResultWriter(Exception? exceptionToThrow = default)
    {
        _exceptionToThrow = exceptionToThrow;
    }

    /// <summary>
    /// Gets the number of write requests received by the fake.
    /// </summary>
    public int CallCount { get; private set; }

    /// <summary>
    /// Gets the puzzle run result received by the latest write request.
    /// </summary>
    public PuzzleRunResult? WrittenResult { get; private set; }

    /// <summary>
    /// Gets the cancellation token received by the latest write request.
    /// </summary>
    public CancellationToken ReceivedCancellationToken { get; private set; }

    /// <summary>
    /// Records the supplied puzzle result without accessing the file system.
    /// </summary>
    /// <param name="result">The puzzle run result to record.</param>
    /// <param name="cancellationToken">
    /// The cancellation token supplied by the application service.
    /// </param>
    /// <returns>A task representing the simulated write operation.</returns>
    public Task WriteAsync(
        PuzzleRunResult result,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(result);

        cancellationToken.ThrowIfCancellationRequested();

        CallCount++;
        WrittenResult = result;
        ReceivedCancellationToken = cancellationToken;

        return _exceptionToThrow is null
            ? Task.CompletedTask
            : Task.FromException(_exceptionToThrow);
    }
}