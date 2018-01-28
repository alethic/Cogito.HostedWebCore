# Cogito.HostedWebCore
Mechanism to launch the IIS Hosted Web Core.

https://www.nuget.org/packages/Cogito.HostedWebCore


```
new AppHostBuilder()
    .UseLogger(logger)
    .ConfigureWeb(typeof(AppHostBuilder).Assembly.GetManifestResourceStream("Web.config"), w => w
        .Element.Element("system.web")
            .Element("compilation")
                .SetAttributeValue("tempDirectory", Environment.ExpandEnvironmentVariables(@"%TEMP%\Temporary ASP.NET Files")))
    .ConfigureApp(typeof(AppHostBuilder).Assembly.GetManifestResourceStream("ApplicationHost.config"), h => h
        .Site(1, s => s
            .RemoveBindings()
            .AddBinding(protocol, bindingInformation)
            .Application("/", a => a
                .VirtualDirectory("/", v => v
                    .UsePhysicalPath(NormalizePath(@"C:\Path\To\WebApp1")))
                .VirtualDirectory("/Temp", v => v
                    .UsePhysicalPath(NormalizePath(config?.TempPath ?? throw new Exception("Missing Web TempPath configuration.")))))
            .Application("/VApp1", a => a
                .VirtualDirectory("/", v => v
                    .UsePhysicalPath(NormalizePath(@"C:\Path\To\WebApp2"))))
            .Application("/VApp2", a => a
                .VirtualDirectory("/", v => v
                    .UsePhysicalPath(NormalizePath(@"C:\Path\To\WebAPp3"))))));
```
