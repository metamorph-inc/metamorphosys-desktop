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
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using GME.MGA;

namespace CyPhy2CAD_CSharp.DataRep
{
    // This is "half" side of a full joint, specified in one Connector
    public class KinematicJoint
    {
        public enum KinematicJointType
        {
            REVOLUTE,
            PRISMATIC,
            CYLINDRICAL,
            SPHERICAL,
            FIXED,
            UNDEFINED
        }

        public string CyPhyID; // Cyphy ID
        public string Name; // Cyphy Name

        public KinematicJointType JointType;

        // Possible limits on this joint. Not all of these have to be specified, hence the double? type
        public double? RotationLimitDefault;
        public double? RotationLimitMin;
        public double? RotationLimitMax;
        public double? TranslationLimitDefault;
        public double? TranslationLimitMin;
        public double? TranslationLimitMax;

        // These are referring to Datums within the StructuralInterfaceConstraint this KinematicJoint belongs to
        // Don't modify these classes from here
        public DataRep.Datum Axis; // Axis of rotation/sliding
        public DataRep.Datum AlignmentPlane; // Plane of alignment
        public DataRep.Datum RotationDefaultReference;
        public DataRep.Datum RotationMinReference;
        public DataRep.Datum RotationMaxReference;
        public DataRep.Datum TranslationDefaultReference;
        public DataRep.Datum TranslationMinReference;
        public DataRep.Datum TranslationMaxReference;

        public GeometryMarkerRep Marker;

        public string TopLevelID; // If the joint has a connection on the assembly level, it's name is going to be used as joint ID in the kinematic model

        public CyPhy.KinematicJoint CyphyJoint; // The original joint

        public KinematicJoint(CyPhy.KinematicJoint kJoint)
        {
            CyphyJoint = kJoint;
            JointType = GetJointType(kJoint);
            Name = kJoint.Name;
            CyPhyID = kJoint.ID;

            // TODO: Error handling - many things can go wrong here
            if (kJoint is CyPhy.HasKinematicRotationalLimit)
            {
                foreach (var limit in (kJoint as CyPhy.HasKinematicRotationalLimit).SrcConnections.KinematicRotationalLimitCollection)
                {
                    if (limit.Attributes.LimitType == CyPhyClasses.KinematicRotationalLimit.AttributesClass.LimitType_enum.Default)
                    {
                        RotationLimitDefault = Double.Parse(limit.SrcEnds.Parameter.Attributes.Value);
                    }
                    else if (limit.Attributes.LimitType == CyPhyClasses.KinematicRotationalLimit.AttributesClass.LimitType_enum.Max)
                    {
                        RotationLimitMax = Double.Parse(limit.SrcEnds.Parameter.Attributes.Value);
                    }
                    else if (limit.Attributes.LimitType == CyPhyClasses.KinematicRotationalLimit.AttributesClass.LimitType_enum.Min)
                    {
                        RotationLimitMin = Double.Parse(limit.SrcEnds.Parameter.Attributes.Value);
                    }
                }
            }
            if (kJoint is CyPhy.HasKinematicTranslationalLimit)
            {
                foreach (var limit in (kJoint as CyPhy.HasKinematicTranslationalLimit).DstConnections.KinematicTranslationallLimitCollection)
                {
                    if (limit.Attributes.LimitType == CyPhyClasses.KinematicTranslationallLimit.AttributesClass.LimitType_enum.Default)
                    {
                        TranslationLimitDefault = Double.Parse(limit.DstEnds.Parameter.Attributes.Value);
                    }
                    else if (limit.Attributes.LimitType == CyPhyClasses.KinematicTranslationallLimit.AttributesClass.LimitType_enum.Max)
                    {
                        TranslationLimitMax = Double.Parse(limit.DstEnds.Parameter.Attributes.Value);
                    }
                    else if (limit.Attributes.LimitType == CyPhyClasses.KinematicTranslationallLimit.AttributesClass.LimitType_enum.Min)
                    {
                        TranslationLimitMin = Double.Parse(limit.DstEnds.Parameter.Attributes.Value);
                    }
                }
            }

            if (!String.IsNullOrEmpty(kJoint.Attributes.GeometricMarker))
            {
                Marker = new GeometryMarkerRep(kJoint.Attributes.GeometricMarker);
                if (kJoint.ParentContainer.ParentContainer.Kind == "Component")
                {
                    Marker.ComponentID = CyPhyClasses.Component.Cast(kJoint.ParentContainer.ParentContainer.Impl).Attributes.InstanceGUID;
                }
            }


        }

        public void FindTopLevelPort(CyPhy.ComponentAssembly topAssembly)
        {
            return;
            /*
            CyPhy.KinematicJoint joint = CyphyJoint;
            string id = null;
            while (true)
            {
                if (joint.DstConnections.JointCompositionCollection.Any())
                {
                    if (joint.DstConnections.JointCompositionCollection.First().DstEnds.KinematicJoint == null)
                    {
                        id = joint.Name;
                    }
                    else
                    {
                        joint = joint.DstConnections.JointCompositionCollection.First().DstEnds.KinematicJoint;
                    }
                }
                else
                {
                    if (joint != CyphyJoint && joint.ParentContainer.Guid == topAssembly.Guid)
                        id = joint.Name;
                    break;
                }
            }
            if (id == null)
            {
                while (true)
                {
                    if (joint.SrcConnections.JointCompositionCollection.Any())
                    {
                        if (joint.SrcConnections.JointCompositionCollection.First().SrcEnds.KinematicJoint == null)
                        {
                            id = joint.Name;
                            break;
                        }
                        else
                        {
                            joint = joint.SrcConnections.JointCompositionCollection.First().SrcEnds.KinematicJoint;
                        }
                    }
                    else
                    {
                        if (joint != CyphyJoint && joint.ParentContainer.Guid == topAssembly.Guid)
                            id = joint.Name;
                        break;
                    }
                }
            }

            this.TopLevelID = id;
             */
        }

        KinematicJointType GetJointType(CyPhy.KinematicJoint joint)
        {
            if (joint is CyPhy.KinematicCylindricalJoint)
            {
                return KinematicJointType.CYLINDRICAL;
            }
            else if (joint is CyPhy.KinematicTranslationalJoint)
            {
                return KinematicJointType.PRISMATIC;
            }
            else if (joint is CyPhy.KinematicRevoluteJoint)
            {
                return KinematicJointType.REVOLUTE;
            }
            else if (joint is CyPhy.KinematicSphericalJoint)
            {
                return KinematicJointType.SPHERICAL;
            }
            else
            {
                return KinematicJointType.UNDEFINED;
            }
        }

    }


}
