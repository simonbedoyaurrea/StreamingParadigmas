using System.Net.Http.Json;
using streamingParadigmas.Clases;

public class TmdbService
{
    private readonly HttpClient _http;
    private readonly string apiKey = "fa47a3f3e8e14705697f7606fa3e61c7"; // Reemplaza por tu API Key de TMDB

    public TmdbService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Pelicula>> GetPeliculasPopularesAsync()
    {
        var peliculas = new List<Pelicula>();
        int peliculasPorPagina = 20;
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
                    TimeSpan.FromMinutes(120) // valor estimado
                )
            ).ToList();

            peliculas.AddRange(nuevasPeliculas);

        }

        return peliculas;
    }


    private class TmdbResponse
    {
        public List<TmdbMovie> results { get; set; }
    }

    private class TmdbMovie
    {
        public string title { get; set; }
        public double vote_average { get; set; }
        public string poster_path { get; set; }
        public int? runtime { get; set; } // opcional, no viene en /popular
    }
}
