@ECHO OFF

REM These are path variables, if your system has things in a different place

REM This is some stupid crap to get DOS short paths for no spaces in the stuff
FOR %%A IN ("%WINDIR%\Microsoft.NET\Framework\v3.5") DO SET NETLOC=%%~sA
FOR %%A IN ("%PROGRAMFILES(X86)%\SigCheck") DO SET SIGCHECK=%%~sA
FOR %%A IN ("%PROGRAMFILES%\7-Zip") DO SET ZIP7=%%~sA

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

MKDIR deploy\source\Properties
MKDIR deploy\source\images
MKDIR deploy\source\Fishing
MKDIR deploy\source\FFACETools
MKDIR deploy\FishDB

COPY Properties\* deploy\source\Properties\
COPY images\* deploy\source\images\
COPY Fishing\* deploy\source\Fishing\
COPY FFACETools\* deploy\source\FFACETools\
COPY *.cs deploy\source
COPY *.resx deploy\source
COPY *.suo deploy\source
COPY *.user deploy\source
COPY *.sln deploy\source
COPY *.csproj deploy\source
COPY *.dll 
COPY *.cd deploy\source
COPY *.bat deploy\source
COPY *.log deploy\source
COPY *.config deploy\source
COPY *.mwb deploy\source
COPY bin\x86\Release\*.exe deploy
COPY bin\x86\Release\*.dll deploy
COPY bin\x86\Release\FishDB\* deploy\FishDB
COPY bin\x86\Release\*.wav deploy

DEL FishingForm_v%filever%_mC-FD_(with_source).zip
DEL FishingForm_mC-FD_(with_source).zip
DEL FishingForm_v%filever%_mC-FD.zip
DEL FishingForm_mC-FD.zip

CD deploy

"%ZIP7%\7z" a ..\FishingForm_v%filever%_mC-FD_(with_source).zip -mx9 -tzip *
"%ZIP7%\7z" a ..\FishingForm_mC-FD_(with_source).zip -mx9 -tzip *
"%ZIP7%\7z" a ..\FishingForm_v%filever%_mC-FD.zip -mx9 -tzip -x!source\ *
"%ZIP7%\7z" a ..\FishingForm_mC-FD.zip -mx9 -tzip -x!source\ *

CD ..\

ECHO.
ECHO Deployable Archives Created:
ECHO FishingForm_v%filever%_mC-FD_(with_source).zip
ECHO FishingForm_mC-FD_(with_source).zip
ECHO FishingForm_v%filever%_mC-FD.zip
ECHO FishingForm_mC-FD.zip
ECHO.

PAUSE

RD /Q /S deploy
