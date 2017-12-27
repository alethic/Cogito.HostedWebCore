using System;

namespace Cogito.HostedWebCore
{

    public class AppHostException : 
        Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public AppHostException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public AppHostException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AppHostException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}