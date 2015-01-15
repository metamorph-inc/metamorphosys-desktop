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
using System.Runtime.InteropServices;
using System.Text;
using GME.CSharp;
using GME;
using GME.MGA;
using GME.MGA.Core;
using System.Linq;
using META;

using CyPhyGUIs;

using Tonka = ISIS.GME.Dsml.CyPhyML.Interfaces;
using TonkaClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;

namespace CyPhy2Schematic
{
    /// <summary>
    /// This class implements the necessary COM interfaces for a GME interpreter component.
    /// </summary>
    [Guid(ComponentConfig.guid),
    ProgId(ComponentConfig.progID),
    ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class CyPhy2SchematicInterpreter : IMgaComponentEx, IGMEVersionInfo, ICyPhyInterpreter
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
        /// Don't perform MGA operations here unless you open a transaction.
        /// </summary>
        /// <param name="project">The handle of the project opened in GME, for which the interpreter was called.</param>
        public void Initialize(MgaProject project)
        {
            if (Logger == null)
                Logger = new GMELogger(project, ComponentName);
            MgaGateway = new MgaGateway(project);
            project.CreateTerritoryWithoutSink(out MgaGateway.territory);
        }

        private CyPhy2Schematic_Settings InitializeSettingsFromWorkflow(CyPhy2Schematic_Settings settings)
        {
            // Seed with settings from workflow.
            String str_WorkflowParameters = "";
            try
            {
                MgaGateway.PerformInTransaction(delegate
                {
                    var testBench = TonkaClasses.TestBench.Cast(this.mainParameters.CurrentFCO);
                    var workflowRef = testBench.Children.WorkflowRefCollection.FirstOrDefault();
                    var workflow = workflowRef.Referred.Workflow;
                    var taskCollection = workflow.Children.TaskCollection;
                    var myTask = taskCollection.FirstOrDefault(t => t.Attributes.COMName == this.ComponentProgID);
                    str_WorkflowParameters = myTask.Attributes.Parameters;
                },
                transactiontype_enum.TRANSACTION_NON_NESTED,
                abort: true
                );

                Dictionary<string, string> dict_WorkflowParameters = (Dictionary<string, string>)
                    Newtonsoft.Json.JsonConvert.DeserializeObject(str_WorkflowParameters, typeof(Dictionary<string, string>));

                if (dict_WorkflowParameters != null)
                {
                    settings = new CyPhy2Schematic_Settings();
                    foreach (var property in settings.GetType().GetProperties()
                                                               .Where(p => p.GetCustomAttributes(typeof(WorkflowConfigItemAttribute), false).Any())
                                                               .Where(p => dict_WorkflowParameters.ContainsKey(p.Name)))
                    {
                        property.SetValue(settings, dict_WorkflowParameters[property.Name], null);
                    }
                }
            }
            catch (NullReferenceException)
            {
                Logger.WriteInfo("Could not find workflow object for CyPhy2Schematic interpreter.");
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                Logger.WriteWarning("Workflow Parameter has invalid Json String: {0}", str_WorkflowParameters);
            }

            return settings;
        }



        #region IMgaComponentEx Members

        private MgaGateway MgaGateway { get; set; }
        private GMELogger Logger { get; set; }

        public void InvokeEx(MgaProject project, MgaFCO currentobj, MgaFCOs selectedobjs, int param)
        {
            if (this.enabled == false)
            {
                return;
            }

            try
            {
                // Need to call this interpreter in the same way as the MasterInterpreter will call it.
                // initialize main parameters
                var parameters = new InterpreterMainParameters()
                {
                    Project = project,
                    CurrentFCO = currentobj,
                    SelectedFCOs = selectedobjs,
                    StartModeParam = param
                };

                this.mainParameters = parameters;
                parameters.ProjectDirectory = project.GetRootDirectoryPath();

                // set up the output directory
                MgaGateway.PerformInTransaction(delegate
                {
                    string outputDirName = project.Name;
                    if (currentobj != null)
                    {
                        outputDirName = currentobj.Name;
                    }

                    var outputDirAbsPath = Path.GetFullPath(Path.Combine(
                                                            parameters.ProjectDirectory,
                                                            "results",
                                                            outputDirName));

                    parameters.OutputDirectory = outputDirAbsPath;

                    if (Directory.Exists(outputDirAbsPath))
                    {
                        Logger.WriteWarning("Output directory {0} already exists. Unexpected behavior may result.", outputDirAbsPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(outputDirAbsPath);
                    }

                    //this.Parameters.PackageName = Schematic.Factory.GetModifiedName(currentobj.Name);
                });

                PreConfigArgs preConfigArgs = new PreConfigArgs()
                {
                    ProjectDirectory = parameters.ProjectDirectory,
                    Project = parameters.Project
                };

                // call the preconfiguration with no parameters and get preconfig
                var preConfig = this.PreConfig(preConfigArgs);
                
                // get previous GUI config
                var settings_ = META.ComComponent.DeserializeConfiguration(parameters.ProjectDirectory,
                                                                           typeof(CyPhy2Schematic_Settings),
                                                                           this.ComponentProgID);
                CyPhy2Schematic_Settings settings = (settings_ != null) ? settings_ as CyPhy2Schematic_Settings : new CyPhy2Schematic_Settings();

                // Set configuration based on Workflow Parameters. This will override all [WorkflowConfigItem] members.
                settings = InitializeSettingsFromWorkflow(settings);
                
                // Don't skip GUI -- we've been invoked directly here.
                settings.skipGUI = null;

                // get interpreter config through GUI
                var config = this.DoGUIConfiguration(preConfig, settings);
                if (config == null)
                {
                    Logger.WriteWarning("Operation canceled by the user.");
                    return;
                }

                // if config is valid save it and update it on the file system
                META.ComComponent.SerializeConfiguration(parameters.ProjectDirectory, config, this.ComponentProgID);

                // assign the new configuration to mainParameters
                parameters.config = config;

                // call the main (ICyPhyComponent) function
                this.Main(parameters);
            }
            catch (Exception ex)
            {
                Logger.WriteError("Interpretation failed {0}<br>{1}", ex.Message, ex.StackTrace);
            }
            finally
            {
                if (MgaGateway != null &&
                    MgaGateway.territory != null)
                {
                    MgaGateway.territory.Destroy();
                }
                DisposeLogger();
                MgaGateway = null;
                project = null;
                currentobj = null;
                selectedobjs = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
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

        #region Dependent Interpreters

        private bool CallElaborator(
            MgaProject project,
            MgaFCO currentobj,
            MgaFCOs selectedobjs,
            int param,
            bool expand = true)
        {
            bool result = false;
            try
            {
                this.Logger.WriteDebug("Elaborating model...");
                var elaborator = new CyPhyElaborateCS.CyPhyElaborateCSInterpreter();
                elaborator.Initialize(project);
                int verbosity = 128;
                result = elaborator.RunInTransaction(project, currentobj, selectedobjs, verbosity);

                if (this.result.Traceability == null)
                {
                    this.result.Traceability = new META.MgaTraceability();
                }

                if (elaborator.Traceability != null)
                {
                    elaborator.Traceability.CopyTo(this.result.Traceability);
                }
                this.Logger.WriteDebug("Elaboration is done.");
            }
            catch (Exception ex)
            {
                this.Logger.WriteError("Exception occurred in Elaborator : {0}", ex.ToString());
                result = false;
            }

            return result;
        }

        #endregion

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
        /// ProgId of the configuration class of this interpreter.
        /// </summary>
        public string InterpreterConfigurationProgId
        {
            get
            {
                return (typeof(CyPhy2Schematic_Settings).GetCustomAttributes(typeof(ProgIdAttribute), false)[0] as ProgIdAttribute).Value;
            }
        }

        /// <summary>
        /// Preconfig gets called first. No transaction is open, but one may be opened.
        /// In this function model may be processed and some object ids get serialized
        /// and returned as preconfiguration (project-wise configuration).
        /// </summary>
        /// <param name="preConfigParameters"></param>
        /// <returns>Null if no configuration is required by the DoGUIConfig.</returns>
        public IInterpreterPreConfiguration PreConfig(IPreConfigParameters preConfigParameters)
        {
            //var preConfig = new CyPhy2Schematic_v2PreConfiguration()
            //{
            //    ProjectDirectory = preConfigParameters.ProjectDirectory
            //};

            //return preConfig;
            return null;
        }

        /// <summary>
        /// Shows a form for the user to select/change settings through a GUI. All interactive 
        /// GUI operations MUST happen within this function scope.
        /// </summary>
        /// <param name="preConfig">Result of PreConfig</param>
        /// <param name="previousConfig">Previous configuration to initialize the GUI.</param>
        /// <returns>Null if operation is canceled by the user. Otherwise returns with a new
        /// configuration object.</returns>
        public IInterpreterConfiguration DoGUIConfiguration(
            IInterpreterPreConfiguration preConfig,
            IInterpreterConfiguration previousConfig)
        {
            CyPhy2Schematic_Settings settings = (previousConfig as CyPhy2Schematic_Settings);

            // If none found, we should do GUI.
            // If available, seed the GUI with the previous settings.
            if (settings == null || settings.skipGUI == null)
            {
                // Do GUI
                var gui = new CyPhy2Schematic.GUI.CyPhy2Schematic_GUI();
                gui.settings = settings;

                var result = gui.ShowDialog();

                if (result == DialogResult.OK)
                {
                    return gui.settings;
                }
                else
                {
                    // USER CANCELED.
                    return null;
                }
            }

            return settings;
        }

        private Queue<Tuple<string, TimeSpan>> runtime = new Queue<Tuple<string, TimeSpan>>();
        private DateTime startTime = DateTime.Now;

        /// <summary>
        /// No GUI and interactive elements are allowed within this function.
        /// </summary>
        /// <param name="parameters">Main parameters for this run and GUI configuration.</param>
        /// <returns>Result of the run, which contains a success flag.</returns>
        public IInterpreterResult Main(IInterpreterMainParameters parameters)
        {
            if (Logger == null)
                Logger = new GMELogger(parameters.Project, ComponentName);

            this.runtime.Clear();
            this.mainParameters = parameters;
            var configSuccess = this.mainParameters != null;
            this.UpdateSuccess("Configuration", configSuccess);
            this.result.Labels = "Schematic";

            try
            {
                MgaGateway.PerformInTransaction(delegate
                {
                    this.WorkInMainTransaction();
                },
                transactiontype_enum.TRANSACTION_NON_NESTED,
                abort: true
                );

                this.PrintRuntimeStatistics();
                if (this.result.Success)
                {
                    Logger.WriteInfo("Generated files are here: <a href=\"file:///{0}\" target=\"_blank\">{0}</a>", this.mainParameters.OutputDirectory);
                    Logger.WriteInfo("Schematic Interpreter has finished. [SUCCESS: {0}, Labels: {1}]", this.result.Success, this.result.Labels);
                }
                else
                {
                    Logger.WriteError("Schematic Interpreter failed! See error messages above.");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError("Exception: {0}<br> {1}", ex.Message, ex.StackTrace);
            }
            finally
            {
                if (MgaGateway != null &&
                    MgaGateway.territory != null)
                {
                    MgaGateway.territory.Destroy();
                }
                DisposeLogger();
                MgaGateway = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            META.Logger.RemoveFileListener(this.ComponentName);

            var SchematicSettings = this.mainParameters.config as CyPhy2Schematic_Settings;

            return this.result;
        }

        public void DisposeLogger()
        {
            if (Logger != null)
                Logger.Dispose();
            Logger = null;
        }

        private void PrintRuntimeStatistics()
        {
            Logger.WriteDebug("======================================================");
            Logger.WriteDebug("Start time: {0}", this.startTime);
            foreach (var time in this.runtime)
            {
                Logger.WriteDebug("{0} = {1}", time.Item1, time.Item2);
            }
            Logger.WriteDebug("======================================================");
        }

        #endregion

        #region CyPhy2Schematic Specific code

        /// <summary>
        /// This function does the job. CyPhy2Schematic translation.
        /// </summary>
        private void WorkInMainTransaction()
        {
            var config = (this.mainParameters.config as CyPhy2Schematic_Settings);

            this.result.Success = true;
            Schematic.CodeGenerator.Mode mode = Schematic.CodeGenerator.Mode.EDA;

            if (config.doSpice != null)
            {
                this.result.RunCommand = "runspice.bat";
                mode = Schematic.CodeGenerator.Mode.SPICE;
            }
            else if (config.doSpiceForSI != null)
            {
                this.result.RunCommand = "runspice.bat";
                mode = Schematic.CodeGenerator.Mode.SPICE_SI;
            }
            else
            {
                mode = Schematic.CodeGenerator.Mode.EDA;
                if (config.doChipFit != null)
                {
                    Boolean chipFitViz = false;
                    if (Boolean.TryParse(config.showChipFitVisualizer, out chipFitViz)
                        && chipFitViz)
                    {
                        this.result.RunCommand = "chipfit.bat chipfit_display";
                    }
                    else
                    {
                        this.result.RunCommand = "chipFit.bat";
                    }
                }
                else if (config.doPlaceRoute != null)
                {
                    this.result.RunCommand = "placement.bat";
                }
                else if (config.doPlaceOnly != null)
                {
                    this.result.RunCommand = "placeonly.bat";
                }
                else
                {
                    this.result.RunCommand = "dir";
                }
            }

            // Call Elaborator
            var elaboratorSuccess = this.CallElaborator(this.mainParameters.Project,
                                                        this.mainParameters.CurrentFCO, 
                                                        this.mainParameters.SelectedFCOs,
                                                        this.mainParameters.StartModeParam);
            this.UpdateSuccess("Elaborator", elaboratorSuccess);

            bool successTranslation = true;
            try
            {
                Schematic.CodeGenerator.Logger = Logger;
                var schematicCodeGenerator = new Schematic.CodeGenerator(this.mainParameters, mode);
                
                var gcResult = schematicCodeGenerator.GenerateCode();
                
                if (mode == Schematic.CodeGenerator.Mode.EDA &&
                    (config.doPlaceRoute != null || config.doPlaceOnly != null))
                {
                    this.result.RunCommand += gcResult.runCommandArgs;
                }
                
                successTranslation = true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.ToString());
                successTranslation = false;
            }
            this.UpdateSuccess("Schematic translation", successTranslation);
        }

        #endregion

    }
}
