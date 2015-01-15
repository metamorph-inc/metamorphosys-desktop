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
using GME.MGA;
using System.Reflection;
using Xunit;
using System.IO;

namespace ElaboratorTest
{
    public abstract class XmeImportFixture
    {
        protected abstract string xmeFilename { get; }
        private Exception importException;
        private string _mgaFile;
        internal string mgaFile
        {
            get
            {
                if (importException != null)
                {
                    throw new Exception("Xme import failed", importException);
                }
                return _mgaFile;
            }
        }

        public XmeImportFixture()
        {
            try
            {
                this._mgaFile = ImportXME2Mga();
            }
            catch (Exception e)
            {
                importException = e;
            }
        }

        public string ImportXME2Mga()
        {
            string projectConnStr;
            MgaUtils.ImportXMEForTest(xmeFilename, out projectConnStr);
            return projectConnStr.Substring("MGA=".Length);
        }
    }

    public class ElaboratorFixture : XmeImportFixture
    {
        protected override string xmeFilename
        {
            get { return Path.Combine("..", "..", "..", "..", "models", "DynamicsTeam", "Elaborator", "Elaborator.xme"); }
        }
    }

    public class Test : IUseFixture<ElaboratorFixture>
    {
        [STAThread]
        static int Main(string[] args)
        {
            int ret = Xunit.ConsoleClient.Program.Main(new string[] {
                Assembly.GetAssembly(typeof(Test)).CodeBase.Substring("file:///".Length),
                //"/noshadow",
            });
            Console.In.ReadLine();
            return ret;
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        [Trait("ProjectImport/Open", "Elaborator")]
        public void ProjectMgaOpen()
        {
            var mgaReference = "MGA=" + this.mgaFile;

            MgaProject project = new MgaProject();
            project.OpenEx(mgaReference, "CyPhyML", null);
            MgaHelper.CheckParadigmVersionUpgrade(project);
            project.Close(true);
            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }


        [Fact]
        [Trait("Model", "Elaborator")]
        public void InstanceGUIDComponentReferenceChain()
        {

            string objectAbsPath = "/@01_ComponentAssemblies/@01_InstanceGUIDTests/@InstanceGUIDComponentReferenceChain";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            // check context
            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");

        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void InstanceGUIDSimple()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@01_InstanceGUIDTests/@InstanceGUIDSimple";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void InstanceGUIDWithComponentAssemblyRef()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@01_InstanceGUIDTests/@InstanceGUIDWithComponentAssemblyRef";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void InstanceGUIDWithComponentAssemblyRefOutsideOfTree()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@01_InstanceGUIDTests/@InstanceGUIDWithComponentAssemblyRefOutsideOfTree";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void InstanceGUIDWithComponentAssemblyRefOutsideOfTreeWhichHasOnlyComponents()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@01_InstanceGUIDTests/@InstanceGUIDWithComponentAssemblyRefOutsideOfTreeWhichHasOnlyComponents";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void ComponentRefPointsToComponent()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@ComponentRefPointsToComponent";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }



        [Fact]
        [Trait("Model", "Elaborator")]
        public void ComponentRefPointsToComponentAndConnections()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@ComponentRefPointsToComponentAndConnections";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void HasComponentAssembliesMultiLevels()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@HasComponentAssembliesMultiLevels";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        // META-2667 needs a GME >= 14.1.18
        //[Fact]
        [Trait("Model", "Elaborator")]
        public void HasSubtypes()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@ContainsDerivedArchetype";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void SelfLoop()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@SelfLoop";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void HasComponentAssembliesTwoLevels()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@HasComponentAssembliesTwoLevels";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void HasComponentAssemblyRefsMultiLevel()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@HasComponentAssemblyRefsMultiLevel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void HasInheritedComponents()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@HasInheritedComponents";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void SimpleHierarchy()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@SimpleHierarchy";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void CircularBetweenSubtrees()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Invalid/@CircularBetweenSubtrees";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void CircularReference()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Invalid/@CircularReference";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void CircularReferencesBetweenTrees()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Invalid/@CircularReferencesBetweenTrees";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void WithinTheTree()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Invalid/@WithinTheTree";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceDC()
        {
            string objectAbsPath = "/@DesignSpace/@DC";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void HasDesignSpace()
        {
            string objectAbsPath = "/@02_TestBenches/@Invalid/@HasDesignSpace";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void StructuralFEATestInjectionPointTest()
        {
            string objectAbsPath = "/@02_TestBenches/@StructuralFEATestInjectionPointTest";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void StructuralFEATestInjectionPointTest2()
        {
            string objectAbsPath = "/@02_TestBenches/@StructuralFEATestInjectionPointTest2";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void StructuralFEATestInjectionPointTest3()
        {
            string objectAbsPath = "/@02_TestBenches/@StructuralFEATestInjectionPointTest3";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void NullReferenceTestInjectionPoint()
        {
            string objectAbsPath = "/@02_TestBenches/@Valid/@NullReferenceTestInjectionPoint";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void AmbiguousTestInjectionPointReference()
        {
            string objectAbsPath = "/@02_TestBenches/@Invalid/@AmbiguousTestInjectionPointReference";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void OneTestInjectionPointIsNotInTheSystem()
        {
            string objectAbsPath = "/@02_TestBenches/@Invalid/@OneTestInjectionPointIsNotInTheSystem";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void TestInjectionPointPointsToTheReferredComponentAssembly()
        {
            string objectAbsPath = "/@02_TestBenches/@Invalid/@TestInjectionPointPointsToTheReferredComponentAssembly";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void ContainsDerived()
        {
            string objectAbsPath = "/@01_ComponentAssemblies/@Valid/@ContainsDerived";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceComponentRefPointsToComponent()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@ComponentRefPointsToComponent";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }



        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceComponentRefPointsToComponentAndConnections()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@ComponentRefPointsToComponentAndConnections";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceHasComponentAssembliesMultiLevels()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@HasComponentAssembliesMultiLevels";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        // META-2667 needs a GME >= 14.1.18
        //[Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceHasSubtypes()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@ContainsDerivedArchetype";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceSelfLoop()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@SelfLoop";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceHasComponentAssembliesTwoLevels()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@HasComponentAssembliesTwoLevels";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceHasComponentAssemblyRefsMultiLevel()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@HasComponentAssemblyRefsMultiLevel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceHasInheritedComponents()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@HasInheritedComponents";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceSimpleHierarchy()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@SimpleHierarchy";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceContainsDerived()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Valid/@ContainsDerived";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceCircularBetweenSubtrees()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Invalid/@CircularBetweenSubtrees";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceCircularReference()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Invalid/@CircularReference";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceCircularReferencesBetweenTrees()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Invalid/@CircularReferencesBetweenTrees";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "Elaborator")]
        public void DesignSpaceWithinTheTree()
        {
            string objectAbsPath = "/@03_DesignSpaces/@Invalid/@WithinTheTree";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }


        private string mgaFile { get { return this.fixture.mgaFile; } }
        private ElaboratorFixture fixture;

        public void SetFixture(ElaboratorFixture data)
        {
            this.fixture = data;
        }
    }
}
