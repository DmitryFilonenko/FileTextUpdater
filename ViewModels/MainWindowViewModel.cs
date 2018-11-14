using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
            MessageBox.Show("Search - " + SearchText + " Replace - " + ReplaceText + " Pattern - " + PatternText);
        }
        #endregion
    }
}
