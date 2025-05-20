using Microsoft.AspNetCore.Mvc;
using PARADIGMASFINAL.Services;

namespace PARADIGMASFINAL.Controllers
{
    public class SeriesController : Controller
    {
        
        private ProveedorService _pService;

        public SeriesController(ProveedorService pService)
        {
            _pService = pService;
        }

        //pasa la lista de series al index con el carrusel
        public async Task<IActionResult> Index()
        {
            var series = _pService.DarSerie();
            return View(series); 
        }

        //pasa la serie seleccionada y muestra sus datos
        public IActionResult Descripcion(string nombre)
        {
            var serie =_pService.BuscarSerie(nombre);

            if (serie == null)
                return NotFound();

            return View(serie);
        }
    }
}
