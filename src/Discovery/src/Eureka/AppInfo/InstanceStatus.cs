// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;
using Steeltoe.Common.CasingConventions;

namespace Steeltoe.Discovery.Eureka.AppInfo;

/// <summary>
/// Lists the statuses a Eureka application instance can have.
/// </summary>
[JsonConverter(typeof(SnakeCaseAllCapsEnumMemberJsonConverter))]
public enum InstanceStatus
{
    /// <summary>
    /// Indicates the status is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// App is ready to accept traffic.
    /// </summary>
    Up,

    /// <summary>
    /// Indicates an unexpected, unplanned outage.
    /// </summary>
    Down,

    /// <summary>
    /// App is starting, not ready to accept traffic yet.
    /// </summary>
    Starting,

    /// <summary>
    /// Indicates a planned downtime, typically used for maintenance.
    /// </summary>
    OutOfService
}
