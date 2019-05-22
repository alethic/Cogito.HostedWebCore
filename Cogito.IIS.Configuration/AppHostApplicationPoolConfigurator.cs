using System;
using System.Linq;
using System.Xml.Linq;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    public class AppHostApplicationPoolConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostApplicationPoolConfigurator(XElement element)
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
        AppHostApplicationPoolConfigurator SetAttributeValue(string attributeName, string attributeValue)
        {
            return this.Configure(e => e.SetAttributeValue(attributeName, attributeValue));
        }

        /// <summary>
        /// Sets the application pool name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AppHostApplicationPoolConfigurator Name(string name)
        {
            return SetAttributeValue("name", name);
        }

        /// <summary>
        /// When true, indicates to the World Wide Web Publishing Service (W3SVC) that the application pool should be automatically started when it is created or when IIS is started.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AppHostApplicationPoolConfigurator AutoStart(bool? value)
        {
            return SetAttributeValue("autoStart", value?.ToString());
        }

        /// <summary>
        /// When true, enables a 32-bit application to run on a computer that runs a 64-bit version of Windows.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AppHostApplicationPoolConfigurator Enable32BitAppOnWin64(bool? value)
        {
            return SetAttributeValue("enable32BitAppOnWin64", value?.ToString());
        }

        /// <summary>
        /// Specifies the request-processing mode that is used to process requests for managed content.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AppHostApplicationPoolConfigurator ManagedPipelineMode(AppHostApplicationPoolManagedPipelineMode? value)
        {
            return SetAttributeValue("managedPipelineMode", value.HasValue ? Enum.GetName(typeof(AppHostApplicationPoolManagedPipelineMode), value.Value) : null);
        }

        /// <summary>
        /// Indicates to HTTP.sys how many requests to queue for an application pool before rejecting future requests. The default value is 1000.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppHostApplicationPoolConfigurator QueueLength(int? value)
        {
            return SetAttributeValue("queueLength", value?.ToString());
        }

        /// <summary>
        /// Indicates to HTTP.sys how many requests to queue for an application pool before rejecting future requests. The default value is 1000.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppHostApplicationPoolConfigurator ProcessModel(Action<AppHostApplicationPoolProcessModelConfigurator> configure)
        {
            this.Configure("processModel", e => configure?.Invoke(new AppHostApplicationPoolProcessModelConfigurator(e)));
            return this;
        }

    }

}
