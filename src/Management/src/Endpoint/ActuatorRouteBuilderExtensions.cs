// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Steeltoe.Management.Endpoint;

public static class ActuatorRouteBuilderExtensions
{
    /// <summary>
    /// Maps the registered actuators, when using ASP.NET attribute-based endpoint routing.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IEndpointRouteBuilder" /> to add routes to.
    /// </param>
    /// <returns>
    /// An <see cref="IEndpointConventionBuilder" /> that can be used to further customize the actuator endpoints.
    /// </returns>
    public static IEndpointConventionBuilder MapActuators(this IEndpointRouteBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        IServiceProvider serviceProvider = builder.ServiceProvider;
        var mapper = serviceProvider.GetRequiredService<ActuatorEndpointMapper>();

        var conventionBuilder = new ActuatorConventionBuilder();
        mapper.Map(builder, conventionBuilder);
        return conventionBuilder;
    }

    /// <summary>
    /// Maps the registered actuators, when using ASP.NET conventional routing.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IRouteBuilder" /> to add routes to.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="builder" /> so that additional calls can be chained.
    /// </returns>
    public static IRouteBuilder MapActuators(this IRouteBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        IServiceProvider serviceProvider = builder.ServiceProvider;
        var mapper = serviceProvider.GetRequiredService<ActuatorEndpointMapper>();

        mapper.Map(builder);
        return builder;
    }
}
