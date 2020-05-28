using System.Collections.Generic;
using NUnit.Framework;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    public class FilePathsTests
    {
        [Test]
        public void When_CreateEmpty_Expect_Empty()
        {
            FilePaths fp = new FilePaths();
            Assert.AreEqual(fp.inFile, "");
            Assert.AreEqual(fp.outFile, "");
        }

        [Test]
        public void When_CreateExplicit_Expect_Explicit()
        {
            FilePaths fp = new FilePaths("inFile", "outFile");
            Assert.AreEqual(fp.inFile, "inFile");
            Assert.AreEqual(fp.outFile, "outFile");
        }
    }

    [TestFixture]
    public class VertexTests
    {
        [Test]
        public void When_CreateEmptyFaceIndices_Expect_EmptyFaceIndices()
        {
            Vertex v = new Vertex(1,1,1);
            Assert.AreEqual(v.coordinate, Vect3.One);
            Assert.IsEmpty(v.faceIndices);
        }

        [Test]
        public void When_CreateExplicitVector_Expect_Explicit()
        {
            Vect3 vect = new Vect3(1, 2, 3);
            Vertex v = new Vertex(vect);
            Assert.AreEqual(v.coordinate, vect);
            Assert.IsEmpty(v.faceIndices);
        }

        [Test]
        public void When_CreateExplicitDoubles_Expect_Explicit()
        {
            Vect3 vect = new Vect3(1, 2, 3);
            Vertex v = new Vertex(1, 2, 3);
            Vertex v2 = new Vertex(vect);
            Assert.AreEqual(v.coordinate, vect);
            Assert.AreEqual(v.coordinate, v2.coordinate);
            Assert.IsEmpty(v.faceIndices);
        }

        [Test]
        public void When_AddNegativeIndex_Expect_Exception()
        {
            Assert.Throws<NegativeIndexException>(new TestDelegate(When_AddNegativeIndex_Expect_Exception_Helper));
        }
        private void When_AddNegativeIndex_Expect_Exception_Helper()
        {
            Vertex v = new Vertex(1,1,1);
            v.AddFaceIndex(-1);
        }

        [Test]
        public void When_AddValidIndex_Expect_Valid()
        {
            Vertex v = new Vertex(1,2,3);
            v.AddFaceIndex(5);
            Assert.AreEqual(v.faceIndices.Count, 1);
            Assert.AreEqual(v.faceIndices[0], 5);
        }

        [Test]
        public void When_AddMultipleFaces_Expect_AllFaces() {
            Vect3 vector = new Vect3(1,2,3);
            Vertex vertex = new Vertex(vector);
            List<int> faces = new List<int>{1,2,3,4,5};
            vertex.AddFaceIndices(faces);
            Assert.AreEqual(vertex.faceIndices.Count, faces.Count);
            Assert.AreEqual(vertex.faceIndices, faces);
        }

        [Test]
        public void When_AddNoFaces_Expect_NoFaces() {
            Vect3 vector = new Vect3(1,2,3);
            Vertex vertex = new Vertex(vector);
            List<int> faces = new List<int>();
            vertex.AddFaceIndices(faces);

            Assert.AreEqual(vertex.faceIndices.Count, 0);
            Assert.AreEqual(vertex.faceIndices, faces);
        }

    }

    [TestFixture]
    public class FaceTests
    {
        [Test]
        public void When_CreateExplicitVector_Expect_Explicit()
        {
            Vect3 vect = new Vect3(1, 2, 3);
            int[] vertices = {1,2,3};
            Face f = new Face(vect, vertices);
            Assert.AreEqual(f.normal, vect);
            Assert.AreEqual(f.vertexIndices, vertices);
        }

        [Test]
        public void When_CreateExplicitDoubles_Expect_Explicit()
        {
            Vect3 vect = new Vect3(1, 2, 3);
            int[] vertices = {5,2,3};
            Face f = new Face(1, 2, 3, vertices);
            Assert.AreEqual(f.normal, vect);
            CollectionAssert.AreEquivalent(f.vertexIndices, vertices);
        }

        [Test]
        public void When_AddNegativeIndex_Expect_Exception()
        {
            Assert.Throws<NegativeIndexException>(new TestDelegate(When_AddNegativeIndex_Expect_Exception_Helper));
        }
        private void When_AddNegativeIndex_Expect_Exception_Helper()
        {
            Vect3 vect = new Vect3(1,1,1);
            Face f = new Face(vect, new int[]{1,-1,3});
        }

        [Test]
        public void When_AddDuplicateIndex_Expect_Exception()
        {
            Assert.Throws<InvalidFaceException>(new TestDelegate(When_AddDuplicateIndex_Expect_Exception_Helper));
        }
        private void When_AddDuplicateIndex_Expect_Exception_Helper()
        {
            Vect3 vect = new Vect3(1,1,1);
            Face f = new Face(vect, new int[]{1,1,3});
        }

        [Test]
        public void When_AddValidIndex_Expect_Valid()
        {
            Face f = new Face(1,1,1 ,new int[]{5,3,2});
            Assert.AreEqual(f.vertexIndices.Length, 3);
            Assert.AreEqual(f.normal, Vect3.One);
        }
    }

    [TestFixture]
    public class NodeEdgePairTests
    {
        [Test]
        public void When_CreateExplicit_Expect_Explicit()
        {
            NodeEdgePair nodeEdgePair = new NodeEdgePair(1, 2);
            Assert.AreEqual(nodeEdgePair.nodeIndex, 1);
            Assert.AreEqual(nodeEdgePair.edgeIndex, 2);
        }

        [Test]
        public void When_AccessDefaultNodeIndices_Expect_Exception()
        {
            Assert.Throws<NegativeIndexException>(AccessDefaultNeighborNodeIndex);
            Assert.Throws<NegativeIndexException>(AccessDefaultEdgeIndex);
        }
        private void AccessDefaultNeighborNodeIndex()
        {
            NodeEdgePair nodeEdgePair = new NodeEdgePair();
            int myInt = nodeEdgePair.nodeIndex;
        }
        private void AccessDefaultEdgeIndex()
        {
            NodeEdgePair nodeEdgePair = new NodeEdgePair();
            int myInt = nodeEdgePair.edgeIndex;
        }


        [Test]
        public void When_CreateNegativeNodeIndex_Expect_Exception()
        {
            Assert.Throws<NegativeIndexException>(When_CreateNegativeNodeIndex_Expect_Exception_Helper);
        }
        private void When_CreateNegativeNodeIndex_Expect_Exception_Helper()
        {
            NodeEdgePair nodeEdgePair = new NodeEdgePair(-1, 2);
        }

        [Test]
        public void When_CreateNegativeEdgeIndex_Expect_Exception()
        {
            Assert.Throws<NegativeIndexException>(When_CreateNegativeEdgeIndex_Expect_Exception_Helper);
        }
        private void When_CreateNegativeEdgeIndex_Expect_Exception_Helper()
        {
            NodeEdgePair nodeEdgePair = new NodeEdgePair(1, -2);
        }
    }

    [TestFixture]
    public class NodeTests
    {
        [Test]
        public void When_CreateEmpty_Expect_Empty()
        {
            Node n = new Node();
            Assert.AreEqual(n.coordinate, Vect3.Zero);
            Assert.IsEmpty(n.neighbors);
        }

        [Test]
        public void When_CreateWithoutIndex_Expect_NegativeIndex() {
            Node n = new Node();
            Assert.AreEqual(-1, n.index);
        }

        [Test]
        public void When_CreateExplicitVector_Expect_Explicit()
        {
            Vect3 vect = new Vect3(1, 2, 3);
            Node n = new Node(vect);
            Assert.AreEqual(n.coordinate, vect);
            Assert.IsEmpty(n.neighbors);
        }

        [Test]
        public void When_CreateExplicitDouble_Expect_Explicit()
        {
            Vect3 vect = new Vect3(1, 2, 3);
            Node n = new Node(1, 2, 3);
            Assert.AreEqual(n.coordinate, vect);
            Assert.IsEmpty(n.neighbors);
        }

        [Test]
        public void When_CreateDeepCopy_ExpectSeparateInstance() {
            Node node = new Node(1, 2, 3);

            node.index = 1;

            node.neighbors.Add(new NodeEdgePair(12, 12));

            Node nodeIndexDifference = node.DeepCopy();
            Node nodeCoordinateDifference = node.DeepCopy();
            Node nodeNeighborDifference = node.DeepCopy();

            nodeIndexDifference.index = 712341;
            nodeCoordinateDifference.coordinate.x = 12215;
            nodeNeighborDifference.neighbors[0].edgeIndex = 125126;

            Assert.AreNotEqual(nodeIndexDifference, node);
            Assert.AreNotEqual(nodeCoordinateDifference, node);
            Assert.AreNotEqual(nodeNeighborDifference, node);
        }
    }

    [TestFixture]
    public class EdgeTests
    {
        public void When_CreateExplicit_Expect_Explicit()
        {
            Edge edge = new Edge(1, 2);
            Assert.AreEqual(edge.nodeIndex1, 1);
            Assert.AreEqual(edge.nodeIndex2, 2);
        }

        [Test]
        public void When_CreateWithoutIndex_Expect_NegativeIndex() {
            Edge edge = new Edge();
            Assert.AreEqual(-1, edge.index);
        }

        [Test]
        public void When_AccessDefaultEdgeIndices_Expect_Exception()
        {
            Assert.Throws<NegativeIndexException>(AccessDefaultNodeIndexOne);
            Assert.Throws<NegativeIndexException>(AccessDefaultNodeIndexTwo);
        }
        private void AccessDefaultNodeIndexOne()
        {
            Edge edge = new Edge();
            int myInt = edge.nodeIndex1;
        }
        private void AccessDefaultNodeIndexTwo()
        {
            Edge edge = new Edge();
            int myInt = edge.nodeIndex2;
        }

        public void When_CreateNegativeNodeIndex1_Expect_Exception()
        {
            Assert.Throws<NegativeIndexException>(When_CreateNegativeNodeIndex1_Expect_Exception_Helper);
        }
        private void When_CreateNegativeNodeIndex1_Expect_Exception_Helper()
        {
            Edge edge = new Edge(-1, 2);
        }

        public void When_CreateNegativeNodeIndex2_Expect_Exception()
        {
            Assert.Throws<NegativeIndexException>(When_CreateNegativeNodeIndex2_Expect_Exception_Helper);
        }
        private void When_CreateNegativeNodeIndex2_Expect_Exception_Helper()
        {
            Edge edge = new Edge(1, -2);
        }

        [Test]
        public void When_CreateDeepCopy_ExpectSeparateInstance() {
            Edge edge = new Edge(1, 2);

            Edge edgeDeepCopy = edge.DeepCopy();

            edge.nodeIndex1 = 7;

            Assert.AreNotEqual(edge, edgeDeepCopy);
        }
    }
}
