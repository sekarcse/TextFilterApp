using TextFilterApp.Domain.Interfaces;

namespace TextFilterApp.Application.Filters;

/// <summary>
/// Filters out words that contain a specific letter (case-insensitive).
/// </summary>
internal sealed class ContainsLetterFilter(char letter) : ITextFilter
{
    private readonly char _letter = char.ToLower(letter);

    public IEnumerable<string> Apply(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            if (!word.Contains(_letter, StringComparison.OrdinalIgnoreCase))
            {
                yield return word;
            }
        }
    }
}