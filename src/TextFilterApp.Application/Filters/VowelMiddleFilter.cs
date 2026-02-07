using TextFilterApp.Domain.Interfaces;

/// <summary>
/// Filters out words that have a vowel in the middle position(s).
/// For odd-length words, the middle character is checked (e.g. "clean" -> 'e').
/// For even-length words, both middle characters are checked (e.g. "what" -> 'h' and 'a').
/// </summary>
internal sealed class VowelMiddleFilter : ITextFilter
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
       if (string.IsNullOrEmpty(word)) return false;

        int middle = word.Length / 2;

        if (word.Length % 2 == 1)
        {
            // Odd length: single middle character (e.g. "clean" length 5, middle index 2 => 'e')
            return Vowels.Contains(char.ToLower(word[middle]));
        }
        else
        {
            // Even length: two middle characters (e.g. "what" length 4, indices 1-2 => 'ha')
            return Vowels.Contains(char.ToLower(word[middle - 1])) ||
                   Vowels.Contains(char.ToLower(word[middle]));
        }
    }
}