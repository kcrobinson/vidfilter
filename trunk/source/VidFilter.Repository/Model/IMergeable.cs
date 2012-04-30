using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Model
{
    public interface IMergeable
    {
        string Id { get; }
        void MergeFrom(IMergeable newRecord);
    }
}
