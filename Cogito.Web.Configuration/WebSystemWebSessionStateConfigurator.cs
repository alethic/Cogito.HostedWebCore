using System;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.web/sessionState'.
    /// </summary>
    public class WebSystemWebSessionStateConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebSessionStateConfigurator(XElement element)
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
        WebSystemWebSessionStateConfigurator SetAttributeValue(string attributeName, string attributeValue)
        {
            return this.Configure(e => e.SetAttributeValue(attributeName, attributeValue));
        }

        /// <summary>
        /// Sets the 'mode' attribute.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator Mode(WebSystemWebSessionStateMode? mode)
        {
            return SetAttributeValue("mode", mode.HasValue ? Enum.GetName(typeof(WebSystemWebSessionStateMode), mode.Value) : null);
        }

        /// <summary>
        /// Sets the 'stateConnectionString' attribute.
        /// </summary>
        /// <param name="stateConnectionString"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator StateConnectionString(string stateConnectionString)
        {
            return SetAttributeValue("stateConnectionString", stateConnectionString);
        }

        /// <summary>
        /// Sets the 'stateNetworkTimeout' attribute.
        /// </summary>
        /// <param name="stateNetworkTimeout"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator StateNetworkTimeout(TimeSpan? stateNetworkTimeout)
        {
            return SetAttributeValue("stateNetworkTimeout", stateNetworkTimeout.HasValue ? ((int)stateNetworkTimeout.Value.TotalSeconds).ToString() : null);
        }

        /// <summary>
        /// Sets the 'sqlConnectionString' attribute.
        /// </summary>
        /// <param name="sqlConnectionString"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator SqlConnectionString(string sqlConnectionString)
        {
            return SetAttributeValue("sqlConnectionString", sqlConnectionString);
        }

        /// <summary>
        /// Sets the 'sqlCommandTimeout' attribute.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator SqlCommandTimeout(TimeSpan? timeout)
        {
            return SetAttributeValue("sqlCommandTimeout", timeout.HasValue ? ((int)timeout.Value.TotalSeconds).ToString() : null);
        }

        /// <summary>
        /// Sets the 'sqlCommandTimeout' attribute.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator SqlCommandTimeout(int seconds)
        {
            return SqlCommandTimeout(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// Sets the 'sqlConnectionRetryInterval' attribute.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator SqlConnectionRetryInterval(TimeSpan? interval)
        {
            return SetAttributeValue("sqlConnectionRetryInterval", interval.HasValue ? ((int)interval.Value.TotalSeconds).ToString() : null);
        }

        /// <summary>
        /// Sets the 'sqlConnectionRetryInterval' attribute.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator SqlConnectionRetryInterval(int seconds)
        {
            return SqlConnectionRetryInterval(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// Sets the 'customProvider' attribute.
        /// </summary>
        /// <param name="customProvider"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator CustomProvider(string customProvider)
        {
            return SetAttributeValue("customProvider", customProvider);
        }

        /// <summary>
        /// Sets the 'cookieless' attribute.
        /// </summary>
        /// <param name="cookieless"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator Cookieless(WebSystemWebSessionStateCookieless? cookieless)
        {
            return SetAttributeValue("cookieless", cookieless.HasValue ? Enum.GetName(typeof(WebSystemWebSessionStateCookieless), cookieless.Value) : null);
        }

        /// <summary>
        /// Sets the 'cookieName' attribute.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator CookieName(string cookieName)
        {
            return SetAttributeValue("cookieName", cookieName);
        }

        /// <summary>
        /// Sets the 'allowCustomSqlDatabase' attribute.
        /// </summary>
        /// <param name="allowCustomSqlDatabase"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator AllowCustomSqlDatabase(bool? allowCustomSqlDatabase)
        {
            return SetAttributeValue("allowCustomSqlDatabase", allowCustomSqlDatabase?.ToString());
        }

        /// <summary>
        /// Sets the 'compressionEnabled' attribute.
        /// </summary>
        /// <param name="compressionEnabled"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator CompressionEnabled(bool? compressionEnabled)
        {
            return SetAttributeValue("compressionEnabled", compressionEnabled?.ToString());
        }

        /// <summary>
        /// Sets the 'timeout' attribute.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator Timeout(TimeSpan? timeout)
        {
            return SetAttributeValue("timeout", timeout.HasValue ? ((int)timeout.Value.TotalMinutes).ToString() : null);
        }

        /// <summary>
        /// Sets the 'timeout' attribute.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator Timeout(int seconds)
        {
            return Timeout(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// Sets the 'partitionResolverType' attribute.
        /// </summary>
        /// <param name="partitionResolverType"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator PartitionResolverType(string partitionResolverType)
        {
            return SetAttributeValue("partitionResolverType", partitionResolverType);
        }

        /// <summary>
        /// Sets the 'partitionResolverType' attribute.
        /// </summary>
        /// <param name="partitionResolverType"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator PartitionResolverType(Type partitionResolverType)
        {
            return PartitionResolverType(partitionResolverType?.AssemblyQualifiedName);
        }

        /// <summary>
        /// Sets the 'useHostingIdentity' attribute.
        /// </summary>
        /// <param name="useHostingIdentity"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator UseHostingIdentity(bool? useHostingIdentity)
        {
            return SetAttributeValue("useHostingIdentity", useHostingIdentity?.ToString());
        }

        /// <summary>
        /// Sets the 'sessionIDManagerType' attribute.
        /// </summary>
        /// <param name="sessionIDManagerType"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator SessionIDManagerType(string sessionIDManagerType)
        {
            return SetAttributeValue("sessionIDManagerType", sessionIDManagerType);
        }

        /// <summary>
        /// Sets the 'sessionIDManagerType' attribute.
        /// </summary>
        /// <param name="sessionIDManagerType"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator SessionIDManagerType(Type sessionIDManagerType)
        {
            return SessionIDManagerType(sessionIDManagerType?.AssemblyQualifiedName);
        }

        /// <summary>
        /// Sets the 'cookieSameSite' attribute.
        /// </summary>
        /// <param name="cookieSameSite"></param>
        /// <returns></returns>
        public WebSystemWebSessionStateConfigurator CookieSameSite(WebSystemWebSessionStateCookieSameSite? cookieSameSite)
        {
            return SetAttributeValue("cookieSameSite", cookieSameSite.HasValue ? Enum.GetName(typeof(WebSystemWebSessionStateCookieSameSite), cookieSameSite.Value) : null);
        }

    }

}
