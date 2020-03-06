using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Pen = System.Drawing.Pen;
using SystemColors = System.Windows.SystemColors;

namespace SmartSign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private Line line;
        private bool d_st = false;
        private float preX;
        private float preY;


        private void Paint_dock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            System.Windows.Point p = e.GetPosition(this);
            d_st = true;
            preX =(float) p.X;
            preY = (float) p.Y;
            
        }

        private void Paint_dock_MouseUp(object sender, MouseButtonEventArgs e)
        {
          d_st = false;
        }

        private void Paint_dock_MouseMove(object sender, MouseEventArgs e)
        {
            if (d_st)
            {
                try
                {
                    System.Windows.Point p2 = e.GetPosition(this);
                    line = new Line();
                    line.Stroke = SystemColors.WindowFrameBrush;
                    line.StrokeThickness = 3;
                    float x2 = (float)p2.X;
                    float y2 = (float)p2.Y;
                    line.X1 = preX;
                    line.Y1 = preY;
                    line.X2 = x2;
                    line.Y2 = y2;
                    preX = x2;
                    preY = y2;
                    paint_dock.Children.Add(line);
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Clr_bt_Click(object sender, RoutedEventArgs e)
        {
            paint_dock.Children.Clear();
        }

        private void set_ui(object sender, RoutedEventArgs e)
        {

        }

        private void Save_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)paint_dock.RenderSize.Width, (int)paint_dock.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                renderTarget.Render(paint_dock);
                var croped_image = new CroppedBitmap(renderTarget, new Int32Rect(200, 50, 350, 200));
                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(croped_image));
                String file_path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "smart_sign.jpg");
                using (var io = new FileStream(file_path,FileMode.Create))
                {
                    encoder.Save(io);
                }
                MessageBox.Show("Signature saved Successfully at" + Environment.NewLine + file_path);
                paint_dock.Children.Clear();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    
}


