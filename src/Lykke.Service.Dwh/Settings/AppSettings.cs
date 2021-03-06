﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Dwh.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public DwhSettings DwhService { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DwhSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        public Dictionary<string, string> SqlConnection { get; set; }
    }
}
