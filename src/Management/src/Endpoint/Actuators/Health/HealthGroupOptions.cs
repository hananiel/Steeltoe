// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

namespace Steeltoe.Management.Endpoint.Actuators.Health;

public sealed class HealthGroupOptions
{
    /// <summary>
    /// Gets or sets a comma-separated list of health contributor IDs and/or health check registration names to include in this group.
    /// </summary>
    public string? Include { get; set; }

    /// <summary>
    /// Gets or sets when to show components in this group, overriding the endpoint-level setting.
    /// </summary>
    public ShowValues? ShowComponents { get; set; }

    /// <summary>
    /// Gets or sets when to show details of components in this group, overriding the endpoint-level setting.
    /// </summary>
    public ShowValues? ShowDetails { get; set; }
}
