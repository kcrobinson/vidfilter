using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Repository
{
    public class DatabaseFactory
    {
        public static IDatabase GetDatabase()
        {
            return new RavenDB();
        }
    }
}
