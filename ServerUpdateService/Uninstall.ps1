﻿$serviceName = "ARKServerUpdateService.exe"

# verify if the service already exists, and if yes remove it
if (Get-Service $serviceName -ErrorAction SilentlyContinue)
{
    "service already installed, stopping…"
    # using WMI to remove Windows service because PowerShell does not have CmdLet for this
    $WMIFilter = "name='$serviceName'"
    $serviceToRemove = Get-WmiObject -Class Win32_Service -Filter $WMIFilter
    $serviceToRemove | Stop-Service
    $serviceToRemove.delete()
    "service removed"
}
else
{
    # just do nothing
    "service does not exist"
}

Pause
