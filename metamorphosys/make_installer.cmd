@rem BUILD SOLUTIONS
C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild META\make.msbuild /t:All /m /nodeReuse:false || exit /b !ERRORLEVEL!
C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild tonka\make.msbuild /t:All /m /nodeReuse:false || exit /b !ERRORLEVEL!

@rem BUILD INSTALLER
pushd META\deploy
..\bin\Python27\Scripts\python.exe build_msi.py || (popd & exit /b !ERRORLEVEL!)
popd