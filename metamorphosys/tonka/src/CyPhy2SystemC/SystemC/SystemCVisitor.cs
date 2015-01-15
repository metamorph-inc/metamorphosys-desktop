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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CyPhy2SystemC.SystemC
{
    class SystemCVisitor : Visitor
    {
        private StringBuilder includeLines;
        private StringBuilder prologLines;
        private StringBuilder signalDefLines;
        private StringBuilder moduleDefLines;
        private StringBuilder wiringLines;
        private StringBuilder traceLines;
        private StringBuilder epilogLines;

        private SortedSet<SourceFile> sources;
        private string OutputDirectory;

        public SortedSet<SourceFile> Sources
        {
            get
            {
                return sources;
            }
        }

        public SystemCVisitor(string OutputDirectory)
        {
            this.OutputDirectory = OutputDirectory;
            includeLines = new StringBuilder();
            prologLines = new StringBuilder();
            signalDefLines = new StringBuilder();
            moduleDefLines = new StringBuilder();
            wiringLines = new StringBuilder();
            traceLines = new StringBuilder();
            epilogLines = new StringBuilder();

            sources = new SortedSet<SourceFile>();
        }


        /// <summary>
        /// Gets an epilog string with an sc_start() function call.
        /// </summary>
        /// <param name="obj">The testbench object possibly containing a "simulationTime" parameter.</param>
        /// <returns>A string containing an sc_start function call, with a time-limit parameter.</returns>
        /// <seealso>MOT-516: Modify CyPhy2SystemC interpreter to actually use the "simulationTime" parameter.
        public string getScStartString(TestBench obj)
        {
            string rVal = "\tsc_start();\n";
            string timeLimitParamName = "simulationTime";
            double number = 0;
            string scTimeUnitsString = "SC_SEC";
            try
            {
                if ( obj.TestParameters.ContainsKey( timeLimitParamName ) )
                {
                    number = Double.Parse( obj.TestParameters[ timeLimitParamName ].Attributes.Value );
                    switch (obj.TestParameters[timeLimitParamName].AllReferred.Name)
                    {
                        // As of 2014, the smallest time unit in GME's QUDT unit library is the Microsecond.
                        case "Femptosecond":
                            scTimeUnitsString = "SC_FS";
                            break;
                        case "Picosecond":
                            scTimeUnitsString = "SC_PS";
                            break;
                        case "Nanosecond":
                            scTimeUnitsString = "SC_NS";
                            break;
                        case "Microsecond":
                            scTimeUnitsString = "SC_US";
                            break;
                        case "Millisecond":
                            scTimeUnitsString = "SC_MS";
                            break;
                        case "Second":
                            scTimeUnitsString = "SC_SEC";
                            break;
                        default:
                            scTimeUnitsString = "SC_SEC";
                            break;
                    }   
                }
                rVal = String.Format("\tsc_start( {0}, {1} );\n", number, scTimeUnitsString);
            }
            catch
            {
                rVal = String.Format("\t// Unable to parse the {0} parameter.\n", timeLimitParamName) + rVal;
            }
            return rVal;
        }

        public override void visit(TestBench obj)
        {
            if (CodeGenerator.verbose) CodeGenerator.GMEConsole.Info.WriteLine("Generate TestBench: {0}", obj.Name);

            includeLines.AppendFormat("// TestBench generated by SystemC Composer from: {0} ({1})\n", obj.Impl.Impl.Project.ParadigmConnStr, obj.Name);
            includeLines.Append("// Copyright (c) 2014 MetaMorph, Inc.\n\n");
            includeLines.Append("#include <stdlib.h>\n");
            includeLines.Append("#include <time.h>\n");
            includeLines.Append("#include <systemc.h>\n\n");

            prologLines.Append("int sc_main(int argc, char *argv[]) {\n");

            traceLines.Append("\tsc_trace_file *vcd_log = sc_create_vcd_trace_file(\"SystemCTestBench\");\n");

            epilogLines.Append("\tsrand((unsigned int)(time(NULL) & 0xffffffff));\n");
            epilogLines.Append( getScStartString( obj ) );
            epilogLines.Append("\tsc_close_vcd_trace_file(vcd_log);\n\n");
            if (obj.TestComponents.Count == 1)
            {
                epilogLines.AppendFormat("\treturn i_{0}.error_cnt;\n", obj.TestComponents.FirstOrDefault().Name);
            }
            else
            {
                epilogLines.AppendFormat("\treturn 0; // More then one test component is used.\n");
            }
            epilogLines.Append("}\n");
        }

        public override void visit(Component obj)
        {
            if (!obj.HasSystemCModel)
            {
                return;
            }

            if (obj is ArduinoComponent)
            {
                ArduinoComponent arduinoObj = obj as ArduinoComponent;
                if (CodeGenerator.verbose) CodeGenerator.GMEConsole.Info.WriteLine("Generate {0} Module: {1} with firmware {2}", arduinoObj.ArduinoModule, arduinoObj.Name, arduinoObj.FirmwarePath);

                foreach (var source in obj.Sources)
                {
                    if (source.Type == SourceFile.SourceType.Header)
                    {
                        includeLines.AppendFormat("#include <{0}>\n", source.Path);
                    }
                    sources.Add(source);
                }
                includeLines.AppendFormat("namespace {0} {{\n", arduinoObj.Namespace);
                includeLines.AppendFormat("using namespace ArduinoAPI;\n");
                includeLines.AppendFormat("#include  \"{0}\"\n", arduinoObj.FirmwarePath);
                includeLines.AppendFormat("}} // namespace {0}\n", arduinoObj.Namespace);


                moduleDefLines.AppendFormat("\t{0} i_{1}(\"{1}\", {2}, {3});\n", arduinoObj.ArduinoModule, arduinoObj.Name, arduinoObj.SetupFunction, arduinoObj.LoopFunction);
            }
            else
            {

                if (CodeGenerator.verbose) CodeGenerator.GMEConsole.Info.WriteLine("Generate Module: {0}", obj.Name);

                foreach (var source in obj.Sources)
                {
                    if (source.Type == SourceFile.SourceType.Header)
                    {
                        includeLines.AppendFormat("#include <{0}>\n", source.Path);
                    }
                    sources.Add(source);
                    string fullPath = Path.Combine(obj.ComponentDirectory, source.Path);
                    if (String.IsNullOrWhiteSpace(obj.ComponentDirectory) == false && File.Exists(fullPath))
                    {
                        Directory.CreateDirectory(Path.Combine(this.OutputDirectory, Path.GetDirectoryName(source.Path)));
                        File.Copy(fullPath, Path.Combine(this.OutputDirectory, source.Path));
                    }
                }

                moduleDefLines.AppendFormat("\t{0} i_{0}(\"{0}\");\n", obj.Name);
            }
        }

        public override void visit(Port obj)
        {
            if (CodeGenerator.verbose) CodeGenerator.GMEConsole.Info.WriteLine("Generate Port: {0}", obj.Name);

            if (obj.DstConnections.Count > 0)
            {
                var signalConn = obj.DstConnections.FirstOrDefault();

                if (CodeGenerator.verbose) CodeGenerator.GMEConsole.Info.WriteLine("Generate Signal: {0}", signalConn.Name);
                signalDefLines.AppendFormat("\tsc_signal<{0}> {1};\n", signalConn.DataType, signalConn.Name);
                traceLines.AppendFormat("\tsc_trace(vcd_log, {0}, \"{0}\");\n", signalConn.Name);
                wiringLines.AppendFormat("\ti_{0}.{1}({2});\n", obj.Parent.Name, obj.Name, signalConn.Name);

                foreach (var conn in obj.DstConnections)
                {
                    wiringLines.AppendFormat("\ti_{0}.{1}({2});\n", conn.DstPort.Parent.Name, conn.DstPort.Name, signalConn.Name);
                }
                wiringLines.Append("\n");
            }
            else
            {
                if (obj.IsOutput)
                {
                    CodeGenerator.GMEConsole.Warning.WriteLine("Output port is not connected: <a href=\"mga:{0}\">{1}</a>", obj.Impl.ID, obj.Impl.Path);
                    var signalName = obj.Name + obj.Impl.ID.Replace('-', '_') + "_nc";
                    signalDefLines.AppendFormat("\tsc_signal<{0}> {1};\n", obj.DataType, signalName);
                    wiringLines.AppendFormat("\ti_{0}.{1}({2});\n", obj.Parent.Name, obj.Name, signalName);
                    traceLines.AppendFormat("\tsc_trace(vcd_log, {0}, \"{0}\");\n", signalName);
                }
            }
        }
       

        public void WriteFile(string filePath)
        {
            using (var f = new StreamWriter(filePath)) {
                f.WriteLine(includeLines);
                f.WriteLine(prologLines);
                f.WriteLine(signalDefLines);
                f.WriteLine(moduleDefLines);
                f.WriteLine(wiringLines);
                f.WriteLine(traceLines);
                f.WriteLine(epilogLines);
                f.Close();
            }   
        }
    }
}
