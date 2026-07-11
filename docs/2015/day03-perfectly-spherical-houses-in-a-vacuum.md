# Advent of Code 2015 — Day 03: Perfectly Spherical Houses in a Vacuum

> A step-by-step explanation of coordinate movement, unique house tracking, tests, and application flow used in this repository.

## Goal of this guide

This guide explains:

- how movement directions are represented as coordinates;
- why `HashSet` is used to track visited houses;
- how Part One tracks Santa;
- how Part Two alternates between Santa and Robo-Santa;
- why movement logic is extracted into a shared method;
- how the solution is tested;
- how Day 03 runs through the application architecture.

## Problem summary

Santa delivers presents to houses located on an infinite two-dimensional grid.

He starts at one house and receives movement directions:

```text
^ → north
v → south
> → east
< → west
```

After every movement, Santa delivers a present to the house at his new position.

The starting house also receives a present before any movement occurs.

The puzzle has two parts:

1. Count the unique houses visited by Santa.
2. Count the unique houses visited by Santa and Robo-Santa while they take turns following directions.

A house must only be counted once, even if it is visited multiple times.

## Coordinate system

Each house is represented by two coordinates:

```text
(X, Y)
```

Both Santa and Robo-Santa begin at:

```text
(0, 0)
```

Movement changes one coordinate at a time:

| Direction | Coordinate change |
| --- | --- |
| `^` | `(X, Y + 1)` |
| `v` | `(X, Y - 1)` |
| `>` | `(X + 1, Y)` |
| `<` | `(X - 1, Y)` |

For example:

```text
Start: (0, 0)
^     → (0, 1)
>     → (1, 1)
v     → (1, 0)
<     → (0, 0)
```

Santa returns to the starting house, but only four unique houses were visited.

## Why `HashSet` is used

The puzzle asks for the number of unique houses.

A `HashSet<T>` stores unique values and does not add the same value more than once.

The visited houses are represented by coordinate tuples:

```csharp
var visitedHouses = new HashSet<(int X, int Y)>();
```

For example:

```csharp
visitedHouses.Add((0, 0));
visitedHouses.Add((0, 1));
visitedHouses.Add((0, 0));
```

The second addition of `(0, 0)` does not create another item.

The final set contains only:

```text
(0, 0)
(0, 1)
```

Therefore:

```csharp
visitedHouses.Count
```

returns `2`.

Tuples work correctly in a `HashSet` because two tuples are considered equal when their corresponding values are equal.

```text
(0, 0) equals (0, 0)
(0, 0) does not equal (0, 1)
```

## Part One

Part One tracks a single position belonging to Santa.

The algorithm begins with:

```csharp
var currentPosition = (X: 0, Y: 0);
```

The starting house must be added before processing any directions:

```csharp
var visitedHouses = new HashSet<(int X, int Y)>
{
    currentPosition,
};
```

For every direction:

1. Calculate Santa's new position.
2. Add the new position to `visitedHouses`.
3. Continue with the next direction.
4. Return the number of unique positions.

The main loop is:

```csharp
foreach (var direction in input.Trim())
{
    currentPosition = Move(currentPosition, direction);
    visitedHouses.Add(currentPosition);
}
```

The result is:

```csharp
return visitedHouses.Count.ToString();
```

### Part One example

For this input:

```text
^>v<
```

Santa visits:

| Step | Direction | Position | New unique house? |
| ---: | --- | --- | --- |
| Start | — | `(0, 0)` | Yes |
| `1` | `^` | `(0, 1)` | Yes |
| `2` | `>` | `(1, 1)` | Yes |
| `3` | `v` | `(1, 0)` | Yes |
| `4` | `<` | `(0, 0)` | No |

The starting house is visited twice, but it is counted only once.

The Part One answer is:

```text
4
```

## Shared movement method

Both puzzle parts use the same movement rules.

The movement logic is extracted into a private `Move` method:

