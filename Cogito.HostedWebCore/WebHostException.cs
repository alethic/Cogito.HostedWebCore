using System;

namespace Cogito.HostedWebCore
{

    public class WebHostException : 
        Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public WebHostException()
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public WebHostException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public WebHostException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

    }

}