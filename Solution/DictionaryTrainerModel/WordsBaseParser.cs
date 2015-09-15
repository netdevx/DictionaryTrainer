using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace AnSoft.DictionaryTrainer.Model
{
    public class WordsBaseParser
    {
        private string[] separators = new string[] { " ", ".", ",", ";", Convert.ToChar(160).ToString() };

        public IEnumerable<Word> ExtractWordsFromFile(string fileName)
        {
            var wordList = new List<Word>();
            
            var rows = new List<string>();
            string line;
            using(var reader = File.OpenText(fileName))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (!String.IsNullOrEmpty(line))
                        rows.Add(line);
                }
            }

            foreach(var s in rows)
            {
                var row = s;
                var firstBracket = row.IndexOf('(');
                var lastBracket = row.LastIndexOf(')');
                if (firstBracket >=0 && lastBracket >= 0)
                    row = row.Remove(firstBracket, lastBracket - firstBracket + 1).Trim();
                var words = row.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim()).ToArray();
                var translations = words.Where(w => this.IsRussian(w));

                var word = new Word() { ID = Guid.NewGuid(), Language = Language.En, Spelling = words[1], UsingFrequencyNumber = wordList.Count + 1, CreateDate = DateTime.Now };
                foreach(var t in translations)
                {
                    var translation = new Word() { ID = Guid.NewGuid(), Language = Language.Rus, Spelling = t, CreateDate = DateTime.Now };
                    word.Translations.Add(translation);
                }
                
                wordList.Add(word);
            }

            return wordList;
        }

        private bool IsRussian(string str)
        {
            char[] chr = str.ToCharArray();

            return chr.All(c => (c >= 'А' && c <= 'ё') || c == '-');
        }
    }
}
