using System;
using System.Collections.Generic;
using System.Text;

namespace Cogito.HostedWebCore.Tests
{

    public static class Program
    {

        public static void Main()
        {
            new WebHostBuilder()
                .Configure(c => c
                    .SetBindingInformation("localhost", 12346)
                    .ConfigureVirtualDirectory("/", v => v.UsePhysicalPath("wwwroot"))
                    .UseFailedRequestLogging(@"%TEMP%\log.txt"))
                .Build()
                .Run();
        }

    }

}
