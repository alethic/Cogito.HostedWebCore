using Cogito.IIS.Configuration;

namespace Cogito.HostedWebCore.Tests
{

    public static class Program
    {

        public static void Main()
        {
            new AppHostBuilder()
                .ConfigureApp(c => c
                    .Site(1, s => s
                        .RemoveBindings()
                        .AddHttpBinding("localhost", 12311)
                        .Application("/", a => a
                            .VirtualDirectory("/", v => v.UsePhysicalPath("wwwroot")))
                        .UseFailedRequestLogging(@"%TEMP%\log.txt")))
                .Build()
                .Run();
        }

    }

}
