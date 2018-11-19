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
        event Action CancelByUser;
        public enum State { BeforeSearch, Search, AfterSearch, Replace, Done, Cancel}

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


        string _currentText;
        public string CurrentText
        {
            get { return _currentText; }
            set
            {
                _currentText = value;
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
                case "Next search":
                    SetState(State.BeforeSearch);
                    break;
                default:
                    Cancel();
                    return;
            }
        }
        #endregion

        #region SearchFiles

        private void SetState(State state)
        {
            switch (state)
            {            
                case State.BeforeSearch:
                    ImageVisibility = Visibility.Hidden;
                    IsSearch = Visibility.Visible;
                    IsReplace = Visibility.Hidden;
                    ButtonText = "Select folder";
                    SearchText = "";
                    ReplaceText = "";
                    PatternText = "*.*";
                    break;
                case State.Search:
                    ImageVisibility = Visibility.Visible;
                    IsSearch = Visibility.Visible;
                    IsReplace = Visibility.Hidden;
                    ButtonText = "Cancel";
                    break;
                case State.AfterSearch:
                    ImageVisibility = Visibility.Hidden;
                    IsSearch = Visibility.Hidden;
                    IsReplace = Visibility.Visible;
                    CurrentText = "Files to replace - " + Mediator.FilesList.Count;
                    ButtonText = "Replace text";
                    break;
                case State.Replace:
                    ImageVisibility = Visibility.Visible;                    
                    IsSearch = Visibility.Hidden;
                    IsReplace = Visibility.Visible;
                    CurrentText = "Files to replace - " + Mediator.FilesList.Count;
                    ButtonText = "Cancel";
                    break;
                case State.Cancel:
                    ImageVisibility = Visibility.Hidden;
                    IsSearch = Visibility.Hidden;
                    IsReplace = Visibility.Visible;
                    CurrentText = "Canceled by user.";
                    ButtonText = "Next search";
                    break;
                case State.Done:
                    ImageVisibility = Visibility.Hidden;
                    IsSearch = Visibility.Hidden;
                    IsReplace = Visibility.Visible;
                    CurrentText = "Updated " + Mediator.UpdatedFiles + " files";
                    ButtonText = "Next search";
                    break;
            }
        }

        private void Search()
        {            
            Replacer.TaskFinished += Replacer_TaskFinished;
            CancelByUser += MainWindowViewModel_CancelByUser;
            InitMediator();
            
            bool isDirSelected = Replacer.GetPathToDir();
            if (isDirSelected)
            {
                SetState(State.Search);
                Replacer.GetFilesAsync();                
            }            
        }

        private void MainWindowViewModel_CancelByUser()
        {
            if (Mediator.AsyncThread != null)
            {
                Mediator.AsyncThread.Abort();
                SetState(State.Cancel);                
            }
        }

        private void Replacer_TaskFinished(CurrentTask currentTask)
        {
            SetState(State.Done);            

            switch (currentTask)
            {
                case CurrentTask.SearchFiles:
                    SetState(State.AfterSearch);                    
                    break;
                case CurrentTask.ReplaceText:
                    SetState(State.Done);                    
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
            SetState(State.Replace);
            Replacer.ReplaceTextAsync();
            ButtonText = "Cancel";
        }
        #endregion

        #region Cancel
        void Cancel()
        {
            CancelByUser?.Invoke();
        }
        #endregion
    }
}
