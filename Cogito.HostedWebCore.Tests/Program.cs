namespace Cogito.HostedWebCore.Tests
{

    public static class Program
    {

        public static void Main()
        {
            new WebHostBuilder()
                .Configure(c => c
                    .SetBindingInformation("localhost", 12311)
                    .ConfigureVirtualDirectory("/", v => v.UsePhysicalPath("wwwroot"))
                    .UseFailedRequestLogging(@"%TEMP%\log.txt"))
                .Build()
                .Run();
        }

    }

}
