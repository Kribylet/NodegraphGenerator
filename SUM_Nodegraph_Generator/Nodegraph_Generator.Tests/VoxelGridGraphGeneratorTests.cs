using System.Collections;
using NUnit.Framework;
using Nodegraph_Generator;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    class VoxelGridGraphGeneratorTests
    {
        // [TestCaseSource(typeof(DistanceGridTestData), "Rectangle7_7_12Grid")]
        // public void When_SkeletonizeSmallRectangleGrid_Expect_Line(int[,,] rectangleGrid)
        // {
        //     VoxelGridGraphGenerator.Skeletonize(ref rectangleGrid);

        //     string failString = "";

        //     for (int z = 0; z <= rectangleGrid.GetUpperBound(2); z++)
        //     {
        //         for (int y = rectangleGrid.GetUpperBound(1); y >= 0; y--)
        //         {
        //             for (int x = 0; x <= rectangleGrid.GetUpperBound(0); x++)
        //             {
        //                 failString += "[" + rectangleGrid[x,y,z] + "]";
        //             }
        //             failString += "\n";
        //         }
        //         failString += "\n";
        //     }

        //     Assert.Fail(failString);
        // }

        // [TestCaseSource(typeof(VoxelGridTestData), "Rectangle7_7_12Grid")]
        // public void When_GenerateDistanceGridFromSmallRectangleGridFromVoxel_Expect_DistanceMapped(VoxelGrid voxelGrid)
        // {
        //     int[,,] coordinateDistanceGrid = VoxelGridGraphGenerator.CreateStartGrid(voxelGrid);

        //     VoxelGridGraphGenerator.markExternal(ref coordinateDistanceGrid, voxelGrid);

        //     VoxelGridGraphGenerator.markInternal(ref coordinateDistanceGrid, voxelGrid);

        //     string failString = "";

        //     for (int z = 0; z <= coordinateDistanceGrid.GetUpperBound(2); z++)
        //     {
        //         for (int y = coordinateDistanceGrid.GetUpperBound(1); y >= 0; y--)
        //         {
        //             for (int x = 0; x <= coordinateDistanceGrid.GetUpperBound(0); x++)
        //             {
        //                 failString += "[" + coordinateDistanceGrid[x,y,z] + "]";
        //             }
        //             failString += "\n";
        //         }
        //         failString += "\n";
        //     }

        //     int[,,] expectedDistanceGrid = new int[7,7,12];

        //     for (int x = 1; x < 6; x++)
        //     {
        //         for (int y = 1; y < 6; y++)
        //         {
        //             for (int z = 1; z < 11; z++)
        //             {
        //                 expectedDistanceGrid[x,y,z] += 1;
        //             }
        //         }
        //     }

        //     for (int x = 2; x < 5; x++)
        //     {
        //         for (int y = 2; y < 5; y++)
        //         {
        //             for (int z = 2; z < 10; z++)
        //             {
        //                 expectedDistanceGrid[x,y,z] += 1;
        //             }
        //         }
        //     }

        //     for (int x = 3; x < 4; x++)
        //     {
        //         for (int y = 3; y < 4; y++)
        //         {
        //             for (int z = 3; z < 9; z++)
        //             {
        //                 expectedDistanceGrid[x,y,z] += 1;
        //             }
        //         }
        //     }

        //     for (int z = 0; z <= coordinateDistanceGrid.GetUpperBound(2); z++)
        //     {
        //         for (int y = coordinateDistanceGrid.GetUpperBound(1); y >= 0; y--)
        //         {
        //             for (int x = 0; x <= coordinateDistanceGrid.GetUpperBound(0); x++)
        //             {
        //                 failString += "[" + coordinateDistanceGrid[x,y,z] + "]";
        //             }
        //             failString += "\n";
        //             for (int x = 0; x <= coordinateDistanceGrid.GetUpperBound(0); x++)
        //             {
        //                 failString += "[" + expectedDistanceGrid[x,y,z] + "]";
        //             }
        //             failString += "\n\n";

        //         }
        //         failString += "\n";
        //     }

        //     for (int z = 0; z <= coordinateDistanceGrid.GetUpperBound(2); z++)
        //     {
        //         for (int y = coordinateDistanceGrid.GetUpperBound(1); y >= 0; y--)
        //         {
        //             for (int x = 0; x <= coordinateDistanceGrid.GetUpperBound(0); x++)
        //             {
        //                 Assert.AreEqual(coordinateDistanceGrid[x,y,z], expectedDistanceGrid[x,y,z]);
        //             }
        //         }
        //     }
        // }
    }
}

public class DistanceGridTestData
{
    public static IEnumerable Rectangle7_7_12Grid
    {
        get
        {
            int[,,] rectangleGrid = new int[7,7,12];

            for (int x = 1; x < 6; x++)
            {
                for (int y = 1; y < 6; y++)
                {
                    for (int z = 1; z < 11; z++)
                    {
                        rectangleGrid[x,y,z] += 1;
                    }
                }
            }

            for (int x = 2; x < 5; x++)
            {
                for (int y = 2; y < 5; y++)
                {
                    for (int z = 2; z < 10; z++)
                    {
                        rectangleGrid[x,y,z] += 1;
                    }
                }
            }

            for (int x = 3; x < 4; x++)
            {
                for (int y = 3; y < 4; y++)
                {
                    for (int z = 3; z < 9; z++)
                    {
                        rectangleGrid[x,y,z] += 1;
                    }
                }
            }

            yield return new TestCaseData(rectangleGrid);
        }
    }
}

public class VoxelGridTestData
{
    public static IEnumerable Rectangle7_7_12Grid
    {
        get
        {

            VoxelGrid voxelGrid = new VoxelGrid();
            bool[][][] coordinateGrid = new bool[7][][];

            for (int i = 0; i < 7; i++)
            {
                coordinateGrid[i] = new bool [7][];

                for (int j = 0; j < coordinateGrid[i].Length; j++)
                {
                    coordinateGrid[i][j] = new bool [12];
                }
            }

            for (int x = 1; x < 6; x++)
            {
                for (int y = 1; y < 6; y++)
                {
                    for (int z = 1; z < 11; z++)
                    {
                        coordinateGrid[x][y][z] = true;
                    }
                }
            }

            voxelGrid.xBound = 7;
            voxelGrid.yBound = 7;
            voxelGrid.zBound = 12;

            voxelGrid.coordinateGrid = coordinateGrid;

            yield return new TestCaseData(voxelGrid);
        }
    }
}