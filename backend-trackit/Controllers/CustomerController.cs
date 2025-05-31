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

        [HttpGet("DataOrderNotAccepted/{id_customer}")]
        public IActionResult getDataOrderNotAccepted(int id_customer)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);
            List<OrderCustomer> orders = custContext.getDataOrderNotAccepted(id_customer);
 
            return Ok(orders);
        }

        [HttpDelete("CancelOrder/{id_order}")]
        public IActionResult cancelDataOrder(int id_order)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);
            bool result = custContext.cancelOrder(id_order);

            if (result)
            {
                return Ok(new { message = "BERHASIL cancel order" });
            }

            return Ok(new { message = "GAGAL cancel order" });
        }
    }
}
