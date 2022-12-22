// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Steeltoe.Common;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Common.Reflection;
using Steeltoe.Connector.Services;

namespace Steeltoe.Connector.Oracle;

public static class OracleProviderServiceCollectionExtensions
{
    /// <summary>
    /// Add Oracle and its IHealthContributor to a ServiceCollection.
    /// </summary>
    /// <param name="services">
    /// Service collection to add to.
    /// </param>
    /// <param name="configuration">
    /// App configuration.
    /// </param>
    /// <param name="contextLifetime">
    /// Lifetime of the service to inject.
    /// </param>
    /// <param name="addSteeltoeHealthChecks">
    /// Add steeltoeHealth checks even if community health checks exist.
    /// </param>
    /// <returns>
    /// IServiceCollection for chaining.
    /// </returns>
    /// <remarks>
    /// OracleConnection is retrievable as both OracleConnection and IDbConnection.
    /// </remarks>
    public static IServiceCollection AddOracleConnection(this IServiceCollection services, IConfiguration configuration,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped, bool addSteeltoeHealthChecks = false)
    {
        ArgumentGuard.NotNull(services);
        ArgumentGuard.NotNull(configuration);

        var info = configuration.GetSingletonServiceInfo<OracleServiceInfo>();

        DoAdd(services, info, configuration, contextLifetime, addSteeltoeHealthChecks);
        return services;
    }

    /// <summary>
    /// Add Oracle and its IHealthContributor to a ServiceCollection.
    /// </summary>
    /// <param name="services">
    /// Service collection to add to.
    /// </param>
    /// <param name="configuration">
    /// App configuration.
    /// </param>
    /// <param name="serviceName">
    /// cloud foundry service name binding.
    /// </param>
    /// <param name="contextLifetime">
    /// Lifetime of the service to inject.
    /// </param>
    /// <param name="addSteeltoeHealthChecks">
    /// Add steeltoeHealth checks even if community health checks exist.
    /// </param>
    /// <returns>
    /// IServiceCollection for chaining.
    /// </returns>
    /// <remarks>
    /// OracleConnection is retrievable as both OracleConnection and IDbConnection.
    /// </remarks>
    public static IServiceCollection AddOracleConnection(this IServiceCollection services, IConfiguration configuration, string serviceName,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped, bool addSteeltoeHealthChecks = false)
    {
        ArgumentGuard.NotNull(services);
        ArgumentGuard.NotNullOrEmpty(serviceName);
        ArgumentGuard.NotNull(configuration);

        var info = configuration.GetRequiredServiceInfo<OracleServiceInfo>(serviceName);

        DoAdd(services, info, configuration, contextLifetime, addSteeltoeHealthChecks);
        return services;
    }

    private static void DoAdd(IServiceCollection services, OracleServiceInfo info, IConfiguration configuration, ServiceLifetime contextLifetime,
        bool addSteeltoeHealthChecks)
    {
        Type oracleConnection = ReflectionHelpers.FindType(OracleTypeLocator.Assemblies, OracleTypeLocator.ConnectionTypeNames);
        var options = new OracleProviderConnectorOptions(configuration);
        var factory = new OracleProviderConnectorFactory(info, options, oracleConnection);
        services.Add(new ServiceDescriptor(typeof(IDbConnection), factory.Create, contextLifetime));
        services.Add(new ServiceDescriptor(oracleConnection, factory.Create, contextLifetime));

        if (!services.Any(s => s.ServiceType == typeof(HealthCheckService)) || addSteeltoeHealthChecks)
        {
            services.Add(new ServiceDescriptor(typeof(IHealthContributor),
                ctx => new RelationalDbHealthContributor((IDbConnection)factory.Create(ctx), ctx.GetService<ILogger<RelationalDbHealthContributor>>()),
                ServiceLifetime.Singleton));
        }
    }
}