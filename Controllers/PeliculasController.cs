using Microsoft.AspNetCore.Mvc;
using streamingParadigmas.Clases;

public class PeliculasController : Controller
{
    private readonly TmdbService _tmdbService;

    public PeliculasController(TmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    public async Task<IActionResult> Index()
    {
        var peliculas = await _tmdbService.GetPeliculasPopularesAsync();
        return View(peliculas); // pasa la lista a la vista
    }

    public async Task<IActionResult> Descripcion(string nombre)
    {
        var peliculas = await _tmdbService.GetPeliculasPopularesAsync();
        var pelicula = peliculas.FirstOrDefault(p => p.Nombre == nombre);

        if (pelicula == null)
            return NotFound();

        return View(pelicula);
    }
}
