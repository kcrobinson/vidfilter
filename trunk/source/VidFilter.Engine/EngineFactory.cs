using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class EngineFactory
    {
        public static IEngine GetEngine()
        {
            return new Engine();
        }
    }
}
