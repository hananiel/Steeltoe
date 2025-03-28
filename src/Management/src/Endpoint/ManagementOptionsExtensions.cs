// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Http;
using Steeltoe.Management.Endpoint.Configuration;

namespace Steeltoe.Management.Endpoint;

internal static class ManagementOptionsExtensions
{
    public static string? GetBasePath(this ManagementOptions managementOptions, PathString requestPath)
    {
        ArgumentNullException.ThrowIfNull(managementOptions);

        return requestPath.StartsWithSegments(ConfigureManagementOptions.DefaultCloudFoundryPath)
            ? ConfigureManagementOptions.DefaultCloudFoundryPath
            : managementOptions.Path;
    }
}
