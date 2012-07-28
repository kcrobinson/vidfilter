using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using VidFilter.Engine;
using System.Configuration;
using VidFilter.Repository.Model;

namespace VidFilter
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : UserControl
    {
        MainWindowModel MainModel = MainWindow.MainModel;

        public AdminPanel()
        {
            InitializeComponent();
            this.DataContext = MainModel;
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            var denormalizedMovie = MainModel.Selected;
            Movie movie = new Movie(denormalizedMovie.FullPath);
            movie.FrameRate = denormalizedMovie.FrameRate;
            movie.PlayLength = denormalizedMovie.PlayLength;
            movie.ResolutionHeight = denormalizedMovie.ResolutionHeight;
            movie.ResolutionWidth = denormalizedMovie.ResolutionWidth;
            movie.ColorspaceName = denormalizedMovie.ColorspaceName;

            HelperMethods.InsertMovieToDatabase(movie);
            MainModel.RefreshFromDatabase();
        }

        private void LoadMovieRowButton_Click(object sender, RoutedEventArgs e)
        {
            LoadMovie(LoadMovieRowTextBox.Text);
            LoadMovieRowTextBox.Text = String.Empty;
        }

        private void LoadMovie(string filePath)
        {
            if (!File.Exists(filePath)) return;
            
            Engine.ProbeRequest request = new Engine.ProbeRequest();
            request.FilePath = filePath;
            Engine.ProbeResult result = App.Engine.ProbeVideoFile(request);
            
            if (!result.IsSuccess) return;

            FileInfo file = new FileInfo(filePath);
            MainModel.Selected = new DenormalizedMovie()
            {
                FileName = file.Name,
                Directory = file.DirectoryName,
                FrameRate = result.FrameRate,
                ResolutionWidth = result.ResolutionWidth,
                ResolutionHeight = result.ResolutionHeight,
                PlayLength = result.PlayLength,
                ColorspaceName = result.Colorspace
            };
        }

        private void MovieSelectorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void RemoveMovieButton_Click(object sender, RoutedEventArgs e)
        {
            var friendlyName = MovieSelectorListBox.SelectedItem as FriendlyName;
            if (friendlyName == null) return;
            App.Database.DeleteMovieAndImage(friendlyName.Id);
            MainModel.Selected = null;
            MainModel.RefreshFromDatabase();
        }
    }
}
