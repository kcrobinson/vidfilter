using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;
using VidFilter.Engine;
using System.IO;

namespace VidFilter
{
    class MainWindowModel
    {
        public ICollectionView Movies { get; private set; }

        public MainWindowModel()
        {
            IDatabase database = DatabaseFactory.GetDatabase();
            //IEnumerable<Movie> movies = database.QueryAllMovies();
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie(new FileInfo(@"C:\users\krobins\vidfilter\clips\bus.avi"))
            };

            Movies = CollectionViewSource.GetDefaultView(movies);
        }
    }
}
