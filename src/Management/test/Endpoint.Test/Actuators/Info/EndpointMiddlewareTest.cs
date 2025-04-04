// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.TestResources;
using Steeltoe.Management.Endpoint.Actuators.Info;
using Steeltoe.Management.Endpoint.Configuration;

namespace Steeltoe.Management.Endpoint.Test.Actuators.Info;

public sealed class EndpointMiddlewareTest : BaseTest
{
    private readonly Dictionary<string, string?> _appSettings = new()
    {
        ["management:endpoints:path"] = "/management",
        ["management:endpoints:info:enabled"] = "true",
        ["management:endpoints:info:id"] = "info-management",
        ["management:endpoints:actuator:exposure:include:0"] = "*",
        ["info:application:name"] = "foobar",
        ["info:application:version"] = "1.0.0'",
        ["info:application:date"] = "5/1/2008",
        ["info:application:time"] = "8:30:52 AM",
        ["info:NET:type"] = "Core",
        ["info:NET:version"] = "2.0.0",
        ["info:NET:ASPNET:type"] = "Core",
        ["info:NET:ASPNET:version"] = "2.0.0"
    };

    [Fact]
    public async Task InfoActuator_ReturnsExpectedData()
    {
        // Note: This test pulls in from git.properties and appsettings created
        // in the Startup class
        WebHostBuilder builder = TestWebHostBuilderFactory.Create();
        builder.UseStartup<Startup>();
        builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(_appSettings));

        using IWebHost app = builder.Build();
        await app.StartAsync(TestContext.Current.CancellationToken);

        using HttpClient client = app.GetTestClient();

        var dictionary = await client.GetFromJsonAsync<Dictionary<string, Dictionary<string, JsonElement>>>("http://localhost/management/info-management",
            SerializerOptions, TestContext.Current.CancellationToken);

        Assert.NotNull(dictionary);

        Assert.Equal(6, dictionary.Count);
        Assert.True(dictionary.ContainsKey("application"));
        Assert.True(dictionary.ContainsKey("NET"));
        Assert.True(dictionary.ContainsKey("git"));

        Dictionary<string, JsonElement> appNode = dictionary["application"];
        Assert.NotNull(appNode);
        Assert.Equal("foobar", appNode["name"].ToString());

        Dictionary<string, JsonElement> netNode = dictionary["NET"];
        Assert.NotNull(netNode);
        Assert.Equal("Core", netNode["type"].ToString());

        Dictionary<string, JsonElement> gitNode = dictionary["git"];
        Assert.NotNull(gitNode);
        Assert.True(gitNode.ContainsKey("build"));
        Assert.True(gitNode.ContainsKey("branch"));
        Assert.True(gitNode.ContainsKey("commit"));
        Assert.True(gitNode.ContainsKey("closest"));
        Assert.True(gitNode.ContainsKey("dirty"));
        Assert.True(gitNode.ContainsKey("remote"));
        Assert.True(gitNode.ContainsKey("tags"));
        gitNode["build"].TryGetProperty("time", out JsonElement bTime);
        Assert.Equal("2017-07-12T18:40:39Z", bTime.GetString());
        gitNode["commit"].TryGetProperty("time", out JsonElement cTime);
        Assert.Equal("2017-06-08T12:47:02Z", cTime.GetString());
    }

    [Fact]
    public async Task InfoActuator_UsesCustomJsonSerializerOptions()
    {
        // Note: This test pulls in from git.properties and appsettings created
        // in the Startup class
        var settings = new Dictionary<string, string?>
        {
            ["management:endpoints:CustomJsonConverters:0"] = "Steeltoe.Management.Endpoint.Actuators.Info.EpochSecondsDateTimeConverter"
        };

        WebHostBuilder builder = TestWebHostBuilderFactory.Create();
        builder.UseStartup<Startup>();
        builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(settings));

        using IWebHost app = builder.Build();
        await app.StartAsync(TestContext.Current.CancellationToken);

        using HttpClient client = app.GetTestClient();
        string response = await client.GetStringAsync(new Uri("http://localhost/actuator/info"), TestContext.Current.CancellationToken);

        Assert.Contains("1499884839000", response, StringComparison.Ordinal);
        Assert.DoesNotContain("2017-07-12T18:40:39Z", response, StringComparison.Ordinal);
        Assert.Contains("1496926022000", response, StringComparison.Ordinal);
        Assert.DoesNotContain("2017-06-08T12:47:02Z", response, StringComparison.Ordinal);
    }

    [Fact]
    public void RoutesByPathAndVerb()
    {
        var endpointOptions = GetOptionsFromSettings<InfoEndpointOptions>();
        ManagementOptions managementOptions = GetOptionsMonitorFromSettings<ManagementOptions>().CurrentValue;

        Assert.True(endpointOptions.RequiresExactMatch());
        Assert.Equal("/actuator/info", endpointOptions.GetPathMatchPattern(managementOptions.Path));
        Assert.Equal("/cloudfoundryapplication/info", endpointOptions.GetPathMatchPattern(ConfigureManagementOptions.DefaultCloudFoundryPath));
        Assert.Single(endpointOptions.AllowedVerbs);
        Assert.Contains("Get", endpointOptions.AllowedVerbs);
    }
}
