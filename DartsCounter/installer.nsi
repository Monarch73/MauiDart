; DartsCounter Installer Script for NSIS
; Generated for Monarch73

!include "MUI2.nsh"

; General
Name "DartsCounter"
OutFile "DartsCounterInstaller.exe"
InstallDir "$APPDATA\DartsCounter"
InstallDirRegKey HKCU "Software\DartsCounter" "Install_Dir"

; Request application privileges for Windows Vista and higher
RequestExecutionLevel user

; Interface Settings
!define MUI_ABORTWARNING
!define MUI_ICON "bin\Release
et9.0-windows10.0.26100.0\win10-x64\publish\appicon.ico"
!define MUI_UNICON "bin\Release
et9.0-windows10.0.26100.0\win10-x64\publish\appicon.ico"

; Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

; Languages
!insertmacro MUI_LANGUAGE "English"

; Installer Sections
Section "MainSection" SEC01
    SetOutPath "$INSTDIR"
    File /r "bin\Release
et9.0-windows10.0.26100.0\win10-x64\publish\*"
    
    ; Create uninstaller
    WriteUninstaller "$INSTDIR\Uninstall.exe"
    
    ; Registry keys for uninstallation
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartsCounter" "DisplayName" "DartsCounter"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartsCounter" "UninstallString" "$"$INSTDIR\Uninstall.exe$""
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartsCounter" "DisplayIcon" "$INSTDIR\DartsCounter.exe"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartsCounter" "Publisher" "Monarch73"

    ; Create Start Menu shortcuts
    CreateDirectory "$SMPROGRAMS\DartsCounter"
    CreateShortcut "$SMPROGRAMS\DartsCounter\DartsCounter.lnk" "$INSTDIR\DartsCounter.exe" "" "$INSTDIR\appicon.ico"
    CreateShortcut "$SMPROGRAMS\DartsCounter\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
SectionEnd

; Uninstaller Section
Section "Uninstall"
    Delete "$SMPROGRAMS\DartsCounter\DartsCounter.lnk"
    Delete "$SMPROGRAMS\DartsCounter\Uninstall.lnk"
    RMDir "$SMPROGRAMS\DartsCounter"
    
    Delete "$INSTDIR\Uninstall.exe"
    RMDir /r "$INSTDIR"
    
    DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartsCounter"
    DeleteRegKey HKCU "Software\DartsCounter"
SectionEnd
