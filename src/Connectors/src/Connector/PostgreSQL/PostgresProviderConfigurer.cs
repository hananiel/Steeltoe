// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Steeltoe.Common;
using Steeltoe.Common.Extensions;
using Steeltoe.Connector.Services;

namespace Steeltoe.Connector.PostgreSql;

public class PostgresProviderConfigurer
{
    internal string Configure(PostgresServiceInfo si, PostgresProviderConnectorOptions configuration)
    {
        UpdateConfiguration(si, configuration);
        return configuration.ToString();
    }

    internal void UpdateConfiguration(PostgresServiceInfo si, PostgresProviderConnectorOptions configuration)
    {
        if (si == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(si.Uri))
        {
            if (si.Port > 0)
            {
                configuration.Port = si.Port;
            }

            configuration.Username = si.UserName;
            configuration.Password = si.Password;
            configuration.Host = si.Host;
            configuration.Database = si.Path;

            if (si.Query != null)
            {
                foreach (KeyValuePair<string, string> kvp in UriExtensions.ParseQuerystring(si.Query))
                {
                    if (kvp.Key.Equals("sslmode", StringComparison.OrdinalIgnoreCase))
                    {
                        // Npgsql parses SSL Mode into an enum, the first character must be capitalized
                        configuration.SslMode = FirstCharToUpperInvariant(kvp.Value);
                    }
                    else if (kvp.Key == "sslcert")
                    {
                        // TODO: Map this client cert into the npgsql client cert callback
                        configuration.ClientCertificate = kvp.Value;
                    }
                    else if (kvp.Key == "sslkey")
                    {
                        // TODO: Map this client cert into the npgsql client cert callback
                        configuration.ClientKey = kvp.Value;
                    }
                    else if (kvp.Key == "sslrootcert")
                    {
                        // TODO: Map this client cert into the npgsql remote cert validation callback
                        configuration.SslRootCertificate = kvp.Value;
                    }
                    else
                    {
                        configuration.Options.Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }
    }

    // from https://stackoverflow.com/a/4405876/761468
    private string FirstCharToUpperInvariant(string input)
    {
        ArgumentGuard.NotNullOrEmpty(input);

        return string.Concat(input[0].ToString(CultureInfo.InvariantCulture).ToUpperInvariant(), input.AsSpan(1));
    }
}
