using Microsoft.AspNetCore.Mvc;
using backend_trackit.Models;
using backend_trackit.Context;
using backend_trackit.Helper;

namespace backend_trackit.Controllers
{
    [ApiController]
    [Route("trackit/[controller]")]
    public class KurirController : ControllerBase
    {
        public readonly string __constr;
        private readonly ICloudinaryHelper _cloudinaryHelper;

        public KurirController(IConfiguration configuration, ICloudinaryHelper cloudinaryHelper)
        {
            __constr = configuration.GetConnectionString("koneksi");
            _cloudinaryHelper = cloudinaryHelper;
        }

        [HttpGet("DataKurir/{id_kecamatan}")]
        public IActionResult getAllDataKurir(int id_kecamatan)
        {
            KurirContext kurirContext = new KurirContext(this.__constr);
            List<DropdownKurir> dataKurir = kurirContext.getDataKurir(id_kecamatan);
            return Ok(dataKurir);
        }

        [HttpGet("DataPengiriman/{id_kurir}")]
        public IActionResult getDataPengirimanByKurir(int id_kurir)
        {
            KurirContext kurirContext = new KurirContext(this.__constr);
            List<ListPengirimanKurir> dataPengiriman = kurirContext.getDataPengirimanByKurir(id_kurir);
            return Ok(dataPengiriman);
        }

        /*[HttpPut("UpdateFotoBukti")]
        public async Task<IActionResult> UpdateFotoBuktiPenyelesaian([FromForm] UpdateFotoBuktiRequest request)
        {
            PaketContext paketContext = new PaketContext(this.__constr);

            try
            {
                // Validasi input
                if (string.IsNullOrEmpty(request.NoResi))
                    return BadRequest(new UpdateFotoBuktiResponse
                    {
                        Success = false,
                        Message = "No Resi diperlukan"
                    });

                if (request.FotoBukti == null)
                    return BadRequest(new UpdateFotoBuktiResponse
                    {
                        Success = false,
                        Message = "File foto diperlukan"
                    });

                // Cek apakah paket exists dan valid untuk completion
                var isValidPaket = await paketContext.IsPaketExistsAndValidForCompletion(request.NoResi);
                if (!isValidPaket)
                    return NotFound(new UpdateFotoBuktiResponse
                    {
                        Success = false,
                        Message = "Paket tidak ditemukan atau tidak dapat diselesaikan"
                    });

                // Get current foto bukti (untuk delete old image jika ada)
                var currentFotoBukti = await paketContext.GetCurrentFotoBuktiAsync(request.NoResi);
                string? oldPublicId = null;

                if (!string.IsNullOrEmpty(currentFotoBukti) && currentFotoBukti.Contains("cloudinary"))
                {
                    // Extract publicId from URL untuk delete nanti
                    oldPublicId = ExtractPublicIdFromUrl(currentFotoBukti);
                }

                // Upload foto baru ke Cloudinary
                var uploadResult = await _cloudinaryHelper.UploadImageAsync(
                    request.FotoBukti,
                    "delivery-proof"
                );

                if (!uploadResult.Success)
                    return BadRequest(new UpdateFotoBuktiResponse
                    {
                        Success = false,
                        Message = uploadResult.Message ?? "Upload gagal"
                    });

                // Update database
                var updateSuccess = await paketContext.UpdateFotoBuktiPenyelesaianAsync(
                    request.NoResi,
                    uploadResult.ImageUrl!
                );

                if (!updateSuccess)
                    return StatusCode(500, new UpdateFotoBuktiResponse
                    {
                        Success = false,
                        Message = "Gagal update database"
                    });

                // Delete old image dari Cloudinary (jika ada)
                if (!string.IsNullOrEmpty(oldPublicId))
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await _cloudinaryHelper.DeleteImageAsync(oldPublicId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    });
                }

                return Ok(new UpdateFotoBuktiResponse
                {
                    Success = true,
                    Message = "Foto bukti penyelesaian berhasil diupdate",
                    FotoUrl = uploadResult.ImageUrl,
                    NoResi = request.NoResi
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new UpdateFotoBuktiResponse
                {
                    Success = false,
                    Message = "Terjadi kesalahan server"
                });
            }
        }

        // Helper method untuk extract publicId dari Cloudinary URL
        private string? ExtractPublicIdFromUrl(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl) || !imageUrl.Contains("cloudinary"))
                    return null;

                var uri = new Uri(imageUrl);
                var segments = uri.AbsolutePath.Split('/');
                var uploadIndex = Array.IndexOf(segments, "upload");

                if (uploadIndex >= 0 && uploadIndex + 1 < segments.Length)
                {
                    var publicIdParts = segments.Skip(uploadIndex + 2); // Skip "upload" and version
                    var publicId = string.Join("/", publicIdParts);
                    return Path.GetFileNameWithoutExtension(publicId);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }*/
    }
}
