using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImagePreviewSQL
{
    public class ImageManager
    {
        private ImageDatabase database;

        private List<ImageStruct> imageMainList = new List<ImageStruct>();
        private List<ImageStruct> imageList = new List<ImageStruct>();
        private int imageIndex;

        public List<ImageStruct> ImageList
        {
            get { return imageList; }
        }
        public BitmapImage BitmapImage
        {
            get { return imageList[imageIndex].Image.Bitmap; }
        }
        public string Name
        {
            get 
            {
                return imageList[imageIndex].Name; 
            }
        }
        public bool ItemsLeft
        {
            get
            {
                if (imageMainList.Count == 0)
                    return false;
                return true;
            }
        }

        public ImageManager(string connectionString)
        {
            database = new ImageDatabase(connectionString);
            database.ReadAll();
            UpdateMain();
        }

        /// <summary>
        /// Used to add image from client to database
        /// </summary>
        public string AddImage(string path)
        {
            // Creates image
            ImagePreview image = null;
            try
            {
                image = new ImagePreview(path);
            }
            catch(Exception exception)
            {
                return exception.Message;
            }

            // Gets name from file path
            string name = Path.GetFileName(path);

            // Adds image to database
            database.AddImage(name, image.ByteArray);

            // Read all from database
            database.ReadAll();

            // Adds image to main list
            int index = database.Rows-1;
            imageMainList.Add(new ImageStruct(database.IdArray[index], database.NameArray[index], image));

            UpdateList();
            return "Image " + name + " was added to database.";
        }

        /// <summary>
        /// Used to delete image from database
        /// </summary>
        public void DeleteImage(int index)
        {
            // Delete image from database
            database.DeleteImage(imageList[index].Id);

            // Removes image from main list by index
            imageMainList.RemoveAt(index);

            // Updates list
            UpdateList();
        }

        /// <summary>
        /// Used to delete all images from database
        /// </summary>
        public void DeleteAllImages()
        {
            // Delete all images from database
            for (int i = 0; i < imageMainList.Count; i++)
                database.DeleteImage(imageList[i].Id);

            // Clears main list
            imageMainList.Clear();

            // Updates list
            UpdateList();
        }

        /// <summary>
        /// Used to save image to client
        /// </summary>
        public void SaveImage(string path)
        {
            FileStream outFile = File.Create(path);

            // Gets byte array from image
            byte[] byteArray = imageList[imageIndex].Image.ByteArray;

            // Writes all data to file
            outFile.Write(byteArray, 0, byteArray.Length);

            outFile.Close();
        }

        /// <summary>
        /// Used to search image
        /// </summary>
        public void SearchImage(string search)
        {
            // Checks if search is invalid
            if (string.IsNullOrEmpty(search))
                UpdateList();

            // Loops through list and checks if contains search
            imageList.Clear();
            for (int i = 0; i < imageMainList.Count; i++)
                if (imageMainList[i].Name.Contains(search))
                    imageList.Add(imageMainList[i]);
        }

        /// <summary>
        /// Used to update list index
        /// </summary>
        public void UpdateIndex(int index)
        {
            imageIndex = index;
        }

        // To update reload all images into main list
        private void UpdateMain()
        {
            // Clears list
            imageMainList.Clear();

            // Loops through database lists
            for (int i = 0; i < database.Rows; i++)
            {
                ImagePreview image = new ImagePreview(database.BinaryList[i]);
                imageMainList.Add(new ImageStruct(database.IdArray[i], database.NameArray[i], image));
            }
            UpdateList();
        }

        // Used to update list
        private void UpdateList()
        {
            imageList.Clear();
            for (int i = 0; i < imageMainList.Count; i++)
                imageList.Add(imageMainList[i]);
        }
    }
}
