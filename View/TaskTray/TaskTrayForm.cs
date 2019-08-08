using GuiTestHelper.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuiTestHelper.View.TaskTray
{
    public partial class TaskTrayForm : Form
    {
        // キーメッセージ定義
        const int MOD_CONTROL = 0x0002;
        const int MOD_SHIFT = 0x0004;
        const int MOD_WIN = 0x0008;

        /// <summary>
        /// HotKeyのイベントを示すメッセージＩＤ
        /// </summary>
        const int WH_HOTKEY = 0x0312;

        /// <summary>
        /// HotKey登録の際に指定するID
        /// 解除の際や、メッセージ処理を行う際に識別する値となる
        /// 
        /// 0x000 ～ 0xbfff 内の適当な値を指定
        /// </summary>
        const int HOTKEY_ID = 0xbfaa;

        public TaskTrayForm()
        {
            InitializeComponent();

            // ホットキーの登録
            var result = RegisterHotKey(Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, (int)Keys.P);
            Debug.Print("RegisterHotKey:" + result);
        }

        private void ToolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            CaptureRectangular();
        }

        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            // ホットキーの解除
            var result = UnregisterHotKey(Handle, HOTKEY_ID);
            Debug.Print("UnregistHotKey:" + result);

            // 現在のアプリケーションを終了
            System.Windows.Application.Current.Shutdown();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WH_HOTKEY)
            {
                if (((int)m.WParam) == HOTKEY_ID)
                {
                    CaptureRectangular();
                }
            }
        }

        private void CaptureRectangular()
        {
            PrtScViewModel screenvm = new PrtScViewModel();
            if (screenvm.CaptureRectangular() == false)
            {
                Debug.WriteLine("矩形の取得に失敗。何もせずに処理を戻す");
                return;
            }
        }

        [DllImport("user32.dll")]
        extern static int RegisterHotKey(IntPtr HWnd, int ID, int MOD_KEY, int KEY);

        [DllImport("user32.dll")]
        extern static int UnregisterHotKey(IntPtr HWnd, int ID);
    }
}
