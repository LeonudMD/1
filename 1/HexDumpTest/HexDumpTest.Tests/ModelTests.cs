using System;
using System.IO;
using Xunit;

namespace HexDumpApp.Tests
{
    public class ModelTests : IDisposable
    {
        private readonly string _testDirectory;

        public ModelTests()
        {
            // Создаём временную директорию для тестовых файлов
            _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectory);
        }

        [Fact]
        public void GenerateHexDump_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange
            var model = new Model();
            string nonExistentFilePath = Path.Combine(_testDirectory, "nonexistentfile.bin");

            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() => model.GenerateHexDump(nonExistentFilePath));
            Assert.Equal("Файл не найден.", exception.Message);
        }

        [Fact]
        public void GenerateHexDump_EmptyFile_ReturnsEmptyHexDump()
        {
            // Arrange
            var model = new Model();
            string emptyFilePath = Path.Combine(_testDirectory, "emptyfile.bin");
            File.WriteAllBytes(emptyFilePath, new byte[0]);

            // Act
            string hexDump = model.GenerateHexDump(emptyFilePath);

            // Assert
            Assert.Equal(string.Empty, hexDump);
        }

        [Fact]
        public void GenerateHexDump_SmallFile_ReturnsCorrectHexDump()
        {
            // Arrange
            var model = new Model();
            string filePath = Path.Combine(_testDirectory, "smallfile.bin");
            byte[] data = { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"
            File.WriteAllBytes(filePath, data);

            // Act
            string hexDump = model.GenerateHexDump(filePath);

            // Assert
            string expectedHexDump =
        @"00000000  48 65 6C 6C 6F                                       Hello";

            // Удаляем пробелы и символы новой строки перед сравнением
            Assert.Equal(expectedHexDump.Replace(" ", string.Empty), hexDump.Replace(" ", string.Empty).TrimEnd());
        }

        [Fact]
        public void GenerateHexDump_LargeFile_ReturnsCorrectHexDump()
        {
            // Arrange
            var model = new Model();
            string filePath = Path.Combine(_testDirectory, "largefile.bin");
            byte[] data = new byte[256];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)i;
            }
            File.WriteAllBytes(filePath, data);

            // Act
            string hexDump = model.GenerateHexDump(filePath);

            // Assert
            // Проверим начало и конец Hex Dump с использованием Trim()
            Assert.StartsWith("00000000  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F  ................", hexDump);
            Assert.EndsWith("000000F0  F0 F1 F2 F3 F4 F5 F6 F7 F8 F9 FA FB FC FD FE FF  ................".Trim(), hexDump.Trim());
        }

        public void Dispose()
        {
            // Удаляем временную директорию после тестов
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }
    }
}
