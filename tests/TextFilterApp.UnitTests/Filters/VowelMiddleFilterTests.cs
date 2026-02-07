using TextFilterApp.Application.Filters;

namespace TextFilterApp.UnitTests.Filters;

public class VowelMiddleFilterTests
{
    private readonly VowelMiddleFilter _filter = new();

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
        var result = _filter.Apply(input).ToList();

        // Assert
        if (shouldBeFiltered)
            Assert.Empty(result);
        else
            Assert.Single(result);
    }

    [Fact]
    public void Apply_WithEmptyCollection_ShouldReturnEmpty()
    {
        var result = _filter.Apply([]).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void Apply_WithMultipleWords_ShouldFilterCorrectly()
    {
        // Arrange
        var input = new[] { "clean", "the", "what", "rather", "currently" };

        // Act
        var result = _filter.Apply(input).ToList();

        // Assert - "the" and "rather" should remain
        Assert.Equal(2, result.Count);
        Assert.Contains("the", result);
        Assert.Contains("rather", result);
    }

    [Fact]
    public void Apply_SingleCharacterWord_ShouldFilterIfVowel()
    {
        var result = _filter.Apply(["a", "b"]).ToList();

        Assert.Single(result);
        Assert.Equal("b", result[0]);
    }

    [Theory]
    [InlineData("CLEAN", true)]    // uppercase vowel in middle
    [InlineData("THE", false)]     // uppercase, no vowel in middle
    public void Apply_ShouldBeCaseInsensitive(string word, bool shouldBeFiltered)
    {
        var result = _filter.Apply([word]).ToList();

        if (shouldBeFiltered)
            Assert.Empty(result);
        else
            Assert.Single(result);
    }
}