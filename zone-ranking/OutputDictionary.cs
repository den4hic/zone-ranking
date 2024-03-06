using System.Text;
using System.Xml.Serialization;

namespace InvertedIndexDictionary
{
    [XmlRoot("OutputDictionary")]
    public class OutputDictionary
    {
        public static int OverallSize { get; set; }

        [XmlArray("Result")]
        [XmlArrayItem("Word")]
        public List<WordItem> Result { get; set; }

        [XmlElement("OverallNumberOfWords")]
        public int OverallNumberOfWords { get; set; }

        public OutputDictionary() { }

        public OutputDictionary(Dictionary<string, List<int>> result, int overallNumberOfWords)
        {
            Result = new List<WordItem>();
            foreach (var entry in result)
            {
                Result.Add(new WordItem { Key = entry.Key, Values = entry.Value });
            }

            OverallNumberOfWords = overallNumberOfWords;
        }

        public void ConsoleOutput()
        {
            foreach (var word in Result)
            {
                Console.Write(word.Key + " ");
                foreach (var wordValue in word.Values)
                {
                    Console.Write(wordValue + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine(WriteInfoAboutText());
        }

        private string WriteInfoAboutText()
        {
            StringBuilder info = new StringBuilder();
            foreach (var book in FileReader.Books)
            {
                info.AppendLine(book.Key + " -> " + book.Value);
            }
            info.AppendLine("Amount of unique words: " + Result.Count);
            info.AppendLine("Amount of books: " + Result.SelectMany(x => x.Values).Distinct().Count());
            info.AppendLine("Overall number of words: " + OverallNumberOfWords);
            info.AppendLine("Overall size of all books: " + OverallSize);
            return info.ToString();
        }

        public void FileOutput()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(WriteInfoAboutText());

            foreach (var word in Result)
            {
                stringBuilder.Append(word.Key + " ");
                foreach (var wordValue in word.Values)
                {
                    stringBuilder.Append(wordValue + " ");
                }
                stringBuilder.AppendLine();
            }

            File.WriteAllText(@"../../../result.txt", stringBuilder.ToString());
        }

        public void Serialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OutputDictionary));

            using (FileStream fileStream = new FileStream("../../../result.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fileStream, this);
            }
        }
    }

    public class WordItem
    {
        [XmlElement("Key")]
        public string? Key { get; set; }

        [XmlArray("Values")]
        [XmlArrayItem("Value")]
        public List<int>? Values { get; set; }
    }
}
