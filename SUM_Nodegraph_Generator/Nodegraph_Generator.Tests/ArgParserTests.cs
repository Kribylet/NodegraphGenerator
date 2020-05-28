using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Nodegraph_Generator.Tests
{
    [TestFixture]
    public class ArgParserTests
    {
        private const string WindowsPathFormat = "..\\..\\..\\..\\";
        private const string MacPathFormat = "../../../../";
        private static bool windowsRuntime = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private string OSString = windowsRuntime ? WindowsPathFormat : MacPathFormat;

        [TestCase("Input_Files", "input.txt")]
        [TestCase("Output_Files", "output.txt")]
        public void When_Correct_GetPath_Expect_True(string directoryName, string fileName)
        {
            string path = Path.Combine("Input_Files", "input.txt");

            // IO must be either "Input_Files" or "Output_Files".
            // Expected output for test files.
            // Paths contain "Nodegraph_Generator.Tests" 
            string relativePath = Path.Combine(OSString, path);
            string inputPath = ArgParser.GetInputPath(relativePath);

            string expectedPath = Path.Combine("kandidatprojektet", "SUM_Nodegraph_Generator", path);

            StringAssert.Contains(expectedPath, inputPath);
        }

        [Test]
        public void When_Correct_GetInputPath_Expect_True()
        {
            string inputStr = Path.Combine("Input_Files", "input.txt");

            // IO must be either "Input_Files" or "Output_Files".
            // Expected output for test files.
            // Paths contain "Nodegraph_Generator.Tests" 
            string relativePath = Path.Combine(OSString, inputStr);
            string inputPath = ArgParser.GetInputPath(relativePath);

            string expectedInputPath = Path.Combine("kandidatprojektet", "SUM_Nodegraph_Generator", inputStr);

            StringAssert.Contains(expectedInputPath, inputPath);
        }

        [Test]
        public void When_Correct_GetOutputPath_Expect_True()
        {
            string outputStr = Path.Combine("Output_Files", "output.txt");

            // IO must be either "Input_Files" or "Output_Files".
            // Expected output for test files.
            // Paths contain "Nodegraph_Generator.Tests" 
            string relativePath = Path.Combine(OSString, outputStr);
            string outputPath = ArgParser.GetOutputPath(relativePath);

            string expectedOutputPath = Path.Combine("kandidatprojektet", "SUM_Nodegraph_Generator", outputStr);

            StringAssert.Contains(expectedOutputPath, outputPath);
        }

        [Test]
        public void When_Correct_ArgumentsIsGiven_Expect_True()
        {
            string inputStr = Path.Combine("Input_Files", "input.txt");
            string outputStr = Path.Combine("Output_Files", "output.txt");

            // IO must be either "Input_Files" or "Output_Files".
            // Expected output for test files.
            // Paths contain "Nodegraph_Generator.Tests" 
            string relativeInputPath = Path.Combine(OSString, inputStr);
            string relativeOutputPath = Path.Combine(OSString, outputStr);

            string[] inputArgs = { relativeInputPath, relativeOutputPath };
            FilePaths result = ArgParser.Parse(inputArgs);

            string expectedInputPath = Path.Combine("kandidatprojektet", "SUM_Nodegraph_Generator", inputStr);
            string expectedOutputPath = Path.Combine("kandidatprojektet", "SUM_Nodegraph_Generator", outputStr);

            StringAssert.Contains(expectedInputPath, result.inFile);
            StringAssert.Contains(expectedOutputPath, result.outFile);
            // When no flag is given it should be voxelSoluton by default.
            Assert.True(result.voxelSolution);
        }

        [Test]
        public void When_VoxelFlagIsGiven_Expect_VoxelSolution()
        {
            string inputStr = Path.Combine("Input_Files", "input.txt");
            string outputStr = Path.Combine("Output_Files", "output.txt");

            // IO must be either "Input_Files" or "Output_Files".
            // Expected output for test files.
            // Paths contain "Nodegraph_Generator.Tests" 
            string relativeInputPath = Path.Combine(OSString, inputStr);
            string relativeOutputPath = Path.Combine(OSString, outputStr);

            string[] inputArgs = { relativeInputPath, relativeOutputPath, "-v" };
            FilePaths result = ArgParser.Parse(inputArgs);

            Assert.True(result.voxelSolution);
        }

        [Test]
        public void When_MeshFlagIsGiven_Expect_MeshSolution()
        {
            string inputStr = Path.Combine("Input_Files", "input.txt");
            string outputStr = Path.Combine("Output_Files", "output.txt");

            // IO must be either "Input_Files" or "Output_Files".
            // Expected output for test files.
            // Paths contain "Nodegraph_Generator.Tests" 
            string relativeInputPath = Path.Combine(OSString, inputStr);
            string relativeOutputPath = Path.Combine(OSString, outputStr);

            string[] inputArgs = { relativeInputPath, relativeOutputPath, "-m" };
            FilePaths result = ArgParser.Parse(inputArgs);

            Assert.False(result.voxelSolution);
        }

        [Test]
        public void When_TooFewArgs_Expect_Exception()
        {
            Assert.Throws<ArgumentException>(new TestDelegate(When_TooFewArgs_Expect_Exception_Helper));
        }

        private void When_TooFewArgs_Expect_Exception_Helper()
        {
            string[] args = { "a" };
            ArgParser.CheckValidArgs(args);
        }

        [Test]
        public void When_TooManyArgs_Expect_Exception()
        {
            Assert.Throws<ArgumentException>(new TestDelegate(When_TooManyArgs_Expect_Exception_Helper));
        }

        private void When_TooManyArgs_Expect_Exception_Helper()
        {
            string[] args = { "a", "b", "c", "d" };
            ArgParser.CheckValidArgs(args);
        }

        [Test]
        public void When_InvalidFlag_Expect_Exception()
        {
            Assert.Throws<ArgumentException>(new TestDelegate(When_InvalidFlag_Expect_Exception_Helper));
        }

        private void When_InvalidFlag_Expect_Exception_Helper()
        {
            string[] args = { "a", "b", "-c" };
            ArgParser.CheckValidArgs(args);
        }   

        [Test]
        public void When_OutputFileExists_Expect_Exception()
        {
            Assert.Throws<ArgumentException>(new TestDelegate(When_OutputFileExists_Expect_Exception_Helper));
        }
        private void When_OutputFileExists_Expect_Exception_Helper()
        {
            string outputStr = "Output_Files/existingOutput.txt";
            string relativeOutputPath = OSString + outputStr;
            ArgParser.GetOutputPath(relativeOutputPath);
        }

        [Test]
        public void When_InputFileDosntExist_Expect_Exception()
        {
            Assert.Throws<ArgumentException>(new TestDelegate(When_InputFileDosntExist_Expect_Exception_Helper));
        }
        private void When_InputFileDosntExist_Expect_Exception_Helper()
        {
            string inputStr = "Input_Files/existingInput.txt";
            string relativeInputPath = OSString + inputStr;
            ArgParser.GetInputPath(relativeInputPath);
        }
    }
}

