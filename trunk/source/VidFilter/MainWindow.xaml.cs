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
using System.Windows.Shapes;
using VidFilter.Engine;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace VidFilter
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowModel _Model;
        public MainWindowModel MainModel
        {
            get
            {
                if(_Model == null) 
                    _Model = this.Resources["model"] as MainWindowModel;
                return _Model;
            }
        }
        
        private readonly bool Debug = true;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                RefreshModel();
            }
            catch (Exception ex)
            {
                AddProcessMessageFromException("Exception thrown while initializing data from database", ex);
            }
        }

        #region Event Handlers

        private void MovieSelectorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FriendlyName movieItem = MovieSelectorListBox.SelectedItem as FriendlyName;
            if (movieItem == null)
            {
                return;
            }

            try
            {
                MainModel.Selected = App.Database.QueryMovie(movieItem.Id, allowException: Debug);
            }
            catch (Exception ex)
            {
                AddProcessMessageFromException("Failure getting movie from database", ex);
                return;
            }
            if (MainModel.Selected == null)
            {
                return;
            }

            MovieDataGrid.Visibility = System.Windows.Visibility.Visible;
            MovieFileNameValue.Content = MainModel.Selected.Movie.FileName;
            MovieFullPathValue.Content = MainModel.Selected.Movie.FullName;
            MovieFrameRateValue.Content = MainModel.Selected.Movie.FormattedFramerate;
            MovieResolutionValue.Content = MainModel.Selected.Movie.FormattedResolution;
            MovieColorspaceValue.Content = MainModel.Selected.Colorspace;

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(MainModel.Selected.Image.FullName, UriKind.Absolute);
            src.EndInit();
            SampleFrameImage.Source = src;
            SampleFrameImage.Stretch = Stretch.None;

            string fullPath = MainModel.Selected.Movie.DirectoryPath;
            if (!string.IsNullOrWhiteSpace(fullPath))
            {
                FileInfo fileInfo = new FileInfo(fullPath);
                MovieFullPathChange.Text = fileInfo.DirectoryName;
            }

            int frameRate = MainModel.Selected.Movie.FrameRate;
            if (frameRate > 0)
            {
                MovieFrameRateChangeSlider.Maximum = frameRate;
                MovieFrameRateChangeSlider.Minimum = 1;
                MovieFrameRateChangeSlider.SmallChange = 1;
                MovieFrameRateChangeSlider.Value = MovieFrameRateChangeSlider.Maximum;
            }

            int width = MainModel.Selected.Movie.ResolutionWidth;
            int height = MainModel.Selected.Movie.ResolutionHeight;
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
        }

        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            FriendlyName movieItem = MovieSelectorListBox.SelectedItem as FriendlyName;
            if (movieItem == null)
            {
                AddProcessMessageLine("Not a valid movie selction");
                return;
            }

            Movie movie = MainModel.Selected != null ? MainModel.Selected.Movie : null;
            if (movie == null)
            {
                AddProcessMessageLine("No movie selected");
                return;
            }

            AddProcessMessageLine("Processing movie at " + movie.FullName);
            EngineRequest engineRequest = new EngineRequest()
            {
                InputFile = movie.FileInfo,
                // InputFrameRate = movie.FrameRate,
                // InputHeight = movie.ResolutionHeight,
                // InputWidth = movie.ResolutionWidth,
                OutputFrameRate = IntTryParse(MovieFrameRateChangeText),
                OutputHeight = IntTryParse(MovieResolutionHeightChangeText),
                OutputWidth = IntTryParse(MovieResolutionWidthChangeText),
                // OutputPath = string.IsNullOrWhiteSpace()
                OutputCodec = "rawvideo",
                // OutputColorspace = MovieColorspaceChange.SelectedItem.ToString(),
            };
            EngineResult engineResult = App.Engine.ProcessRequest(engineRequest);
            if (!engineResult.IsSuccess)
            {
                if (engineResult.Exception != null)
                {
                    AddProcessMessageFromException("Exception thrown", engineResult.Exception);
                }
                else
                {
                    AddProcessMessageLine("Failure processing movie");
                    AddProcessMessageLine("Message: " + engineResult.Message);
                    AddProcessMessageLine("Std Out: " + engineResult.StdOutput);
                    AddProcessMessageLine("StdErr: " + engineResult.StdError);
                }
                return;
            }
            AddProcessMessageLine("Process succeeded.");

            AddProcessMessageLine("Probing process result");
            ProbeRequest probeRequest = new ProbeRequest()
            {
                FilePath = engineResult.OutFile.FullName
            };
            ProbeResult probeResult = App.Engine.ProbeVideoFile(probeRequest);
            if (!probeResult.IsSuccess)
            {
                AddProcessMessageLine("Failure: " + probeResult.ErrorMessage);
                return;
            }
            Movie newMovie = new Movie(engineResult.OutFile);
            newMovie.FrameRate = probeResult.FrameRate;
            newMovie.ResolutionHeight = probeResult.ResolutionHeight;
            newMovie.ResolutionWidth = probeResult.ResolutionWidth;
            // newMovie.ColorSpaceId = probeResult.Colorspace;
            newMovie.BitRate = probeResult.BitRate;
            newMovie.PlayLength = probeResult.PlayLength;
            newMovie.ParentMovieId = movie.Id;

            ImageRequest imageRequest = new ImageRequest()
            {
                MovieLength = probeResult.PlayLength,
                MoviePath = newMovie.FullName
            };
            ImageResult imageResult = App.Engine.CreateImage(imageRequest);
            if (!imageResult.IsSuccess)
            {
                AddProcessMessageLine(imageResult.ErrorMessage);
                return;
            }
            Engine.Image image = new Engine.Image(imageResult.OutFile)
            {
                //ColorSpaceId = 
                ResolutionHeight = probeResult.ResolutionHeight,
                ResolutionWidth = probeResult.ResolutionWidth
            };
            AddProcessMessageLine();
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

        private void InsertRecordButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            InsertForm insertForm = new InsertForm();
            insertForm.Disposed += new EventHandler(InsertForm_Disposed);
            insertForm.Show();
        }

        private void InsertForm_Disposed(object sender, EventArgs e)
        {
            RefreshModel();
            this.IsEnabled = true;
        }

        private void RemoveRecordButton_Click(object sender, RoutedEventArgs e)
        {

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
            MovieResolutionWidthChangeText.Text = (MainModel.Selected.Movie.ResolutionWidth * value / gcd).ToString();
            MovieResolutionHeightChangeText.Text = (MainModel.Selected.Movie.ResolutionHeight * value / gcd).ToString();
        }

        private void MovieResolutionWidthChangeText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int newValue = 0;
            int.TryParse(MovieResolutionWidthChangeText.Text, out newValue);
            if (newValue != 0)
            {
                int gcd = (int)MovieResolutionGCD.Content;
                double estimate = newValue * gcd / MainModel.Selected.Movie.ResolutionWidth;
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
                double estimate = newValue * gcd / MainModel.Selected.Movie.ResolutionHeight;
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
            AddProcessMessageLine("Inserting movie in database");
            OperationStatus status = App.Database.InsertMovie(movie);
            if (!status.IsSuccess)
            {
                if (status.Exception != null)
                {
                    AddProcessMessageFromException("Exception thrown while inserting movie", status.Exception);
                }
                else
                {
                    AddProcessMessageLine("Failure inserting movie");
                    AddProcessMessageLine("Message: " + status.Message);
                }
                return;
            }
            AddProcessMessageLine("Success");
            MainModel.Movies.Add(status.ResultingFriendlyName);
        }

        private void InsertImageToDatabase(VidFilter.Engine.Image image)
        {
            AddProcessMessageLine("Inserting image in database");
            OperationStatus status = App.Database.InsertImage(image);
            if (!status.IsSuccess)
            {
                if (status.Exception != null)
                {
                    AddProcessMessageFromException("Exception thrown while inserting image", status.Exception);
                }
                else
                {
                    AddProcessMessageLine("Failure inserting image");
                    AddProcessMessageLine("Message: " + status.Message);
                }
                return;
            }
            AddProcessMessageLine("Success");
        }

        private void AddProcessMessageLine(string text=null)
        {
            text = text ?? string.Empty;
            ProcessStatusTextBox.AppendText(text + "\r\n");
            ProcessStatusTextBox.ScrollToEnd();
        }

        private void AddProcessMessageFromException(string message, Exception ex)
        {
            if (ex == null) { return; }
            AddProcessMessageLine(message);
            AddProcessMessageLine(ex.Message);
            AddProcessMessageLine();
            AddProcessMessageLine("Stack Trace:");
            AddProcessMessageLine(ex.StackTrace);
        }

        private void RefreshModel()
        {
            MainModel.Movies.Clear();
            foreach (FriendlyName movie in App.Database.QueryAllMovies(allowException: Debug))
            {
                MainModel.Movies.Add(movie);
            }

            MainModel.Colorspaces.Clear();
            foreach (Colorspace colorspace in App.Database.QueryAllColorspaces(allowException: Debug))
            {
                MainModel.Colorspaces.Add(colorspace);
            }
        }

        private void ValueTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (sender != null)
            {
                textBox.SelectAll();
            }
        }

    }
}
