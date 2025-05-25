using backend_trackit.Context;
using backend_trackit.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_trackit.Controllers
{
    [ApiController]
    [Route("trackit/[controller]")]
    public class CustomerController : ControllerBase
    {
        public readonly string __constr;
        public CustomerController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("koneksi");
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrderCustomer(OrderCustomer order)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);

            bool result = custContext.orderPengiriman(order);

            if (result)
            {
                return Ok(new { message = "BERHASIL menambahkan pesanan" });
            }

            return StatusCode(500, new { message = "GAGAL menambahkan pesanan!" });
        }
    }
}
