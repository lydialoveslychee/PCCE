@echo off
if "%~1"=="" echo Please provide the path to scan. & pause & exit
set "dir=%~1"
echo Scanning directory: %dir%
if not exist "%dir%" echo Path does not exist. & pause & exit
for /d %%a in ("%dir%\*-*") do (
    if /i "%%~na" neq "en-us" (
        echo Deleting directory: %%a
        rmdir /s /q "%%a"
    )
)
echo Done.
pause