﻿#pragma warning disable SA1636 // File header copyright text should match

// <copyright file="LabelSet.cs" company="OpenTelemetry Authors">
// Copyright 2018, OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
#pragma warning restore SA1636 // File header copyright text should match

using System;
using System.Collections.Generic;
using System.Linq;

namespace Steeltoe.Management.OpenTelemetry.Metrics
{
    /// <summary>
    /// Normalized name value pairs of metric labels.
    /// </summary>
    [Obsolete("OpenTelemetry Metrics API is not considered stable yet, see https://github.com/SteeltoeOSS/Steeltoe/issues/711 more information")]
    public abstract class LabelSet
    {
        /// <summary>
        /// Empty LabelSet.
        /// </summary>
        public static readonly LabelSet BlankLabelSet = new BlankLabelSet();

        /// <summary>
        /// Gets or sets the labels after sorting and removing duplicates.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<string, string>> Labels { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
    }
}
