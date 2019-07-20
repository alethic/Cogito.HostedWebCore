using System;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.webServer/asp'.
    /// </summary>
    public class WebSystemWebServerAspConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebServerAspConfigurator(XElement element)
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
        WebSystemWebServerAspConfigurator SetAttributeValue(string attributeName, object attributeValue)
        {
            return this.Configure(e => e.SetAttributeValue(attributeName, attributeValue));
        }

        /// <summary>
        /// Helper method to set an attribute value.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        WebSystemWebServerAspConfigurator SetAttributeValue(string attributeName, bool? attributeValue)
        {
            return this.Configure(e => e.SetAttributeValue(attributeName, attributeValue is bool b ? (b ? "true" : "false") : null));
        }

        public WebSystemWebServerAspConfigurator AppAllowClientDebug(bool? enabled = null)
        {
            return SetAttributeValue("appAllowClientDebug", enabled);
        }

        public WebSystemWebServerAspConfigurator AppAllowDebugging(bool? enabled = null)
        {
            return SetAttributeValue("appAllowDebugging", enabled);
        }

        public WebSystemWebServerAspConfigurator BufferingOn(bool? enabled = null)
        {
            return SetAttributeValue("bufferingOn", enabled);
        }

        public WebSystemWebServerAspConfigurator CalcLineNumber(bool? enabled = null)
        {
            return SetAttributeValue("calcLineNumber", enabled);
        }

        public WebSystemWebServerAspConfigurator CodePage(uint? value = null)
        {
            return SetAttributeValue("calcLineNumber", value);
        }

        public WebSystemWebServerAspConfigurator EnableApplicationRestart(bool? enabled = null)
        {
            return SetAttributeValue("enableApplicationRestart", enabled);
        }

        public WebSystemWebServerAspConfigurator EnableAspHtmlFallback(bool? enabled = null)
        {
            return SetAttributeValue("enableAspHtmlFallback", enabled);
        }

        public WebSystemWebServerAspConfigurator EnableChunkedEncoding(bool? enabled = null)
        {
            return SetAttributeValue("enableChunkedEncoding", enabled);
        }

        public WebSystemWebServerAspConfigurator EnableParentPaths(bool? enabled = null)
        {
            return SetAttributeValue("enableParentPaths", enabled);
        }

        public WebSystemWebServerAspConfigurator ErrorsToNTLog(bool? enabled = null)
        {
            return SetAttributeValue("errorsToNTLog", enabled);
        }

        public WebSystemWebServerAspConfigurator ExceptionCatchEnable(bool? enabled = null)
        {
            return SetAttributeValue("exceptionCatchEnable", enabled);
        }

        public WebSystemWebServerAspConfigurator Lcid(uint? value = null)
        {
            return SetAttributeValue("lcid", value);
        }

        public WebSystemWebServerAspConfigurator LogErrorRequests(bool? enabled = null)
        {
            return SetAttributeValue("logErrorRequests", enabled);
        }

        public WebSystemWebServerAspConfigurator RunOnEndAnonymously(bool? enabled = null)
        {
            return SetAttributeValue("runOnEndAnonymously", enabled);
        }

        public WebSystemWebServerAspConfigurator ScriptErrorMessage(string value = null)
        {
            return SetAttributeValue("scriptErrorMessage", value);
        }

        public WebSystemWebServerAspConfigurator ScriptErrorSentToBrowser(bool? enabled = null)
        {
            return SetAttributeValue("scriptErrorSentToBrowser", enabled);
        }

        public WebSystemWebServerAspConfigurator ScriptLanguage(string value = null)
        {
            return SetAttributeValue("scriptLanguage", value);
        }

        public WebSystemWebServerAspConfigurator Cache(Action<WebSystemWebServerAspCacheConfigurator> configure)
        {
            this.Configure("cache", e => configure?.Invoke(new WebSystemWebServerAspCacheConfigurator(e)));
            return this;
        }

        public WebSystemWebServerAspConfigurator Limits(Action<WebSystemWebServerAspLimitsConfigurator> configure)
        {
            this.Configure("limits", e => configure?.Invoke(new WebSystemWebServerAspLimitsConfigurator(e)));
            return this;
        }

        public WebSystemWebServerAspConfigurator Session(Action<WebSystemWebServerAspSessionConfigurator> configure)
        {
            this.Configure("session", e => configure?.Invoke(new WebSystemWebServerAspSessionConfigurator(e)));
            return this;
        }

    }

}
