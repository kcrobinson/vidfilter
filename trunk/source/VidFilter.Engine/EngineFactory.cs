using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class EngineFactory
    {
        /// <summary>
        /// Engine is the only implementation of IEngine, but if another implementation happens, 
        /// this method can be updated to provide the new implemenation to all using projects based
        /// on whatever logic you want.
        /// </summary>
        /// <returns>An instantiated instance of the Engine class</returns>
        public static IEngine GetEngine()
        {
            return new Engine();
        }
    }
}
