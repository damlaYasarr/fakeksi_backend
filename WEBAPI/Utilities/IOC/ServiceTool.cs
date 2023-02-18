namespace WEBAPI.Utilities.IOC
{
    public class ServiceTool
    {   
        // here provides autofac injection created
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
