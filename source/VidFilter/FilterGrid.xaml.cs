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
using VidFilter.Engine;
using System.IO;
using System.Configuration;
using Model = VidFilter.Repository.Model;
using VidFilter.Repository.Model;

namespace VidFilter
{
    /// <summary>
    /// Interaction logic for FilterGrid.xaml
    /// </summary>
    public partial class FilterGrid : UserControl
    {
        MainWindowModel MainModel = MainWindow.MainModel;

        public FilterGrid()
        {
            InitializeComponent();
            this.DataContext = MainModel;
        }

        #region Event Handlers

        private void MovieSelectorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FriendlyName movieItem = MovieSelectorListBox.SelectedItem as FriendlyName;
            if (movieItem == null)
            {
                MainModel.Selected = null;
                return;
            }

            try
            {
                MainModel.Selected = App.Database.QueryMovie(movieItem.Id, allowException: MainModel.IsDebug);
            }
            catch (Exception ex)
            {
                MainModel.AddDebugInfo("Failure getting movie from database", ex);
                return;
            }
            if (MainModel.Selected == null)
            {
                return;
            }

            MovieDataGrid.Visibility = System.Windows.Visibility.Visible;
            MovieFileNameValue.Content = MainModel.Selected.FileName;
            MovieFullPathValue.Content = MainModel.Selected.Directory;
            MovieFrameRateValue.Content = MainModel.Selected.FormattedFramerate;
            MovieResolutionValue.Content = MainModel.Selected.FormattedResolution;
            MovieColorspaceValue.Content = MainModel.Selected.ColorspaceName;

            string fullPath = MainModel.Selected.Directory;
            if (!string.IsNullOrWhiteSpace(fullPath))
            {
                FileInfo fileInfo = new FileInfo(fullPath);
                MovieFullPathChange.Text = fileInfo.DirectoryName;
            }

            int frameRate = MainModel.Selected.FrameRate;
            if (frameRate > 0)
            {
                MovieFrameRateChangeSlider.Maximum = frameRate;
                MovieFrameRateChangeSlider.Minimum = 1;
                MovieFrameRateChangeSlider.SmallChange = 1;
                MovieFrameRateChangeSlider.Value = MovieFrameRateChangeSlider.Maximum;
            }

            int width = MainModel.Selected.ResolutionWidth;
            int height = MainModel.Selected.ResolutionHeight;
            if (width > 0 && height > 0)
            {
                int gcd = 1;
                for (int i = 2; i < Math.Min(width, height); i++)
                {
                    if (width % i == 0 && height % i == 0)
                    {
                        gcd = i;
                    }
                }
                MovieResolutionGCD.Content = gcd;
                MovieResolutionChangeSlider.Maximum = gcd;
                MovieResolutionChangeSlider.Minimum = 1;
                MovieResolutionChangeSlider.SmallChange = 1;
                MovieResolutionChangeSlider.Value = MovieResolutionChangeSlider.Maximum;
            }

            foreach (var item in MovieColorspaceChange.Items)
            {
                string colorspaceItem = item as string;
                if (String.Equals(MainModel.Selected.ColorspaceName, colorspaceItem, StringComparison.OrdinalIgnoreCase))
                {
                    MovieColorspaceChange.SelectedItem = item;
                    break;
                }
            }

            if (ImageWindow != null)
            {
                ImageWindow.SetImage(MainModel.Selected.SampleImagePath);
            }
            e.Handled = true;
        }

        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            FriendlyName movieItem = MovieSelectorListBox.SelectedItem as FriendlyName;
            if (movieItem == null)
            {
                MainModel.AddDebugInfo("Not a valid movie selction");
                return;
            }

            DenormalizedMovie movie = MainModel.Selected;
            if (movie == null)
            {
                MainModel.AddDebugInfo("No movie selected");
                return;
            }

