namespace ImageAPI.Models
{
    public class ImageUploadDTO
    {
        public IFormFile File { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