```csharp
private static (int X, int Y) Move(
    (int X, int Y) currentPosition,
    char direction) =>
    direction switch
    {
        '^' => (currentPosition.X, currentPosition.Y + 1),
        'v' => (currentPosition.X, currentPosition.Y - 1),
        '>' => (currentPosition.X + 1, currentPosition.Y),
        '<' => (currentPosition.X - 1, currentPosition.Y),
        _ => currentPosition,
    };
```

The method receives:

```text
current position
+ movement direction
```

and returns:

```text
new position
```

For example:

```text
Move((0, 0), '^') → (0, 1)
Move((0, 1), '>') → (1, 1)
Move((1, 1), 'v') → (1, 0)
```

Extracting this logic avoids duplicating the same `switch` in Part One and Part Two.

It also separates two responsibilities:

```text
Move
→ calculates one movement

SolvePartOne / SolvePartTwo
→ controls who moves and tracks visited houses
```

## Part Two

Part Two introduces Robo-Santa.

Santa and Robo-Santa:

- both begin at `(0, 0)`;
- share the same list of directions;
- take turns following directions;
- deliver presents to the same shared collection of houses.

The implementation stores two positions:

```csharp
var santaPosition = (X: 0, Y: 0);
var roboSantaPosition = (X: 0, Y: 0);
```

Only one starting position must be added to the `HashSet` because both characters begin in the same house:

```csharp
var visitedHouses = new HashSet<(int X, int Y)>
{
    santaPosition,
};
```

### Alternating turns

Directions use zero-based indexes:

```text
index 0 → Santa
index 1 → Robo-Santa
index 2 → Santa
index 3 → Robo-Santa
```

The remainder operator determines whose turn it is:

```csharp
index % 2 == 0
```

An even index belongs to Santa.

An odd index belongs to Robo-Santa.

The loop processes every direction:

```csharp
for (var index = 0; index < directions.Length; index++)
{
    var direction = directions[index];

    if (index % 2 == 0)
    {
        santaPosition = Move(santaPosition, direction);
        visitedHouses.Add(santaPosition);
    }
    else
    {
        roboSantaPosition = Move(roboSantaPosition, direction);
        visitedHouses.Add(roboSantaPosition);
    }
}
```

Both positions are added to the same `HashSet`.

This is important because a house visited by both Santa and Robo-Santa must still be counted only once.

### Part Two example

For this input:

```text
^>v<
```

the directions alternate as follows:

| Index | Direction | Character | New position |
| ---: | --- | --- | --- |
| Start | — | Both | `(0, 0)` |
| `0` | `^` | Santa | `(0, 1)` |
| `1` | `>` | Robo-Santa | `(1, 0)` |
| `2` | `v` | Santa | `(0, 0)` |
| `3` | `<` | Robo-Santa | `(0, 0)` |

The unique houses are:

```text
(0, 0)
(0, 1)
(1, 0)
```

The Part Two answer is:

```text
3
```

## Complexity

Let `n` be the number of movement directions.

| Part | Time complexity | Space complexity | Reason |
| --- | ---: | ---: | --- |
| Part One | `O(n)` average | `O(n)` | Every direction is processed once, and up to `n + 1` unique houses can be stored. |
| Part Two | `O(n)` average | `O(n)` | Every direction is processed once, while all unique positions are stored in one shared set. |

Adding an item to a `HashSet` has average complexity:

```text
O(1)
```

Therefore, processing all directions takes average time:

```text
O(n)
```

The worst-case number of unique houses is:

```text
starting house + one new house for every direction
```

which is:

```text
n + 1
```

That is why additional memory usage is `O(n)`.

## Common mistakes

### Forgetting the starting house

Santa delivers a present before processing the first direction.

The starting position must therefore be added immediately:

```csharp
var visitedHouses = new HashSet<(int X, int Y)>
{
    currentPosition,
};
```

Without it, every answer would potentially be one house too small.

### Counting every visit instead of every unique house

A normal counter would count repeated visits:

```text
visit (0, 0)
visit (0, 1)
visit (0, 0)
```

