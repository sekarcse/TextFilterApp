# Text Filter Application

A text filtering application built with Clean Architecture principles. Efficiently processes text files of any size using streaming and applies configurable filters.

## Quick Start

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run --project src/TextFilterApp.Console/TextFilterApp.Console.csproj samples/input.txt
```

### Test
```bash
dotnet test
```

## Features

- **Three Text Filters:**
  - Vowel middle filter - removes words with vowels in the middle position
  - Minimum length filter - removes words shorter than specified length
  - Character exclusion filter - removes words containing specific characters

- **Performance Optimized:**
  - Streaming architecture for memory efficiency
  - Handles multi-GB files with minimal memory footprint
  - Tested with 1GB files (~25 seconds, <80MB memory)

- **Quality Assurance:**
  - Comprehensive unit tests (xUnit)
  - Integration tests
  - CI pipeline with GitHub Actions

## Architecture

Clean Architecture with clear separation of concerns:

- **Domain** - Core interfaces and business rules
- **Application** - Filter implementations and services
- **Infrastructure** - File I/O and external dependencies
- **Console** - User interface

Built with Factory and Strategy design patterns. Follows SOLID principles for maintainability and extensibility.

## Project Structure
```
src/
├── TextFilterApp.Domain/         # Core interfaces
├── TextFilterApp.Application/     # Business logic & filters
├── TextFilterApp.Infrastructure/  # File operations
└── TextFilterApp.Console/         # Entry point

tests/
├── TextFilterApp.UnitTests/       # Unit tests
└── TextFilterApp.IntegrationTests/ # Integration tests
```

## Requirements

- .NET 8.0 or higher
- Works on Windows, macOS, and Linux

---

*Detailed documentation and design decisions available upon request.*
