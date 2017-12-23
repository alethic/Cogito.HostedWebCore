using System.Xml.Linq;

namespace Cogito.IIS.Configuration
{

    public static class AppHostSiteLoggingExtensions
    {

        /// <summary>
        /// Configures failed request logging.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static AppHostSiteConfigurator UseFailedRequestLogging(this AppHostSiteConfigurator configurator, string directory)
        {
            configurator.Element.Elements("traceFailedRequestsLogging").Remove();
            configurator.Element.Add(new XElement("traceFailedRequestsLogging",
                new XAttribute("enabled", true),
                new XAttribute("directory", directory)));
            return configurator;
        }

    }

}
