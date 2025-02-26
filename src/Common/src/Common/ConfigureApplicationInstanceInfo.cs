// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Steeltoe.Common;

internal sealed class ConfigureApplicationInstanceInfo : IConfigureOptions<ApplicationInstanceInfo>
{
    private readonly IConfiguration _configuration;

    public ConfigureApplicationInstanceInfo(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _configuration = configuration;
    }

    public void Configure(ApplicationInstanceInfo options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.ApplicationName ??= _configuration.GetValue<string>("spring:application:name") ??
            GetAspNetApplicationName() ?? Assembly.GetEntryAssembly()!.GetName().Name;
    }

    private string? GetAspNetApplicationName()
    {
        // When using UseStartup<T>() on the host builder, ASP.NET Core sets the below key to point to the assembly containing T.
        return _configuration.GetValue<string>("applicationName");
    }
}
