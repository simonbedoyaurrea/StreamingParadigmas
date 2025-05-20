using Microsoft.AspNetCore.Mvc;
using PARADIGMASFINAL.Services;
using streamingParadigmas.Clases;

namespace PARADIGMASFINAL.Controllers
{
    public class ProveedorController : Controller
    {

        private ProveedorService _pService;

        public ProveedorController(ProveedorService pService)
        {
            _pService = pService;
        }


        public IActionResult Index()
        {
            List<Cuenta> cuentas = _pService.MostrarCuentas();
            return View(cuentas);
        }

        public IActionResult Login()
        {

            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Registro(string username)
        {
            _pService.AgregarCuenta(username);
            HttpContext.Session.SetString("CuentaSesion", username);
            return RedirectToAction("Index","Cuentas");
        }

        [HttpPost]
        public IActionResult Login(string username)
        {
            var cuenta = _pService.IniciarSesion(username);
            if (cuenta != null)
            {
                HttpContext.Session.SetString("CuentaSesion", cuenta.Usuario.Nombre);
            }

            return RedirectToAction("Index","Cuentas");
        }



    }
}
