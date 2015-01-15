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
using System.Reflection;
using Xunit;
using System.IO;
using ISIS.VehicleForge;

namespace ISISVehicleForgeAPITest
{

    public class CredentialsFixture
    {
        internal ISIS.Web.Credentials credentials { get; set; }

        public CredentialsFixture()
        {
            string vfCredentialFile = Path.Combine(META.VersionInfo.MetaPath, "vf.txt");
            if (File.Exists(vfCredentialFile) == false)
            {
                string userPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if ( Environment.OSVersion.Version.Major >= 6 )
                {
                    userPath = Directory.GetParent(userPath).FullName;
                }

                vfCredentialFile = Path.Combine(userPath, "vf.txt");

                if (File.Exists(vfCredentialFile) == false)
                {
                    throw new FileNotFoundException(vfCredentialFile);
                }
            }

            List<string> vfFileContent = File.ReadLines(vfCredentialFile).ToList();
            if (vfFileContent.Count < 3)
            {
                string message = string.Format("{0} file must contain 3 lines vehicle forge url, username, pass.", vfCredentialFile);
                throw new ArgumentOutOfRangeException(message);
            }

            this.credentials = new ISIS.Web.Credentials(vfFileContent[0], vfFileContent[1], vfFileContent[2]);
        }
    }


    public class Test : IUseFixture<CredentialsFixture>
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

        private CredentialsFixture fixture { get; set; }

        public void SetFixture(CredentialsFixture data)
        {
            this.fixture = data;
        }

        [Fact]
        public void VehicleForgeURLIsInvalid()
        {
            using (ISIS.VehicleForge.VFWebClient webClient = new ISIS.VehicleForge.VFWebClient("https://testbench2.vf.isis.vanderbilt.edu/", this.fixture.credentials))
            {
                Assert.Throws<ISIS.VehicleForge.VFInvalidURLException>(() => { webClient.SendGetRequest(""); });
            }
        }

        //[Fact]
        public void AuthenticationFailure()
        {
            ISIS.Web.Credentials credentialShouldFail = new ISIS.Web.Credentials(this.fixture.credentials);
            credentialShouldFail.Password = "shouldFail";

            using (ISIS.VehicleForge.VFWebClient webClient = new ISIS.VehicleForge.VFWebClient(this.fixture.credentials.Url, credentialShouldFail))
            {
                Assert.Throws<ISIS.VehicleForge.VFLoginException>(() => { webClient.SendGetRequest(""); });
            }
        }

        [Fact]
        public void AuthenticationSuccess()
        {
            using (ISIS.VehicleForge.VFWebClient webClient = new ISIS.VehicleForge.VFWebClient(this.fixture.credentials.Url, this.fixture.credentials))
            {
                Assert.DoesNotThrow(() => { webClient.SendGetRequest(""); });
            }
        }

        [Fact]
        public void GetProfile()
        {
            using (VFWebClient webClient = new VFWebClient(this.fixture.credentials.Url, this.fixture.credentials))
            {
                Assert.DoesNotThrow(() => { webClient.GetProfile(); });
            }
        }

        //[Fact]
        public void SearchComponentOnExchange()
        {
            ISIS.Web.IComponentLibrarySearchResult<VFComponent> searchResult;
            string categoryToSearch = "Engine";

            using (VFWebClient vfWebClient = new VFWebClient(this.fixture.credentials.Url, this.fixture.credentials))
            {
                VFExchange exchange = new VFExchange(vfWebClient);

                var filter = new VFExchangeFilter()
                {
                    Category = categoryToSearch,
                    StartPosition = 0,
                    NumberOfResults = 10
                };

                searchResult = exchange.Search(filter);

                Assert.Equal(5, searchResult.hits);
            }
        }

        [Fact]
        public void SearchEmptyCategoryOnExchange()
        {
            ISIS.Web.IComponentLibrarySearchResult<VFComponent> searchResult;
            string categoryToSearch = "";

            using (VFWebClient vfWebClient = new VFWebClient(this.fixture.credentials.Url, this.fixture.credentials))
            {
                VFExchange exchange = new VFExchange(vfWebClient);

                var filter = new VFExchangeFilter()
                {
                    Category = categoryToSearch,
                };

                searchResult = exchange.Search(filter);

                Assert.Null(searchResult);
            }
        }

        //[Fact]
        public void SearchComponentOnExchangeWithFilter()
        {
            ISIS.Web.IComponentLibrarySearchResult<VFComponent> searchResult;

            using (VFWebClient vfWebClient = new VFWebClient(this.fixture.credentials.Url, this.fixture.credentials))
            {
                VFExchange exchange = new VFExchange(vfWebClient);

                Dictionary<string, ISIS.Web.ComponentLibraryFilterParameter> filterParams = 
                    new Dictionary<string, ISIS.Web.ComponentLibraryFilterParameter>();

                string categoryToSearch = "Engine";
                string filterName = "Power";
                string minimumValue = "400";

                // Create a new filter object
                var newParam = new ISIS.Web.ContinuousFilterParameter(filterName)
                {
                    MinValue = minimumValue,
                };

                filterParams[filterName] = newParam;
                
                var filter = new VFExchangeFilter()
                {
                    Category = categoryToSearch,
                    ParameterList = filterParams
                };

                searchResult = exchange.Search(filter);

                Assert.Equal(1, searchResult.hits);
            }
        }

        //[Fact]
        public void DownloadComponentFromExchange()
        {
            string metaDirectory = META.VersionInfo.MetaPath;
            var componentToDownload = new ISIS.VehicleForge.VFComponent();

            // https://testbench.vf.isis.vanderbilt.edu/rest/exchange/components/5282583bc51df0577fa0fd23
            // Engine_Caterpillar_C9_280kW

            componentToDownload.zip_url = "/rest/exchange/components/5282583bc51df0577fa0fd23/zip";

            using (VFWebClient vfWebClient = new VFWebClient(this.fixture.credentials.Url, this.fixture.credentials))
            {
                VFExchange exchange = new VFExchange(vfWebClient);

                string newZipPath = exchange.DownloadComponent(componentToDownload, metaDirectory);

                Assert.NotNull(newZipPath);

                if (File.Exists(newZipPath))
                {
                    File.Delete(newZipPath);
                }
            }
        }

        //[Fact]
        public void DownloadNonExistentComponentFromExchange()
        {
            string metaDirectory = META.VersionInfo.MetaPath;
            var componentToDownload = new ISIS.VehicleForge.VFComponent();

            // Might make sense to randomly generate a component id and try it - 24 hex characters
            componentToDownload.zip_url = "/rest/exchange/components/aaaabbbbccccddddeeeeffff/zip";

            using (VFWebClient vfWebClient = new VFWebClient(this.fixture.credentials.Url, this.fixture.credentials))
            {
                VFExchange exchange = new VFExchange(vfWebClient);

                string newZipPath = exchange.DownloadComponent(componentToDownload, metaDirectory);

                Assert.Null(newZipPath);
            }
        }
    }
}
