using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace InvertedIndexDictionary
{
    internal class TextParser
    {
        public string[] Words { get; set; }
        public string Text { get; }
        public List<string> TextWithZones { get; set; }
        public int Id { get; set; }
        public int NumberOfWords { get; set; } 
        public TextParser(string text, int id)
        {
            Text = text;
            Id = id;
            Words = RemoveSpecialCharacters(text);
        }

		public TextParser(List<string> textWithZones, int id)
		{
			TextWithZones = textWithZones;
			Id = id;
		}

		private string[] RemoveSpecialCharacters(string word)
        {
            return word.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '-', '!', '?', '—', '…', '*', '¡', '¿', '‘', '’', '”', '“', '"', '(', ')', '/', '#', '•', '[', '©', '+', '=', '<', '>', '«', '»', '`' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public Dictionary<string, List<int>> GetWords()
        {
            Dictionary<string, List<int>> wordBookIds = new Dictionary<string, List<int>>();
            IEnumerable<string> words = Words;

            NumberOfWords = words.Count();

            foreach (string word in words)
            {
                if (!int.TryParse(word, out int index))
                {
                    string lowercaseWord = word.ToLower().Trim();

                    if (!lowercaseWord.StartsWith("'") && !lowercaseWord.StartsWith("...") && !lowercaseWord.EndsWith("...") &&!wordBookIds.ContainsKey(lowercaseWord))
                    {
                        wordBookIds[lowercaseWord] = new List<int>();
                        wordBookIds[lowercaseWord].Add(Id);
                    }

                }
            }

            wordBookIds = wordBookIds.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            return wordBookIds;
        }

		static bool ContainsEnglishCharacters(string input)
		{
			return Regex.IsMatch(input, @"^[a-zA-Z]+$");
		}

		public Dictionary<string, List<(int fileId, int zone)>> GetWordsWithZones()
		{
            Dictionary<string, List<(int fileId, int zone)>> wordBookIds = new Dictionary<string, List<(int fileId, int zone)>>();

            for(int i = 0; i < 3; i++)
            {
                var itemWords = RemoveSpecialCharacters(TextWithZones[i]);
                
                foreach (var word in itemWords)
                {
					string lowercaseWord = word.ToLower().Trim();
					if (ContainsEnglishCharacters(lowercaseWord))
                    {
						if (!lowercaseWord.StartsWith("'") && !lowercaseWord.StartsWith("...") && !lowercaseWord.EndsWith("...") && !wordBookIds.ContainsKey(lowercaseWord))
                        {
                            wordBookIds[lowercaseWord] = new List<(int fileId, int zone)>();
                            wordBookIds[lowercaseWord].Add((Id, i));
						} else if (wordBookIds.ContainsKey(lowercaseWord) && !wordBookIds[lowercaseWord].Contains((Id, i)))
                        {
                            wordBookIds[lowercaseWord].Add((Id, i));
                        }
					}
				}
            }

            wordBookIds = wordBookIds.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            return wordBookIds;
		}
	}
}
