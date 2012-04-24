using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VidFilter.Model;

namespace VidFilter.Repository
{
    public interface IDatabase : IDisposable
    {
        IEnumerable<T> Query<T>(params string[] Ids);
        OperationStatus Insert<T>(params T[] records) where T : IMergeable;
        OperationStatus Delete<T>(params T[] records);
        OperationStatus Update<T>(params T[] records) where T : IMergeable;
    }
}
