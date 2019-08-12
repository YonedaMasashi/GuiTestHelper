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

    public class AutoCompData
    {
        public string Name { get; set; }
    }

    public class CaptureEditViewModel : ViewModelBase
    {

        // オートコンプリートリスト:フォルダパス
        private ObservableCollection<AutoCompData> _AutoCompOutputFolderPathSource;
        public ObservableCollection<AutoCompData> AutoCompOutputFolderPathSource {

            get {
                UpdateAutoCompOutputFolderPathSource();
                return _AutoCompOutputFolderPathSource;
            }
        }

        /// <summary>
        /// オートコンプリートで選択されたもの
        /// </summary>
        private AutoCompData _selectedFolderPath;
        public AutoCompData SelectedFolderPath {
            get { return _selectedFolderPath; }
            set {
                _selectedFolderPath = value;
            }
        }

        /// <summary>
        /// オートコンプリートに関わらず、入力した値
        /// </summary>
        private string _selectedFolderPathName;
        public string SelectedFolderPathName {
            get { return _selectedFolderPathName; }
            set { _selectedFolderPathName = value; }
        }

        public void UpdateAutoCompOutputFolderPathSource()
        {
            if (_AutoCompOutputFolderPathSource == null)
            {
                _AutoCompOutputFolderPathSource = new ObservableCollection<AutoCompData>();
            }
            _AutoCompOutputFolderPathSource.Clear();

            foreach (var elem in InputHistoryList.Instance().InputHistory.OutputFolderHistory)
            {
                _AutoCompOutputFolderPathSource.Add(new AutoCompData() { Name = elem });
            }
        }



        // オートコンプリートリスト:ファイル名
        private ObservableCollection<AutoCompData> _AutoCompOutputFileNameSource;
        public ObservableCollection<AutoCompData> AutoCompOutputFileNameSource {

            get {
                UpdateAutoCompOutputFileNameSource();
                return _AutoCompOutputFileNameSource;
            }
        }

        /// <summary>
        /// オートコンプリートで選択されたもの
        /// </summary>
        private AutoCompData _selectedFileName;
        public AutoCompData SelectedFileName {
            get { return _selectedFileName; }
            set {
                _selectedFileName = value;
            }
        }

        /// <summary>
        /// オートコンプリートに関わらず、入力した値
        /// </summary>
        private string _selectedFileNameName;
        public string SelectedFileNameName {
            get { return _selectedFileNameName; }
            set { _selectedFileNameName = value; }
        }
        public void UpdateAutoCompOutputFileNameSource()
        {
            // フォルダパス
            if (_AutoCompOutputFileNameSource == null)
            {
                _AutoCompOutputFileNameSource = new ObservableCollection<AutoCompData>();
            }
            foreach (var elem in InputHistoryList.Instance().InputHistory.OutputFileNameHistory)
            {
                _AutoCompOutputFileNameSource.Add(new AutoCompData() { Name = elem });
            }
        }
    }
}
