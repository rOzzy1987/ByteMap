@echo OFF

if "%~1"=="" goto blank

SET FOLDER=ByteMap.%1.bin

mkdir %FOLDER%
copy /Y ByteMap\bin\Release\* %FOLDER%
copy /Y ByteMap.Cmd\bin\Release\* %FOLDER%
copy /Y resource\test.png %FOLDER%
cd %FOLDER%
del *.config
del *.xml
del *.pdb
7z a ..\..\..\Releases\ByteMap.%1.7z *
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

:end