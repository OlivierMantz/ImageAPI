using Xunit;
using ImageAPI.Interfaces;
using ImageAPI.Models;
using Microsoft.Extensions.Options;
using ImageAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Moq;
using Azure.Storage.Blobs;

namespace ImageAPITests.Controllers.Tests
{
    public class ImageControllerTests
    {
        [Fact()]
        public async Task Upload()
        {
            // Arrange 
            // Mock IFormFile setup
            var fileName = "test.jpg";
            var content = "Fake image content";
            var contentType = "image/jpeg";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(_ => _.FileName).Returns(fileName);
            formFileMock.Setup(_ => _.ContentType).Returns(contentType);
            formFileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            formFileMock.Setup(_ => _.Length).Returns(stream.Length);

            var file = formFileMock.Object;

            var someOptions = Options.Create(new AzureStorageConfig());

            var controller = new ImagesController(someOptions, new TestFileUploader(true));

            // Act
            var result = await controller.Upload(file);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        //[Fact()]
        //public async Task GetThumbnails()
        //{
        //    // Arrange 
        //    var someOptions = Options.Create(new AzureStorageConfig());

        //    var controller = new ImagesController(someOptions, new TestFileUploader(true));

        //    // Act
        //    var result = await controller.GetThumbNails();

        //    // Assert
        //    Assert.IsType<AcceptedAtActionResult>(result);

        //    //var okResult = Assert.IsType<OkObjectResult>(result);
        //    //var thumbnailUrls = Assert.IsAssignableFrom<List<string>>(okResult.Value);


        //}
    }
}
