﻿namespace Backend.Identity;

public class AbpIdentityOptions
{
    public ExternalLoginProviderDictionary ExternalLoginProviders { get; }

    public AbpIdentityOptions()
    {
        ExternalLoginProviders = new ExternalLoginProviderDictionary();
    }
}