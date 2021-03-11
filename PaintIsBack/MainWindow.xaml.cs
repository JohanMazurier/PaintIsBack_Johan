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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintIsBack
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Thickness { get; set; } = 1;
        public System.Windows.Point CurrentPosition { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
                CurrentPosition = e.GetPosition(this);
            
                
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeStartLineCap = PenLineCap.Round;
                line.StrokeEndLineCap = PenLineCap.Round;
                line.StrokeThickness = Thickness;
                line.X1 = CurrentPosition.X;
                line.Y1 = CurrentPosition.Y-80;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y-80;

                CurrentPosition = e.GetPosition(this);

                Canvas.Children.Add(line);
            }
        }

        private void ThicknessSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int thickness = ((ComboBox)e.Source).SelectedIndex +1;
            Thickness = thickness;
        }
    }
}
