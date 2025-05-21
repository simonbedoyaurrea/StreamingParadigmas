using Microsoft.AspNetCore.Mvc;
using PARADIGMASFINAL.Services;
using streamingParadigmas.Clases;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            

            var cuentaIniciada = HttpContext.Session.GetString("CuentaSesion");
            var cuentaEncontrada = _pService.BuscarCuenta(cuentaIniciada);

            if (cuentaEncontrada == null)
            {
                Console.WriteLine("no se encontro la cuenta");
                return RedirectToAction("Registro", "Proveedor");
            }

            
            return View(cuentaEncontrada);


        }

        public IActionResult JugarJuego(string juego)
        {
            var cuentaIniciada = HttpContext.Session.GetString("CuentaSesion");

            if (string.IsNullOrEmpty(cuentaIniciada))
            {
                return RedirectToAction("Login", "Proveedor");
            }

            var cuentaEncontrada = _pService.BuscarCuenta(cuentaIniciada);
            if (cuentaEncontrada == null)
            {
                return NotFound("Cuenta no encontrada");
            }

            var juegoJugado = _pService.BuscarJuego(juego);
            if (juegoJugado == null)
            {
                return NotFound("Juego no encontrado");
            }

            

            cuentaEncontrada.JugarJuego(juegoJugado);

            TempData["Mensaje"] = $"¡Has jugado {juegoJugado.Nombre} y se ha registrado exitosamente!";

            if (!string.IsNullOrEmpty(cuentaEncontrada.NotificacionCategoria))
            {
                TempData["CategoriaMensaje"] = cuentaEncontrada.NotificacionCategoria;
                cuentaEncontrada.NotificacionCategoria = null;  


            }
            if (!string.IsNullOrEmpty(cuentaEncontrada.NotificacionRecordRoto))
            {
                TempData["RecordMensaje"] = cuentaEncontrada.NotificacionRecordRoto;
                cuentaEncontrada.NotificacionRecordRoto = null;


            }

            return RedirectToAction("Index", "Juegos");
        }

        public IActionResult VerContenido(string contenido)
        {
            Console.WriteLine(contenido);
            var cuentaIniciada = HttpContext.Session.GetString("CuentaSesion");

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

            TempData["Mensaje"] = $"¡Has visto {contenidoEncontrado.Nombre} y se ha registrado exitosamente!";

            if (!string.IsNullOrEmpty(cuentaEncontrada.NotificacionCategoria))
            {
                TempData["CategoriaMensaje"] = cuentaEncontrada.NotificacionCategoria;
                cuentaEncontrada.NotificacionCategoria = null;
            }
            if (!string.IsNullOrEmpty(cuentaEncontrada.NotificacionContenidoVisto))
            {
                TempData["ContenidoVistoMensaje"] = cuentaEncontrada.NotificacionContenidoVisto;
                cuentaEncontrada.NotificacionContenidoVisto = null;
            }

            if (contenidoEncontrado is Pelicula)
                return RedirectToAction("Index", "Peliculas");

            if (contenidoEncontrado is Serie)
                return RedirectToAction("Index", "Series");
            
            return RedirectToAction("Index", "Peliculas");


        }
    }
}
