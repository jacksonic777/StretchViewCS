#define AppName "StretchViewCS"
#ifndef AppVersion
#define AppVersion "1.0.0.0"
#endif
#ifndef PackageDir
#define PackageDir "..\artifacts\release\StretchViewCS-1.0.0.0"
#endif
#ifndef OutputDir
#define OutputDir "..\artifacts\installer"
#endif

[Setup]
AppId={{6F95D01D-6F35-4A32-9ACF-4D6C49A7CC98}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=StretchViewCS
AppPublisherURL=
AppSupportURL=
AppUpdatesURL=
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputDir={#OutputDir}
OutputBaseFilename=StretchViewCS-Setup-{#AppVersion}
SetupIconFile=..\StretchViewCS\StretchViewCS\appIcon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64compatible
UninstallDisplayIcon={app}\StretchViewCS.exe

[Languages]
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#PackageDir}\StretchViewCS.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#PackageDir}\StretchViewCS.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#PackageDir}\System.Configuration.ConfigurationManager.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#PackageDir}\help\*"; DestDir: "{app}\help"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\StretchViewCS.exe"; WorkingDir: "{app}"
Name: "{group}\Uninstall {#AppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\StretchViewCS.exe"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\StretchViewCS.exe"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
