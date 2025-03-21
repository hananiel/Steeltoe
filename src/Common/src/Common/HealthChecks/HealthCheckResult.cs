// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;
using Steeltoe.Common.CasingConventions;
using Steeltoe.Common.Json;

namespace Steeltoe.Common.HealthChecks;

/// <summary>
/// The result of a health check.
/// </summary>
public sealed class HealthCheckResult
{
    /// <summary>
    /// Gets or sets the status of the health check.
    /// </summary>
    /// <remarks>
    /// Used by the health middleware to determine the HTTP Status code.
    /// </remarks>
    [JsonConverter(typeof(SnakeCaseAllCapsEnumMemberJsonConverter))]
    public HealthStatus Status { get; set; } = HealthStatus.Unknown;

    /// <summary>
    /// Gets or sets a description of the health check result.
    /// </summary>
    /// <remarks>
    /// Currently only used on check failures.
    /// </remarks>
    public string? Description { get; set; }

    /// <summary>
    /// Gets details of the health check.
    /// </summary>
    [JsonIgnoreEmptyCollection]
    public IDictionary<string, object> Details { get; } = new Dictionary<string, object>();
}
