﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public interface IWordSessionProvider
    {
        IEnumerable<Word> GetNextWords(Language language);

        IEnumerable<Word> GetWordsToRepeat(Language language);
    }
}
