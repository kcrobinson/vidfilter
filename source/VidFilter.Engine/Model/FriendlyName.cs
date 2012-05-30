using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;

namespace VidFilter.Engine
{
    public class FriendlyName
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public FriendlyName(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
