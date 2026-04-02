; DartMaster Installer Script for NSIS
; Generated for Monarch73

!include "MUI2.nsh"

; General
Name "DartMaster"
OutFile "DartMasterInstaller.exe"
InstallDir "$APPDATA\DartMaster"
InstallDirRegKey HKCU "Software\DartMaster" "Install_Dir"

; Request application privileges for Windows Vista and higher
RequestExecutionLevel user

; Interface Settings
!define MUI_ABORTWARNING
!define MUI_ICON "bin\Release\net9.0-windows10.0.26100.0\win-x64\publish\appicon.ico"
!define MUI_UNICON "bin\Release\net9.0-windows10.0.26100.0\win-x64\publish\appicon.ico"

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
    File /r "bin\Release\net9.0-windows10.0.26100.0\win-x64\publish\*"
    
    ; Create uninstaller
    WriteUninstaller "$INSTDIR\Uninstall.exe"
    
    ; Registry keys for uninstallation
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartMaster" "DisplayName" "DartMaster"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartMaster" "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartMaster" "DisplayIcon" "$INSTDIR\DartMaster.exe"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartMaster" "Publisher" "Monarch73"

    ; Create Start Menu shortcuts
    CreateDirectory "$SMPROGRAMS\DartMaster"
    CreateShortcut "$SMPROGRAMS\DartMaster\DartMaster.lnk" "$INSTDIR\DartMaster.exe" "" "$INSTDIR\appicon.ico"
    CreateShortcut "$SMPROGRAMS\DartMaster\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
SectionEnd

; Uninstaller Section
Section "Uninstall"
    Delete "$SMPROGRAMS\DartMaster\DartMaster.lnk"
    Delete "$SMPROGRAMS\DartMaster\Uninstall.lnk"
    RMDir "$SMPROGRAMS\DartMaster"
    
    Delete "$INSTDIR\Uninstall.exe"
    RMDir /r "$INSTDIR"
    
    DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\DartMaster"
    DeleteRegKey HKCU "Software\DartMaster"
SectionEnd
