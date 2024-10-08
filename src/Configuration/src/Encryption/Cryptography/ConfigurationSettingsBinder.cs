// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;

namespace Steeltoe.Configuration.Encryption.Cryptography;

internal static class ConfigurationSettingsBinder
{
    private const string ConfigurationPrefix = "encrypt";

    public static void Initialize(ConfigServerDecryptionSettings settings, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(configuration);

        IConfigurationSection configurationSection = configuration.GetSection(ConfigurationPrefix);
        configurationSection.Bind(settings);
    }
}
