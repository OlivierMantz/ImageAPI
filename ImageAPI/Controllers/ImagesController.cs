using Azure.Storage;
using Azure.Storage.Blobs;
using ImageAPI.Helpers;
using ImageAPI.Interfaces;
using ImageAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ImageAPI.Controllers
{
    // Using https://github.com/Azure-Samples/storage-blob-upload-from-webapp/tree/master/ImageResizeWebApp/ImageResizeWebApp

    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly AzureStorageConfig _storageConfig;
        private readonly IFileUploader _fileUploader;

        public ImagesController(IOptions<AzureStorageConfig> config, IFileUploader fileUploader)
        {
            _storageConfig = config.Value;
            _fileUploader = fileUploader;
        }

        // POST /api/images/upload
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            try
            {
                var guid = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(file.FileName);

                var isUploaded = await _fileUploader.UploadFile(file, guid, _storageConfig);

                if (isUploaded)
                {
                    return Ok(new { Guid = guid, FileExtension = fileExtension });
                }
                else
                {
                    return BadRequest("Image upload failed.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/images/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllImages()
        {
            var storageCredentials = new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);
            var blobServiceClient = new BlobServiceClient(new Uri($"https://{_storageConfig.AccountName}.blob.core.windows.net"), storageCredentials);
            var containerClient = blobServiceClient.GetBlobContainerClient(_storageConfig.ImageContainer);

            var images = new List<string>();
            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                images.Add(blobItem.Name);
            }

            return Ok(images);
        }

        //GET /api/{guid}
        [HttpGet("{guid}")]
        public async Task<IActionResult> GetImageByGuid(string guid)
        {
            var storageCredentials = new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);
            var blobServiceClient = new BlobServiceClient(new Uri($"https://{_storageConfig.AccountName}.blob.core.windows.net"), storageCredentials);
            var containerClient = blobServiceClient.GetBlobContainerClient(_storageConfig.ImageContainer);

            var blobClient = containerClient.GetBlobClient(guid);
            if (await blobClient.ExistsAsync())
            {
                var blobDownloadInfo = await blobClient.DownloadAsync();
                return File(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
            }

            return NotFound();
        }

    }
}