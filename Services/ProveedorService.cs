using streamingParadigmas.Clases;

namespace PARADIGMASFINAL.Services
{
    public class ProveedorService
    {
        private Proveedor _proveedor;
        public List<Cuenta> l_cuentas;


        //se inyecta un proveedor (está con singleton) y se inicializa la lista de cuentas
        public ProveedorService(Proveedor proveedor)
        {
            _proveedor = proveedor;
            l_cuentas = new List<Cuenta>();

        }

        //metodos para devolver listas (pelicula, serie o juego)

        public List<Pelicula> DarPeliculas()
        {
            try {
                List<Pelicula> peliculas = _proveedor.L_catalogoContenido.OfType<Pelicula>().ToList();
                return peliculas;
            }
            catch(Exception ex)
            {
                throw new Exception("lista de  peliculas no dispobible  o error en la funcion DarPeliculas"+ ex.Message);
            }
           
           
        }
        public List<Juego> DarJuegos()
        {
            
            try {
                List<Juego> juegos = _proveedor.L_catalogoJuegos;
                return juegos;
            }
            catch (Exception ex)
            {
                throw new Exception("lista de juegos no encontrados  o error en la funcion DarJuegos" + ex.Message);
            }

        }
        public List<Serie> DarSerie()
        {
            try {
                List<Serie> series = _proveedor.L_catalogoContenido.OfType<Serie>().ToList();
                return series;
            }
            
             catch (Exception ex)
            {
                throw new Exception("lista de series no disponible  o error en la funcion DarSerie" + ex.Message);
            }
        }

        //metodos para buscar un solo tipo de contenido o juego

        public Pelicula BuscarPelicula(string nombre)
        {
            
            try {
                Pelicula pelicula = _proveedor.L_catalogoContenido.OfType<Pelicula>().ToList().Find(p => p.Nombre == nombre);
                return pelicula;
            }
            catch (Exception ex)
            {
                throw new Exception("pelicula no encontrada o error en la funcion BuscarPelicula" + ex.Message);
            }

        }
        public Serie BuscarSerie(string nombre)
        {
            try {
                Serie serie = _proveedor.L_catalogoContenido.OfType<Serie>().ToList().Find(s => s.Nombre == nombre);
                return serie;
            }
            catch (Exception ex)
            {
                throw new Exception("serie no encontrada o error en la funcion BuscarSerie" + ex.Message);
            }


        }

        public Contenido BuscarContenido(string nombre)
        {
            
            
            try {
                Contenido cont = _proveedor.L_catalogoContenido.Find(c => c.Nombre == nombre);
                return cont;
            }
            catch (Exception ex)
            {
                throw new Exception("contenido no encontrado o error en la funcion BuscarContenido" + ex.Message);
            }
        }

        public Juego BuscarJuego(string nombreJuego)
        {
            try {
                return _proveedor.L_catalogoJuegos.Find(j => j.Nombre == nombreJuego);
            }
            catch (Exception ex)
            {
                throw new Exception("Juego no encontrado o error en la funcion BuscarJuego" + ex.Message);
            }
        }


        //meotodos para las cuentas buscar,mostrar y agregar

        public Cuenta BuscarCuenta(string nombreUsuario)
        {
            try{
                return l_cuentas.Find(c => c.Usuario.Nombre == nombreUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Cuenta no encontrada o error en la funcion BuscarCuenta" + ex.Message);
            }
        }

       

        public List<Cuenta> MostrarCuentas()
        {
            try {
                return l_cuentas;
            }
            catch (Exception ex)
            {
                throw new Exception("lista de cuentas no disponible  o error en la funcion MostrarCuentas" + ex.Message);
            }

        }

        public bool AgregarCuenta(string nombre)
        {
            try {

                bool cuentaExistente = l_cuentas.Any(c => c.Usuario.Nombre == nombre);

                if (cuentaExistente)
                    return false;
                
                Usuario usu = new Usuario(nombre);
                Cuenta cu = new Cuenta(usu);
                l_cuentas.Add(cu);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(" error en la funcion AgregarCuenta" + ex.Message);
            }
        }

        public Cuenta IniciarSesion(string nombre)
        {

            try
            {
                Cuenta cuentaBuscada = l_cuentas.Find(c => c.Usuario.Nombre == nombre);
                return cuentaBuscada;
            }
            catch(Exception ex)
            {
                throw new Exception("cuenta no encontrada"+ex.Message);
            }

        }
        public List<Pelicula> CargarPeliculasDesdeArchivo(string rutaArchivo)
        {
            var peliculas = new List<Pelicula>();

            foreach (var linea in File.ReadAllLines(rutaArchivo))
            {
                var partes = linea.Split('|');

                if (partes.Length == 4)
                {
                    string nombre = partes[0].Trim();
                    float calificacion = float.Parse(partes[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                    string imagen = partes[2].Trim();
                    TimeSpan duracion = TimeSpan.Parse(partes[3].Trim());

                    var pelicula = new Pelicula(nombre, calificacion, imagen, duracion);
                    peliculas.Add(pelicula);
                }
            }

            return peliculas;
        }

    }
}
