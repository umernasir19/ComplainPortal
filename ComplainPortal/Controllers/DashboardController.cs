using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ComplainPortal.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }
        
        public JsonResult SendEmail()
        {
            SendEmailWithSmtp();
            return Json(new { statuscode=200,success=true}, JsonRequestBehavior.AllowGet);
        }

        public void SendEmailWithSmtp()
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential("umer.nasir@aku.edu", "");
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress("umer.nasir@aku.edu");

                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Host = "exmbxcas4.aku.edu";
                    
                    smtpClient.Credentials = basicCredential;
                    smtpClient.Port = 25;

                    message.From = fromAddress;
                    message.Subject = "test";
                    // Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = "<h1>Test</h1>";
                    message.To.Add("umer.nasir@aku.edu");

                    try
                    {
                        smtpClient.Send(message);
                    }
                    catch (Exception ex)
                    {
                        //Error, could not send the message
                       // Response.Write(ex.Message);
                    }
                }
            }
            //try
            //{


            //    MailMessage mail = new MailMessage();
            //    SmtpClient SmtpServer = new SmtpClient("exmbxcas4.aku.edu");

            //    mail.From = new MailAddress("umer.nasir@aku.edu");
            //    mail.To.Add("umer.nasir@aku.edu");
            //    mail.Subject = "Test Mail";
            //    mail.Body = "This is for testing SMTP mail from GMAIL";

            //    SmtpServer.Port = 25;
            //    SmtpServer.Credentials = new System.Net.NetworkCredential("umer.nasir@aku.edu", "qwerty1.");
            //    SmtpServer.EnableSsl = true;

            //    SmtpServer.Send(mail);
            //    return true;
            //}
            //catch(Exception ex)
            //{
            //    return false;
            //}
        }

        public ActionResult ViewTemplete()
        {
            return View();
        }
    }
}