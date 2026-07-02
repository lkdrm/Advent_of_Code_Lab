using Aoc.Abstractions.Puzzles;
using Aoc.Year2015;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddYear2015Puzzles();

using var seriviceProvider = services.BuildServiceProvider();

var puzzles = seriviceProvider.GetRequiredService<IEnumerable<IPuzzle>>()
    .OrderBy(puzzle => puzzle.Metadata.Id.Year)
    .ThenBy(puzzle => puzzle.Metadata.Id.Day);

Console.WriteLine("Registered Advent of Code puzzles:");
Console.WriteLine();

foreach (var puzzle in puzzles)
{
    Console.WriteLine($"{puzzle.Metadata.Id}/{puzzle.Metadata.Title}");
}