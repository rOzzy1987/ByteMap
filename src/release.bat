@echo OFF

if "%~1"=="" goto blank

call clean.bat

echo Nuget restore
dotnet restore > ByteMap.%1.log

echo Build cmd
dotnet msbuild -property:Configuration=Release ByteMap.Cmd\ByteMap.Cmd.csproj >> ByteMap.%1.log
if %ERRORLEVEL% gtr 0 goto error

echo Build WinForms
dotnet msbuild -property:Configuration=Release ByteMap\ByteMap.csproj >> ByteMap.%1.log
if %ERRORLEVEL% gtr 0 goto error

SET FOLDER=ByteMap.%1.bin

mkdir %FOLDER%
copy /Y ByteMap\bin\Release\* %FOLDER% >> nul
copy /Y ByteMap.Cmd\bin\Release\* %FOLDER% >> nul
copy /Y resource\test.png %FOLDER% >> nul
cd %FOLDER%
del *.config
del *.xml
del *.pdb
7z a ..\..\..\Releases\ByteMap.%1.7z * >> nul
cd ..

rmdir /S /Q %FOLDER%

echo ----
echo Release ByteMap.%1 done
echo ----
goto end

:blank
echo ----
echo Set release name!
echo ----
goto end

:error
echo ----
echo Error!
echo See ByteMap.%1.log
echo ----

:end