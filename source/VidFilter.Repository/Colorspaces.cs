using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VidFilter.Repository.Model;

namespace VidFilter.Repository
{
    public class Colorspaces
    {
        private List<Colorspace> _Colorspaces;

        public Colorspaces()
        {
            _Colorspaces = new List<Colorspace>();
        }

        public void AddColorspace(Colorspace colorspace)
        {
            if (_Colorspaces.Any(c => String.Equals(c.Name, colorspace.Name)))
                return;
            _Colorspaces.Add(colorspace);
        }

        public IEnumerable<string> GetAllNames()
        {
            return _Colorspaces.Select(c => c.Name);
        }

        public Colorspace Get(string name)
        {
            return _Colorspaces.Where(c => String.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
        }

        public string GetNameFromCodeName(string codeName)
        {
            return _Colorspaces.Where(c => String.Equals(c.CodeName, codeName, StringComparison.OrdinalIgnoreCase)).Select(c => c.Name).SingleOrDefault();
        }

        public struct SimplifiedColorspace
        {
            public string Name;
            public string Id;
        }

        public IEnumerable<string> Load(string filePath)
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(filePath);
            }
            catch(Exception e)
            {
                throw new Exception("Failure loading colorspace file", e);
            }
            
            var nameSpace = xdoc.Root.GetDefaultNamespace();
            var configLines = xdoc.Descendants(nameSpace + "Colorspace");
            List<string> errorMessages = new List<string>();
            List<Colorspace> colorspaces = new List<Colorspace>();
            foreach (var line in configLines)
            {
                Colorspace colorspace = new Colorspace();
                foreach (var att in line.Attributes())
                {
                    switch(att.Name.LocalName.ToLower())
                    {
                        case "name":
                            colorspace.Name = att.Value;
                            break;
                        case "codename":
                            colorspace.CodeName = att.Value;
                            break;
                        case "numchannels":
                            colorspace.NumChannels = parseInt(att.Value);
                            break;
                        case "bitsperpixel":
                            colorspace.BitsPerPixel = parseInt(att.Value);
                            break;
                    }
                }
                if (this._Colorspaces.Any(c => String.Equals(c.Name, colorspace.Name)) && !String.IsNullOrWhiteSpace(colorspace.Name))
                {
                    errorMessages.Add(String.Format("Colorspace with name {0} already exists", colorspace.Name));
                }
                else if (this._Colorspaces.Any(c => String.Equals(c.CodeName, colorspace.CodeName)) && !String.IsNullOrWhiteSpace(colorspace.CodeName))
                {
                    errorMessages.Add(String.Format("Colorspace with CodeName {0} already exists", colorspace.CodeName));
                }
                errorMessages.AddRange(colorspace.Validate());
                colorspaces.Add(colorspace);
            }
            if (!errorMessages.Any())
            {
                this._Colorspaces = colorspaces;
            }
            return errorMessages;
        }

        private int parseInt(string str)
        {
            int i;
            int.TryParse(str, out i);
            return i;
        }
    }
}
