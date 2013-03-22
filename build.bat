@ECHO OFF

ECHO Building Project

C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe /property:Configuration=Release;Platform=x86 FishingForm.csproj

ECHO.
ECHO Build Complete
ECHO.

pause

ECHO.
ECHO Creating Deployable Archives
ECHO.

FOR /F "tokens=1-3" %%i IN ('C:\PROGRA~2\SigCheck\sigcheck.exe bin\x86\Release\Fishing.exe') DO (IF "%%i %%j"=="File version:" SET filever=%%k)

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

"C:\Program Files\7-Zip\7z" a ..\FishingForm_v%filever%_mC-FD_(with_source).zip -mx9 -tzip *
"C:\Program Files\7-Zip\7z" a ..\FishingForm_mC-FD_(with_source).zip -mx9 -tzip *
"C:\Program Files\7-Zip\7z" a ..\FishingForm_v%filever%_mC-FD.zip -mx9 -tzip -x!source\ *
"C:\Program Files\7-Zip\7z" a ..\FishingForm_mC-FD.zip -mx9 -tzip -x!source\ *

cd ..\

ECHO.
ECHO Deployable Archives Created:
ECHO FishingForm_v%filever%_mC-FD_(with_source).zip
ECHO FishingForm_mC-FD_(with_source).zip
ECHO FishingForm_v%filever%_mC-FD.zip
ECHO FishingForm_mC-FD.zip
ECHO.

pause

rd /Q /S deploy