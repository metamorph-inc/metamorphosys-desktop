﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 10.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace CyPhy2Modelica_v2.Templates
{
    using System;
    using System.IO;
    using System.Diagnostics;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    
    
    #line 1 "C:\META\meta_trunk\src\CyPhy2Modelica_v2\Templates\jmodelica_simulate_py.tt"
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public partial class jmodelica_simulate_py : jmodelica_simulate_pyBase
    {
        public virtual string TransformText()
        {
            this.Write(" \r\n");
            this.Write(" \r\n# ---------------------------------------------------------\r\n# Auto generated " +
                    "from jmodelica_simulate_py.tt\r\n# Execute this script using system Python (with J" +
                    "Modelica)\r\n# ---------------------------------------------------------\r\n\r\nimport" +
                    " os\r\nimport sys\r\nimport json\r\nimport logging\r\ntry:\r\n    from pyfmi import load_f" +
                    "mu\r\nexcept:\r\n    print \"load_fmu module was not found. Call this script using sy" +
                    "stem Python and with JModelica installed.\"\r\n\r\nfrom optparse import OptionParser\r" +
                    "\n\r\nparser = OptionParser()\r\nparser.add_option(\"-f\", \"--fmu\", dest=\"fmu_path\", he" +
                    "lp=\"Path to Modelica model within its package\")\r\n\r\nlogging.basicConfig(filename=" +
                    "\"j_simulate.log\",\r\n                    level=logging.DEBUG,\r\n                   " +
                    " format=\"%(asctime)s %(levelname)s: %(message)s\",\r\n                    datefmt=\"" +
                    "%Y-%m-%d %H:%M:%S\")\r\n\r\ndef simulate_fmu(fmu_path):\r\n    log = logging.getLogger(" +
                    ")\r\n    # Load the fmu and update parameters given in json from update_parameters" +
                    ".\r\n    executable_fmu = load_fmu(fmu_path)\r\n    if os.path.isfile(\'j_parameters." +
                    "json\'):\r\n        vars = []\r\n        values = []\r\n        with open(\'j_parameters" +
                    ".json\', \'r\') as f_in:\r\n            params = json.load(f_in)\r\n            for key" +
                    ", value in params.iteritems():\r\n                vars.append(key)\r\n              " +
                    "  values.append(value)\r\n        log.info(\'variables : {0}\'.format(vars))\r\n      " +
                    "  log.info(\'values : {0}\'.format(values))\r\n        executable_fmu.set(vars, valu" +
                    "es)\r\n    else:\r\n        log.info(\'No j_parameters.json found assumes updating of" +
                    " parameters not needed..\')\r\n    # Setup the simulation options (start and stopti" +
                    "me read from model)\r\n    with open(\'model_config.json\', \'r\') as f_in:\r\n        e" +
                    "xperiment = json.load(f_in)[\'experiment\']\r\n\r\n    opts = executable_fmu.simulate_" +
                    "options()\r\n    opts[\'solver\'] = experiment[\'Algorithm\'][\'JModelica\']\r\n    log.in" +
                    "fo(\'Using solver for JModelica : {0}\'.format(opts[\'solver\']))\r\n    if experiment" +
                    "[\'IntervalMethod\'] == \'NumberOfIntervals\':\r\n        opts[\'ncp\'] = int(experiment" +
                    "[\'NumberOfIntervals\'])\r\n    else:\r\n        log.warning(\'Number of intervals not " +
                    "a parameter of JModelica, trying to calculate ncp from start- and stoptime.\')\r\n " +
                    "       try:\r\n            opts[\'ncp\'] = int((float(experiment[\'StopTime\']) - floa" +
                    "t(experiment[\'StartTime\']))//float(experiment[\'Interval\']))\r\n            log.inf" +
                    "o(\'Calculated ncp : {0}\'.format(opts[\'ncp\']))\r\n        except:\r\n            # Fa" +
                    "ll back on default, i.e. internal steps.\r\n            log.warning(\'Failed to cal" +
                    "culate ncp from interval, falling back on JModelica default...\')\r\n    # Run the " +
                    "simulation and save the result object\r\n    sim_result = executable_fmu.simulate(" +
                    "options=opts)\r\n    log.info(\'FMU was simulated: {0}\'.format(fmu_path))\r\n\r\ndef ma" +
                    "in():\r\n    log = logging.getLogger()\r\n    working_dir = os.getcwd()  # should be" +
                    " CyPhy\r\n    this_script_path = os.path.abspath(__file__)\r\n\r\n    log.info(\'Runnin" +
                    "g {0} from {1}\'.format(this_script_path, working_dir))\r\n\r\n    (options, args) = " +
                    "parser.parse_args() \r\n    fmu_path = options.fmu_path\r\n\r\n    if not fmu_path:\r\n " +
                    "       # Try to find any .fmu in this directory\r\n        log.info(\'fmu_path not " +
                    "provided, looking for it in working_dir\')\r\n        for file in os.listdir(workin" +
                    "g_dir):\r\n            if file.endswith(\'.fmu\'):\r\n                fmu_path = os.pa" +
                    "th.abspath(file)\r\n                log.info(\'Found {0}!\'.format(fmu_path))\r\n     " +
                    "           break\r\n    if not fmu_path:\r\n        raise IOError(\'Could not find a " +
                    ".fmu file inside {0}\'.format(working_dir))\r\n    log.info(\'fmu_path : {0}\'.format" +
                    "(fmu_path))\r\n    simulate_fmu(fmu_path)\r\n\r\n\r\nif __name__ == \'__main__\':\r\n    roo" +
                    "t_dir = os.getcwd()\r\n    log = logging.getLogger()\r\n    try:\r\n        main()\r\n  " +
                    "  except:\r\n        import traceback\r\n        trace = traceback.format_exc()\r\n   " +
                    "     # Generate this file on failed executions, https://github.com/scipy/scipy/i" +
                    "ssues/1840\r\n        with open(os.path.join(root_dir, \'_j_simulate_FAILED.txt\'), " +
                    "\'wb\') as f_out:\r\n            f_out.write(trace)\r\n        log.error(\'See {0} for " +
                    "details.\'.format(os.path.join(root_dir, \'_j_simulate_FAILED.txt\')))\r\n        sys" +
                    ".exit(2)\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 111 "C:\META\meta_trunk\src\CyPhy2Modelica_v2\Templates\jmodelica_simulate_py.tt"
 public string ToolName ="";
	public string DymolaHome = "";
    public string ResultMatFile = "";
        
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
    public class jmodelica_simulate_pyBase
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
