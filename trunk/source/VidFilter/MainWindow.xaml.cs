using System;
using System.Windows;
using System.Windows.Controls;

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
                MainModel.AddDebugMessage("Exception thrown while initializing data from database", ex);
            }
        }

        static MainWindowModel _Model;
        public static MainWindowModel MainModel
        {
            get
            {
                if (_Model == null)
                    _Model = new MainWindowModel();
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
            if(e.OriginalSource.GetType() == typeof(TabControl))
                MainModel.Selected = null;
        }
    }
}
