using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public interface IEngine
    {
        /// <summary>
        /// Generates and saves a movie file based on the input file of the request and the requested properties.
        /// </summary>
        /// <param name="request">Request object that contains all selected options for the output file.</param>
        /// <returns>Response object contains information about the execution of the method. This includes a link
        /// to the resulting movie file, if it exists, and any error messages or exceptions caused as a result of execution.</returns>
        EngineResult ProcessRequest(EngineRequest request);
    }
}
