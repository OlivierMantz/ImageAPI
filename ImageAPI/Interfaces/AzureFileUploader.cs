using ImageAPI.Helpers;
using ImageAPI.Models;

namespace ImageAPI.Interfaces
{
    public class AzureFileUploader : IFileUploader
    {
        public async Task<bool> UploadFile(IFormFile formFile, AzureStorageConfig storageConfig)
        {
            if (formFile.Length > 0)
            {
                using Stream stream = formFile.OpenReadStream();
                var isUploaded = await StorageHelper.UploadFileToStorage(stream, formFile.FileName, storageConfig);
                return isUploaded;
            }
            return false;
        }
    }
}
