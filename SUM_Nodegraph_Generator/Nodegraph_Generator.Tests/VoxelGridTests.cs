using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    public class VoxelGridTests
    {
        private const String WindowsPathFormat = "..\\..\\..\\..\\";
        private const String MacPathFormat = "../../../../";
        private static bool windowsRuntime = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private String OSString = windowsRuntime ? WindowsPathFormat : MacPathFormat;

        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleCube")]
        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleRightCorridor")]
        [TestCaseSource(typeof(SimpleGeometryClass), "SimpleLeftTurn")]
        public void When_CreateVoxelGridFromComponent_Expect_EmptyBorders(Structure simpleCube)
        {
            VoxelGrid voxelGrid = new VoxelGrid(simpleCube.components[0]);
            Assert.That(bordersAreEmpty(voxelGrid));
        }

        private bool bordersAreEmpty(VoxelGrid voxelGrid)
        {
            int maxZ = voxelGrid.coordinateGrid[0][0].Length-1;

            for (int x = 0; x < voxelGrid.coordinateGrid.Length; x++)
            {
                for (int y = 0; y < voxelGrid.coordinateGrid[0].Length; y++)
                {
                    if (voxelGrid.coordinateGrid[x][y][0]) return false;
                    if (voxelGrid.coordinateGrid[x][y][maxZ]) return false;
                }
            }

            int maxX = voxelGrid.coordinateGrid.Length-1;

            for (int y = 0; y < voxelGrid.coordinateGrid[0].Length; y++)
            {
                for (int z = 0; z < voxelGrid.coordinateGrid[0][0].Length; z++)
                {
                    if (voxelGrid.coordinateGrid[0][y][z]) return false;
                    if (voxelGrid.coordinateGrid[maxX][y][z]) return false;
                }
            }

            int maxY = voxelGrid.coordinateGrid[0].Length-1;

            for (int x = 0; x < voxelGrid.coordinateGrid.Length; x++)
            {
                for (int z = 0; z < voxelGrid.coordinateGrid[0][0].Length; z++)
                {
                    if (voxelGrid.coordinateGrid[x][0][z]) return false;
                    if (voxelGrid.coordinateGrid[x][maxY][z]) return false;
                }
            }

            return true;
        }
    }
}
