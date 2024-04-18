using AppEncuesta.Models;

namespace AppEncuesta.DAO.Servicio
{
    public interface IEnviaCorreo<T> where T : class
    {

           public void SendEmail(T request);
        
    }
}
