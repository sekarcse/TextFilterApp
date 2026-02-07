namespace TextFilterApp.Domain.Interfaces;

public interface ITextFilter
{
    IEnumerable<string> Apply(IEnumerable<string> words);
}