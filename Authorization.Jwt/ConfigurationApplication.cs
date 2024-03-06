namespace Authentication.Jwt
{
    public class ConfigurationApplication
    {
        public static IConfiguration AppSetting
        {
            get;
        }
        static ConfigurationApplication()
        {
            AppSetting = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }
    }
}
