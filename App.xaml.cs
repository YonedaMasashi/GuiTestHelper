using GuiTestHelper.Model;
using GuiTestHelper.View.TaskTray;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GuiTestHelper
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private TaskTrayForm taskTrayForm;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            taskTrayForm = new TaskTrayForm();

            InputHistoryList.Instance().LoadHistory();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            taskTrayForm.Dispose();
        }
    }
}
