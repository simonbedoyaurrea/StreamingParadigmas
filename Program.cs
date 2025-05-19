using PARADIGMASFINAL.Services;
using streamingParadigmas.Clases;

namespace PARADIGMASFINAL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            List<Juego> juegos = new List<Juego>() {
                new Juego("The Last of Us", "Zombies", 9.5f, "tlou.png"),
                new Juego("God of War", "Acción", 9.8f, "gow.png"),
                new Juego("FIFA 24", "Deportes", 8.2f, "fifa24.png"),
                new Juego("Minecraft", "Aventura", 9.0f, "minecraft.png"),
                new Juego("Valorant", "Shooter", 8.7f, "valorant.png"),
                new Juego("Cyberpunk 2077", "RPG", 7.9f, "cyberpunk.png"),
                new Juego("Fortnite", "Battle Royale", 8.4f, "fortnite.png"),
                new Juego("Elden Ring", "RPG", 9.6f, "eldenring.png"),
                new Juego("League of Legends", "MOBA", 8.5f, "lol.png"),
                new Juego("Red Dead Redemption 2", "Western", 9.7f, "rdr2.png")
            };

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient<TmdbService>();
            builder.Services.AddHttpClient<RawgService>();
            builder.Services.AddSingleton<Proveedor>();
            builder.Services.AddSingleton<ProveedorService>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddMemoryCache();
            builder.Services.AddSession();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var proveedor = scope.ServiceProvider.GetRequiredService<Proveedor>();
                var tmdbService = scope.ServiceProvider.GetRequiredService<TmdbService>();
                var rawgService = scope.ServiceProvider.GetRequiredService<RawgService>();

                // Agregar juegos
                var juegosRawg = rawgService.GetJuegosPopularesAsync().GetAwaiter().GetResult();
                foreach (Juego juego in juegosRawg)
                {
                    proveedor.AgregarCatalogoJuego(juego);
                }

                // Agregar películas desde la API
                var peliculas = tmdbService.GetPeliculasPopularesAsync().GetAwaiter().GetResult();
                foreach (var pelicula in peliculas)
                {
                    proveedor.AgregarCatalogoContenido(pelicula);
                }

                var series = tmdbService.GetSeriesPopularesAsync().GetAwaiter().GetResult();
                foreach (var serie in series)
                {
                    proveedor.AgregarCatalogoContenido(serie);
                }
            }

            // Configuración
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
