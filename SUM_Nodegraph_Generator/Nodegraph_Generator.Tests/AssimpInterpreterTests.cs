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

        [TestCase ("StrippedTunnel.fbx")]
        [TestCase ("StrippedTurnZone.fbx")]
        public void When_Interpret_Expect_SceneEquvialentVertexAsStructure (string filename) {
            string filepath = ArgParser.GetPath(Path.Combine (OSString, "Input_Files", filename));
            AssimpContext context = new AssimpContext ();
            Scene scene;
            try {
                NormalSmoothingAngleConfig config = new NormalSmoothingAngleConfig (0.0f);
                context.SetConfig (config);
                scene = context.ImportFile (filepath,
                    PostProcessSteps.Triangulate |
                    PostProcessSteps.JoinIdenticalVertices |
                    PostProcessSteps.CalculateTangentSpace |
                    PostProcessSteps.GenerateNormals);
            } catch {
                Assert.Fail ("Assimp config setup failed.");
                // Return so compiler understands we never get further and scene is always initialized below this point.
                return;
            }

            DataInterpreter assimp = new AssimpInterpreter ();
            Structure struc = assimp.Interpret (filepath);

            Assert.AreEqual (struc.components.Count, scene.Meshes.Count, "Number of meshes in scene and components in structure are not equal.");

            int totalVerticesScene = 0;
            int totalVerticesStructure = 0;
            for (int i = 0; i < scene.Meshes.Count; i++) {
                totalVerticesScene += scene.Meshes[i].Vertices.Count;
                totalVerticesStructure += struc.components[i].vertices.Count;
            }

            Assert.True (totalVerticesStructure <= totalVerticesScene, "Structure has more vertices than assimp mesh.");
            for (int i = 0; i < scene.Meshes.Count; i++) {
                foreach (Vertex vertex1 in struc.components[i].vertices) {
                    bool foundEquivalent = false;
                    foreach (var vertex2 in scene.Meshes[i].Vertices) {
                        if (CoordinateEqual (vertex1, vertex2)) {
                            foundEquivalent = true;
                            break;
                        }
                    }
                    Assert.True (foundEquivalent);
                }
            }
        }

        [TestCase ("StrippedTunnel.fbx")]
        [TestCase ("StrippedTurnZone.fbx")]
        public void When_Interpret_Expect_SceneEquvialentFaceAsStructure (string filename) {
            string filepath = ArgParser.GetPath(Path.Combine(OSString, "Input_Files", filename));
            AssimpContext context = new AssimpContext ();
            Scene scene;
            try {
                NormalSmoothingAngleConfig smoothingConfig = new NormalSmoothingAngleConfig(0.0f);
                SortByPrimitiveTypeConfig removeConfig = new SortByPrimitiveTypeConfig(PrimitiveType.Point | PrimitiveType.Line);
                context.SetConfig(smoothingConfig);
                context.SetConfig(removeConfig);
                scene = context.ImportFile (filepath,
                                           PostProcessSteps.Triangulate |
                                           PostProcessSteps.JoinIdenticalVertices |
                                           PostProcessSteps.CalculateTangentSpace |
                                           PostProcessSteps.GenerateNormals |
                                           PostProcessSteps.FindDegenerates |
                                           PostProcessSteps.SortByPrimitiveType |
                                           PostProcessSteps.FixInFacingNormals);
            } catch {
                Assert.Fail ("Assimp config setup failed.");
                // Return so compiler understands we never get further and scene is always initialized below this point.
                return;
            }

            DataInterpreter assimp = new AssimpInterpreter ();
            Structure struc = assimp.Interpret (filepath);

            Assert.AreEqual (struc.components.Count, scene.Meshes.Count, "Number of meshes in scene and components in structure are not equal.");

            int totalFacesScene = 0;
            int totalFacesStructure = 0;
            for (int i = 0; i < scene.Meshes.Count; i++) {
                totalFacesScene += scene.Meshes[i].Faces.Count;
                totalFacesStructure += struc.components[i].faces.Count;
            }
            Assert.AreEqual (totalFacesScene, totalFacesStructure, "Structure and Mesh doesn't have the same number of faces.");

            // Equivalent faces in component and mesh should have the same index but we don't want the test to rely on it so we look through the whole list.
            for (int i = 0; i < scene.Meshes.Count; i++) {
                foreach (Face faceStructure in struc.components[i].faces) {
                    Mesh mesh = scene.Meshes[i];
                    Component comp = struc.components[i];
                    bool foundEquivalent = false;
                    foreach (var faceScene in mesh.Faces) {
                        if (CoordinateEqual (faceStructure.normal, mesh.Normals[faceScene.Indices[0]]) &&
                            CoordinateEqual (comp.vertices[faceStructure.vertexIndices[0]], mesh.Vertices[faceScene.Indices[0]]) &&
                            CoordinateEqual (comp.vertices[faceStructure.vertexIndices[1]], mesh.Vertices[faceScene.Indices[1]]) &&
                            CoordinateEqual (comp.vertices[faceStructure.vertexIndices[2]], mesh.Vertices[faceScene.Indices[2]])) {
                            foundEquivalent = true;
                            break;
                        }
                    }
                    Assert.True (foundEquivalent);
                }
            }
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