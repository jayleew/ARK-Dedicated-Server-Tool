﻿$serviceName = "ARKServerUpdateService"
$serviceDisplayName = "ARKServerUpdateService"
$serviceDescription = "Handles auto-updates for ARK game server."
$serviceExecutable = "ARKServerUpdateService.exe"

# verify if the service already exists, and if yes remove it first
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

"installing service…"

# detect current execution directory
$directoryPath = Split-Path $MyInvocation.MyCommand.Path

# creating credentials which can be used to run my windows service
#$secpasswd = ConvertTo-SecureString "MyPa$$word" -AsPlainText -Force
#$mycreds = New-Object System.Management.Automation.PSCredential (".\MyUserName", $secpasswd)
# OR
#$myCredentials = Get-Credential

$binaryPath = $directoryPath + "\" + $serviceExecutable

# creating widnows service using all provided parameters, -displayName $serviceName, -credential $myCredentials
New-Service -name $serviceName -displayName $serviceDisplayName -binaryPathName $binaryPath -startupType Automatic -Description $serviceDescription

#Start-Service -Name $serviceName

Get-Service $serviceName

"installation completed"

Pause