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

        [HttpGet("DataOrderProcessedByKecamatan/{id_kecamatan}")]
        public IActionResult getDataOrderProcessedByKecamatanPengirim(int id_kecamatan)
        {
            AdminContext adminContext = new AdminContext(this.__constr);
            List<OrderCustomerProcessed> orders = adminContext.getDataOrderOnProcessByKecamatan(id_kecamatan);
            return Ok(orders);
        }

        [HttpGet("DataOrderProcessedByResi/{no_resi}")]
        public IActionResult getDataOrderProcessedByResi(string no_resi)
        {
            AdminContext adminContext = new AdminContext(this.__constr);
            List<OrderCustomerProcessed> orders = adminContext.getDataOrderProcessedByResi(no_resi);

            return Ok(orders);
        }

        [HttpPost("OrderAcceptedGudangKecamatanPengirim")]
        public IActionResult orderAcceptedGudangKecamatanPengirim(OrderAccepted orderAccepted)
        {
            AdminContext adminContext = new AdminContext(this.__constr);

            bool changeStatus = adminContext.changeStatusOrder(orderAccepted.id_order);

            if (changeStatus)
            {
                bool accept = adminContext.processOrderAccepted(orderAccepted);

                if (accept)
                {
                    bool trackingHistory = adminContext.createTrackingHistoryAccepted(orderAccepted);

                    if (trackingHistory)
                    {
                        return Ok(new { message = "BERHASIL memproses pesanan!" });
                    }

                    return StatusCode(500, new { message = "GAGAL memproses pesanan 3!!!" });
                }

                return StatusCode(500, new { message = "GAGAL memproses pesanan 2!!" });
            }

            return StatusCode(500, new { message = "GAGAL memproses pesanan 1!" });
        }

        [HttpPost("OrderSendKecamatanPenerima")]
        public IActionResult orderSendKecamatanPenerima(OrderProcessed orderProcessed)
        {
            AdminContext adminContext = new AdminContext(this.__constr);

            bool changeStatus = adminContext.changeStatusPaket(orderProcessed.no_resi, 2);

            if (changeStatus)
            {
                bool trackingHistory = adminContext.createTrackingHistoryAfterAccepted(orderProcessed);

                if (trackingHistory)
                {
                    return Ok(new { message = "BERHASIL memproses pesanan!" });
                }

                return StatusCode(500, new { message = "GAGAL memproses pesanan!!!" });
            }

            return StatusCode(500, new { message = "GAGAL memproses pesanan!!!" });
        }

        [HttpPost("OrderAcceptedGudangKecamatanPenerima")]
        public IActionResult orderAcceptedGudangKecamatanPenerima(OrderSendKecamatanPenerima orderProcessed)
        {
            AdminContext adminContext = new AdminContext(this.__constr);

            bool changeStatus = adminContext.changeStatusPaket(orderProcessed.no_resi, 3);

            if (changeStatus)
            {
                bool changeKurir = adminContext.changeKurirPaket(orderProcessed.no_resi, orderProcessed.id_kurir);

                if (changeKurir)
                {
                    bool trackingHistory = adminContext.createTrackingHistorySendKecamatanPenerima(orderProcessed);

                    if (trackingHistory)
                    {
                        return Ok(new { message = "BERHASIL memproses pesanan!" });
                    }

                    return StatusCode(500, new { message = "GAGAL memproses pesanan 3!!!" });
                }

                return StatusCode(500, new { message = "GAGAL memproses pesanan 2!!!" });
            }

            return StatusCode(500, new { message = "GAGAL memproses pesanan 1!!!" });
        }

        [HttpPost("OrderSendAlamatPenerima")]
        public IActionResult orderSendAlamatPenerima(OrderProcessed orderProcessed)
        {
            AdminContext adminContext = new AdminContext(this.__constr);

            bool changeStatus = adminContext.changeStatusPaket(orderProcessed.no_resi, 4);

            if (changeStatus)
            {
                bool trackingHistory = adminContext.createTrackingHistoryAfterAccepted(orderProcessed);

                if (trackingHistory)
                {
                    return Ok(new { message = "BERHASIL memproses pesanan!" });
                }
            }

            return StatusCode(500, new { message = "GAGAL memproses pesanan!!!" });
        }

        [HttpPost("CreateAkunKurirKecamatan")]
        public IActionResult createAkunKurirKecamatan(DataKurir data)
        {
            AdminContext adminContext = new AdminContext(this.__constr);

            bool createAkun = adminContext.createAkunKurir(data);

            if (createAkun)
            {
                return Ok(new { message = "BERHASIL menambahkan akun kurir" });
            }

            return StatusCode(500, new { message = "GAGAL menambahkan akun kurir!" });
        }

        [HttpGet("DataKurirKecamatan/{id_kecamatan}")]
        public IActionResult getDataKurirKecamatan(int id_kecamatan)
        {
            AdminContext adminContext = new AdminContext(this.__constr);
            List<DataKurir> dataKurirs = adminContext.getDataKurirKecamatan(id_kecamatan);

            return Ok(dataKurirs);  
        }

        [HttpDelete("DeleteDataKurir/{id_kurir}")]
        public IActionResult deleteDataKurir(int id_kurir)
        {
            AdminContext adminContext = new AdminContext(this.__constr);

            bool deleteAkun = adminContext.deleteAkunKurir(id_kurir);

            if (deleteAkun)
            {
                return Ok(new { message = "BERHASIL menghapus akun kurir" });
            }

            return StatusCode(500, new { message = "GAGAL menghapus akun kurir!" });
        }
    }
}
