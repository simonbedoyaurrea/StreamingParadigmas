using streamingParadigmas.Clases;
using Castle.DynamicProxy;

namespace PARADIGMASFINAL.Aspectos
{
    public class LoginInterceptor:IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"➡️ Interceptando: {invocation.Method.Name}");

            if (invocation.Method.Name == "IniciarSesion" && invocation.Arguments.Length > 0)
            {
                string username = invocation.Arguments[0]?.ToString();
                Console.WriteLine($"🔍 Buscando cuenta para: {username}");
            }

            try
            {
                invocation.Proceed(); 

                if (invocation.ReturnValue is Cuenta cuenta)
                {
                    if (cuenta != null)
                    {
                        Console.WriteLine($"Cuenta encontrada: {cuenta.Usuario?.Nombre}");
                    }
                    else
                    {
                        Console.WriteLine("Cuenta no encontrada.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante IniciarSesion: {ex.Message}");
                throw; 
            }
        }
    }
}
