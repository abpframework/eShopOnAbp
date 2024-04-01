@echo off
cls

set directories= bin, obj, logs, node_modules
set /a count = 0

echo Deleting all following directories:
for %%d in (%directories%) do echo %%d
echo.

FOR /d /r . %%d in (%directories%) DO if exist "%%d" (
    echo. Deleting: %%d
    rd /s/q "%%d"
    set /a count += 1
    )

echo.
echo The specified directories were successfully scanned.
echo Number of deleted directories: %count%
echo Press any key to exit.
pause > nul
