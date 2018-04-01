using System;
using System.Linq;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    public static class WebElementConfiguratorExtensions
    {

        /// <summary>
        /// Configures the current element.
        /// </summary>
        /// <typeparam name="TWeb"></typeparam>
        /// <param name="self"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static TWeb Configure<TWeb>(this TWeb self, Action<XElement> configure)
            where TWeb : IWebElementConfigurator
        {
            configure?.Invoke(self.Element);
            return self;
        }

        /// <summary>
        /// Configures the given named element.
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static TWeb Configure<TWeb>(this TWeb self, string elementName, Action<XElement> configure)
            where TWeb : IWebElementConfigurator
        {
            if (string.IsNullOrWhiteSpace(elementName))
                throw new ArgumentException(nameof(elementName));

            var e = self.Element
                .Elements(elementName)
                .FirstOrDefault();
            if (e == null)
                self.Element.Add(e = new XElement(elementName));

            configure?.Invoke(e);
            return self;
        }

    }

}
