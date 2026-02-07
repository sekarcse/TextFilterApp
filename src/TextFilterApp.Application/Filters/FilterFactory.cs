using TextFilterApp.Domain.Interfaces;

namespace TextFilterApp.Application.Filters;

/// <summary>
/// Public factory for creating internal filter instances.
/// Keeps filter implementations internal while providing controlled access through interfaces.
/// </summary>
public static class FilterFactory
{
    public static ITextFilter CreateVowelMiddleFilter() => new VowelMiddleFilter();

    public static ITextFilter CreateMinimumLengthFilter(int minimumLength) => new MinimumLengthFilter(minimumLength);

    public static ITextFilter CreateContainsLetterFilter(char letter) => new ContainsLetterFilter(letter);
}