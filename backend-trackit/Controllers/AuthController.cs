using backend_trackit.Context;
using backend_trackit.Helper;
using backend_trackit.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_trackit.Controllers
{
    [ApiController]
    [Route("trackit/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly string __constr;
        private readonly IConfiguration __config;
        public AuthController(IConfiguration configuration)
        {
            __config = configuration;
            __constr = __config.GetConnectionString("koneksi");
        }

        [HttpGet("{nomorTelepon}")]
        public IActionResult cariNomorTelepon (string nomorTelepon)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);
            bool customerExist = custContext.cariNomorTelepon(nomorTelepon);

            if (!customerExist)
            {
                return StatusCode(500, new { message = "TIDAK MENEMUKAN  customer dengan nomor telepon itu!" });
            }
            
            return Ok(new { message = "Customer dengan nomor telepon itu DITEMUKAN!" });
            
        }

        [HttpPost("loginCustomer")]
        public IActionResult loginCustomer(LoginCustomer loginCustomer)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);
            bool customerExist = custContext.checkLoginCustomer(loginCustomer.no_telepon, loginCustomer.pin);

            if (!customerExist)
            {
                return Unauthorized(new { message = "No telepon atau pin SALAH!" });
            }

            List<Customer> user = custContext.getDataCustomerLogin(loginCustomer.no_telepon, loginCustomer.pin);
            return Ok(user);
        }

        [HttpPost("loginPegawai")]
        public IActionResult loginPegawai([FromBody]LoginPegawai loginPegawai)
        {
            PegawaiContext pegawaiContext = new PegawaiContext(this.__constr);
            Pegawai pegawai = pegawaiContext.getPegawaiLogin(loginPegawai.email, loginPegawai.password);

            if(pegawai == null)
            {
                return Unauthorized(new { message = "Email atau password salah" });
            }

            JWThelper jwtHelper = new JWThelper(this.__config);
            var token = jwtHelper.generateTokenPegawai(pegawai);

            return Ok(new
            {
                token = token,
                pegawai
            });
        }
    }
}
