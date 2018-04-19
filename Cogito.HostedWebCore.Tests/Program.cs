using System;
using System.Xml.Linq;

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
                        .AddBinding("http", ":12323:")
                        .Application("/", a => a
                            .VirtualDirectory("/", v => v.UsePhysicalPath("wwwroot")))))
                .Build()
                .Run();
        }

    }

}
