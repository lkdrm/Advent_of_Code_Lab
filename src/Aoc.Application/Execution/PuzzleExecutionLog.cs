using Aoc.Abstractions.Inputs;
using Aoc.Abstractions.Puzzles;
using Microsoft.Extensions.Logging;

namespace Aoc.Application.Execution;

internal static partial class PuzzleExecutionLog
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Information, Message = "Starting puzzle {PuzzleId}, part {PuzzlePart}, using {InputKind} input.")]
    internal static partial void ExecutionStarted(this ILogger<PuzzleExecutionService> logger, PuzzleId puzzleId, PuzzlePart puzzlePart, PuzzleInputKind inputKind);

    [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Completed puzzle {PuzzleId}, part {PuzzlePart}, in {DurationMilliseconds} ms.")]
    internal static partial void PuzzlePartCompleted(this ILogger<PuzzleExecutionService> logger, PuzzleId puzzleId, PuzzlePart puzzlePart, double durationMilliseconds);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Information, Message = "Cancelled puzzle {PuzzleId}, part {PuzzlePart}, using {InputKind} input.")]
    internal static partial void ExecutionCancelled(this ILogger<PuzzleExecutionService> logger, PuzzleId puzzleId, PuzzlePart puzzlePart, PuzzleInputKind inputKind);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Error, Message = "Failed puzzle {PuzzleId}, part {PuzzlePart}, using {InputKind} input.")]
    internal static partial void ExecutionFailed(this ILogger<PuzzleExecutionService> logger, PuzzleId puzzleId, PuzzlePart puzzlePart, PuzzleInputKind inputKind, Exception exception);
}
