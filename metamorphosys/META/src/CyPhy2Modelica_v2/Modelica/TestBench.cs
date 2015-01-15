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

=======================
This version of the META tools is a fork of an original version produced
by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
Their license statement:

Copyright (C) 2011-2014 Vanderbilt University

Developed with the sponsorship of the Defense Advanced Research Projects
Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
as defined in DFARS 252.227-7013.

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

using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2Modelica_v2.Modelica
{
    public class TestBench : ModelBase<CyPhy.TestBench>
    {
        public TestBench(CyPhy.TestBench impl)
            : base(impl)
        {
            ComponentAssemblies = new SortedSet<ComponentAssembly>();
            TestComponents = new SortedSet<Component>();

            ComponentAssemblyInstances = new SortedSet<ComponentAssemblyInstance>();
            TestComponentInstances = new SortedSet<ComponentInstance>();
            Connections = new SortedSet<Connection>();
            Environments = new SortedSet<Environment>();
            Parameters = new SortedSet<UnitParameter>();
            Metrics = new SortedSet<Metric>();
            Limits = new SortedSet<Limit>();
        }

        public string FullName { get; set; }
        public SortedSet<ComponentAssembly> ComponentAssemblies { get; set; }
        public SortedSet<Component> TestComponents { get; set; }
        public SortedSet<ComponentAssemblyInstance> ComponentAssemblyInstances { get; set; }
        public SortedSet<ComponentInstance> TestComponentInstances { get; set; }
        public SortedSet<Connection> Connections { get; set; }
        public SortedSet<Environment> Environments { get; set; }
        public SortedSet<UnitParameter> Parameters { get; set; }
        public SortedSet<Metric> Metrics { get; set; }
        public SortedSet<Limit> Limits { get; set; }
        public int CanvasXMax { get; set; }
        public int CanvasYMax { get; set; }
        public Modelica.SolverSettings SolverSettings { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("within {0}.{1};", CodeGenerator.MainPackage, "TestBenches"));
            sb.AppendLine(string.Format("model {0}", Name));
            sb.AppendLine("  //Parameters");
            foreach (var parameter in Parameters)
            {
                sb.AppendLine(string.Format("  parameter {0} {1}{2}={3};", parameter.Value.ClassName, parameter.Name, parameter.Modifier, parameter.Value.Value));
            }
            sb.AppendLine();
            sb.AppendLine("  //Metrics");
            foreach (var metric in Metrics)
            {
                if (metric.PostProcessing)
                {
                    sb.AppendLine(string.Format("  Real {0}=0 \"{1}{2}\";", metric.Name, "PostProcessing : ", metric.Description));
                }
                else
                {
                    sb.AppendLine("  // Only PostProcessing Metrics supported at this time.");
                }
            }
            sb.AppendLine();
            sb.AppendLine("  //Environments");
            foreach (var environment in Environments)
            {
                sb.Append(environment.ToString());
            }
            sb.AppendLine();
            sb.AppendLine("  //ComponentAssemblies");
            foreach (var componentAssemblyInstance in ComponentAssemblyInstances)
            {

                sb.AppendLine(string.Format("   {0} {1} {2};",
                    componentAssemblyInstance.InstanceOf.FullName, componentAssemblyInstance.ToString(), componentAssemblyInstance.Annotation));
            }
            sb.AppendLine();
            sb.AppendLine("  //TestComponents");
            foreach (var testComponentInstance in TestComponentInstances)
            {
                sb.AppendLine(string.Format("  {0} {1} {2};",
                    testComponentInstance.InstanceOf.FullName, testComponentInstance.ToString(), testComponentInstance.Annotation));
            }

            sb.AppendLine("equation");
            foreach (var connection in Connections)
            {
                sb.AppendLine(connection.ToString());
            }

            this.GetAnnotation(sb);

            sb.AppendLine(string.Format("end {0};", Name));

            return sb.ToString();
        }

        private void GetAnnotation(StringBuilder sb)
        {
            int xMin = 0;
            int xMax = CanvasXMax / CodeGenerator.ScaleFactor;
            int yMin = -CanvasYMax / CodeGenerator.ScaleFactor;
            int yMax = 0;

            int xMiddle = (xMax + xMin) / 2;
            int yMiddle = (yMax + yMin) / 2;

            int width = Math.Min(yMax - yMin, xMax - xMin);
            int height = Math.Min(yMax - yMin, xMax - xMin);


            sb.AppendLine();
            sb.AppendLine(" // Annotations");
            sb.AppendFormat("annotation (Documentation(info=\"<HTML><p>Generated test bench from CyPhy using the META tools. Tool Version: {0} Interpreter Version: {1}</p></HTML>\"),", META.VersionInfo.MetaVersion.Replace("\\", "\\\\"), System.Reflection.Assembly.GetExecutingAssembly().FullName);
            sb.AppendFormat("experiment(StartTime={0}, StopTime={1}, Algorithm=\"{2}\"",
                    SolverSettings.StartTime, SolverSettings.StopTime, SolverSettings.DymolaSolver);
            
            if (SolverSettings.Tolerance > 0)
            {
                sb.Append(", Tolerance=" + SolverSettings.Tolerance);
            }

            if (SolverSettings.UsesNumberOfIntervals && SolverSettings.NumberOfIntervals > 0)
            {
                sb.Append(", NumberOfIntervals=" + SolverSettings.NumberOfIntervals);
            }
            else if (SolverSettings.UsesNumberOfIntervals == false && SolverSettings.IntervalLength > 0)
            {
                sb.Append(", Interval=" + SolverSettings.IntervalLength);
            }
            sb.AppendLine("),");

            sb.AppendLine("Icon(coordinateSystem(preserveAspectRatio=true, extent={{" + xMin + "," + yMin + "},{" + xMax + "," + yMax + "}}),");
            sb.AppendLine("  graphics={");

            // green tick
            sb.AppendLine("    Line(");
            sb.AppendLine("      points={{" +
                (xMiddle - width / 2) + "," + (yMiddle - 0.05 * height) + "},{" +
                (xMiddle - 0.65 * width / 2) + "," + (yMiddle - 0.25 * height) + "},{" +
                (xMiddle - 0.05 * width / 2) + "," + (yMiddle + 0.25 * height) + "}},");
            sb.AppendLine("      color={0,255,0},");
            sb.AppendLine("      smooth=Smooth.None,");
            sb.AppendLine("      thickness=0.5),");

            // red X
            sb.AppendLine("    Line(");
            sb.AppendLine("      points={{" +
                (xMiddle) + "," + (yMiddle - 0.25 * height) + "},{" +
                (xMiddle + width / 2) + "," + (yMiddle + 0.25 * height) + "}},");
            sb.AppendLine("      color={255,0,0},");
            sb.AppendLine("      smooth=Smooth.None,");
            sb.AppendLine("      thickness=0.5),");
            sb.AppendLine("    Line(");
            sb.AppendLine("      points={{" +
                (xMiddle) + "," + (yMiddle + 0.25 * height) + "},{" +
                (xMiddle + width / 2) + "," + (yMiddle - 0.25 * height) + "}},");
            sb.AppendLine("      color={255,0,0},");
            sb.AppendLine("      smooth=Smooth.None,");
            sb.AppendLine("      thickness=0.5)}),");
            sb.AppendLine("  Diagram(coordinateSystem(preserveAspectRatio=true, extent={{" + xMin + "," + yMin + "},{" + xMax + "," + yMax + "}})));");
        }
    }

}
