using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Web;

namespace RealEstateAPI.Models
{
    public static class MailSender
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json
        private static string[] Scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailSend, GmailService.Scope.GmailCompose, GmailService.Scope.MailGoogleCom };
        private static string ApplicationName = "RealEstateAPI";

        public static void SendMail(Feedback feedback) {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                //ApiKey = "AIzaSyAau3j9w4M4PH5uSUAftJJbrZW6loQZvqE",
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            #region send Message

            feedback.FeedbackId = Guid.NewGuid();

            MailMessage msg = new MailMessage();
            msg.Subject = "Contacto Cliente";
            msg.From = new MailAddress("contact@moneta.studio", "Contacto Moneta");
            msg.To.Add(new MailAddress("guadalupe.marrero@gmail.com"));
            msg.BodyTransferEncoding = TransferEncoding.Base64;

            msg.Body = getHtmlBody(feedback);
            msg.IsBodyHtml = true;

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(msg);

            MemoryStream buffer = new MemoryStream();
            mimeMessage.WriteTo(buffer);
            buffer.Position = 0;

            StreamReader sr = new StreamReader(buffer);
            string rawString = sr.ReadToEnd();

            Message message = new Message
            {
                Raw = Base64UrlEncode(rawString)
            };

            //message = 
            service.Users.Messages.Send(message, "contact@moneta.studio").Execute();
            #endregion
        }

        private static string getHtmlBody(Feedback feedback) {
            string body = "<body><p>Favor de no responder a este correo</p><p>Notificacion no. {0}</p><p>Correo de remitente: {1}</p><p>Asunto: {2}</p><p>Cuerpo: <br><br>{3}</p></body>";
            body = String.Format(body, feedback.FeedbackId.ToString(), feedback.Email, feedback.Subject, feedback.Body);
            body = body.Replace(Environment.NewLine, "<br>");
            return body;
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }


    }
}
