[CmdletBinding()]
param(
    [string]$Configuration = "Release",
    [string]$Platform = "Any CPU"
)

$ErrorActionPreference = "Stop"

trap {
    Write-Error $_
    exit 1
}

function Get-RepoRoot {
    $scriptDirectory = Split-Path -Parent $PSCommandPath
    return [System.IO.Path]::GetFullPath((Join-Path $scriptDirectory ".."))
}

function Get-AssemblyVersion {
    param(
        [string]$AssemblyInfoPath
    )

    $match = Select-String -Path $AssemblyInfoPath -Pattern 'AssemblyFileVersion\("([^"]+)"\)'
    if ($null -eq $match) {
        throw "AssemblyFileVersion was not found: $AssemblyInfoPath"
    }

    return $match.Matches[0].Groups[1].Value
}

function Find-InnoCompiler {
    $command = Get-Command "iscc.exe" -ErrorAction SilentlyContinue
    if ($null -ne $command) {
        return $command.Source
    }

    $candidatePaths = @(
        "C:\Program Files (x86)\Inno Setup 6\ISCC.exe",
        "C:\Program Files\Inno Setup 6\ISCC.exe",
        (Join-Path $env:LOCALAPPDATA "Programs\Inno Setup 6\ISCC.exe")
    )

    foreach ($candidatePath in $candidatePaths) {
        if (Test-Path -LiteralPath $candidatePath -PathType Leaf) {
            return $candidatePath
        }
    }

    throw "ISCC.exe was not found. Install Inno Setup 6 or add ISCC.exe to PATH."
}

function Invoke-ReleasePackage {
    param(
        [string]$RepoRoot,
        [string]$Configuration,
        [string]$Platform
    )

    $releaseScriptPath = Join-Path $RepoRoot "tools\Create-ReleasePackage.ps1"
    if (-not (Test-Path -LiteralPath $releaseScriptPath -PathType Leaf)) {
        throw "Release package script was not found: $releaseScriptPath"
    }

    & powershell -NoProfile -ExecutionPolicy Bypass -File $releaseScriptPath -Configuration $Configuration -Platform $Platform
    if ($LASTEXITCODE -ne 0) {
        throw "Release package creation failed."
    }
}

$repoRoot = Get-RepoRoot
$projectRoot = Join-Path $repoRoot "StretchViewCS\StretchViewCS"
$assemblyInfoPath = Join-Path $projectRoot "Properties\AssemblyInfo.cs"
$version = Get-AssemblyVersion -AssemblyInfoPath $assemblyInfoPath
$packageDirectory = Join-Path $repoRoot ("artifacts\release\StretchViewCS-" + $version)
$installerOutputDirectory = Join-Path $repoRoot "artifacts\installer"
$issPath = Join-Path $repoRoot "installer\StretchViewCS.iss"

Invoke-ReleasePackage -RepoRoot $repoRoot -Configuration $Configuration -Platform $Platform

if (-not (Test-Path -LiteralPath $packageDirectory -PathType Container)) {
    throw "Package directory was not found: $packageDirectory"
}

if (-not (Test-Path -LiteralPath $installerOutputDirectory -PathType Container)) {
    New-Item -ItemType Directory -Path $installerOutputDirectory -Force | Out-Null
}

$isccPath = Find-InnoCompiler
$isccArguments = @(
    "/DAppVersion=$version",
    "/DPackageDir=$packageDirectory",
    "/DOutputDir=$installerOutputDirectory",
    $issPath
)

& $isccPath @isccArguments
if ($LASTEXITCODE -ne 0) {
    throw "Inno Setup compiler failed."
}

$setupPath = Join-Path $installerOutputDirectory ("StretchViewCS-Setup-" + $version + ".exe")
Write-Output "InstallerPath=$setupPath"
