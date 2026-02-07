namespace TextFilterApp.UnitTests.Helpers;

/// <summary>
/// Test helper for creating and managing temporary test files.
/// Implements IDisposable to ensure cleanup even if tests fail.
/// </summary>
internal sealed class TemporaryTestFile : IDisposable
{
    private readonly string _filePath;

    public string FilePath => _filePath;

    public TemporaryTestFile(string content)
    {
        var testDirectory = Path.Combine(Path.GetTempPath(), "TextFilterTests");
        Directory.CreateDirectory(testDirectory);
        
        _filePath = Path.Combine(testDirectory, $"test_{Guid.NewGuid()}.txt");
        File.WriteAllText(_filePath, content);
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
        {
            try
            {
                File.Delete(_filePath);
            }
            catch
            {
                // Ignore cleanup errors in tests
            }
        }
    }
}
