using System;

namespace Cogito.IIS.Configuration
{

    public static class AppHostSiteExtensions
    {

        /// <summary>
        /// Adds a 'http' binding with the specified information.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static AppHostSiteConfigurator AddHttpBinding(this AppHostSiteConfigurator self, string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException(nameof(host));
            if (port < 1 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port));

            return self.AddBinding("http", $"*:{port}:{host}");
        }

        /// <summary>
        /// Adds a 'http' binding with the specified information.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static AppHostSiteConfigurator AddHttpsBinding(this AppHostSiteConfigurator self, string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException(nameof(host));
            if (port < 1 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port));

            return self.AddBinding("https", $"*:{port}:{host}");
        }

    }

}