            MainModel.AddDebugInfo("Processing movie at " + movie.FullPath);
            EngineRequest engineRequest = new EngineRequest()
            {
                InputFileName = movie.FileName,
                InputDirectory = movie.Directory,
                // InputFrameRate = movie.FrameRate,
                // InputHeight = movie.ResolutionHeight,
                // InputWidth = movie.ResolutionWidth,
                OutputFrameRate = IntTryParse(MovieFrameRateChangeText),
                OutputHeight = IntTryParse(MovieResolutionHeightChangeText),
                OutputWidth = IntTryParse(MovieResolutionWidthChangeText),
                // OutputPath = string.IsNullOrWhiteSpace()
                OutputCodec = "rawvideo",
                OutputColorspace = MovieColorspaceChange.SelectedItem != null ? MovieColorspaceChange.SelectedItem.ToString() : null,
            };
            EngineResult engineResult = App.Engine.ProcessRequest(engineRequest);
            if (!engineResult.IsSuccess)
            {
                if (engineResult.Exception != null)
                {
                    MainModel.AddDebugInfo("Exception thrown", engineResult.Exception);
                }
                else
                {
                    MainModel.AddDebugInfo("Failure processing movie");
                    MainModel.AddDebugInfo("Message: " + engineResult.Message);
                    MainModel.AddDebugInfo("Std Out: " + engineResult.StdOutput);
                    MainModel.AddDebugInfo("StdErr: " + engineResult.StdError);
                }
                return;
            }
            MainModel.AddDebugInfo("Process succeeded.");

            MainModel.AddDebugInfo("Probing process result");
            ProbeRequest probeRequest = new ProbeRequest()
            {
                FilePath = engineResult.OutFile.FullName
            };
            ProbeResult probeResult = App.Engine.ProbeVideoFile(probeRequest);
            if (!probeResult.IsSuccess)
            {
                MainModel.AddDebugInfo("Failure: " + probeResult.ErrorMessage);
                return;
            }
            Movie newMovie = new Movie(engineResult.OutFile);
            newMovie.FrameRate = probeResult.FrameRate;
            newMovie.ResolutionHeight = probeResult.ResolutionHeight;
            newMovie.ResolutionWidth = probeResult.ResolutionWidth;
            newMovie.ColorspaceName = probeResult.Colorspace;
            newMovie.BitRate = probeResult.BitRate;
            newMovie.PlayLength = probeResult.PlayLength;
            newMovie.ParentMovieId = movie.MovieId;

