using Microsoft.AspNetCore.Mvc;
using PARADIGMASFINAL.Services;
using streamingParadigmas.Clases;

public class PeliculasController : Controller
{
    
    private ProveedorService _pService;


    public PeliculasController(ProveedorService pService)
    {
        _pService = pService;
    }

    public async Task<IActionResult> Index()
    {
        var peliculas = _pService.DarPeliculas();
        return View(peliculas); 
    }

    public IActionResult Descripcion(string nombre)
    {
        var pelicula = _pService.BuscarPelicula(nombre);
        

        if (pelicula == null)
            return NotFound();

        return View(pelicula);
    }
}
