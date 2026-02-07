using FluentAssertions;
using TextFilterApp.Infrastructure.FileReaders;
using TextFilterApp.UnitTests.Helpers;

namespace TextFilterApp.UnitTests.Infrastructure;

public class StreamingTextFileReaderTests
{
    private readonly StreamingTextFileReader _sut = new();

    [Fact]
    public void ReadWords_WithSimpleText_ShouldReturnWords()
    {
        // Arrange
        using var testFile = new TemporaryTestFile("hello world test");

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert
        result.Should().HaveCount(3)
            .And.Equal(["hello", "world", "test"]);
    }

    [Fact]
    public void ReadWords_WithMultipleWhitespaceTypes_ShouldSplitCorrectly()
    {
        // Arrange
        using var testFile = new TemporaryTestFile("hello\tworld\ntest\r\nfoo  bar");

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert
        result.Should().HaveCount(5)
            .And.Equal(["hello", "world", "test", "foo", "bar"]);
    }

    [Fact]
    public void ReadWords_WithEmptyFile_ShouldReturnEmpty()
    {
        // Arrange
        using var testFile = new TemporaryTestFile("");

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ReadWords_WithOnlyWhitespace_ShouldReturnEmpty()
    {
        // Arrange
        using var testFile = new TemporaryTestFile("   \t\n\r\n   ");

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ReadWords_WithSingleCharacterWords_ShouldReturnThem()
    {
        // Arrange
        using var testFile = new TemporaryTestFile("a b c d");

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert
        result.Should().HaveCount(4)
            .And.Equal(["a", "b", "c", "d"]);
    }

    [Fact]
    public void ReadWords_WithWordAtMaxLength_ShouldReturnCompleteWord()
    {
        // Arrange - word exactly 1000 characters
        var longWord = new string('a', 1000);
        using var testFile = new TemporaryTestFile($"start {longWord} end");

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Should().Be("start");
        result[1].Should().HaveLength(1000);
        result[2].Should().Be("end");
    }

    [Fact]
    public void ReadWords_WithWordExceedingMaxLength_ShouldChunkIt()
    {
        // Arrange - word 2500 characters (will be chunked at 1000)
        var veryLongWord = new string('b', 2500);
        using var testFile = new TemporaryTestFile($"start {veryLongWord} end");

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert - should be: "start", chunk1(1000), chunk2(1000), chunk3(500), "end"
        result.Should().HaveCount(5);
        result[0].Should().Be("start");
        result[1].Should().HaveLength(1000);
        result[2].Should().HaveLength(1000);
        result[3].Should().HaveLength(500);
        result[4].Should().Be("end");
    }

    [Fact]
    public void ReadWords_WithFileEndingWithoutWhitespace_ShouldIncludeLastWord()
    {
        // Arrange
        using var testFile = new TemporaryTestFile("hello world");

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert
        result.Should().HaveCount(2)
            .And.Equal(["hello", "world"]);
    }

    [Fact]
    public void ReadWords_WithLeadingAndTrailingWhitespace_ShouldTrimCorrectly()
    {
        // Arrange
        using var testFile = new TemporaryTestFile("   hello world   ");

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert
        result.Should().HaveCount(2)
            .And.Equal(["hello", "world"]);
    }

    [Fact]
    public void ReadWords_WithLargeFile_ShouldStreamEfficiently()
    {
        // Arrange - create a file with many words to test streaming
        var words = Enumerable.Range(1, 10000).Select(i => $"word{i}");
        using var testFile = new TemporaryTestFile(string.Join(" ", words));

        // Act
        var result = _sut.ReadWords(testFile.FilePath).ToList();

        // Assert
        result.Should().HaveCount(10000);
        result[0].Should().Be("word1");
        result[9999].Should().Be("word10000");
    }

    [Fact]
    public void ReadWords_WithNonExistentFile_ShouldThrowFileNotFoundException()
    {
        // Arrange
        var filePath = Path.Combine(Path.GetTempPath(), "TextFilterTests", "nonexistent.txt");

        // Act
        var act = () => _sut.ReadWords(filePath).ToList();

        // Assert
        act.Should().Throw<FileNotFoundException>()
            .WithMessage("*not found*");
    }

    [Fact]
    public void ReadWords_WithNullFilePath_ShouldThrowArgumentException()
    {
        // Act
        var act = () => _sut.ReadWords(null!).ToList();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*cannot be null*");
    }

    [Fact]
    public void ReadWords_WithEmptyFilePath_ShouldThrowArgumentException()
    {
        // Act
        var act = () => _sut.ReadWords("").ToList();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*cannot be null*");
    }

    [Fact]
    public void ReadWords_WithWhitespaceFilePath_ShouldThrowArgumentException()
    {
        // Act
        var act = () => _sut.ReadWords("   ").ToList();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*cannot be null*");
    }

}