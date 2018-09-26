using System.Xml.Linq;

using Cogito.IIS.Configuration;
using Microsoft.Extensions.Logging;

namespace Cogito.HostedWebCore.Tests
{

    public static class Program
    {

        public static void Main()
        {
            var f = new LoggerFactory()
                .AddConsole();

            new AppHostBuilder()
                .SetAppConfigPath("%TEMP%\\ApplicationHost.config")
                .SetWebConfigPath("%TEMP%\\Web.config")
                .UseLogger(f.CreateLogger(""))
                .ConfigureWeb(c => c.Location("Foo", l => l.Element.Add(new XElement("bar"))))
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
