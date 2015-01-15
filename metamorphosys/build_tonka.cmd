@ECHO OFF
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe tonka\make.msbuild /t:All /m /nodeReuse:false /property:TONKA_PATH=%~dp0\tonka
