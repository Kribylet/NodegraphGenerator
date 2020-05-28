using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    public class GridTemplateTests
    {
        private const String WindowsPathFormat = "..\\..\\..\\..\\";
        private const String MacPathFormat = "../../../../";
        private static bool windowsRuntime = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private String OSString = windowsRuntime ? WindowsPathFormat : MacPathFormat;

        [Test]
        public void When_MirrorPlaneTop_Expect_Mirrored()
        {
            PointType[][][] blackUp = new PointType[][][] /*x y z*/
            {
                new PointType[][]
                {
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED},
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED},
                    new PointType[]{PointType.BLACK,PointType.BLACK,PointType.BLACK}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED},
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED},
                    new PointType[]{PointType.BLACK,PointType.BLACK,PointType.BLACK}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED},
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED},
                    new PointType[]{PointType.BLACK,PointType.BLACK,PointType.BLACK}
                }
            };

            GridTemplate blackUpGridTemplate = new GridTemplate(blackUp);

            GridTemplate mirroredBlackUp = GridTemplate.MirrorInPlane(blackUpGridTemplate, PlaneEnum.XZ);

            PointType[][][] blackDown = new PointType[][][] /*x y z*/
            {
                new PointType[][]
                {
                    new PointType[]{PointType.BLACK,PointType.BLACK,PointType.BLACK},
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED},
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.BLACK,PointType.BLACK,PointType.BLACK},
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED},
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.BLACK,PointType.BLACK,PointType.BLACK},
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED},
                    new PointType[]{PointType.IGNORED,PointType.IGNORED,PointType.IGNORED}
                }
            };

            GridTemplate blackDownGridTemplate = new GridTemplate(blackDown);

            Assert.AreEqual(blackDownGridTemplate, mirroredBlackUp);
        }

        [Test]
        public void When_MirrorPlane_Expect_Mirrored()
        {
            GridTemplate b1 = new  GridTemplate(GridTemplate.b1USW);

            PointType[][][] b1XZMirror = new PointType[][][] /*x y z*/
            {
                new PointType[][]
                {
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.BLACK,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.X,PointType.X}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.X,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.X,PointType.BLACK}
                }
            };

            PointType[][][] b1XYMirror = new PointType[][][] /*x y z*/
            {
                new PointType[][]
                {
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.X,PointType.X,PointType.WHITE},
                    new PointType[]{PointType.X,PointType.BLACK,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.BLACK,PointType.X,PointType.WHITE},
                    new PointType[]{PointType.X,PointType.X,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                }
            };

            PointType[][][] b1YZMirror = new PointType[][][] /*x y z*/
            {
                new PointType[][]
                {
                    new PointType[]{PointType.WHITE,PointType.X,PointType.BLACK},
                    new PointType[]{PointType.WHITE,PointType.X,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.WHITE,PointType.X,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.BLACK,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                }
            };

            Assert.Multiple(() =>
            {
                Assert.AreEqual(new GridTemplate(b1XYMirror), GridTemplate.MirrorInPlane(b1, PlaneEnum.XY));
                Assert.AreEqual(new GridTemplate(b1XZMirror), GridTemplate.MirrorInPlane(b1, PlaneEnum.XZ));
                Assert.AreEqual(new GridTemplate(b1YZMirror), GridTemplate.MirrorInPlane(b1, PlaneEnum.YZ));
            });
        }

        [Test]
        public void When_ReflectPlane_Expect_Reflected()
        {
            GridTemplate b2 = new GridTemplate(GridTemplate.b2USW);
            GridTemplate b3 = new GridTemplate(GridTemplate.b3USW);

            PointType[][][] b2R1Mirror = new PointType[][][] /*x y z*/
            {
                new PointType[][]
                {
                    new PointType[]{PointType.WHITE,PointType.X,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.X,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.WHITE,PointType.X,PointType.BLACK},
                    new PointType[]{PointType.WHITE,PointType.BLACK,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.WHITE,PointType.X,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.X,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                }
            };

            PointType[][][] b2R2Mirror = new PointType[][][] /*x y z*/
            {
                new PointType[][]
                {
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.WHITE,PointType.X,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.BLACK,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.X,PointType.X}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.WHITE,PointType.X,PointType.X},
                    new PointType[]{PointType.WHITE,PointType.X,PointType.BLACK},
                    new PointType[]{PointType.WHITE,PointType.X,PointType.X}
                }
            };

            PointType[][][] b3R3Mirror = new PointType[][][] /*x y z*/
            {
                new PointType[][]
                {
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE},
                    new PointType[]{PointType.WHITE,PointType.WHITE,PointType.WHITE}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.X,PointType.X,PointType.X},
                    new PointType[]{PointType.X,PointType.BLACK,PointType.X},
                    new PointType[]{PointType.X,PointType.X,PointType.X}
                },
                new PointType[][]
                {

                    new PointType[]{PointType.X,PointType.X,PointType.X},
                    new PointType[]{PointType.X,PointType.BLACK,PointType.X},
                    new PointType[]{PointType.X,PointType.X,PointType.X}
                }
            };

            Assert.Multiple(() =>
            {
                Assert.AreEqual(new GridTemplate(b2R1Mirror), GridTemplate.Reflect(b2, Reflection.R1));
                Assert.AreEqual(new GridTemplate(b2R2Mirror), GridTemplate.Reflect(b2, Reflection.R2));
                Assert.AreEqual(new GridTemplate(b3R3Mirror), GridTemplate.Reflect(b3, Reflection.R3));
            });
        }
    }
}
