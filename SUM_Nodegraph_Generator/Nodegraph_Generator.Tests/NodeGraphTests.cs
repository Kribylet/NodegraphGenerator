using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using NUnit.Framework;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    public class NodeGraphTests
    {
        [Test]
        public void When_CreateEmptyNodeGraph_Expect_Empty()
        {
            NodeGraph nodeGraph = new NodeGraph();

            Assert.IsEmpty(nodeGraph.Nodes);
            Assert.IsEmpty(nodeGraph.Edges);
        }

        [Test]
        public void When_AddNode_Expect_NodeAdded()
        {
            NodeGraph nodeGraph = new NodeGraph();

            Node node = new Node(1,1,1);

            nodeGraph.AddNode(node);
            
            Assert.AreEqual(1, nodeGraph.Nodes.Count);
            Assert.AreEqual(node.coordinate, nodeGraph.Nodes[0].coordinate);
            Assert.AreNotEqual(node.index, nodeGraph.Nodes[0].index);
        }

        [Test]
        public void When_AddNodeWithNeighbor_Expect_EdgeAdded()
        {
            NodeGraph nodeGraph = new NodeGraph();

            Node node = new Node(1,1,1);
            Node secondNode = new Node(2,2,2);

            nodeGraph.AddNode(node);
            nodeGraph.AddNode(secondNode, node);

            Assert.AreEqual(1, nodeGraph.Edges.Count);
            Assert.AreEqual(0, nodeGraph.Edges[0].index);
            Assert.That(nodeGraph.AreLinked(node, secondNode));
        }

        [Test]
        public void When_AddNodeWithNeighborByCoordinate_Expect_EdgeAdded()
        {
            NodeGraph nodeGraph = new NodeGraph();

            Node node = new Node(1,1,1);
            Node secondNode = new Node(2,2,2);

            nodeGraph.AddNode(node);
            nodeGraph.AddNode(secondNode, new Vect3(1,1,1));

            Assert.AreEqual(1, nodeGraph.Edges.Count);
            Assert.AreEqual(0, nodeGraph.Edges[0].index);
            Assert.That(nodeGraph.AreLinked(node, secondNode));
        }

        [Test]
        public void When_AddNodeWithNeighborByIndex_Expect_EdgeAdded()
        {
            NodeGraph nodeGraph = new NodeGraph();

            Node node = new Node(1,1,1);
            Node secondNode = new Node(2,2,2);

            nodeGraph.AddNode(node);
            nodeGraph.AddNode(secondNode, 0);

            Assert.AreEqual(1, nodeGraph.Edges.Count);
            Assert.AreEqual(0, nodeGraph.Edges[0].index);
            Assert.That(nodeGraph.AreLinked(node, secondNode));
        }

        [Test]
        public void When_AddNodeWithSameLocation_Expect_NoChange()
        {
            NodeGraph nodeGraph = new NodeGraph();

            Node firstNode = new Node(1,1,1);
            Node secondNode = new Node(2,2,2);
            Node thirdNode = new Node(3,3,3);

            nodeGraph.AddNode(firstNode);
            nodeGraph.AddNode(secondNode);
            nodeGraph.AddNode(thirdNode);

            Node testNode = new Node(1,1,1);

            nodeGraph.AddNode(testNode);
            nodeGraph.AddNode(testNode, secondNode);
            nodeGraph.AddNode(testNode, secondNode, thirdNode);
            nodeGraph.AddNode(testNode, testNode);

            Assert.AreEqual(3, nodeGraph.Nodes.Count);

        }

        [TestCaseSource(typeof(TestNodeGraphClass), "UnlinkedNodeGraph")]
        public void When_AddNodeWithMultipleNeighbors_Expect_AllLinked(NodeGraph nodeGraph) {

            Node newNode = new Node(5,5,5);

            Node neighbor1 = nodeGraph.Nodes[0];
            Node neighbor2 = nodeGraph.Nodes[1];

            nodeGraph.AddNode(newNode, neighbor1, neighbor2);

            Assert.That(nodeGraph.AreLinked(newNode, neighbor1));
            Assert.That(nodeGraph.AreLinked(newNode, neighbor2));
        }

        [Test]
        public void When_AddNodeWithNeighborAndWidth_Expect_NodeAddedWithWidth(){
            NodeGraph nodeGraph = new NodeGraph();
            Node node1 = new Node(new Vect3(1,1,1));
            Node node2 = new Node(new Vect3(2,2,2));
            Node node3 = new Node(new Vect3(3,3,3));
            
            nodeGraph.AddNode(node1);
            nodeGraph.AddNode(node2, (node1, 5.5));
            nodeGraph.AddNode(node3, (node1, 5.5), (node2, 10));
            
            Assert.That(nodeGraph.AreLinked(node1, node2));
            Assert.AreEqual(5.5, nodeGraph.GetEdgeWidth(node1, node2));
            Assert.That(nodeGraph.AreLinked(node3, node2));
            Assert.That(nodeGraph.AreLinked(node3, node1));
            Assert.AreEqual(5.5, nodeGraph.GetEdgeWidth(node1, node3));
            Assert.AreEqual(10, nodeGraph.GetEdgeWidth(node2, node3));
        }

        [Test]
        public void When_AddNodeWithNeighborAndWidthByCoordinate_Expect_NodeAddedWithWidth(){
            NodeGraph nodeGraph = new NodeGraph();
            Node node1 = new Node(new Vect3(1,1,1));
            Node node2 = new Node(new Vect3(2,2,2));
            Node node3 = new Node(new Vect3(3,3,3));
            
            nodeGraph.AddNode(node1);
            nodeGraph.AddNode(node2, (new Vect3(1,1,1), 5.5));
            nodeGraph.AddNode(node3, (new Vect3(1,1,1), 5.5), (new Vect3(2,2,2), 10));
            
            Assert.That(nodeGraph.AreLinked(node1, node2));
            Assert.AreEqual(5.5, nodeGraph.GetEdgeWidth(node1, node2));
            Assert.That(nodeGraph.AreLinked(node3, node2));
            Assert.That(nodeGraph.AreLinked(node3, node1));
            Assert.AreEqual(5.5, nodeGraph.GetEdgeWidth(node1, node3));
            Assert.AreEqual(10, nodeGraph.GetEdgeWidth(node2, node3));
        }

        [Test]
        public void When_AddNodeWithNeighborAndWidthByIndex_Expect_NodeAddedWithWidth(){
            NodeGraph nodeGraph = new NodeGraph();
            Node node1 = new Node(new Vect3(1,1,1));
            Node node2 = new Node(new Vect3(2,2,2));
            Node node3 = new Node(new Vect3(3,3,3));
            
            nodeGraph.AddNode(node1);
            nodeGraph.AddNode(node2, (0, 5.5));
            nodeGraph.AddNode(node3, (0, 5.5), (1, 10));
            
            Assert.That(nodeGraph.AreLinked(node1, node2));
            Assert.AreEqual(5.5, nodeGraph.GetEdgeWidth(node1, node2));
            Assert.That(nodeGraph.AreLinked(node3, node2));
            Assert.That(nodeGraph.AreLinked(node3, node1));
            Assert.AreEqual(5.5, nodeGraph.GetEdgeWidth(node1, node3));
            Assert.AreEqual(10, nodeGraph.GetEdgeWidth(node2, node3));
        }

        [Test]
        public void When_AddNodeWithNonExistentNeighbor_Expect_NullNodeException() {
            Assert.Throws<NullNodeException>(new TestDelegate(AddNodeWithNonExistentNeighborByBadNode));
            Assert.Throws<NullNodeException>(new TestDelegate(AddNodeWithNonExistentNeighborByBadCoordinate));
            Assert.Throws<NullNodeException>(new TestDelegate(AddNodeWithNonExistentNeighborByBadIndex));
        }

        private void AddNodeWithNonExistentNeighborByBadNode() {
            // Build CoreLinkedNodeGraph for helper function
            NodeGraph nodeGraph = new NodeGraph();
            Node node = new Node(1,1,1);
            Node secondNode = new Node(2,2,2);
            Node thirdNode = new Node(3,3,3);
            Node fourthNode = new Node(4,4,4);
            Node fifthNode = new Node(5,5,5);
            nodeGraph.AddNode(node);
            nodeGraph.AddNode(secondNode, node);
            nodeGraph.AddNode(thirdNode, node);
            nodeGraph.AddNode(fourthNode, node);
            nodeGraph.AddNode(fifthNode, node);

            Node newNode = new Node(6,6,6);
            Node nonNodeGraphNode = new Node(7,7,7);
            nodeGraph.AddNode(newNode, nonNodeGraphNode);
        }

        private void AddNodeWithNonExistentNeighborByBadCoordinate() {
            // Build CoreLinkedNodeGraph for helper function
            NodeGraph nodeGraph = new NodeGraph();
            Node node = new Node(1,1,1);
            Node secondNode = new Node(2,2,2);
            Node thirdNode = new Node(3,3,3);
            Node fourthNode = new Node(4,4,4);
            Node fifthNode = new Node(5,5,5);
            nodeGraph.AddNode(node);
            nodeGraph.AddNode(secondNode, node);
            nodeGraph.AddNode(thirdNode, node);
            nodeGraph.AddNode(fourthNode, node);
            nodeGraph.AddNode(fifthNode, node);

            Node newNode = new Node(6,6,6);
            nodeGraph.AddNode(newNode, new Vect3(7,7,7));
        }

        private void AddNodeWithNonExistentNeighborByBadIndex() {
            // Build CoreLinkedNodeGraph for helper function
            NodeGraph nodeGraph = new NodeGraph();
            Node node = new Node(1,1,1);
            Node secondNode = new Node(2,2,2);
            Node thirdNode = new Node(3,3,3);
            Node fourthNode = new Node(4,4,4);
            Node fifthNode = new Node(5,5,5);
            nodeGraph.AddNode(node);
            nodeGraph.AddNode(secondNode, node);
            nodeGraph.AddNode(thirdNode, node);
            nodeGraph.AddNode(fourthNode, node);
            nodeGraph.AddNode(fifthNode, node);

            Node newNode = new Node(6,6,6);
            nodeGraph.AddNode(newNode, 12515);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_RemoveNode_Expect_NodeRemoved(NodeGraph nodeGraph) {
            Node nodeToDelete = nodeGraph.Nodes[0];

            nodeGraph.RemoveNode(nodeToDelete);

            Assert.That(!nodeGraph.Nodes.Contains(nodeToDelete));

            foreach (Node node in nodeGraph.Nodes) {
                foreach (NodeEdgePair pair in node.neighbors)
                {
                    Assert.That(pair.nodeIndex != nodeToDelete.index);
                }
            }
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_DeepCopyNodeGraph_Expect_ExactCopy(NodeGraph nodeGraph) {
            NodeGraph nodeGraphCopy = nodeGraph.DeepCopy();

            Assert.AreEqual(nodeGraph, nodeGraphCopy);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_ModifyRetrievedNodeExternally_Expect_UnchangedInternally(NodeGraph nodeGraph) {
            Node myNode1 = nodeGraph.GetNode(0).DeepCopy();
            Node myNode2 = nodeGraph.GetNode(new Vect3(1,1,1)).DeepCopy();

            myNode1.coordinate = new Vect3(999,999,999);
            myNode2.coordinate = new Vect3(777,777,777);

            Assert.AreNotEqual(myNode1, myNode2);

            Assert.AreNotEqual(myNode1, nodeGraph.GetNode(0));
            Assert.AreNotEqual(myNode2, nodeGraph.GetNode(myNode2.index));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_ModifyRetrievedEdgeExternally_Expect_UnchangedInternally(NodeGraph nodeGraph) {
            Edge myEdge1 = nodeGraph.GetEdge(0, 1).DeepCopy();
            Edge myEdge2 = nodeGraph.GetEdge(new Vect3(1,1,1), new Vect3(2,2,2)).DeepCopy();
            Edge myEdge3 = nodeGraph.GetEdge(nodeGraph.Nodes[0], nodeGraph.Nodes[1]).DeepCopy();

            myEdge1.nodeIndex1 = 999;
            myEdge2.nodeIndex1 = 777;
            myEdge2.nodeIndex1 = 555;

            Assert.AreNotEqual(myEdge1, myEdge2);
            Assert.AreNotEqual(myEdge1, myEdge3);
            Assert.AreNotEqual(myEdge2, myEdge3);

            Assert.AreNotEqual(myEdge1, nodeGraph.GetEdge(0, 1));
            Assert.AreNotEqual(myEdge2, nodeGraph.GetEdge(new Vect3(1,1,1), new Vect3(2,2,2)));
            Assert.AreNotEqual(myEdge2, nodeGraph.GetEdge(nodeGraph.Nodes[0], nodeGraph.Nodes[1]));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_GetNodeCopyByIndex_ExpectCopy(NodeGraph nodeGraph) {
            Node node = nodeGraph.GetNode(0).DeepCopy();

            node.coordinate = new Vect3(123,123,123);

            Assert.AreNotEqual(node, nodeGraph.GetNode(0));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_GetNodeCopyByCoordinate_ExpectCopy(NodeGraph nodeGraph) {
            Node node = nodeGraph.GetNode(new Vect3(1,1,1)).DeepCopy();

            node.index = 123154;

            Assert.AreNotEqual(node, nodeGraph.GetNode(new Vect3(1,1,1)));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_RemoveNodeByCoordinate_Expect_NodeRemoved(NodeGraph nodeGraph) {
            Node nodeToDelete = nodeGraph.Nodes[0];

            nodeGraph.RemoveNode(nodeToDelete.coordinate);

            Assert.That(!nodeGraph.Nodes.Contains(nodeToDelete));

            foreach (Node node in nodeGraph.Nodes) {
                foreach (NodeEdgePair pair in node.neighbors)
                {
                    Assert.That(pair.nodeIndex != nodeToDelete.index);
                }
            }
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_RemoveNodeByIndex_Expect_NodeRemoved(NodeGraph nodeGraph) {
            Node nodeToDelete = nodeGraph.Nodes[0];

            nodeGraph.RemoveNode(nodeToDelete.index);

            Assert.That(!nodeGraph.Nodes.Contains(nodeToDelete));

            foreach (Node node in nodeGraph.Nodes) {
                foreach (NodeEdgePair pair in node.neighbors)
                {
                    Assert.That(pair.nodeIndex != nodeToDelete.index);
                }
            }
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_RemoveAllNode_Expect_EmptyNodeGraph(NodeGraph nodeGraph) {
            List<Node> nodesToDelete = new List<Node>();

            nodesToDelete.AddRange(nodeGraph.Nodes);
            foreach (Node nodeToDelete in nodesToDelete)
            {
                nodeGraph.RemoveNode(nodeToDelete);
            }

            Assert.IsEmpty(nodeGraph.Nodes);
            Assert.IsEmpty(nodeGraph.Edges);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_ClearNodeGraph_Expect_DefaultState(NodeGraph nodeGraph) {
            List<Node> nodesToDelete = new List<Node>();
            nodesToDelete.AddRange(nodeGraph.Nodes);

            foreach (Node nodeToDelete in nodesToDelete)
            {
                nodeGraph.RemoveNode(nodeToDelete);
            }

            nodeGraph.ReIndexNodeGraph();

            Assert.AreEqual(nodeGraph, new NodeGraph());
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_CreateNewEdge_Expect_EdgeAdded(NodeGraph nodeGraph) {
            Node edgeStartNode = nodeGraph.Nodes[1];
            Node edgeEndNode = nodeGraph.Nodes[2];
            
            nodeGraph.LinkNodes(edgeStartNode, edgeEndNode, 15);

            Assert.That(nodeGraph.Edges[4].nodeIndex1 == edgeStartNode.index);
            Assert.That(nodeGraph.Edges[4].nodeIndex2 == edgeEndNode.index);
            Assert.AreEqual(15, nodeGraph.Edges[4].width);
            Assert.AreEqual(15, nodeGraph.GetEdgeWidth(edgeStartNode, edgeEndNode));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_CreateNewEdgeByCoordinate_Expect_EdgeAdded(NodeGraph nodeGraph) {
            Node edgeStartNode = nodeGraph.Nodes[1];
            Node edgeEndNode = nodeGraph.Nodes[2];
            
            nodeGraph.LinkNodes(edgeStartNode.coordinate, edgeEndNode.coordinate, 8);

            Assert.That(nodeGraph.Edges[4].nodeIndex1 == edgeStartNode.index);
            Assert.That(nodeGraph.Edges[4].nodeIndex2 == edgeEndNode.index);
            Assert.AreEqual(8, nodeGraph.Edges[4].width);
            Assert.AreEqual(8, nodeGraph.GetEdgeWidth(edgeStartNode.coordinate, edgeEndNode.coordinate));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_CreateNewEdgeByIndex_Expect_EdgeAdded(NodeGraph nodeGraph) {
            Node edgeStartNode = nodeGraph.Nodes[1];
            Node edgeEndNode = nodeGraph.Nodes[2];
            
            nodeGraph.LinkNodes(edgeStartNode.index, edgeEndNode.index, 5);

            Assert.That(nodeGraph.AreLinked(edgeStartNode, edgeEndNode));
            Assert.AreEqual(5, nodeGraph.Edges[4].width);
            Assert.AreEqual(5, nodeGraph.GetEdgeWidth(edgeStartNode, edgeEndNode));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_SetEdgeWidth_Expect_WidthChanged(NodeGraph nodeGraph) {
            Node node1 = nodeGraph.Nodes[0];
            Node node2 = nodeGraph.Nodes[1];
            Node node3 = nodeGraph.Nodes[2];
            Node node4 = nodeGraph.Nodes[3];

            nodeGraph.SetEdgeWidth(nodeGraph.GetEdge(node1, node2).index, 5);
            nodeGraph.SetEdgeWidth(node1.index, node3.index, 2);
            nodeGraph.SetEdgeWidth(node1, node4, 3);

            Assert.AreEqual(5, nodeGraph.GetEdge(node1, node2).width);
            Assert.AreEqual(2, nodeGraph.GetEdge(node1, node3).width);
            Assert.AreEqual(3, nodeGraph.GetEdge(node1, node4).width);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_RemoveNodeWithAllEdges_Expect_AllEdgesRemoved(NodeGraph nodeGraph) {

            nodeGraph.RemoveNode(0);

            Assert.AreEqual(0, nodeGraph.Edges.Count);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_CheckExistingEdges_Expect_Found(NodeGraph nodeGraph) {
            for (int i = 1; i < nodeGraph.Nodes.Count; i++)
            {
                Assert.That(nodeGraph.AreLinked(0, i),  "Count: " + nodeGraph.Nodes.Count);
                Assert.That(nodeGraph.AreLinked(i, 0),  "Count: " + nodeGraph.Nodes.Count);
            }
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_CheckNonExistingEdges_Expect_NotFound(NodeGraph nodeGraph) {

            for (int i = 1; i < nodeGraph.Nodes.Count; i++)
            {
                for (int j = 1; j < nodeGraph.Nodes.Count; j++)
                {
                    // Assume a node cannot be linked to itself.
                    Assert.That(!nodeGraph.AreLinked(i,j));
                }
            }
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_RemoveEdge_Expect_EdgeRemoved(NodeGraph nodeGraph) {

            Edge edgeToRemove = nodeGraph.Edges[0];
            nodeGraph.RemoveEdge(edgeToRemove);

            Assert.That(!nodeGraph.Edges.Contains(edgeToRemove));
            Assert.That(!nodeGraph.AreLinked(nodeGraph.Nodes[0], nodeGraph.Nodes[1]));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_RemoveEdgeByNodes_Expect_EdgeRemoved(NodeGraph nodeGraph) {

            Node edgeStart = nodeGraph.Nodes[0];
            Node edgeEnd = nodeGraph.Nodes[1];

            nodeGraph.RemoveEdge(edgeStart, edgeEnd);

            Assert.That(!nodeGraph.AreLinked(edgeStart, edgeEnd));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_RemoveEdgeByIndex_Expect_EdgeRemoved(NodeGraph nodeGraph) {

            Node edgeStart = nodeGraph.Nodes[0];
            Node edgeEnd = nodeGraph.Nodes[1];

            nodeGraph.RemoveEdge(nodeGraph.GetEdge(edgeStart, edgeEnd).index);

            Assert.That(!nodeGraph.AreLinked(edgeStart, edgeEnd));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_RemoveEdgeByCoordinates_Expect_EdgeRemoved(NodeGraph nodeGraph) {

            Node edgeStart = nodeGraph.Nodes[0];
            Node edgeEnd = nodeGraph.Nodes[1];

            Edge edge = nodeGraph.GetEdge(edgeStart.coordinate, edgeEnd.coordinate);
            nodeGraph.RemoveEdge(edgeStart.coordinate, edgeEnd.coordinate);

            Assert.That(!nodeGraph.AreLinked(edgeStart, edgeEnd));
            Assert.That(!nodeGraph.Edges.Exists(x => x.index == edge.index));

            foreach (NodeEdgePair neighbor in edgeStart.neighbors)
            {
                Assert.AreNotEqual(edgeStart.index, neighbor.nodeIndex);
            }

            foreach (NodeEdgePair neighbor in edgeEnd.neighbors)
            {
                Assert.AreNotEqual(edgeEnd.index, neighbor.nodeIndex);
            }
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_RemoveEdgeByIndices_Expect_EdgeRemoved(NodeGraph nodeGraph) {

            Node edgeStart = nodeGraph.Nodes[0];
            Node edgeEnd = nodeGraph.Nodes[1];

            nodeGraph.RemoveEdge(edgeStart.index, edgeEnd.index);

            Assert.That(!nodeGraph.AreLinked(edgeStart, edgeEnd));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_RemoveEdge_Expect_OrderUnimportant(NodeGraph nodeGraph) {

            NodeGraph nodeGraphCopy = nodeGraph.DeepCopy();
            Node edgeStart = nodeGraph.Nodes[0];
            Node edgeEnd = nodeGraph.Nodes[1];

            nodeGraph.RemoveEdge(edgeStart, edgeEnd);
            nodeGraphCopy.RemoveEdge(edgeEnd, edgeStart);

            Assert.That(!nodeGraph.AreLinked(edgeStart, edgeEnd));
            Assert.That(!nodeGraphCopy.AreLinked(edgeStart, edgeEnd));

            Assert.AreEqual(nodeGraph, nodeGraphCopy);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_MoveNode_Expect_NodeMoved(NodeGraph nodeGraph) {

            NodeGraph nodeGraphCopy = nodeGraph.DeepCopy();

            Node node2 = nodeGraph.GetNode(new Vect3(2,2,2));
            Node node3 = nodeGraph.GetNode(new Vect3(3,3,3));
            Node node4 = nodeGraph.GetNode(new Vect3(4,4,4));
            Node node5 = nodeGraph.GetNode(new Vect3(5,5,5));

            nodeGraph.MoveNode(new Vect3(1,1,1), new Vect3(70,70,70));

            Node node1 = nodeGraph.GetNode(new Vect3(70,70,70));

            Assert.That(nodeGraph.AreLinked(node1, node2));
            Assert.That(nodeGraph.AreLinked(node1, node3));
            Assert.That(nodeGraph.AreLinked(node1, node4));
            Assert.That(nodeGraph.AreLinked(node1, node5));

            Assert.AreEqual(5, nodeGraph.Nodes.Count);

            Assert.AreEqual(nodeGraph.GetNode(new Vect3(1,1,1)), null);

            Assert.AreNotEqual(nodeGraph, nodeGraphCopy);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_MergeNode_Expect_NodeMerged(NodeGraph nodeGraph) {

            NodeGraph nodeGraphCopy = nodeGraph.DeepCopy();

            Node node1 = nodeGraph.GetNode(new Vect3(1,1,1));
            Node node2 = nodeGraph.GetNode(new Vect3(2,2,2));
            Node node3 = nodeGraph.GetNode(new Vect3(3,3,3));
            Node node4 = nodeGraph.GetNode(new Vect3(4,4,4));
            Node node5 = nodeGraph.GetNode(new Vect3(5,5,5));

            nodeGraph.LinkNodes(node2, node3);

            node1 = nodeGraph.MoveNode(node1, node2.coordinate);

            Assert.That(nodeGraph.AreLinked(node1, node3));
            Assert.That(nodeGraph.AreLinked(node1, node4));
            Assert.That(nodeGraph.AreLinked(node1, node5));

            Assert.AreEqual(4, nodeGraph.Nodes.Count);
            Assert.AreEqual(3, nodeGraph.Edges.Count);

            foreach (Node node in nodeGraph.Nodes)
            {
                foreach (NodeEdgePair neighbor in node.neighbors)
                {
                    Assert.AreNotEqual(node2.index, neighbor.nodeIndex);
                }
            }

            foreach (Edge edge in nodeGraph.Edges)
            {
                Assert.AreNotEqual(node2.index, edge.nodeIndex1);
                Assert.AreNotEqual(node2.index, edge.nodeIndex2);
            }

            Assert.AreEqual(nodeGraph.GetNode(new Vect3(1,1,1)), null);

            Assert.AreNotEqual(nodeGraph, nodeGraphCopy);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_MergeNode_Expect_MinDistanceKept(NodeGraph nodeGraph) {

            NodeGraph nodeGraphCopy = nodeGraph.DeepCopy();

            Node node1 = nodeGraph.GetNode(new Vect3(1,1,1));
            Node node2 = nodeGraph.GetNode(new Vect3(2,2,2));
            Node node3 = nodeGraph.GetNode(new Vect3(3,3,3));
            Node node4 = nodeGraph.GetNode(new Vect3(4,4,4));
            Node node5 = nodeGraph.GetNode(new Vect3(5,5,5));

            nodeGraph.LinkNodes(node1, node4);
            nodeGraph.SetEdgeWidth(node1, node2, 5);
            nodeGraph.SetEdgeWidth(node2, node3, 10);

            node1 = nodeGraph.MoveNode(node2, node1.coordinate);

            Assert.That(nodeGraph.AreLinked(new Vect3(1,1,1), new Vect3(3,3,3)));
            Assert.AreEqual(5d, nodeGraph.GetEdgeWidth(new Vect3(1,1,1), new Vect3(3,3,3)));

            Assert.AreNotEqual(nodeGraph, nodeGraphCopy);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_ReIndexNodeGraph_Expect_AscendingZeroIndexing(NodeGraph nodeGraph) {

            nodeGraph.RemoveNode(0);

            nodeGraph.ReIndexNodeGraph();

            for (int i = 0; i < nodeGraph.Nodes.Count; i++)    
            {
                Assert.AreEqual(i, nodeGraph.Nodes[i].index);
            }

            for (int i = 0; i < nodeGraph.Edges.Count; i++)    
            {
                Assert.AreEqual(i, nodeGraph.Edges[i].index);
            }
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_ReIndexNodeGraphWithOffset_Expect_AscendingOffsetIndexing(NodeGraph nodeGraph) {

            int nodeOffset = 37;
            int edgeOffset = 39;

            nodeGraph.RemoveNode(0);

            nodeGraph.ReIndexNodeGraph(nodeOffset, edgeOffset);

            for (int i = 0; i < nodeGraph.Nodes.Count; i++)    
            {
                Assert.AreEqual(i + nodeOffset, nodeGraph.Nodes[i].index);
            }

            for (int i = 0; i < nodeGraph.Edges.Count; i++)    
            {
                Assert.AreEqual(i + edgeOffset, nodeGraph.Edges[i].index);
            }
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_NodeGraphChangedExternally_Expect_UnchangedInternally(NodeGraph nodeGraph) {

            NodeGraph nodeGraphCopy = nodeGraph.DeepCopy();

            List<Node> myNodes = nodeGraph.NodesDeepCopy;
            List<Edge> myEdges = nodeGraph.EdgesDeepCopy;

            myNodes.RemoveAt(0);
            myEdges.RemoveAt(0);
            myNodes[0].index = 77;
            myNodes[0].coordinate = new Vect3(6,3,2);
            myNodes[0].neighbors.Add(new NodeEdgePair(1561, 1611));

            myEdges[0].index = 156125;
            myEdges[0].nodeIndex1 = 156125;
            myEdges[0].nodeIndex2 = 156125;


            Assert.AreEqual(nodeGraph, nodeGraphCopy);
        }
    }

    public class TestNodeGraphClass
    {
        public static IEnumerable SimpleNodeGraph
        {
            get
            {
                NodeGraph nodeGraph = new NodeGraph();

                Node node = new Node(1,1,1);
                Node secondNode = new Node(2,2,2);

                nodeGraph.AddNode(node);
                nodeGraph.AddNode(secondNode, new Vect3(1,1,1));

                yield return new TestCaseData(nodeGraph);
            }
        }
        public static IEnumerable UnlinkedNodeGraph
        {
            get
            {
                NodeGraph nodeGraph = new NodeGraph();

                Node node = new Node(1,1,1);
                Node secondNode = new Node(2,2,2);
                Node thirdNode = new Node(3,3,3);
                Node fourthNode = new Node(4,4,4);

                nodeGraph.AddNode(node);
                nodeGraph.AddNode(secondNode);
                nodeGraph.AddNode(thirdNode);
                nodeGraph.AddNode(fourthNode);

                yield return new TestCaseData(nodeGraph);
            }
        }

        public static IEnumerable LinkedFiveNodeGraph
        {
            get
            {
                NodeGraph nodeGraph = new NodeGraph();

                Node node = new Node(1,1,1);
                Node secondNode = new Node(2,2,2);
                Node thirdNode = new Node(3,3,3);
                Node fourthNode = new Node(4,4,4);
                Node fifthNode = new Node(5,5,5);

                nodeGraph.AddNode(node);
                nodeGraph.AddNode(secondNode, node);
                nodeGraph.AddNode(thirdNode, secondNode);
                nodeGraph.AddNode(fourthNode, thirdNode);
                nodeGraph.AddNode(fifthNode, fourthNode);

                yield return new TestCaseData(nodeGraph);
            }
        }

        public static IEnumerable CoreLinkedNodeGraph
        {
            get
            {
                NodeGraph nodeGraph = new NodeGraph();

                Node node = new Node(1,1,1);
                Node secondNode = new Node(2,2,2);
                Node thirdNode = new Node(3,3,3);
                Node fourthNode = new Node(4,4,4);
                Node fifthNode = new Node(5,5,5);

                nodeGraph.AddNode(node);
                nodeGraph.AddNode(secondNode, node);
                nodeGraph.AddNode(thirdNode, node);
                nodeGraph.AddNode(fourthNode, node);
                nodeGraph.AddNode(fifthNode, node);

                yield return new TestCaseData(nodeGraph);
            }
        }
    }


}
