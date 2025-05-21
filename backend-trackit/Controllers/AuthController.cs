using backend_trackit.Context;
using backend_trackit.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_trackit.Controllers
{
    [ApiController]
    [Route("trackit/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly string __constr;
        public AuthController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("koneksi");
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

        [HttpPost("login")]
        public IActionResult loginCustomer([FromBody]LoginCustomer loginCustomer)
        {
            CustomerContext custContext = new CustomerContext(this.__constr);
            bool customerExist = custContext.checkLoginCustomer(loginCustomer.no_telepon, loginCustomer.pin);

            if (!customerExist)
            {
                return StatusCode(500, new { message = "No telepon atau pin SALAH!" });
            }

            List<Customer> user = custContext.getDataCustomerLogin(loginCustomer.no_telepon, loginCustomer.pin);
            return Ok(user);

        }
    }
}
