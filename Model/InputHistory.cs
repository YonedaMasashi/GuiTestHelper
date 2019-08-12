using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

namespace GuiTestHelper.Model
{
    class InputHistoryList
    {
        private static InputHistoryList _InputHistoryList = new InputHistoryList();

        public InputHistory _InputHistory;
        public InputHistory InputHistory {
            get { return _InputHistory; }
            set { _InputHistory = value; }
        }
        
        private InputHistoryList()
        {
            _InputHistory = new InputHistory();
        }

        public static InputHistoryList Instance()
        {
            return _InputHistoryList;
        }

        public void AddOutputFolder(string folderPath)
        {
            if (_InputHistory.OutputFolderHistory.Contains(folderPath) == true)
            {
                return;
            }
            _InputHistory.OutputFolderHistory.Add(folderPath);

            if (_InputHistory.OutputFolderHistory.Count() > 0 && _InputHistory.OutputFolderHistory.Count() > 20)
            {
                _InputHistory.OutputFolderHistory.Remove(_InputHistory.OutputFolderHistory[0]);
            }
        }

        public void AddOutputFileName(string filePath)
        {
            if (_InputHistory.OutputFileNameHistory.Contains(filePath) == true)
            {
                return;
            }
            _InputHistory.OutputFileNameHistory.Add(filePath);

            if (_InputHistory.OutputFileNameHistory.Count() > 0 && _InputHistory.OutputFileNameHistory.Count() > 20)
            {
                _InputHistory.OutputFileNameHistory.Remove(_InputHistory.OutputFileNameHistory[0]);
            }
        }

        public void OutputHistory()
        {
            _InputHistory.FilterNoExistFilePath();
            string targetListOnJSON = JsonConvert.SerializeObject(_InputHistory, Formatting.Indented);

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(GetFilePath(), false, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                sw.Write(targetListOnJSON);
            }
        }

        public void LoadHistory()
        {
            using (StreamReader sr = new StreamReader(GetFilePath(), Encoding.GetEncoding("Shift_JIS")))
            {
                string jsonstring = sr.ReadToEnd();
                _InputHistory = JsonConvert.DeserializeObject<InputHistory>(jsonstring);
                _InputHistory.FilterNoExistFilePath();
            }
        }


        private string GetFilePath()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPath = Path.Combine(path, "InputHistory.json");
            return fullPath;
        }
    }

    [DataContract]
    class InputHistory
    {
        public InputHistory()
        {
        }

        [DataMember]
        List<string> _outputFolderHistory = new List<string>();
        public List<string> OutputFolderHistory {
            get { return _outputFolderHistory; }
            set { _outputFolderHistory = value; }
        }

        [DataMember]
        List<string> _outputFileNameHistory = new List<string>();
        public List<string> OutputFileNameHistory {
            get { return _outputFileNameHistory; }
            set { _outputFileNameHistory = value; }
        }

        public void FilterNoExistFilePath()
        {
            _outputFolderHistory = _outputFolderHistory.Where(elem => System.IO.Directory.Exists(elem) == true).ToList();
        }
    }
}
