using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Cogito.HostedWebCore
{

    /// <summary>
    /// Provides operations to configure, start and stop the hosted IIS web server.
    /// </summary>
    public static class WebServer
    {

        static readonly string HWEBCORE = Environment.ExpandEnvironmentVariables(@"%WINDIR%\system32\inetsrv\hwebcore.dll");
        static readonly string SYSTEM_WEB_CONFIG = Environment.ExpandEnvironmentVariables(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), @"config\web.config"));

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static WebServer()
        {
            ApplicationHostConfigPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".config");
            RootWebConfigPath = Environment.ExpandEnvironmentVariables(SYSTEM_WEB_CONFIG);
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

            if (File.Exists(HWEBCORE) == false)
                throw new FileNotFoundException("Unable to find IIS Hostable Web Core entry point. Ensure IIS Hostable Web Core is installed.");
            if (File.Exists(SYSTEM_WEB_CONFIG) == false)
                throw new FileNotFoundException("Unable to find default system Web.config file.");
            if (File.Exists(ApplicationHostConfigPath) == false)
                throw new FileNotFoundException("Cannot find ApplicationHostConfigPath file.");
            if (File.Exists(RootWebConfigPath) == false)
                throw new FileNotFoundException("Cannot find RootWebConfigPath file.");

            ValidateFilesExist(
                "configuration/system.webServer/globalModules",
                XDocument.Load(ApplicationHostConfigPath)
                    .Elements("configuration")
                    .Elements("system.webServer")
                    .Elements("globalModules")
                    .Elements("add")
                    .Select(i => ((string)i.Attribute("name"), (string)i.Attribute("image")))
                    .Where(i => !string.IsNullOrWhiteSpace(i.Item2)));

            ValidateFilesExist(
                "configuration/system.webServer/isapiFilters",
                XDocument.Load(ApplicationHostConfigPath)
                    .Elements("configuration")
                    .Elements("system.webServer")
                    .Elements("isapiFilters")
                    .Elements("filter")
                    .Select(i => ((string)i.Attribute("name"), (string)i.Attribute("path")))
                    .Where(i => !string.IsNullOrWhiteSpace(i.Item2)));

            ValidateFilesExist(
                "configuration/location/system.webServer/handlers",
                XDocument.Load(ApplicationHostConfigPath)
                    .Elements("configuration")
                    .Elements("location")
                    .Elements("system.webServer")
                    .Elements("handlers")
                    .Elements("add")
                    .Select(i => ((string)i.Attribute("name"), (string)i.Attribute("scriptProcessor")))
                    .Where(i => !string.IsNullOrWhiteSpace(i.Item2)));

            HostedWebCoreInternal.Activate(
                ApplicationHostConfigPath,
                RootWebConfigPath,
                InstanceName);
        }

        /// <summary>
        /// Validate that each of the specified files exists.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="namesAndPaths"></param>
        static void ValidateFilesExist(string category, IEnumerable<(string Name, string Path)> namesAndPaths)
        {
            foreach (var kvp in namesAndPaths)
                if (File.Exists(Environment.ExpandEnvironmentVariables(kvp.Path)) == false)
                    throw new FileNotFoundException($"Validating {category}. Missing file for {kvp.Name}.", kvp.Path);
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
                var hwc = NativeMethods.LoadLibrary(HWEBCORE);
                if (hwc == IntPtr.Zero)
                    throw new InvalidOperationException("Error returned from LoadLibrary for Hosted Web Core.");

                WebCoreActivate = (FnWebCoreActivate)Marshal.GetDelegateForFunctionPointer(
                    NativeMethods.GetProcAddress(hwc, "WebCoreActivate"),
                    typeof(FnWebCoreActivate));

                WebCoreShutdown = (FnWebCoreShutdown)Marshal.GetDelegateForFunctionPointer(
                    NativeMethods.GetProcAddress(hwc, "WebCoreShutdown"),
                    typeof(FnWebCoreShutdown));
            }

            /// <summary>
            /// Specifies if Hostable WebCore ha been activated
            /// </summary>
            public static bool IsActivated => isActiviated;

            /// <summary>
            /// Activate the HWC.
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
            /// Shutdown HWC.
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
