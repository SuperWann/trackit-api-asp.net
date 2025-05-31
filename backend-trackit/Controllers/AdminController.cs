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

        [HttpPost("AcceptOrder")]
        public IActionResult processOrderAccepted(ProcessOrderAccepted orderAccepted)
        {
            AdminContext adminContext = new AdminContext(this.__constr);

            bool changeStatus = adminContext.changeStatusOrder(orderAccepted.id_order);

            if (changeStatus)
            {
                bool result = adminContext.processOrderAccepted(orderAccepted);

                if (result)
                {
                    return Ok(new { message = "BERHASIL memproses pesanan" });
                }

                return StatusCode(500, new { message = "GAGAL memproses pesanan 2" });
            }

            return StatusCode(500, new { message = "GAGAL menambahkan pesanan!" });
        }
    }
}
