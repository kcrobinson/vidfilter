using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Repository
{
    public class DatabaseFactory
    {
        public static IDatabase GetDatabase(params KeyValuePair<string, object>[] options)
        {
            return new RavenDB(options);
        }
    }
}
