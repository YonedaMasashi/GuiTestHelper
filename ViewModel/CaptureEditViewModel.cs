using GuiTestHelper.FW;
using GuiTestHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiTestHelper.ViewModel
{

    public class CaptureEditViewModel : ViewModelBase
    {
        private ObservableCollection<string> _AutoCompOutputFolderPathSource;
        public ObservableCollection<string> AutoCompOutputFolderPathSource {

            get {
                UpdateAutoCompOutputFolderPathSource();
                return _AutoCompOutputFolderPathSource;
            }
        }

        /// <summary>
        /// オートコンプリートで選択されたもの
        /// </summary>
        private string _selectedFolderPath;
        public string SelectedFolderPath {
            get { return _selectedFolderPath; }
            set {
                _selectedFolderPath = value;
                OnPropertyChanged("SelectedFolderPath");
            }
        }

        /// <summary>
        /// オートコンプリートに関わらず、入力した値
        /// </summary>
        private string _selectedFolderPathName;
        public string SelectedFolderPathName {
            get { return _selectedFolderPathName; }
            set {
                _selectedFolderPathName = value;
                OnPropertyChanged("SelectedFolderPathName");
            }
        }

        public void UpdateAutoCompOutputFolderPathSource()
        {
            if (_AutoCompOutputFolderPathSource == null)
            {
                _AutoCompOutputFolderPathSource = new ObservableCollection<string>();
            }

            foreach (var elem in InputHistoryList.Instance().InputHistory.OutputFolderHistory)
            {
                if (_AutoCompOutputFolderPathSource.Contains(elem) == false)
                {
                    _AutoCompOutputFolderPathSource.Add(elem);
                }
            }
        }



        // オートコンプリートリスト:ファイル名
        private ObservableCollection<string> _AutoCompOutputFileNameSource;
        public ObservableCollection<string> AutoCompOutputFileNameSource {

            get {
                UpdateAutoCompOutputFileNameSource();
                return _AutoCompOutputFileNameSource;
            }
        }

        /// <summary>
        /// オートコンプリートで選択されたもの
        /// </summary>
        private string _selectedFileName;
        public string SelectedFileName {
            get { return _selectedFileName; }
            set {
                _selectedFileName = value;
                OnPropertyChanged("SelectedFileName");
            }
        }

        /// <summary>
        /// オートコンプリートに関わらず、入力した値
        /// </summary>
        private string _selectedFileNameName;
        public string SelectedFileNameName {
            get { return _selectedFileNameName; }
            set {
                _selectedFileNameName = value;
                OnPropertyChanged("SelectedFileNameName");
            }
        }
        public void UpdateAutoCompOutputFileNameSource()
        {
            // フォルダパス
            if (_AutoCompOutputFileNameSource == null)
            {
                _AutoCompOutputFileNameSource = new ObservableCollection<string>();
            }
            foreach (var elem in InputHistoryList.Instance().InputHistory.OutputFileNameHistory)
            {
                if (_AutoCompOutputFileNameSource.Contains(elem) == false)
                {
                    _AutoCompOutputFileNameSource.Add(elem);
                }
            }
        }
    }
}
