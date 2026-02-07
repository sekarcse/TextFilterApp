using TextFilterApp.Domain.Interfaces;

namespace TextFilterApp.Application.Services;

/// <summary>
/// Applies a chain of text filters to a collection of words.
/// Filters are applied in the order they are provided.
/// </summary>
public sealed class TextFilterService(IEnumerable<ITextFilter> filters) : ITextFilterService
{
    private readonly IEnumerable<ITextFilter> _filters = filters ?? throw new ArgumentNullException(nameof(filters));

    public IEnumerable<string> ApplyFilters(IEnumerable<string> words)
    {
        // Chain filters together - each filter processes the output of the previous one
        var result = words;

        foreach (var filter in _filters)
        {
            result = filter.Apply(result);
        }

        return result;
    }
}