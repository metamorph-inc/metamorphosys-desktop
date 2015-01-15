/*
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using GME.CSharp;
using GME;
using GME.MGA;
using GME.MGA.Core;
using CyPhyGUIs;
using System.Windows.Forms;

using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using System.Diagnostics;
using META;

namespace CyPhy2CADPCB
{
    /// <summary>
    /// This class implements the necessary COM interfaces for a GME interpreter component.
    /// </summary>
    [Guid(ComponentConfig.guid),
    ProgId(ComponentConfig.progID),
    ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class CyPhy2CADPCBInterpreter : IMgaComponentEx, IGMEVersionInfo, ICyPhyInterpreter
    {
        /// <summary>
        /// Contains information about the GUI event that initiated the invocation.
        /// </summary>
        public enum ComponentStartMode
        {
            GME_MAIN_START = 0, 		// Not used by GME
            GME_BROWSER_START = 1,      // Right click in the GME Tree Browser window
            GME_CONTEXT_START = 2,		// Using the context menu by right clicking a model element in the GME modeling window
            GME_EMBEDDED_START = 3,		// Not used by GME
            GME_MENU_START = 16,		// Clicking on the toolbar icon, or using the main menu
            GME_BGCONTEXT_START = 18,	// Using the context menu by right clicking the background of the GME modeling window
            GME_ICON_START = 32,		// Not used by GME
            GME_SILENT_MODE = 128 		// Not used by GME, available to testers not using GME
        }

        /// <summary>
        /// This function is called for each interpreter invocation before Main.
        /// Don't perform MGA operations here unless you open a tansaction.
        /// </summary>
        /// <param name="project">The handle of the project opened in GME, for which the interpreter was called.</param>
        public void Initialize(MgaProject project)
        {
            if (Logger == null)
            {
                Logger = new GMELogger(project, this.ComponentName);
            }
        }

        /// <summary>
        /// The main entry point of the interpreter. A transaction is already open,
        /// GMEConsole is available. A general try-catch block catches all the exceptions
        /// coming from this function, you don't need to add it. For more information, see InvokeEx.
        /// </summary>
        /// <param name="project">The handle of the project opened in GME, for which the interpreter was called.</param>
        /// <param name="currentobj">The model open in the active tab in GME. Its value is null if no model is open (no GME modeling windows open). </param>
        /// <param name="selectedobjs">
        /// A collection for the selected model elements. It is never null.
        /// If the interpreter is invoked by the context menu of the GME Tree Browser, then the selected items in the tree browser. Folders
        /// are never passed (they are not FCOs).
        /// If the interpreter is invoked by clicking on the toolbar icon or the context menu of the modeling window, then the selected items 
        /// in the active GME modeling window. If nothing is selected, the collection is empty (contains zero elements).
        /// </param>
        /// <param name="startMode">Contains information about the GUI event that initiated the invocation.</param>
        [ComVisible(false)]
        public void Main(MgaProject project, MgaFCO currentobj, MgaFCOs selectedobjs, ComponentStartMode startMode)
        {
            throw new NotImplementedException("Function Main(MgaProject, MgaFCO, MgaFCOs, ComponentStartMode) not implemented.");
        }


        #region CyPhyGUIs

        /// <summary>
        /// Result of the latest run of this interpreter.
        /// </summary>
        private InterpreterResult result = new InterpreterResult();

        /// <summary>
        /// Parameter of this run.
        /// </summary>
        private IInterpreterMainParameters mainParameters { get; set; }

        /// <summary>
        /// Output directory where all files must be generated
        /// </summary>
        private string OutputDirectory
        {
            get
            {
                return this.mainParameters.OutputDirectory;
            }
        }

        private Queue<Tuple<string, TimeSpan>> runtime = new Queue<Tuple<string, TimeSpan>>();
        private DateTime startTime = DateTime.Now;

        private void UpdateSuccess(string message, bool success)
        {
            this.result.Success = this.result.Success && success;

            this.runtime.Enqueue(new Tuple<string, TimeSpan>(message, DateTime.Now - this.startTime));
            if (success)
            {
                Logger.WriteInfo("{0} : OK", message);
            }
            else
            {
                Logger.WriteError("{0} : FAILED", message);
            }
        }

        /// <summary>
        /// Name of the log file. (It is not a full path)
        /// </summary>
        private string LogFileFilename { get; set; }

        /// <summary>
        /// Full path to the log file.
        /// </summary>
        private string LogFilePath
        {
            get
            {
                return Path.Combine(this.result.LogFileDirectory, this.LogFileFilename);
            }
        }

        public IInterpreterPreConfiguration PreConfig(IPreConfigParameters preConfigParameters)
        {
            return null;
        }

        public IInterpreterConfiguration DoGUIConfiguration(IInterpreterPreConfiguration preconfig, IInterpreterConfiguration previousConfig)
        {
            CyPhy2CADPCB_Settings settings = (previousConfig as CyPhy2CADPCB_Settings);

            if (String.IsNullOrWhiteSpace(settings.LayoutFilePath))
            {
                // string outd = preconfig.ProjectDirectory;

                // Prompt the user for what layout JSON file they want to use.
                // Open file dialog box
                DialogResult dr;
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.CheckFileExists = true;
                    ofd.DefaultExt = "*.json";
                    ofd.Multiselect = false;
                    ofd.Filter = "JSON file (*.json)|*.json";
                    dr = ofd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        settings.SavedLayoutFilePath = ofd.FileName;
                    }
                    else
                    {
                        Logger.WriteError("No file was selected. CAD PCB generation will not complete.", CyPhyGUIs.SmartLogger.MessageType_enum.Error);
                        return null;
                    }
                }
            }
            return settings;
        }

        public IInterpreterResult Main(IInterpreterMainParameters parameters)
        {
            if (parameters.config == null)
            {
                throw new ArgumentNullException("Parameter 'parameters' cannot be null.");
            }
            if (false == (parameters.config is CyPhy2CADPCB_Settings))
            {
                throw new ArgumentException("Parameter 'parameters' is not of type CyPhy2CADPCB_Settings.");
            }

            this.mainParameters = parameters;
            
            try
            {
                // Call into CyPhy2CAD.
                // - Pass same context variables.
                // - Call the Main function so that no GUI is needed.
                // - Catch the "runcommand" that they pass out.

                CyPhy2CAD_CSharp.CyPhy2CAD_CSharpInterpreter cyphy2cad = new CyPhy2CAD_CSharp.CyPhy2CAD_CSharpInterpreter()
                {
                    InteractiveMode = true,   // JS: will be false in the future
                };
                cyphy2cad.Initialize(parameters.Project);

                String auxDir = Path.Combine(parameters.ProjectDirectory,
                                             "CAD");

                var cyphy2cad_parameters = new InterpreterMainParameters()
                {
                    config = new CyPhy2CAD_CSharp.CyPhy2CADSettings()
                    {
                        OutputDirectory = parameters.OutputDirectory,
                        AuxiliaryDirectory = auxDir
                    },
                    CurrentFCO = parameters.CurrentFCO,
                    OutputDirectory = parameters.OutputDirectory,
                    Project = parameters.Project,
                    ProjectDirectory = parameters.ProjectDirectory,
                    SelectedFCOs = parameters.SelectedFCOs                    
                };

                this.Logger.WriteInfo("CyPhy2CADPCB cadauxdir [{0}]", auxDir);

                this.Logger.WriteDebug("Running CyPhy2CAD.Main(...)");
                var cyphy2cad_result = cyphy2cad.Main(cyphy2cad_parameters);
                this.Logger.WriteDebug("Completed CyPhy2CAD.Main(...)");

                this.Logger.WriteInfo("CyPhy2CADPCB Layout.json path: [{0}]", (this.mainParameters.config as CyPhy2CADPCB_Settings).GetLayoutPath);
                GenerateScriptFiles(parameters.OutputDirectory);
                GenerateRunBatFile(parameters.OutputDirectory);

                this.result.RunCommand = "runAddComponentToPcbConstraints.bat";  //cyPhy2CAD_RunCommand
                this.result.Success = true;

                this.Logger.WriteInfo("CyPhy2CADPCB finished successfully.");
            }
            catch (Exception ex)
            {
                this.result.Success = false;
                this.Logger.WriteError("CyPhy2CADPCB has failed! ", ex.ToString());
            }
            finally
            {
                this.Logger.WriteInfo("Generated files are here: <a href=\"file:///{0}\" target=\"_blank\">{0}</a>", parameters.OutputDirectory);
            }
            return this.result;
        }

        private void GenerateRunBatFile(String OutDir)
        {
            this.Logger.WriteInfo("GenerateRunBatFile()  Generating: [{0}]...", Path.Combine(OutDir, "runAddComponentToPcbConstraints.bat"));
            StreamWriter file = new StreamWriter(Path.Combine(OutDir, "runAddComponentToPcbConstraints.bat"));
            //file.WriteLine("python Synthesize_PCB_CAD_connections.py {0}", layoutFilePath);  // replaced by fancy META python path code below

            var pcb_bat = CyPhy2CADPCB.Properties.Resources.runCreateCADAssembly;
            var pcb_bat_toOutput = String.Format(pcb_bat, 
                                                 (this.mainParameters.config as CyPhy2CADPCB_Settings).GetLayoutPath);

            file.Write(pcb_bat_toOutput);
            file.Close();
        }

        public void GenerateScriptFiles(String OutDir)
        {
            this.Logger.WriteInfo("GenerateScriptFiles()  Generating: [{0}]...", Path.Combine(OutDir, "Synthesize_PCB_CAD_connections.py"));

            var pcb_python = CyPhy2CADPCB.Properties.Resources.Synthesize_PCB_CAD_connections;
            using (StreamWriter writer = new StreamWriter(Path.Combine(OutDir, "Synthesize_PCB_CAD_connections.py")))
            {
                writer.Write(Encoding.UTF8.GetString(pcb_python));
            }
        }

        /// <summary>
        /// ProgId of the configuration class of this interpreter.
        /// </summary>
        public string InterpreterConfigurationProgId
        {
            get
            {
                return (typeof(CyPhy2CADPCB_Settings).GetCustomAttributes(typeof(ProgIdAttribute), false)[0] as ProgIdAttribute).Value;
            }
        }

        #endregion

        #region IMgaComponentEx Members

        MgaGateway MgaGateway { get; set; }
        GMELogger Logger { get; set; }

        public void InvokeEx(MgaProject project, MgaFCO currentobj, MgaFCOs selectedobjs, int param)
        {
            if (!enabled)
            {
                return;
            }

            try
            {
                if (Logger == null)
                {
                    Logger = new GMELogger(project, this.ComponentName);
                }
                MgaGateway = new MgaGateway(project);
                project.CreateTerritoryWithoutSink(out MgaGateway.territory);

                MgaGateway.PerformInTransaction(delegate
                {
                    Main(project, currentobj, selectedobjs, Convert(param));
                });
            }
            finally
            {
                if (MgaGateway.territory != null)
                {
                    MgaGateway.territory.Destroy();
                }
                MgaGateway = null;
                project = null;
                currentobj = null;
                selectedobjs = null;
                if (Logger != null)
                {
                    Logger.Dispose();
                }
                Logger = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private ComponentStartMode Convert(int param)
        {
            switch (param)
            {
                case (int)ComponentStartMode.GME_BGCONTEXT_START:
                    return ComponentStartMode.GME_BGCONTEXT_START;
                case (int)ComponentStartMode.GME_BROWSER_START:
                    return ComponentStartMode.GME_BROWSER_START;

                case (int)ComponentStartMode.GME_CONTEXT_START:
                    return ComponentStartMode.GME_CONTEXT_START;

                case (int)ComponentStartMode.GME_EMBEDDED_START:
                    return ComponentStartMode.GME_EMBEDDED_START;

                case (int)ComponentStartMode.GME_ICON_START:
                    return ComponentStartMode.GME_ICON_START;

                case (int)ComponentStartMode.GME_MAIN_START:
                    return ComponentStartMode.GME_MAIN_START;

                case (int)ComponentStartMode.GME_MENU_START:
                    return ComponentStartMode.GME_MENU_START;
                case (int)ComponentStartMode.GME_SILENT_MODE:
                    return ComponentStartMode.GME_SILENT_MODE;
            }

            return ComponentStartMode.GME_SILENT_MODE;
        }

        #region Component Information
        public string ComponentName
        {
            get { return GetType().Name; }
        }

        public string ComponentProgID
        {
            get
            {
                return ComponentConfig.progID;
            }
        }

        public componenttype_enum ComponentType
        {
            get { return ComponentConfig.componentType; }
        }
        public string Paradigm
        {
            get { return ComponentConfig.paradigmName; }
        }
        #endregion

        #region Enabling
        bool enabled = true;
        public void Enable(bool newval)
        {
            enabled = newval;
        }
        #endregion

        #region Interactive Mode
        protected bool interactiveMode = true;
        public bool InteractiveMode
        {
            get
            {
                return interactiveMode;
            }
            set
            {
                interactiveMode = value;
            }
        }
        #endregion

        #region Custom Parameters
        SortedDictionary<string, object> componentParameters = null;

        public object get_ComponentParameter(string Name)
        {
            if (Name == "type")
                return "csharp";

            if (Name == "path")
                return GetType().Assembly.Location;

            if (Name == "fullname")
                return GetType().FullName;

            object value;
            if (componentParameters != null && componentParameters.TryGetValue(Name, out value))
            {
                return value;
            }

            return null;
        }

        public void set_ComponentParameter(string Name, object pVal)
        {
            if (componentParameters == null)
            {
                componentParameters = new SortedDictionary<string, object>();
            }

            componentParameters[Name] = pVal;
        }
        #endregion

        #region Unused Methods
        // Old interface, it is never called for MgaComponentEx interfaces
        public void Invoke(MgaProject Project, MgaFCOs selectedobjs, int param)
        {
            throw new NotImplementedException();
        }

        // Not used by GME
        public void ObjectsInvokeEx(MgaProject Project, MgaObject currentobj, MgaObjects selectedobjs, int param)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region IMgaVersionInfo Members

        public GMEInterfaceVersion_enum version
        {
            get { return GMEInterfaceVersion_enum.GMEInterfaceVersion_Current; }
        }

        #endregion

        #region Registration Helpers

        [ComRegisterFunctionAttribute]
        public static void GMERegister(Type t)
        {
            Registrar.RegisterComponentsInGMERegistry();

        }

        [ComUnregisterFunctionAttribute]
        public static void GMEUnRegister(Type t)
        {
            Registrar.UnregisterComponentsInGMERegistry();
        }

        #endregion

        [Serializable]
        [ComVisible(true)]
        [ProgId("ISIS.META.CADPCBConfig")]
        [Guid("98347693-FC33-4F1E-A2C0-8E97C41B23D4")]
        public class CADPCBConfig : IInterpreterConfiguration, IInterpreterPreConfiguration
        {
            public string ProjectDirectory { get; set; }
            public string AuxiliaryDirectory { get; set; }
            public bool UseProjectManifest { get; set; }
            public List<string> StepFormats { get; set; }
        }

    }
}
