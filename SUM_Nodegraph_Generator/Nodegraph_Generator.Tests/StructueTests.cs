using NUnit.Framework;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    public class StructueTests
    {

        [Test]
        public void When_Create_Expect_EmptyComponents(){
            Structure struc = new Structure();
            Assert.IsEmpty(struc.components);
        }

        [Test]
        public void When_AddComponent_Expect_Added(){
            Structure struc = new Structure();
            Component comp = new Component();
            int a = comp.CreateVertex(1,2,3);
            int b = comp.CreateVertex(2,2,2);
            int c = comp.CreateVertex(5,1,3);
            Vect3 normal = Vect3.Cross(new Vect3(1,2,3) - new Vect3(2,2,2), new Vect3(1,2,3) - new Vect3(5,1,3));
            comp.CreateFace(normal, new int[]{a,b,c});

            struc.addComponent(comp);
            Assert.AreEqual(struc.components.Count, 1);
            Assert.AreEqual(struc.components[0].faces.Count, 1);
            Assert.AreEqual(struc.components[0].vertices.Count, 3);
        }

                [Test]
        public void When_RemoveComponent_Expect_Removed(){
            Structure struc = new Structure();
            Component comp = new Component();
            
            int a = comp.CreateVertex(1,2,3);
            int b = comp.CreateVertex(2,2,2);
            int c = comp.CreateVertex(5,1,3);
            Vect3 normal = Vect3.Cross(new Vect3(1,2,3) - new Vect3(2,2,2), new Vect3(1,2,3) - new Vect3(5,1,3));
            comp.CreateFace(normal, new int[]{a,b,c});

            int strucIndex = struc.addComponent(comp);
            Assert.AreEqual(struc.components.Count, 1);
            struc.removeComponent(strucIndex);
            Assert.AreEqual(struc.components.Count, 0);
        }

    }
}
