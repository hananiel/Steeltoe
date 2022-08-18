// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common;
using Steeltoe.Common.Reflection;
using Steeltoe.Connector.EntityFrameworkCore;
using Steeltoe.Connector.Services;

namespace Steeltoe.Connector.PostgreSql.EntityFrameworkCore;

public static class PostgresDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UseNpgsql(this DbContextOptionsBuilder optionsBuilder, IConfiguration config, object npgsqlOptionsAction = null)
    {
        ArgumentGuard.NotNull(optionsBuilder);
        ArgumentGuard.NotNull(config);

        string connection = GetConnection(config);

        return DoUseNpgsql(optionsBuilder, connection, npgsqlOptionsAction);
    }

    public static DbContextOptionsBuilder UseNpgsql(this DbContextOptionsBuilder optionsBuilder, IConfiguration config, string serviceName,
        object npgsqlOptionsAction = null)
    {
        ArgumentGuard.NotNull(optionsBuilder);
        ArgumentGuard.NotNull(config);
        ArgumentGuard.NotNullOrEmpty(serviceName);

        string connection = GetConnection(config, serviceName);

        return DoUseNpgsql(optionsBuilder, connection, npgsqlOptionsAction);
    }

    public static DbContextOptionsBuilder<TContext> UseNpgsql<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, IConfiguration config,
        object npgsqlOptionsAction = null)
        where TContext : DbContext
    {
        ArgumentGuard.NotNull(optionsBuilder);
        ArgumentGuard.NotNull(config);

        string connection = GetConnection(config);

        return DoUseNpgsql(optionsBuilder, connection, npgsqlOptionsAction);
    }

    public static DbContextOptionsBuilder<TContext> UseNpgsql<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, IConfiguration config,
        string serviceName, object npgsqlOptionsAction = null)
        where TContext : DbContext
    {
        ArgumentGuard.NotNull(optionsBuilder);
        ArgumentGuard.NotNull(config);
        ArgumentGuard.NotNullOrEmpty(serviceName);

        string connection = GetConnection(config, serviceName);

        return DoUseNpgsql(optionsBuilder, connection, npgsqlOptionsAction);
    }

    private static MethodInfo FindUseNpgsqlMethod(Type type, IReadOnlyList<Type> parameterTypes)
    {
        return type.GetMethods().FirstOrDefault(method => MatchesSignature(method, parameterTypes));
    }

    private static bool MatchesSignature(MethodBase method, IReadOnlyList<Type> parameterTypes)
    {
        if (method.IsPublic && method.IsStatic && method.Name.Equals("UseNpgsql", StringComparison.InvariantCulture))
        {
            ParameterInfo[] parameters = method.GetParameters();
            return parameters.Length == 3 && parameters[0].ParameterType == parameterTypes[0] && parameters[1].ParameterType == parameterTypes[1];
        }

        return false;
    }

    private static string GetConnection(IConfiguration config, string serviceName = null)
    {
        PostgresServiceInfo info = string.IsNullOrEmpty(serviceName)
            ? config.GetSingletonServiceInfo<PostgresServiceInfo>()
            : config.GetRequiredServiceInfo<PostgresServiceInfo>(serviceName);

        var postgresConfig = new PostgresProviderConnectorOptions(config);

        var factory = new PostgresProviderConnectorFactory(info, postgresConfig, null);
        return factory.CreateConnectionString();
    }

    private static DbContextOptionsBuilder DoUseNpgsql(DbContextOptionsBuilder builder, string connection, object npgsqlOptionsAction = null)
    {
        Type extensionType = EntityFrameworkCoreTypeLocator.PostgreSqlDbContextOptionsType;

        MethodInfo useMethod = FindUseNpgsqlMethod(extensionType, new[]
        {
            typeof(DbContextOptionsBuilder),
            typeof(string)
        });

        if (extensionType == null)
        {
            throw new ConnectorException("Unable to find UseNpgsql extension, are you missing Postgres EntityFramework Core assembly");
        }

        object result = ReflectionHelpers.Invoke(useMethod, null, new[]
        {
            builder,
            connection,
            npgsqlOptionsAction
        });

        if (result == null)
        {
            throw new ConnectorException($"Failed to invoke UseNpgsql extension, connection: {connection}");
        }

        return (DbContextOptionsBuilder)result;
    }

    private static DbContextOptionsBuilder<TContext> DoUseNpgsql<TContext>(DbContextOptionsBuilder<TContext> builder, string connection,
        object npgsqlOptionsAction = null)
        where TContext : DbContext
    {
        return (DbContextOptionsBuilder<TContext>)DoUseNpgsql((DbContextOptionsBuilder)builder, connection, npgsqlOptionsAction);
    }
}