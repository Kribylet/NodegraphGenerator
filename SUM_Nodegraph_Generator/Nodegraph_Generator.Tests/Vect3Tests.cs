using NUnit.Framework;
using System;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    class Vect3Tests
    {
        [Test]
        public void When_CreateEmpty_Expect_Empty()
        {
            Vect3 vect = new Vect3();
            Assert.AreEqual(vect.x, 0);
            Assert.AreEqual(vect.y, 0);
            Assert.AreEqual(vect.z, 0);
        }

        [Test]
        public void When_CreateExplicit_Expect_Explicit()
        {
            Vect3 vect = new Vect3(1, 2, 3);
            Assert.AreEqual(vect.x, 1);
            Assert.AreEqual(vect.y, 2);
            Assert.AreEqual(vect.z, 3);
        }

        [Test]
        public void When_CompareEqual_Expect_Equal()
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            Vect3 vect2 = new Vect3(1, 2, 3);
            Assert.AreEqual(vect1.x, vect2.x);
            Assert.AreEqual(vect1.y, vect2.y);
            Assert.AreEqual(vect1.z, vect2.z);
            Assert.IsTrue(vect1 == vect2);
            Assert.IsFalse(vect1 != vect2);
            Assert.AreEqual(vect1, vect2);
        }

        [Test]
        public void When_CompareDiff_Expect_Diff()
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            Vect3 vect2 = new Vect3(1, 2, 4);
            Assert.IsTrue(vect1 != vect2);
            Assert.IsFalse(vect1 == vect2);
            Assert.AreNotEqual(vect1, vect2);
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public void When_Add_Expect_Add(double input)
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            vect1 += new Vect3(input, input, input);
            Vect3 vect2 = new Vect3(1 + input, 2 + input, 3 + input);
            Assert.AreEqual(vect1, vect2);
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public void When_Sub_Expect_Sub(double input)
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            vect1 -= new Vect3(input, input, input);
            Vect3 vect2 = new Vect3(1 - input, 2 - input, 3 - input);
            Assert.AreEqual(vect1, vect2);
        }

        [TestCase(2)]
        [TestCase(0)]
        [TestCase(-2)]
        public void When_Mult_Expect_Mult(double input)
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            vect1 *= input;
            Vect3 vect2 = new Vect3(1 * input, 2 * input, 3 * input);
            Assert.AreEqual(vect1, vect2);
        }

        [TestCase(2)]
        [TestCase(-2)]
        public void When_Div_Expect_Div(double input)
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            vect1 /= input;
            Vect3 vect2 = new Vect3(1 / input, 2 / input, 3 / input);
            Assert.AreEqual(vect1, vect2);
        }

        [Test]
        public void When_DivByZero_Expect_Exception()
        {
            Assert.Throws<DivideByZeroException>(new TestDelegate(When_DivByZero_Expect_Exception_Helper));
        }

        private void When_DivByZero_Expect_Exception_Helper()
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            vect1 /= 0;
        }

        [Test]
        public void When_ReadZero_Expect_Zero()
        {
            Vect3 zero = new Vect3(0, 0, 0);
            Assert.AreEqual(zero, Vect3.Zero);
        }
        [Test]
        public void When_ReadOne_Expect_One()
        {
            Vect3 one = new Vect3(1, 1, 1);
            Assert.AreEqual(one, Vect3.One);
        }
        [Test]
        public void When_ReadRight_Expect_Right()
        {
            Vect3 right = new Vect3(1, 0, 0);
            Assert.AreEqual(right, Vect3.Right);
        }
        [Test]
        public void When_ReadUp_Expect_Up()
        {
            Vect3 up = new Vect3(0, 1, 0);
            Assert.AreEqual(up, Vect3.Up);
        }
        [Test]
        public void When_ReadForward_Expect_Forward()
        {
            Vect3 forward = new Vect3(0, 0, 1);
            Assert.AreEqual(forward, Vect3.Forward);
        }

        [Test]
        public void When_NormalizeLarger_Expect_Normal()
        {
            Vect3 vect1 = new Vect3(1, 1, 0);
            Vect3 vect2 = new Vect3(1 / Math.Sqrt(2), 1 / Math.Sqrt(2), 0);
            vect1 = vect1.GetNormalized();
            Assert.AreEqual(vect1, vect2);
            Assert.That(Util.NearlyEqual(1, vect1.Length()));

        }

        [Test]
        public void When_NormalizeSmaller_Expect_Normal()
        {
            Vect3 vect1 = new Vect3(0.1d, 0.1d, 0);
            Vect3 vect2 = new Vect3(1 / Math.Sqrt(2), 1 / Math.Sqrt(2), 0);
            vect1 = vect1.GetNormalized();
            Assert.AreEqual(vect1, vect2);
            Assert.That(Util.NearlyEqual(1, vect1.Length()));

        }

        [Test]
        public void When_NormalizeNegative_Expect_SameSignedness()
        {
            Vect3 vect1 = new Vect3(-1, -1, -1);

            Assert.That(vect1.x < 0);
            Assert.That(vect1.y < 0);
            Assert.That(vect1.z < 0);
        }

        [Test]
        public void When_NormalizeZero_Expect_Exception()
        {
            Assert.Throws<ZeroVectorException>(new TestDelegate(When_NormalizeZero_Expect_Exception_Helper));
        }

        private void When_NormalizeZero_Expect_Exception_Helper()
        {
            Vect3 vect = Vect3.Zero;
            vect.GetNormalized();
        }

        [Test]
        public void When_CalculateOrthogonalDotProduct_Expect_Zero()
        {
            Assert.AreEqual(Vect3.Dot(Vect3.Up, Vect3.Forward), 0);
        }

        [Test]
        public void When_CalculateParallelNormalizedDotProduct_Expect_One()
        {
            Assert.AreEqual(Vect3.Dot(Vect3.Forward, Vect3.Forward), 1);
        }

        [Test]
        public void When_CalculateDotProduct_Expect_Dot()
        {
            Vect3 v1 = new Vect3(1, 2, 3);
            Vect3 v2 = new Vect3(3, 2, 1);
            Assert.AreEqual(Vect3.Dot(v1, v2), 10);
        }

        [Test]
        public void When_CalculateCrossOfSelf_Expect_Zero()
        {
            Vect3 v1 = new Vect3(1, 2, 3);
            Assert.AreEqual(Vect3.Cross(v1, v1), Vect3.Zero);
        }

        [Test]
        public void When_CalculateCrossOfOrthogonal_Expect_Orthogonal()
        {
            Assert.AreEqual(Vect3.Cross(Vect3.Up, Vect3.Forward), Vect3.Right);
        }

        [Test]
        public void When_CalculateCrossOfParallel_Expect_Zero()
        {
            Vect3 v1 = new Vect3(1, 2, 3);
            Vect3 v2 = new Vect3(2, 4, 6);
            Assert.AreEqual(Vect3.Cross(v1, v2), Vect3.Zero);
        }

        [Test]
        public void When_CalculateCrossOfTwoVectors_Expect_OrthogonalToBoth()
        {
            Vect3 v1 = new Vect3(1, 2, 3);
            Vect3 v2 = new Vect3(3, 2, 1);
            Vect3 v3 = new Vect3(-4, 8, -4);
            Assert.AreEqual(Vect3.Cross(v1, v2), v3);
        }

        [Test]
        public void When_ProjectOnZeroVector_Expect_ZeroVector()
        {
            Vect3 v1 = new Vect3(1, 2, 3);
            Assert.AreEqual(Vect3.Zero.ProjectOnVector(v1), Vect3.Zero);
            Assert.AreEqual(v1.ProjectOnVector(Vect3.Zero), Vect3.Zero);
        }

        [Test]
        public void When_ProjectVectors_Expect_Valid()
        {
            Vect3 v1 = new Vect3(1, 2, 3);
            Vect3 v2 = new Vect3(3, 2, 1);
            Vect3 v3 = new Vect3(15d / 7d, 10d / 7d, 5d / 7d);
            Assert.AreEqual(v1.ProjectOnVector(v2), v3);
        }

        [Test]
        public void When_ProjectOnZeroNormalPlane_Expect_Exception()
        {
            Assert.Throws<InvalidPlaneException>(new TestDelegate(When_ProjectOnZeroNormalPlane_Expect_Exception_Helper));
        }
        private void When_ProjectOnZeroNormalPlane_Expect_Exception_Helper()
        {
            new Vect3(1, 2, 3).ProjectOnPlane(Vect3.Zero);
        }

        [Test]
        public void When_ProjectOnPlaneWithZeroVect_Expect_Zero()
        {
            Assert.AreEqual(Vect3.Zero.ProjectOnPlane(Vect3.Up), Vect3.Zero);
            Assert.AreEqual(Vect3.Zero.ProjectOnPlane(Vect3.Right, Vect3.Up), Vect3.Zero);
        }

        [Test]
        public void When_ProjectOnOrthogonalPlaneWithNormal_Expect_Zero()
        {
            Vect3 vect = 2 * Vect3.One;
            Assert.AreEqual(vect.ProjectOnPlane(Vect3.One), Vect3.Zero);
        }

        [Test]
        public void When_ProjectOnPlaneWithParallelBases_Expect_Exception()
        {
            Assert.Throws<InvalidPlaneException>(new TestDelegate(When_ProjectOnPlaneWithParallelBases_Expect_Exception_Helper));
        }
        private void When_ProjectOnPlaneWithParallelBases_Expect_Exception_Helper()
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            Vect3 vect2 = new Vect3(1, 1, 1);
            Vect3 vect3 = new Vect3(2, 2, 2);
            vect3.ProjectOnPlane(vect2, vect3);
        }

        [Test]
        public void When_ProjectOnOrthogonalPlaneWithBases_Expect_Zero()
        {
            Vect3 vect = Vect3.Up;
            Assert.AreEqual(vect.ProjectOnPlane(Vect3.Right, Vect3.Forward), Vect3.Zero);
        }

        [Test]
        public void When_ProjectOnPlane_Expect_Valid()
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            Vect3 vect2 = new Vect3(1, 0, 3);
            Assert.AreEqual(vect1.ProjectOnPlane(Vect3.Up), vect2);
            Assert.AreEqual(vect1.ProjectOnPlane(Vect3.Forward, Vect3.Right), vect2);
        }

        [Test]
        public void When_GetNormalPlaneOfZeroVect_Expect_Exception()
        {
            Assert.Throws<InvalidPlaneException>(new TestDelegate(When_GetNormalPlaneOfZeroVect_Expect_Exception_Helper));
        }
        private void When_GetNormalPlaneOfZeroVect_Expect_Exception_Helper()
        {
            Vect3.Zero.GetNormalPlane();
        }

        [Test]
        public void When_DotWithNormalPlane_Expect_Zero()
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            Tuple<Vect3, Vect3> plane = vect1.GetNormalPlane();
            Assert.Zero(Vect3.Dot(vect1, plane.Item1));
            Assert.Zero(Vect3.Dot(vect1, plane.Item2));
        }

        [Test]
        public void When_CrossWithNormalPlane_Expect_StartVector()
        {
            Vect3 vect1 = new Vect3(1, 2, 3);
            Tuple<Vect3, Vect3> plane = vect1.GetNormalPlane();
            Vect3 vect2 = Vect3.Cross(plane.Item1, plane.Item2);
            Assert.AreEqual(vect1.GetNormalized(), vect2.GetNormalized());
        }

        [TestCase(1, 2, 3)]
        [TestCase(1, 0, 0)]
        [TestCase(0.0006d, 0.0004d, 0.12312d)]
        [TestCase(50, 750, 3)]
        public void When_NormalizeTwice_Expect_Same(double x, double y, double z)
        {
            Vect3 v1 = new Vect3(x, y, z);
            v1 = v1.GetNormalized();
            Vect3 v2 = v1.GetNormalized();

            Assert.That(Util.NearlyEqual(v1.Length(), 1));
            Assert.That(Util.NearlyEqual(v2.Length(), 1));

            Assert.AreEqual(v1, v2);
        }

        [TestCase(-10E100,-10E100, -10E100)]
        [TestCase(-10E10, -10E10, -10E10)]
        [TestCase(-1, -1, -1)]
        [TestCase(-10E-10, -10E-10, -10E-10)]
        [TestCase(-10E-100, -10E-100, -10E-100)]
        [TestCase(0, 0, 0)]
        [TestCase(10E-100, 10E-100, 10E-100)]
        [TestCase(10E-10, 10E-10, 10E-10)]
        [TestCase(1, 1, 1)]
        [TestCase(10E10, 10E10, 10E10)]
        [TestCase(10E100,10E100, 10E100)]
        public void When_CompareSimilar_Expect_Equality(double x, double y, double z)
        {
            Vect3 v1 = new Vect3(x, y, z);
            Vect3 v2 = new Vect3(x + Util.DOUBLE_EPSILON/2d,
                                 y + Util.DOUBLE_EPSILON/2d,
                                 z + Util.DOUBLE_EPSILON/2d);

            Assert.That(Util.NearlyEqual(v1.x, v2.x), "Found inequality: {0} {1}", v1.x, v2.x);
            Assert.That(Util.NearlyEqual(v1.y, v2.y), "Found inequality: {0} {1}", v1.y, v2.y);
            Assert.That(Util.NearlyEqual(v1.z, v2.z), "Found inequality: {0} {1}", v1.z, v2.z);

            Assert.AreEqual(v1, v2);
        }

        [TestCase(-10E5, -10E5, -10E5)]
        [TestCase(-10E2, -10E2, -10E2)]
        [TestCase(-1, -1, -1)]
        [TestCase(-10E-10, -10E-10, -10E-10)]
        [TestCase(-10E-100, -10E-100, -10E-100)]
        [TestCase(0, 0, 0)]
        [TestCase(10E-100, 10E-100, 10E-100)]
        [TestCase(10E-10, 10E-10, 10E-10)]
        [TestCase(1, 1, 1)]
        [TestCase(10E2, 10E2, 10E2)]
        [TestCase(10E5, 10E5, 10E5)]
        public void When_Compare_Different_Expect_Inequality(double x, double y, double z)
        {
            Vect3 v1 = new Vect3(x, y, z);
            Vect3 v2 = new Vect3(x + (3d/2d)*Util.DOUBLE_EPSILON,
                                 y + (3d/2d)*Util.DOUBLE_EPSILON,
                                 z + (3d/2d)*Util.DOUBLE_EPSILON);

            Assert.That(!Util.NearlyEqual(v1.x, v2.x), "Found equality: {0} {1}", v1.x, v2.x);
            Assert.That(!Util.NearlyEqual(v1.y, v2.y), "Found equality: {0} {1}", v1.y, v2.y);
            Assert.That(!Util.NearlyEqual(v1.z, v2.z), "Found equality: {0} {1}", v1.z, v2.z);

            Assert.AreNotEqual(v1, v2);
        }

        [Test]
        public void When_FindAngleBetweenPerpendicular_Expect_Ninety()
        {
            Assert.That(Util.NearlyEqual(Vect3.Up.AngleTo(Vect3.Forward), 90d));
            Assert.That(Util.NearlyEqual(Vect3.Up.AngleTo(Vect3.Right), 90d));
            Assert.That(Util.NearlyEqual(Vect3.Right.AngleTo(Vect3.Forward), 90d));
        }

        [Test]
        public void When_FindAngleToVectors_Expect_InsideValidInterval()
        {
            Vect3 baseVector = new Vect3(1,0,0);
            Vect3 movingVector = new Vect3(1,0,0);
            for (int newX = -10; newX <= 10; newX++) {
                movingVector.x = newX;
                for (int newY = -10; newY <= 10; newY++) {
                    movingVector.y = newY;
                    for (int newZ = -10; newZ <= 10; newZ++) {
                        movingVector.z = newZ;
                        if (movingVector.Equals(Vect3.Zero)) {continue;}
                        Assert.LessOrEqual(movingVector.AngleTo(baseVector), 180,
                            "Invalid angle found for {0} and {1}: {2}", movingVector, baseVector, movingVector.AngleTo(baseVector));
                        Assert.GreaterOrEqual(movingVector.AngleTo(baseVector), 0,
                            "Invalid angle found for {0} and {1}: {2}", movingVector, baseVector, movingVector.AngleTo(baseVector));
                        Assert.LessOrEqual(baseVector.AngleTo(movingVector), 180,
                            "Invalid angle found for {0} and {1}: {2}", baseVector, movingVector, baseVector.AngleTo(movingVector));
                        Assert.GreaterOrEqual(baseVector.AngleTo(movingVector), 0,
                            "Invalid angle found for {0} and {1}: {2}", baseVector, movingVector, baseVector.AngleTo(movingVector));
                        Assert.That(Util.NearlyEqual(movingVector.AngleTo(baseVector), baseVector.AngleTo(movingVector)));
                    }
                }
            }
        }

        [Test]
        public void When_FindAngleWithZeroVector_Except_Exception()
        {
            Assert.Throws<ZeroVectorException>(new TestDelegate(When_FindAngleWithZeroVector_Except_Exception_Helper));
        }
        private void When_FindAngleWithZeroVector_Except_Exception_Helper()
        {
            Vect3.Zero.AngleTo(Vect3.One);
        }

        [Test]
        public void When_FindAngleToZeroVector_Except_Exception()
        {
            Assert.Throws<ZeroVectorException>(new TestDelegate(When_FindAngleToZeroVector_Except_Exception_Helper));
        }
        private void When_FindAngleToZeroVector_Except_Exception_Helper()
        {
            Vect3.One.AngleTo(Vect3.Zero);
        }

        [Test]
        public void When_FindAngleBetweenSameVector_Expect_Zero()
        {
            Vect3 v = new Vect3(5, 1, 2);
            Assert.That(Util.NearlyEqual(v.AngleTo(v), 0));
        }

        [Test]
        public void When_FindAngleBetween45Degrees_Expect_45()
        {
            Vect3 v = new Vect3(1, 1, 0);
            Assert.That(Util.NearlyEqual(v.AngleTo(Vect3.Up), 45d));
        }

        [Test]
        public void When_FindingClosestPointOnLineToPointInSpan_Expect_ShorterThanEndPointLines() {
            Vect3 lineStartPoint = new Vect3(0,0,0);
            Vect3 lineEndPoint = new Vect3(10,0,0);

            Vect3 point1 = new Vect3(5, 1, 0);
            Vect3 point2 = new Vect3(9, 500, 0);

            Vect3 closestLinePoint1 = Vect3.FindClosestLinePoint(lineStartPoint, lineEndPoint, point1);
            Vect3 closestLinePoint2 = Vect3.FindClosestLinePoint(lineStartPoint, lineEndPoint, point2);

            Assert.AreEqual(new Vect3(5,0,0), closestLinePoint1);
            Assert.AreEqual(new Vect3(9,0,0), closestLinePoint2);
        }

        [Test]
        public void When_FindingClosestPointOnLineToPointOutsideSpan_Expect_EndPoints() {
            Vect3 lineStartPoint = new Vect3(0,0,0);
            Vect3 lineEndPoint = new Vect3(10,0,0);

            Vect3 point1 = new Vect3(-15, 1, 0);
            Vect3 point2 = new Vect3(15, 500, 0);

            Vect3 closestLinePoint1 = Vect3.FindClosestLinePoint(lineStartPoint, lineEndPoint, point1);
            Vect3 closestLinePoint2 = Vect3.FindClosestLinePoint(lineStartPoint, lineEndPoint, point2);

            Assert.AreEqual(new Vect3(0,0,0), closestLinePoint1);
            Assert.AreEqual(new Vect3(10,0,0), closestLinePoint2);
        }

        [Test]
        public void When_CalculateTriangleArea_Expect_Correct() {
            Vect3 point1 = new Vect3(0, 0,  0);
            Vect3 point2 = new Vect3(10, 0,  0);
            Vect3 point3 = new Vect3(0, 10,  0);

            Vect3 point4 = new Vect3(0, 0,  0);
            Vect3 point5 = new Vect3(-10, 0,  0);
            Vect3 point6 = new Vect3(0, -10,  0);

            Vect3 point7 = new Vect3(0, 0,  0);
            Vect3 point8 = new Vect3(0, 150, 150);
            Vect3 point9 = new Vect3(150, 150,  150);

            Assert.AreEqual(50, Vect3.TriangleArea(point1, point2, point3));
            Assert.AreEqual(50, Vect3.TriangleArea(point4, point5, point6));
            Assert.That(Util.NearlyEqual(15909.902576697319, Vect3.TriangleArea(point7, point8, point9)), "{0} vs. {1} failed", 15909.902576697319, Vect3.TriangleArea(point7, point8, point9));
        }

        [Test]
        public void When_CalculateTriangleAreaAsSum_Expect_Equal() {
            Vect3 point1 = new Vect3(0, 0,  0);
            Vect3 point2 = new Vect3(10, 0,  0);
            Vect3 point3 = new Vect3(0, 10,  0);

            Vect3 point4 = new Vect3(1,1,0);

            double triangleSum = Vect3.TriangleArea(point1, point2, point4) + Vect3.TriangleArea(point1, point3, point4) + Vect3.TriangleArea(point2, point3, point4);

            Assert.That(Util.NearlyEqual(Vect3.TriangleArea(point1, point2, point3), triangleSum));
        }
    }
}
