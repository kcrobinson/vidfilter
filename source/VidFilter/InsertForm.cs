using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            ProbeRequest request = new ProbeRequest()
            {
                FilePath = FilePathTextBox.Text
            };
            ProbeResult result = App.Engine.ProbeVideoFile(request);

            FrameRateTextBox.Text = result.FrameRate > 0 ? result.FrameRate.ToString() : string.Empty;
            ResolutionWidthTextBox.Text = result.ResolutionWidth > 0 ? result.ResolutionWidth.ToString() : string.Empty;
            ResolutionHeightTextBox.Text = result.ResolutionHeight > 0 ? result.ResolutionHeight.ToString() : string.Empty;
            MovieColorspaceComboBox.SelectedText = result.Colorspace;
        }

        private void InsertMovieButton_Click(object sender, EventArgs e)
        {
            Movie movie = new Movie(FilePathTextBox.Text);
            movie.FrameRate = IntTryParse(FrameRateTextBox);
            movie.ResolutionWidth = IntTryParse(ResolutionWidthTextBox);
            movie.ResolutionHeight = IntTryParse(ResolutionHeightTextBox);
            Colorspace colorspace = MovieColorspaceComboBox.SelectedItem as Colorspace;
            if (colorspace != null)
            {
                movie.ColorSpaceId = colorspace.Id;
            }

            OperationStatus status = App.Database.InsertMovie(movie);
            if (status.IsSuccess)
            {
                InsertStatusTextBox.Text = "Success adding movie";
                ClearAllFields();
            }
            else
            {
                InsertStatusTextBox.Text = status.Message;
                if (status.Exception != null)
                {
                    InsertStatusTextBox.Text += "\r\n" + status.Exception.Message;
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
                InsertStatusTextBox.Text = "Success adding colorspace";
                ClearAllFields();
            }
            else
            {
                InsertStatusTextBox.Text = status.Message;
                if (status.Exception != null)
                {
                    InsertStatusTextBox.Text += "\r\n" + status.Exception.Message;
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
    }
}
