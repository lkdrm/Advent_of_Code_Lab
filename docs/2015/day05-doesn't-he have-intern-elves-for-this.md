# Advent of Code 2015 вЂ” Day 05: Doesn't He Have Intern-Elves For This?

[Open the original puzzle](https://adventofcode.com/2015/day/5)

## Overview

Day 05 is a string-classification problem.

The input contains one candidate string per line. Each candidate is evaluated
independently against a set of rules, and the final answer is the number of
strings classified as nice.

The two puzzle parts use different rule sets:

- Part One combines vowel counting, adjacent duplicate detection, and forbidden
  pair rejection;
- Part Two searches for a repeated non-overlapping pair and a character that
  repeats with exactly one character between its occurrences.

The implementation separates every rule into a small predicate method. The
public solving methods compose those predicates and count candidates for which
all required conditions are true.

## Reduced problem

The story can be reduced to the following pipeline:

```text
multiline input
    -> candidate strings
    -> evaluate every candidate
    -> count candidates satisfying every rule
    -> return the count as text
```

No state needs to be shared between different input lines.

## Input parsing

Both puzzle parts consume the same input format, so candidate parsing is shared.

The parser separates the input using both carriage-return and line-feed
characters:

```csharp
input.Split(
    ['\r', '\n'],
    StringSplitOptions.RemoveEmptyEntries |
    StringSplitOptions.TrimEntries)
```

This accepts the common line-ending forms:

```text
Windows: \r\n
Linux:   \n
```

`RemoveEmptyEntries` ignores empty items produced by consecutive separators and
an optional final line ending. `TrimEntries` removes surrounding whitespace from
each candidate.

Both public methods reject null, empty, and whitespace-only puzzle input before
parsing.

## Predicate composition

Each rule is represented as a method returning `bool`.

Part One conceptually evaluates:

```text
has at least three vowels
AND has a consecutive duplicate
AND does not contain a forbidden pair
```

Part Two evaluates:

```text
has a repeated pair without overlap
AND has a repeating letter with one character between
```

This decomposition keeps each method responsible for one question. A method
named `HasAtLeastThreeVowels`, for example, does not call the other Part One
rules. The solving method owns their composition.

## Part One

A candidate is nice under the original rules only when all three conditions are
satisfied.

### Rule 1: at least three vowels

The accepted vowels are:

```text
a e i o u
```

The algorithm scans the candidate and increments a counter whenever the current
character is a vowel.

Pattern matching expresses the set directly:

```csharp
character is 'a' or 'e' or 'i' or 'o' or 'u'
```

The method returns immediately when the third vowel is found. Any remaining
characters cannot change the result.

Time complexity:

```text
O(m)
```

Extra space:

```text
O(1)
```

where `m` is the candidate length.

### Rule 2: consecutive duplicate characters

The candidate must contain two identical adjacent characters.

For every valid starting index `i`, compare:

```csharp
candidate[i] == candidate[i + 1]
```

Because the code accesses `i + 1`, the loop stops before the last character:

```text
i < candidate.Length - 1
```

The first match proves the rule, so the method returns immediately.

Examples:

| Candidate fragment | Result |
| --- | --- |
| `aa` | Match |
| `xx` | Match |
| `ab` | No match |

Time complexity is `O(m)` and extra space is `O(1)`.

### Rule 3: forbidden pairs

The forbidden adjacent pairs are:

```text
ab
cd
pq
xy
```

The implementation scans every adjacent pair in its original order. If a
forbidden pair is found, the candidate is rejected immediately.

The order is important:

```text
candidate[i], candidate[i + 1]
```

Reversing those positions would turn `ab` into `ba` and produce an incorrect
classification.

Time complexity is `O(m)` and extra space is `O(1)`. The interpolated
two-character values are short-lived implementation details; no collection of
pairs is retained.

### Part One examples

| Candidate | Classification | Reason |
| --- | --- | --- |
| `ugknbfddgicrmopn` | Nice | Satisfies all three rules |
| `aaa` | Nice | Three vowels and `aa`, with no forbidden pair |
| `jchzalrnumimnmhp` | Naughty | No consecutive duplicate |
| `haegwjzuvuyypxyu` | Naughty | Contains `xy` |
| `dvszwmarrgswjxmb` | Naughty | Contains fewer than three vowels |

## Part Two

Part Two replaces all Part One rules. Vowel counts, adjacent duplicates, and
forbidden pairs are no longer considered.

A candidate is nice only when both revised rules are satisfied.

### Rule 1: repeated pair without overlap

At least one two-character pair must occur twice without sharing a character.

For a pair beginning at index `i`, a second valid occurrence must begin at
`i + 2` or later.

The implementation creates the current two-character pair and uses ordinal
`IndexOf` starting at `i + 2`:

```text
current pair = candidate[i..i + 2]
search from i + 2
```

Starting at `i + 2` encodes the non-overlap requirement directly.

#### Why `aaa` does not satisfy the rule

```text
a a a
0 1 2
```

The pair `aa` appears at indexes `0` and `1`, but both occurrences use the
character at index `1`. They overlap.

#### Why `aaaa` satisfies the rule

```text
a a a a
0 1 2 3
```

The pair `aa` appears at indexes `0` and `2`. These occurrences use separate
characters and therefore do not overlap.

#### Ordinal comparison

`StringComparison.Ordinal` compares the underlying character values without
culture-specific language rules. Puzzle tokens are exact character sequences,
so ordinal comparison expresses the intended semantics.

#### Complexity

`IndexOf` may scan the remaining suffix for every starting position.

Worst-case time per candidate:

```text
O(mВІ)
```

Peak additional space is `O(1)`, although the current `Substring` call creates
short temporary strings during the scan.

The official candidates are short, so this direct implementation favors
clarity and measured behavior over a more complicated lookup structure.

### Evaluated Dictionary alternative

An alternative implementation can store the first position of each pair:

```text
(first character, second character) -> first index
```

That approach has average `O(m)` lookup time, but it also creates a dictionary,
allocates internal storage, calculates hashes, and performs bucket lookups for
every candidate.

Local measurements on the short Day 05 strings did not show a runtime benefit.
The simpler ordinal `IndexOf` implementation was therefore retained.

This illustrates an important performance principle:

> Better asymptotic complexity does not guarantee lower runtime for small,
> bounded inputs. Measure before keeping an optimization.

### Rule 2: repeating letter with one between

The second rule looks for this shape:

```text
x ? x
```

For every valid index `i`, compare:

```csharp
candidate[i] == candidate[i + 2]
```

Because the code accesses `i + 2`, the loop condition is:

```text
i < candidate.Length - 2
```

Examples:

| Fragment | Result |
| --- | --- |
| `xyx` | Match |
| `aaa` | Match |
| `abcdefeghi` | Match through `efe` |
| `abc` | No match |

Time complexity is `O(m)` and extra space is `O(1)`.

### Part Two examples

| Candidate | Classification | Reason |
| --- | --- | --- |
| `qjhvhtzxzqqjkmpb` | Nice | Satisfies both revised rules |
| `xxyxx` | Nice | Repeated `xx` and repeating `x` with one character between |
| `uurcxstgmygtbstg` | Naughty | Has a repeated pair but not the second rule |
| `ieodomkazucvgmuy` | Naughty | Has the second rule but no repeated non-overlapping pair |

## Counting candidates

Each puzzle part follows the same outer structure:

```text
count = 0

for each candidate:
    if every rule for this Part is satisfied:
        count++

return count as text
```

For Part One, every candidate is scanned by a constant number of linear
predicates. If `N` is the total number of characters across all candidates,
the time complexity is:

```text
O(N)
```

For Part Two, let `mбµў` be the length of candidate `i`. The worst-case time is:

```text
O(sum(mбµўВІ))
```

The parsed candidate array and strings require space proportional to the input
size. Individual rule checks retain only constant local state.

## Correctness

Part One is correct because:

1. every non-empty parsed candidate is evaluated exactly once;
2. the three predicates correspond to the three required rules;
3. short-circuit `AND` accepts a candidate only when every predicate is true;
4. the counter is incremented exactly once for each accepted candidate.

Part Two is correct because:

1. searching from `i + 2` prevents the repeated pair from overlapping its
   original occurrence;
2. comparing indexes `i` and `i + 2` exactly represents one character between
   matching characters;
3. both predicates must be true before the candidate is counted.

Therefore, each returned value equals the number of candidates satisfying the
complete rule set for the selected puzzle part.

## Automated tests

The Day 05 test suite covers:

- strings satisfying every Part One rule;
- the vowel `u` as part of the accepted vowel set;
- candidates without consecutive duplicate characters;
- forbidden-pair rejection;
- all official Part Two examples;
- overlapping pair behavior through `aaa` and `aaaa`;
- counting multiple candidates in one puzzle input.

Test names follow the project convention and do not contain underscores.

## Application integration

`Day05` implements `IPuzzle`, so assembly scanning discovers and registers it
automatically. No manual dependency-injection registration is required.

The safe demo input belongs at:

```text
src/Aoc.Abstractions/Inputs/demo/2015/day05.txt
```

After CLI execution, the Markdown result writer stores answers at:

```text
Results/demo/2015/day05.md
Results/personal/2015/day05.md
```

Demo and personal results remain separate, and generated reports never contain
the original puzzle input.

## Common mistakes

- Forgetting that `u` is one of the five accepted vowels.
- Reversing `candidate[i]` and `candidate[i + 1]` when constructing a pair.
- Accessing `i + 1` or `i + 2` without adjusting the loop boundary.
- Counting pair occurrences without checking whether they overlap.
- Treating `aaa` as two independent `aa` pairs.
- Applying Part One rules to Part Two.
- Returning after the first non-forbidden pair instead of checking the complete
  candidate.
- Assuming an `O(m)` dictionary solution must be faster for short strings
  without measuring it.

## Learning outcomes

Day 05 demonstrates:

- parsing multiline text across common line endings;
- decomposing classification rules into independent predicates;
- composing Boolean conditions with short-circuit evaluation;
- adjacent-character and fixed-distance index comparisons;
- preventing overlap through a search start index;
- ordinal string comparison;
- early return after a rule is proven;
- distinguishing asymptotic complexity from measured runtime;
- choosing a clear algorithm for small bounded inputs;
- counting independently classified records.