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

using GME;
using GME.CSharp;
using GME.MGA;
using GME.MGA.Core;
using GME.Util;
using Cyber = ISIS.GME.Dsml.CyberComposition.Interfaces;
using CyberClasses = ISIS.GME.Dsml.CyberComposition.Classes;
using Simulink = ISIS.GME.Dsml.CyberComposition.Simulink.Interfaces;
using SimulinkClasses = ISIS.GME.Dsml.CyberComposition.Simulink.Classes;
using SignalFlow = ISIS.GME.Dsml.CyberComposition.SignalFlow.Interfaces;
using SignalFlowClasses = ISIS.GME.Dsml.CyberComposition.SignalFlow.Classes;
using avm;

namespace Cyber2AVM
{
    public class AVMComponentBuilder
    {
        private avm.Component _avmComponent = new avm.Component();

        private int _idCounter = 0;

        public GMEConsole GMEConsole { get; set; }

        public avm.Component getAVMComponent()
        {
            return _avmComponent;
        }

        private void createAVMCyberParameter(List<avm.modelica.Parameter> avmCyberModelParameterList, Cyber.ParameterRef cyberModelParameterRef)
        {
            string idString = "ID" + _idCounter;
            _idCounter++;

            // Initialize a avmCyber Parameter
            avm.modelica.Parameter avmCyberParameter = new avm.modelica.Parameter();
            
            ////
            // Set avm cyber parameter Value
            avmCyberParameter.Value = new avm.Value();
            
            // Assign ValueExpressionType for avm Value
            avm.DerivedValue avmDerivedValue = new avm.DerivedValue();
            avmDerivedValue.ValueSource = idString;
            avmCyberParameter.Value.ValueExpression = avmDerivedValue;    

            // Assign ID attr content
            avmCyberParameter.Value.ID = META2AVM_Util.UtilFuncs.ensureMetaIDAttribute(cyberModelParameterRef);
            ////
            
            // Assign Locator relative to the parent cyberModel
            avmCyberParameter.Locator = cyberModelParameterRef.Name;

            /// Now do the outer representation
            avm.PrimitiveProperty avmProperty = new avm.PrimitiveProperty();
            avmProperty.Name = avmCyberParameter.Locator;
            avmProperty.ID = "property." + idString;
            avmProperty.Notes = avmCyberParameter.Notes;

            avmProperty.Value = new avm.Value();
            avmProperty.Value.ID = idString;
            
            
            // Three cases for getting the data type and/or value of the parameter object
            //  TODO: JEP The Simulink case is extremely messy - assume that the value is a real-type scalar for now, and we'll take care of it later
            avmProperty.Value.DimensionType = DimensionTypeEnum.Scalar;
            avmProperty.Value.DataType = DataTypeEnum.Real;
            avm.ParametricValue avmParametricValue = new avm.ParametricValue();
            
            //avm.FixedValue maxVal = new avm.FixedValue();
            //maxVal.Value = "1.0e10";
    
            //avmParametricValue.Maximum = maxVal;
            //avm.FixedValue minVal = new avm.FixedValue();
            //minVal.Value = "-1.0e10";
            //avmParametricValue.Minimum = minVal;
            //avm.FixedValue avmFixedValue = new avm.FixedValue();
            //avmFixedValue.Value = "0";
            /* Cyber.ParameterBase pbase = cyberModelParamterRef.Referred as Cyber.ParameterBase;
            if (pbase.Kind == "VFL_Parameter")
            {
                Simulink.SF_Parameter sfparam = pbase as Simulink.SF_Parameter;

            }
            else if (pbase.Kind == "Data")
            {

            }
            else if (pbase.Kind == "SFData")
            {

            } */
            avmProperty.Value.ValueExpression = avmParametricValue;
            

            // Add avm param to the given List
            avmCyberModelParameterList.Add(avmCyberParameter);
            _avmComponent.Property.Add(avmProperty);
        }

