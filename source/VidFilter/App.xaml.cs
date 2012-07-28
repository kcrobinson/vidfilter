using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using VidFilter.Repository;
using VidFilter.Engine;

namespace VidFilter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            string databaseType = ConfigurationManager.AppSettings["DatabaseType"];
            if (databaseType == null)
            {
                throw new Exception("Did not specify DatabaseType in configuration");
            }
            switch(databaseType.ToLower())
            {
                case "hosted":
                    Database = DatabaseFactory.GetDatabase(HostedDatabaseOptions);
                    break;
                case "embedded":
                    Database = DatabaseFactory.GetDatabase(EmbeddedDatabaseOptions);
                    break;
                default:
                    throw new Exception(String.Format("Unrecognized value for DatabaseType in configuration: '{0}'", databaseType));
            }

            var errorMessages = Colorspaces.Load(ColorspaceFilePath);
            if (errorMessages.Any())
            {
                throw new Exception("Error loading Colorspaces file\r\n" + String.Join("\r\n", errorMessages));
            }
        }

        private static readonly KeyValuePair<string, object> DatabaseTypeOption = 
            new KeyValuePair<string, object>("DatabaseType", ConfigurationManager.AppSettings["DatabaseType"]);
        private static readonly KeyValuePair<string, object> HostedConnectionPath = 
            new KeyValuePair<string, object>("ConnectionPath", ConfigurationManager.AppSettings["HostedConnectionPath"]);
        private static readonly KeyValuePair<string, object> EmbeddedConnectionPath = 
            new KeyValuePair<string, object>("ConnectionPath", ConfigurationManager.AppSettings["EmbeddedConnectionPath"]);
        
        private static readonly string ColorspaceFilePath = ConfigurationManager.AppSettings["ColorspaceFilePath"];
        public static readonly int DebugMessageLimit = ParseInt(ConfigurationManager.AppSettings["DebugMessageLimit"]);

        public static int ParseInt(string str)
        {
            int i;
            int.TryParse(str, out i);
            return i;
        }

        private static readonly KeyValuePair<string, object>[] HostedDatabaseOptions = new []
        {
            DatabaseTypeOption,
            HostedConnectionPath
        };

        private static readonly KeyValuePair<string, object>[] EmbeddedDatabaseOptions = new[]
        {
            DatabaseTypeOption,
            EmbeddedConnectionPath
        };

        public static IDatabase Database;
        public static readonly IEngine Engine = EngineFactory.GetEngine();
        public static readonly Colorspaces Colorspaces = new Colorspaces();
    }
}
