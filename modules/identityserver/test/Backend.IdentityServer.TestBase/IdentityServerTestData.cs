using System;
using Volo.Abp.DependencyInjection;

namespace Backend.IdentityServer;

public class IdentityServerTestData : ISingletonDependency
{
    public Guid Client1Id { get; } = Guid.NewGuid();

    public Guid ApiResource1Id { get; } = Guid.NewGuid();

    public Guid IdentityResource1Id { get; } = Guid.NewGuid();
}
