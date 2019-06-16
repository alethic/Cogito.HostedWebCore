using System;
using System.Linq;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    public static class XElementExtensions
    {

        /// <summary>
        /// Returns the element with the specified name, or creates and adds one if required.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XElement ElementOrAdd(this XContainer parent, XName name)
        {
            var e = parent.Element(name);
            if (e == null)
                parent.Add(e = new XElement(name));

            return e;
        }

        /// <summary>
        /// Returns the element with the specified name, or creates and adds one if required.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XElement ElementOrAdd(this XContainer parent, XName name, Func<XElement, bool> predicate)
        {
            var e = parent.Elements(name).FirstOrDefault(predicate);
            if (e == null)
                parent.Add(e = new XElement(name));

            return e;
        }

    }

}
