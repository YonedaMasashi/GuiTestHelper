using GuiTestHelper.FW;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    /// CaptureEditView.xaml の相互作用ロジック
    /// </summary>
    public partial class CaptureEditView : Window
    {

        private System.Windows.Point clickPoint = new System.Windows.Point(0, 0);
        private System.Windows.Shapes.Rectangle currentRect = null;
        private Stack<System.Windows.Shapes.Rectangle> rectStack = new Stack<System.Windows.Shapes.Rectangle>();

        public DelegateCommand UndoCommand { get; set; }

        public CaptureEditView(Bitmap bitmap)
        {
            InitializeComponent();

            UndoCommand = new DelegateCommand(Undo);

            if (bitmap.Width < 600)
            {
                this.Width = 600;
            } else
            {
                this.Width = bitmap.Width + 20;
            }

            this.Height = bitmap.Height + 100 + 20;
            ImageEditCanvas.Width = bitmap.Width;
            ImageEditCanvas.Height = bitmap.Height;

            IntPtr hbitmap = bitmap.GetHbitmap();
            ImageBg.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                (hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(hbitmap);
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private void Undo()
        {
            if (rectStack.Count() >= 1)
            {
                var removeRect = rectStack.Pop();
                ImageEditCanvas.Children.Remove(removeRect);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageLabel.Foreground = new SolidColorBrush(Colors.Red);
            ErrorMessageLabel.Text = "";

            // エラーチェック
            // ...出力フォルダパス検査
            if (string.IsNullOrEmpty(OutputFolderPath.Text) == true)
            {
                ErrorMessageLabel.Text = "出力フォルダパスを入力してください";
                return;
            }

            if (Directory.Exists(this.OutputFolderPath.Text) == false)
            {
                ErrorMessageLabel.Text = "出力フォルダが存在しません";
                return;
            }

            // ...ファイル名禁則文字チェック
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            if (string.IsNullOrEmpty(FileName.Text) == false && FileName.Text.IndexOfAny(invalidChars) >= 0)
            {
                ErrorMessageLabel.Text = "ファイル名に禁則文字が含まれています";
                return;
            }

            // レイアウトを再計算させる
            var size = new System.Windows.Size(ImageEditCanvas.RenderSize.Width, ImageEditCanvas.RenderSize.Height);
            ImageEditCanvas.Measure(size);
            ImageEditCanvas.Arrange(new Rect(size));

            // VisualObjectをBitmapに変換する
            var renderBitmap = new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96.0d,
                96.0d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(ImageEditCanvas);

            // ファイルパスを作成
            var fileName = this.FileName.Text;
            if (string.IsNullOrEmpty(fileName) == true)
            {
                fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            }
            if (fileName.Contains(".png") == false)
            {
                fileName += ".png";
            }

            var path = System.IO.Path.Combine(this.OutputFolderPath.Text, fileName);
            
            // 出力用のFileStreamを作成する
            using (var os = new FileStream(path, FileMode.Create))
            {
                // 変換したBitmapをエンコードしてFileStreamに保存する。
                // BitmapEncoder が指定されなかった場合は、PNG形式とする。
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(os);

                ErrorMessageLabel.Foreground = new SolidColorBrush(Colors.Blue);
                ErrorMessageLabel.Text = "出力完了";
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            Undo();
        }

        private void ImageEditCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // マウス押下時の座標を保持
            this.clickPoint = e.GetPosition(this.ImageEditCanvas);

            // 描画オブジェクトの生成
            this.currentRect = new System.Windows.Shapes.Rectangle
            {
                Stroke = System.Windows.Media.Brushes.Red,
                StrokeThickness = 2
            };
            Canvas.SetLeft(this.currentRect, clickPoint.X);
            Canvas.SetTop(this.currentRect, clickPoint.Y);

            // オブジェクトをキャンバスに追加
            this.ImageEditCanvas.Children.Add(this.currentRect);
        }

        private void ImageEditCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // 「マウスを左クリックしていないとき、描画中のオブジェクトが無いとき」は何もしない
            if (e.LeftButton == MouseButtonState.Released || this.currentRect == null)
            {
                return;
            }

            // クリック座標とマウス座標から、長方形の位置とサイズを求める
            // ※ 小さい座標が左上位置、大きい座標がサイズ※
            System.Windows.Point mousePoint = e.GetPosition(ImageEditCanvas);
            double x = Math.Min(mousePoint.X, this.clickPoint.X);
            double y = Math.Min(mousePoint.Y, this.clickPoint.Y);
            double width = Math.Max(mousePoint.X, this.clickPoint.X) - x;
            double height = Math.Max(mousePoint.Y, this.clickPoint.Y) - y;

            // 描画中オブジェクトｎ情報を更新
            this.currentRect.Width = width;
            this.currentRect.Height = height;
            Canvas.SetLeft(this.currentRect, x);
            Canvas.SetTop(this.currentRect, y);
        }

        private void ImageEditCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // スタックに積む
            rectStack.Push(this.currentRect);

            // 描画中オブジェクトの参照を削除
            this.currentRect = null;
        }
    }
}
