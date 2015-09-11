﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 10.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace CyPhyPET.Templates
{
    using System;
    using System.IO;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;
    using System.Collections;
    using System.Collections.Generic;
    
    
    #line 1 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public partial class PCCDriver : PCCDriverBase
    {
        public virtual string TransformText()
        {
            this.Write(" \r\n");
            this.Write(@"# ===========================================================================
# Auto generated from PCCDriver.tt
# ===========================================================================
# imports
import os
import sys
import logging

# OpenMDAO Assembly Component (Optimization)
from openmdao.main.api import Assembly, set_as_top
from openmdao.main.file_supp import FileMetadata

# Import from META-PCC Module
from PCC.model_calls import ListGen, InitializeCluster
from PCC.pcc_driver import PCCdriver
from PCC.model_calls import UseLocalParallel # Call this function to enable parallel execution

from test_bench import TestBench

# Remove all log-handlers that except for the two from pym
log = logging.getLogger()
while len(log.handlers) > 2:
    log.removeHandler(log.handlers[-1])
");
            
            #line 37 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
 if (DriverName.Contains("parallel_execution"))
 {
            
            #line default
            #line hidden
            this.Write("UseLocalParallel()\r\n ");
            
            #line 40 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
 } 
 else
 {

            
            #line default
            #line hidden
            this.Write("#UseLocalParallel()\r\n ");
            
            #line 45 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"

 }
 
            
            #line default
            #line hidden
            this.Write(@"

class PCC_Experiment_v1(Assembly):
    """""" Documentation comment for this Assembly. """"""

    def __init__(self):
        super(PCC_Experiment_v1, self).__init__()

        # Create Assembly Instance
        self.add('TestBench', TestBench())

        # Add driver
        self.add('driver', PCCdriver())
        self.driver.DOEgenerator = ListGen()

        # Add files (for parallel execution)
        log.debug('Adding files : ')
        for path, dirs, files in os.walk('.'):
            if path[2:5]!='Sim':
                for filename in files:
                    log.debug('    {0}'.format(os.path.join(path[2:],filename)))
                    needed = FileMetadata (os.path.join(path[2:],filename),input=True,binary=True)
                    self.external_files.append(needed)
        log.debug(' to list of required files.')
        # Load configuration for the PCC-driver
        self.driver.load_json_file('");
            
            #line 73 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PCCConfigJson));
            
            #line default
            #line hidden
            this.Write("\')\r\n        log.debug(\'");
            
            #line 74 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PCCConfigJson));
            
            #line default
            #line hidden
            this.Write(" succesfully loaded\')\r\n        #don\'t re-copy model files to remote server every " +
                    "time. Reuse them.\r\n        self.driver.reload_model = False \r\n\r\n        # Design" +
                    " Variables\r\n");
            
            #line 79 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
 foreach (var name in DesignVariables)
   {
        
            
            #line default
            #line hidden
            this.Write("        self.driver.add_parameter(\'");
            
            #line 82 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(name));
            
            #line default
            #line hidden
            this.Write("\', low = 0, high = 1)\r\n");
            
            #line 83 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
}
            
            #line default
            #line hidden
            this.Write("        # Extra design variables (Properties).\r\n");
            
            #line 85 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
 foreach (var kvp in this.PCCPropertyInputs)
       {
            
            #line default
            #line hidden
            this.Write("        self.driver.add_parameter(\'TestBench.");
            
            #line 87 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(kvp.Key));
            
            #line default
            #line hidden
            this.Write("\', low = 0, high = 1)\r\n");
            
            #line 88 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
}
            
            #line default
            #line hidden
            this.Write("\r\n        # Objectives\r\n");
            
            #line 91 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
foreach (var name in Objectives)
	{
            
            #line default
            #line hidden
            this.Write("        self.driver.add_objective(\'");
            
            #line 93 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(name));
            
            #line default
            #line hidden
            this.Write("\')\r\n");
            
            #line 94 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
}
            
            #line default
            #line hidden
            this.Write("        self.driver.case_outputs=[\r\n");
            
            #line 96 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
foreach (var name in Objectives)
	{
            
            #line default
            #line hidden
            this.Write("            \'");
            
            #line 98 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(name));
            
            #line default
            #line hidden
            this.Write("\',\r\n");
            
            #line 99 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"
}
            
            #line default
            #line hidden
            this.Write(@"        ]

        self.driver.workflow.add(['TestBench'])

def main():
    import time
    #    InitializeCluster(['ubuntu@ec2-107-22-122-74.compute-1.amazonaws.com'],'/home/ubuntu/openmdao-0.6.2/bin/python')
    #    InitializeCluster(['ubuntu@ec2-23-22-37-92.compute-1.amazonaws.com',\
    #                       'ubuntu@ec2-107-22-122-74.compute-1.amazonaws.com',\
    #                       'ubuntu@ec2-23-22-252-101.compute-1.amazonaws.com',\
    #                       'ubuntu@ec2-54-226-198-113.compute-1.amazonaws.com'],\
    #                       '/home/ubuntu/openmdao-0.6.2/bin/python')

    pcc_problem = PCC_Experiment_v1()
    set_as_top(pcc_problem)
    tt = time.time()
    pcc_problem.run()

    print ""Elapsed time: "", time.time()-tt, ""seconds""

if __name__ == ""__main__"":
    main()
");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 122 "C:\META\meta_trunk\src\CyPhyPET\Templates\PCCDriver.tt"

	public string DriverName {get;set;}
	public string PCCConfigJson {get;set;}
	public List<string> Objectives {get;set;}
	public List<string> DesignVariables{get;set;}
	public Dictionary<string, string> PCCPropertyInputs {get;set;}

        
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
    public class PCCDriverBase
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
    }
    #endregion
}
