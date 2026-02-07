using FluentAssertions;
using TextFilterApp.Application.Filters;

namespace TextFilterApp.UnitTests.Filters;

public class VowelMiddleFilterTests
{
    private readonly VowelMiddleFilter _sut = new();

    [Theory]
    [InlineData("clean", true)]      // length 5, middle index 2 = 'e' (vowel) -> filtered
    [InlineData("currently", true)]  // length 9, middle index 4 = 'e' (vowel) -> filtered
    [InlineData("what", true)]       // length 4, middle indices 1-2 = 'ha' -> 'a' is vowel -> filtered
    [InlineData("the", false)]       // length 3, middle index 1 = 'h' (not vowel) -> kept
    [InlineData("rather", false)]    // length 6, middle indices 2-3 = 'th' (no vowels) -> kept
    public void Apply_ShouldCorrectlyFilterByMiddleVowel(string word, bool shouldBeFiltered)
    {
        // Arrange
        var input = new[] { word };

        // Act
        var result = _sut.Apply(input).ToList();

        // Assert
        if (shouldBeFiltered)
            result.Should().BeEmpty();
        else
            result.Should().ContainSingle();
    }

    [Fact]
    public void Apply_WithEmptyCollection_ShouldReturnEmpty()
    {
        var result = _sut.Apply([]).ToList();

        result.Should().BeEmpty();
    }

    [Fact]
    public void Apply_WithMultipleWords_ShouldFilterCorrectly()
    {
        // Arrange
        var input = new[] { "clean", "the", "what", "rather", "currently" };

        // Act
        var result = _sut.Apply(input).ToList();

        // Assert - "the" and "rather" should remain
        result.Should().HaveCount(2)
            .And.Contain("the")
            .And.Contain("rather");
    }

    [Fact]
    public void Apply_SingleCharacterWord_ShouldFilterIfVowel()
    {
        var result = _sut.Apply(["a", "b"]).ToList();

        result.Should().ContainSingle()
            .Which.Should().Be("b");
    }

    [Theory]
    [InlineData("CLEAN", true)]    // uppercase vowel in middle
    [InlineData("THE", false)]     // uppercase, no vowel in middle
    public void Apply_ShouldBeCaseInsensitive(string word, bool shouldBeFiltered)
    {
        var result = _sut.Apply([word]).ToList();

        if (shouldBeFiltered)
            Assert.Empty(result);
        else
            Assert.Single(result);
    }
}