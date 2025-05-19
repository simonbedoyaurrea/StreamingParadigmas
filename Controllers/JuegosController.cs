using Microsoft.AspNetCore.Mvc;
using PARADIGMASFINAL.Services;
using streamingParadigmas.Clases;

namespace PARADIGMASFINAL.Controllers
{
    public class JuegosController : Controller
    {
        private ProveedorService _pService;

        public JuegosController(ProveedorService pService)
        {
            _pService = pService;
        }

        public IActionResult Index()
        {
           List<Juego> juegos= _pService.DarJuegos();
            return View(juegos);
        }
        public IActionResult Descripcion(string nombre)
        {
            var juego = _pService.BuscarJuego(nombre);


            if (juego == null)
                return NotFound();

            return View(juego);
        }
    }
}
