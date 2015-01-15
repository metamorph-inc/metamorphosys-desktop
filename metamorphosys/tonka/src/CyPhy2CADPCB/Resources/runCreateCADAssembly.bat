echo off
pushd %~dp0
%SystemRoot%\SysWoW64\REG.exe query "HKLM\software\META" /v "META_PATH"

SET QUERY_ERRORLEVEL=%ERRORLEVEL%

IF %QUERY_ERRORLEVEL% == 0 (
    FOR /F "skip=2 tokens=2,*" %%A IN ('%SystemRoot%\SysWoW64\REG.exe query "HKLM\software\META" /v "META_PATH"') DO SET META_PATH=%%B)
    SET META_PYTHON_EXE="%META_PATH%\bin\Python27\Scripts\Python.exe"
    %META_PYTHON_EXE% Synthesize_PCB_CAD_connections.py "{0}"

	set ERROR_CODE=%ERRORLEVEL%
	if %ERRORLEVEL% NEQ 0 (
	set ERROR_MSG="Error from runAddComponentToPcbAssembly.bat: Encountered error during execution Synthesize_PCB_CAD_connections.py, error level is %ERROR_CODE%"
	goto :ERROR_SECTION
	)
)

IF %QUERY_ERRORLEVEL% == 1 (
    echo on
    echo "META tools not installed." >> _FAILED.txt
    echo "See Error Log: _FAILED.txt"
    exit /b %QUERY_ERRORLEVEL%
)
popd

cmd /c runCreateCADAssembly.bat

exit 0

:ERROR_SECTION
echo %ERROR_MSG% >>_FAILED.txt
echo ""
echo "See Error Log: _FAILED.txt"
ping -n 8 127.0.0.1 > nul
exit /b %ERROR_CODE%