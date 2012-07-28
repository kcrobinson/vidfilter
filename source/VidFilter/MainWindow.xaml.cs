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
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainModel;

            try
            {
                MainModel.RefreshFromDatabase();
            }
            catch (Exception ex)
            {
                MainModel.AddDebugInfo("Exception thrown while initializing data from database", ex);
            }
        }

        static MainWindowModel _Model;
        public static MainWindowModel MainModel
        {
            get
            {
                if (_Model == null)
                    _Model = new MainWindowModel(App.DebugMessageLimit);
                return _Model;
            }
        }

        private void HomeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.Database != null)
            {
                App.Database.Dispose();
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainModel.Selected = null;
        }
    }
}
