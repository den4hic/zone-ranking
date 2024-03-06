using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace InvertedIndexDictionary
{
    internal class FileReader
    {
        public static int NumberOfFiles { get; set; }
        public static Dictionary<int, string> Books { get; set; } = new Dictionary<int, string>();
        public static List<string> GetTextFromFile(string fileName)
        {
            XDocument xDocument = XDocument.Load("../../../books/" + fileName);
            FileInfo file = new FileInfo("../../../books/" + fileName);
            OutputDictionary.OverallSize += (int)file.Length / 1024;

            XElement? text = xDocument.Element("{http://www.gribuser.ru/xml/fictionbook/2.0}FictionBook")?
                .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}body");

            XElement? title = xDocument.Element("{http://www.gribuser.ru/xml/fictionbook/2.0}FictionBook")?
                .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}description")?
                .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}title-info")?
                .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}book-title");

            XElement? authorFirstName = xDocument.Element("{http://www.gribuser.ru/xml/fictionbook/2.0}FictionBook")?
               .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}description")?
               .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}title-info")?
               .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}author")?
               .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}first-name");

			XElement? authorSecondName = xDocument.Element("{http://www.gribuser.ru/xml/fictionbook/2.0}FictionBook")?
			   .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}description")?
			   .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}title-info")?
			   .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}author")?
			   .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}last-name");

            XElement? genreInfo = xDocument.Element("{http://www.gribuser.ru/xml/fictionbook/2.0}FictionBook")?
               .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}description")?
               .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}title-info")?
               .Element("{http://www.gribuser.ru/xml/fictionbook/2.0}genre");

			string titleName = title.Value;

            string titleInfo = genreInfo.Value + " " + titleName;

            string authorName = "";

            if (authorFirstName != null && authorSecondName != null)
            {
				authorName = authorFirstName.Value + " " + authorSecondName.Value;
			}

            Books.Add(NumberOfFiles, titleName);

            Console.OutputEncoding = UTF8Encoding.UTF8;

            NumberOfFiles++;

            List<string> zoneStrings = new List<string>();

            zoneStrings.Add(titleInfo);
            zoneStrings.Add(authorName);
            zoneStrings.Add(text.Value);

            return zoneStrings;
        }
    }
}
