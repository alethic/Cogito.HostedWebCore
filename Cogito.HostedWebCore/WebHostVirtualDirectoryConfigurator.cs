using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Cogito.HostedWebCore
{

    public class WebHostVirtualDirectoryConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebHostVirtualDirectoryConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Configures this virtual directory at the specified path.
        /// </summary>
        /// <param name="physicalPath"></param>
        /// <returns></returns>
        public WebHostVirtualDirectoryConfigurator UsePhysicalPath(string physicalPath)
        {
            if (string.IsNullOrWhiteSpace(physicalPath))
                throw new ArgumentException(nameof(physicalPath));

            // build absolute path from current working directory
            if (Path.IsPathRooted(physicalPath) == false)
                physicalPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), physicalPath);

            if (Directory.Exists(physicalPath) == false)
                throw new DirectoryNotFoundException();

            element.SetAttributeValue("physicalPath", physicalPath);
            return this;
        }

    }

}
