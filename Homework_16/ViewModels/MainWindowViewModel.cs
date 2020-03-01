using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMVVM.Common;
using CommonMVVM.ViewModel;
using Homework_16.Models;

namespace Homework_16.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Class Properties
        public WordCounter MyWordCounter { get; set; } //object of Model
        public int WordsAmount { get; set; } = 0;
        #endregion

        #region Class Commands
        public DelegateCommand OpenFileToGetWordsCommand { get; set; }

        public DelegateCommand OpenFileToSearchWordsCommand { get; set; }
        public DelegateCommand CountWordsCommand { get; set; }
        public DelegateCommand AddWordCommand { get; set; } 
        #endregion


        #region Class Constuctors
        public MainWindowViewModel()
        {
            MyWordCounter = new WordCounter();
            OpenFileToGetWordsCommand = new DelegateCommand(OnOpenFileToGetWords);
            OpenFileToSearchWordsCommand = new DelegateCommand(OnOpenFileToSearchWords);
            CountWordsCommand = new DelegateCommand(OnCountWords, CanCountWords);
            AddWordCommand = new DelegateCommand(OnAddWord, CanAddWord);
        }
        #endregion

        #region Class Methods
        private void OnOpenFileToGetWords(object obj)
        {
            MyWordCounter.OpenFileToGetWords(WordsAmount);
        }

        private void OnOpenFileToSearchWords(object obj)
        {
            MyWordCounter.OpenFileToSearchWords();
        }

        private void OnCountWords(object obj)
        {
            MyWordCounter.CountWords();
        }
        private bool CanCountWords(object obj)
        {
            return MyWordCounter?.WordsToSearch.Count > 0 && !string.IsNullOrEmpty(MyWordCounter?.FileToSearchWords?.FullName);
        }

        private void OnAddWord(object obj)
        {
            MyWordCounter.AddWord(obj.ToString());
        }

        private bool CanAddWord(object obj)
        {
            if (!(obj is string word)) return false;
            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrEmpty(word)) return false;
            return true;
        } 
        #endregion
    }
}
