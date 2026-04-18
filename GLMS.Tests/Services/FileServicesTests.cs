using GLMS.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace GLMS.Tests.Services
{
    // These tests check that only PDF files are accepted
    public class FileServiceTests
    {
        [Fact]
        public void IsPdf_WithPdfFile_ShouldReturnTrue()
        {
            // Arrange
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var service = new FileService(mockEnvironment.Object);

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("agreement.pdf");

            // Act
            bool result = service.IsPdf(fileMock.Object);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPdf_WithExeFile_ShouldReturnFalse()
        {
            // Arrange
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var service = new FileService(mockEnvironment.Object);

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("malware.exe");

            // Act
            bool result = service.IsPdf(fileMock.Object);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPdf_WithEmptyFileName_ShouldReturnFalse()
        {
            // Arrange
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var service = new FileService(mockEnvironment.Object);

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("");

            // Act
            bool result = service.IsPdf(fileMock.Object);

            // Assert
            Assert.False(result);
        }
    }
}