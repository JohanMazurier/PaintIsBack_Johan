using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaintIsBack
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public int Thickness { get; set; } = 1;

        public System.Windows.Point CurrentPosition { get; set; }

        public Window1()
        {
            InitializeComponent();
        }

        private Point startPoint;

        public Line _line = new Line();

        public Rectangle _rectangle = new Rectangle();

        public Ellipse _ellipse = new Ellipse();

        public Polygon _triangle = new Polygon();

        public bool line = true;

        public bool rectangle = false;

        public bool ellipse = false;

        public bool triangle = false;

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (line)
            {
                _line = new Line();
                startPoint = e.GetPosition(canvas);
                startPoint.Y += 100;
            }

            if (rectangle)
            {
                _rectangle = new Rectangle();
                startPoint = e.GetPosition(canvas);
                Canvas.SetLeft(_rectangle, startPoint.X);
                Canvas.SetTop(_rectangle, startPoint.Y);
                canvas.Children.Add(_rectangle);
            }

            if (ellipse)
            {
                _ellipse = new Ellipse();
                startPoint = e.GetPosition(canvas);
                Canvas.SetLeft(_ellipse, startPoint.X);
                Canvas.SetTop(_ellipse, startPoint.Y);
                canvas.Children.Add(_ellipse);
            }

            if (triangle)
            {
                _triangle = new Polygon();
                startPoint = e.GetPosition(canvas);
/*                Canvas.SetLeft(_triangle, startPoint.X);
                Canvas.SetTop(_triangle, startPoint.Y);*/
                canvas.Children.Add(_triangle);

                _triangle.Points.Add(new Point(0, 0));
                _triangle.Points.Add(new Point(0, 0));
                _triangle.Points.Add(new Point(0, 0));


            }
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (line && e.LeftButton == MouseButtonState.Pressed)
            {

                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeStartLineCap = PenLineCap.Round;
                line.StrokeEndLineCap = PenLineCap.Round;
                line.StrokeThickness = Thickness;
                line.X1 = startPoint.X;
                line.Y1 = startPoint.Y - 100;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y - 100;

                startPoint = e.GetPosition(this);

                canvas.Children.Add(line);
            }

            if (rectangle && e.LeftButton == MouseButtonState.Pressed)
            {
                _rectangle.Stroke = Brushes.Black;
                _rectangle.StrokeThickness = Thickness;
                Canvas.SetLeft(_rectangle, startPoint.X);
                Canvas.SetTop(_rectangle, startPoint.Y);
                var pos = e.GetPosition(canvas);

                var x = Math.Min(pos.X, startPoint.X);
                var y = Math.Min(pos.Y, startPoint.Y);

                var w = Math.Max(pos.X, startPoint.X) - x;
                var h = Math.Max(pos.Y, startPoint.Y) - y;

                _rectangle.Width = w;
                _rectangle.Height = h;

                Canvas.SetLeft(_rectangle, x);
                Canvas.SetTop(_rectangle, y);
            }

            if (ellipse && e.LeftButton == MouseButtonState.Pressed)
            {
                _ellipse.Stroke = Brushes.Black;
                _ellipse.StrokeThickness = Thickness;
                Canvas.SetLeft(_ellipse, startPoint.X);
                Canvas.SetTop(_ellipse, startPoint.Y);
                var pos = e.GetPosition(canvas);

                var x = Math.Min(pos.X, startPoint.X);
                var y = Math.Min(pos.Y, startPoint.Y);

                var w = Math.Max(pos.X, startPoint.X) - x;
                var h = Math.Max(pos.Y, startPoint.Y) - y;

                _ellipse.Width = w;
                _ellipse.Height = h;

                Canvas.SetLeft(_ellipse, x);
                Canvas.SetTop(_ellipse, y);
            }

            if (triangle && e.LeftButton == MouseButtonState.Pressed)
            {
                _triangle.Stroke = Brushes.Black;
                _triangle.StrokeThickness = Thickness;
                Canvas.SetLeft(_triangle, startPoint.X);
                Canvas.SetTop(_triangle, startPoint.Y);
                var pos = e.GetPosition(canvas);

                Point p1 = new Point(0, pos.Y - startPoint.Y);
                Point p2 = new Point(pos.X-startPoint.X, pos.Y- startPoint.Y);
                Point p3 = new Point((pos.X - startPoint.X )/ 2, 0);
                _triangle.Points[0] = p1;
                _triangle.Points[1] = p2;
                _triangle.Points[2] = p3;

                Canvas.SetLeft(_triangle, startPoint.X);
                Canvas.SetTop(_triangle, startPoint.Y);
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (line && e.LeftButton == MouseButtonState.Released)
            {
                startPoint = new Point(0, 0);
            }

            if (rectangle && e.LeftButton == MouseButtonState.Released)
            {
                _rectangle = new Rectangle();
            }

            if (ellipse && e.LeftButton == MouseButtonState.Released)
            {
                _ellipse = new Ellipse();
            }

            if (triangle && e.LeftButton == MouseButtonState.Released)
            {
                _triangle = new Polygon();
            }
        }

        private void ThicknessSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int thickness = ((System.Windows.Controls.ComboBox)e.Source).SelectedIndex + 1;
            Thickness = thickness;
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96d;
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    pngEncoder.Save(ms);
                    ms.Close();
                    File.WriteAllBytes(sfd.FileName, ms.ToArray());
                }
                catch (Exception err)
                {
                    System.Windows.MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }

        private void Line_Click(object sender, RoutedEventArgs e)
        {
            line = true;
            rectangle = false;
            ellipse = false;
            triangle = false;
        }

        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            line = false;
            rectangle = true;
            ellipse = false;
            triangle = false;
        }

        private void Ellipse_Click(object sender, RoutedEventArgs e)
        {
            line = false;
            rectangle = false;
            ellipse = true;
            triangle = false;
        }

        private void Triangle_Click(object sender, RoutedEventArgs e)
        {
            line = false;
            rectangle = false;
            ellipse = false;
            triangle = true;
        }
    }
}
