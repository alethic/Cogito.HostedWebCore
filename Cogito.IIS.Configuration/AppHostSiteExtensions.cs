using System;

namespace Cogito.IIS.Configuration
{

    public static class AppHostSiteExtensions
    {

        public static AppHostSiteConfigurator SetBindingInformation(this AppHostSiteConfigurator self, string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException(nameof(host));
            if (port < 1)
                throw new ArgumentOutOfRangeException(nameof(port));

            return self.SetBindingInformation($"*:{port}:{host}");
        }

    }

}
