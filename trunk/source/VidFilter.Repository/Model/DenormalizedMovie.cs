using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VidFilter.Repository.Model
{
    public class DenormalizedMovie : INotifyPropertyChanged
    {
        public string MovieId { get; set; }

        private string _FileName;
        public string FileName 
        { 
            get
            {
                return _FileName;
            }
            set
            {
                if (value != _FileName)
                {
                    _FileName = value;
                    NotifyPropertyChanged("FileName");
                    NotifyPropertyChanged("FullPath");
                }
            }
        }

        private string _Directory;
        public string Directory
        {
            get
            {
                return _Directory;
            }
            set
            {
                if (value != _Directory)
                {
                    _Directory = value;
                    NotifyPropertyChanged("Directory");
                    NotifyPropertyChanged("FullPath");
                }
            }
        }

        private int _FrameRate;
        public int FrameRate
        {
            get
            {
                return _FrameRate;
            }
            set
            {
                if (value != _FrameRate)
                {
                    _FrameRate = value;
                    NotifyPropertyChanged("FrameRate");
                    NotifyPropertyChanged("FormattedFramerate");
                }
            }
        }

        private int _ResolutionWidth;
        public int ResolutionWidth
        {
            get
            {
                return _ResolutionWidth;
            }
            set
            {
                if (value != _ResolutionWidth)
                {
                    _ResolutionWidth = value;
                    NotifyPropertyChanged("ResolutionWidth");
                    NotifyPropertyChanged("FormattedResolution");
                }
            }
        }

        private int _ResolutionHeight;
        public int ResolutionHeight
        {
            get
            {
                return _ResolutionHeight;
            }
            set
            {
                if (value != _ResolutionHeight)
                {
                    _ResolutionHeight = value;
                    NotifyPropertyChanged("ResolutionHeight");
                    NotifyPropertyChanged("FormattedResolution");
                }
            }
        }

        public string ColorspaceName { get; set; }

        private decimal _PlayLength;
        public decimal PlayLength
        {
            get
            {
                return _PlayLength;
            }
            set
            {
                if (value != _PlayLength)
                {
                    _PlayLength = value;
                    NotifyPropertyChanged("PlayLength");
                    NotifyPropertyChanged("FormattedPlayLength");
                }
            }
        }

        public string SampleImagePath { get; set; }

        public string FormattedResolution
        {
            get
            {
                return ResolutionWidth + "x" + ResolutionHeight;
            }
        }

        public string FormattedFramerate
        {
            get
            {
                return FrameRate + " FPS";
            }
        }

        public string FormattedPlayLength
        {
            get
            {
                int seconds = (int)PlayLength;
                int milliseconds = (int)((PlayLength - seconds) * 1000);
                TimeSpan timeSpan = new TimeSpan(0, 0, 0, seconds, milliseconds);
                return timeSpan.ToString(@"hh\:mm\:ss\.ff");
            }
        }

        public string FullPath
        {
            get
            {
                return String.Join("\\", Directory, FileName);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
