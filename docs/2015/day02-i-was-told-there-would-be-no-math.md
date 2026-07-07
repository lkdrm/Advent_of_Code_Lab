# Advent of Code 2015 — Day 02: I Was Told There Would Be No Math

> A step-by-step explanation of the parsing, geometry calculations, tests, and application flow used in this repository.

## Goal of this guide

This guide explains how the solution calculates:

- wrapping paper required for each present;
- ribbon required for each present;
- totals for multiple presents in one input file;
- input parsing from `length x width x height` lines;
- test coverage and application flow.

## Problem summary

Each input line describes one rectangular present:

```text
length x width x height
```

For example:

```text
2x3x4
```

means:

```text
length = 2
width = 3
height = 4
```

The puzzle has two parts:

1. Calculate the total wrapping paper required for all presents.
2. Calculate the total ribbon required for all presents.

## Examples

| Present dimensions | Part One: wrapping paper | Part Two: ribbon |
| --- | ---: | ---: |
| `2x3x4` | `58` | `34` |
| `1x1x10` | `43` | `14` |

The demo input contains both examples:

```text
2x3x4
1x1x10
```

Its expected totals are:

| Part | Total |
| --- | ---: |
| Part One | `101` |
| Part Two | `48` |

## Part One: wrapping paper

For each present, Part One requires:

```text
surface area of every face
+ area of the smallest face as extra slack
```

A rectangular present has three unique face areas:

```text
length × width
width × height
height × length
```

Each face appears twice on the box, so the surface area is:

```text
2 × (length × width + width × height + height × length)
```

For `2x3x4`:

```text
length × width = 2 × 3 = 6
width × height = 3 × 4 = 12
height × length = 4 × 2 = 8

surface area = 2 × (6 + 12 + 8) = 52
smallest face area = 6
total wrapping paper = 52 + 6 = 58
```

## Part Two: ribbon

For each present, Part Two requires:

```text
the smallest perimeter around the present
+ ribbon for the bow
```

The smallest perimeter uses the two smallest dimensions.

For `2x3x4`, the two smallest dimensions are `2` and `3`:

```text
smallest perimeter = 2 × (2 + 3) = 10
```

The bow length is defined by the puzzle as the present volume:

```text
length × width × height
```

For `2x3x4`:

```text
bow = 2 × 3 × 4 = 24
total ribbon = 10 + 24 = 34
```

The implementation calculates all three possible perimeters:

```text
2 × (length + width)
2 × (width + height)
2 × (height + length)
```

Then it chooses the smallest one.

For `1x1x10`:

```text
smallest perimeter = 2 × (1 + 1) = 4
bow = 1 × 1 × 10 = 10
total ribbon = 4 + 10 = 14
```

## Parsing and implementation

The input contains one present per line:

```text
2x3x4
1x1x10
```

The solution processes the input in two stages:

```text
input string
→ split into non-empty lines
→ parse each line into length, width, and height
→ calculate the required value for each present
→ sum all present totals
```

`Day02` uses `ParsePresents` to split the input safely:

```csharp
var lines = input.Split(
    '\n',
    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
```

This handles:

- multiple presents in one file;
- a trailing line break at the end of the file;
- Windows-style line endings;
- accidental surrounding whitespace.

Each line must contain exactly three dimensions:

```text
length x width x height
```

For example:

```text
2x3x4
```

becomes:

```text
Length = 2
Width = 3
Height = 4
```

The implementation validates that every dimension is a positive integer.

Invalid examples include:

```text
2x3
2x3x4x5
2x0x4
2x-3x4
2xwidthx4
```

## `PresentDimensions`

The solution uses a small private type called `PresentDimensions`.

```text
PresentDimensions
→ represents one rectangular present
→ stores Length, Width, and Height
→ calculates wrapping paper
→ calculates ribbon
```

This keeps the responsibilities separated:

```text
Day02
→ parses the full input
→ sums totals for all presents

PresentDimensions
→ knows the geometry of one present
→ calculates paper and ribbon for one present
```

The type is private because it belongs only to Day 02.

It is not a general-purpose model for the entire application, so exposing it publicly would add unnecessary API surface.

## Complexity

Let `n` be the total number of characters in the input.

| Operation | Time complexity | Additional memory | Reason |
| --- | ---: | ---: | --- |
| Parsing input | `O(n)` | `O(n)` | The input is split into lines and each line is inspected. |
| Calculating one present | `O(1)` | `O(1)` | A present always has exactly three dimensions and three face combinations. |
| Calculating all presents | `O(n)` | `O(n)` | Every input line is parsed and processed once. |

The geometry calculation itself is constant time because a rectangular present always has:

```text
3 unique face areas
3 possible perimeters
1 volume
```

The current implementation uses `string.Split`, which creates a collection of input lines. That is why the total additional memory is `O(n)`.

This is completely acceptable for Advent of Code inputs and keeps the parsing logic simple and readable.

## Tests

Day 02 is covered by unit tests in `Aoc.Year2015.Tests`.

The tests verify the official examples for individual presents:

```text
2x3x4
→ Part One: 58
→ Part Two: 34

1x1x10
→ Part One: 43
→ Part Two: 14
```

The tests also verify totals for multiple presents:

```text
2x3x4
1x1x10

→ Part One total: 101
→ Part Two total: 48
```

The multi-present tests include Windows-style line endings and a trailing line break:

```text
2x3x4\r\n
1x1x10\r\n
```

This confirms that `ParsePresents` handles normal text-file input correctly.

Running all tests locally:

```powershell
dotnet test AdventOfCodeLab.slnx
```

At this point, the solution contains `23` passing tests.

## Application flow

Day 02 follows the same execution flow as Day 01:

```text
Aoc.Cli
→ IPuzzleExecutionService
→ IPuzzleInputProvider
→ FilePuzzleInputProvider
→ inputs/demo/2015/day02.txt

IPuzzleExecutionService
→ finds Day02 through the IPuzzle registry
→ loads the input asynchronously
→ runs Part One, Part Two, or both
→ measures execution time
→ returns PuzzleRunResult

Aoc.Cli
→ displays the answers and execution times
```

For the current demo run, the CLI requests:

```text
Puzzle: 2015 / Day 02
Part: Both
Input: Demo
```

The demo input is committed to the repository:

```text
inputs/demo/2015/day02.txt
```

Personal Advent of Code inputs can be stored separately:

```text
inputs/local/2015/day02.txt
```

The `inputs/local` directory is ignored by Git.

## What this day demonstrates

Day 02 adds input parsing and simple geometry calculations to the project architecture.

It demonstrates:

- parsing structured text input;
- validating positive integer dimensions;
- separating input parsing from present-level calculations;
- modelling one present with `PresentDimensions`;
- calculating surface area, slack, perimeters, and volume;
- testing both individual examples and total calculations;
- running the puzzle through the same DI and application flow as Day 01.