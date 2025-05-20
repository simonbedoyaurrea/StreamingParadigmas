using Microsoft.Extensions.Caching.Memory;
using streamingParadigmas.Clases;

namespace PARADIGMASFINAL.Services
{
    public class RawgService
    {
        //se usa HttpClient para lograr hacer peticiones a la api y IMemoryCache se usa para no hacer mas peticiones de loq ue deberiamos
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly string apiKey = "99730ff97a9f4160a163ba011f41cb61";

        public RawgService(HttpClient http,IMemoryCache cache)
        {
            _http = http;
            _cache = cache;

        }
        //metodo para obtener los juegos populaes de la API de RAWG 
        public async Task<List<Juego>> GetJuegosPopularesAsync()
        {
            try {
                var juegos = new List<Juego>();

                var url = $"https://api.rawg.io/api/games?ordering=-added&page_size=50&key={apiKey}";

                var response = await _http.GetFromJsonAsync<RawgResponse>(url);

                var nuevosJuegos = response.results.Select(p =>
                  new Juego(
                            p.name,
                            p.genres[0].name,
                            (float)p.rating,
                            p.background_image

                   )
                 ).ToList();

                juegos.AddRange(nuevosJuegos);

                return juegos;
            }
            catch (Exception ex) {
                throw new Exception("error en la funcion de obtener juegos populares");
            }

            
        }


        //clases de apoyo para hacer la peticion de los generos
        private class RawgResponse
        {
            public List<RawgJuego> results { get; set; }
        }

        private class RawgJuego
        {
            public string name { get; set; }
            public double rating { get; set; }
            public string background_image { get; set; }

            public List<Genero> genres { get; set; }


        }
        public class Genero
        {
            public string name { get; set; }
        }
    }
}

