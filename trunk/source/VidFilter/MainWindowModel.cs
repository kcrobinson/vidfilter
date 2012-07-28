using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
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

        public string DebugInformation
        {
            get
            {
                return String.Join("\r\n", DebugMessages);
            }
        }

        private int debugCapacity;
        public ObservableCollection<string> DebugMessages { get; set; }
        public void AddDebugMessage(string message = null)
        {
            if (DebugMessages.Count > debugCapacity - 1)
            {
                DebugMessages.RemoveAt(0);
            }
            DebugMessages.Add(message);
            NotifyPropertyChanged("DebugInformation");
        }
        public void AddDebugMessage(string message, Exception ex)
        {
            AddDebugMessage(message);
            AddDebugMessage(ex.Message);
            AddDebugMessage(ex.StackTrace);
        }

        public bool IsDebug;

        public MainWindowModel()
        {
            Movies = new ObservableCollection<FriendlyName>();
            Colorspaces = new ObservableCollection<string>();
            DebugMessages = new ObservableCollection<string>();
            
            bool.TryParse(ConfigurationManager.AppSettings["Debug"], out IsDebug);
            int.TryParse(ConfigurationManager.AppSettings["DebugMessageLimit"], out debugCapacity);
            if (debugCapacity < 1)
            {
                debugCapacity = 1;
                AddDebugMessage("Failure parsing positive value for DebugMessageLimit from configuration");
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

        public void RefreshFromDatabase()
        {
            this.AddDebugMessage("Refreshing movies");
            Movies.Clear();
            foreach (FriendlyName movie in App.Database.QueryAllMovies(allowException: IsDebug))
            {
                Movies.Add(movie);
            }

            this.AddDebugMessage("Refreshing colorspaces");
            Colorspaces.Clear();
            foreach (string colorspace in App.Colorspaces.GetAllNames())
            {
                Colorspaces.Add(colorspace);
            }
            this.AddDebugMessage("Refresh complete");
        }
    }
}
