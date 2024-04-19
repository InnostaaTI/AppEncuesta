using AppEncuesta.DAO.Servicio;
using AppEncuesta.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace AppEncuesta.DAO
{
    public class procesa 
    {

      

        public bool proceso(Encuesta of, ref string rsp)
        {
         
            Models.Correo datosCorreo = new Models.Correo();

            Servicio.EnviaCorreo enviaCorreo = new Servicio.EnviaCorreo();
            DAO.Conexion conexion = new DAO.Conexion();
            string hotel = of.idPropiedad.ToString() == "1" ? "BOUTIQUE HOTEL" : "APARTHOTEL";
            string correo = traeCorreo(of.idPropiedad, of.Folio);
            datosCorreo.Cc = "";
            datosCorreo.Bco = "";
            datosCorreo.To = "";
            datosCorreo.Subject = "Encuesta de Sastifacción enviada por folio: "+ of.Folio ;
           datosCorreo.Body = @"<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Tarjeta de Correo</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f0f0f0;
        }
        .card {
            max-width: 400px;
            margin: 20px auto;
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }
        .card-header {
            background-color: #102C12;
            color: #fff;
            padding: 20px;
            text-align: center;
        }
        .card-body {
            padding: 20px;
        }
        .card-footer {
            background-color: #f0f0f0;
            padding: 20px;
            text-align: center;
        }
    </style>
</head>
<body>
    <div class=""card"">
        <div class=""card-header"">
            <h2>Encuesta de Sastifacción enviada por folio: " + of.Folio + @"</h2>
        </div>
        <div class=""card-body"">
    
            <p> <b>Hotel: </b>" + hotel + @"</p>
            <p> <b>Habitación: </b>" + of.Habi + @"</p>
             <p> <b>Checkin: </b>" + respuesta(of.Pregunta1) + @"</p>
             <p><b>Atenciones: </b> " + respuesta(of.Pregunta2) + @"</p>
            <p><b>Instalaciones: </b>" + respuesta(of.Pregunta3) + @"</p> 
             <p><b>Ama de Llaves: </b>" + respuesta(of.Pregunta4) + @"</p>
             <p><b>Limpieza: </b>" + respuesta(of.Pregunta5) + @"</p>
            <p><b>Alimentos: </b>" + respuesta(of.Pregunta6) + @"</p>
           <p><b>CheckOut: </b>" + respuesta(of.Pregunta7) + @"</p>
            <p><b>Comentarios: </b>" + of.Comentarios + @"</p>
            <p><b>Correo cliente: </b>" + correo + @"</p>
            
           
        </div>
        <div class=""card-footer"">
            <p>© 2024 Hoteles La Cordillera | Powered by <a href=""solucionesinspira.com"">Inversiones Santillan</a></p>
        </div>
    </div>
</body>
</html>";





            string jsonDatos = "{}";
            string query = "";
            
            // Define una clase que herede de DbContext para acceder a la base de datos
            using (SqlConnection connection = new SqlConnection(conexion.traeConexion()))
            {
                try
                {
                    connection.Open();

                }
                catch (Exception ex)
                {

                    var datos = new
                    {
                        code = "01",
                        message = "Error al conectarse a la base de datos: " + ex.Message,

                    };
                    jsonDatos = JsonConvert.SerializeObject(datos);
                    rsp = jsonDatos;
                    return false;


                }
            }


            DateTime fechaLarga = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy"));
            try
            {

                bool valida = true;
                if (!EsNumerico(of.idPropiedad)) { valida = false; }
                if (!EsNumerico(of.Pregunta1)) { valida = false; }
                if (!EsNumerico(of.Pregunta2)) { valida = false; }
                if (!EsNumerico(of.Pregunta3)) { valida = false; }
                if (!EsNumerico(of.Pregunta4)) { valida = false; }
                if (!EsNumerico(of.Pregunta5)) { valida = false; }
                if (!EsNumerico(of.Pregunta6)) { valida = false; }
                if (!EsNumerico(of.Pregunta7)) { valida = false; }
                string comen = of.Comentarios.ToString();

                if (comen.Length > 250)
                {
                    comen = comen.Substring(1, 250);
                }

               

                if (!valida)
                {
                    var datos = new
                    {
                        code = "02",
                        message = "formato no valido",

                    };
                    jsonDatos = JsonConvert.SerializeObject(datos);
                    rsp = jsonDatos;
                    return false;

                }

                query = "SP_grabaEncuesta";
                decimal idRespuesta = 0;
                using (SqlConnection connection = new SqlConnection(conexion.traeConexion()))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@idHotel", of.idPropiedad));
                    cmd.Parameters.Add(new SqlParameter("@hab", of.Habi));
                    cmd.Parameters.Add(new SqlParameter("@CheckIn", of.Pregunta1));
                    cmd.Parameters.Add(new SqlParameter("@Atencion", of.Pregunta2));
                    cmd.Parameters.Add(new SqlParameter("@Instalaciones", of.Pregunta3));
                    cmd.Parameters.Add(new SqlParameter("@amallaves", of.Pregunta4));
                    cmd.Parameters.Add(new SqlParameter("@Limpieza", of.Pregunta5));
                    cmd.Parameters.Add(new SqlParameter("@Alimentos", of.Pregunta6));
                    cmd.Parameters.Add(new SqlParameter("@CheckOut", of.Pregunta7));
                    cmd.Parameters.Add(new SqlParameter("@Comentarios", comen));
                    cmd.Parameters.Add(new SqlParameter("@fecha", fechaLarga));
                    cmd.Parameters.Add(new SqlParameter("@Folio", of.Folio));
                    cmd.Parameters.Add(new SqlParameter("@Correo", correo));
                    connection.Open();
                    cmd.ExecuteNonQuery();


                   // _emailService
  

                    var datos = new
                    {
                        code = "00",
                        message = "Encuesta guardada con exito",

                    };

                    enviaCorreo.Enviar(datosCorreo);


                    jsonDatos = JsonConvert.SerializeObject(datos);
                    rsp = jsonDatos;
                    return true;


                }

            }
            catch (Exception ex)
            {

                var datos = new
                {
                    code = "03",
                    message = "No se pudo recibir la encuesta: " + ex.Message,

                };
                jsonDatos = JsonConvert.SerializeObject(datos);
                rsp = jsonDatos;
                return false;

            }



        }

        public string traeCorreo(string propiedad,string Folio)
        {
            string valor = "N/A";
            try
            {
                string Codhotel = propiedad == "1" ? "073" : "074";
            DAO.Conexion cn = new Conexion();
             string conexion = cn.traeConexionSegmento(Codhotel);
           
            string query = $"SELECT EMAIL  FROM [V_Huespedes_y_Reservaciones] WHERE FOLIO = " + Folio;

            using (SqlConnection connection = new SqlConnection(conexion))
            {
                SqlCommand command = new SqlCommand(query, connection);

               
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        valor = result.ToString();
                    }
                          }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return valor;
        }

        static bool EsNumerico(string input)
        {
            return int.TryParse(input, out _);
        }


       private string respuesta(string var)
        {
            string rsp = "";
            if (var == "1")
                rsp = "Excelente";
            if (var == "2")
                rsp = "Promedio";
            if (var == "3")
                rsp = "No Satisfactorio";


            return rsp;
        }

       


    }



}
