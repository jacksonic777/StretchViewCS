[CmdletBinding()]
param(
    [string]$Configuration = "Release",
    [string]$Platform = "Any CPU"
)

$ErrorActionPreference = "Stop"

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

function Ensure-FileExists {
    param(
        [string]$Path
    )

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Required file was not found: $Path"
    }
}

function Ensure-DirectoryExists {
    param(
        [string]$Path
    )

    if (-not (Test-Path -LiteralPath $Path -PathType Container)) {
        throw "Required directory was not found: $Path"
    }
}

function Invoke-ReleaseBuild {
    param(
        [string]$RepoRoot,
        [string]$Configuration,
        [string]$Platform
    )

    $msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    Ensure-FileExists -Path $msbuildPath

    $solutionPath = Join-Path $RepoRoot "StretchViewCS.sln"
    Ensure-FileExists -Path $solutionPath

    $msbuildArguments = @(
        $solutionPath
        "/t:Build"
        "/p:Configuration=$Configuration"
        "/p:Platform=$Platform"
    )

    & $msbuildPath @msbuildArguments
    if ($LASTEXITCODE -ne 0) {
        throw "Release build failed."
    }
}

function New-ReleasePackage {
    param(
        [string]$RepoRoot,
        [string]$Configuration
    )

    $projectRoot = Join-Path $RepoRoot "StretchViewCS\StretchViewCS"
    $outputRoot = Join-Path $projectRoot "bin\$Configuration\net48"
    $assemblyInfoPath = Join-Path $projectRoot "Properties\AssemblyInfo.cs"
    $version = Get-AssemblyVersion -AssemblyInfoPath $assemblyInfoPath

    $stageRoot = Join-Path $RepoRoot "artifacts\release"
    $packageDirectoryName = "StretchViewCS-$version"
    $packageDirectory = Join-Path $stageRoot $packageDirectoryName
    $zipPath = Join-Path $stageRoot ($packageDirectoryName + ".zip")

    Ensure-DirectoryExists -Path $outputRoot
    Ensure-FileExists -Path (Join-Path $outputRoot "StretchViewCS.exe")
    Ensure-FileExists -Path (Join-Path $outputRoot "StretchViewCS.exe.config")
    Ensure-FileExists -Path (Join-Path $outputRoot "System.Configuration.ConfigurationManager.dll")
    Ensure-FileExists -Path (Join-Path $outputRoot "appIcon.png")
    Ensure-DirectoryExists -Path (Join-Path $outputRoot "help")
    Ensure-FileExists -Path (Join-Path $outputRoot "ReadMe.ja.txt")
    Ensure-FileExists -Path (Join-Path $outputRoot "ReadMe.en.txt")
    Ensure-FileExists -Path (Join-Path $outputRoot "README.ja.md")
    Ensure-FileExists -Path (Join-Path $outputRoot "README.en.md")

    if (Test-Path -LiteralPath $packageDirectory) {
        Remove-Item -LiteralPath $packageDirectory -Recurse -Force
    }

    if (Test-Path -LiteralPath $zipPath) {
        Remove-Item -LiteralPath $zipPath -Force
    }

    New-Item -ItemType Directory -Path $packageDirectory -Force | Out-Null

    Copy-Item -LiteralPath (Join-Path $outputRoot "StretchViewCS.exe") -Destination $packageDirectory
    Copy-Item -LiteralPath (Join-Path $outputRoot "StretchViewCS.exe.config") -Destination $packageDirectory
    Copy-Item -LiteralPath (Join-Path $outputRoot "System.Configuration.ConfigurationManager.dll") -Destination $packageDirectory
    Copy-Item -LiteralPath (Join-Path $outputRoot "appIcon.png") -Destination $packageDirectory
    Copy-Item -LiteralPath (Join-Path $outputRoot "help") -Destination $packageDirectory -Recurse
    Copy-Item -LiteralPath (Join-Path $outputRoot "ReadMe.ja.txt") -Destination $packageDirectory
    Copy-Item -LiteralPath (Join-Path $outputRoot "ReadMe.en.txt") -Destination $packageDirectory
    Copy-Item -LiteralPath (Join-Path $outputRoot "README.ja.md") -Destination $packageDirectory
    Copy-Item -LiteralPath (Join-Path $outputRoot "README.en.md") -Destination $packageDirectory

    Compress-Archive -Path (Join-Path $packageDirectory "*") -DestinationPath $zipPath -CompressionLevel Optimal

    Write-Output "PackageDirectory=$packageDirectory"
    Write-Output "ZipPath=$zipPath"
}

$repoRoot = Get-RepoRoot
Invoke-ReleaseBuild -RepoRoot $repoRoot -Configuration $Configuration -Platform $Platform
New-ReleasePackage -RepoRoot $repoRoot -Configuration $Configuration

