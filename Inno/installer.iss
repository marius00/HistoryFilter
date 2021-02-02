#define ApplicationVersion 1

[Setup]
AppVerName=HistoryFilter
AppName=HistoryFilter (c) EvilSoft
VersionInfoVersion=1.0
AppId=historyfilter
DefaultDirName={code:DefDirRoot}\HistoryFilter
Uninstallable=Yes
OutputDir=..\Installer
SetupIconFile=..\download.ico


[Tasks]
Name: desktopicon; Description: "Create a &desktop icon"; GroupDescription: "Icons:"
Name: starticon; Description: "Create a &startmenu icon"; GroupDescription: "Icons:"


[Icons]
Name: "{commonprograms}\HistoryFilter"; Filename: "{app}\\HistoryFilter.exe"; Tasks: starticon
Name: "{commondesktop}\HistoryFilter"; Filename: "{app}\\HistoryFilter.exe"; Tasks: desktopicon


[Files]
Source: "..\bin\Release\*"; Excludes: "*.pdb"; DestDir: "{app}"; Flags: overwritereadonly replacesameversion recursesubdirs createallsubdirs touch ignoreversion

[Run]
Filename: "{app}\HistoryFilter.exe"; Description: "Launch HistoryFilter"; Flags: postinstall nowait
 


[Setup]
UseSetupLdr=yes
DisableProgramGroupPage=yes
DiskSpanning=no
AppVersion={#ApplicationVersion}
VersionInfoProductTextVersion={#ApplicationVersion}
PrivilegesRequired=admin
DisableWelcomePage=Yes
ArchitecturesInstallIn64BitMode=x64
AlwaysShowDirOnReadyPage=Yes
DisableDirPage=No
OutputBaseFilename=HistoryFilterInstaller


[UninstallDelete]
Type: filesandordirs; Name: {app}

[Languages]
Name: eng; MessagesFile: compiler:Default.isl

[Code]
function IsRegularUser(): Boolean;
begin
Result := not (IsAdminLoggedOn or IsPowerUserLoggedOn);
end;

function DefDirRoot(Param: String): String;
begin
if IsRegularUser then
Result := ExpandConstant('{localappdata}')
else
Result := ExpandConstant('{pf}')
end;

