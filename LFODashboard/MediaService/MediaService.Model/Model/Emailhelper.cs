//using System.Net.Mail;
//using System.Net;

//namespace MediaService.BL.Model
//{
//    public class Emailhelper
//    {
//        public static string Email(string EmailBody, string recipEmail, string Subject)
//        {
//            // Sender's email address and password
//            string senderEmail = "italerts@ritcologistics.com";
//            string password = "Ritco@336";

//            // Recipient's email address
//            string recipientEmail = recipEmail;

//            // Create a new MailMessage object
//            MailMessage mail = new MailMessage(senderEmail, recipientEmail);

//            // Set the subject and body of the email
//            mail.Subject = Subject;
//            mail.IsBodyHtml = true; // Set to true for HTML email
//            mail.Body = EmailBody;

//            // Create a new SmtpClient instance for Outlook.com
//            SmtpClient smtpClient = new SmtpClient("webmail.ritcologistics.com");

//            // Set SMTP server port and credentials
//            smtpClient.Port = 587;
//            smtpClient.Credentials = new NetworkCredential(senderEmail, password);
//            smtpClient.EnableSsl = true; // Enable SSL/TLS encryption

//            try
//            {
//                // Send the email
//                smtpClient.Send(mail);
//                Console.WriteLine("Email sent successfully!");
//                return "Y";
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Failed to send email: " + ex.Message);
//                return "N";
//            }
//        }
//    }
//}
