# Advent of Code 2015 — Day 04: The Ideal Stocking Stuffer

[Open the original puzzle](https://adventofcode.com/2015/day/4)

## Overview

Day 04 introduces hash-based brute-force searching.

The puzzle provides a secret key. A positive decimal number is appended to that key, and the resulting text is hashed with MD5.

The goal is to find the lowest positive number that produces an MD5 hash with the required number of leading hexadecimal zeroes.

Example candidate:

```text
abcdef609043
```

Its MD5 hash starts with:

```text
000001dbbfa...
```

Therefore, it satisfies the Part One requirement.

## Part One

Find the lowest positive number whose generated MD5 hash starts with five hexadecimal zeroes.

Official examples:

| Secret key | Lowest number |
| --- | ---: |
| `abcdef` | `609043` |
| `pqrstuv` | `1048970` |

## Part Two

Part Two increases the required prefix from five to six hexadecimal zeroes.

For the demo key used by this project:

| Secret key | Lowest number |
| --- | ---: |
| `abcdef` | `6742839` |

The algorithm remains unchanged. Only the required number of leading zeroes differs.

## Why brute force is required

MD5 is designed so that small input changes produce unpredictable output changes.

There is no useful mathematical relationship between:

```text
abcdef1
abcdef2
abcdef3
```

and their resulting hashes.

Therefore, candidates must be tested sequentially:

```text
1
2
3
...
```

The first matching candidate is guaranteed to be the lowest because the search begins at `1` and proceeds in increasing order.

## MD5 representation

An MD5 hash contains 128 bits:

```text
128 bits / 8 = 16 bytes
```

Each byte is represented by two hexadecimal characters:

```text
1 byte = 8 bits = 2 hexadecimal characters
```

For example:

```text
Binary byte:       00000000
Hexadecimal value: 00
```

## Five leading hexadecimal zeroes

Five hexadecimal zeroes represent twenty zero bits:

```text
5 × 4 bits = 20 bits
```

The required binary prefix is:

```text
00000000 00000000 0000....
```

Therefore:

```csharp
hash[0] == 0
hash[1] == 0
(hash[2] & 0xF0) == 0
```

The first two bytes must be completely zero.

Only the upper four bits of the third byte must be zero.

The mask:

```text
0xF0 = 11110000
```

selects the upper four bits.

## Six leading hexadecimal zeroes

Six hexadecimal zeroes represent twenty-four zero bits:

```text
6 × 4 bits = 24 bits
```

This means that the first three bytes must be completely zero:

```csharp
hash[0] == 0
hash[1] == 0
hash[2] == 0
```

The shared implementation supports both cases by calculating:

```csharp
var completeZeroBytes = requiredLeadingHexZeroes / 2;
var requiresHalfByte = requiredLeadingHexZeroes % 2 != 0;
```

## Algorithm

The algorithm performs the following steps:

1. Validate and trim the secret key.
2. Convert the secret key to ASCII bytes once.
3. Create one reusable candidate buffer.
4. Start checking numbers from `1`.
5. Write the current number directly into the candidate buffer.
6. Calculate the MD5 hash into a reusable 16-byte buffer.
7. Check the required leading bits.
8. Return the first matching number.

Simplified pseudocode:

```text
secretKeyBytes = encode(secretKey)

for number from 1:
    candidate = secretKeyBytes + decimalBytes(number)
    hash = MD5(candidate)

    if hash has required zero prefix:
        return number
```

## Avoiding allocations inside the loop

A straightforward implementation could use:

```csharp
var candidate = $"{secretKey}{number}";
var bytes = Encoding.ASCII.GetBytes(candidate);
var hash = MD5.HashData(bytes);
var hexadecimalHash = Convert.ToHexString(hash);
```

This is readable, but every iteration creates several temporary objects:

- a candidate string;
- a candidate byte array;
- a hash byte array;
- a hexadecimal string.

Day 04 may perform millions of iterations, causing substantial garbage-collection pressure.

The optimized implementation reuses memory instead.

## Reusable candidate buffer

The secret key is encoded once:

```csharp
var secretKeyBytes = Encoding.ASCII.GetBytes(secretKey);
```

One candidate buffer is then created:

```csharp
var candidateBytes =
    new byte[secretKeyBytes.Length + 10];
```

Ten additional bytes are sufficient because the largest positive `Int32` contains ten decimal digits.

The secret key occupies the beginning of the buffer:

```text
[a][b][c][d][e][f][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
```

Only the number portion changes between iterations:

```text
[a][b][c][d][e][f][1]
[a][b][c][d][e][f][2]
[a][b][c][d][e][f][3]
```

## Utf8Formatter

`Utf8Formatter.TryFormat` writes the number directly into the existing byte buffer:

```csharp
Utf8Formatter.TryFormat(
    number,
    numberDestination,
    out var numberByteCount);
```

It avoids creating:

```csharp
number.ToString()
```

and avoids encoding that temporary string into another byte array.

Decimal digits use the same byte values in ASCII and UTF-8, so the formatted bytes can be hashed directly.

## Span

`Span<T>` represents a safe view over a contiguous region of memory.

It does not own or copy the underlying data.

For example:

```csharp
var numberDestination = candidateBytes.AsSpan(
    secretKeyBytes.Length);
```

This span points to the part of the candidate buffer located immediately after the secret key.

The source passed to MD5 is also a span:

```csharp
candidateBytes.AsSpan(0, candidateLength)
```

Only the meaningful bytes are hashed. Unused bytes at the end of the buffer are ignored.

## stackalloc

MD5 always produces exactly sixteen bytes, so a small fixed-size buffer can be allocated on the stack:

```csharp
Span<byte> hash = stackalloc byte[16];
```

This buffer is created once before the loop and reused for every candidate.

The hash from the previous iteration is no longer needed, so it can be safely overwritten.

Stack memory:

- has a short method-level lifetime;
- does not require garbage collection;
- is suitable for small, fixed-size buffers.

Large or user-controlled values should not generally be allocated with `stackalloc`, because excessive stack usage can cause a stack overflow.

For that reason, the variable-size candidate buffer remains a managed array, while only the fixed sixteen-byte hash uses `stackalloc`.

## Shared puzzle-part implementation

Part One and Part Two share input validation and normalization:

```csharp
private static string Solve(
    string input,
    int requiredLeadingHexZeroes)
```

The public methods only select the required difficulty:

```csharp
Solve(input, requiredLeadingHexZeroes: 5);
Solve(input, requiredLeadingHexZeroes: 6);
```

This keeps the rules for trimming, validation, searching, and formatting in one place.

## Correctness

The algorithm is correct because:

1. it starts with the smallest allowed positive number, `1`;
2. candidates are checked in strictly increasing order;
3. every candidate is hashed using the exact `secretKey + number` representation;
4. the binary prefix check is equivalent to the required hexadecimal prefix;
5. the algorithm returns immediately when the first valid candidate is found.

Therefore, the returned number is the lowest number satisfying the puzzle requirement.

## Complexity

Let:

- `k` be the first matching number;
- `n` be the length of the secret key and decimal candidate.

The algorithm hashes every candidate from `1` through `k`.

Time complexity:

```text
O(k × n)
```

Because the secret key and candidate number are short, this behaves approximately as:

```text
O(k)
```

Additional memory usage:

```text
O(n)
```

The implementation reuses the candidate and hash buffers instead of allocating new buffers for every iteration.

## Probability

Each hexadecimal character has sixteen possible values.

The approximate probability of five leading zeroes is:

```text
1 / 16⁵ = 1 / 1,048,576
```

For six leading zeroes:

```text
1 / 16⁶ = 1 / 16,777,216
```

Adding one more required hexadecimal zero makes a random match approximately sixteen times less likely.

This does not mean every individual Part Two input will take exactly sixteen times longer, because hash results are effectively unpredictable.

## Validation

Both puzzle parts reject:

- `null`;
- an empty string;
- whitespace-only input.

Trailing file whitespace is removed before hashing:

```csharp
var secretKey = input.Trim();
```

The input is treated as a secret key rather than a numeric value. Therefore, numeric validation such as rejecting negative numbers is not applicable.

## Automated tests

The Day 04 test suite verifies:

- both official Part One examples;
- a known Part Two result;
- null-input validation for both parts;
- empty and whitespace-only validation for both parts.

Test names follow the project convention and do not contain underscores.

## Application integration

`Day04` implements `IPuzzle`, so it is discovered automatically through assembly scanning.

No manual dependency-injection registration is required.

The demo input is stored at:

```text
src/Aoc.Abstractions/Inputs/demo/2015/day04.txt
```

When the puzzle is executed through the CLI, its answers are automatically written to:

```text
Results/demo/2015/day04.md
```

The result writer preserves independently executed puzzle parts and avoids rewriting unchanged Markdown files.

## Security note

MD5 is used here only because the puzzle explicitly requires it.

MD5 is not considered suitable for password storage, digital signatures, or modern cryptographic security. Production security code should use algorithms designed for its specific security requirements.

## Learning outcomes

Day 04 demonstrates:

- brute-force search;
- MD5 hashing;
- binary and hexadecimal representation;
- bit masks;
- byte-level prefix validation;
- reusable buffers;
- `Span<T>` and `ReadOnlySpan<T>`;
- `stackalloc`;
- `Utf8Formatter`;
- allocation reduction in hot loops;
- shared logic between puzzle parts;
- complexity and probability analysis.