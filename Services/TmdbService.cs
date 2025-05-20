using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using streamingParadigmas.Clases;

public class TmdbService
{
    //se usa HttpClient para lograr hacer peticiones a la api y IMemoryCache se usa para no hacer mas peticiones de loq ue deberiamos
    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;
    private readonly string apiKey = "fa47a3f3e8e14705697f7606fa3e61c7"; 

    public TmdbService(HttpClient http, IMemoryCache cache)
    {
        _http = http;
        _cache = cache;
    }

    //metodo para obtener la listas de las peliculas populares de la API de TMDB
    public async Task<List<Pelicula>> GetPeliculasPopularesAsync()
    {

        try {
            if (_cache.TryGetValue("peliculas_populares", out List<Pelicula> peliculasEnCache))
            {
                return peliculasEnCache;
            }

            var peliculas = new List<Pelicula>();
            int paginasNecesarias = 5;

            for (int page = 1; page <= paginasNecesarias; page++)
            {
                var url = $"https://api.themoviedb.org/3/movie/popular?api_key={apiKey}&language=es-ES&page={page}";
                var response = await _http.GetFromJsonAsync<TmdbResponse>(url);

                var nuevasPeliculas = response.results.Select(p =>
                    new Pelicula(
                        p.title,
                        (float)p.vote_average,
                        "https://image.tmdb.org/t/p/w500" + p.poster_path,
                        TimeSpan.FromMinutes(120)
                    )
                ).ToList();

                peliculas.AddRange(nuevasPeliculas);
            }

            _cache.Set("peliculas_populares", peliculas, TimeSpan.FromHours(1));
            return peliculas;
        }
        catch (Exception ex)
        {
            throw new Exception("error en la funcion de obtener peliculas populares");
        }
       
    }

    //Metodo para obtener las series mas populares de TMDB
    public async Task<List<Serie>> GetSeriesPopularesAsync()
    {
        try {
            if (_cache.TryGetValue("series_populares", out List<Serie> seriesEnCache))
            {
                return seriesEnCache;
            }

            var series = new List<Serie>();

            for (int page = 1; page <= 2; page++)
            {
                var response = await _http.GetAsync($"https://api.themoviedb.org/3/tv/popular?api_key={apiKey}&language=es-ES&page={page}");
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var results = doc.RootElement.GetProperty("results");

                foreach (var item in results.EnumerateArray())
                {
                    int id = item.GetProperty("id").GetInt32();
                    string nombre = item.GetProperty("name").GetString();
                    float calificacion = (float)item.GetProperty("vote_average").GetDouble();
                    string poster = item.GetProperty("poster_path").GetString();
                    string imagen = $"https://image.tmdb.org/t/p/w500{poster}";

                    var detalleResponse = await _http.GetAsync($"https://api.themoviedb.org/3/tv/{id}?api_key={apiKey}&language=es-ES");
                    var detalleJson = await detalleResponse.Content.ReadAsStringAsync();
                    using var detalleDoc = JsonDocument.Parse(detalleJson);
                    int temporadas = detalleDoc.RootElement.GetProperty("number_of_seasons").GetInt32();

                    var serie = new Serie(temporadas, nombre, calificacion, imagen);
                    series.Add(serie);
                }
            }

            _cache.Set("series_populares", series, TimeSpan.FromHours(1));
            return series;
        }
        catch (Exception ex)
        {
            throw new Exception("error en la funcion de obtener series populares");
        }
        
    }

    // Clases internas, se usaron para lograr hacer la peticion de la duracion
    private class TmdbResponse
    {
        public List<TmdbMovie> results { get; set; }
    }

    private class TmdbMovie
    {
        public string title { get; set; }
        public double vote_average { get; set; }
        public string poster_path { get; set; }
        public int? runtime { get; set; }
    }
}
