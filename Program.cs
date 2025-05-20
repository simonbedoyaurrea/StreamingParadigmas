using PARADIGMASFINAL.Services;
using streamingParadigmas.Clases;

namespace PARADIGMASFINAL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


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
                var proveedorServicio = scope.ServiceProvider.GetRequiredService<ProveedorService>();
                var tmdbService = scope.ServiceProvider.GetRequiredService<TmdbService>();
                var rawgService = scope.ServiceProvider.GetRequiredService<RawgService>();

                
               
                List<Pelicula> peliculasTexto=proveedorServicio.CargarPeliculasDesdeArchivo("./peliculas.txt");
                


                // Agregar juegos
                var juegosRawg = rawgService.GetJuegosPopularesAsync().GetAwaiter().GetResult();
                foreach (Juego juego in juegosRawg)
                {
                    proveedor.AgregarCatalogoJuego(juego);
                }

                // Agregar pel�culas desde la API
                var peliculas = tmdbService.GetPeliculasPopularesAsync().GetAwaiter().GetResult();
                foreach (var pelicula in peliculas)
                {
                    proveedor.AgregarCatalogoContenido(pelicula);
                }
                foreach (var pelicula in peliculasTexto)
                {
                    proveedor.AgregarCatalogoContenido(pelicula);
                }

                var series = tmdbService.GetSeriesPopularesAsync().GetAwaiter().GetResult();
                foreach (var serie in series)
                {
                    proveedor.AgregarCatalogoContenido(serie);
                }
            }

            // Configuraci�n
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
