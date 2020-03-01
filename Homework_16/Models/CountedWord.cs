using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_16.Models
{
    public class CountedWord
    {
        public string Word { get; set; }
        public int Frequency { get; set; }

        public CountedWord(string word, int frequency)
        {
            Word = word;
            Frequency = frequency;
        }
    }
}
