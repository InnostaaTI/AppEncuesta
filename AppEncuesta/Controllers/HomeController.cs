using AppEncuesta.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppEncuesta.Controllers
{
    [Route("v1/encuesta")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpPost]

        public IActionResult Notificar([FromBody] Encuesta dato)
        {
            DAO.procesa D = new DAO.procesa();

            string rs = string.Empty;
            bool band = false;
            band = D.proceso(dato, ref rs);

            if (band)
            {
                return StatusCode(StatusCodes.Status200OK, rs);
            }
            else
            {
                return StatusCode(StatusCodes.Status502BadGateway, rs);
            }

        }
        [HttpGet]

        public IActionResult saludo()
        {

            return Ok("Hola Mundo");


        }
    }
}