That produces three visits but only two unique houses.

A `HashSet` solves this by keeping each coordinate only once.

### Using separate sets in Part Two

Santa and Robo-Santa must contribute to one combined result.

Using separate sets and adding their counts would count shared houses twice.

The correct design uses one shared set:

```csharp
var visitedHouses = new HashSet<(int X, int Y)>();
```

Both characters add their positions to this collection.

### Moving both characters for every direction

Each direction belongs to only one character.

This is incorrect:

```text
Santa moves
and Robo-Santa moves
for the same direction
```

The correct behavior alternates:

```text
even index → Santa
odd index → Robo-Santa
```

### Updating the wrong position

Part Two must preserve two independent positions.

When Santa moves, only `santaPosition` changes.

When Robo-Santa moves, only `roboSantaPosition` changes.

The other character remains in their previous house.

### Using a list with repeated `Contains` checks

A `List<(int X, int Y)>` could store positions, but checking uniqueness with `Contains` requires scanning the list.

Repeated checks could result in `O(n²)` time.

A `HashSet` expresses the requirement more clearly and provides average `O(1)` insertion and lookup.

## Tests

Day 03 is covered by unit tests in `Aoc.Year2015.Tests`.

### Part One tests

The tests verify the official examples:

| Input | Expected unique houses |
| --- | ---: |
| `>` | `2` |
| `^>v<` | `4` |
| `^v^v^v^v^v` | `2` |

These examples verify:

- movement to a new house;
- returning to the starting house;
- repeated visits to the same houses;
- unique coordinate counting.

### Part Two tests

The tests verify alternating movement between Santa and Robo-Santa:

| Input | Expected unique houses |
| --- | ---: |
| `^v` | `3` |
| `^>v<` | `3` |
| `^v^v^v^v^v` | `11` |

These examples verify:

- separate Santa and Robo-Santa positions;
- alternating turns;
- shared visited-house tracking;
- overlapping and non-overlapping routes.

Day 03 contributes six puzzle tests.

Running the complete test suite locally:

```powershell
dotnet test AdventOfCodeLab.slnx
```

After adding Day 03, the solution contains `29` passing tests.

## Application flow

Day 03 follows the same application flow as the previous puzzles:

```text
Aoc.Cli
→ IPuzzleExecutionService
→ IPuzzleInputProvider
→ FilePuzzleInputProvider
→ demo or local input file

IPuzzleExecutionService
→ finds Day03 through the IPuzzle registry
→ loads the input asynchronously
→ runs Part One, Part Two, or both
→ measures execution time
→ returns PuzzleRunResult

Aoc.Cli
→ displays the answers and execution times
```

Day 03 is registered through dependency injection:

```csharp
services.AddSingleton<IPuzzle, Day03>();
```

Because the CLI receives all registered `IPuzzle` implementations, Day 03 automatically appears in the interactive puzzle selection menu.

For the demo run, the CLI requests:

```text
Puzzle: 2015 / Day 03
Part: Both
Input: Demo
```

The demo input is stored in:

```text
src/Aoc.Abstractions/Inputs/demo/2015/day03.txt
```

The current demo input is:

```text
^>v<
```

Its expected results are:

| Part | Result |
| --- | ---: |
| Part One | `4` |
| Part Two | `3` |

Personal Advent of Code input can be stored locally in:

```text
src/Aoc.Abstractions/Inputs/local/2015/day03.txt
```

The local input directory is ignored by Git, while the demo input can be committed to the repository.

## What this day demonstrates

Day 03 introduces coordinate-based movement and unique-value tracking.

It demonstrates:

- representing positions with named tuples;
- modelling movement on a two-dimensional grid;
- using `HashSet` to store unique coordinates;
- relying on tuple value equality;
- alternating work using even and odd indexes;
- maintaining independent state for Santa and Robo-Santa;
- sharing one collection between multiple actors;
- extracting common movement logic into a reusable method;
- testing repeated visits and alternating routes;
- integrating another puzzle through dependency injection and the existing CLI.