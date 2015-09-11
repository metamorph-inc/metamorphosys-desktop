﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 10.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace CyPhy2CAD_CSharp.Template
{
    using System;
    using System.IO;
    using System.Diagnostics;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    
    
    #line 1 "C:\Users\di\Desktop\META_Working\src\C_Sharp_Dev\CyPhy2CAD_CSharp\Template\zip_to_remote_py.tt"
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public partial class zip_to_remote_py : zip_to_remote_pyBase
    {
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
        public virtual string TransformText()
        {
            this.GenerationEnvironment = null;
            this.Write(" \r\n");
            this.Write("\r\n\r\n# ---------------------------------------------------\r\n# Auto generated by Cy" +
                    "Phy2CAD\r\n# ---------------------------------------------------\r\nimport zipfile\r\n" +
                    "import os\r\nimport shutil\r\nimport subprocess\r\n\r\ncomponents_folder = [ \\\r\n");
            
            #line 22 "C:\Users\di\Desktop\META_Working\src\C_Sharp_Dev\CyPhy2CAD_CSharp\Template\zip_to_remote_py.tt"
 foreach (var folder in CadFolders)
	{ 
            
            #line default
            #line hidden
            this.Write("    r\"");
            
            #line 24 "C:\Users\di\Desktop\META_Working\src\C_Sharp_Dev\CyPhy2CAD_CSharp\Template\zip_to_remote_py.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(folder));
            
            #line default
            #line hidden
            this.Write("\", \\\r\n");
            
            #line 25 "C:\Users\di\Desktop\META_Working\src\C_Sharp_Dev\CyPhy2CAD_CSharp\Template\zip_to_remote_py.tt"
  } 
            
            #line default
            #line hidden
            this.Write("    ]\r\n\r\n# call Copy_Parts.bat\r\ncopy_bat = \'Copy_Parts.bat\'\r\nif os.path.exists(co" +
                    "py_bat):\r\n    try:\r\n        result, error = subprocess.Popen(copy_bat, stdout = " +
                    "subprocess.PIPE, stderr= subprocess.PIPE).communicate()\r\n        with open(\'zip_" +
                    "log.txt\', \'w\') as zip_log:\r\n            zip_log.write(result)\r\n            zip_l" +
                    "og.write(error)\r\n    except Exception as msg:\r\n        with open(\'_FAILED.txt\', " +
                    "\'w\') as f_out:\r\n            f_out.write(str(msg))\r\n            f_out.write(\'\\nNo" +
                    "t able to copy cad files.\')\r\n        if os.name == \'nt\':\r\n            os._exit(3" +
                    ")\r\n        elif os.name == \'posix\':\r\n            os._exit(os.EX_OSFILE)\r\n\r\nsearc" +
                    "h_path = \'search_META.pro\'\r\nif os.path.exists(search_path):\r\n    shutil.copyfile" +
                    "(search_path, search_path + \'.local\')\r\n\r\nwith open (search_path, \'w\') as search_" +
                    "path_file:\r\n    if os.path.exists(\'Cad_Auxiliary_Directory\'):\r\n        search_pa" +
                    "th_file.write(\'\".\\Cad_Auxiliary_Directory\"\\n\')\r\n\r\n    for folder in components_f" +
                    "older:\r\n        search_path_file.write(\'\".\\\\\' + folder + \'\"\\n\')\r\n\r\n# zip\r\noutput" +
                    "_filename = \'source_data.zip\'\r\n\r\nif os.path.exists(output_filename):\r\n    os.rem" +
                    "ove(output_filename)\r\n\r\nwith zipfile.ZipFile(output_filename, \'w\') as z:\r\n    pa" +
                    "rent_dir_name = os.path.basename(os.getcwd())\r\n    os.chdir(\'..\\\\\')\r\n    for dir" +
                    "path,dirs,files in os.walk(parent_dir_name):\r\n      for f in files:\r\n        if " +
                    "output_filename == f:\r\n            continue\r\n        fn = os.path.join(dirpath, " +
                    "f)\r\n        z.write(fn, compress_type=zipfile.ZIP_DEFLATED)\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 72 "C:\Users\di\Desktop\META_Working\src\C_Sharp_Dev\CyPhy2CAD_CSharp\Template\zip_to_remote_py.tt"

public List<string> CadFolders{get;set;}

        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public class zip_to_remote_pyBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
    }
    #endregion
}
