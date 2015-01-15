# Copyright (C) 2013-2015 MetaMorph Software, Inc

# Permission is hereby granted, free of charge, to any person obtaining a
# copy of this data, including any software or models in source or binary
# form, as well as any drawings, specifications, and documentation
# (collectively "the Data"), to deal in the Data without restriction,
# including without limitation the rights to use, copy, modify, merge,
# publish, distribute, sublicense, and/or sell copies of the Data, and to
# permit persons to whom the Data is furnished to do so, subject to the
# following conditions:

# The above copyright notice and this permission notice shall be included
# in all copies or substantial portions of the Data.

# THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
# THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
# LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
# OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

# =======================
# This version of the META tools is a fork of an original version produced
# by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
# Their license statement:

# Copyright (C) 2011-2014 Vanderbilt University

# Developed with the sponsorship of the Defense Advanced Research Projects
# Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
# as defined in DFARS 252.227-7013.

# Permission is hereby granted, free of charge, to any person obtaining a
# copy of this data, including any software or models in source or binary
# form, as well as any drawings, specifications, and documentation
# (collectively "the Data"), to deal in the Data without restriction,
# including without limitation the rights to use, copy, modify, merge,
# publish, distribute, sublicense, and/or sell copies of the Data, and to
# permit persons to whom the Data is furnished to do so, subject to the
# following conditions:

# The above copyright notice and this permission notice shall be included
# in all copies or substantial portions of the Data.

# THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
# THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
# LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
# OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

$cptpd_name = "CREO_PARAMETRIC_TOOLKIT_PARENT_DIR"
$fso = New-Object -ComObject Scripting.FileSystemObject
$cptpd_value = $fso.GetFolder("C:\Program Files\PTC\Creo 2.0\Common Files\M030")
[Environment]::SetEnvironmentVariable($cptpd_name, $cptpd_value.ShortPath,"User")

pushd 'c:\Program Files (x86)\Microsoft Visual Studio 10.0\VC'
cmd /c "vcvarsall.bat&set" |
foreach {
  if ($_ -match "=") {
    $v = $_.split("="); set-item -force -path "ENV:\$($v[0])"  -value "$($v[1])"
  }
}
popd
write-host "`nVisual Studio 2010 Command Prompt variables set." -ForegroundColor Yellow

# pushd CADCommonFunctions
# MSBuild.exe .\CADCommonFunctions.sln /target:Rebuild /property:Configuration=Release /property:Platform=x64
# popd

# pushd CADCreoParametricCommonFunctions
# MSBuild.exe .\CADCreoParametricCommonFunctions.sln /target:Rebuild /property:Configuration=Release /property:Platform=x64
# popd

# pushd CADCreoParametricToolkitFunctions
# MSBuild.exe .\CADCreoParametricToolkitFunctions.sln /target:Rebuild /property:Configuration=Release /property:Platform=x64
# popd

pushd CADCreoParametricCreateAssembly
MSBuild.exe .\CADCreoParametricCreateAssembly.sln /target:Rebuild /property:Configuration=Release /property:Platform=x64
& 'C:\Program Files\PTC\Creo 2.0\Parametric\bin\protk_unlock.bat' ./x64/Release/CADCreoParametricCreateAssembly.exe
copy-item ./x64/Release/CADCreoParametricCreateAssembly.exe $Env:PROE_ISIS_EXTENSIONS/bin/
popd

