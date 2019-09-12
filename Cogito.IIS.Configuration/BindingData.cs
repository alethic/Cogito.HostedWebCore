namespace Cogito.IIS.Configuration
{

    /// <summary>
    /// Describes the set of IIS binding information.
    /// </summary>
    public class BindingData
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public BindingData()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="bindingInformation"></param>
        public BindingData(string protocol, string bindingInformation)
        {
            Protocol = protocol;
            BindingInformation = bindingInformation;
        }

        /// <summary>
        /// Describes the protocol.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Describes the IIS binding information.
        /// </summary>
        public string BindingInformation { get; set; }

    }

}
