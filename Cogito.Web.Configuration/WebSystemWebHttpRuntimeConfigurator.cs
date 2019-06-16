using System;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.web'.
    /// </summary>
    public class WebSystemWebHttpRuntimeConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebHttpRuntimeConfigurator(XElement element)
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
        WebSystemWebHttpRuntimeConfigurator SetAttributeValue(string attributeName, object attributeValue)
        {
            if (attributeValue != null)
                return this.Configure(e => e.SetAttributeValue(attributeName, attributeValue));
            else
                return this.Configure(e => e.Attribute(attributeName)?.Remove());
        }

        /// <summary>
        /// Sets the 'apartmentThreading' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator ApartmentThreading(bool? value)
        {
            return SetAttributeValue("apartmentThreading", value);
        }

        /// <summary>
        /// Sets the 'appRequestQueueLimit' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator AppRequestQueueLimit(int? value)
        {
            return SetAttributeValue("appRequestQueueLimit", value);
        }

        /// <summary>
        /// Sets the 'delayNotificationTimeout' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator DelayNotificationTimeout(int? value)
        {
            return SetAttributeValue("delayNotificationTimeout", value);
        }

        /// <summary>
        /// Sets the 'encoderType' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator EncoderType(string value)
        {
            return SetAttributeValue("encoderType", value);
        }

        /// <summary>
        /// Sets the 'enable' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator Enable(bool? value)
        {
            return SetAttributeValue("enable", value);
        }

        /// <summary>
        /// Sets the 'enableHeaderChecking' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator EnableHeaderChecking(bool? value)
        {
            return SetAttributeValue("enableHeaderChecking", value);
        }

        /// <summary>
        /// Sets the 'enableKernelOutputCache' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator EnableKernelOutputCache(bool? value)
        {
            return SetAttributeValue("enableKernelOutputCache", value);
        }

        /// <summary>
        /// Sets the 'enableVersionHeader' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator EnableVersionHeader(bool? value)
        {
            return SetAttributeValue("enableVersionHeader", value);
        }

        /// <summary>
        /// Sets the 'executionTimeout' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator ExecutionTimeout(int? value)
        {
            return SetAttributeValue("executionTimeout", value);
        }

        /// <summary>
        /// Sets the 'maxQueryStringLength' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator MaxQueryStringLength(int? value)
        {
            return SetAttributeValue("maxQueryStringLength", value);
        }

        /// <summary>
        /// Sets the 'maxRequestLength' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator MaxRequestLength(int? value)
        {
            return SetAttributeValue("maxRequestLength", value);
        }

        /// <summary>
        /// Sets the 'maxUrlLength' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator MaxUrlLength(int? value)
        {
            return SetAttributeValue("maxUrlLength", value);
        }

        /// <summary>
        /// Sets the 'maxWaitChangeNotification' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator MaxWaitChangeNotification(int? value)
        {
            return SetAttributeValue("maxWaitChangeNotification", value);
        }

        /// <summary>
        /// Sets the 'minFreeThreads' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator MinFreeThreads(int? value)
        {
            return SetAttributeValue("minFreeThreads", value);
        }

        /// <summary>
        /// Sets the 'minLocalRequestFreeThreads' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator MinLocalRequestFreeThreads(int? value)
        {
            return SetAttributeValue("minLocalRequestFreeThreads", value);
        }

        /// <summary>
        /// Sets the 'relaxedUrlToFileSystemMapping' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator RelaxedUrlToFileSystemMapping(bool? value)
        {
            return SetAttributeValue("relaxedUrlToFileSystemMapping", value);
        }

        /// <summary>
        /// Sets the 'requestLengthDiskThreshold' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator RequestLengthDiskThreshold(int? value)
        {
            return SetAttributeValue("relaxedUrlToFileSystemMapping", value);
        }

        /// <summary>
        /// Sets the 'requestPathInvalidCharacters' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator RequestPathInvalidCharacters(string value)
        {
            return SetAttributeValue("requestPathInvalidCharacters", value);
        }

        /// <summary>
        /// Sets the 'requestValidationMode' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator RequestValidationMode(int? value)
        {
            return SetAttributeValue("requestValidationMode", value);
        }

        /// <summary>
        /// Sets the 'requestValidationType' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator RequestValidationType(string value)
        {
            return SetAttributeValue("requestValidationType", value);
        }

        /// <summary>
        /// Sets the 'requireRootedSaveAsPath' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator RequireRootedSaveAsPath(bool? value)
        {
            return SetAttributeValue("requireRootedSaveAsPath", value);
        }

        /// <summary>
        /// Sets the 'sendCacheControlHeader' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator SendCacheControlHeader(bool? value)
        {
            return SetAttributeValue("sendCacheControlHeader", value);
        }

        /// <summary>
        /// Sets the 'shutdownTimeout' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator ShutdownTimeout(int? value)
        {
            return SetAttributeValue("shutdownTimeout", value);
        }

        /// <summary>
        /// Sets the 'useFullyQualifiedRedirectUrl' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator UseFullyQualifiedRedirectUrl(bool? value)
        {
            return SetAttributeValue("useFullyQualifiedRedirectUrl", value);
        }

        /// <summary>
        /// Sets the 'waitChangeNotification' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebSystemWebHttpRuntimeConfigurator WaitChangeNotification(int? value)
        {
            return SetAttributeValue("waitChangeNotification", value);
        }

    }

}
