using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImagePreviewSQL
{
    public class ImagePreview
    {
        private BitmapImage bitmap;
        private byte[] byteArray; 

        public BitmapImage Bitmap
        {
            get { return bitmap; }
        }
        public byte[] ByteArray
        {
            get { return byteArray; }
        }

        public ImagePreview(byte[] byteArray)
        {
            this.byteArray = byteArray;
            GetBitmapFromByteArray(byteArray);
        }
        public ImagePreview(string path)
        {
            long length = new FileInfo(path).Length;
            byteArray = new byte[length];

            try
            {
                FileStream fileStream = File.OpenRead(path);
                fileStream.Read(byteArray);
                fileStream.Close();

                GetBitmapFromByteArray(byteArray);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        // Converts source byte array to bitmap
        private void GetBitmapFromByteArray(byte[] byteArray)
        {
            Stream stream = new MemoryStream(byteArray);
            bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
        }
    }
}
