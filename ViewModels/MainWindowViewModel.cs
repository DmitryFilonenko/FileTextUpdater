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

        Visibility _isReplace = Visibility.Hidden;
        public Visibility IsReplace
        {
            get { return _isReplace; }
            set
            {
                _isReplace = value;
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

        string _patternText = "*.*";
        public string PatternText
        {
            get { return _patternText; }
            set
            {
                _patternText = value;
                OnPropertyChanged();
            }
        }


        string _beforeReplaceText;
        public string BeforeReplaceText
        {
            get { return _beforeReplaceText; }
            set
            {
                _beforeReplaceText = value;
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

        #region Commands

        ICommand _buttonCommand;
        public ICommand ButtonCommand
        {
            get
            {
                if (_buttonCommand == null)
                {
                    _buttonCommand = new RelayCommand(
                    p => true,
                    p => SelectAction());
                }
                return _buttonCommand;
            }
        }

        private void SelectAction()
        {
            switch (ButtonText)
            {
                case "Select folder":
                    Search();
                    break;
                case "Replace text":
                    Replace();
                    break;
                default:
                    Cancel();
                    return;
            }
        }
        #endregion

        #region SearchFiles

        private void Search()
        {
            Replacer.TaskFinished += Replacer_TaskFinished;
            InitMediator();
            bool isDirSelected = Replacer.GetPathToDir();
            if (isDirSelected)
            {                
                ImageVisibility = Visibility.Visible;
                Replacer.GetFilesAsync();
                ButtonText = "Cancel";
            }            
        }

        private void Replacer_TaskFinished(CurrentTask currentTask)
        {
            ImageVisibility = Visibility.Hidden;

            switch (currentTask)
            {
                case CurrentTask.SearchFiles:
                    BeforeReplaceText = "Files to replace - " + Mediator.FilesList.Count;
                    ButtonText = "Replace text";
                    IsSearch = Visibility.Hidden;
                    IsReplace = Visibility.Visible;
                    break;
                case CurrentTask.ReplaceText:
                    BeforeReplaceText = "Updated files - " + Mediator.UpdatedFiles;
                    ButtonText = "";
                    break;
                default:
                    break;
            }

        }

        private void InitMediator()
        {
            Mediator.Search = SearchText;
            Mediator.Replace = ReplaceText;
            Mediator.Pattern = PatternText;
        }
        #endregion

        #region ReplaceText

        private void Replace()
        {
            InitMediator();
            ImageVisibility = Visibility.Visible;
            Replacer.ReplaceTextAsync();
            ButtonText = "Cancel";
        }
        #endregion

        #region Cancel
        void Cancel()
        {
            MessageBox.Show("Cancel!");
        }
        #endregion
    }
}
