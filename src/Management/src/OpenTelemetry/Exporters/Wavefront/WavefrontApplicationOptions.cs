// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Steeltoe.Common;

namespace Steeltoe.Management.OpenTelemetry.Exporters.Wavefront;

public class WavefrontApplicationOptions
{
    internal const string WavefrontPrefix = "wavefront:application";

    public string Source { get; set; }

    public string Name { get; set; }

    public string Service { get; set; }

    public string Cluster { get; set; }

    public WavefrontApplicationOptions(IConfiguration config)
    {
        ArgumentGuard.NotNull(config);

        IConfigurationSection section = config.GetSection(WavefrontPrefix);

        if (section == null)
        {
            throw new InvalidOperationException($"Failed to locate configuration section '{WavefrontPrefix}'.");
        }

        section.Bind(this);
    }
}
