using streamingParadigmas.Clases;

namespace PARADIGMASFINAL.Services
{
    public class ProveedorService
    {
        private Proveedor _proveedor;
        public List<Cuenta> l_cuentas;

        public ProveedorService(Proveedor proveedor)
        {
            _proveedor = proveedor;
            l_cuentas = new List<Cuenta>();

        }

        public List<Pelicula> DarPeliculas()
        {
            List<Pelicula> peliculas = _proveedor.L_catalogoContenido.OfType<Pelicula>().ToList();
            return peliculas;
           
        }
        public List<Juego> DarJuegos()
        {
            List<Juego> juegos = _proveedor.L_catalogoJuegos;
            return juegos;

        }
        public Pelicula BuscarPelicula(string nombre)
        {
            Pelicula pelicula = _proveedor.L_catalogoContenido.OfType<Pelicula>().ToList().Find(p => p.Nombre == nombre);
            return pelicula;

        }
        public Serie BuscarSerie(string nombre)
        {
            Serie serie = _proveedor.L_catalogoContenido.OfType<Serie>().ToList().Find(s => s.Nombre == nombre);
            return serie;

        }

        public Contenido BuscarContenido(string nombre)
        {
            Contenido cont = _proveedor.L_catalogoContenido.Find(c => c.Nombre == nombre);
            return cont;

        }

        public List<Serie> DarSerie()
        {
            List<Serie> series = _proveedor.L_catalogoContenido.OfType<Serie>().ToList();
            return series;

        }

        public Cuenta BuscarCuenta(string nombreUsuario)
        {
            return l_cuentas.Find(c => c.Usuario.Nombre == nombreUsuario);

        }

        public Juego BuscarJuego(string nombreJuego)
        {

            return _proveedor.L_catalogoJuegos.Find(j => j.Nombre == nombreJuego);
        }

       


        public List<Cuenta> MostrarCuentas()
        {
            return l_cuentas;
        }

        public void AgregarCuenta(string nombre)
        {
            //_proveedor.AgregarCuenta(nombre);
            Usuario usu = new Usuario(nombre);
            Cuenta cu = new Cuenta(usu);
            l_cuentas.Add(cu);
        }

        public Cuenta IniciarSesion(string nombre)
        {

            try
            {
                Cuenta cuentaBuscada = l_cuentas.Find(c => c.Usuario.Nombre == nombre);
                return cuentaBuscada;
            }
            catch
            {
                throw new Exception("cuenta no encontrada");
            }

        }
    }
}
