using System;
using System.IO;
using Microsoft.SPOT;
using System.Collections;
using System.Reflection;
using SecretLabs.NETMF.IO;
using Microsoft.SPOT.Hardware;

namespace netduino.helpers.Helpers {
    /*
    Copyright (C) 2011 by Fabien Royer

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
    */
    /// <summary>
    /// General-purpose resource management class supporting bitmaps, strings and assemblies
    /// </summary>
    public class SDResourceLoader : IDisposable
    {
        public Hashtable Strings;
        public Hashtable Bitmaps;
        public Hashtable RTTLSongs;

        const string SdMountPoint = "SD";

        /// <summary>
        /// Mounts an SD card and reads the content of the manifest to instantiate the corresponding objects.
        /// By default, the resources to need to be loaded are in a file named 'resources.txt' at the root of the SD card.
        /// Filenames referenced in the manifest can be placed in subdirectories relative to the SD card mount point.
        ///
        /// Sample manifest:
        /// 
        /// bitmap:name=spaceinvaders.bmp.bin;width=32;height=8
        /// bitmap:name=c64-computer.bmp.bin;width=256;height=16
        /// string:name=hi;value=Hello!
        /// string:name=gameover;value=Game Over!
        /// rttl:name=songs.txt
        /// assembly:file=assm.pe;name=ASSM,Version=1.0.0.0;class=ASSM.TestClass;method=Print
        /// 
        /// Any line starting with a '*' will be skipped.
        /// 
        /// Note: leave the method parameter empty to only load the assembly.
        /// Only provide a method as the entry point to be called once all the other assemblies have been loaded.
        /// 
        /// Note: bitmap resources are expected to be a 1-bit depth images in binary format.
        /// 
        /// </summary>
        /// <param name="resourceManifest"></param>
        public SDResourceLoader(Cpu.Pin chipSelect, string resourceManifest = "resources.txt", SPI.SPI_module spiModule = SPI.SPI_module.SPI1)
        {
            Strings = new Hashtable();
            Bitmaps = new Hashtable();
            RTTLSongs = new Hashtable();

            // BUG BUG
            // In firmware v4.1.1.0 alpha 3, only the mount point string matters. The rest is hard-coded.
            // This will need to be changed once the final firmware is released.
            StorageDevice.MountSD(SdMountPoint, spiModule, chipSelect);

            // Read the content of the resource manifest and build the corresponding resources
            using (TextReader reader = new StreamReader(SdMountPoint + @"\" + resourceManifest)){
                string line;
                
                // Parse each line of the manifest and build the corresponding resource object
                while ((line = reader.ReadLine()).Length != 0) {
                    Debug.Print(line);

                    // Skip any line starting with a '*'
                    if (line[0] == '*') {
                        continue;
                    }

                    // Split the line on the colon delimiter to obtain the type of the resource
                    string[] list = line.Split(':');

                    Hashtable hash = Parse(list[1]);

                    if (list[0] == "bitmap") {
                        BuildBitmapResource(Int32.Parse((string) hash["width"]), Int32.Parse((string) hash["height"]), (string) hash["name"]);
                    }
                    else if (list[0] == "string") {
                        Strings.Add(hash["name"],hash["value"]);
                    } 
                    else if (list[0] == "rttl") {
                        BuildRttlResource((string)hash["name"]);
                    } 
                    else if (list[0] == "assembly") {
                        LoadAssembly(hash);
                    }
                }
            }
        }
        /// <summary>
        /// Loads an assembly in little-endian PE format and invokes the entry point method if provided
        /// </summary>
        /// <param name="args">A hash table providing the parameters needed to load/execute the assembly</param>
        protected void LoadAssembly(Hashtable args) {
            using (var assmfile = new FileStream(SdMountPoint + @"\" + args["file"], FileMode.Open, FileAccess.Read, FileShare.None)) {
                var assmbytes = new byte[(int) assmfile.Length];
                assmfile.Read(assmbytes, 0, (int) assmfile.Length);
                var assm = Assembly.Load(assmbytes);
                var versionString = (string)args["name"] + ", Version=" + (string)args["version"];
                var obj = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(versionString, (string)args["class"]);

                if (args.Contains("method")) {
                    var type = assm.GetType((string)args["class"]);
                    MethodInfo mi = type.GetMethod((string)args["method"]);
                    mi.Invoke(obj, null);
                }
            }
        }

        /// <summary>
        /// Helper function facilitating the parsing of manifest lines
        /// </summary>
        /// <param name="str">A string of ';' delimited name/value pairs, each separated by an '=' sign.</param>
        /// <returns>A hashtable of name/value pairs</returns>
        protected Hashtable Parse(string str) {
            string[] bitmapParams = str.Split(';');
            var hash = new Hashtable();
            foreach (string paramString in bitmapParams) {
                string[] pair = paramString.Split('=');

                hash.Add(pair[0], pair[1]);
            }

            return hash;
        }

        /// <summary>
        /// Creates a 1-bit depth bitmap object from a binary file
        /// </summary>
        /// <param name="widthinPixels">Width of the bitmap in pixels</param>
        /// <param name="heightinPixels">Height of the bitmap in pixels</param>
        /// <param name="filename">Filename containing the binary data defining the bitmap</param>
        protected void BuildBitmapResource(int widthinPixels, int heightinPixels, string filename) {
            using (var bmpfile = new FileStream(SdMountPoint + @"\" + filename, FileMode.Open, FileAccess.Read, FileShare.None)) {
                // Note: don't exceed the amount of RAM available in the netduino by loading files that are too large!
                // This will result in an out-of-memory exception.
                var bitmapdata = new byte[(int) bmpfile.Length];
                bmpfile.Read(bitmapdata, 0, (int) bmpfile.Length);
                var bitmap = new Imaging.Bitmap(bitmapdata, widthinPixels, heightinPixels);
                Bitmaps.Add(filename, bitmap);
            }
        }

        /// <summary>
        /// Create song objects from a set of RTTL string in a file
        /// </summary>
        /// <param name="filename">Name of the file with the RTTL encoded strings</param>
        protected void BuildRttlResource(string filename) {
            // Read the content of the resource manifest and build the corresponding resources
            using (TextReader reader = new StreamReader(SdMountPoint + @"\" + filename)){
                string rttlData;
                while ((rttlData = reader.ReadLine()).Length != 0) {
                    var song = new Sound.RttlSong(rttlData);
                    RTTLSongs.Add(song.Name, song);
                }
            }
        }

        /// <summary>
        /// Releases the SD card mount point.
        /// </summary>
        public void Dispose() {
            StorageDevice.Unmount(SdMountPoint);
        }
    }
}
