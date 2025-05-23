using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Exchanger.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Exchanger.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchanger.Tests.CloudinaryTests
{
    public class CloudinaryServiceTest
    {
        private IFormFile CreateFakeFormFile(string fileName = "test.png", string contentType = "image/png")
        {
            var content = new byte[10]; // пустий масив – лише для стріму
            var stream = new MemoryStream(content);
            return new FormFile(stream, 0, content.Length, "Data", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        [Fact]
        public async Task ShouldUploadToCloudListOfImages()
        {
            // 1) Мок Cloudinary
            var cloudinaryMock = new Mock<ICloudinaryClient>();

            var fakeUploadResult = new ImageUploadResult
            {
                SecureUrl = new Uri("http://res.cloudinary.com/fake/image.jpg"),
                PublicId = "some_public_id"
            };

            // Тут важливо мокнути одно-аргументний UploadAsync
            cloudinaryMock
                .Setup(c => c.UploadAsync(
                    It.IsAny<ImageUploadParams>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeUploadResult);


            var service = new CloudinaryService(cloudinaryMock.Object);

            // 2) Фейкові файли
            var userId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var files = new List<IFormFile>
        {
            CreateFakeFormFile("a.png", "image/png"),
            CreateFakeFormFile("b.jpg", "image/jpeg")
        };

            // 3) Виклик тестованого методу
            var results = await service.UploadListingImagesToCloudAsync(files, userId, listingId);

            // 4) Ассерти
            results.Length.Should().Be(files.Count);

            foreach (var result in results)
            {
                result.IsSuccess.Should().BeTrue();
                result.UploadResult.Should().Be(fakeUploadResult);
            }

            // Переконатися, що виклик відбувся рівно для кожного файлу
            cloudinaryMock.Verify(c => c.UploadAsync(
                    It.IsAny<ImageUploadParams>(),
                    It.IsAny<CancellationToken>()), Times.Exactly(files.Count));
        }
    }
}
