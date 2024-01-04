using ImageAPI.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ImageAPI.Interfaces
{
    public interface IFileUploader
    {
        public Task<bool> UploadFile(IFormFile file, string guid, AzureStorageConfig storageConfig);
    }
}
