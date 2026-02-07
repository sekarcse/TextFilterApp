namespace TextFilterApp.Application.Services;

public interface ITextFilterService
{
    IEnumerable<string> ApplyFilters(IEnumerable<string> words);
}