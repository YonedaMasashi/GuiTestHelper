using GuiTestHelper.FW;
using GuiTestHelper.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiTestHelper.ViewModel
{
    class PrtScViewModel : ViewModelBase
    {
        Bitmap _bmp = null;

        internal bool CaptureRectangular()
        {
            ScreenSnipView view = new ScreenSnipView();
            view.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            view.ShowDialog();
            if (view.Result == false)
            {
                return false;
            }
            _bmp = view.bmp;

            CaptureEditView captureEditView = new CaptureEditView(_bmp);
            captureEditView.Show();

            return true;
        }
    }
}
