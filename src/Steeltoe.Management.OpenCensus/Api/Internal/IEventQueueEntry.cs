﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Steeltoe.Management.Census.Internal
{
    public interface IEventQueueEntry
    {
        void Process();
    }
}
