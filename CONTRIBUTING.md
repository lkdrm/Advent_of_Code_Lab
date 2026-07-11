# Contributing to Advent of Code Lab

## Development workflow

All changes must be developed in a separate branch.

Example:

```bash
git switch main
git pull
git switch -c issue/2015/DayXX
```

Changes must be submitted through a pull request targeting the `main` branch.

Direct changes to `main` are not allowed.

## Definition of Done for every puzzle day

A puzzle day is considered complete only when it includes:

- an `IPuzzle` implementation;
- correct `PuzzleMetadata`;
- Part One implementation;
- Part Two implementation;
- dependency-injection registration;
- a committed demo input;
- unit tests for both parts;
- a step-by-step Markdown guide;
- successful execution through the CLI;
- required XML documentation comments;
- a successful CI run.

## Documentation comments

XML documentation comments are required for every puzzle day.

Documentation must be added to:

- the `DayXX` class;
- the `Metadata` property;
- `SolvePartOne`;
- `SolvePartTwo`;
- shared helper methods;
- nested types and their important methods;
- the corresponding test class;
- every test method.

Comments should explain the purpose and expected behavior of the code.

Comments should not simply repeat the implementation line by line.

## Demo input

Every puzzle day must include a demo input:

```text
src/Aoc.Abstractions/Inputs/demo/{year}/dayXX.txt
```

Personal Advent of Code input must not be committed.

Personal input should be stored in:

```text
src/Aoc.Abstractions/Inputs/local/{year}/dayXX.txt
```

## Testing requirements

Before creating a pull request, run the complete test suite:

```bash
dotnet test AdventOfCodeLab.slnx
```

The new puzzle must also be verified through the CLI:

```bash
dotnet run --project src/Aoc.Cli
```

Both puzzle parts must return the expected results for the demo input.

## Pull request requirements

Every change must be submitted through a pull request targeting `main`.

The pull request must contain:

- a clear title;
- a summary of the changes;
- local test results;
- demo results;
- a completed pull request checklist.

## CI and merge policy

A pull request must not be merged while the required CI check is:

- pending;
- failing;
- cancelled;
- skipped.

A pull request may be merged only after the required `Build and test` check has completed successfully.

If additional commits are pushed to the pull request, CI must pass again before merge.

The `main` branch must always remain buildable and fully tested.