using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace SendNotification
{
    public static class SendNotification
    {
        static readonly string username = "freestuff2085a@gmail.com";
        static readonly string password = "xxxx";
        [FunctionName("SendNotification")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            SendEmail("freestuff2085a@gmail.com", "javoucsd@gmail.com", "you got notification from Github", "testring");

            string responseMessage = $"Email has been sent successfully to me";

            return new OkObjectResult(responseMessage);
        }

        public static async void SendEmail(string senderEmail, string receiverEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(senderEmail));
            email.To.Add(MailboxAddress.Parse(receiverEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email          
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(username, password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }

}
