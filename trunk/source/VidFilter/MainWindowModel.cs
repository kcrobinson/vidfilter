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

namespace VidFilter
{
    public class MainWindowModel : DependencyObject
    {
        public ObservableCollection<FriendlyName> Movies { get; set; }

        public ObservableCollection<Colorspace> Colorspaces { get; set; }

        public DenormalizedMovie Selected { get; set; }

        public MainWindowModel()
        {
            Movies = new ObservableCollection<FriendlyName>();
            Colorspaces = new ObservableCollection<Colorspace>();
        }
    }
}
