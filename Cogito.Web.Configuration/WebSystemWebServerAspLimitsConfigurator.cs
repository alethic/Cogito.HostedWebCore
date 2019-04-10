using System;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.webServer/asp/limits'.
    /// </summary>
    public class WebSystemWebServerAspLimitsConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebServerAspLimitsConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        /// <summary>
        /// Helper method to set an attribute value.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        WebSystemWebServerAspLimitsConfigurator SetAttributeValue(string attributeName, object attributeValue)
        {
            if (attributeValue != null)
                return this.Configure(e => e.SetAttributeValue(attributeName, attributeValue));
            else
                return this.Configure(e => e.Attribute(attributeName)?.Remove());
        }

        public WebSystemWebServerAspLimitsConfigurator BufferingLimit(uint? value)
        {
            return SetAttributeValue("bufferingLimit", value);
        }

        public WebSystemWebServerAspLimitsConfigurator MaxRequestEntityAllowed(uint? value)
        {
            return SetAttributeValue("maxRequestEntityAllowed", value);
        }

        public WebSystemWebServerAspLimitsConfigurator ProcessorThreadMax(uint? value)
        {
            return SetAttributeValue("processorThreadMax", value);
        }

        public WebSystemWebServerAspLimitsConfigurator QueueConnectionTestTime(TimeSpan? value)
        {
            return SetAttributeValue("queueConnectionTestTime", value);
        }

        public WebSystemWebServerAspLimitsConfigurator QueueTimeout(TimeSpan? value)
        {
            return SetAttributeValue("queueTimeout", value);
        }

        public WebSystemWebServerAspLimitsConfigurator RequestQueueMax(uint? value)
        {
            return SetAttributeValue("requestQueueMax", value);
        }

        public WebSystemWebServerAspLimitsConfigurator ScriptTimeout(TimeSpan? value)
        {
            return SetAttributeValue("scriptTimeout", value);
        }

    }

}
