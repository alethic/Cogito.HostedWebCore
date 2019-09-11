using System;

using Cogito.IIS.Configuration;
using Cogito.Web.Configuration;

using Microsoft.Extensions.Logging;

namespace Cogito.HostedWebCore.Tests
{

    public static class Program
    {

        public static void Main()
        {
            new AppHostBuilder()
                .SetAppConfigPath("%TEMP%\\ApplicationHost.config")
                .SetWebConfigPath("%TEMP%\\Web.config")
                .UseLogger(new LoggerFactory().AddConsole().CreateLogger(""))
                .ConfigureWeb("Web.config", c => c
                    .SystemWeb(w => w
                        .Compilation(z => z.TempDirectory(Environment.ExpandEnvironmentVariables(@"%TEMP%\T")))))
                .ConfigureApp("ApplicationHost.config", c => c
                    .ApplicationPoolDefaults(p => p
                        .ProcessModel(m => m
                            .ShutdownTimeLimit(TimeSpan.FromMinutes(15))))
                    .Site(1, s => s
                        .RemoveBindings()
                        .AddHttpBinding("localhost", 11010)
                        .Application("/", a => a
                            .VirtualDirectory("/", v => v.UsePhysicalPath("wwwroot")))
                        .UseFailedRequestLogging(@"%TEMP%\log.txt")))
                .OnStarted(h => Console.WriteLine("Started"))
                .OnStopped(h => Console.WriteLine("Stopped"))
                .Build()
                .Run();
        }

    }

}
