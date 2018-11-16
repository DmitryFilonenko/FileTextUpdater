using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TextReplacerWpf.Infrstr;
using TextReplacerWpf.Models;

namespace TextReplacerWpf.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler PropertyChanged;
    

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Prorerties

        Visibility _isSerch = Visibility.Visible;
        public Visibility IsSearch
        {
            get { return _isSerch; }
            set
            {
                _isSerch = value;
                OnPropertyChanged();
            }
        }

        string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        string _replaceText;
        public string ReplaceText
        {
            get { return _replaceText; }
            set
            {
                _replaceText = value;
                OnPropertyChanged();
            }
        }

        string _patternText;

        public string PatternText
        {
            get { return _patternText; }
            set
            {
                _patternText = value;
                OnPropertyChanged();
            }
        }

        string _buttonText = "Select folder";

        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        Visibility _imageVisibility = Visibility.Hidden;

        public Visibility ImageVisibility
        {
            get { return _imageVisibility; }
            set
            {
                _imageVisibility = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ReplaceCommand
        ICommand _replaceCommand;
        public ICommand ReplaceCommand
        {
            get
            {
                if (_replaceCommand == null)
                {
                    _replaceCommand = new RelayCommand(
                    p => true,
                    p => Replace());
                }
                return _replaceCommand;
            }
        }

        private void Replace()
        {
            Replacer.TaskFinished += Replacer_TaskFinished;
            InitMediator();
            bool isDirSelected = Replacer.GetPathToDir();
            if (isDirSelected)
            {
                IsSearch = Visibility.Hidden;
                ImageVisibility = Visibility.Visible;
                Replacer.GetFilesAsync();
            }            
        }

        private void Replacer_TaskFinished()
        {
            ImageVisibility = Visibility.Hidden;            
            MessageBox.Show("Найдено файлов для замены - " + Mediator.FilesList.Count);
        }

        private void InitMediator()
        {
            Mediator.Search = SearchText;
            Mediator.Replace = ReplaceText;
            Mediator.Pattern = PatternText;
        }
        #endregion
    }
}
