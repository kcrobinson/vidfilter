using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VidFilter.Engine;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VidFilter
{
    public partial class InsertForm : Form
    {
        IEnumerable<Colorspace> AvailableColorspaces { get; set; }
        
        public InsertForm()
        {
            InitializeComponent();
            try
            {
                RefreshAvailableColorspaces();
            }
            catch { }
        }

        private void RefreshAvailableColorspaces()
        {
            AvailableColorspaces = App.Database.QueryAllColorspaces();
            MovieColorspaceComboBox.Items.Clear();
            foreach (Colorspace colorspace in AvailableColorspaces)
            {
                MovieColorspaceComboBox.Items.Add(colorspace);
            }
        }

        private void FileLoadButton_Click(object sender, EventArgs e)
        {
            StatusTextBox.Text = string.Empty;

            ProbeRequest request = new ProbeRequest()
            {
                FilePath = FilePathTextBox.Text
            };
            ProbeResult result = App.Engine.ProbeVideoFile(request);

            if (!result.IsSuccess)
            {
                StatusTextBox.Text = "Error loading file\r\n" + result.ErrorMessage;
                return;
            }

            FrameRateTextBox.Text = result.FrameRate.ToString();
            ResolutionWidthTextBox.Text = result.ResolutionWidth.ToString();
            ResolutionHeightTextBox.Text = result.ResolutionHeight.ToString();
            MovieColorspaceComboBox.SelectedText = result.Colorspace;
            PlayLengthTextBox.Text = result.PlayLength.ToString("F2");
            BitrateTextBox.Text = result.BitRate.ToString();
        }

        private void InsertMovieButton_Click(object sender, EventArgs e)
        {
            StatusTextBox.Text = string.Empty;

            Movie movie = new Movie(FilePathTextBox.Text);
            movie.FrameRate = IntTryParse(FrameRateTextBox);
            movie.ResolutionWidth = IntTryParse(ResolutionWidthTextBox);
            movie.ResolutionHeight = IntTryParse(ResolutionHeightTextBox);
            movie.BitRate = IntTryParse(BitrateTextBox);
            movie.PlayLength = DecimalTryParse(PlayLengthTextBox);
            Colorspace colorspace = MovieColorspaceComboBox.SelectedItem as Colorspace;
            if (colorspace != null)
            {
                movie.ColorSpaceId = colorspace.Id;
            }

            ImageRequest imageRequest = new ImageRequest()
            {
                MoviePath = movie.FullName,
                MovieLength = movie.PlayLength
            };
            ImageResult imageResult = App.Engine.CreateImage(imageRequest);
            if (!imageResult.IsSuccess)
            {
                StatusTextBox.Text += "Error creating image: " + imageResult.ErrorMessage;
            }
            Image image = new Image(imageResult.OutFile)
            {
                ColorSpaceId = movie.ColorSpaceId,
                ResolutionHeight = movie.ResolutionHeight,
                ResolutionWidth = movie.ResolutionWidth
            };
            OperationStatus status = App.Database.InsertImage(image);
            if (!status.IsSuccess)
            {
                StatusTextBox.Text += "Error inserting image to database: " + status.Message;
                return;
            }
            movie.SampleFrameId = image.Id;

            status = App.Database.InsertMovie(movie);
            if (status.IsSuccess)
            {
                StatusTextBox.Text = "Success adding movie";
                ClearAllFields();
            }
            else
            {
                StatusTextBox.Text = status.Message;
                if (status.Exception != null)
                {
                    StatusTextBox.Text += "\r\n" + status.Exception.Message;
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void ClearAllFields()
        {
            FilePathTextBox.Text = string.Empty;
            ResolutionHeightTextBox.Text = string.Empty;
            ResolutionWidthTextBox.Text = string.Empty;
            FrameRateTextBox.Text = string.Empty;
            MovieColorspaceComboBox.SelectedIndex = 0;
            BitrateTextBox.Text = string.Empty;
            PlayLengthTextBox.Text = string.Empty;
        }

        private void InsertColorpsaceButton_Click(object sender, EventArgs e)
        {
            Colorspace colorspace = new Colorspace();
            colorspace.Name = ColorspaceNameTextBox.Text;
            colorspace.CodeName = ColorspaceIdTextBox.Text;
            colorspace.NumChannels = IntTryParse(ColorspaceChannelCountTextBox);
            colorspace.BitsPerPixel = IntTryParse(ColorsapceBitsPerPixelTextBox);

            OperationStatus status = App.Database.InsertOrUpdateColorspace(colorspace);
            if (status.IsSuccess)
            {
                StatusTextBox.Text = "Success adding colorspace";
                ClearAllFields();
            }
            else
            {
                StatusTextBox.Text = status.Message;
                if (status.Exception != null)
                {
                    StatusTextBox.Text += "\r\n" + status.Exception.Message;
                }
            }
            RefreshAvailableColorspaces();
        }

        private int IntTryParse(TextBox textbox)
        {
            int value = 0;
            int.TryParse(textbox.Text, out value);
            return value;
        }

        private decimal DecimalTryParse(TextBox textbox)
        {
            decimal value = 0;
            decimal.TryParse(textbox.Text, out value);
            return value;
        }
    }
}
