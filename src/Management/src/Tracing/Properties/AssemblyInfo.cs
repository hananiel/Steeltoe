// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using Aspire;
using Steeltoe.Common.Configuration;

[assembly: ConfigurationSchema("Spring:Application", typeof(SpringApplicationSettings))]

[assembly: InternalsVisibleTo("Steeltoe.Management.Tracing.Test")]
