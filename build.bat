@ECHO OFF

REM These are path variables, if your system has things in a different place
REM Please ensure SIGCHECK has no spaces in it. DOS style short paths may be useful.

SET NETLOC=C:\Windows\Microsoft.NET\Framework\v3.5
SET SIGCHECK=D:\PROGRA~2\SigCheck
SET ZIP7=D:\Program Files\7-Zip

REM Check parameters, if those happen

SET HELPPARAMS=-H HELP /H /HELP
SET DEPLOYPARAMS=/D /DEPLOY -D DEPLOY
FOR %%A IN (%*) DO (
	FOR %%H IN (%HELPPARAMS%) DO (
		IF /I "%%A"=="%%H" (
			ECHO This script is used to build FishingForm. Options:
			ECHO %HELPPARAMS% -- Display this message and exit
			ECHO %DEPLOYPARAMS% -- Skip building and just make deploy files
			PAUSE
			EXIT /B
		)
	)
	FOR %%D IN (%DEPLOYPARAMS%) DO (
		IF /I "%%A"=="%%D" (
			SET DEPLOY=1
		)
	)
)

EXIT /B

ECHO Checking Required Programs

SET MISSING=
IF NOT DEFINED DEPLOY (
	IF NOT EXIST "%NETLOC%\MSBuild.exe" (
		SET MISSING=1
		ECHO %NETLOC%\MSBuild.exe not found. Please ensure proper build files exist or run %~f0 deploy
	)
)
IF NOT EXIST "%SIGCHECK%\sigcheck.exe" (
	SET MISSING=1
	ECHO %SIGCHECK%\sigcheck.exe not found. Please download from microsoft site--google it--and adjust paths.
)
IF NOT EXIST "%ZIP7%\7z.exe" (
	SET MISSING=1
	ECHO %ZIP7%\7z.exe not found. Please ensure it or another zip creation program is installed and adjust paths.
)
IF DEFINED MISSING (
	ECHO.
	ECHO Required files were not found. Exiting.
	PAUSE
	EXIT /B
)

IF NOT DEFINED DEPLOY (
	ECHO Building Project

	%NETLOC%\MSBuild.exe /property:Configuration=Release;Platform=x86 FishingForm.csproj

	ECHO.
	ECHO Build Complete
	ECHO.

	PAUSE
)

ECHO.
ECHO Creating Deployable Archives
ECHO.

FOR /F "tokens=1-3" %%i IN ('%SIGCHECK%\sigcheck.exe bin\x86\Release\Fishing.exe') DO (IF "%%i %%j"=="File version:" SET filever=%%k)

mkdir deploy\source\Properties
mkdir deploy\source\images
mkdir deploy\source\Fishing
mkdir deploy\source\FFACETools
mkdir deploy\FishDB

copy Properties\* deploy\source\Properties\
copy images\* deploy\source\images\
copy Fishing\* deploy\source\Fishing\
copy FFACETools\* deploy\source\FFACETools\
copy *.cs deploy\source
copy *.resx deploy\source
copy *.suo deploy\source
copy *.user deploy\source
copy *.sln deploy\source
copy *.csproj deploy\source
copy *.dll 
copy *.cd deploy\source
copy *.bat deploy\source
copy *.log deploy\source
copy *.config deploy\source
copy bin\x86\Release\*.exe deploy
copy bin\x86\Release\*.dll deploy
copy bin\x86\Release\FishDB\* deploy\FishDB
copy bin\x86\Release\*.wav deploy

del FishingForm_v%filever%_mC-FD_(with_source).zip
del FishingForm_mC-FD_(with_source).zip
del FishingForm_v%filever%_mC-FD.zip
del FishingForm_mC-FD.zip

cd deploy

"%ZIP7%\7z" a ..\FishingForm_v%filever%_mC-FD_(with_source).zip -mx9 -tzip *
"%ZIP7%\7z" a ..\FishingForm_mC-FD_(with_source).zip -mx9 -tzip *
"%ZIP7%\7z" a ..\FishingForm_v%filever%_mC-FD.zip -mx9 -tzip -x!source\ *
"%ZIP7%\7z" a ..\FishingForm_mC-FD.zip -mx9 -tzip -x!source\ *

cd ..\

ECHO %filever% > version

ECHO.
ECHO Deployable Archives Created:
ECHO FishingForm_v%filever%_mC-FD_(with_source).zip
ECHO FishingForm_mC-FD_(with_source).zip
ECHO FishingForm_v%filever%_mC-FD.zip
ECHO FishingForm_mC-FD.zip
ECHO.

pause

rd /Q /S deploy