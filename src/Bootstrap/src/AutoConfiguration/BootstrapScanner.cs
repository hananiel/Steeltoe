// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Steeltoe.Common;
using Steeltoe.Common.DynamicTypeAccess;
using Steeltoe.Common.Hosting;
using Steeltoe.Common.Logging;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.ConfigServer;
using Steeltoe.Configuration.Encryption;
using Steeltoe.Configuration.Placeholder;
using Steeltoe.Configuration.RandomValue;
using Steeltoe.Configuration.SpringBoot;
using Steeltoe.Connectors.CosmosDb;
using Steeltoe.Connectors.CosmosDb.DynamicTypeAccess;
using Steeltoe.Connectors.MongoDb;
using Steeltoe.Connectors.MongoDb.DynamicTypeAccess;
using Steeltoe.Connectors.MySql;
using Steeltoe.Connectors.MySql.DynamicTypeAccess;
using Steeltoe.Connectors.PostgreSql;
using Steeltoe.Connectors.PostgreSql.DynamicTypeAccess;
using Steeltoe.Connectors.RabbitMQ;
using Steeltoe.Connectors.RabbitMQ.DynamicTypeAccess;
using Steeltoe.Connectors.Redis;
using Steeltoe.Connectors.Redis.DynamicTypeAccess;
using Steeltoe.Connectors.SqlServer;
using Steeltoe.Connectors.SqlServer.RuntimeTypeAccess;
using Steeltoe.Discovery.Configuration;
using Steeltoe.Discovery.Consul;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Logging.DynamicConsole;
using Steeltoe.Logging.DynamicSerilog;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Management.Prometheus;
using Steeltoe.Management.Tracing;

namespace Steeltoe.Bootstrap.AutoConfiguration;

internal sealed class BootstrapScanner
{
    private readonly HostBuilderWrapper _wrapper;
    private readonly AssemblyLoader _loader;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger _logger;

    public BootstrapScanner(HostBuilderWrapper wrapper, IReadOnlySet<string> assemblyNamesToExclude, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(wrapper);
        ArgumentNullException.ThrowIfNull(assemblyNamesToExclude);
        ArgumentGuard.ElementsNotNullOrWhiteSpace(assemblyNamesToExclude);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _wrapper = wrapper;
        _loader = new AssemblyLoader(assemblyNamesToExclude);
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger("Steeltoe.Bootstrap.AutoConfiguration");
    }

    public void ConfigureSteeltoe()
    {
        if (_loggerFactory is BootstrapLoggerFactory bootstrapLoggerFactory)
        {
            _wrapper.ConfigureServices(services => services.UpgradeBootstrapLoggerFactory(bootstrapLoggerFactory));
        }

        if (!WireIfLoaded(WireConfigServer, SteeltoeAssemblyNames.ConfigurationConfigServer))
        {
            WireIfLoaded(WireCloudFoundryConfiguration, SteeltoeAssemblyNames.ConfigurationCloudFoundry);
        }

        WireIfLoaded(WireRandomValueProvider, SteeltoeAssemblyNames.ConfigurationRandomValue);
        WireIfLoaded(WireSpringBootProvider, SteeltoeAssemblyNames.ConfigurationSpringBoot);
        WireIfLoaded(WireDecryptionProvider, SteeltoeAssemblyNames.ConfigurationEncryption);
        WireIfLoaded(WirePlaceholderResolver, SteeltoeAssemblyNames.ConfigurationPlaceholder);
        WireIfLoaded(WireConnectors, SteeltoeAssemblyNames.Connectors);

        if (!WireIfLoaded(WireDynamicSerilog, SteeltoeAssemblyNames.LoggingDynamicSerilog))
        {
            WireIfLoaded(WireDynamicConsole, SteeltoeAssemblyNames.LoggingDynamicConsole);
        }

        WireIfLoaded(WireDiscoveryConfiguration, SteeltoeAssemblyNames.DiscoveryConfiguration);
        WireIfLoaded(WireDiscoveryConsul, SteeltoeAssemblyNames.DiscoveryConsul);
        WireIfLoaded(WireDiscoveryEureka, SteeltoeAssemblyNames.DiscoveryEureka);
        WireIfLoaded(WireAllActuators, SteeltoeAssemblyNames.ManagementEndpoint);
        WireIfLoaded(WirePrometheus, SteeltoeAssemblyNames.ManagementPrometheus);
        WireIfLoaded(WireDistributedTracingLogProcessor, SteeltoeAssemblyNames.ManagementTracing);
    }

    private void WireConfigServer()
    {
        _wrapper.AddConfigServer(_loggerFactory);

        _logger.LogInformation("Configured Config Server configuration provider");
    }

    private void WireCloudFoundryConfiguration()
    {
        _wrapper.AddCloudFoundryConfiguration(_loggerFactory);

        _logger.LogInformation("Configured Cloud Foundry configuration provider");
    }

