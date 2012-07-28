using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VidFilter
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        public ImageWindow()
        {
            InitializeComponent();
        }

        public void SetImage(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                return;
            }
            this.SampleVideoFrame.Stretch = Stretch.None;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmap.EndInit();

            this.Height = this.MinHeight = bitmap.Height;
            this.Width = this.MinWidth = bitmap.Width;
            this.SampleVideoFrame.Source = bitmap;
        }
    }
}
