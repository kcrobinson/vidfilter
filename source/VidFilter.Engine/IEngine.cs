﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public interface IEngine
    {
        EngineResult ProcessRequest(EngineRequest request);
    }
}
