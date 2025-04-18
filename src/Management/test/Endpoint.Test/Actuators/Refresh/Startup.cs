// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Management.Endpoint.Actuators.CloudFoundry;
using Steeltoe.Management.Endpoint.Actuators.Refresh;

namespace Steeltoe.Management.Endpoint.Test.Actuators.Refresh;

public sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCloudFoundryActuator();
        services.AddRefreshActuator();
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}
