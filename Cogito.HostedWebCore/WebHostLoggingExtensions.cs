using System.Xml.Linq;

namespace Cogito.HostedWebCore
{

    public static class WebHostLoggingExtensions
    {

        /// <summary>
        /// Configures failed request logging.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static WebHostConfigurator UseFailedRequestLogging(this WebHostConfigurator configurator, string directory)
        {
            configurator.SiteElement.Elements("traceFailedRequestsLogging").Remove();
            configurator.SiteElement.Add(new XElement("traceFailedRequestsLogging",
                new XAttribute("enabled", true),
                new XAttribute("directory", directory)));
            return configurator;
        }

    }

}
