namespace Backend.IdentityServer;

public static class IdentityServerDbProperties
{
    public static string DbTablePrefix { get; set; } = "IdentityServer";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "IdentityServer";
}
