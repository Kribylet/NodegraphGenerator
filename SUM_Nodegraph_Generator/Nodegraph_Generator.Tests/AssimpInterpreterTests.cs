using System;
using System.IO;
using System.Runtime.InteropServices;
using Assimp;
using Assimp.Configs;
using NUnit.Framework;

namespace Nodegraph_Generator.Tests {
    [TestFixture]
    public class AssimpInterpreterTests {

        private const String WindowsPathFormat = "..\\..\\..\\..\\";
        private const String MacPathFormat = "../../../../";
        private static bool windowsRuntime = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private String OSString = windowsRuntime ? WindowsPathFormat : MacPathFormat;

        /*
         * Checks if the two vertices (assimp vertex is Vector3D) are equivalent with Util.NearlyEqual.
         */
        public bool CoordinateEqual (Vertex vect1, Vector3D vect2) {
            return Util.NearlyEqual (vect1.coordinate.x, vect2.X) && Util.NearlyEqual (vect1.coordinate.y, vect2.Y) &&
                Util.NearlyEqual (vect1.coordinate.z, vect2.Z);
        }

        /*
         * Checks if the two vectors are equivalent with Util.NearlyEqual.
         */
        public bool CoordinateEqual (Vect3 vect1, Vector3D vect2) {
            return Util.NearlyEqual (vect1.x, vect2.X) && Util.NearlyEqual (vect1.y, vect2.Y) &&
                Util.NearlyEqual (vect1.z, vect2.Z);
        }

        [Test]
        public void When_NonExistingFile_Expect_FileNotFoundException () {
            Assert.Throws<FileNotFoundException> (new TestDelegate (When_NonExistingFile_Expect_FileNotFoundException_Helper));
        }

        public void When_NonExistingFile_Expect_FileNotFoundException_Helper () {
            DataInterpreter assimp = new AssimpInterpreter ();
            assimp.Interpret ("hej.fbx");
        }

        [Test]
        public void When_InvalidFile_Expect_IOException () {
            Assert.Throws<IOException> (new TestDelegate (When_InvalidFile_Expect_IOException_Helper));
        }

        public void When_InvalidFile_Expect_IOException_Helper () {
            string filepath = ArgParser.GetPath(Path.Combine(OSString, "Input_Files", "notfbx.fbx"));
            DataInterpreter assimp = new AssimpInterpreter ();
            assimp.Interpret (filepath);
        }

    }
}