@echo off

setlocal

call ..\Certification\set_tokens.bat

set PROJECTNAME=UpdateCheck
set TOKEN=%UPDATE_CHECK_CODECOV_TOKEN%
set TESTPROJECTNAME=%PROJECTNAME%.Test
set RESULTFILENAME=Coverage-%PROJECTNAME%.xml

set OPENCOVER_VERSION=4.7.1221
set OPENCOVER=OpenCover.%OPENCOVER_VERSION%
set OPENCOVER_EXE=".\packages\%OPENCOVER%\tools\OpenCover.Console.exe"

set CODECOV_UPLOADER_VERSION=0.7.2
set CODECOV_UPLOADER=CodecovUploader.%CODECOV_UPLOADER_VERSION%
set CODECOV_UPLOADER_EXE=".\packages\%CODECOV_UPLOADER%\tools\codecov.exe"

set NUINT_CONSOLE_VERSION=3.15.5
set NUINT_CONSOLE=NUnit.ConsoleRunner.%NUINT_CONSOLE_VERSION%
set NUINT_CONSOLE_EXE=".\packages\%NUINT_CONSOLE%\tools\nunit3-console.exe"

set REPORTGENERATOR_VERSION=5.2.0
set REPORTGENERATOR=ReportGenerator.%REPORTGENERATOR_VERSION%
set REPORTGENERATOR_EXE=".\packages\%REPORTGENERATOR%\tools\net8.0\ReportGenerator.exe"

set FRAMEWORK=net481
set RESULTFILEPATH=".\Test\%TESTPROJECTNAME%\bin\x64\Debug\%FRAMEWORK%\%RESULTFILENAME%"

nuget install OpenCover -Version %OPENCOVER_VERSION% -OutputDirectory packages
nuget install CodecovUploader -Version %CODECOV_UPLOADER_VERSION% -OutputDirectory packages
nuget install NUnit.ConsoleRunner -Version %NUINT_CONSOLE_VERSION% -OutputDirectory packages

if not exist %OPENCOVER_EXE% goto error_console1
if not exist %CODECOV_UPLOADER_EXE% goto error_console2
if not exist %NUINT_CONSOLE_EXE% goto error_console3

if exist ".\Test\%TESTPROJECTNAME%\publish" rd /S /Q ".\Test\%TESTPROJECTNAME%\publish"

dotnet build ./Test/%TESTPROJECTNAME% -c Debug -f %FRAMEWORK% /p:Platform=x64

if exist .\Test\%TESTPROJECTNAME%\*.log del .\Test\%TESTPROJECTNAME%\*.log
if exist %RESULTFILEPATH% del %RESULTFILEPATH%
dotnet test ./Test/%TESTPROJECTNAME% -c Debug -f %FRAMEWORK% /p:Platform=x64 --no-build
if exist %RESULTFILEPATH% %CODECOV_UPLOADER_EXE% -f %RESULTFILEPATH% -t %TOKEN%
goto end

:error_console1
echo ERROR: OpenCover.Console not found.
goto end

:error_console2
echo ERROR: Codecov not found.
goto end

:error_console3
echo ERROR: nunit3-console not found.
goto end

:error_not_built
echo ERROR: %TESTPROJECTNAME%.dll not built (both Debug and Release are required).
goto end

:end
