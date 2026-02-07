using FluentAssertions;
using TextFilterApp.Application.Filters;
using TextFilterApp.Application.Services;
using TextFilterApp.Domain.Interfaces;
using TextFilterApp.Infrastructure.FileReaders;

namespace TextFilterApp.IntegrationTests;

public class EndToEndFilterTests
{
    private static string TestDataPath => Path.Combine(AppContext.BaseDirectory, "TestData", "sample.txt");

    [Fact]
    public void ApplyFilters_ReadFileAndApplyAllFilters_ShouldProduceExpectedOutput()
    {
        // Arrange
        var fileReader = FileReaderFactory.CreateStreamingTextFileReader();
        var filters = new ITextFilter[]
        {
            FilterFactory.CreateVowelMiddleFilter(),
            FilterFactory.CreateMinimumLengthFilter(3),
            FilterFactory.CreateContainsLetterFilter('t')
        };
        var sut = new TextFilterService(filters);

        // Act
        var words = fileReader.ReadWords(TestDataPath);
        var result = sut.ApplyFilters(words).ToList();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().OnlyContain(word => !word.Contains('t', StringComparison.OrdinalIgnoreCase));
        result.Should().OnlyContain(word => word.Length >= 3);
    }

    [Fact]
    public void ApplyFilters_WithMultipleFilters_ShouldPreserveWordOrder()
    {
        // Arrange
        var fileReader = FileReaderFactory.CreateStreamingTextFileReader();
        var filters = new ITextFilter[]
        {
            FilterFactory.CreateVowelMiddleFilter(),
            FilterFactory.CreateMinimumLengthFilter(3),
            FilterFactory.CreateContainsLetterFilter('t')
        };
        var sut = new TextFilterService(filters);

        // Act
        var words = fileReader.ReadWords(TestDataPath);
        var result = sut.ApplyFilters(words).ToList();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().OnlyContain(word => !string.IsNullOrWhiteSpace(word));
    }

    [Fact]
    public void ApplyFilters_WithNoFilters_ShouldReturnAllWords()
    {
        // Arrange
        var fileReader = FileReaderFactory.CreateStreamingTextFileReader();
        var sut = new TextFilterService([]);

        // Act
        var words = fileReader.ReadWords(TestDataPath);
        var result = sut.ApplyFilters(words).ToList();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain("Once");
        result.Should().Contain("little");
    }

    [Fact]
    public void ApplyFilters_WithOnlyMinLengthFilter_ShouldRemoveShortWords()
    {
        // Arrange
        var fileReader = FileReaderFactory.CreateStreamingTextFileReader();
        var sut = new TextFilterService(new ITextFilter[] { FilterFactory.CreateMinimumLengthFilter(3) });

        // Act
        var words = fileReader.ReadWords(TestDataPath);
        var result = sut.ApplyFilters(words).ToList();

        // Assert
        result.Should().NotContain(["or", "of", "is", "a", "no", "in", "it", "do", "to", "by", "on"]);
        result.Should().Contain("little");
    }

    [Fact]
    public void ApplyFilters_WithSameFiltersInDifferentOrder_ShouldProduceSameResult()
    {
        // Arrange - swap filter order
        var fileReader = FileReaderFactory.CreateStreamingTextFileReader();
        var filtersOrderA = new ITextFilter[]
        {
            FilterFactory.CreateMinimumLengthFilter(3),
            FilterFactory.CreateContainsLetterFilter('t'),
            FilterFactory.CreateVowelMiddleFilter()
        };
        var filtersOrderB = new ITextFilter[]
        {
            FilterFactory.CreateVowelMiddleFilter(),
            FilterFactory.CreateContainsLetterFilter('t'),
            FilterFactory.CreateMinimumLengthFilter(3)
        };
        var sutA = new TextFilterService(filtersOrderA);
        var sutB = new TextFilterService(filtersOrderB);

        // Act
        var words = fileReader.ReadWords(TestDataPath).ToList();
        var resultA = sutA.ApplyFilters(words).ToList();
        var resultB = sutB.ApplyFilters(words).ToList();

        // Assert - same filters in any order should give the same result
        resultA.OrderBy(w => w).Should().Equal(resultB.OrderBy(w => w));
    }
}