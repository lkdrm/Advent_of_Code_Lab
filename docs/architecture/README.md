# Advent of Code Lab Architecture

This directory contains version-controlled Mermaid diagrams describing the structure and behavior of Advent of Code Lab.

The diagrams are kept in the repository so architectural changes can be reviewed together with the code that they describe.

## Architecture diagrams

| Document | Description |
| --- | --- |
| [Solution architecture](solution-architecture.md) | Projects, layers, dependencies, and automated-test boundaries |
| [Complete startup and puzzle execution flow](complete-startup-and-puzzle-execution-flow.md) | Runtime sequence from application startup and dependency injection to CLI result presentation |
| [Input and result file flow](input-and-result-file-flow.md) | Demo and personal input loading, puzzle execution, Markdown result merging, and safe file replacement |
| [Puzzle extension flow](puzzle-extension-flow.md) | How a new `DayXX` implementation is tested, documented, discovered through assembly scanning, and added to the CLI |
| [GitHub CI flow](github-CI-flow.md) | Restore, build, tests, TRX reporting, pull-request summary updates, and merge validation |

## Documentation responsibilities

- The repository root [`README.md`](../../README.md) provides the project overview, quick start, features, and roadmap.
- This directory contains diagrams synchronized with the versioned source code.
- The [project Wiki](https://github.com/lkdrm/Advent_of_Code_Lab/wiki) provides extended explanations, learning notes, development workflows, and troubleshooting.
- [`docs/2015`](../2015) contains detailed reasoning and implementation guides for individual puzzles.
- [`CONTRIBUTING.md`](../../CONTRIBUTING.md) defines branch, implementation, testing, documentation, and pull-request rules.

## Maintenance rule

Update the relevant diagram whenever a change affects:

- project or layer dependencies;
- application startup or dependency injection;
- puzzle execution or cancellation;
- input or result-file behavior;
- automatic puzzle discovery;
- logging or result persistence;
- CI or pull-request automation.
