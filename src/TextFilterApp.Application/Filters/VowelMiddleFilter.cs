using TextFilterApp.Domain.Interfaces;

public sealed class VowelMiddleFilter : ITextFilter
{
    private static readonly HashSet<char> Vowels = ['a', 'e', 'i', 'o', 'u'];

    public IEnumerable<string> Apply(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            if (!HasVowelInMiddle(word))
                yield return word;
        }
    }

    private static bool HasVowelInMiddle(string word)
    {
        if (word.Length < 1) return false;

        int middle = word.Length / 2;
        // TODO: Handle even-length words (2 middle chars)
        return Vowels.Contains(char.ToLower(word[middle]));
    }
}