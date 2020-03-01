using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Homework_16.Annotations;
using Microsoft.Win32;

namespace Homework_16.Models
{
    public class WordCounter : INotifyPropertyChanged
    {
        #region Class Fields
        private FileInfo _fileToGetWords;
        private FileInfo _fileToSearchWords;
        private OpenFileDialog _openFileDialog = new OpenFileDialog();

        #endregion


        #region Class Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<string> WordsToSearch { get; set; } //final collection of words, that need to be searched
        public ObservableCollection<CountedWord> CountedWords { get; set; } //final collection of searched and counted words

        public FileInfo FileToGetWords
        {
            get => _fileToGetWords;
            set
            {
                if (Equals(_fileToGetWords, value)) return;

                _fileToGetWords = value;

                OnPropertyChanged();
            }
        }
        public FileInfo FileToSearchWords
        {
            get => _fileToSearchWords;
            set
            {
                if (Equals(_fileToSearchWords, value)) return;

                _fileToSearchWords = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Class Constructors 
        public WordCounter()
        {
            WordsToSearch = new ObservableCollection<string>();
            CountedWords = new ObservableCollection<CountedWord>();
        }
        #endregion
        #region Class Methods
        public void OpenFileToGetWords(int numberOfWords)
        {
            if (_openFileDialog.ShowDialog() != true) return;
            FileToGetWords = new FileInfo(_openFileDialog.FileName);
            List<string> lines = new List<string>(); //collection to store every line of file
            List<string> words = new List<string>(); //collection to store every word of file
            using (FileStream fs = new FileStream(_fileToGetWords.FullName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine(); //reading every line of file
                        if (!(string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))) //if line isn't empty...
                        {
                            lines.Add(line); //... add it to the collection
                        }
                    }
                }
            }
            foreach (var line in lines)
            {

                var punctuation = line.Where(Char.IsPunctuation).Distinct().ToArray();
                var word = line.Split().Select(x => x.Trim(punctuation)); //split every line into words
                words.AddRange(word); //add words of every line to the collection
            }
            words.RemoveAll(s => string.IsNullOrWhiteSpace(s)); //if some white spaces were counted as words, remove them
            WordsToSearch.Clear();
            int counter = 0;
            foreach (var word in words)
            {
                WordsToSearch.Add(word); 
                if (!(++counter < numberOfWords)) //add as much words as we need (but at least one) to the final collection
                {
                    break;
                }
            }
        }

        public void OpenFileToSearchWords()
        {
            if (_openFileDialog.ShowDialog() != true) return;
            FileToSearchWords = new FileInfo(_openFileDialog.FileName);
        }

        public void CountWords()
        {
            CountedWords.Clear();
            List<string> lines = new List<string>(); //collection to store every line of file
            using (FileStream fs = new FileStream(_fileToSearchWords.FullName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine(); //read every line of file, where we want to find words
                        if (!(string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))) //if line isn't empty...
                        {
                            lines.Add(line); //... add it to the collection
                        }
                    }
                }
            }

            foreach (var word in WordsToSearch)
            {
                int number = 0;
                foreach (var line in lines)
                {
                    number += new Regex(word).Matches(line).Count; //count number of that word in every line of file
                }
                CountedWords.Add(new CountedWord(word, number)); //add counted word to final collection
            }
        }

        public void AddWord(string newWord)
        {
            if (WordsToSearch == null) WordsToSearch = new ObservableCollection<string>();
            WordsToSearch.Add(newWord);
        }





        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion


    }
}
