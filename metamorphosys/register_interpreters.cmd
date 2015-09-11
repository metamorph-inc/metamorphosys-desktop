rem Unregisters all CyPhyML interpreters, then registers CyPhyML.mta and all interpreters
rem  Useful for removing old interpreters that don't exist in git anymore, or switching trees

: META_PATH=C:\Users\kevin\Documents\meta-tonka\
Setlocal EnableDelayedExpansion
META\bin\Python27\Scripts\Python.exe -c "reg = __import__('win32com.client').client.DispatchEx('Mga.MgaRegistrar'); [reg.UnregisterComponent(progid, 2) for progid in reg.GetAssociatedComponentsDisp('CyPhyML', 7, 2)]" || exit /b !ERRORLEVEL!
META\bin\Python27\Scripts\Python.exe -c "import sys; import os.path; filename=r'META\generated\CyPhyML\models\CyPhyML.mta'; not os.path.isfile(filename) and sys.exit(0); reg = __import__('win32com.client').client.DispatchEx('Mga.MgaRegistrar'); reg.RegisterParadigmFromDataDisp('MGA=' + os.path.abspath(filename), 1)" || exit /b !ERRORLEVEL!
%windir%\SysWOW64\reg add HKLM\Software\META /v META_PATH /t REG_SZ /d "%~dp0META"\ /f || exit /b !ERRORLEVEL!
if exist "META\src\bin\CPMDecorator.dll" regsvr32 /s "META\src\bin\CPMDecorator.dll" || exit /b !ERRORLEVEL!
rem TODO: register Decorators?
if exist "META\src\bin\CyPhyAddOn.dll" %windir%\SysWOW64\regsvr32 /s "META\src\bin\CyPhyAddOn.dll" || exit /b !ERRORLEVEL!
if exist "META\src\ComponentLibraryManagerAddOn\bin\Release\ComponentLibraryManagerAddOn.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\ComponentLibraryManagerAddOn\bin\Release\ComponentLibraryManagerAddOn.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyDecoratorAddon\bin\Release\CyPhyDecoratorAddon.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyDecoratorAddon\bin\Release\CyPhyDecoratorAddon.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyMdaoAddOn\bin\Release\CyPhyMdaoAddOn.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyMdaoAddOn\bin\Release\CyPhyMdaoAddOn.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyMetaLink\bin\Release\CyPhyMetaLink.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyMetaLink\bin\Release\CyPhyMetaLink.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhySignalBlocksAddOn\bin\Release\CyPhySignalBlocksAddOn.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhySignalBlocksAddOn\bin\Release\CyPhySignalBlocksAddOn.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CLM_light\bin\Release\CLM_light.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CLM_light\bin\Release\CLM_light.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhy2CAD_CSharp\bin\Release\CyPhy2CAD_CSharp.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhy2CAD_CSharp\bin\Release\CyPhy2CAD_CSharp.dll" || exit /b !ERRORLEVEL!
if exist "tonka\src\CyPhy2CADPCB\bin\Release\CyPhy2CADPCB.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "tonka\src\CyPhy2CADPCB\bin\Release\CyPhy2CADPCB.dll" || exit /b !ERRORLEVEL!
if exist "tonka\src\CyPhy2MfgBom\bin\Release\CyPhy2MfgBom.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "tonka\src\CyPhy2MfgBom\bin\Release\CyPhy2MfgBom.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhy2Modelica_v2\bin\Release\CyPhy2Modelica_v2.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhy2Modelica_v2\bin\Release\CyPhy2Modelica_v2.dll" || exit /b !ERRORLEVEL!
if exist "tonka\src\CyPhy2Schematic\bin\Release\CyPhy2Schematic.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "tonka\src\CyPhy2Schematic\bin\Release\CyPhy2Schematic.dll" || exit /b !ERRORLEVEL!
if exist "tonka\src\CyPhy2SystemC\bin\Release\CyPhy2SystemC.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "tonka\src\CyPhy2SystemC\bin\Release\CyPhy2SystemC.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyCADAnalysis\bin\Release\CyPhyCADAnalysis.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyCADAnalysis\bin\Release\CyPhyCADAnalysis.dll" || exit /b !ERRORLEVEL!
if exist "META\src\bin\CyPhyCAExporter.dll" %windir%\SysWOW64\regsvr32 /s "META\src\bin\CyPhyCAExporter.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyComplexity\bin\Release\CyPhyComplexity.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyComplexity\bin\Release\CyPhyComplexity.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyComponentAuthoring\bin\Release\CyPhyComponentAuthoring.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyComponentAuthoring\bin\Release\CyPhyComponentAuthoring.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyComponentExporter\bin\Release\CyPhyComponentExporter.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyComponentExporter\bin\Release\CyPhyComponentExporter.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyComponentImporter\bin\Release\CyPhyComponentImporter.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyComponentImporter\bin\Release\CyPhyComponentImporter.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyComponentParameterEditor\bin\Release\CyPhyComponentParameterEditor.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyComponentParameterEditor\bin\Release\CyPhyComponentParameterEditor.dll" || exit /b !ERRORLEVEL!
if exist "META\src\bin\CyPhyCriticalityMeter.dll" %windir%\SysWOW64\regsvr32 /s "META\src\bin\CyPhyCriticalityMeter.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyDesignExporter\bin\Release\CyPhyDesignExporter.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyDesignExporter\bin\Release\CyPhyDesignExporter.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyDesignImporter\bin\Release\CyPhyDesignImporter.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyDesignImporter\bin\Release\CyPhyDesignImporter.dll" || exit /b !ERRORLEVEL!
if exist "META\src\bin\CyPhyDesignSpaceRefactor.dll" %windir%\SysWOW64\regsvr32 /s "META\src\bin\CyPhyDesignSpaceRefactor.dll" || exit /b !ERRORLEVEL!
if exist "META\src\bin\CyPhyDSRefiner.dll" %windir%\SysWOW64\regsvr32 /s "META\src\bin\CyPhyDSRefiner.dll" || exit /b !ERRORLEVEL!
if exist "META\src\bin\CyPhyElaborate.dll" %windir%\SysWOW64\regsvr32 /s "META\src\bin\CyPhyElaborate.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyElaborateCS\bin\Release\CyPhyElaborateCS.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyElaborateCS\bin\Release\CyPhyElaborateCS.dll" || exit /b !ERRORLEVEL!
if exist "META\src\bin\CyPhyFormulaEvaluator.dll" %windir%\SysWOW64\regsvr32 /s "META\src\bin\CyPhyFormulaEvaluator.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyMasterInterpreter\bin\Release\CyPhyMasterInterpreter.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyMasterInterpreter\bin\Release\CyPhyMasterInterpreter.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyMetaLink\bin\Release\CyPhyMetaLink.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyMetaLink\bin\Release\CyPhyMetaLink.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyMultiJobRun\bin\Release\CyPhyMultiJobRun.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyMultiJobRun\bin\Release\CyPhyMultiJobRun.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyPET\bin\Release\CyPhyPET.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyPET\bin\Release\CyPhyPET.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyPrepareIFab\bin\Release\CyPhyPrepareIFab.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyPrepareIFab\bin\Release\CyPhyPrepareIFab.dll" || exit /b !ERRORLEVEL!
if exist "META\src\Release\CyPhyPython.dll" %windir%\SysWOW64\regsvr32 /s "META\src\Release\CyPhyPython.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhyReliabilityAnalysis\bin\Release\CyPhyReliabilityAnalysis.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhyReliabilityAnalysis\bin\Release\CyPhyReliabilityAnalysis.dll" || exit /b !ERRORLEVEL!
if exist "META\src\CyPhySoT\bin\Release\CyPhySoT.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\CyPhySoT\bin\Release\CyPhySoT.dll" || exit /b !ERRORLEVEL!
if exist "META\src\bin\DesignSpaceHelper.dll" %windir%\SysWOW64\regsvr32 /s "META\src\bin\DesignSpaceHelper.dll" || exit /b !ERRORLEVEL!
if exist "META\src\ModelicaImporter\bin\Release\ModelicaImporter.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\ModelicaImporter\bin\Release\ModelicaImporter.dll" || exit /b !ERRORLEVEL!
if exist "META\src\Run_PRISMATIC_toolchain\bin\Release\Run_PRISMATIC_toolchain.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\Run_PRISMATIC_toolchain\bin\Release\Run_PRISMATIC_toolchain.dll" || exit /b !ERRORLEVEL!
if exist "META\src\SubTreeMerge\bin\Release\SubTreeMerge.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "META\src\SubTreeMerge\bin\Release\SubTreeMerge.dll" || exit /b !ERRORLEVEL!
if exist "tonka\src\ShowNet\bin\Release\ShowNet.dll" %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase "tonka\src\ShowNet\bin\Release\ShowNet.dll" || exit /b !ERRORLEVEL!
