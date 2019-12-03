using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Diary.Framework.Help
{
    public static class ImageHelper
    {
        public static ImageSource OpenImageDialogRetImage()
        {
            var openDialog = new OpenFileDialog();

            openDialog.Filter = "Image files (*.JPG,*.PNG)|*.jpg;*.png";

            if (openDialog.ShowDialog() == true)
                return new BitmapImage(new Uri(openDialog.FileName));
            return null;
        }

        public static byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            try
            {
                if (imageSource != null)
                {
                    var encoder = FileNameToEncoder(imageSource.ToString());
                    byte[] bytes = null;
                    var bitmapSource = imageSource as BitmapSource;

                    if (bitmapSource != null)
                    {
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                        using (var stream = new MemoryStream())
                        {
                            encoder.Save(stream);
                            bytes = stream.ToArray();
                        }
                    }

                    return bytes;
                }
            }catch(Exception ex) { }

            return null;
        }


        private static BitmapEncoder FileNameToEncoder(string fileName)
        {
            BitmapEncoder encoder = null;

            var ex = Path.GetExtension(fileName);
            switch (ex)
            {
                case ".jpg":
                    encoder = new JpegBitmapEncoder();
                    break;

                case ".png":
                    encoder = new PngBitmapEncoder();
                    break;
                default:
                    encoder = new JpegBitmapEncoder();
                    break;
            }

            return encoder;
        }

        public static ImageSource ByteToImage(byte[] imageData)
        {
            if (imageData!=null && imageData.Length != 0)
            {
                var biImg = new BitmapImage();
                var ms = new MemoryStream(imageData);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                var imgSrc = biImg as ImageSource;

                return imgSrc;
            }

            return null;
        }
    }
}