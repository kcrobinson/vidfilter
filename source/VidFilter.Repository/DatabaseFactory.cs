using System.Collections.Generic;

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
