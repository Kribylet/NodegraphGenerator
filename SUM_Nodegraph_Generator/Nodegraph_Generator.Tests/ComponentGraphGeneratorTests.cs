using System;
using System.Collections;
using System.Collections.Generic;
using Nodegraph_Generator;
using NUnit.Framework;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    class ComponentGraphGeneratorTests
    {
        // Suppress warnings about unused helper variables
        #pragma warning disable 414
        private static readonly int SimpleCubeFrontFace1 = 0;
        private static readonly int SimpleCubeFrontFace2 = 1;
        private static readonly int SimpleCubeRightFace1 = 2;
        private static readonly int SimpleCubeRightFace2 = 3;
        private static readonly int SimpleCubeLeftFace1 = 4;
        private static readonly int SimpleCubeLeftFace2 = 5;
        private static readonly int SimpleCubeBottomFace1 = 6;
        private static readonly int SimpleCubeBottomFace2 = 7;
        private static readonly int SimpleCubeBackFace1 = 8;
        private static readonly int SimpleCubeBackFace2 = 9;
        private static readonly int SimpleCubeTopFace1 = 10;
        private static readonly int SimpleCubeTopFace2 = 11;

        private static readonly int SimpleCubeSingleFloorVertex1 = 1;
        private static readonly int SimpleCubeSingleFloorVertex2 = 3;
        private static readonly int SimpleCubePairedVertex1 = 0;
        private static readonly int SimpleCubePairedVertex2 = 5;

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_FindFloorOfSimpleCube_Expect_TwoFaces(Structure structure) {

            Component simpleCube = structure.components[0];
            Assert.AreEqual(2, ComponentGraphGenerator.GetFloorFaces(simpleCube).Count);
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_FindLowestVertexOfSimpleCube_Expect_YIsZero(Structure structure) {

            Component simpleCube = structure.components[0];
            Assert.That(Util.NearlyEqual(simpleCube.vertices[ComponentGraphGenerator.FindLowestVertex(simpleCube)].coordinate.y, 0));
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_FindFloorFaceOfSimpleCube_Expect_FloorFace(Structure structure) {

            Component simpleCube = structure.components[0];

            int lowestVertex = ComponentGraphGenerator.FindLowestVertex(simpleCube);

            int faceIndex = ComponentGraphGenerator.FindStartFloorFace(simpleCube, lowestVertex);

            Assert.That( faceIndex == SimpleCubeBottomFace1 || faceIndex == SimpleCubeBottomFace2);
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_FindFloorFacesOfSimpleCube_Expect_TwoBottomFaces(Structure structure) {

            Component simpleCube = structure.components[0];

            List<int> floorFaces = ComponentGraphGenerator.GetFloorFaces(simpleCube);

            Assert.AreEqual(2, floorFaces.Count);
            Assert.Contains(6, floorFaces);
            Assert.Contains(7, floorFaces);
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_GetShellVerticesOfSimpleCube_Expect_FloorSideVertices(Structure structure)
        {

            Component simpleCube = structure.components[0];

            List<int> floorFaces = ComponentGraphGenerator.GetFloorFaces(simpleCube);
            LinkedList<int> floorSideVertices = ComponentGraphGenerator.GetShellVertices(simpleCube, floorFaces);

            Assert.AreEqual(4, floorSideVertices.Count);
            Assert.Contains(1, floorSideVertices);
            Assert.Contains(3, floorSideVertices);
            Assert.Contains(0, floorSideVertices);
            Assert.Contains(5, floorSideVertices);
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_SplitLinkedListOfSimpleCube_Expect_LongSideVertices(Structure structure)
        {

            Component simpleCube = structure.components[0];

            List<int> floorFaces = ComponentGraphGenerator.GetFloorFaces(simpleCube);
            LinkedList<int> floorSideVertices = ComponentGraphGenerator.GetShellVertices(simpleCube, floorFaces);
           (LinkedList<int>, LinkedList<int>) sides = ComponentGraphGenerator.SplitLinkedList(simpleCube, floorSideVertices);

            Assert.AreEqual(2, sides.Item1.Count);
            Assert.AreEqual(2, sides.Item2.Count);
            Assert.Contains(0, sides.Item1);
            Assert.Contains(1, sides.Item1);
            Assert.Contains(3, sides.Item2);
            Assert.Contains(5, sides.Item2);
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_CreatingNodegraphFromSimpleCube_Expect_Result(Structure structure)
        {
            Component simpleCube = structure.components[0];
            NodeGraph nodegraph = ComponentGraphGenerator.GenerateComponentNodeGraph(simpleCube);
            NodeGraph expectedNodegraph = CreateNodegraphSimpleCube();
            Assert.AreEqual(expectedNodegraph, nodegraph);
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube2ExtraVertices")]
        public void When_SplitLinkedListOfSimpleCube2ExtraVertices_Expect_LongSideVertices(Structure structure)
        {
            Component simpleCube2ExtraVertices = structure.components[0];

            List<int> floorFaces = ComponentGraphGenerator.GetFloorFaces(simpleCube2ExtraVertices);
            LinkedList<int> floorSideVertices = ComponentGraphGenerator.GetShellVertices(simpleCube2ExtraVertices, floorFaces);
            (LinkedList<int>, LinkedList<int>) sides = ComponentGraphGenerator.SplitLinkedList(simpleCube2ExtraVertices, floorSideVertices);

            Assert.AreEqual(3, sides.Item1.Count);
            Assert.AreEqual(3, sides.Item2.Count);
            Assert.Contains(0, sides.Item1);
            Assert.Contains(1, sides.Item1);
            Assert.Contains(8, sides.Item1);
            Assert.Contains(3, sides.Item2);
            Assert.Contains(5, sides.Item2);
            Assert.Contains(9, sides.Item2);
        }

        [Test]
        public void When_FindLowestVertexEmptyComponent_ExpectThrowEmptyListException() {
            Assert.Throws<EmptyListException>(new TestDelegate(When_FindLowestVertexEmptyComponent_ExpectThrowEmptyListException_Helper));
        }

        public void When_FindLowestVertexEmptyComponent_ExpectThrowEmptyListException_Helper()
        {
            Component c = new Component();
            ComponentGraphGenerator.FindLowestVertex(c);
        }

        [Test]
        public void When_FindStartFloorFaceEmptyComponent_ExpectThrowEmptyListException() {
            Assert.Throws<EmptyListException>(new TestDelegate(When_FindStartFloorFaceEmptyComponent_ExpectThrowEmptyListException_Helper));
        }

        public void When_FindStartFloorFaceEmptyComponent_ExpectThrowEmptyListException_Helper() {
            Component c = new Component();
            ComponentGraphGenerator.FindStartFloorFace(c, 1);
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        public void When_CreatingSimpleCubeGraph_ExpectRightWidth(Structure structure)
        {
            Component simpleCube = structure.components[0];
            NodeGraph simpleCubeGraph = ComponentGraphGenerator.GenerateComponentNodeGraph(simpleCube);
            Assert.AreEqual(1, simpleCubeGraph.GetEdgeWidth(0));
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCubeBig")]
        public void When_CreatingSimpleCubeBigGraph_ExpectRightWidth(Structure structure)
        {
            Component simpleCubeBig = structure.components[0];
            NodeGraph simpleCubeBigGraph = ComponentGraphGenerator.GenerateComponentNodeGraph(simpleCubeBig);
            Assert.AreEqual(7, simpleCubeBigGraph.GetEdgeWidth(0));
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleLeftTurn")]
        public void When_CreatingSimpleLeftTurnGraph_ExpectRightWidths(Structure structure)
        {
            Component SimpleLeftTurn = structure.components[0];
            NodeGraph SimpleLeftTurnGraph = ComponentGraphGenerator.GenerateComponentNodeGraph(SimpleLeftTurn);
            Assert.AreEqual(1, SimpleLeftTurnGraph.GetEdgeWidth(0));
            Assert.AreEqual(Math.Sqrt(0.5), SimpleLeftTurnGraph.GetEdgeWidth(1));
            Assert.AreEqual(1, SimpleLeftTurnGraph.GetEdgeWidth(2));
        }
        /**
        * HELP FUNCTION
        * Used for creating the expected result from GenerateComponentNodeGraph
        * when using SimpleCube
        */
        public NodeGraph CreateNodegraphSimpleCube()
        {
            NodeGraph nodeGraph = new NodeGraph();

            //Calculated middlepoint between 0,3 and 1,5 in SimpleCube
            Node node = new Node(0.5, 0, 0);
            Node secondNode = new Node(0.5, 0, 1);

            nodeGraph.AddNode(node);
            nodeGraph.AddNode(secondNode,0);
            nodeGraph.SetEdgeWidth(0, 1);

            return nodeGraph;
        }

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleRightCorridorWithExcessFloorPoint")]
        public void When_GenerateComponentNodeGraphFromComponentWithUnevenFloorVertices_Expect_NodeGraph(Structure structure) {
            Component component = structure.components[0];

            NodeGraph nodeGraph = ComponentGraphGenerator.GenerateComponentNodeGraph(component);

            Vect3 node1coordinate = new Vect3(-0.5, -0.5, 0);
            Vect3 node2coordinate = new Vect3(7.5, -0.5, 0);

            Edge resultEdge = new Edge(1,0);
            resultEdge.index = 0;
            resultEdge.width = 1;

            Assert.AreEqual(2, nodeGraph.Nodes.Count);
            Assert.AreEqual(1, nodeGraph.Edges.Count);
            Assert.That(nodeGraph.Nodes.Exists(n => n.coordinate == node1coordinate));
            Assert.That(nodeGraph.Nodes.Exists(n => n.coordinate == node2coordinate));
            Assert.Contains(resultEdge, nodeGraph.Edges);
        }
    }
}

public class SimpleGeometryClass
{
    public static IEnumerable SimpleCube
    {
        get
        {
            // Create a 1x1x1 cube (12 faces, 8 vertices) for test purposes.
            yield return new TestCaseData(SetSimpleCube());
        }
    }

    public static IEnumerable SimpleCube2ExtraVertices
    {
        get
        {
            Structure structure = SetSimpleCube();

            var downNormal = new Vect3(0, -1, 0);

            structure.components[0].AddVertex(new Vertex(0, 0, 2));
            structure.components[0].AddVertex(new Vertex(1, 0, 2));

            structure.components[0].CreateFace(downNormal, new int[] { 8, 0, 3 });
            structure.components[0].CreateFace(downNormal, new int[] { 8, 9, 3 });

            yield return new TestCaseData(structure);
        }
    }
    public static IEnumerable SimpleCubeBig
    {
        get
        {
            // Create a 7x7x7 cube (12 faces, 8 vertices) for test purposes.
            Structure structure = new Structure();
            structure.addComponent(new Component());
            structure.components[0].AddVertex(new Vertex(0, 0, 0));
            structure.components[0].AddVertex(new Vertex(0, 0, 7));
            structure.components[0].AddVertex(new Vertex(0, 7, 0));
            structure.components[0].AddVertex(new Vertex(7, 0, 0));
            structure.components[0].AddVertex(new Vertex(0, 7, 7));
            structure.components[0].AddVertex(new Vertex(7, 0, 7));
            structure.components[0].AddVertex(new Vertex(7, 7, 0));
            structure.components[0].AddVertex(new Vertex(7, 7, 7));

            var frontNormal = new Vect3(0, 0, -1);
            var rightNormal = new Vect3(1, 0, 0);
            var leftNormal = new Vect3(-1, 0, 0);
            var downNormal = new Vect3(0, -1, 0);
            var backNormal = new Vect3(0, 0, 1);
            var upNormal = new Vect3(0, 1, 0);

            structure.components[0].CreateFace(frontNormal, new int[] { 0, 3, 6 });
            structure.components[0].CreateFace(frontNormal, new int[] { 0, 2, 6 });
            structure.components[0].CreateFace(rightNormal, new int[] { 3, 5, 7 });
            structure.components[0].CreateFace(rightNormal, new int[] { 3, 6, 7 });
            structure.components[0].CreateFace(leftNormal, new int[] { 1, 0, 2 });
            structure.components[0].CreateFace(leftNormal, new int[] { 1, 4, 2 });
            structure.components[0].CreateFace(downNormal, new int[] { 0, 3, 5 });
            structure.components[0].CreateFace(downNormal, new int[] { 0, 1, 5 });
            structure.components[0].CreateFace(backNormal, new int[] { 5, 1, 4 });
            structure.components[0].CreateFace(backNormal, new int[] { 5, 7, 4 });
            structure.components[0].CreateFace(upNormal, new int[] { 2, 6, 7 });
            structure.components[0].CreateFace(upNormal, new int[] { 2, 4, 7 });

            yield return new TestCaseData(structure);
        }
    }
    public static IEnumerable SimpleLeftTurn
    {
        get
        {
            // Create a "left turn" seen from above for test purposes. Looks something like this:
            //  __
            // |__  \
            //    \  \
            //     |__|
            //

            Structure structure = new Structure();
            structure.addComponent(new Component());
            structure.components[0].AddVertex(new Vertex(2, 0, 0));//0
            structure.components[0].AddVertex(new Vertex(3, 0, 0));//1
            structure.components[0].AddVertex(new Vertex(3, 1, 0));//2
            structure.components[0].AddVertex(new Vertex(2, 1, 0));//3
            structure.components[0].AddVertex(new Vertex(0, 2, 0));//4
            structure.components[0].AddVertex(new Vertex(1, 2, 0));//5
            structure.components[0].AddVertex(new Vertex(1, 3, 0));//6
            structure.components[0].AddVertex(new Vertex(0, 3, 0));//7

            structure.components[0].AddVertex(new Vertex(2, 0, 1));//8
            structure.components[0].AddVertex(new Vertex(3, 0, 1));//9
            structure.components[0].AddVertex(new Vertex(2, 1, 1));//10
            structure.components[0].AddVertex(new Vertex(3, 1, 1));//11
            structure.components[0].AddVertex(new Vertex(0, 2, 1));//12
            structure.components[0].AddVertex(new Vertex(0, 3, 1));//13
            structure.components[0].AddVertex(new Vertex(1, 2, 1));//14
            structure.components[0].AddVertex(new Vertex(1, 3, 1));//15

            var frontNormal = new Vect3(0, 0, -1);
            var rightNormal = new Vect3(1, 0, 0);
            var leftNormal = new Vect3(-1, 0, 0);
            var downNormal = new Vect3(0, -1, 0);
            var backNormal = new Vect3(0, 0, 1);
            var upNormal = new Vect3(0, 1, 0);
            var downleftNormal = new Vect3(-1, -1, 0);
            var uprightNormal = new Vect3(1, 1, 0);

            structure.components[0].CreateFace(downNormal, new int[] { 0, 1, 2 });
            structure.components[0].CreateFace(downNormal, new int[] { 3, 2, 0 });
            structure.components[0].CreateFace(downNormal, new int[] { 3, 2, 5 });
            structure.components[0].CreateFace(downNormal, new int[] { 5, 6, 2 });
            structure.components[0].CreateFace(downNormal, new int[] { 4, 5, 6 });
            structure.components[0].CreateFace(downNormal, new int[] { 4, 6, 7 });

            structure.components[0].CreateFace(upNormal, new int[] { 8, 9, 10 });
            structure.components[0].CreateFace(upNormal, new int[] { 11, 10, 8 });
            structure.components[0].CreateFace(upNormal, new int[] { 11, 10, 13 });
            structure.components[0].CreateFace(upNormal, new int[] { 13, 14, 10 });
            structure.components[0].CreateFace(upNormal, new int[] { 12, 13, 14 });
            structure.components[0].CreateFace(upNormal, new int[] { 12, 14, 15 });

            structure.components[0].CreateFace(leftNormal, new int[] { 0, 3, 11 });
            structure.components[0].CreateFace(leftNormal, new int[] { 8, 11, 0 });
            structure.components[0].CreateFace(leftNormal, new int[] { 4, 7, 15 });
            structure.components[0].CreateFace(leftNormal, new int[] { 15, 12, 4 });

            structure.components[0].CreateFace(rightNormal, new int[] { 1, 2, 10 });
            structure.components[0].CreateFace(rightNormal, new int[] { 9, 10, 7 });

            structure.components[0].CreateFace(backNormal, new int[] { 0, 1, 9 });
            structure.components[0].CreateFace(backNormal, new int[] { 8, 9, 0 });
            structure.components[0].CreateFace(backNormal, new int[] { 4, 5, 13 });
            structure.components[0].CreateFace(backNormal, new int[] { 12, 13, 4 });

            structure.components[0].CreateFace(frontNormal, new int[] { 7, 6, 14 });
            structure.components[0].CreateFace(frontNormal, new int[] { 15, 14, 7 });

            structure.components[0].CreateFace(downleftNormal, new int[] { 3, 5, 13 });
            structure.components[0].CreateFace(downleftNormal, new int[] { 11, 13, 3 });

            structure.components[0].CreateFace(uprightNormal, new int[] { 2, 6, 14 });
            structure.components[0].CreateFace(uprightNormal, new int[] { 10, 14, 2 });

            yield return new TestCaseData(structure);
        }
    }
    public static Structure SetSimpleCube()
    {
        Structure structure = new Structure();
        structure.addComponent(new Component());
        structure.components[0].AddVertex(new Vertex(0, 0, 0));
        structure.components[0].AddVertex(new Vertex(0, 0, 1));
        structure.components[0].AddVertex(new Vertex(0, 1, 0));
        structure.components[0].AddVertex(new Vertex(1, 0, 0));
        structure.components[0].AddVertex(new Vertex(0, 1, 1));
        structure.components[0].AddVertex(new Vertex(1, 0, 1));
        structure.components[0].AddVertex(new Vertex(1, 1, 0));
        structure.components[0].AddVertex(new Vertex(1, 1, 1));

        var frontNormal = new Vect3(0, 0, -1);
        var rightNormal = new Vect3(1, 0, 0);
        var leftNormal = new Vect3(-1, 0, 0);
        var downNormal = new Vect3(0, -1, 0);
        var backNormal = new Vect3(0, 0, 1);
        var upNormal = new Vect3(0, 1, 0);

        structure.components[0].CreateFace(frontNormal, new int[] { 0, 3, 6 });
        structure.components[0].CreateFace(frontNormal, new int[] { 0, 2, 6 });
        structure.components[0].CreateFace(rightNormal, new int[] { 3, 5, 7 });
        structure.components[0].CreateFace(rightNormal, new int[] { 3, 6, 7 });
        structure.components[0].CreateFace(leftNormal, new int[] { 1, 0, 2 });
        structure.components[0].CreateFace(leftNormal, new int[] { 1, 4, 2 });
        structure.components[0].CreateFace(downNormal, new int[] { 0, 3, 5 });
        structure.components[0].CreateFace(downNormal, new int[] { 0, 1, 5 });
        structure.components[0].CreateFace(backNormal, new int[] { 5, 1, 4 });
        structure.components[0].CreateFace(backNormal, new int[] { 5, 7, 4 });
        structure.components[0].CreateFace(upNormal, new int[] { 2, 6, 7 });
        structure.components[0].CreateFace(upNormal, new int[] { 2, 4, 7 });

        return structure;
    }

    public static IEnumerable SimpleRightCorridor
    {
        get
        {
            // Create a 8x1x1 corridor (12 faces, 8 vertices) for test purposes.
            Structure structure = new Structure();
            structure.addComponent(new Component());
            structure.components[0].AddVertex(new Vertex(-0.5,-0.5,-0.5));
            structure.components[0].AddVertex(new Vertex(-0.5,-0.5,0.5));
            structure.components[0].AddVertex(new Vertex(-0.5,0.5,-0.5));
            structure.components[0].AddVertex(new Vertex(7.5,-0.5,-0.5));
            structure.components[0].AddVertex(new Vertex(-0.5,0.5,0.5));
            structure.components[0].AddVertex(new Vertex(7.5,-0.5,0.5));
            structure.components[0].AddVertex(new Vertex(7.5,0.5,-0.5));
            structure.components[0].AddVertex(new Vertex(7.5,0.5,0.5));

            var frontNormal = new Vect3(0,0,-1);
            var rightNormal = new Vect3(1,0,0);
            var leftNormal = new Vect3(-1,0,0);
            var downNormal = new Vect3(0,-1,0);
            var backNormal = new Vect3(0,0,1);
            var upNormal = new Vect3(0,1,0);

            structure.components[0].CreateFace(frontNormal, new int[]{0,3,6});
            structure.components[0].CreateFace(frontNormal, new int[]{0,2,6});
            structure.components[0].CreateFace(rightNormal, new int[]{3,5,7});
            structure.components[0].CreateFace(rightNormal, new int[]{3,6,7});
            structure.components[0].CreateFace(leftNormal, new int[]{1,0,2});
            structure.components[0].CreateFace(leftNormal, new int[]{1,4,2});
            structure.components[0].CreateFace(downNormal, new int[]{0,3,5});
            structure.components[0].CreateFace(downNormal, new int[]{0,1,5});
            structure.components[0].CreateFace(backNormal, new int[]{5,1,4});
            structure.components[0].CreateFace(backNormal, new int[]{5,7,4});
            structure.components[0].CreateFace(upNormal, new int[]{2,6,7});
            structure.components[0].CreateFace(upNormal, new int[]{2,4,7});

            yield return new TestCaseData(structure);
        }
    }

    public static IEnumerable SimpleRightCorridorWithExcessFloorPoint
    {
        get
        {
            // Create a 8x1x1 corridor (14 faces, 9 vertices) for test purposes.
            Structure structure = new Structure();
            structure.addComponent(new Component());
            structure.components[0].AddVertex(new Vertex(-0.5,-0.5,-0.5));
            structure.components[0].AddVertex(new Vertex(-0.5,-0.5,0.5));
            structure.components[0].AddVertex(new Vertex(-0.5,0.5,-0.5));
            structure.components[0].AddVertex(new Vertex(7.5,-0.5,-0.5));
            structure.components[0].AddVertex(new Vertex(-0.5,0.5,0.5));
            structure.components[0].AddVertex(new Vertex(7.5,-0.5,0.5));
            structure.components[0].AddVertex(new Vertex(7.5,0.5,-0.5));
            structure.components[0].AddVertex(new Vertex(7.5,0.5,0.5));
            structure.components[0].AddVertex(new Vertex(3.5,-0.5,-0.5));

            var frontNormal = new Vect3(0,0,-1);
            var rightNormal = new Vect3(1,0,0);
            var leftNormal = new Vect3(-1,0,0);
            var downNormal = new Vect3(0,-1,0);
            var backNormal = new Vect3(0,0,1);
            var upNormal = new Vect3(0,1,0);

            structure.components[0].CreateFace(frontNormal, new int[]{0,2,8});
            structure.components[0].CreateFace(frontNormal, new int[]{2,6,8});
            structure.components[0].CreateFace(frontNormal, new int[]{3,6,8});
            structure.components[0].CreateFace(rightNormal, new int[]{3,5,7});
            structure.components[0].CreateFace(rightNormal, new int[]{3,6,7});
            structure.components[0].CreateFace(leftNormal, new int[]{1,0,2});
            structure.components[0].CreateFace(leftNormal, new int[]{1,4,2});
            structure.components[0].CreateFace(downNormal, new int[]{0,1,8});
            structure.components[0].CreateFace(downNormal, new int[]{1,5,8});
            structure.components[0].CreateFace(downNormal, new int[]{3,5,8});
            structure.components[0].CreateFace(backNormal, new int[]{5,1,4});
            structure.components[0].CreateFace(backNormal, new int[]{5,7,4});
            structure.components[0].CreateFace(upNormal, new int[]{2,6,7});
            structure.components[0].CreateFace(upNormal, new int[]{2,4,7});

            yield return new TestCaseData(structure);
        }
    }

    public static IEnumerable TIntersectWithOneLowerCorridor
    {
        get
        {
            // Create a 8x1x1 corridor (12 faces, 8 vertices) for test purposes.
            Structure structure = new Structure();
            Component simpleRightCorridor = new Component();
            simpleRightCorridor.AddVertex(new Vertex(-0.5,-0.5,-0.5));
            simpleRightCorridor.AddVertex(new Vertex(-0.5,-0.5,0.5));
            simpleRightCorridor.AddVertex(new Vertex(-0.5,0.5,-0.5));
            simpleRightCorridor.AddVertex(new Vertex(7.5,-0.5,-0.5));
            simpleRightCorridor.AddVertex(new Vertex(-0.5,0.5,0.5));
            simpleRightCorridor.AddVertex(new Vertex(7.5,-0.5,0.5));
            simpleRightCorridor.AddVertex(new Vertex(7.5,0.5,-0.5));
            simpleRightCorridor.AddVertex(new Vertex(7.5,0.5,0.5));

            var frontNormal = new Vect3(0,0,-1);
            var rightNormal = new Vect3(1,0,0);
            var leftNormal = new Vect3(-1,0,0);
            var downNormal = new Vect3(0,-1,0);
            var backNormal = new Vect3(0,0,1);
            var upNormal = new Vect3(0,1,0);

            simpleRightCorridor.CreateFace(frontNormal, new int[]{0,3,6});
            simpleRightCorridor.CreateFace(frontNormal, new int[]{0,2,6});
            simpleRightCorridor.CreateFace(rightNormal, new int[]{3,5,7});
            simpleRightCorridor.CreateFace(rightNormal, new int[]{3,6,7});
            simpleRightCorridor.CreateFace(leftNormal, new int[]{1,0,2});
            simpleRightCorridor.CreateFace(leftNormal, new int[]{1,4,2});
            simpleRightCorridor.CreateFace(downNormal, new int[]{0,3,5});
            simpleRightCorridor.CreateFace(downNormal, new int[]{0,1,5});
            simpleRightCorridor.CreateFace(backNormal, new int[]{5,1,4});
            simpleRightCorridor.CreateFace(backNormal, new int[]{5,7,4});
            simpleRightCorridor.CreateFace(upNormal, new int[]{2,6,7});
            simpleRightCorridor.CreateFace(upNormal, new int[]{2,4,7});

            Component offsetSimpleRightCorridor = simpleRightCorridor.Transform(Vect3.Transforms.Translation(-7.5,0,0));

            Component simpleLeftCorridor = offsetSimpleRightCorridor.Transform(Vect3.Transforms.Translation(8,0,0));
            Component simpleBackCorridor = simpleRightCorridor.Transform(Vect3.Transforms.Rotation(0,-1*Math.PI/2d,0));
            simpleBackCorridor = simpleBackCorridor.Transform(Vect3.Transforms.Translation(0,-0.5,-7.5));

            structure.addComponent(offsetSimpleRightCorridor);
            structure.addComponent(simpleLeftCorridor);
            structure.addComponent(simpleBackCorridor);

            yield return new TestCaseData(structure);
        }
    }
    public static IEnumerable ThreeHeightVariantRightCorridors
    {
        get
        {
            // Create a 8x1x1 corridor (12 faces, 8 vertices) for test purposes.
            Structure structure = new Structure();
            structure.addComponent(new Component());
            structure.components[0].AddVertex(new Vertex(0,0,0)); // left bottom forward
            structure.components[0].AddVertex(new Vertex(0,0,1)); // left bottom back
            structure.components[0].AddVertex(new Vertex(0,2,0)); // left top forward
            structure.components[0].AddVertex(new Vertex(8,0,0)); // right bottom forward
            structure.components[0].AddVertex(new Vertex(0,2,1)); // left top back
            structure.components[0].AddVertex(new Vertex(8,0,1)); // right bottom back
            structure.components[0].AddVertex(new Vertex(8,4,0)); // right top forward
            structure.components[0].AddVertex(new Vertex(8,4,1)); // right top back

            var frontNormal = new Vect3(0,0,-1);
            var rightNormal = new Vect3(1,0,0);
            var leftNormal = new Vect3(-1,0,0);
            var downNormal = new Vect3(0,-1,0);
            var backNormal = new Vect3(0,0,1);
            var upNormal = new Vect3(0,1,0);

            structure.components[0].CreateFace(frontNormal, new int[]{0,3,6});
            structure.components[0].CreateFace(frontNormal, new int[]{0,2,6});
            structure.components[0].CreateFace(rightNormal, new int[]{3,5,7});
            structure.components[0].CreateFace(rightNormal, new int[]{3,6,7});
            structure.components[0].CreateFace(leftNormal, new int[]{1,0,2});
            structure.components[0].CreateFace(leftNormal, new int[]{1,4,2});
            structure.components[0].CreateFace(downNormal, new int[]{0,3,5});
            structure.components[0].CreateFace(downNormal, new int[]{0,1,5});
            structure.components[0].CreateFace(backNormal, new int[]{5,1,4});
            structure.components[0].CreateFace(backNormal, new int[]{5,7,4});
            structure.components[0].CreateFace(upNormal, new int[]{2,6,7});
            structure.components[0].CreateFace(upNormal, new int[]{2,4,7});

            structure.addComponent(new Component());
            structure.components[1].AddVertex(new Vertex(8,0,0)); // left bottom forward
            structure.components[1].AddVertex(new Vertex(8,0,1)); // left bottom back
            structure.components[1].AddVertex(new Vertex(8,4,0)); // left top forward
            structure.components[1].AddVertex(new Vertex(16,0,0)); // right bottom forward
            structure.components[1].AddVertex(new Vertex(8,4,1)); // left top back
            structure.components[1].AddVertex(new Vertex(16,0,1)); // right bottom back
            structure.components[1].AddVertex(new Vertex(16,4,0)); // right top forward
            structure.components[1].AddVertex(new Vertex(16,4,1)); // right top back

            structure.components[1].CreateFace(frontNormal, new int[]{0,3,6});
            structure.components[1].CreateFace(frontNormal, new int[]{0,2,6});
            structure.components[1].CreateFace(rightNormal, new int[]{3,5,7});
            structure.components[1].CreateFace(rightNormal, new int[]{3,6,7});
            structure.components[1].CreateFace(leftNormal, new int[]{1,0,2});
            structure.components[1].CreateFace(leftNormal, new int[]{1,4,2});
            structure.components[1].CreateFace(downNormal, new int[]{0,3,5});
            structure.components[1].CreateFace(downNormal, new int[]{0,1,5});
            structure.components[1].CreateFace(backNormal, new int[]{5,1,4});
            structure.components[1].CreateFace(backNormal, new int[]{5,7,4});
            structure.components[1].CreateFace(upNormal, new int[]{2,6,7});
            structure.components[1].CreateFace(upNormal, new int[]{2,4,7});

            structure.addComponent(new Component());
            structure.components[2].AddVertex(new Vertex(16,0,0)); // left bottom forward
            structure.components[2].AddVertex(new Vertex(16,0,1)); // left bottom back
            structure.components[2].AddVertex(new Vertex(16,4,0)); // left top forward
            structure.components[2].AddVertex(new Vertex(24,0,0)); // right bottom forward
            structure.components[2].AddVertex(new Vertex(16,4,1)); // left top back
            structure.components[2].AddVertex(new Vertex(24,0,1)); // right bottom back
            structure.components[2].AddVertex(new Vertex(24,2,0)); // right top forward
            structure.components[2].AddVertex(new Vertex(24,2,1)); // right top back

            structure.components[2].CreateFace(frontNormal, new int[]{0,3,6});
            structure.components[2].CreateFace(frontNormal, new int[]{0,2,6});
            structure.components[2].CreateFace(rightNormal, new int[]{3,5,7});
            structure.components[2].CreateFace(rightNormal, new int[]{3,6,7});
            structure.components[2].CreateFace(leftNormal, new int[]{1,0,2});
            structure.components[2].CreateFace(leftNormal, new int[]{1,4,2});
            structure.components[2].CreateFace(downNormal, new int[]{0,3,5});
            structure.components[2].CreateFace(downNormal, new int[]{0,1,5});
            structure.components[2].CreateFace(backNormal, new int[]{5,1,4});
            structure.components[2].CreateFace(backNormal, new int[]{5,7,4});
            structure.components[2].CreateFace(upNormal, new int[]{2,6,7});
            structure.components[2].CreateFace(upNormal, new int[]{2,4,7});

            yield return new TestCaseData(structure);
        }
    }
}