using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls; // for Canvas

namespace SnippingTool
{
    public partial class SnippingOverlay : Window
    {
        private System.Windows.Point startPoint;
        private Rectangle selectionRectangle;

        public SnippingOverlay()
        {
            InitializeComponent();
            Loaded += (_, _) => Keyboard.Focus(this);
            selectionRectangle = new Rectangle
            {
                Stroke = System.Windows.Media.Brushes.White,
                StrokeThickness = 1
            };
            SnipCanvas.Children.Add(selectionRectangle);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(this);
            Canvas.SetLeft(selectionRectangle, startPoint.X);
            Canvas.SetTop(selectionRectangle, startPoint.Y);
            selectionRectangle.Width = 0;
            selectionRectangle.Height = 0;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Point pos = e.GetPosition(this);

                double x = Math.Min(pos.X, startPoint.X);
                double y = Math.Min(pos.Y, startPoint.Y);
                double width = Math.Abs(pos.X - startPoint.X);
                double height = Math.Abs(pos.Y - startPoint.Y);

                Canvas.SetLeft(selectionRectangle, x);
                Canvas.SetTop(selectionRectangle, y);
                selectionRectangle.Width = width;
                selectionRectangle.Height = height;
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point endPoint = e.GetPosition(this);

            double x = Math.Min(startPoint.X, endPoint.X);
            double y = Math.Min(startPoint.Y, endPoint.Y);
            double width = Math.Abs(endPoint.X - startPoint.X);
            double height = Math.Abs(endPoint.Y - startPoint.Y);

            CaptureAndCopyToClipboard((int)x, (int)y, (int)width, (int)height);
            this.Close();
        }

        private void CaptureAndCopyToClipboard(int x, int y, int width, int height)
        {
            using var bmp = new System.Drawing.Bitmap(width, height);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height), System.Drawing.CopyPixelOperation.SourceCopy);

            System.Windows.Clipboard.SetImage(System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()
            ));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
