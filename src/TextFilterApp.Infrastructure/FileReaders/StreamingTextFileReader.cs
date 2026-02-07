using System.Text;

namespace TextFilterApp.Infrastructure.FileReaders;

/// <summary>
/// Reads words from a text file using streaming for memory efficiency.
/// Suitable for files of any size — only holds one word in memory at a time.
/// </summary>
internal sealed class StreamingTextFileReader : IFileReader
{
    private const int BufferSize = 8192;
    private const int MaxWordLength = 1_000;

    public IEnumerable<string> ReadWords(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file was not found: {filePath}", filePath);

        return ReadWordsInternal(filePath);
    }

    private static IEnumerable<string> ReadWordsInternal(string filePath)
    {
        using var stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            BufferSize,
            FileOptions.SequentialScan);

        using var reader = new StreamReader(stream);

        var wordBuilder = new StringBuilder();
        var buffer = new char[BufferSize];
        int charsRead;

        while ((charsRead = reader.Read(buffer, 0, buffer.Length)) > 0)
        {
            for (int i = 0; i < charsRead; i++)
            {
                char c = buffer[i];

                if (char.IsWhiteSpace(c))
                {
                    if (wordBuilder.Length > 0)
                    {
                        yield return wordBuilder.ToString();
                        wordBuilder.Clear();
                    }
                }
                else
                {
                    wordBuilder.Append(c);

                    // Guard against excessively long string
                    if (wordBuilder.Length >= MaxWordLength)
                    {
                        yield return wordBuilder.ToString();
                        wordBuilder.Clear();
                    }
                }
            }
        }

        // Don't forget the last word if file doesn't end with whitespace
        if (wordBuilder.Length > 0)
        {
            yield return wordBuilder.ToString();
        }
    }
}
