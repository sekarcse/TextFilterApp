using FluentAssertions;
using TextFilterApp.Application.Filters;

namespace TextFilterApp.UnitTests.Filters;

public class MinimumLengthFilterTests
{
    [Theory]
    [InlineData("hi", 3, false)]       // length 2 < 3, filtered out
    [InlineData("the", 3, true)]       // length 3 >= 3, kept
    [InlineData("hello", 3, true)]     // length 5 >= 3, kept
    [InlineData("a", 3, false)]        // length 1 < 3, filtered out
    [InlineData("go", 2, true)]        // length 2 >= 2, kept
    public void Apply_ShouldFilterByMinimumLength(string word, int minLength, bool shouldBeKept)
    {
        // Arrange
        var sut = new MinimumLengthFilter(minLength);

        // Act
        var result = sut.Apply([word]).ToList();

        // Assert
        if (shouldBeKept)
            result.Should().ContainSingle();
        else
            result.Should().BeEmpty();
    }

    [Fact]
    public void Apply_WithMultipleWords_ShouldFilterShortOnes()
    {
        // Arrange
        var sut = new MinimumLengthFilter(3);
        var input = new[] { "I", "am", "the", "king", "of", "code" };

        // Act
        var result = sut.Apply(input).ToList();

        // Assert
        result.Should().HaveCount(3)
            .And.Equal(["the", "king", "code"]);
    }

    [Fact]
    public void Apply_WithEmptyCollection_ShouldReturnEmpty()
    {
        // Arrange
        var sut = new MinimumLengthFilter(3);

        // Act
        var result = sut.Apply([]).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Apply_WithMinLengthZero_ShouldKeepAll()
    {
        // Arrange
        var sut = new MinimumLengthFilter(0);
        var input = new[] { "", "a", "ab", "abc" };

        // Act
        var result = sut.Apply(input).ToList();

        // Assert
        result.Should().HaveCount(4);
    }

    [Fact]
    public void Constructor_WithNegativeLength_ShouldThrow()
    {
        // Act & Assert
        var act = () => new MinimumLengthFilter(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}