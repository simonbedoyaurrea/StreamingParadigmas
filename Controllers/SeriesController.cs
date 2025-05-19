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

        public async Task<IActionResult> Index()
        {
            var series = _pService.DarSerie();
            return View(series); // pasa la lista a la vista
        }

        public IActionResult Descripcion(string nombre)
        {
            var serie =_pService.BuscarSerie(nombre);

            if (serie == null)
                return NotFound();

            return View(serie);
        }
    }
}
