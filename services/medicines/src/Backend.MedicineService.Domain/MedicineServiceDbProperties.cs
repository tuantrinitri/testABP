namespace Backend.MedicineService;

public static class MedicineServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = ""; // Tiền tố trong tên các table

    public static string DbSchema { get; set; } = null; // Mặc định là dbo

    public const string ConnectionStringName = "MedicineService"; // Tên của ConnectionString
}