using ReactiveUI;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragPoints
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //定义点
            DragPoint[] m_points = new DragPoint[] { new DragPoint(100, 50), new DragPoint(50, 100), new DragPoint(150, 100),new DragPoint(100,150), new DragPoint(75, 250), new DragPoint(125, 250) };
            //定义线
            DragLine[] m_lines = new DragLine[] { new DragLine(m_points[0], m_points[3]), new DragLine(m_points[1], m_points[3]), new DragLine(m_points[2], m_points[3]), new DragLine(m_points[4], m_points[3]), new DragLine(m_points[5], m_points[3]) };
            object[] m_elements = m_points.Concat<object>(m_lines).ToArray();
            //定义图像
            FileInfo m_fileInfo = new FileInfo("Test.jpg");
            _imageUri = new Uri(m_fileInfo.FullName, UriKind.Absolute);
            var m_bitmap = new BitmapImage();
            m_bitmap.BeginInit();
            m_bitmap.CacheOption = BitmapCacheOption.OnLoad;
            m_bitmap.UriSource = _imageUri;
            m_bitmap.EndInit();
            m_bitmap.Freeze();
            LocalImage.Source = m_bitmap;
            LocalHost.ItemsSource = m_elements;
        }

        private Uri _imageUri;

        private Point _startPoint;

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var m = e.Source as Ellipse;
            m.CaptureMouse();
            _startPoint = e.GetPosition(this);
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var m = e.Source as Ellipse;
            if (m.IsMouseCaptured)
                m.ReleaseMouseCapture();
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            var m = e.Source as Ellipse;
            if (m.IsMouseCaptured)
            {
                Point p = e.GetPosition(this);
                (m.DataContext as DragPoint).Move(p - _startPoint);
                _startPoint = p;
            }
        }
    }

    public sealed class DragPoint : ReactiveObject
    {
        private double _x;

        public double X
        {
            get
            {
                return _x;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref _x, value);
            }
        }

        private double _y;

        public double Y
        {
            get
            {
                return _y;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref _y, value);
            }
        }

        public DragPoint(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public void Move(Vector offset)
        {
            X += offset.X;
            Y += offset.Y;
        }
    }

    public sealed class DragLine : ReactiveObject
    {
        private readonly DragPoint _point1;

        public DragPoint Point1 => _point1;

        private readonly DragPoint _point2;

        public DragPoint Point2 => _point2;

        public DragLine(DragPoint point1, DragPoint point2)
        {
            _point1 = point1;
            _point2 = point2;
        }
    }
}