            ImageRequest imageRequest = new ImageRequest()
            {
                MovieLength = probeResult.PlayLength,
                MoviePath = newMovie.FullName
            };
            ImageResult imageResult = App.Engine.CreateImage(imageRequest);
            if (!imageResult.IsSuccess)
            {
                MainModel.AddDebugInfo(imageResult.ErrorMessage);
                return;
            }
            Model.Image image = new Model.Image(imageResult.OutFile)
            {
                //ColorSpaceId = 
                ResolutionHeight = probeResult.ResolutionHeight,
                ResolutionWidth = probeResult.ResolutionWidth
            };
            MainModel.AddDebugInfo();
            InsertImageToDatabase(image);
            newMovie.SampleFrameId = image.Id;
            InsertMovieToDatabase(newMovie);
        }

        private int IntTryParse(TextBox textBox)
        {
            int value = 0;
            int.TryParse(textBox.Text, out value);
            return value;
        }

        private void MovieFrameRateChangeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)MovieFrameRateChangeSlider.Value;
            MovieFrameRateChangeText.Text = value.ToString();
        }

        private void MovieFrameRateChangeText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int origValue = (int)MovieFrameRateChangeSlider.Value;
            int newValue = 0;
            int.TryParse(MovieFrameRateChangeText.Text, out newValue);

            if (newValue < MovieFrameRateChangeSlider.Minimum || newValue > MovieFrameRateChangeSlider.Maximum)
            {
                MovieFrameRateChangeText.Text = origValue.ToString();
            }
            else
            {
                MovieFrameRateChangeSlider.Value = newValue;
            }
        }

        private void MovieResolutionChangeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)MovieResolutionChangeSlider.Value;
            int gcd = (int)MovieResolutionGCD.Content;
            MovieResolutionWidthChangeText.Text = (MainModel.Selected.ResolutionWidth * value / gcd).ToString();
            MovieResolutionHeightChangeText.Text = (MainModel.Selected.ResolutionHeight * value / gcd).ToString();
        }

        private void MovieResolutionWidthChangeText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int newValue = 0;
            int.TryParse(MovieResolutionWidthChangeText.Text, out newValue);
            if (newValue != 0)
            {
                int gcd = (int)MovieResolutionGCD.Content;
                double estimate = newValue * gcd / MainModel.Selected.ResolutionWidth;
                MovieResolutionChangeSlider.Value = Math.Round(estimate);
            }
            else
            {
                MovieResolutionChangeSlider_ValueChanged(null, null);
            }
        }

        private void MovieResolutionHeightChangeText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int newValue = 0;
            int.TryParse(MovieResolutionHeightChangeText.Text, out newValue);
            if (newValue != 0)
            {
                int gcd = (int)MovieResolutionGCD.Content;
                double estimate = newValue * gcd / MainModel.Selected.ResolutionHeight;
                MovieResolutionChangeSlider.Value = Math.Round(estimate);
            }
            else
            {
                MovieResolutionChangeSlider_ValueChanged(null, null);
            }
        }

        #endregion

        private void InsertMovieToDatabase(Movie movie)
        {
            MainModel.AddDebugInfo("Inserting movie in database");
            OperationStatus status = App.Database.InsertMovie(movie);
            if (!status.IsSuccess)
            {
                if (status.Exception != null)
                {
                    MainModel.AddDebugInfo("Exception thrown while inserting movie", status.Exception);
                }
                else
                {
                    MainModel.AddDebugInfo("Failure inserting movie");
                    MainModel.AddDebugInfo("Message: " + status.Message);
                }
                return;
            }
            MainModel.AddDebugInfo("Success");
            MainModel.Movies.Add(status.ResultingFriendlyName);
        }

        private void InsertImageToDatabase(Model.Image image)
        {
            MainModel.AddDebugInfo("Inserting image in database");
            OperationStatus status = App.Database.InsertImage(image);
            if (!status.IsSuccess)
            {
                if (status.Exception != null)
                {
                    MainModel.AddDebugInfo("Exception thrown while inserting image", status.Exception);
                }
                else
                {
                    MainModel.AddDebugInfo("Failure inserting image");
                    MainModel.AddDebugInfo("Message: " + status.Message);
                }
                return;
            }
            MainModel.AddDebugInfo("Success");
        }

        private void ValueTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (sender != null)
            {
                textBox.SelectAll();
            }
        }

        private void ViewImageButton_Click(object sender, RoutedEventArgs e)
        {
            showImageWindow = !showImageWindow;
            if (showImageWindow)
            {
                if (MainModel.Selected != null)
                {
                    ImageWindow.SetImage(MainModel.Selected.SampleImagePath);
                    ImageWindow.Show();
                }
                else
                {
                    showImageWindow = false;
                }
            }
            else
            {
                DisposeImageWindow();
            }
        }

        private ImageWindow _ImageWindow;
        public ImageWindow ImageWindow
        {
            get
            {
                if (_ImageWindow == null && showImageWindow)
                {
                    _ImageWindow = new ImageWindow();
                    _ImageWindow.Closed += OnImageWindowClose;
                }
                return _ImageWindow;
            }
        }

        private bool showImageWindow = false;

        private void DisposeImageWindow()
        {
            if (_ImageWindow != null)
            {
                ImageWindow.Close();
                _ImageWindow = null;
            }
            this.showImageWindow = false;
        }

        private void OnImageWindowClose(object sender, EventArgs e)
        {
            this.DisposeImageWindow();
        }
    }
}
