using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool SendEmailWithSmtp()
        {
            try
            {


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
               
                mail.From = new MailAddress("AEMS_NotificationService@aku.edu");
                mail.To.Add("umer.nasir@aku.edu");
                mail.Subject = "Test Mail";
                mail.Body = "This is for testing SMTP mail from GMAIL";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("username", "password");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public ActionResult ViewTemplete()
        {
            return View();
        }
    }
}