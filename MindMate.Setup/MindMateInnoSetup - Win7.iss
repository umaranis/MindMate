; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "MindMate"
#define MyAppVersion "0.8"
#define MyAppURL "http://www.umaranis.com/"
#define MyAppExeName "MindMate.Win.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{D7CB0669-98E8-4AA3-914A-C590AF80C52C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=..\MindMate\LICENSE.txt
OutputBaseFilename=MindMate-Setup-Win7-v{#MyAppVersion}
Compression=lzma
SolidCompression=yes
AppPublisher=Syed Umar Anis

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\MindMate.Win7\bin\RelWin7\MindMate.Win.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MindMate.Win7\bin\RelWin7\images\*"; DestDir: "{app}\images"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "..\MindMate.Win7\bin\RelWin7\CustomFontDialog.dll"; DestDir: "{app}"
Source: "..\MindMate.Win7\bin\RelWin7\MindMate.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MindMate.Win7\bin\RelWin7\Ribbon.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MindMate.Win7\bin\RelWin7\Settings.Yaml"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MindMate.Win7\bin\RelWin7\System.Windows.Forms.Calendar.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MindMate.Win7\bin\RelWin7\TaskScheduler.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MindMate.Win7\bin\RelWin7\YamlDotNet.dll"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