        private void createAVMCyberConnector(List<avm.modelica.Connector> avmCyberModelConnectorList,  ISIS.GME.Common.Interfaces.Model cyberModelSignalInterface)
        {

            //Intialize new avm.modelica.Connector
            avm.modelica.Connector avmCyberConnector = new avm.modelica.Connector();

            // Assign the Class to avmCyberConnector from cyber SignalInterface
            avmCyberConnector.Class = META2AVM_Util.UtilFuncs.getMetaAttributeContent(cyberModelSignalInterface, "Class");

            // Assign name to the avmconnector from cyber SignalInterface
            //avmCyberConnector.Name = META2AVM_Util.UtilFuncs.getMetaObjectName(cyberModelSignalInterface);
            avmCyberConnector.Name = cyberModelSignalInterface.Name;

            // Assign Locator to the avmconnector from cyber SignalInterface relative path from its CyberModel
            avmCyberConnector.Locator = cyberModelSignalInterface.Name; // the Port is a direct child of the CyberModel hence locator is the name

            // Assign id to the avmconnector from cyber SignalInterface UDM ID
            avmCyberConnector.ID = META2AVM_Util.UtilFuncs.ensureMetaIDAttribute(cyberModelSignalInterface);
            


            // Now create the outer connector
            avm.Connector avmConnector = new avm.Connector();
            avmConnector.Name = avmCyberConnector.Name;
            avmConnector.ID = avmCyberConnector.ID;

            avm.modelica.Connector innerConnector = new avm.modelica.Connector();
            innerConnector.ID = META2AVM_Util.UtilFuncs.ensureMetaIDAttribute();
            innerConnector.Class = avmCyberConnector.Class;
            innerConnector.Locator = avmCyberConnector.Locator;
            innerConnector.Name = avmCyberConnector.Name;
            innerConnector.PortMap.Add(avmCyberConnector.ID);
     
            avmConnector.Role.Add(innerConnector);
            _avmComponent.Connector.Add(avmConnector);

            // Add the created avm connector to the given list
            avmCyberModelConnectorList.Add(avmCyberConnector);
        }

        public void createAVMCyberModel(Cyber.ModelicaComponent cyberModel, string filename)
        {
            // Initialize new AVM cyber model
            avm.cyber.CyberModel avmCyberModel = new avm.cyber.CyberModel();

            // Set Class
            avmCyberModel.Class = cyberModel.Attributes.Class;

            // Set Name
            _avmComponent.Name = cyberModel.Name;
            avmCyberModel.Name = cyberModel.Name;

            _avmComponent.ID = META2AVM_Util.UtilFuncs.ensureMetaIDAttribute();

            _avmComponent.SchemaVersion = "2.5";
            avmCyberModel.Notes = "";
            avmCyberModel.UsesResource = "cyber.path";

            // Create locator string
            string[] path_parts = filename.Split('\\');
            string[] filename_parts = path_parts[path_parts.Length - 1].Split('.');
            avmCyberModel.Locator = cyberModel.Path.Replace('/', '.');

            avm.Resource res = new avm.Resource();
            res.Path = "Cyber\\" + path_parts[path_parts.Length - 1];
            res.Name = "CyberModel";
            res.ID = "cyber.path";
            
            _avmComponent.ResourceDependency.Add(res);

            avm.Resource res2 = new avm.Resource();
            res2.Path = "Cyber\\" + filename_parts[0] + ".xml";
            res2.Name = "CyberXML";
            res2.ID = "cyberxml.path";

            _avmComponent.ResourceDependency.Add(res2);

            // Set Type
            GMEConsole.Out.WriteLine(cyberModel.GetType().Name);
            if (cyberModel.Impl.MetaBase.Name == "SimulinkWrapper")
            {
               avmCyberModel.Type = avm.cyber.ModelType.Simulink;
            }
            else //(cyberModel.Impl.MetaBase.Name == "SignalFlowWrapper")
            {
               avmCyberModel.Type = avm.cyber.ModelType.SignalFlow;
            }
            //else
            //{
                // Other ModelicaComponent types not handled for now
            //    return;
            //}

            // for every InputSignalInterface in the ModelicaComponent create avm.modelica.Connector.
            // Note: Connector in AVM Cyber Model in a copy of AVM ModelicaModel
            foreach (Cyber.InputSignalInterface cyberInputSignalInterface in cyberModel.Children.InputSignalInterfaceCollection)
            {
                createAVMCyberConnector(avmCyberModel.Connector, cyberInputSignalInterface);
            }

            // for every OutputSignalInterface in the ModelicaComponent create avm.modelica.connector
            foreach (Cyber.OutputSignalInterface cyberOutputSignalInterface in cyberModel.Children.OutputSignalInterfaceCollection)
            {
                createAVMCyberConnector(avmCyberModel.Connector, cyberOutputSignalInterface);
            }

            // for every BusInterface in the ModelicaComponent create avm.modelica.connector
            // FIX: may require fix. Not sure!
            foreach (Cyber.BusInterface cyberBusInterface in cyberModel.Children.BusInterfaceCollection)
            {
                createAVMCyberConnector(avmCyberModel.Connector, cyberBusInterface);
            }

            // for every ParameterRef in the ModelicaComponent create avm.modelica.parameter
            foreach (Cyber.ParameterRef cyberParameterRef in cyberModel.Children.ParameterRefCollection)
            {
                createAVMCyberParameter(avmCyberModel.Parameter, cyberParameterRef);
            }
            
            // Add it to _avmComponent's DomainModel List
            _avmComponent.DomainModel.Add(avmCyberModel);
            // 
        }
    }
}
