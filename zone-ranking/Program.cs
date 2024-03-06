using InvertedIndexDictionary;
using System.Collections.Generic;
using System.Diagnostics;

List<string> files = new List<string>() { "book1.fb2", "book2.fb2", "book3.fb2", "book4.fb2", "book5.fb2", "book6.fb2", "book7.fb2", "book8.fb2", "book9.fb2", "book10.fb2" };
List<int> pointer = new List<int>();
List<Dictionary<string, List<int>>> allWords = new List<Dictionary<string, List<int>>>();
List<Dictionary<string, List<(int fileId, int zone)>>> allWordsWithZones = new List<Dictionary<string, List<(int fileId, int zone)>>>();

int overallNumberOfWords = 0;

for (int i = 0; i < files.Count; i++)
{
    pointer.Add(0);
    TextParser textParser = new TextParser(FileReader.GetTextFromFile(files[i]), i);

	allWordsWithZones.Add(textParser.GetWordsWithZones());

    overallNumberOfWords += textParser.NumberOfWords;
}

Dictionary<string, List<(int fileId, int zone)>> result = new Dictionary<string, List<(int fileId, int zone)>>();

for (int i = 0; i < files.Count; i++)
{
    foreach (var word in allWordsWithZones[i])
    {
        if (!result.ContainsKey(word.Key))
        {
            result[word.Key] = new List<(int fileId, int zone)>();
        }
        result[word.Key].AddRange(word.Value);
    }
}

result = result.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

BooleanSearch booleanSearch = new BooleanSearch();

booleanSearch.GetSearch(result, "cat OR the");

booleanSearch.ConsoleOutputInvertedIndex();