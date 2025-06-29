using backend_trackit.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace backend_trackit.Helper
{
    public interface ICloudinaryHelper
    {
        Task<CloudinaryUploadResult> UploadImageAsync(IFormFile file, string folder = "uploads");
        Task<bool> DeleteImageAsync(string publicId);
        string GetOptimizedUrl(string imageUrl, int width = 800, int height = 600);
    }

    public class CloudinaryHelper : ICloudinaryHelper
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryHelper> _logger;

        public CloudinaryHelper(IConfiguration config, ILogger<CloudinaryHelper> logger)
        {
            _logger = logger;

            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<CloudinaryUploadResult> UploadImageAsync(IFormFile file, string folder = "uploads")
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new CloudinaryUploadResult
                    {
                        Success = false,
                        Message = "File tidak valid"
                    };
                }

                // Validasi file type
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
                if (!allowedTypes.Contains(file.ContentType.ToLower()))
                {
                    return new CloudinaryUploadResult
                    {
                        Success = false,
                        Message = "Format file tidak didukung. Gunakan JPG, PNG, atau WebP"
                    };
                }

                // Validasi ukuran file (max 5MB)
                if (file.Length > 5 * 1024 * 1024)
                {
                    return new CloudinaryUploadResult
                    {
                        Success = false,
                        Message = "Ukuran file terlalu besar. Maksimal 5MB"
                    };
                }

                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = $"trackit/{folder}",
                    UseFilename = false,
                    UniqueFilename = true,
                    Overwrite = false,
                    /*Quality = "auto:good",
                    FetchFormat = "auto"*/
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new CloudinaryUploadResult
                    {
                        Success = true,
                        ImageUrl = uploadResult.SecureUrl?.ToString(),
                        PublicId = uploadResult.PublicId,
                        Message = "Upload berhasil"
                    };
                }
                else
                {
                    _logger.LogError($"Cloudinary upload failed: {uploadResult.Error?.Message}");
                    return new CloudinaryUploadResult
                    {
                        Success = false,
                        Message = "Upload gagal"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading to Cloudinary");
                return new CloudinaryUploadResult
                {
                    Success = false,
                    Message = "Terjadi kesalahan saat upload"
                };
            }
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            try
            {
                if (string.IsNullOrEmpty(publicId))
                    return false;

                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                return result.Result == "ok";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting image with publicId: {publicId}");
                return false;
            }
        }

        public string GetOptimizedUrl(string imageUrl, int width = 800, int height = 600)
        {
            if (string.IsNullOrEmpty(imageUrl) || !imageUrl.Contains("cloudinary"))
                return imageUrl;

            try
            {
                // Extract public ID from Cloudinary URL
                var uri = new Uri(imageUrl);
                var segments = uri.AbsolutePath.Split('/');
                var publicIdIndex = Array.IndexOf(segments, "upload") + 1;

                if (publicIdIndex > 0 && publicIdIndex < segments.Length)
                {
                    var publicId = string.Join("/", segments.Skip(publicIdIndex));
                    publicId = Path.GetFileNameWithoutExtension(publicId);

                    var transformation = new Transformation()
                        .Width(width)
                        .Height(height)
                        .Crop("fill")
                        .Quality("auto:good")
                        .FetchFormat("auto");

                    return _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to optimize image URL");
            }

            return imageUrl;
        }
    }
}