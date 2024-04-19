using System.Data.SqlClient;

namespace AppEncuesta.DAO
{
    public class Conexion
    {

        public string traeConexion()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }

        public string traeConexionSegmento(string Codhotel)
        {
           

            string conexion = traeConexion();
            string valor = "";
            string query = $"SELECT LineaConexion  FROM Conexion_Segmentos WHERE SEG = '" + Codhotel + "'";

            using (SqlConnection connection = new SqlConnection(conexion))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        valor = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                }
            }

            return valor;
        }

    }
}
