using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
        private readonly int _defaultTizeSize = 16;

        private BitmapImage _bitMapImage;
        private string _fileNameSafe;

        private int _imageWidth;
        private int _imageHeight;


        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = $"{System.AppDomain.CurrentDomain.BaseDirectory}ExampleInput";
            dlg.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png|All Files (*.*)|*.*";
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


                ProcessImage.IsEnabled = true;
            }
        }

        private void ProcessImageButton_Click(object sender, RoutedEventArgs e)
        {
            Bitmap originalBitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(_bitMapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                originalBitmap = new Bitmap(bitmap);
            }

            _imageWidth = originalBitmap.Width;
            _imageHeight = originalBitmap.Height;

            int tileSize = 0;
            
            if(int.TryParse(TileSizeInput.Text, out tileSize))
            {

            }
            else
            {
                tileSize = _defaultTizeSize;
            }

            int totalTiles = _imageWidth / tileSize;


            //extra row/column per side of a square with the length of the tile
            int newPixelsPerTile = (int)Math.Sqrt(tileSize);

            //multiply by number of tiles in tile set
            int sizeToIncreaseBy = newPixelsPerTile * totalTiles;

            int newDimensions = _imageWidth + sizeToIncreaseBy / 2;


            Bitmap expandedBitMap = new Bitmap(newDimensions, newDimensions);


            int xOffset = 1;
            int yOffset = 1;
            Color color = Color.Transparent;

            for (int x = 0; x < newDimensions ; x++)
            {          
                    if (x % (tileSize + 0) == 0 || x % (tileSize + 1) == 0 || x == 1)
                        xOffset--;
                
                for (int y = 0; y < newDimensions; y++)
                {
  
                        if ((y + 0) % (tileSize + 0) == 0 || y % (tileSize + 1) == 0 || y == 1)
                            yOffset--;

                    color = originalBitmap.GetPixel(x + xOffset, y + yOffset);


                    expandedBitMap.SetPixel(x, y, color);


                }

                yOffset = 1;

            }



            BitmapImage alteredImage = BitmapHelper.ToBitmapImage(expandedBitMap);
            BitmapHelper.SaveBitMapImage(alteredImage, _fileNameSafe);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = BitmapHelper.OutPutFolder,
                UseShellExecute = true,
                Verb = "open"
            });
        }


    }
}
