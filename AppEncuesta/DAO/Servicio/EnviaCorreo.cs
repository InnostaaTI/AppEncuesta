using AppEncuesta.Models;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace AppEncuesta.DAO.Servicio
{
    public class EnviaCorreo 
    {




        public  bool Enviar(Correo correoen)//Recibe un objeto llamado CorreoDTO
        {
            try

            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();

                string emailHost = configuration["EmailHost"];
                string emailUsername = configuration["EmailUsername"];
                string emailPassword = configuration["EmailPassword"];
                int emailPort = int.Parse(configuration["EmailPort"]);
                string emailTo = configuration["EmailTo"];
                string emailBcc = configuration["EmailBcc"];
                string emailCc = configuration["EmailCc"];
                string emailName = configuration["EmailName"];



                var email = new MimeMessage();//Estructura del correo
                email.From.Add(new MailboxAddress(emailName, emailUsername)); //1ro). Nombre de quien envia el correo 2do). Direccion de quien esta enviando el correo
                email.To.Add(MailboxAddress.Parse(emailTo));// Correo hacia quien va dirigido
                email.Subject = correoen.Subject;//Asunto del correo
                if (emailBcc != string.Empty)
                    email.Bcc.Add(MailboxAddress.Parse(emailBcc));

                if (emailCc != string.Empty)
                {
                    var ccAddresses = emailCc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cc in ccAddresses)
                    {
                        email.Cc.Add(MailboxAddress.Parse(cc.Trim()));
                    }
                }
                //if (emailCc != string.Empty)
                //    email.Cc.Add(MailboxAddress.Parse(emailCc));

                email.Body = new TextPart(TextFormat.Html)//Contenido que tendra el correo
                {
                    Text = correoen.Body
                };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();//Esto debe ir tal como se ve ya que es detecta las letras mayus y min.
                smtp.Connect(emailHost, emailPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(emailUsername, emailPassword);//Autenticacion se realiza mediante el correo y la clave.
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;//Si todo se encuentra bien indicará que el correo se envio exitosamente.
            }
            catch(Exception ex)
            {

                string emailName = ex.Message.ToString();
                return false;
            }
        }
    }
}
