using System;

namespace Cogito.IIS.Configuration
{

    public class AppHostConfigurationException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public AppHostConfigurationException(string message) :
            base(message)
        {

        }

    }

}
