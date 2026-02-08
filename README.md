# Text Filter Application

A text filtering application demonstrating Clean Architecture principles. Processes text files efficiently using streaming, regardless of file size.

[![Build and Test](https://github.com/sekarcse/TextFilterApp/actions/workflows/dotnet.yml/badge.svg)](https://github.com/sekarcse/TextFilterApp/actions/workflows/dotnet.yml)

## Quick Start
```bash
# Build the project
dotnet build

# Run with sample file
dotnet run --project src/TextFilterApp.Console/TextFilterApp.Console.csproj samples/input.txt

# Run tests
dotnet test
```

## What It Does

Applies three configurable text filters:

1. **Vowel Middle Filter** - Filters words with vowels in the middle
   - Odd-length words: checks center character
   - Even-length words: checks both middle characters
   - Example: "clean" (middle 'e') → removed, "the" (middle 'h') → kept

2. **Minimum Length Filter** - Filters words below a certain length (default: 3 characters)

3. **Character Filter** - Filters words containing a specific character (case-insensitive)

## Performance

Built for efficiency with large files:
- **Memory:** O(1) space complexity - uses streaming, not loading entire file
- **Tested:** 1GB file processed in ~25 seconds using <90MB RAM
- **Scalable:** File size doesn't affect memory usage

## Architecture

Clean Architecture with clear layer separation:
```
Console App (UI)
    ↓
Application (Filters & Services)
    ↓
Infrastructure (File I/O)
    ↓
Domain (Interfaces)
```

**Design patterns used:**
- Factory pattern for filter creation
- Strategy pattern for interchangeable filters  
- Streaming with `yield return` for memory efficiency

**SOLID principles applied:**
- Each filter has a single responsibility
- Easy to add new filters without changing existing code
- All filters are interchangeable through `ITextFilter` interface
- Small, focused interfaces
- Components depend on abstractions

## Project Structure
```
src/
├── TextFilterApp.Domain/           # Core interfaces
├── TextFilterApp.Application/      # Filters and business logic
├── TextFilterApp.Infrastructure/   # File reading
└── TextFilterApp.Console/          # Entry point

tests/
├── TextFilterApp.UnitTests/        # Unit tests
└── TextFilterApp.IntegrationTests/ # End-to-end tests
```

## Adding New Filters

The design makes it simple to extend:

1. Create a class implementing `ITextFilter`
2. Add a factory method in `FilterFactory`
3. Use it in your pipeline

Example: To add a palindrome filter, just implement `PalindromeFilter : ITextFilter` and add `FilterFactory.CreatePalindromeFilter()`. No changes to existing code needed.

## Design Notes

**Why Factory Pattern?**  
Keeps filter implementations internal and provides a clean API. Makes it easy to add configuration or validation later without breaking consumers.

**Why Streaming?**  
Traditional `File.ReadAllText()` loads everything into memory. For a 1GB file, that's over 1GB of RAM. Streaming processes one word at a time, keeping memory constant regardless of file size.

## Testing

Uses xUnit for testing:
- Unit tests for individual filters
- Integration tests for the full pipeline
- Edge case coverage (special characters, empty files, etc.)
- CI pipeline runs tests on every push
```bash
dotnet test                                      # Run all tests
dotnet test tests/TextFilterApp.UnitTests/      # Unit tests only
```

## Requirements

- .NET 9.0 or higher
- Cross-platform (Windows, macOS, Linux)

---

*Built to demonstrate clean architecture, SOLID principles, and production-ready code practices.*
