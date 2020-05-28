using NUnit.Framework;
using System.Collections.Generic;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    public class ComponentTests
    {

        [Test]
        public void When_CreateEmpty_Expect_Empty(){
            Component comp = new Component();
            Assert.IsEmpty(comp.faces);
            Assert.IsEmpty(comp.vertices);
        }

        [Test]
        public void When_AddVertex_Expect_VertexAdded(){
            Vertex vertex = new Vertex(1,2,3);
            Component comp = new Component();
            int index = comp.AddVertex(vertex);

            Assert.AreEqual(comp.vertices.Count, 1);
            Assert.AreEqual(comp.GetVertex(index), vertex);
        }

        [Test]
        public void When_CreateVertex_Expect_VertexAdded(){
            Component comp = new Component();
            int index = comp.CreateVertex(1,2,3);

            Assert.AreEqual(comp.vertices.Count, 1);
            Assert.AreEqual(comp.GetVertex(index).coordinate, new Vect3(1,2,3));
        }

        [Test]
        public void When_GetCoordinate_Expect_CorrectCoordinate(){
            Vertex vertex = new Vertex(1,2,3);
            Component comp = new Component();
            int index = comp.AddVertex(vertex);
            Assert.AreEqual(comp.GetCoordinate(index), vertex.coordinate);
        }

        [Test]
        public void When_CreateFaceFromArray_Expect_FaceAdded(){
            Vertex vertex1 = new Vertex(5,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Vertex vertex3 = new Vertex(8,2,2);
            Component comp = new Component();
            int[] vertices = new int[3];
            vertices[0] = comp.AddVertex(vertex1);
            vertices[1] = comp.AddVertex(vertex2);
            vertices[2] = comp.AddVertex(vertex3);

            Vect3 normal = Vect3.Cross(vertex1.coordinate - vertex2.coordinate, vertex1.coordinate - vertex3.coordinate);
            int a = comp.CreateFace(normal, vertices);

            Assert.AreEqual(comp.faces.Count, 1);
            CollectionAssert.AreEquivalent(comp.GetFace(a).vertexIndices, vertices);
        }

        [Test]
        public void When_CreateFaceFromList_Expect_FaceAdded(){
            Vertex vertex1 = new Vertex(1,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Vertex vertex3 = new Vertex(3,2,1);
            Component comp = new Component();
            List<int> vertices = new List<int>();
            vertices.Add(comp.AddVertex(vertex1));
            vertices.Add(comp.AddVertex(vertex2));
            vertices.Add(comp.AddVertex(vertex3));

            Vect3 normal = Vect3.Cross(vertex1.coordinate - vertex2.coordinate, vertex1.coordinate - vertex3.coordinate);
            int a = comp.CreateFace(normal, vertices);

            Assert.AreEqual(comp.faces.Count, 1);
            CollectionAssert.AreEquivalent(comp.GetFace(a).vertexIndices, vertices.ToArray());
        }

        [Test]
        public void When_AddFace_Expect_FaceAddedToVertices(){
            Vertex vertex1 = new Vertex(10,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Vertex vertex3 = new Vertex(3,2,1);
            Component comp = new Component();
            int[] vertices = new int[3];
            vertices[0] = comp.AddVertex(vertex1);
            vertices[1] = comp.AddVertex(vertex2);
            vertices[2] = comp.AddVertex(vertex3);
            Vect3 normal = Vect3.Cross(vertex1.coordinate - vertex2.coordinate, vertex1.coordinate - vertex3.coordinate);
            Face f = new Face(normal, vertices);

            int a = comp.AddFace(f);

            Assert.AreEqual(comp.GetFaceIndicesFromVertex(vertices[0])[0], a);
            Assert.AreEqual(comp.GetFaceIndicesFromVertex(vertices[1])[0], a);
            Assert.AreEqual(comp.GetFaceIndicesFromVertex(vertices[2])[0], a);
        }

        [Test]
        public void When_AddInvalidFace_Expect_Exception(){
            Assert.Throws<InvalidFaceException>(new TestDelegate(When_AddInvalidFace_Expect_Exception_Helper));
        }

        public void When_AddInvalidFace_Expect_Exception_Helper(){
            Vertex vertex1 = new Vertex(1,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Vertex vertex3 = new Vertex(3,8,10);
            Component comp = new Component();
            int[] vertices = new int[3];
            vertices[0] = comp.AddVertex(vertex1);
            vertices[1] = comp.AddVertex(vertex2);
            vertices[2] = comp.AddVertex(vertex3);
            Face face = new Face(new Vect3(1,1,1), vertices);
            comp.AddFace(face);
        }

        [Test]
        public void When_AddFaceIncorrectNrOfVertices_Expect_Exception(){
            Assert.Throws<InvalidFaceException>(new TestDelegate(When_AddFaceIncorrectNrOfVertices_Expect_Exception_Helper));
        }
        public void When_AddFaceIncorrectNrOfVertices_Expect_Exception_Helper(){
            Vertex vertex1 = new Vertex(1,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Component comp = new Component();
            int[] vertices = new int[2];
            vertices[0] = comp.AddVertex(vertex1);
            vertices[1] = comp.AddVertex(vertex2);
            Vect3 normal = Vect3.Cross(vertex1.coordinate - vertex2.coordinate, vertex1.coordinate - new Vect3(5,5,1));
            int a = comp.CreateFace(normal, vertices);
        }

        [Test]
        public void When_CreateFace_Expect_FaceAddedToVertices(){
            Vertex vertex1 = new Vertex(1,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Vertex vertex3 = new Vertex(3,8,1);
            Component comp = new Component();
            int[] vertices = new int[3];
            vertices[0] = comp.AddVertex(vertex1);
            vertices[1] = comp.AddVertex(vertex2);
            vertices[2] = comp.AddVertex(vertex3);

            Vect3 normal = Vect3.Cross(new Vect3(1,2,3) - new Vect3(2,2,2), new Vect3(1,2,3) - new Vect3(3,8,1));
            int a = comp.CreateFace(normal, vertices);

            Assert.AreEqual(comp.GetFaceIndicesFromVertex(vertices[0])[0], a);
            Assert.AreEqual(comp.GetFaceIndicesFromVertex(vertices[1])[0], a);
            Assert.AreEqual(comp.GetFaceIndicesFromVertex(vertices[2])[0], a);
        }

        [Test]
        public void When_CreateFaceWithoutNormal_Expect_Normal(){
            Vertex vertex1 = new Vertex(1,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Vertex vertex3 = new Vertex(3,8,1);
            Component comp = new Component();
            int[] vertices = new int[3];
            vertices[0] = comp.AddVertex(vertex1);
            vertices[1] = comp.AddVertex(vertex2);
            vertices[2] = comp.AddVertex(vertex3);

            Vect3 normal = Vect3.Cross(vertex1.coordinate - vertex2.coordinate,
                                        vertex1.coordinate - vertex3.coordinate);

            int a = comp.CreateFace(normal, vertices);
            Assert.AreEqual(comp.GetFace(a).normal, normal);
        }

        public Component HelpFunctionForNeighbouringFaces(){
            Component comp = new Component();
            Vertex vertex0 = new Vertex(1,2,3);
            Vertex vertex1 = new Vertex(8,12,3);
            Vertex vertex2 = new Vertex(1,5,1);
            Vertex vertex3 = new Vertex(4,1,3);
            Vertex vertex4 = new Vertex(10,11,7);
            Vertex vertex5 = new Vertex(8,3,5);
            Vertex vertex6 = new Vertex(9,7,8);
            Vertex vertex7 = new Vertex(5,6,2);
            Vertex vertex8 = new Vertex(3,4,11);

            comp.AddVertex(vertex0);
            comp.AddVertex(vertex1);
            comp.AddVertex(vertex2);
            comp.AddVertex(vertex3);
            comp.AddVertex(vertex4);
            comp.AddVertex(vertex5);
            comp.AddVertex(vertex6);
            comp.AddVertex(vertex7);
            comp.AddVertex(vertex8);

            Vect3 normal0 = Vect3.Cross(vertex0.coordinate - vertex1.coordinate, vertex0.coordinate - vertex2.coordinate);
            Vect3 normal1 = Vect3.Cross(vertex3.coordinate - vertex4.coordinate, vertex3.coordinate - vertex5.coordinate);
            Vect3 normal2 = Vect3.Cross(vertex4.coordinate - vertex5.coordinate, vertex4.coordinate - vertex8.coordinate);
            Vect3 normal3 = Vect3.Cross(vertex4.coordinate - vertex6.coordinate, vertex4.coordinate - vertex8.coordinate);
            Vect3 normal4 = Vect3.Cross(vertex6.coordinate - vertex7.coordinate, vertex6.coordinate - vertex8.coordinate);
            Vect3 normal5 = Vect3.Cross(vertex5.coordinate - vertex7.coordinate, vertex5.coordinate - vertex8.coordinate);

            int face0 = comp.CreateFace(normal0, new int[]{0,1,2});
            int face1 = comp.CreateFace(normal1, new int[]{3,4,5});
            int face2 = comp.CreateFace(normal2, new int[]{4,5,8});
            int face3 = comp.CreateFace(normal3, new int[]{4,6,8});
            int face4 = comp.CreateFace(normal4, new int[]{6,7,8});
            int face5 = comp.CreateFace(normal5, new int[]{5,7,8});

            return comp;
        }

        [Test]
        public void When_GetNeighbouringFacesViaVertices_Expect_CorrectFaces(){
            Component comp = HelpFunctionForNeighbouringFaces();

            List<int> expectedResult = new List<int>();
            CollectionAssert.AreEquivalent(comp.GetNeighbouringFacesViaVertices(0), expectedResult);

            expectedResult.Add(2);
            expectedResult.Add(3);
            expectedResult.Add(5);
            CollectionAssert.AreEquivalent(comp.GetNeighbouringFacesViaVertices(1), expectedResult);

            expectedResult = new List<int>();
            expectedResult.Add(1);
            expectedResult.Add(3);
            expectedResult.Add(4);
            expectedResult.Add(5);
            CollectionAssert.AreEquivalent(comp.GetNeighbouringFacesViaVertices(2), expectedResult);
        }

        [Test]
        public void When_GetNeighbouringFacesViaEdges_Expect_CorrectFaces(){
            Component comp = HelpFunctionForNeighbouringFaces();

            List<int> expectedResult = new List<int>();
            CollectionAssert.AreEquivalent(comp.GetNeighbouringFacesViaEdges(0), expectedResult);

            expectedResult.Add(2);
            CollectionAssert.AreEquivalent(comp.GetNeighbouringFacesViaEdges(1), expectedResult);

            expectedResult = new List<int>();
            expectedResult.Add(1);
            expectedResult.Add(3);
            expectedResult.Add(5);
            CollectionAssert.AreEquivalent(comp.GetNeighbouringFacesViaEdges(2), expectedResult);

            expectedResult = new List<int>();
            expectedResult.Add(3);
            expectedResult.Add(5);
            CollectionAssert.AreEquivalent(comp.GetNeighbouringFacesViaEdges(4), expectedResult);
        }

        [Test]
        public void When_GetVertexIndices_Expect_CorrectVertexIndices(){
            Vertex vertex1 = new Vertex(1,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Vertex vertex3 = new Vertex(3,8,1);
            Component comp = new Component();
            int[] vertices = new int[3];
            vertices[0] = comp.AddVertex(vertex1);
            vertices[1] = comp.AddVertex(vertex2);
            vertices[2] = comp.AddVertex(vertex3);
            Vect3 normal = Vect3.Cross(vertex1.coordinate - vertex2.coordinate, vertex1.coordinate - vertex3.coordinate);

            int faceIndex = comp.CreateFace(normal, vertices);
            CollectionAssert.AreEquivalent(comp.GetVertexIndicesFromFace(faceIndex), vertices);
        }

        [Test]
        public void When_GetFace_Expect_CorrectFace(){
            Vertex vertex1 = new Vertex(1,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Vertex vertex3 = new Vertex(3,8,1);
            Component comp = new Component();
            int[] vertices = new int[3];
            vertices[0] = comp.AddVertex(vertex1);
            vertices[1] = comp.AddVertex(vertex2);
            vertices[2] = comp.AddVertex(vertex3);

            Vect3 normal = Vect3.Cross(vertex1.coordinate - vertex2.coordinate,
                                        vertex1.coordinate - vertex3.coordinate);

            Face face = new Face(normal, vertices);
            int faceIndex = comp.AddFace(face);

            Assert.AreEqual(comp.GetFace(faceIndex), face);
        }

        [Test]
        public void When_GetVertex_Expect_CorrectVertex(){
            Vertex vertex1 = new Vertex(1,2,3);
            Component comp = new Component();

            int vertexIndex = comp.AddVertex(vertex1);
            Assert.AreEqual(comp.GetVertex(vertexIndex), vertex1);
        }

        [Test]
        public void When_CreateDeepCopy_Expect_SeparateEqualInstance(){
            Vertex vertex1 = new Vertex(1,2,3);
            Vertex vertex2 = new Vertex(2,2,2);
            Vertex vertex3 = new Vertex(3,8,1);
            Component comp = new Component();
            int[] vertices = new int[3];
            vertices[0] = comp.AddVertex(vertex1);
            vertices[1] = comp.AddVertex(vertex2);
            vertices[2] = comp.AddVertex(vertex3);

            Vect3 normal = Vect3.Cross(vertex1.coordinate - vertex2.coordinate,
                                        vertex1.coordinate - vertex3.coordinate);

            Face face = new Face(normal, vertices);
            int faceIndex = comp.AddFace(face);

            Component compCopy = comp.DeepCopy();

            for (int i = 0; i < comp.vertices.Count; i++)
            {
                Assert.AreEqual(comp.vertices[i], compCopy.vertices[i]);
            }

            for (int i = 0; i < comp.faces.Count; i++)
            {
                Assert.AreEqual(comp.faces[i], compCopy.faces[i]);
            }

            Assert.AreEqual(comp, compCopy);

            compCopy.vertices[0].coordinate.x = 99;

            Assert.AreNotEqual(comp, compCopy);
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_TranslateCube_Expect_AllVerticesMoved(Structure simpleCube){

            Component cube = simpleCube.components[0];

            Component translatedCube = cube.Transform(Vect3.Transforms.Translation(1,0,0));

            for (int i = 0; i < cube.vertices.Count; i++)
            {
                Assert.AreEqual(cube.vertices[i].coordinate.x + 1, translatedCube.vertices[i].coordinate.x);
            }
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_RotateCubeAroundX_Expect_AllVerticesRotated(Structure simpleCube){

            Component cube = simpleCube.components[0];

            Component translatedCube = cube.Transform(Vect3.Transforms.Rotation(System.Math.PI/2d, 0, 0));

            Assert.AreEqual(new Vect3(0,0,0), translatedCube.vertices[0].coordinate);
            Assert.AreEqual(new Vect3(0,-1,0), translatedCube.vertices[1].coordinate);
            Assert.AreEqual(new Vect3(0,0,1), translatedCube.vertices[2].coordinate);
            Assert.AreEqual(new Vect3(1,0,0), translatedCube.vertices[3].coordinate);
            Assert.AreEqual(new Vect3(0,-1,1), translatedCube.vertices[4].coordinate);
            Assert.AreEqual(new Vect3(1,-1,0), translatedCube.vertices[5].coordinate);
            Assert.AreEqual(new Vect3(1,0,1), translatedCube.vertices[6].coordinate);
            Assert.AreEqual(new Vect3(1,-1,1), translatedCube.vertices[7].coordinate);

        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_RotateCubeAroundY_Expect_AllVerticesRotated(Structure simpleCube){

            Component cube = simpleCube.components[0];

            Component translatedCube = cube.Transform(Vect3.Transforms.Rotation(0, System.Math.PI/2d, 0));

            Assert.AreEqual(new Vect3(0,0,0), translatedCube.vertices[0].coordinate);
            Assert.AreEqual(new Vect3(1,0,0), translatedCube.vertices[1].coordinate);
            Assert.AreEqual(new Vect3(0,1,0), translatedCube.vertices[2].coordinate);
            Assert.AreEqual(new Vect3(0,0,-1), translatedCube.vertices[3].coordinate);
            Assert.AreEqual(new Vect3(1,1,0), translatedCube.vertices[4].coordinate);
            Assert.AreEqual(new Vect3(1,0,-1), translatedCube.vertices[5].coordinate);
            Assert.AreEqual(new Vect3(0,1,-1), translatedCube.vertices[6].coordinate);
            Assert.AreEqual(new Vect3(1,1,-1), translatedCube.vertices[7].coordinate);

        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_RotateCubeAroundZ_Expect_AllVerticesRotated(Structure simpleCube){

            Component cube = simpleCube.components[0];

            Component translatedCube = cube.Transform(Vect3.Transforms.Rotation(0, 0, System.Math.PI/2d));

            Assert.AreEqual(new Vect3(0,0,0), translatedCube.vertices[0].coordinate);
            Assert.AreEqual(new Vect3(0,0,1), translatedCube.vertices[1].coordinate);
            Assert.AreEqual(new Vect3(-1,0,0), translatedCube.vertices[2].coordinate);
            Assert.AreEqual(new Vect3(0,1,0), translatedCube.vertices[3].coordinate);
            Assert.AreEqual(new Vect3(-1,0,1), translatedCube.vertices[4].coordinate);
            Assert.AreEqual(new Vect3(0,1,1), translatedCube.vertices[5].coordinate);
            Assert.AreEqual(new Vect3(-1,1,0), translatedCube.vertices[6].coordinate);
            Assert.AreEqual(new Vect3(-1,1,1), translatedCube.vertices[7].coordinate);

        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_RotateCubeAroundXYZ_Expect_InvertedThroughOrigin(Structure simpleCube){

            Component cube = simpleCube.components[0];

            //Flip CCW around X, CW around Y, CW around Z => Cube has flipped through origin
            Component translatedCube = cube.Transform(Vect3.Transforms.Rotation(-System.Math.PI/2d, System.Math.PI/2d, System.Math.PI/2d));

            Assert.AreEqual(new Vect3(0,0,0), translatedCube.vertices[0].coordinate);
            Assert.AreEqual(new Vect3(-1,0,0), translatedCube.vertices[1].coordinate);
            Assert.AreEqual(new Vect3(0,-1,0), translatedCube.vertices[2].coordinate);
            Assert.AreEqual(new Vect3(0,0,-1), translatedCube.vertices[3].coordinate);
            Assert.AreEqual(new Vect3(-1,-1,0), translatedCube.vertices[4].coordinate);
            Assert.AreEqual(new Vect3(-1,0,-1), translatedCube.vertices[5].coordinate);
            Assert.AreEqual(new Vect3(0,-1,-1), translatedCube.vertices[6].coordinate);
            Assert.AreEqual(new Vect3(-1,-1,-1), translatedCube.vertices[7].coordinate);

        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_RotateCubeCenteredAroundY_Expect_NormalsRotated(Structure simpleCube){

            Component cube = simpleCube.components[0];
            Component offsetCube = cube.Transform(Vect3.Transforms.Translation(-0.5d, 0, -0.5d));

            //CW around Y =>
            // Front => Left
            // Left => Back
            // Back => Right
            // Right => Front
            // Top, Bottom unchanged
            Component translatedCube = offsetCube.Transform(Vect3.Transforms.Rotation(0, System.Math.PI/2d, 0));

            var Front = new Vect3(0,0,-1);
            var Right = new Vect3(1,0,0);
            var Left = new Vect3(-1,0,0);
            var Bottom = new Vect3(0,-1,0);
            var Back = new Vect3(0,0,1);
            var Top = new Vect3(0,1,0);

            // was Front
            Assert.AreEqual(Left, translatedCube.faces[0].normal);
            Assert.AreEqual(Left, translatedCube.faces[1].normal);
            // was Right
            Assert.AreEqual(Front, translatedCube.faces[2].normal);
            Assert.AreEqual(Front, translatedCube.faces[3].normal);
            // was Left
            Assert.AreEqual(Back, translatedCube.faces[4].normal);
            Assert.AreEqual(Back, translatedCube.faces[5].normal);
            // was Bottom
            Assert.AreEqual(Bottom, translatedCube.faces[6].normal);
            Assert.AreEqual(Bottom, translatedCube.faces[7].normal);
            // was Back
            Assert.AreEqual(Right, translatedCube.faces[8].normal);
            Assert.AreEqual(Right, translatedCube.faces[9].normal);
            // was Top
            Assert.AreEqual(Top, translatedCube.faces[10].normal);
            Assert.AreEqual(Top, translatedCube.faces[11].normal);

        }


    }
}
