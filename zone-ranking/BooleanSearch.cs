using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvertedIndexDictionary
{
    internal class BooleanSearch
    {
		private List<double> zoneWeights = new List<double> { 0.3, 0.2, 0.5 };
		public List<int> ResultInvertedIndexList { get; private set; } = new List<int>();
        public Dictionary<int, double> Result { get; set; }

        public void GetSearch(Dictionary<string, List<(int fileId, int zone)>> originalResult, string statement)
		{
			Result = new Dictionary<int, double>();
			foreach (var book in FileReader.Books)
			{
				Result.Add(book.Key, 0);
			}
			for(int i = 0; i < 3; i++)
			{
				GetSearchInvertedIndex(originalResult, statement, i);
			}
			Result = Result.OrderByDescending(pair => pair.Value).ToDictionary();
		}

		public void GetSearchInvertedIndex(Dictionary<string, List<(int fileId, int zone)>> originalResult, string statement, int zoneId)
		{
			Dictionary<string, List<int>> result = originalResult
			.ToDictionary(
				kvp => kvp.Key,
				kvp => kvp.Value.Where(tuple => tuple.zone == zoneId).Select(tuple => tuple.fileId).ToList()
			);
			var operationOrderAndWords = statement.Split(' ');
			var operationOrder = new List<string>();
			var wordsIndexes = new List<List<int>>();
			List<int> allBooksIndexes = FileReader.Books.Keys.ToList();

			foreach (var word in operationOrderAndWords)
			{
				if (word == "AND" || word == "OR" || word == "NOT")
				{
					operationOrder.Add(word);
					continue;
				}
				if (!result.ContainsKey(word))
				{
					wordsIndexes.Add(new List<int>());
					continue;
				}

				wordsIndexes.Add(result[word]);
			}

			for (int i = 0; i < operationOrder.Count; i++)
			{
				if (operationOrder[i] == "NOT")
				{
					var firstRow = wordsIndexes[i];
					wordsIndexes.RemoveAt(i);
					wordsIndexes.Insert(i, allBooksIndexes.Except(firstRow).ToList());
					operationOrder.RemoveAt(i);
				}
			}

			foreach (var operation in operationOrder)
			{
				var firstRow = wordsIndexes[0];
				var secondRow = wordsIndexes[1];
				wordsIndexes.RemoveAt(0);
				wordsIndexes.RemoveAt(0);
				wordsIndexes.Insert(0, OperationForInvertedIndex(firstRow, secondRow, operation));
			}

			ResultInvertedIndexList = wordsIndexes[0];

			foreach (var index in ResultInvertedIndexList)
			{
				Result[index] += zoneWeights[zoneId];
			}
		}

		private List<int> OperationForInvertedIndex(List<int> firstRow, List<int> secondRow, string operation)
		{
			switch (operation)
			{
				case "AND":
					return firstRow.Intersect(secondRow).ToList();
				case "OR":
					return firstRow.Union(secondRow).ToList();
				default:
					break;
			}

			return null;
		}


		public void ConsoleOutputInvertedIndex()
        {
            foreach (var book in Result)
			{
				if (book.Value == 0)
					break;
				Console.WriteLine(book.Key + " -> " + FileReader.Books[book.Key]);
			}
        }

    }
}
