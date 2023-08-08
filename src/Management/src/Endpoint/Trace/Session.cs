// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Steeltoe.Common;

namespace Steeltoe.Management.Endpoint.Trace;

public sealed class Session
{
    public string Id { get; }

    public Session(string id)
    {
        ArgumentGuard.NotNull(id);

        Id = id;
    }
}
