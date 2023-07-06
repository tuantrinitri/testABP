namespace Backend.Identity.Identity;

public class AbpUserConsts
{
    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxUserNameLength { get; set; } = 50;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxNameLength { get; set; } = 128;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxSurnameLength { get; set; } = 128;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxEmailLength { get; set; } = 256;

    /// <summary>
    /// Default value: 16
    /// </summary>
    public static int MaxPhoneNumberLength { get; set; } = 16;
}