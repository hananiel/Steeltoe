﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Steeltoe.Management.Census.Stats.Aggregations
{
    public interface ILastValueDataDouble : IAggregationData
    {
        double LastValue { get; }
    }
}
