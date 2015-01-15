﻿/*
Copyright (C) 2013-2015 MetaMorph Software, Inc

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  
*/

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AraRFAnalysis.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AraRFAnalysis.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @ECHO OFF
        ///PUSHD %~dp0
        ///%SystemRoot%\SysWoW64\REG.exe query &quot;HKLM\software\META&quot; /v &quot;META_PATH&quot;
        ///
        ///SET QUERY_ERRORLEVEL=%ERRORLEVEL%
        ///
        ///IF %QUERY_ERRORLEVEL% == 0 (
        ///    FOR /F &quot;skip=2 tokens=2,*&quot; %%A IN (&apos;%SystemRoot%\SysWoW64\REG.exe query &quot;HKLM\software\META&quot; /v &quot;META_PATH&quot;&apos;) DO SET META_PATH=%%B)
        ///)
        ///IF %QUERY_ERRORLEVEL% == 1 (
        ///    echo on
        ///    echo &quot;META tools not installed.&quot; &gt;&gt; _FAILED.txt
        ///    echo &quot;META tools not installed.&quot;
        ///    exit /b %QUERY_ERRORLEVEL%
        ///)
        ///
        /// REM ----------------------------
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string run_farfield {
            get {
                return ResourceManager.GetString("run_farfield", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @ECHO OFF
        ///PUSHD %~dp0
        ///%SystemRoot%\SysWoW64\REG.exe query &quot;HKLM\software\META&quot; /v &quot;META_PATH&quot;
        ///
        ///SET QUERY_ERRORLEVEL=%ERRORLEVEL%
        ///
        ///IF %QUERY_ERRORLEVEL% == 0 (
        ///    FOR /F &quot;skip=2 tokens=2,*&quot; %%A IN (&apos;%SystemRoot%\SysWoW64\REG.exe query &quot;HKLM\software\META&quot; /v &quot;META_PATH&quot;&apos;) DO SET META_PATH=%%B)
        ///)
        ///IF %QUERY_ERRORLEVEL% == 1 (
        ///    echo on
        ///    echo &quot;META tools not installed.&quot; &gt;&gt; _FAILED.txt
        ///    echo &quot;META tools not installed.&quot;
        ///    exit /b %QUERY_ERRORLEVEL%
        ///)
        ///
        /// REM ----------------------------
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string run_sar {
            get {
                return ResourceManager.GetString("run_sar", resourceCulture);
            }
        }
    }
}
