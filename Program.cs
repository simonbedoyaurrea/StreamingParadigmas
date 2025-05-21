using Castle.DynamicProxy;
using PARADIGMASFINAL.Aspectos;
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

            //// Interceptor
            //builder.Services.AddSingleton<LoginInterceptor>();

            //// Servicio proxy con Castle
            //builder.Services.AddSingleton<IProveedorService>(provider =>
            //{
            //    var proxyGenerator = new ProxyGenerator();
            //    var interceptor = provider.GetRequiredService<LoginInterceptor>();
            //    var realService = new ProveedorService();
            //    return proxyGenerator.CreateInterfaceProxyWithTarget<IProveedorService>(realService, interceptor);
            //});

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

                // agregar películas desde la api y con el .txt
                var peliculas = tmdbService.GetPeliculasPopularesAsync().GetAwaiter().GetResult();
                foreach (var pelicula in peliculas)
                {
                    proveedor.AgregarCatalogoContenido(pelicula);
                }
                foreach (var pelicula in peliculasTexto)
                {
                    proveedor.AgregarCatalogoContenido(pelicula);
                }

                //agregar series desde la api
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
