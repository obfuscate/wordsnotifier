using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsNotifier
{
    public class Translation
    {
        public Translation(string p, string t)
        {
            part = p;
            translation = t;
            frequency = 0;
        }

        public string part;
        public string translation;
        public int frequency;
    }

    public class Translations
    {
        public Translations(int f)
        {
            frequency = f;
            translations = new List<Translation>();
        }

        public int frequency;
        //-- part, translation.
        public List<Translation> translations;
    }

    public class Word
    {
        public Word(string w, string p, string t)
        {
            word = w;
            part = p;
            translation = t;
        }

        public string word;
        public string part;
        public string translation;
    }
}
