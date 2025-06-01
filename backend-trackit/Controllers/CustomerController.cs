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

        [HttpGet("DataOrderProcessed/{id_customer}")]
        public IActionResult getDataOrderProcessed(int id_customer)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);
            List<OrderCustomerProcessed> orders = custContext.getDataOrderProcessedByCustomer(id_customer);

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

        [HttpGet("DataCoordinatePoint/{id_kecamatan_pengirim}/{id_kecamatan_penerima}")]
        public IActionResult getDataCoordinate(int id_kecamatan_pengirim, int id_kecamatan_penerima)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);

            List<CoordinateKecamatan> pengirim = custContext.getCoordinateKecamatan(id_kecamatan_pengirim);
            List<CoordinateKecamatan> penerima = custContext.getCoordinateKecamatan(id_kecamatan_penerima);

            var result = new
            {
                pengirim,
                penerima
            };

            return Ok(result);
        }
    }
}
