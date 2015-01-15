@ECHO ON
REM CLS

echo Checking ANTLR_PATH == %ANTLR_PATH%
	set CHECK_TARGET=%ANTLR_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking ANTLR3_PATH == %ANTLR3_PATH%
	set CHECK_TARGET=%ANTLR3_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking BOOST_PATH == %BOOST_PATH%
	set CHECK_TARGET=%BOOST_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking CTEMPLATE_PATH == %CTEMPLATE_PATH%
	set CHECK_TARGET=%CTEMPLATE_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking GECODE_PATH == %GECODE_PATH%
	set CHECK_TARGET=%GECODE_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking GREAT_PATH == %GREAT_PATH%
	set CHECK_TARGET=%GREAT_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking MATLAB_PATH == %MATLAB_PATH%
	set CHECK_TARGET=%MATLAB_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking PYTHON_HOME == %PYTHON_HOME%
	set CHECK_TARGET=%PYTHON_HOME%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking UDM_PATH == %UDM_PATH%
	set CHECK_TARGET=%UDM_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking XERCES_PATH == %XERCES_PATH%
	set CHECK_TARGET=%XERCES_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED

echo Checking VCP_PATH == %VCP_PATH%
	set CHECK_TARGET=%VCP_PATH%
	IF ("%CHECK_TARGET%")==("") goto FAILED
	IF NOT EXIST "%CHECK_TARGET%" goto FAILED


:SUCCEEDED
	echo SUCCESS
	exit /b 0
:FAILED
	echo FAILED verifying %CHECK_TARGET%
	exit /b 1



