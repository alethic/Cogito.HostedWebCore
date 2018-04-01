using System;
using System.Xml.Linq;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.applicationHost/log'.
    /// </summary>
    public class AppHostLogConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostLogConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        /// <summary>
        /// Configures individual site logging.
        /// </summary>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public AppHostLogConfigurator UseSite()
        {
            element.RemoveAll();
            element.SetAttributeValue("centralLogFileMode", "Site");
            return this;
        }

        /// <summary>
        /// Configures centralized binary logging.
        /// </summary>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public AppHostLogConfigurator UseBinary(
            string directory = null,
            bool? localTimeRollover = false,
            AppHostLogPeriod? period = null,
            long? truncateSize = null)
        {
            element.RemoveNodes();
            element.SetAttributeValue("centralLogFileMode", "CentralBinary");

            return this.Configure("centralBinaryLogFile", e =>
            {
                e.SetAttributeValue("enabled", true);

                if (directory != null)
                    e.SetAttributeValue("directory", directory);
                if (localTimeRollover != null)
                    e.SetAttributeValue("localTimeRollover", localTimeRollover);
                if (period != null)
                    e.SetAttributeValue("period", Enum.GetName(typeof(AppHostLogPeriod), period));
                if (truncateSize != null)
                    e.SetAttributeValue("truncateSize", truncateSize);
            });
        }

        /// <summary>
        /// Configures centralized W3C logging.
        /// </summary>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public AppHostLogConfigurator UseW3C(
            string directory = null,
            bool? localTimeRollover = false,
            AppHostLogExtFileFlags? logExtFileFlags = null,
            AppHostLogPeriod? period = null,
            long? truncateSize = null)
        {
            element.RemoveNodes();
            element.SetAttributeValue("centralLogFileMode", "CentralW3C");

            return this.Configure("centralW3CLogFile", e =>
            {
                e.SetAttributeValue("enabled", true);

                if (directory != null)
                    e.SetAttributeValue("directory", directory);
                if (localTimeRollover != null)
                    e.SetAttributeValue("localTimeRollover", localTimeRollover);
                if (logExtFileFlags != null)
                    e.SetAttributeValue("logExtFileFlags", logExtFileFlags.ToString());
                if (period != null)
                    e.SetAttributeValue("period", Enum.GetName(typeof(AppHostLogPeriod), period));
                if (truncateSize != null)
                    e.SetAttributeValue("truncateSize", truncateSize);
            });
        }

    }

}
