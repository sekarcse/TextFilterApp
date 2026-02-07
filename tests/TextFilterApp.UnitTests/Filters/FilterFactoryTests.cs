using FluentAssertions;
using TextFilterApp.Application.Filters;
using TextFilterApp.Domain.Interfaces;

namespace TextFilterApp.UnitTests.Filters;

public class FilterFactoryTests
{
    [Fact]
    public void CreateVowelMiddleFilter_WhenCalled_ShouldReturnInstance()
    {
        // Act
        var result = FilterFactory.CreateVowelMiddleFilter();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void CreateVowelMiddleFilter_WhenCalled_ShouldReturnITextFilterImplementation()
    {
        // Act
        var result = FilterFactory.CreateVowelMiddleFilter();

        // Assert
        result.Should().BeAssignableTo<ITextFilter>();
    }

    [Fact]
    public void CreateVowelMiddleFilter_WhenUsedToFilter_ShouldRemoveVowelMiddleWords()
    {
        // Act
        var filter = FilterFactory.CreateVowelMiddleFilter();
        var result = filter.Apply(["clean", "the"]).ToList();

        // Assert - "clean" has 'e' in middle (filtered), "the" has 'h' in middle (kept)
        result.Should().ContainSingle()
            .Which.Should().Be("the");
    }

    [Fact]
    public void CreateMinimumLengthFilter_WhenCalled_ShouldReturnInstance()
    {
        // Act
        var result = FilterFactory.CreateMinimumLengthFilter(3);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void CreateMinimumLengthFilter_WhenCalled_ShouldReturnITextFilterImplementation()
    {
        // Act
        var result = FilterFactory.CreateMinimumLengthFilter(3);

        // Assert
        result.Should().BeAssignableTo<ITextFilter>();
    }

    [Fact]
    public void CreateMinimumLengthFilter_WhenUsedToFilter_ShouldRemoveShortWords()
    {
        // Act
        var filter = FilterFactory.CreateMinimumLengthFilter(4);
        var result = filter.Apply(["hi", "hello", "go", "world"]).ToList();

        // Assert - "hi" and "go" filtered (< 4 chars)
        result.Should().HaveCount(2)
            .And.Equal(["hello", "world"]);
    }

    [Fact]
    public void CreateContainsLetterFilter_WhenCalled_ShouldReturnInstance()
    {
        // Act
        var result = FilterFactory.CreateContainsLetterFilter('t');

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void CreateContainsLetterFilter_WhenCalled_ShouldReturnITextFilterImplementation()
    {
        // Act
        var result = FilterFactory.CreateContainsLetterFilter('t');

        // Assert
        result.Should().BeAssignableTo<ITextFilter>();
    }

    [Fact]
    public void CreateContainsLetterFilter_WhenUsedToFilter_ShouldRemoveWordsContainingLetter()
    {
        // Act
        var filter = FilterFactory.CreateContainsLetterFilter('t');
        var result = filter.Apply(["test", "hello", "the", "world"]).ToList();

        // Assert - "test" and "the" filtered (contain 't')
        result.Should().HaveCount(2)
            .And.Equal(["hello", "world"]);
    }

    [Fact]
    public void CreateVowelMiddleFilter_WhenCalledMultipleTimes_ShouldReturnDistinctInstances()
    {
        // Act
        var filter1 = FilterFactory.CreateVowelMiddleFilter();
        var filter2 = FilterFactory.CreateVowelMiddleFilter();

        // Assert - each call should create a new instance
        filter1.Should().NotBeSameAs(filter2);
    }
}
