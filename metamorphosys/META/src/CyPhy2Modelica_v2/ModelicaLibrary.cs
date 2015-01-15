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
using System.IO;
using System.Text.RegularExpressions;

namespace CyPhy2Modelica_v2
{
    public class ModelicaLibrary
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public string Version { get; set; }
        public List<Tuple<string, string>> Versions { get; set; }

        public ModelicaLibrary()
        {
            this.Versions = new List<Tuple<string, string>>();
        }

        public static List<ModelicaLibrary> GetLibrariesFromSettings(CyPhy2Modelica_v2Settings settings, string projectDir)
        {
            var libraries = new List<ModelicaLibrary>();

            // If directory name was saved append package.mo
            // support all formats
            foreach (var dirPath in settings.IncludeDirectoryPath)
            {
                if (LibraryExists(dirPath, projectDir) == false)
                {
                    continue;
                }
                var newLib = ModelicaLibrary.GetLibraryFromPath(dirPath);
                var found = libraries.FirstOrDefault(x => x.Name == newLib.Name);
                if (found == null)
                {
                    libraries.Add(newLib);
                }
            }

            foreach (var dirPath in settings.NonCheckedIncludeDirPaths)
            {
                if (LibraryExists(dirPath, projectDir) == false)
                {
                    continue;
                }
                var newLib = ModelicaLibrary.GetLibraryFromPath(dirPath);
                var found = libraries.FirstOrDefault(x => x.Name == newLib.Name);
                if (found == null)
                {
                    newLib.IsSelected = false;
                    libraries.Add(newLib);
                }
                else
                {
                    found.Versions.Add(newLib.Versions.FirstOrDefault());
                }
            }

            return libraries;
        }

        /// <summary>
        /// Returns null if there is no match of the library.
        /// The user is not allowed to change the config.xml, thus check for null is 
        /// only neccssary when browsing for the library.
        /// </summary>
        /// <param name="libraryPath"></param>
        /// <returns></returns>
        public static ModelicaLibrary GetLibraryFromPath(string libraryPath)
        {
            string fileName = Path.GetFileName(libraryPath);
            ModelicaLibrary modelicaLibrary = null;

            if (fileName == "package.mo")
            {
                fileName = Path.GetFileName(Path.GetDirectoryName(libraryPath));
            }
            else if (fileName.EndsWith(".mo"))
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
            }

            if (fileName.Where(c => ModelicaLibrary.InvalidChars.Contains(c)).Count() > 0)
            {
                return modelicaLibrary;
            }

            string pattern = @"(\w+)(?:[ ](((?:\d+)(?:[.]\d+)*)(?:[ ].*)?))?";

            Match match = Regex.Match(fileName, pattern);
            if (match.Success)
            {
                string packageName = match.Groups[1].Value;

                if (packageName.Contains(' ') || packageName.Contains('.'))
                {
                    return modelicaLibrary;
                }

                string version;
                if (match.Groups[0].Value == fileName)
                {
                    version = string.IsNullOrWhiteSpace(match.Groups[2].Value) ? "default" : match.Groups[2].Value;
                }
                else
                {
                    version = fileName.Substring(packageName.Length).Trim();
                }

                modelicaLibrary = new ModelicaLibrary()
                {
                    Name = packageName,
                    IsSelected = true,
                    Version = version
                };

                modelicaLibrary.Versions.Add(new Tuple<string, string>(version, libraryPath));
            }

            return modelicaLibrary;
        }

        public static CyPhy2Modelica_v2Settings UpdateSettingsFromModelicaLibraries(CyPhy2Modelica_v2Settings currentSettings, List<ModelicaLibrary> libraries)
        {
            currentSettings.IncludeDirectoryPath = new List<string>();
            currentSettings.NonCheckedIncludeDirPaths = new List<string>();

            foreach (var library in libraries)
            {
                if (library.IsSelected)
                {
                    currentSettings.IncludeDirectoryPath.Add(library.GetCurrentLibraryPath());
                    currentSettings.NonCheckedIncludeDirPaths.AddRange(library.GetNonSelectedLibraryPaths());
                }
                else
                {
                    currentSettings.NonCheckedIncludeDirPaths.Add(library.GetCurrentLibraryPath());
                    currentSettings.NonCheckedIncludeDirPaths.AddRange(library.GetNonSelectedLibraryPaths());
                }
            }

            return currentSettings;
        }

        private IEnumerable<string> GetNonSelectedLibraryPaths()
        {
            return this.Versions.Where(x => x.Item1 != Version).Select(x => x.Item2).ToList();
        }

        public override string ToString()
        {
            return Name + " " + Version + " : " + GetCurrentLibraryPath();
        }

        public string GetCurrentLibraryPath()
        {
            var found = this.Versions.Where(x => x.Item1 == this.Version).FirstOrDefault();
            if (found != null)
            {
                return found.Item2;
            }
            else
            {
                return null;
            }
        }

        public void UpdateVersionPath(string version, string newPath)
        {
            var found = this.Versions.Where(x => x.Item1 == version).FirstOrDefault();
            if (found != null)
            {
                this.Versions.Remove(found);
                this.Versions.Add(new Tuple<string, string>(version, newPath));
            }
            else
            {
                this.Versions.Add(new Tuple<string, string>(version, newPath));
            }
        }

        private static List<char> InvalidChars = new List<char>()
        {
            {'!'},
            {'@'},
            {'#'},
            {'$'},
            {'%'},
            {'^'},
            {'&'},
            {'*'},
            {'('},
            {')'},
            {'-'},
            {'{'},
            {'}'},
            {'['},
            {']'},
            {'|'},
            {';'},
            {':'},
            {','},
            {'<'},
            //{'.'},
            {'>'},
            {'/'},
            {'?'},
            {'~'},
            {'`'},
            {'+'},
            {'='},
            //{' '},
        };

        private static bool LibraryExists(string path, string projectDir)
        {
            if (Path.IsPathRooted(path) == false)
            {
                path = Path.Combine(projectDir, path);
            }

            return File.Exists(path) || Directory.Exists(path);
        }
    }
}
