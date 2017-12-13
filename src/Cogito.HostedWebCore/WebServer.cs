using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Cogito.HostedWebCore
{

    /// <summary>
    /// Provides operations to configure, start and stop the hosted IIS web server.
    /// </summary>
    public static class WebServer
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static WebServer()
        {
            ApplicationHostConfigPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".config");
            RootWebConfigPath = Environment.ExpandEnvironmentVariables(@"%WINDIR%\Microsoft.Net\Framework\v4.0.30319\config\web.config");
            InstanceName = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Path to the applicationHost.config file used to configure the IIS instance. The default value of this
        /// property is a temporary file which can be initialized with the desired file contents.
        /// </summary>
        public static string ApplicationHostConfigPath { get; set; }

        /// <summary>
        /// Path to the Web.config file that serves as the root in the Web.config hierarchy.
        /// </summary>
        public static string RootWebConfigPath { get; set; }

        /// <summary>
        /// Machine-unique instance name.
        /// </summary>
        public static string InstanceName { get; set; }

        /// <summary>
        /// Returns <c>true</c> if the hosted web core is currently activated.
        /// </summary>
        public static bool IsActivated => HostedWebCoreInternal.IsActivated;

        /// <summary>
        /// Starts the hosted web server. 
        /// </summary>
        public static void Start()
        {
            if (ApplicationHostConfigPath == null || ApplicationHostConfigPath == "")
                throw new InvalidOperationException("ApplicationHostConfigPath cannot be blank.");
            if (RootWebConfigPath == null || RootWebConfigPath == "")
                throw new InvalidOperationException("RootWebConfigPath cannot be blank.");
            if (InstanceName == null || InstanceName == "")
                throw new InvalidOperationException("InstanceName cannot be blank.");
            if (HostedWebCoreInternal.IsActivated)
                throw new InvalidOperationException("Hostable Web Core is already running.");

            if (File.Exists(ApplicationHostConfigPath) == false)
                throw new FileNotFoundException("Cannot find ApplicationHostConfigPath file.");
            if (File.Exists(RootWebConfigPath) == false)
                throw new FileNotFoundException("Cannot find RootWebConfigPath file.");

            HostedWebCoreInternal.Activate(ApplicationHostConfigPath, RootWebConfigPath, InstanceName);
        }

        /// <summary>
        /// Stops the hosted web server.
        /// </summary>
        public static void Stop()
        {
            if (HostedWebCoreInternal.IsActivated)
                HostedWebCoreInternal.Shutdown(false);
        }

        /// <summary>
        /// Interacts with the native HWEBCORE library.
        /// </summary>
        static class HostedWebCoreInternal
        {

            static bool isActiviated;

            delegate int FnWebCoreActivate([In, MarshalAs(UnmanagedType.LPWStr)]string appHostConfig, [In, MarshalAs(UnmanagedType.LPWStr)]string rootWebConfig, [In, MarshalAs(UnmanagedType.LPWStr)]string instanceName);
            delegate int FnWebCoreShutdown(bool immediate);

            static FnWebCoreActivate WebCoreActivate;
            static FnWebCoreShutdown WebCoreShutdown;

            /// <summary>
            /// Initializes the static instance.
            /// </summary>
            static HostedWebCoreInternal()
            {
                // Load the library and get the function pointers for the WebCore entry points
                const string HWCPath = @"%windir%\system32\inetsrv\hwebcore.dll";
                IntPtr hwc = NativeMethods.LoadLibrary(Environment.ExpandEnvironmentVariables(HWCPath));

                IntPtr procaddr = NativeMethods.GetProcAddress(hwc, "WebCoreActivate");
                WebCoreActivate = (FnWebCoreActivate)Marshal.GetDelegateForFunctionPointer(procaddr, typeof(FnWebCoreActivate));

                procaddr = NativeMethods.GetProcAddress(hwc, "WebCoreShutdown");
                WebCoreShutdown = (FnWebCoreShutdown)Marshal.GetDelegateForFunctionPointer(procaddr, typeof(FnWebCoreShutdown));
            }

            /// <summary>
            /// Specifies if Hostable WebCore ha been activated
            /// </summary>
            public static bool IsActivated => isActiviated;

            /// <summary>
            /// Activate the HWC
            /// </summary>
            /// <param name="appHostConfig">Path to ApplicationHost.config to use</param>
            /// <param name="rootWebConfig">Path to the Root Web.config to use</param>
            /// <param name="instanceName">Name for this instance</param>
            public static void Activate(string appHostConfig, string rootWebConfig, string instanceName)
            {
                int result = WebCoreActivate(appHostConfig, rootWebConfig, instanceName);
                if (result != 0)
                    Marshal.ThrowExceptionForHR(result);

                isActiviated = true;
            }

            /// <summary>
            /// Shutdown HWC
            /// </summary>
            public static void Shutdown(bool immediate)
            {
                if (isActiviated)
                {
                    WebCoreShutdown(immediate);
                    isActiviated = false;
                }
            }

            static class NativeMethods
            {

                [DllImport("kernel32.dll")]
                internal static extern IntPtr LoadLibrary(String dllname);

                [DllImport("kernel32.dll")]
                internal static extern IntPtr GetProcAddress(IntPtr hModule, String procname);

            }

        }

    }

}
