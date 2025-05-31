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

        [HttpGet("DataKurir")]
        public IActionResult getAllDataKurir()
        {
            KurirContext kurirContext = new KurirContext(this.__constr);
            List<Kurir> dataKurir = kurirContext.getDataKurir();
            return Ok(dataKurir);

        }
    }
}
