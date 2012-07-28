using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;
using VidFilter.Engine;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Configuration;
using VidFilter.Repository.Model;

namespace VidFilter
{
    public class MainWindowModel : DependencyObject, INotifyPropertyChanged
    {
        public ObservableCollection<FriendlyName> Movies { get; set; }

        public ObservableCollection<string> Colorspaces { get; set; }

        private DenormalizedMovie selected;
        public DenormalizedMovie Selected 
        { 
            get
            {
                return selected;
            }
            set
            {
                if (value != selected)
                {
                    selected = value;
                    NotifyPropertyChanged("Selected");
                }
            }
        }

        public StringBuilder debugInformation;
        public string DebugInformation
        {
            get
            {
                return debugInformation.ToString();
            }
        }

        public void AddDebugInfo(string info = "")
        {
            debugInformation.Append(info + "\r\n");
            NotifyPropertyChanged("DebugInformation");
        }

        public void AddDebugInfo(string message, Exception ex)
        {
            if (ex == null) { return; }
            var messageList = new[]{
                message,
                ex.Message,
                String.Empty,
                "Stack Trace:",
                ex.StackTrace
            };
            AddDebugInfo(string.Join("\r\n", messageList));
        }

        private ObservableCollection<string> debugMessages;
        private int debugCapacity;
        public ObservableCollection<string> DebugMessages
        {
            get
            {
                return debugMessages;
            }
        }
        public void AddDebugMessage(string message = null)
        {
            if (debugMessages.Count > debugCapacity - 1)
            {
                debugMessages.RemoveAt(0);
            }
            debugMessages.Add(message);
        }
        public void AddDebugMessage(string message, Exception ex)
        {
            AddDebugMessage(message);
            AddDebugMessage(ex.Message);
            AddDebugMessage(ex.StackTrace);
        }

        public bool IsDebug;

        public MainWindowModel(int messageCapacity)
        {
            Movies = new ObservableCollection<FriendlyName>();
            Colorspaces = new ObservableCollection<string>();
            debugInformation = new StringBuilder();
            debugMessages = new ObservableCollection<string>();
            bool.TryParse(ConfigurationManager.AppSettings["Debug"], out IsDebug);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public void RefreshFromDatabase()
        {
            Movies.Clear();
            foreach (FriendlyName movie in App.Database.QueryAllMovies(allowException: IsDebug))
            {
                Movies.Add(movie);
            }

            Colorspaces.Clear();
            foreach (string colorspace in App.Colorspaces.GetAllNames())
            {
                Colorspaces.Add(colorspace);
            }
        }
    }
}
