using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AtlasPadder
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

        private BitmapImage _bitMapImage;
        private string _fileNameSafe;

        private int _imageWidth;
        private int _imageHeight;

        private int _tileSize = 16;

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                _fileNameSafe = dlg.SafeFileName;
                FileNameLabel.Content = selectedFileName;
                _bitMapImage = new BitmapImage();
                _bitMapImage.BeginInit();
                _bitMapImage.UriSource = new Uri(selectedFileName);
                _bitMapImage.EndInit();
                ImageViewer1.Source = _bitMapImage;


                Process.IsEnabled = true;
            }
        }

        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            Bitmap bitMap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(_bitMapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                bitMap = new Bitmap(bitmap);
            }

            _imageWidth = bitMap.Width;
            _imageHeight = bitMap.Height;

            _tileSize = int.Parse(TileSizeInput.Text);

            int totalTiles = _imageWidth / _tileSize;

            //extra row/column per side of a square with the length of the tile
            int newPixelsPerTile = _tileSize * 4;

            //multiply by number of tiles in tile set
            int sizeToIncreaseBy = newPixelsPerTile * totalTiles;

            int newDimensions = _imageWidth + sizeToIncreaseBy / 2;
            Bitmap expandedBitMap = new Bitmap(newDimensions, newDimensions);
            SaveBitMapImage();
        }
        private void SaveBitMapImage()
        {
            string path = $"{System.AppDomain.CurrentDomain.BaseDirectory}ImageOutput/{_fileNameSafe}";

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(_bitMapImage));
            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

    }
}
