using FluentAssertions;
using TextFilterApp.Infrastructure.FileReaders;
using TextFilterApp.UnitTests.Helpers;

namespace TextFilterApp.UnitTests.Infrastructure;

public class FileReaderFactoryTests
{
    [Fact]
    public void CreateStreamingTextFileReader_WhenCalled_ShouldReturnInstance()
    {
        // Act
        var result = FileReaderFactory.CreateStreamingTextFileReader();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void CreateStreamingTextFileReader_WhenCalled_ShouldReturnIFileReaderImplementation()
    {
        // Act
        var result = FileReaderFactory.CreateStreamingTextFileReader();

        // Assert
        result.Should().BeAssignableTo<IFileReader>();
    }

    [Fact]
    public void CreateStreamingTextFileReader_WhenUsedToReadFile_ShouldReturnWords()
    {
        // Arrange
        using var testFile = new TemporaryTestFile("hello world");

        // Act
        var reader = FileReaderFactory.CreateStreamingTextFileReader();
        var words = reader.ReadWords(testFile.FilePath).ToList();

        // Assert
        words.Should().HaveCount(2)
            .And.Equal(["hello", "world"]);
    }
}
