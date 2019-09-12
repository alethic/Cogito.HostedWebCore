Set-PSDebug -Trace 2

$ErrorActionPreference = 'Stop'
$p = [int]::Parse($env:Fabric_Endpoint_ServiceEndpoint)
$n = 'WebUser'

$ErrorActionPreference = 'SilentlyContinue'
& netsh http delete urlacl url=http://*:$p/ | Out-Null
& netsh http delete urlacl url=http://+:$p/ | Out-Null
& netsh http delete urlacl url=http://localhost:$p/ | Out-Null

$ErrorActionPreference = 'Stop'
Test-Path $env:Fabric_Folder_Application\Application.Sids.txt
$s = $($h=@{};(gc $env:Fabric_Folder_Application\Application.Sids.txt).Split(',')|%{$l=$_.Split(':');$h[$l[0]]=$l[1].Trim()};$h[$n])
$o = New-Object System.Security.Principal.SecurityIdentifier $s
$u = $o.Translate([System.Security.Principal.NTAccount]).Value

Write-Host "Adding URL reservation: url=http://*:$p/ user=$u"
& netsh http add urlacl url=http://*:$p/ user=$u

Write-Host "Running aspnet_regiis for $u"
& $env:windir\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -ga $u
& $env:windir\Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe -ga $u

foreach ($reg in @(
    "HKLM:\SOFTWARE\Microsoft\ASP.NET\4.0.30319.0\CompilationMutexName",
    "HKLM:\SOFTWARE\WOW6432Node\Microsoft\ASP.NET\4.0.30319.0\CompilationMutexName")) {
    Write-Host "Setting registry permissions: $reg"
    $acl = Get-Acl $reg
    $rule = New-Object System.Security.AccessControl.RegistryAccessRule ($u, "FullControl", "Allow")
    $acl.SetAccessRule($rule)
    $acl | Set-Acl -Path $reg
}
