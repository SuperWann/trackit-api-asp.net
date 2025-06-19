using Microsoft.AspNetCore.Mvc;
using backend_trackit.Models;
using backend_trackit.Context;

namespace backend_trackit.Controllers
{
    [ApiController]
    [Route("trackit/[controller]")]
    public class KurirController : ControllerBase
    {
        public readonly string __constr;

        public KurirController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("koneksi");
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
    }
}