    private void WireRandomValueProvider()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddRandomValueSource(_loggerFactory));

        _logger.LogInformation("Configured random value configuration provider");
    }

    private void WireSpringBootProvider()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddSpringBootFromEnvironmentVariable(_loggerFactory));

        string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddSpringBootFromCommandLine(args, _loggerFactory));

        _logger.LogInformation("Configured Spring Boot configuration provider");
    }

    private void WireDecryptionProvider()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddDecryption(_loggerFactory));

        _logger.LogInformation("Configured decryption configuration provider");
    }

    private void WirePlaceholderResolver()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddPlaceholderResolver(_loggerFactory));

        _logger.LogInformation("Configured placeholder configuration provider");
    }

    private void WireConnectors()
    {
        WireIfAnyLoaded(WireCosmosDbConnector, CosmosDbPackageResolver.Default);
        WireIfAnyLoaded(WireMongoDbConnector, MongoDbPackageResolver.Default);
        WireIfAnyLoaded(WireMySqlConnector, MySqlPackageResolver.Default);
        WireIfAnyLoaded(WirePostgreSqlConnector, PostgreSqlPackageResolver.Default);
        WireIfAnyLoaded(WireRabbitMQConnector, RabbitMQPackageResolver.Default);
        WireIfAnyLoaded(WireRedisConnector, StackExchangeRedisPackageResolver.Default, MicrosoftRedisPackageResolver.Default);
        WireIfAnyLoaded(WireSqlServerConnector, SqlServerPackageResolver.Default);
    }

    private void WireCosmosDbConnector()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.ConfigureCosmosDb());
        _wrapper.ConfigureServices((host, services) => services.AddCosmosDb(host.Configuration));

        _logger.LogInformation("Configured CosmosDB connector");
    }

    private void WireMongoDbConnector()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.ConfigureMongoDb());
        _wrapper.ConfigureServices((host, services) => services.AddMongoDb(host.Configuration));

        _logger.LogInformation("Configured MongoDB connector");
    }

    private void WireMySqlConnector()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.ConfigureMySql());
        _wrapper.ConfigureServices((host, services) => services.AddMySql(host.Configuration));

        _logger.LogInformation("Configured MySQL connector");
    }

    private void WirePostgreSqlConnector()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.ConfigurePostgreSql());
        _wrapper.ConfigureServices((host, services) => services.AddPostgreSql(host.Configuration));

        _logger.LogInformation("Configured PostgreSQL connector");
    }

    private void WireRabbitMQConnector()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.ConfigureRabbitMQ());
        _wrapper.ConfigureServices((host, services) => services.AddRabbitMQ(host.Configuration));

        _logger.LogInformation("Configured RabbitMQ connector");
    }

    private void WireRedisConnector()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.ConfigureRedis());
        _wrapper.ConfigureServices((host, services) => services.AddRedis(host.Configuration));

        _logger.LogInformation("Configured StackExchange Redis connector");

        // Intentionally ignoring excluded assemblies here.
        if (MicrosoftRedisPackageResolver.Default.IsAvailable())
        {
            _logger.LogInformation("Configured Redis distributed cache connector");
        }
    }

    private void WireSqlServerConnector()
    {
        _wrapper.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.ConfigureSqlServer());
        _wrapper.ConfigureServices((host, services) => services.AddSqlServer(host.Configuration));

        _logger.LogInformation("Configured SQL Server connector");
    }

    private void WireDynamicSerilog()
    {
        _wrapper.ConfigureLogging(loggingBuilder => loggingBuilder.AddDynamicSerilog());

        _logger.LogInformation("Configured dynamic console logger for Serilog");
    }

    private void WireDynamicConsole()
    {
        _wrapper.ConfigureLogging(loggingBuilder => loggingBuilder.AddDynamicConsole());

        _logger.LogInformation("Configured dynamic console logger");
    }

    private void WireDiscoveryConfiguration()
    {
        _wrapper.ConfigureServices(services => services.AddConfigurationDiscoveryClient());

        _logger.LogInformation("Configured configuration discovery client");
    }

    private void WireDiscoveryConsul()
    {
        _wrapper.ConfigureServices(services => services.AddConsulDiscoveryClient());

        _logger.LogInformation("Configured Consul discovery client");
    }

    private void WireDiscoveryEureka()
    {
        _wrapper.ConfigureServices(services => services.AddEurekaDiscoveryClient());

        _logger.LogInformation("Configured Eureka discovery client");
    }

    private void WireAllActuators()
    {
        _wrapper.ConfigureServices(services => services.AddAllActuators());

        _logger.LogInformation("Configured actuators");
    }

    private void WirePrometheus()
    {
        _wrapper.ConfigureServices(services => services.AddPrometheusActuator());

        _logger.LogInformation("Configured Prometheus");
    }

    private void WireDistributedTracingLogProcessor()
    {
        _wrapper.ConfigureServices(services => services.AddTracingLogProcessor());

        _logger.LogInformation("Configured distributed tracing log processor");
    }

    private bool WireIfLoaded(Action wireAction, string assemblyName)
    {
        if (!_loader.IsAssemblyLoaded(assemblyName))
        {
            return false;
        }

        wireAction();
        return true;
    }

    private void WireIfAnyLoaded(Action wireAction, params PackageResolver[] packageResolvers)
    {
        if (Array.Exists(packageResolvers, packageResolver => packageResolver.IsAvailable(_loader.AssemblyNamesToExclude)))
        {
            wireAction();
        }
    }
}
