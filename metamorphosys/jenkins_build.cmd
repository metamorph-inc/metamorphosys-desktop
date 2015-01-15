Setlocal EnableDelayedExpansion

@rem BUILD SOLUTIONS
C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild META\make.msbuild /t:All /m /nodeReuse:false || exit /b !ERRORLEVEL!
C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild tonka\make.msbuild /t:All /m /nodeReuse:false || exit /b !ERRORLEVEL!

@rem RUN TONKA TESTS
del tonka\test\*_result.xml
del tonka\test\*_results.xml
tonka\run_tests_console_output_xml_parallel.py || exit /b !ERRORLEVEL!

@rem RUN META TESTS
del META\test\*_result.xml
del META\test\*_results.xml
c:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild META\test\run.msbuild || exit /b !ERRORLEVEL!

@rem BUILD INSTALLER
pushd META\deploy
..\bin\Python27\Scripts\python.exe build_msi.py || (popd & exit /b !ERRORLEVEL!)
popd