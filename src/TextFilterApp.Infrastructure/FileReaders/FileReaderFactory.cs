namespace TextFilterApp.Infrastructure.FileReaders;

/// <summary>
/// Public factory for creating internal file reader instances.
/// Keeps file reader implementations internal while providing controlled access through interfaces.
/// </summary>
public static class FileReaderFactory
{
    public static IFileReader CreateStreamingTextFileReader() => new StreamingTextFileReader();
}
