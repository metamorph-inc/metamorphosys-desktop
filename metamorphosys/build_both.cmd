Setlocal EnableDelayedExpansion
pushd META
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe make.msbuild /t:All /m /nodeReuse:false || exit /b !ERRORLEVEL!
popd META

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe tonka\make.msbuild /t:All /m /nodeReuse:false || exit /b !ERRORLEVEL!
