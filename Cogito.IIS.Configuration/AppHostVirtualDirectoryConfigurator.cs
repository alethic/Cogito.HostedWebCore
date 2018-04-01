using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    public class AppHostVirtualDirectoryConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostVirtualDirectoryConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        /// <summary>
        /// Configures this virtual directory to point to the specified physical path.
        /// </summary>
        /// <param name="physicalPath"></param>
        /// <returns></returns>
        public AppHostVirtualDirectoryConfigurator UsePhysicalPath(string physicalPath)
        {
            if (string.IsNullOrWhiteSpace(physicalPath))
                throw new ArgumentException(nameof(physicalPath));

            // build absolute path from current working directory
            if (Path.IsPathRooted(physicalPath) == false)
                physicalPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), physicalPath);

            element.SetAttributeValue("physicalPath", physicalPath);
            return this;
        }

    }

}
