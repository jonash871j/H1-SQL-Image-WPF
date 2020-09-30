using System.Windows;
using System.Windows.Controls;
using System.IO;
using ImagePreviewSQL;
using Microsoft.Win32;
using System.Reflection;
using System;

namespace SQL_Image_WPF
{
    public partial class MainWindow : Window
    {
        private ImageManager manager;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                manager = new ImageManager("Data Source=(local); Initial Catalog=PictureBase; Integrated Security=SSPI");
                lib_imageMenu.SelectedIndex = 0;
                tb_imageExploreSearch_TextChanged(null, null);
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        /* Menubar */
        private void mi_exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void mi_about_Click(object sender, RoutedEventArgs e)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MessageBox.Show("By Tobias & Jonas\n01-05-2020\nVersion " + version);
        }

        /* Image menu section */
        private void tb_imageExploreSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            manager.SearchImage(tb_imageExploreSearch.Text);

            lib_imageMenu.Items.Clear();
            for (int i = 0; i < manager.ImageList.Count; i++)
            {
                lib_imageMenu.Items.Add(manager.ImageList[i].Name);
            }
        }
        private void lib_imageMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lib_imageMenu.SelectedIndex == -1)
            {
                img_view.Source = null;
                tb_statusBarImageName.Text = "";
                return;
            }

            manager.UpdateIndex(lib_imageMenu.SelectedIndex);
            img_view.Source = manager.BitmapImage;
            tb_statusBarImageName.Text = manager.Name;
        }
        private void imageMenuAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "All image files (*.*)|*.*";
            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName))
                return;

            try
            {
                tb_statusBarMessage.Text = manager.AddImage(fileDialog.FileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            tb_imageExploreSearch_TextChanged(null, null);
        }
        private void imageMenuGetImage_Click(object sender, RoutedEventArgs e)
        {
            if (!manager.ItemsLeft)
            {
                tb_statusBarMessage.Text = "No images left in data base.";
                return;
            }

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = Path.GetFileNameWithoutExtension(manager.Name);
            fileDialog.Filter = "Data Files (*" + Path.GetExtension(manager.Name) + ")|*" + Path.GetExtension(manager.Name);

            fileDialog.ShowDialog();
            
            if (string.IsNullOrEmpty(fileDialog.FileName))
                return;

            try
            {
                manager.SaveImage(fileDialog.FileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void imageMenuDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            if (lib_imageMenu.SelectedIndex == -1)
                return;

            tb_statusBarMessage.Text = "Image " + manager.Name + " was removed from database.";

            try
            {
                manager.DeleteImage(lib_imageMenu.SelectedIndex);
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            tb_imageExploreSearch_TextChanged(null, null);
        }
        private void imageMenuDeleteAllImages_Click(object sender, RoutedEventArgs e)
        {
            tb_statusBarMessage.Text = "All images was removed";

            try
            {
                manager.DeleteAllImages();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            tb_imageExploreSearch_TextChanged(null, null);
        }
    }
}