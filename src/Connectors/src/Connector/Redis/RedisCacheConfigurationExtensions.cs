// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common;
using Steeltoe.Common.Reflection;
using Steeltoe.Connector.Services;

namespace Steeltoe.Connector.Redis;

public static class RedisCacheConfigurationExtensions
{
    public static RedisServiceConnectorFactory CreateRedisServiceConnectorFactory(this IConfiguration config, string serviceName = null)
    {
        ArgumentGuard.NotNull(config);

        var redisConfig = new RedisCacheConnectorOptions(config);
        return config.CreateRedisServiceConnectorFactory(redisConfig, serviceName);
    }

    public static RedisServiceConnectorFactory CreateRedisServiceConnectorFactory(this IConfiguration config, IConfiguration connectorConfiguration,
        string serviceName = null)
    {
        ArgumentGuard.NotNull(config);
        ArgumentGuard.NotNull(connectorConfiguration);

        var connectorOptions = new RedisCacheConnectorOptions(connectorConfiguration);
        return config.CreateRedisServiceConnectorFactory(connectorOptions, serviceName);
    }

    public static RedisServiceConnectorFactory CreateRedisServiceConnectorFactory(this IConfiguration config, RedisCacheConnectorOptions connectorOptions,
        string serviceName = null)
    {
        ArgumentGuard.NotNull(config);
        ArgumentGuard.NotNull(connectorOptions);

        string[] redisAssemblies =
        {
            "StackExchange.Redis",
            "StackExchange.Redis.StrongName",
            "Microsoft.Extensions.Caching.Redis"
        };

        string[] redisTypeNames =
        {
            "StackExchange.Redis.ConnectionMultiplexer",
            "Microsoft.Extensions.Caching.Distributed.IDistributedCache"
        };

        string[] redisOptionNames =
        {
            "StackExchange.Redis.ConfigurationOptions",
            "Microsoft.Extensions.Caching.Redis.RedisCacheOptions"
        };

        Type redisConnection = ReflectionHelpers.FindType(redisAssemblies, redisTypeNames);
        Type redisOptions = ReflectionHelpers.FindType(redisAssemblies, redisOptionNames);
        MethodInfo initializer = ReflectionHelpers.FindMethod(redisConnection, "Connect");

        RedisServiceInfo info = serviceName == null
            ? config.GetSingletonServiceInfo<RedisServiceInfo>()
            : config.GetRequiredServiceInfo<RedisServiceInfo>(serviceName);

        return new RedisServiceConnectorFactory(info, connectorOptions, redisConnection, redisOptions, initializer);
    }
}