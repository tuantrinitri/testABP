﻿using Volo.Abp.Settings;

namespace Backend.AdministrationService.Settings;

public class AdministrationServiceSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AdministrationServiceSettings.MySetting1));
    }
}
