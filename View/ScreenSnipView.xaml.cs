using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GuiTestHelper.View
{
    /// <summary>
    /// ScreenSnipView.xaml の相互作用ロジック
    /// </summary>
    public partial class ScreenSnipView : Window
    {
        private Point _position;
        private bool _trimEnable = false;

        private bool result = false;
        public bool Result {
            get {
                return result;
            }
        }

        public System.Drawing.Bitmap bmp;

        public ScreenSnipView()
        {
            InitializeComponent();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var path = sender as Path;
            if (path == null) return;

            // 開始座標を取得
            var point = e.GetPosition(path);
            _position = point;

            // マウスキャプチャの設定
            _trimEnable = true;

            path.CaptureMouse();
        }

        private void Path_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var path = sender as Path;
            if (path == null) return;

            // 現在座標を取得
            var point = e.GetPosition(path);

            // マウスキャプチャの終了
            _trimEnable = false;
            this.Cursor = Cursors.Arrow;
            path.ReleaseMouseCapture();

            // 画面キャプチャ
            CaptureScreen(point);

            // アプリケーションの終了
            this.Close();
        }

        private void CaptureScreen(Point point)
        {
            // 座標変換
            var start = PointToScreen(_position);
            var end = PointToScreen(point);

            // キャプチャエリアの取得
            var x = start.X < end.X ? (int)start.X : (int)end.X;
            var y = start.Y < end.Y ? (int)start.Y : (int)end.Y;
            var width = (int)Math.Abs(end.X - start.X);
            var height = (int)Math.Abs(end.Y - start.Y);
            if (width == 0 || height == 0)
            {
                return;
            }

            // スクリーンイメージの取得
            bmp = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format64bppPArgb);
            using (var graph = System.Drawing.Graphics.FromImage(bmp))
            {
                // 画面をコピーする
                graph.CopyFromScreen(new System.Drawing.Point(x, y), new System.Drawing.Point(), bmp.Size);
            }
            result = true;
        }

        private void Path_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_trimEnable)
            {
                return;
            }

            var path = sender as Path;
            if (path == null)
            {
                return;
            }

            // 現在座標を取得
            var point = e.GetPosition(path);

            // キャプチャ領域枠の描画
            DrawStroke(point);
        }

        private void DrawStroke(Point point)
        {
            // 矩形の描画
            var x = _position.X < point.X ? _position.X : point.X;
            var y = _position.Y < point.Y ? _position.Y : point.Y;
            var width = Math.Abs(point.X - _position.X);
            var height = Math.Abs(point.Y - _position.Y);
            ScreenArea.Geometry2 = new RectangleGeometry(new Rect(x, y, width, height));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // プライマリスクリーンサイズの取得
            var screen = System.Windows.Forms.Screen.PrimaryScreen;

            // ウィンドウサイズの設定
            Left = screen.Bounds.Left;
            Top = screen.Bounds.Top;
            Width = screen.Bounds.Width;
            Height = screen.Bounds.Height;
            Cursor = Cursors.Cross;
            // ジオメトリサイズの設定
            ScreenArea.Geometry1 = new RectangleGeometry(new Rect(0, 0, screen.Bounds.Width, screen.Bounds.Height));
        }
    }
}
