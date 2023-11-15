using ImageAPI.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ImageAPI.Interfaces
{
    public class TestFileUploader : IFileUploader
    {
        private bool result;
        public async Task<bool> UploadFile(IFormFile formFile, AzureStorageConfig storageConfig)
        {
            return result;
        }

        public TestFileUploader(bool result)
        {
            this.result = result;
        }

    }
}