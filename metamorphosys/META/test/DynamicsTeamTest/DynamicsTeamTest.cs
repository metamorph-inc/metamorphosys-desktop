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
using System.Reflection;
using GME.MGA.Parser;
using GME.Util;
using GME.MGA;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace DynamicsTeamTest
{
    public class Test
    {

        [STAThread]
        static int Main(string[] args)
        {
            bool testDashboardOnly = false;
            if (testDashboardOnly)
            {
                // FIRST RUN: this requests access to ports
                Fiddler.FiddlerApplication.Startup(8877, Fiddler.FiddlerCoreStartupFlags.Default);

                var trafficSeesionItems = new List<Fiddler.Session>();
                Fiddler.FiddlerApplication.AfterSessionComplete += delegate(Fiddler.Session session)
                {
                    trafficSeesionItems.Add(session);
                };

                // FIXME: would be nice to load a local dashboard, but if we open file:/// it will NOT generate HTTP traffic, how to log it then?
                string baseUrl = "http://zsolt-ws.isis.vanderbilt.edu/META/dashboards/36cfgs4tbs/";

                var options = new ChromeOptions();

                options.AddArgument("--allow-file-access-from-files");
                options.AddArgument("--start-maximized");
                //options.AddArgument("--enable-experimental-extension-apis");

                // FIRST RUN: this requests access to ports
                IWebDriver driver = new ChromeDriver(@"..\..\..\..\3rdParty\chromedriver", options);
                driver.Navigate().GoToUrl(baseUrl);

                driver.SwitchTo().Frame(driver.FindElement(By.TagName("iframe")));

                ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;

                while (true) // Handle timeout somewhere
                {
                    var ajaxIsComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                    if (ajaxIsComplete)
                        break;
                    System.Threading.Thread.Sleep(100);
                }


                var dataLoad = driver.FindElement(By.CssSelector("[id='dataLoadInProgress']"));
                var i = 0;
                while (dataLoad.Displayed && dataLoad.Enabled && i < 10)
                {
                    System.Threading.Thread.Sleep(1000);
                    i++;
                }

                if (i > 10)
                {
                    return 0;
                }

                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile("0.__main_page.png", System.Drawing.Imaging.ImageFormat.Png);

                var maintab = driver.FindElement(By.CssSelector("[id='mainTabs']"));
                var elements = maintab.FindElements(By.XPath("//a[@vf-page]"));

                var pageCounter = 1;

                foreach (var element in elements.ToList())
                {
                    bool visible = element.Displayed && element.Enabled;
                    if (visible)
                    {
                        element.Click();
                        element.SendKeys(OpenQA.Selenium.Keys.Escape);
                        System.Threading.Thread.Sleep(1000);

                        screenshot = screenshotDriver.GetScreenshot();
                        screenshot.SaveAsFile(pageCounter.ToString() + element.GetAttribute("vf-page").Replace("/", "_") + "_page.png", System.Drawing.Imaging.ImageFormat.Png);
                        pageCounter++;

                    }
                }

                foreach (var trafficSeesionItem in trafficSeesionItems.ToList())
                {
                    // HINT: this will log all HTTP traffic no matter which application generates.
                    System.Diagnostics.Trace.WriteLine(String.Format("{0} {1}", trafficSeesionItem.responseCode, trafficSeesionItem.fullUrl));
                }

                driver.Quit();

                Fiddler.FiddlerApplication.oProxy.Detach();

                return 0;
            }

            int ret = Xunit.ConsoleClient.Program.Main(new string[] {
                Assembly.GetAssembly(typeof(Test)).CodeBase.Substring("file:///".Length),
                //"/noshadow",
            });
            Console.In.ReadLine();
            return ret;
        }

        /// <summary>
        /// Call this method for each test-case.
        /// </summary>
        /// <param name="projectDir"> e.g. DriveTrain_v3</param>
        /// <param name="xmeFileName">e.g. DriveLine_v3.xme</param>
        /// <returns>e.g. C:\META\meta_trunk\models\DynamicsTeam\DriveTrain_v3\DriveLine_v3_test.mga</returns>
        public static string ImportXME2Mga(string projectDir, string xmeFileName)
        {
            var xmePath = Path.Combine("..", "..", ".." , "..", "models", "DynamicsTeam", projectDir, xmeFileName);
            string projectConnStr;
            MgaUtils.ImportXMEForTest(xmePath, out projectConnStr);
            return projectConnStr.Substring("MGA=".Length);
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
    }

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
            MgaUtils.ImportXMEForTest(Path.Combine("..", "..", "..", "..", "models", "DynamicsTeam", xmeFilename), out projectConnStr);
            return projectConnStr.Substring("MGA=".Length);
        }
    }
}
