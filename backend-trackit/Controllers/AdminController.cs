using backend_trackit.Context;
using backend_trackit.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_trackit.Controllers
{
    [ApiController]
    [Route("trackit/[controller]")]
    public class AdminController : ControllerBase
    {
        public readonly string __constr;

        public AdminController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("koneksi");
        }

        [HttpGet("DataOrderNotAcceptedByKecamatan/{id_kecamatan}")]
        public IActionResult getDataOrderNotAcceptedByKecamatan(int id_kecamatan)
        {
            AdminContext adminContext = new AdminContext(this.__constr);
            List<OrderCustomer> orders = adminContext.getDataOrderNotAcceptedByKecamatan(id_kecamatan);

            return Ok(orders);
        }

        [HttpGet("DataOrderProcessedByKecamatanPengirim/{id_kecamatan_pengirim}")]
        public IActionResult getDataOrderProcessedByKecamatanPengirim(int id_kecamatan_pengirim)
        {
            AdminContext adminContext = new AdminContext(this.__constr);
            List<OrderCustomerProcessed> orders = adminContext.getDataOrderOnProcessByKecamatan(id_kecamatan_pengirim);
            return Ok(orders);
        }

        [HttpPost("AcceptOrder")]
        public IActionResult processOrderAccepted(ProcessOrderAccepted orderAccepted)
        {
            AdminContext adminContext = new AdminContext(this.__constr);

            bool changeStatus = adminContext.changeStatusOrder(orderAccepted.id_order);

            if (changeStatus)
            {
                bool accept = adminContext.processOrderAccepted(orderAccepted);

                if (accept)
                {
                    bool trackingHistory = adminContext.createTrackingHistory(orderAccepted);

                    if (trackingHistory)
                    {
                        return Ok(new { message = "BERHASIL memproses pesanan!" });
                    }

                    return StatusCode(500, new { message = "GAGAL memproses pesanan!!!" });
                }

                return StatusCode(500, new { message = "GAGAL memproses pesanan!!" });
            }

            return StatusCode(500, new { message = "GAGAL memproses pesanan!" });
        }
    }
}
