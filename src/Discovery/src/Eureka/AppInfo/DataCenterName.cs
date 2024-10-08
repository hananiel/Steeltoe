// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

#if !DEBUG
#pragma warning disable SA1602 // Enumeration items should be documented
#endif

namespace Steeltoe.Discovery.Eureka.AppInfo;

public enum DataCenterName
{
    Netflix,
    Amazon,
    MyOwn
}
