using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Describes a configurator that operates against a known element.
    /// </summary>
    public interface IWebElementConfigurator
    {

        /// <summary>
        /// Gets the root configuration element.
        /// </summary>
        XElement Element { get; }

    }

}
