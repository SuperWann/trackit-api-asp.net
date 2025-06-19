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

        [HttpGet("DataOrderProcessedByResi/{no_resi}")]
        public IActionResult getDataOrderProcessedByResi(string no_resi)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);
            List<OrderCustomerProcessed> orders = custContext.getDataOrderProcessedByResi(no_resi);

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

        [HttpGet("DataTrackingHistories/{no_resi}")]
        public IActionResult getDataTrackingHistories(string no_resi)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);

            List<TrackingHistory> trackingHistories = custContext.getDataTrackingHistories(no_resi);

            return Ok(trackingHistories);
        }

        [HttpPost("AddListAlamat")]
        public IActionResult addListAlamatCustomer(AddListAlamat alamat)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);

            bool addList = custContext.createListAlamat(alamat);

            if (addList)
            {
                return Ok(new { message = "BERHASIL menambahkan alamat"});
            }

            return StatusCode(500, new { message = "GAGAL menambahkan alamat!" });
        }

        [HttpGet("DataListAlamat/{id_customer}")]
        public IActionResult getDataListAlamat(int id_customer)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);

            List<ListAlamat> listAlamat = custContext.getListAlamatByCustomer(id_customer);

            return Ok(listAlamat);
        }

        [HttpDelete("DeleteAlamat/{id_customer}/{id_alamat}")]
        public IActionResult deleteAlamat(int id_customer, int id_alamat)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);
            bool result = custContext.deleteAlamat(id_customer, id_alamat);

            if (result)
            {
                return Ok(new { message = "BERHASIL delete alamat" });
            }

            return StatusCode(500, new { message = "GAGAL delete alamat!" });
        }

        [HttpPatch("UpdateAlamat")]
        public IActionResult updateAlamat(AddListAlamat data)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);

            bool update = custContext.updateAlamat(data);

            if (update)
            {
                return Ok(new { message = "BERHASIL update alamat" });
            }

            return StatusCode(500, new { message = "GAGAL update alamat!" });
        }
    }
}
