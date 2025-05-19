using Microsoft.AspNetCore.Mvc;
using PARADIGMASFINAL.Services;
using streamingParadigmas.Clases;

namespace PARADIGMASFINAL.Controllers
{
    public class CuentasController : Controller
    {
        private ProveedorService _pService;

        public CuentasController(ProveedorService pService)
        {
            _pService = pService;
        }
        public IActionResult Index()
        {
            List<Juego> l_juegos = new List<Juego>{

                new Juego("cuphead", "plataformas", 9.1f, "https://play-lh.googleusercontent.com/d7jNipDFeitiZWFjsZliZ-aX3fWC1rBRvvXK5wONsDfVWC9pAXotdFju5fsK_K_5U-c"),
                 new Juego("Hollow Knight", "Metroidvania", 9.5f, "https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/ncom/software/switch/70010000003208/4643fb058642335c523910f3a7910575f56372f612f7c0c9a497aaae978d3e51"),
                 new Juego("God of War", "Acción", 9.7f, "https://cdn1.epicgames.com/offer/3ddd6a590da64e3686042d108968a6b2/EGS_GodofWar_SantaMonicaStudio_S2_1200x1600-fbdf3cbc2980749091d52751ffabb7b7_1200x1600-fbdf3cbc2980749091d52751ffabb7b7"),
                 new Juego("The Last of Us", "Aventura", 9.8f, "https://static.wikia.nocookie.net/thelastofus/images/4/45/Portada_Parte_I_R_limpia.png/revision/latest?cb=20230206140350&path-prefix=es"),
                 new Juego("DOOM Eternal", "Shooter", 9.2f, "https://cdn1.epicgames.com/offer/b5ac16dc12f3478e99dcfea07c13865c/EGS_DOOMEternal_idSoftware_S1_2560x1440-06b46993a4b6c19a9e614f2dd1202215")
            };
            List<Pelicula> peliculas = new List<Pelicula>()
        {
            new Pelicula("Inception", 8.8f, "https://http2.mlstatic.com/D_NQ_NP_709095-MCO73327750064_122023-O.webp", TimeSpan.FromMinutes(148)),
            new Pelicula("The Matrix", 8.7f, "https://m.media-amazon.com/images/I/613ypTLZHsL._AC_UF894,1000_QL80_.jpg", TimeSpan.FromMinutes(136)),
            new Pelicula("Interstellar", 8.6f, "https://m.media-amazon.com/images/M/MV5BYzdjMDAxZGItMjI2My00ODA1LTlkNzItOWFjMDU5ZDJlYWY3XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg", TimeSpan.FromMinutes(169)),
            new Pelicula("The Dark Knight", 9.0f, "https://pics.filmaffinity.com/El_caballero_oscuro-628375729-large.jpg", TimeSpan.FromMinutes(152))
        };
            Usuario user = new Usuario("bhramih");


            Cuenta cuenta = new Cuenta(user);

            foreach (Juego juego in l_juegos){
                cuenta.JugarJuego(juego);
            }

            foreach (Pelicula pelicula in peliculas)
            {
                cuenta.VerContenido(pelicula);
            }

            var cuentaIniciada = HttpContext.Session.GetString("cuentaActiva");
            var cuentaEncontrada = _pService.BuscarCuenta(cuentaIniciada);

            //return View(cuentaEncontrada);
            return View(cuentaEncontrada);


        }

        public IActionResult JugarJuego(string nombreJuego)
        {
            var cuentaIniciada = HttpContext.Session.GetString("cuentaActiva");

            if (string.IsNullOrEmpty(cuentaIniciada))
            {
                return RedirectToAction("Login", "Proveedor");
            }

            var cuentaEncontrada = _pService.BuscarCuenta(cuentaIniciada);
            if (cuentaEncontrada == null)
            {
                return NotFound("Cuenta no encontrada");
            }

            var juegoJugado = _pService.BuscarJuego(nombreJuego);
            if (juegoJugado == null)
            {
                return NotFound("Juego no encontrado");
            }

            cuentaEncontrada.JugarJuego(juegoJugado);

            return RedirectToAction("Index", "Proveedor");
        }

        public IActionResult VerContenido(string contenido)
        {
            Console.WriteLine(contenido);
            var cuentaIniciada = HttpContext.Session.GetString("cuentaActiva");

            if (string.IsNullOrEmpty(cuentaIniciada))
            {
                return RedirectToAction("Login", "Proveedor");
            }

            var cuentaEncontrada = _pService.BuscarCuenta(cuentaIniciada);
            if (cuentaEncontrada == null)
            {
                return NotFound("Cuenta no encontrada");
            }

            var contenidoEncontrado = _pService.BuscarContenido(contenido);

            
           
            cuentaEncontrada.VerContenido(contenidoEncontrado);

            return RedirectToAction("Index", "Peliculas");
        }
    }
}
