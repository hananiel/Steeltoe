// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

namespace Steeltoe.Connectors.Services;

public class CosmosDbServiceInfo : ServiceInfo
{
    public string Host { get; set; }

    public string MasterKey { get; set; }

    public string ReadOnlyKey { get; set; }

    public string DatabaseId { get; set; }

    public string DatabaseLink { get; set; }

    public CosmosDbServiceInfo(string id)
        : base(id)
    {
    }
}
