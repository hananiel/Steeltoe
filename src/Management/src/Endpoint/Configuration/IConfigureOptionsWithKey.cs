// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Options;

namespace Steeltoe.Management.Endpoint.Configuration;

public interface IConfigureOptionsWithKey<in T> : IConfigureOptions<T>
    where T : class
{
    string ConfigurationKey { get; }
}
