using System;
using System.Xml.Linq;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'applicationPools/add/processModel'.
    /// </summary>
    public class AppHostApplicationPoolProcessModelConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostApplicationPoolProcessModelConfigurator(XElement element)
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
        AppHostApplicationPoolProcessModelConfigurator SetAttributeValue(string attributeName, string attributeValue)
        {
            return this.Configure(e => e.SetAttributeValue(attributeName, attributeValue));
        }

        /// <summary>
        /// Specifies how long (in minutes) a worker process should run idle if no new requests are received and the worker process is not processing requests. After the allocated time passes, the worker process should request that it be shut down by the WWW service.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppHostApplicationPoolProcessModelConfigurator IdleTimeout(TimeSpan? value)
        {
            return SetAttributeValue("idleTimeout", value?.TotalMinutes.ToString());
        }

        /// <summary>
        /// Specifies whether IIS loads the user profile for the application pool identity. Setting this value to false causes IIS to revert to IIS 6.0 behavior. IIS 6.0 does not load the user profile for an application pool identity.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppHostApplicationPoolProcessModelConfigurator LoadUserProfile(bool? value)
        {
            return SetAttributeValue("loadUserProfile", value?.ToString());
        }

        /// <summary>
        /// Specifies the time that the W3SVC service waits after it initiated a recycle. If the worker process does not shut down within the shutdownTimeLimit, it will be terminated by the W3SVC service.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppHostApplicationPoolProcessModelConfigurator ShutdownTimeLimit(TimeSpan? value)
        {
            return SetAttributeValue("shutdownTimeLimit", value?.ToString());
        }

        /// <summary>
        /// Specifies the time that the W3SVC service waits after it initiated a recycle. If the worker process does not shut down within the shutdownTimeLimit, it will be terminated by the W3SVC service.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppHostApplicationPoolProcessModelConfigurator StartupTimeLimit(TimeSpan? value)
        {
            return SetAttributeValue("startupTimeLimit", value?.ToString());
        }

    }

}