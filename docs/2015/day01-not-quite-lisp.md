# Advent of Code 2015 — Day 01: Not Quite Lisp

> A step-by-step explanation of the solution, tests, and application flow used in this repository.

## Goal of this guide

This guide explains more than the final answer.

It covers:

- how the puzzle works;
- how Part One and Part Two differ;
- why the algorithm is efficient;
- how the solution is tested;
- how the puzzle runs through the application architecture.

## Problem summary

The input is a sequence of parentheses.

Each opening parenthesis moves Santa one floor up:

```text
( → +1
```

Each closing parenthesis moves Santa one floor down:

```text
) → -1
```

The puzzle has two parts:

1. Calculate Santa's final floor after processing the entire input.
2. Find the first position where Santa enters the basement, which is floor `-1`.

In this implementation, every character other than `(` and `)` has no effect. This also means a trailing line break in an input file does not change the result.

## Examples

| Input | Final floor | First basement position |
| --- | ---: | ---: |
| `(())` | `0` | Never enters |
| `()()` | `0` | Never enters |
| `)` | `-1` | `1` |
| `()())` | `-1` | `5` |

Positions in Part Two are one-based.

For example, in this input:

```text
()())
12345
```

Santa enters the basement after processing character `5`.

## Algorithm

Both parts process the input from left to right and keep one variable:

```csharp
var floor = 0;
```

For every character:

```text
( → increase floor by 1
) → decrease floor by 1
anything else → do nothing
```

The shared conversion is implemented in `GetFloorChange`:

```csharp
private static int GetFloorChange(char character) =>
    character switch
    {
        '(' => 1,
        ')' => -1,
        _ => 0
    };
```

### Part One

Part One must process the entire input because the answer is the final floor.

```text
Start at floor 0
→ process every character
→ apply its floor change
→ return the final floor
```

Example for `()())`:

| Position | Character | Floor after processing |
| ---: | --- | ---: |
| Start | — | `0` |
| `1` | `(` | `1` |
| `2` | `)` | `0` |
| `3` | `(` | `1` |
| `4` | `)` | `0` |
| `5` | `)` | `-1` |

The final answer is `-1`.

### Part Two

Part Two has an additional condition: return immediately when Santa first reaches floor `-1`.

```text
Start at floor 0
→ process one character
→ update floor
→ check whether floor is -1
→ return its one-based position when the condition is true
```

The important detail is that the floor must be updated before checking it.

For this input:

```text
()())
12345
```

Santa reaches floor `-1` after processing the fifth character, so the answer is `5`.

In C#, string indexes are zero-based, but the puzzle uses one-based positions:

```csharp
return (index + 1).ToString();
```

Without `+ 1`, the solution would return `4`, which would be incorrect.

## Complexity

Let `n` be the number of characters in the input.

| Part | Time complexity | Space complexity | Why |
| --- | --- | --- | --- |
| Part One | `O(n)` | `O(1)` | Every character is processed once, while only the current floor is stored. |
| Part Two | `O(n)` worst case | `O(1)` | The input is processed until the first basement position is found. In the worst case, this can be the final character or never occur. |

`O(1)` space means that memory usage does not grow with the input size.

The solution stores only a few values:

```text
current floor
current index
current character
```

It does not create another string, array, or collection based on the input length.

## Common mistakes

### Returning a zero-based index

C# strings use zero-based indexes:

```text
first character → index 0
second character → index 1
```

The puzzle uses one-based positions:

```text
first character → position 1
second character → position 2
```

That is why Part Two returns:

```csharp
return (index + 1).ToString();
```

### Checking the floor before applying the current character

This order is incorrect:

```csharp
if (floor == -1)
{
    return (index + 1).ToString();
}

floor += GetFloorChange(input[index]);
```

It checks the floor from the previous character, so the answer would be one position late.

The correct order is:

```csharp
floor += GetFloorChange(input[index]);

if (floor == -1)
{
    return (index + 1).ToString();
}
```

### Continuing after entering the basement

Part Two asks for the first basement position.

Once the floor becomes `-1`, processing more characters is unnecessary. Returning immediately makes the intent explicit and avoids extra work.

### Treating line breaks as parentheses

Input files often end with a line break.

`GetFloorChange` returns `0` for every character other than `(` and `)`, so a trailing line break does not affect the answer.

## Tests

Day 01 is covered by unit tests in `Aoc.Year2015.Tests`.

The tests verify known examples from the puzzle description:

- final floor calculations for Part One;
- first basement positions for Part Two;
- both positive and negative floor results.

Examples include:

```text
(())    → Part One: 0
)))     → Part One: -3
)       → Part Two: 1
()())   → Part Two: 5
```

The application layer is tested separately in `Aoc.Application.Tests`.

Those tests verify that `PuzzleExecutionService`:

- executes only Part One when Part One is selected;
- executes only Part Two when Part Two is selected;
- executes both parts when `Both` is selected;
- loads the input only once when both parts are executed;
- does not load input when the requested puzzle is not registered.

This separation is intentional:

```text
Aoc.Year2015.Tests
→ verifies the Day 01 algorithm.

Aoc.Application.Tests
→ verifies the puzzle execution flow.

Aoc.Infrastructure.Tests
→ verifies file-based input loading.
```

Running all tests locally:

```powershell
dotnet test AdventOfCodeLab.slnx
```

At this point, the solution contains `19` passing tests.

## Application flow

Day 01 is not executed directly by the CLI.

The execution flow is:

```text
Aoc.Cli
→ IPuzzleExecutionService
→ IPuzzleInputProvider
→ FilePuzzleInputProvider
→ demo or local input file

IPuzzleExecutionService
→ finds the requested IPuzzle
→ runs Part One, Part Two, or both
→ measures execution time
→ returns PuzzleRunResult

Aoc.Cli
→ displays the answers and execution times
```

For the current demo run, the CLI requests:

```text
Puzzle: 2015 / Day 01
Part: Both
Input: Demo
```

The demo input is stored in:

```text
Inputs/demo/2015/day01.txt
```

Personal Advent of Code inputs can be stored locally in:

```text
Inputs/local/2015/day01.txt
```

The `inputs/local` directory is ignored by Git, while demo inputs are committed so that anyone can clone and run the project.

## What this day demonstrates

Day 01 is a small algorithmic puzzle, but it establishes the foundation used by the rest of the repository:

- a shared `IPuzzle` contract;
- a validated `PuzzleId`;
- separate puzzle, application, infrastructure, and CLI layers;
- asynchronous input loading;
- dependency injection;
- unit tests at multiple layers;
- GitHub Actions CI validation;
- documented reasoning, not only final code.