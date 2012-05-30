using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using VidFilter.Engine;

namespace VidFilter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly IDatabase Database = DatabaseFactory.GetDatabase();
        public static readonly IEngine Engine = EngineFactory.GetEngine();
    }
}
