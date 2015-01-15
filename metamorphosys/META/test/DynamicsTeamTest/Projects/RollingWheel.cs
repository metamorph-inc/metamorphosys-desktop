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
using Xunit;
using System.IO;
using GME.MGA;
using GME.MGA.Meta;

namespace DynamicsTeamTest.Projects
{
    public partial class RollingWheel : IUseFixture<RollingWheelFixture>
    {
        [Fact]
        [Trait("Model", "RollingWheel")]
        [Trait("ComponentLibraryManagerAddon", "RollingWheel")]
        public void ComponentLibraryManagerAddonRelativeProjectPathURI()
        {
            var mgaReference = "MGA=" + mgaFile;

            MgaProject project = new MgaProject();
            project.EnableAutoAddOns(true);
            project.OpenEx(mgaReference, "CyPhyML", null);

            try
            {
                var terr = project.BeginTransactionInNewTerr(transactiontype_enum.TRANSACTION_NON_NESTED);

                // turn off CyPhySignalBlocksAddOn
                var addons = project.AddOnComponents.Cast<IMgaComponentEx>().Where(x => x.ComponentName == "CyPhySignalBlocksAddOnAddon");
                foreach (var addon in addons)
                {
                    try
                    {
                        addon.Enable(false);
                    }
                    catch (Exception ex)
                    {
                        // if one fails keep trying the other ones.

                    }
                }

                var components = project.RootFolder.CreateFolder(project.RootMeta.RootFolder.LegalChildFolderByName["Components"]);
                components.Name = "ComponentLibraryManagerAddonProjectPathURITest";
                var component = components.CreateRootObject(project.RootMeta.RootFolder.DefinedFCOByName["Component", false]);
                component.Name = "ComponentLibraryManagerAddonProjectPathURITest";

                project.CommitTransaction();
            }
            finally
            {
                project.Close(false);
            }
        }


        [Fact]
        [Trait("Model", "RollingWheel")]
        [Trait("CyPhyAddon", "RollingWheel")]
        public void CreatedObjectWithDefaultNamesShouldNotHaveSpaces()
        {
            var mgaReference = "MGA=" + mgaFile;

            MgaProject project = new MgaProject();
            project.EnableAutoAddOns(true);
            project.OpenEx(mgaReference, "CyPhyML", null);

            List<IMgaObject> createdObjects = new List<IMgaObject>();

            try
            {
                var terr = project.BeginTransactionInNewTerr(transactiontype_enum.TRANSACTION_NON_NESTED);

                // turn off CyPhySignalBlocksAddOn
                var addons = project.AddOnComponents.Cast<IMgaComponentEx>().Where(x => x.ComponentName == "CyPhySignalBlocksAddOnAddon");
                foreach (var addon in addons)
                {
                    try
                    {
                        addon.Enable(false);
                    }
                    catch (Exception ex)
                    {
                        // if one fails keep trying the other ones.

                    }
                }

                // Use default names empty suffix
                createdObjects = this.CreateObjectHierarchy(project, "");

                project.CommitTransaction();

                project.BeginTransaction(terr, transactiontype_enum.TRANSACTION_NON_NESTED);

                // check renames
                foreach (var obj in createdObjects)
                {
                    // this will fail on the first one
                    // FIXME: should we collect everything and print a detailed message about failures?
                    Assert.False(obj.Name.Contains(' '), string.Format("Name contains space, but it should not contain any spaces. {0} [{1}]", obj.Name, obj.MetaBase.DisplayedName));
                }

                project.AbortTransaction();
            }
            finally
            {
                project.Close(false);
            }

            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }

        [Fact]
        [Trait("Model", "RollingWheel")]
        [Trait("CyPhyAddon", "RollingWheel")]
        public void CreatedObjectWithNonDefaultNamesCanHaveSpaces()
        {
            var mgaReference = "MGA=" + mgaFile;

            MgaProject project = new MgaProject();
            project.EnableAutoAddOns(true);
            project.OpenEx(mgaReference, "CyPhyML", null);

            List<IMgaObject> createdObjects = new List<IMgaObject>();

            try
            {
                var terr = project.BeginTransactionInNewTerr(transactiontype_enum.TRANSACTION_NON_NESTED);

                // turn off CyPhySignalBlocksAddOn
                var addons = project.AddOnComponents.Cast<IMgaComponentEx>().Where(x => x.ComponentName == "CyPhySignalBlocksAddOnAddon");
                foreach (var addon in addons)
                {
                    try
                    {
                        addon.Enable(false);
                    }
                    catch (Exception ex)
                    {
                        // if one fails keep trying the other ones.

                    }
                }

                // make sure suffix has at least one space
                createdObjects = this.CreateObjectHierarchy(project, " Non default name");

                project.CommitTransaction();

                project.BeginTransaction(terr, transactiontype_enum.TRANSACTION_NON_NESTED);

                // check renames
                foreach (var obj in createdObjects)
                {
                    // this will fail on the first one
                    // FIXME: should we collect everything and print a detailed message about failures?
                    Assert.True(obj.Name.Contains(' '), string.Format("Name does not contain spaces, but it should contain at least one space. {0} [{1}]", obj.Name, obj.MetaBase.DisplayedName));
                }

                project.AbortTransaction();
            }
            finally
            {
                project.Close(false);

            }
            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }


        public List<IMgaObject> CreateObjectHierarchy(MgaProject project, string suffix)
        {
            List<IMgaObject> createdObjects = new List<IMgaObject>();

            foreach (MgaMetaFolder metaFolder in project.RootMeta.RootFolder.LegalChildFolders)
            {
                var folder = project.RootFolder.CreateFolder(metaFolder);
                folder.Name = metaFolder.DisplayedName + suffix;
                createdObjects.Add(folder);

                var newFolder = project.RootFolder.CreateFolder(metaFolder);
                newFolder.Name = "New" + metaFolder.DisplayedName + suffix;
                createdObjects.Add(newFolder);

                foreach (MgaMetaFCO childFCO in metaFolder.LegalRootObjects)
                {
                    var fco = newFolder.CreateRootObject(childFCO);
                    fco.Name = childFCO.DisplayedName + suffix;
                    createdObjects.Add(fco);

                    var newFco = newFolder.CreateRootObject(childFCO);
                    newFco.Name = "New" + childFCO.DisplayedName + suffix;
                    createdObjects.Add(newFco);
                }

                foreach (MgaMetaFolder metaChildFolder in metaFolder.LegalChildFolders)
                {
                    var childFolder = newFolder.CreateFolder(metaChildFolder);
                    childFolder.Name = metaChildFolder.DisplayedName + suffix;
                    createdObjects.Add(childFolder);

                    var newChildFolder = newFolder.CreateFolder(metaChildFolder);
                    newChildFolder.Name = "New" + metaChildFolder.DisplayedName + suffix;
                    createdObjects.Add(newChildFolder);

                    foreach (MgaMetaFCO childFCO in metaChildFolder.LegalRootObjects)
                    {
                        var fco = newChildFolder.CreateRootObject(childFCO);
                        fco.Name = childFCO.DisplayedName + suffix;
                        createdObjects.Add(fco);

                        var newFco = newChildFolder.CreateRootObject(childFCO);
                        newFco.Name = "New" + childFCO.DisplayedName + suffix;
                        createdObjects.Add(newFco);
                    }
                }
            }

            return createdObjects;
        }

    }
}
