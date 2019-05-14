<#
.SYNOPSIS
    .
.DESCRIPTION
    That script transform the current jar project version 8 into dll file
.PARAMETER path
    The path to jar file ex: C:\Users\user1\Desktop\project.jar
.NOTES
    Author: Paulo Lima
    Date:   May 07, 2019
#>
param (
    [Parameter(Mandatory=$true)][string]$path="",
    [string]$dependencies=""
)

$invocation = (Get-Variable MyInvocation).Value 
$scriptPath = Split-Path $invocation.MyCommand.Path
$urlDownloadIKVM = 'http://www.frijters.net/ikvmbin-8.1.5717.0.zip'
$targetFolder = $scriptPath + '\target'
$targetDLLFolder = $targetFolder + '\dll'
$ikvmUnzipFolder = 'ikvm-8.1.5717.0'
$ikvmZipName = 'ikvm8.zip'

function Download-File {
    Param ([string]$url,[string]$location)
    Write-Output "Download file from " $url
    $WebClient = New-Object System.Net.WebClient
    $uri = New-Object System.Uri($url)
    $WebClient.DownloadFile($uri,$location)
    $output = 'Download complete, file created: ' + $location
    Write-Output $output
}

<# This function create all folder if doesn't exist #>
function Create-Folders {
    Param ([string]$path)
    New-Item -ItemType Directory -Force -Path $path
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip
{
    param([string]$zipfile, [string]$outpath)

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

$ikvmcFile = "$($targetFolder)\$($ikvmUnzipFolder)\bin\ikvmc.exe"
#Download IKVM if not present
if(![System.IO.File]::Exists($ikvmcFile)){
    #Download and extract ikvmc
    Create-Folders -path $targetFolder
    $ikvmDownloadLocation = $targetFolder + '\' + $ikvmZipName
    Download-File -url $urlDownloadIKVM -location $ikvmDownloadLocation
    Unzip $ikvmDownloadLocation $targetFolder + '\ikvm8'
}

.\target\ikvm-8.1.5717.0\bin\ikvmc.exe -target:library $path -recurse:$dependencies -out:.\target\commons-ip-1.0.3.dll

#$scriptPath
#$scriptName

<#Remove-Item -path $ikvmDownloadLocation -Confirm:$false
Remove-Item -path  "$($targetFolder)\$($ikvmUnzipFolder)" -Confirm:$false#>


