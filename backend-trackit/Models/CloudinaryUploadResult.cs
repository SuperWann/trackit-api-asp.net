namespace backend_trackit.Models
{
    public class CloudinaryUploadResult
    {
        public bool Success { get; set; }
        public string? ImageUrl { get; set; }
        public string? PublicId { get; set; }
        public string? Message { get; set; }
    }

    public class ImageUploadRequest
    {
        public IFormFile File { get; set; }
        public string? Folder { get; set; } = "uploads";
        public int? CustomerId { get; set; }
        public int? KurirId { get; set; }
    }
}