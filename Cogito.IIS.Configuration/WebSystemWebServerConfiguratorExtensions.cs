using System;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    public static class WebSystemWebServerConfiguratorExtensions
    {

        public static WebSystemWebServerConfigurator GlobalModules(this WebSystemWebServerConfigurator self, Action<AppHostGlobalModuleConfigurator> configurator = null)
        {
            return self.Configure("globalModules", e => configurator?.Invoke(new AppHostGlobalModuleConfigurator(e)));
        }

    }

}
