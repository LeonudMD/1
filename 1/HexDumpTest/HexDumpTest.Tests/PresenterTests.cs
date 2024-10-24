using System;
using Moq;
using Xunit;

namespace HexDumpApp.Tests
{
    public class PresenterTests
    {
        [Fact]
        public void Run_ValidFilePath_ShowsHexDump()
        {
            // Arrange
            var mockView = new Mock<IView>();
            var mockModel = new Mock<IModel>();

            string testFilePath = "C:\\test\\testfile.bin";
            string expectedHexDump = "00000000  48 65 6C 6C 6F 20 57 6F 72 6C 64 21 0A        Hello World!.";

            mockView.Setup(v => v.GetFilePath()).Returns(testFilePath);
            mockModel.Setup(m => m.GenerateHexDump(testFilePath)).Returns(expectedHexDump);

            var presenter = new Presenter(mockView.Object, mockModel.Object);

            // Act
            presenter.Run();

            // Assert
            mockView.Verify(v => v.GetFilePath(), Times.Once);
            mockModel.Verify(m => m.GenerateHexDump(testFilePath), Times.Once);
            mockView.Verify(v => v.ShowHexDump(expectedHexDump), Times.Once);
            mockView.Verify(v => v.ShowError(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Run_EmptyFilePath_ShowsError()
        {
            // Arrange
            var mockView = new Mock<IView>();
            var mockModel = new Mock<IModel>();

            mockView.Setup(v => v.GetFilePath()).Returns(string.Empty);

            var presenter = new Presenter(mockView.Object, mockModel.Object);

            // Act
            presenter.Run();

            // Assert
            mockView.Verify(v => v.GetFilePath(), Times.Once);
            mockView.Verify(v => v.ShowError("Путь к файлу не может быть пустым."), Times.Once);
            mockModel.Verify(m => m.GenerateHexDump(It.IsAny<string>()), Times.Never);
            mockView.Verify(v => v.ShowHexDump(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Run_ModelThrowsException_ShowsError()
        {
            // Arrange
            var mockView = new Mock<IView>();
            var mockModel = new Mock<IModel>();

            string testFilePath = "C:\\test\\nonexistentfile.bin";
            string errorMessage = "Файл не найден.";

            mockView.Setup(v => v.GetFilePath()).Returns(testFilePath);
            mockModel.Setup(m => m.GenerateHexDump(testFilePath)).Throws(new FileNotFoundException(errorMessage));

            var presenter = new Presenter(mockView.Object, mockModel.Object);

            // Act
            presenter.Run();

            // Assert
            mockView.Verify(v => v.GetFilePath(), Times.Once);
            mockModel.Verify(m => m.GenerateHexDump(testFilePath), Times.Once);
            mockView.Verify(v => v.ShowError(errorMessage), Times.Once);
            mockView.Verify(v => v.ShowHexDump(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Run_FilePathWithWhitespace_ShowsError()
        {
            // Arrange
            var mockView = new Mock<IView>();
            var mockModel = new Mock<IModel>();

            string testFilePath = "   ";

            mockView.Setup(v => v.GetFilePath()).Returns(testFilePath);

            var presenter = new Presenter(mockView.Object, mockModel.Object);

            // Act
            presenter.Run();

            // Assert
            mockView.Verify(v => v.GetFilePath(), Times.Once);
            mockView.Verify(v => v.ShowError("Путь к файлу не может быть пустым."), Times.Once);
            mockModel.Verify(m => m.GenerateHexDump(It.IsAny<string>()), Times.Never);
            mockView.Verify(v => v.ShowHexDump(It.IsAny<string>()), Times.Never);
        }
    }
}
