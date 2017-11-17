@echo off
rem Copyright (c) Weekend Game Studio. All rights reserved.

setlocal
set error=0

set fxcpath="C:\Program Files (x86)\Windows Kits\8.1\bin\x86\fxc.exe"
goto compile

:compile

call :CompileShader vs vsLine
call :CompileShader vs vsLine NOISE
call :CompileShader vs vsLine UVSHIFT
call :CompileShader vs vsLine NOISE UVSHIFT

call :CompileShader vs vsLine APPEARING 
call :CompileShader vs vsLine APPEARING NOISE
call :CompileShader vs vsLine APPEARING UVSHIFT
call :CompileShader vs vsLine APPEARING NOISE UVSHIFT

call :CompileShader vs vsLine BIAS 
call :CompileShader vs vsLine BIAS NOISE
call :CompileShader vs vsLine BIAS UVSHIFT
call :CompileShader vs vsLine BIAS NOISE UVSHIFT

call :CompileShader vs vsLine BIAS APPEARING 
call :CompileShader vs vsLine BIAS APPEARING NOISE
call :CompileShader vs vsLine BIAS APPEARING UVSHIFT
call :CompileShader vs vsLine BIAS APPEARING NOISE UVSHIFT


call :CompileShader ps psLine

echo.

if %error% == 0 (
    echo Shaders compiled ok
) else (
    echo There were shader compilation errors!
)

endlocal
exit /b

:CompileShader
if [%3]==[] goto three
if [%4]==[] goto four
if [%5]==[] goto five
if [%6]==[] goto six
if [%7]==[] goto seven
if [%8]==[] goto eight
if [%9]==[] goto nine

set fxc=%fxcpath% /nologo LineMaterial.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /D %6 /D %7 /D %9 /E %2 /Fo %1LineMaterial_%3_%4_%5_%6_%7_%8_%9.fxo
goto end

:nine
set fxc=%fxcpath% /nologo LineMaterial.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /D %6 /D %7 /D %8 /E %2 /Fo %1LineMaterial_%3_%4_%5_%6_%7_%8.fxo
goto end

: eight
set fxc=%fxcpath% /nologo LineMaterial.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /D %6 /D %7 /E %2 /Fo %1LineMaterial_%3_%4_%5_%6_%7.fxo
goto end

:seven
set fxc=%fxcpath% /nologo LineMaterial.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /D %6 /E %2 /Fo %1LineMaterial_%3_%4_%5_%6.fxo
goto end

:six
set fxc=%fxcpath% /nologo LineMaterial.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /E %2 /Fo %1LineMaterial_%3_%4_%5.fxo
goto end

:five
set fxc=%fxcpath% /nologo LineMaterial.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /E %2 /Fo %1LineMaterial_%3_%4.fxo
goto end

:four
set fxc=%fxcpath% /nologo LineMaterial.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /E %2 /Fo %1LineMaterial_%3.fxo
goto end

:three
set fxc=%fxcpath% /nologo LineMaterial.fx /T %1_4_0 /I ..\Helpers.fxh /E %2 /Fo %1LineMaterial%.fxo

:end
echo.
echo %fxc%
%fxc% || set error=1
exit /b