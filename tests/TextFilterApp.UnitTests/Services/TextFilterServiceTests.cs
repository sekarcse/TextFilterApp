using FluentAssertions;
using TextFilterApp.Application.Filters;
using TextFilterApp.Application.Services;
using TextFilterApp.Domain.Interfaces;

namespace TextFilterApp.UnitTests.Services;

public class TextFilterServiceTests
{
    [Fact]
    public void ApplyFilters_WithNoFilters_ShouldReturnAllWords()
    {
        // Arrange
        var sut = new TextFilterService([]);
        var input = new[] { "hello", "world" };

        // Act
        var result = sut.ApplyFilters(input).ToList();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public void ApplyFilters_WithSingleFilter_ShouldApplyIt()
    {
        // Arrange
        var filters = new ITextFilter[] { new MinimumLengthFilter(4) };
        var sut = new TextFilterService(filters);
        var input = new[] { "hi", "hello", "world", "go" };

        // Act
        var result = sut.ApplyFilters(input).ToList();

        // Assert
        result.Should().HaveCount(2)
            .And.Equal(["hello", "world"]);
    }

    [Fact]
    public void ApplyFilters_WithMultipleFilters_ShouldChainThem()
    {
        // Arrange - filter words < 3 chars, then filter words with 't'
        var filters = new ITextFilter[]
        {
            new MinimumLengthFilter(3),
            new ContainsLetterFilter('t')
        };
        var sut = new TextFilterService(filters);
        var input = new[] { "I", "am", "the", "king", "of", "code" };

        // Act
        var result = sut.ApplyFilters(input).ToList();

        // Assert - "I","am","of" removed by length; "the" removed by 't'; "king","code" remain
        result.Should().HaveCount(2)
            .And.Equal(["king", "code"]);
    }

    [Fact]
    public void ApplyFilters_WithAllThreeFilters_ShouldApplyInOrder()
    {
        // Arrange
        var filters = new ITextFilter[]
        {
            new VowelMiddleFilter(),
            new MinimumLengthFilter(3),
            new ContainsLetterFilter('t')
        };
        var sut = new TextFilterService(filters);
        var input = new[] { "the", "clean", "rather", "what", "currently", "shy", "hymn", "test" };

        // Act
        var result = sut.ApplyFilters(input).ToList();

        // Assert
        result.Should().HaveCount(2)
            .And.Equal(["shy", "hymn"]);
    }

    [Fact]
    public void Constructor_WithNullFilters_ShouldThrow()
    {
        // Act & Assert
        var act = () => new TextFilterService(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ApplyFilters_WithEmptyInput_ShouldReturnEmpty()
    {
        // Arrange
        var sut = new TextFilterService(new ITextFilter[] { new MinimumLengthFilter(3) });

        // Act
        var result = sut.ApplyFilters([]).ToList();

        // Assert
        result.Should().BeEmpty();
    }
}