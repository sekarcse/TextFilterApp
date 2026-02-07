using TextFilterApp.Application.Filters;
using TextFilterApp.Application.Services;
using TextFilterApp.Domain.Interfaces;
using TextFilterApp.Infrastructure.FileReaders;

string filePath = args.Length > 0 ? args[0] : "samples/input.txt";

IFileReader fileReader = FileReaderFactory.CreateStreamingTextFileReader();
ITextFilter[] filters =
[
    FilterFactory.CreateVowelMiddleFilter(),
    FilterFactory.CreateMinimumLengthFilter(3),
    FilterFactory.CreateContainsLetterFilter('t')
];
ITextFilterService filterService = new TextFilterService(filters);

var words = fileReader.ReadWords(filePath);
var filteredWords = filterService.ApplyFilters(words);

Console.WriteLine("Filtered output:");
Console.WriteLine(string.Join(" ", filteredWords));