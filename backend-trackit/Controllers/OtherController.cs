using backend_trackit.Context;
using backend_trackit.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_trackit.Controllers
{
    [ApiController]
    [Route("trackit/[controller]")]
    public class OtherController : ControllerBase
    {
        public readonly string __constr;
        public OtherController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("koneksi");
        }

        [HttpGet("DataJenisPaket")]
        public IActionResult getDataJenisPaket()
        {
            OtherContext otherContext = new OtherContext(this.__constr);

            List<JenisPaket> dataJenis = otherContext.getDataJenisPaket();

            if(dataJenis.Count > 0)
            {
                return Ok(dataJenis);
            }

            return BadRequest();
        }

        [HttpGet("DataKecamatanByIdKabupaten/{id_kabupaten}")]
        public IActionResult getDataKecamatanByIdKabupaten(int id_kabupaten)
        {
            OtherContext otherContext = new OtherContext(this.__constr);

            List<Kecamatan> dataKecamatan = otherContext.GetDataKecamatanByIdKabupaten(id_kabupaten);

            if (dataKecamatan.Count > 0)
            {
                return Ok(dataKecamatan);
            }

            return BadRequest();
        }

        [HttpGet("DataKecamatan/{nama_kabupaten}")]
        public IActionResult getDataKecamatan(string nama_kabupaten)
        {
            OtherContext otherContext = new OtherContext(this.__constr);

            List<Kecamatan> dataKecamatan = otherContext.GetDataKecamatanByKabupaten(nama_kabupaten);

            if(dataKecamatan.Count > 0)
            {
                return Ok(dataKecamatan);
            }

            return BadRequest();
        }


        [HttpGet("DataStatusPaket")]
        public IActionResult getDataStatusPaket()
        {
            OtherContext otherContext = new OtherContext(this.__constr);

            List<StatusPaket> dataStatus = otherContext.GetDataStatusPaket();

            if (dataStatus.Count > 0)
            {
                return Ok(dataStatus);
            }

            return BadRequest();
        }

        [HttpGet("DataKabupaten")]
        public IActionResult getAllKabupaten()
        {
            OtherContext othercontext = new OtherContext(this.__constr);

            List<Kabupaten> listKabupaten = othercontext.getAllkabupaten();

            if (listKabupaten.Count > 0)
            {
                return Ok(listKabupaten);
            }

            return BadRequest();
        }
    }
}
