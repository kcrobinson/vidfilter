using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;

namespace VidFilter.Repository.Model
{
    public class FriendlyName
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public FriendlyName() { }

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
