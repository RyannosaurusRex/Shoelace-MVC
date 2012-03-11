@echo off
cd %~dp0
cd ShoelaceMVC
REM create the project template zip
..\tools\7za.exe a -tzip -mx9 ..\ProjectTemplates\CSharp\Web\ShoelaceMVC.zip *
pushd %~dp0
REM create the VSIX file based on the zip and manifest data
tools\7za.exe a -tzip -mx9 ShoelaceMVC.vsix @vsixfiles.txt
REM delete the temporary zip
rmdir /s /q ProjectTemplates