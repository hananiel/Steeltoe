// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Steeltoe.Configuration.Encryption;
using Steeltoe.Configuration.Placeholder;

namespace Steeltoe.Configuration.ConfigServer.Test;

public sealed class ConfigServerServiceCollectionExtensionsTest
{
    [Fact]
    public async Task ConfigureConfigServerClientOptions_ConfiguresConfigServerClientOptions_WithDefaults()
    {
        var builder = new ConfigurationBuilder();
        builder.AddConfigServer();
        IConfiguration configuration = builder.Build();

        var services = new ServiceCollection();
        services.AddSingleton(configuration);
        services.ConfigureConfigServerClientOptions();

        await using ServiceProvider serviceProvider = services.BuildServiceProvider(true);
        var optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<ConfigServerClientOptions>>();

        string? expectedAppName = Assembly.GetEntryAssembly()!.GetName().Name;
        TestHelper.VerifyDefaults(optionsMonitor.CurrentValue, expectedAppName);
    }

    [Fact]
    public void DoesNotAddConfigServerMultipleTimes()
    {
        var builder = new ConfigurationBuilder();
        builder.AddConfigServer();
        builder.AddPlaceholderResolver();
        builder.AddDecryption();
        builder.AddConfigServer();
        builder.AddPlaceholderResolver();
        builder.AddDecryption();
        builder.AddConfigServer();

        builder.EnumerateSources<ConfigServerConfigurationSource>().Should().ContainSingle();

        IConfigurationRoot configurationRoot = builder.Build();

        configurationRoot.EnumerateProviders<ConfigServerConfigurationProvider>().Should().ContainSingle();
    }
}
