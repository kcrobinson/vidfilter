using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
            if (denormalizedMovie == null)
            {
                MainModel.AddDebugMessage("No movie selected");
                return;
            }
            MainModel.AddDebugMessage("Inserting movie to database");

            Movie movie = new Movie(denormalizedMovie.FullPath);
            movie.FrameRate = denormalizedMovie.FrameRate;
            movie.PlayLength = denormalizedMovie.PlayLength;
            movie.ResolutionHeight = denormalizedMovie.ResolutionHeight;
            movie.ResolutionWidth = denormalizedMovie.ResolutionWidth;
            movie.ColorspaceName = denormalizedMovie.ColorspaceName;

            string result = HelperMethods.InsertMovieToDatabase(movie);
            MainModel.AddDebugMessage(result);

            MainModel.RefreshFromDatabase();
            MainModel.AddDebugMessage("Insert Finished");
        }

        private void LoadMovieRowButton_Click(object sender, RoutedEventArgs e)
        {
            MainModel.AddDebugMessage("Loading movie");
            bool success = LoadMovie(LoadMovieRowTextBox.Text);

            if (success)
            {
                LoadMovieRowTextBox.Text = String.Empty;
            }
            MainModel.AddDebugMessage("Load finished");
        }

        private bool LoadMovie(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MainModel.AddDebugMessage("File does not exist");
                return false;
            }

            MainModel.AddDebugMessage("Probing file for details");
            Engine.ProbeRequest request = new Engine.ProbeRequest();
            request.FilePath = filePath;
            Engine.ProbeResult result = App.Engine.ProbeVideoFile(request);

            if (!result.IsSuccess)
            {
                MainModel.AddDebugMessage("Failure probing file");
                MainModel.AddDebugMessage(result.ErrorMessage);
                return false;
            }

            MainModel.AddDebugMessage("Loading details");
            FileInfo file = new FileInfo(filePath);
            MainModel.Selected = new DenormalizedMovie()
            {
                FileName = file.Name,
                Directory = file.DirectoryName,
                FrameRate = result.FrameRate,
                ResolutionWidth = result.ResolutionWidth,
                ResolutionHeight = result.ResolutionHeight,
                PlayLength = result.PlayLength,
                ColorspaceName = App.Colorspaces.GetNameFromCodeName(result.ColorspaceCodeName)
            };
            return true;
        }

        private void MovieSelectorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void RemoveMovieButton_Click(object sender, RoutedEventArgs e)
        {
            var friendlyName = MovieSelectorListBox.SelectedItem as FriendlyName;
            if (friendlyName == null)
            {
                MainModel.AddDebugMessage("No movie selected");
                return;
            };

            MainModel.AddDebugMessage("Removing movie from database: " + friendlyName.Name);
            
            OperationStatus status = App.Database.DeleteMovieAndImage(friendlyName.Id);
            if (!status.IsSuccess)
            {
                if (status.Exception != null)
                {
                    MainModel.AddDebugMessage("Exception thrown while removing records", status.Exception);
                }
                else
                {
                    MainModel.AddDebugMessage("Failure removing records");
                    MainModel.AddDebugMessage(status.Message);
                }
                return;
            }
            
            MainModel.Selected = null;
            MainModel.RefreshFromDatabase();
            MainModel.AddDebugMessage("Removal finished");
        }
    }
}
