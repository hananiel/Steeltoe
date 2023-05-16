// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Data.Entity;
using MySql.Data.EntityFramework;

namespace Steeltoe.Connectors.EntityFramework6.Test;

[DbConfigurationType(typeof(MySqlEFConfiguration))]
public class Good2MySqlDbContext : DbContext
{
    public Good2MySqlDbContext(string str)
    {
    }
}
