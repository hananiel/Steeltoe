// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Common;
using Steeltoe.Management.Endpoint.Hypermedia;

namespace Steeltoe.Management.Endpoint.Refresh;

public static class EndpointServiceCollectionExtensions
{
    /// <summary>
    /// Adds components of the Refresh actuator to the D/I container.
    /// </summary>
    /// <param name="services">
    /// Service collection to add actuator to.
    /// </param>
    /// <param name="config">
    /// Application configuration. Retrieved from the <see cref="IServiceCollection" /> if not provided (this actuator looks for a settings starting with
    /// management:endpoints:refresh).
    /// </param>
    public static void AddRefreshActuator(this IServiceCollection services, IConfiguration config = null)
    {
        ArgumentGuard.NotNull(services);

        config ??= services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddActuatorManagementOptions(config);
        services.AddRefreshActuatorServices(config);
        services.AddActuatorEndpointMapping<RefreshEndpoint>();
    }
}
