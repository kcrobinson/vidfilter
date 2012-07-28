using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;

namespace VidFilter
{
    [RunInstaller(true)]
    public partial class VidFilterInstaller : Installer
    {
        public VidFilterInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            string targetDirectory = Context.Parameters["targetdir"];

            string dataPath = Context.Parameters["DataPath"];
            if (String.IsNullOrWhiteSpace(dataPath))
            {
                dataPath = @"data";
            }

            string exePath = string.Format("{0}VidFilter.exe", targetDirectory);
            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);

            config.AppSettings.Settings["EmbeddedConnectionPath"].Value = dataPath;
            config.AppSettings.Settings["FfmpegDirectoryPath"].Value = String.Empty;
            config.Save();
        }
    }
}
