using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Nodegraph_Generator.Tests
{
    public class XMLCreatorTests
    {

        private const String WindowsPathFormat = "..\\..\\..\\..\\";
        private const String MacPathFormat = "../../../../";
        private static bool windowsRuntime = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private String OSString = windowsRuntime ? WindowsPathFormat : MacPathFormat;

        [TestCaseSource(typeof(MyDataClass), "TestCaseNodeGraphWithEdgeNodeLists")]
        public void When_WriteAndRead_TempNodeGraphWithEdgeNodeLists_Expect_SameGraph(TempNodeGraphWithEdgeNodeLists originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            TempNodeGraphWithEdgeNodeLists restoredGraph = XMLCreator.readXML<TempNodeGraphWithEdgeNodeLists>(filePath);
            Assert.AreEqual(originalGraph, restoredGraph);
        }

        [TestCaseSource(typeof(MyDataClass), "TestCaseNodeGraphWithEdgeNodeLists")]
        public void When_WriteAndRead_TempNodeGraphWithEdgeNodeLists_Expect_SameNodeLists(TempNodeGraphWithEdgeNodeLists originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            TempNodeGraphWithEdgeNodeLists restoredGraph = XMLCreator.readXML<TempNodeGraphWithEdgeNodeLists>(filePath);
            Assert.AreEqual(originalGraph.nodeList, restoredGraph.nodeList);
        }

        [TestCaseSource(typeof(MyDataClass), "TestCaseNodeGraphWithEdgeNodeLists")]
        public void When_WriteAndRead_TempNodeGraphWithEdgeNodeLists_Expect_DifferentEdgeLists(TempNodeGraphWithEdgeNodeLists originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);

            originalGraph.edgeList = new Edge[]
            {
                new Edge(123123, 1124234),
                new Edge(23412312, 23451235)
            };

            TempNodeGraphWithEdgeNodeLists restoredGraph = XMLCreator.readXML<TempNodeGraphWithEdgeNodeLists>(filePath);
            Assert.AreNotEqual(originalGraph.edgeList, restoredGraph.edgeList);
        }

        [TestCaseSource(typeof(MyDataClass), "TestCaseTempNodeGraph")]
        public void When_WriteAndRead_TempNodeGraph_Expect_SameGraph(TempNodeGraph originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            TempNodeGraph restoredGraph = XMLCreator.readXML<TempNodeGraph>(filePath);
            Assert.AreEqual(originalGraph, restoredGraph);
        }

        [TestCaseSource(typeof(MyDataClass), "TestCaseTempNodeGraph")]
        public void When_WriteAndRead_TempNodeGraph_Expect_SameTestString(TempNodeGraph originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            TempNodeGraph restoredGraph = XMLCreator.readXML<TempNodeGraph>(filePath);

            Assert.AreEqual(originalGraph.tempString, restoredGraph.tempString);
        }

        [TestCaseSource(typeof(MyDataClass), "TestCaseTempNodeGraph")]
        public void When_WriteAndRead_TempNodeGraph_Expect_SameTestStringArray(TempNodeGraph originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            TempNodeGraph restoredGraph = XMLCreator.readXML<TempNodeGraph>(filePath);

            Assert.AreEqual(originalGraph.tempNodeArray, restoredGraph.tempNodeArray);
        }

        [TestCaseSource(typeof(MyDataClass), "TestCaseTempNodeGraph")]
        public void When_WriteAndRead_TempNodeGraph_Expect_SameNode(TempNodeGraph originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            TempNodeGraph restoredGraph = XMLCreator.readXML<TempNodeGraph>(filePath);

            Assert.AreEqual(originalGraph.tempNode, restoredGraph.tempNode);
        }

        [TestCaseSource(typeof(MyDataClass), "TestCaseEmptyNodeGraph")]
        public void When_WriteAndRead_Empty_NodeGraph_Expect_SameNodeGraph(NodeGraph originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            NodeGraph restoredGraph = XMLCreator.readXML<NodeGraph>(filePath);
            Assert.AreEqual(originalGraph, restoredGraph);
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "SimpleNodeGraph")]
        public void When_WriteAndReadAndWrite_SimpleNodeGraph_Expect_SameNodeGraph(NodeGraph originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            NodeGraph restoredGraph = XMLCreator.readXML<NodeGraph>(filePath);

            filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput2.xml"));
            XMLCreator.writeXML(restoredGraph, filePath);

            Assert.IsTrue(originalGraph.Equals(restoredGraph));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "LinkedFiveNodeGraph")]
        public void When_WriteAndReadAndWrite_LinkedFiveNodeGraph_Expect_SameNodeGraph(NodeGraph originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            NodeGraph restoredGraph = XMLCreator.readXML<NodeGraph>(filePath);

            filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput2.xml"));
            XMLCreator.writeXML(restoredGraph, filePath);

            Assert.IsTrue(originalGraph.Equals(restoredGraph));
        }

        [TestCaseSource(typeof(TestNodeGraphClass), "CoreLinkedNodeGraph")]
        public void When_WriteAndReadAndWrite_CoreLinkedNodeGraph_Expect_SameNodeGraph(NodeGraph originalGraph)
        {
            string filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput.xml"));
            XMLCreator.writeXML(originalGraph, filePath);
            NodeGraph restoredGraph = XMLCreator.readXML<NodeGraph>(filePath);

            filePath = ArgParser.GetPath(Path.Combine(OSString, "Output_Files", "xmlOutput2.xml"));
            XMLCreator.writeXML(restoredGraph, filePath);

            Assert.IsTrue(originalGraph.Equals(restoredGraph));
        }

        /**
         * Class containing data members used for testing XML serialization / deserialization
         */
        public class MyDataClass
        {
            public static IEnumerable TestCaseTempNodeGraph
            {
                get
                {
                    TempNodeGraph tempNodeGraph = new TempNodeGraph();
                    TempNode tempNode = new TempNode();
                    tempNodeGraph.tempNodeArray.Add(tempNode);
                    yield return new TestCaseData(tempNodeGraph);
                }
            }
            public static IEnumerable TestCaseNodeGraphWithEdgeNodeLists
            {
                get
                {
                    yield return new TestCaseData(new TempNodeGraphWithEdgeNodeLists());
                }
            }
            public static IEnumerable TestCaseEmptyNodeGraph
            {
                get
                {
                    yield return new TestCaseData(new NodeGraph());
                }
            }
        }

        /**
         * TEST CLASS
         * Used for testing deserialization capabilities of DataContractor.
         * Used to see how different datatypes are represented in XML after serialized.
         */
        public class TempNodeGraph
        {
            public String tempString;
            public TempNode tempNode;
            public List<TempNode> tempNodeArray;

            public TempNodeGraph()
            {
                this.tempString = "testString";
                this.tempNode = new TempNode();
                this.tempNodeArray = new List<TempNode>();
            }

            public override bool Equals(Object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    TempNodeGraph other = (TempNodeGraph)obj;

                    return (tempString.Equals(other.tempString)) &&
                           System.Linq.Enumerable.SequenceEqual(tempNodeArray, other.tempNodeArray) &&
                           (tempNode.Equals(other.tempNode));
                }
            }

            public override int GetHashCode()
            {
                return this.tempString.GetHashCode() ^ this.tempNode.GetHashCode() << 2 ^ this.tempNodeArray.GetHashCode() >> 2;
            }
        }

        /**
         * TEST CLASS
         * Rudimentary class mimicing the real Node class.
         */
        public class TempNode
        {
            public int x = 1;
            public int y = 3;
            public int z = 5;

            public override bool Equals(Object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    TempNode other = (TempNode)obj;
                    return (x == other.x) && (y == other.y) && (z == other.z);
                }
            }

            public override int GetHashCode()
            {
                return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
            }
        }

        /**
         * TEST CLASS
         * Used for testing the real Node and Edge classes that are to be used in NodeGraph later.
         */
        public class TempNodeGraphWithEdgeNodeLists
        {
            public Edge[] edgeList = new Edge[]
            {
            new Edge(1, 2),
            new Edge(2, 2)
            };

            public Node[] nodeList = new Node[]
            {
            new Node(1, 1 ,1),
            new Node()
            };

            public override bool Equals(Object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    TempNodeGraphWithEdgeNodeLists other = (TempNodeGraphWithEdgeNodeLists)obj;

                    return System.Linq.Enumerable.SequenceEqual(edgeList, other.edgeList) &&
                           System.Linq.Enumerable.SequenceEqual(nodeList, other.nodeList);
                }
            }

            public override int GetHashCode()
            {
                return this.edgeList.GetHashCode() ^ this.nodeList.GetHashCode();
            }
        }
    }
}

