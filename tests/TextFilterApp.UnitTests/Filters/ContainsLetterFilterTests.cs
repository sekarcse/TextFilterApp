using FluentAssertions;
using TextFilterApp.Application.Filters;

namespace TextFilterApp.UnitTests.Filters;

public class ContainsLetterFilterTests
{
    [Theory]
    [InlineData("test", 't', false)]       // contains 't', filtered out
    [InlineData("hello", 't', true)]       // no 't', kept
    [InlineData("Test", 't', false)]       // contains 'T' (case-insensitive), filtered
    [InlineData("TOTAL", 't', false)]      // contains 'T' uppercase, filtered
    public void Apply_ShouldFilterWordsContainingLetter(string word, char letter, bool shouldBeKept)
    {
        // Arrange
        var sut = new ContainsLetterFilter(letter);

        // Act
        var result = sut.Apply([word]).ToList();

        // Assert
        if (shouldBeKept)
            result.Should().ContainSingle();
        else
            result.Should().BeEmpty();
    }

    [Fact]
    public void Apply_WithMultipleWords_ShouldFilterCorrectly()
    {
        // Arrange
        var sut = new ContainsLetterFilter('t');
        var input = new[] { "the", "quick", "brown", "fox", "test" };

        // Act
        var result = sut.Apply(input).ToList();

        // Assert - "quick", "brown", "fox" don't contain 't'
        result.Should().HaveCount(3)
            .And.Equal(["quick", "brown", "fox"]);
    }

    [Fact]
    public void Apply_WithEmptyCollection_ShouldReturnEmpty()
    {
        // Arrange
        var sut = new ContainsLetterFilter('x');

        // Act
        var result = sut.Apply([]).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Apply_WhenNoWordsContainLetter_ShouldKeepAll()
    {
        // Arrange
        var sut = new ContainsLetterFilter('z');
        var input = new[] { "hello", "world", "foo" };

        // Act
        var result = sut.Apply(input).ToList();

        // Assert
        result.Should().HaveCount(3);
    }
}