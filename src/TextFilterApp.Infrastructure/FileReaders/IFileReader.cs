namespace TextFilterApp.Infrastructure.FileReaders;

/// <summary>
/// Defines a contract for reading words from a file.
/// </summary>
public interface IFileReader
{
    /// <summary>
    /// Reads all words from the specified file.
    /// </summary>
    /// <param name="filePath">Path to the text file.</param>
    /// <returns>A collection of words extracted from the file.</returns>
    IEnumerable<string> ReadWords(string filePath);
}