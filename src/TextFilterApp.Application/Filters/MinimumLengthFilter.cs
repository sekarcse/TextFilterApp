using TextFilterApp.Domain.Interfaces;

namespace TextFilterApp.Application.Filters;

/// <summary>
/// Filters out words that have a length less than the specified minimum.
/// </summary>
internal sealed class MinimumLengthFilter(int minimumLength) : ITextFilter
{
    private readonly int _minimumLength = minimumLength >= 0
        ? minimumLength
        : throw new ArgumentOutOfRangeException(nameof(minimumLength), "Minimum length cannot be negative.");

    public IEnumerable<string> Apply(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            if (word.Length >= _minimumLength)
            {
                yield return word;
            }
        }
    }
}