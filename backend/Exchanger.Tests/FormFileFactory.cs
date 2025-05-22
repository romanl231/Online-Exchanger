using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchanger.Tests
{
    public static class FormFileFactory
    {
        public static IFormFile CreateFakeFormFile(string fileName = "test.png", string contentType = "image/png")
        {
            var content = new byte[10]; // пустий масив – лише для стріму
            var stream = new MemoryStream(content);
            return new FormFile(stream, 0, content.Length, "Data", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}
