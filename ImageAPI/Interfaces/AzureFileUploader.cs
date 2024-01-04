using ImageAPI.Helpers;
using ImageAPI.Models;

namespace ImageAPI.Interfaces
{
    public class AzureFileUploader : IFileUploader
    {
        public async Task<bool> UploadFile(IFormFile formFile, string guid, AzureStorageConfig storageConfig)
        {
            if (formFile.Length > 0)
            {
                string fileExtension = Path.GetExtension(formFile.FileName);
                string fileName = guid + fileExtension;

                using Stream stream = formFile.OpenReadStream();
                var isUploaded = await StorageHelper.UploadFileToStorage(stream, fileName, storageConfig);
                return isUploaded;
            }
            return false;
        }
    }
}
