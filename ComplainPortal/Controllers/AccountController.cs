using ComplainPortal.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace ComplainPortal.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public JsonResult Login(Login userlogin)
        {
            var flag = false;
            //checking xml
            var userrole=  CheckUsersRoleFromXml(userlogin.email);

           
            //calling ldap validation function to validate user
           // returns true if exist 
            flag = validateUserByBind(userlogin.email, userlogin.password);
            //sending response to ajax 
            return Json(new { Login = flag,statuscode=200 }, JsonRequestBehavior.AllowGet);
        }

        #region Validation using Ldap 
        public bool validateUserByBind(string username, string password)
        {
            bool result = true;
            var credentials = new NetworkCredential(username, password);
            var serverId = new LdapDirectoryIdentifier("akudc14.aku.edu");

            var conn = new LdapConnection(serverId, credentials);
            try
            {
                conn.Bind();
            }
            catch (Exception)
            {
                result = false;
            }

            conn.Dispose();

            return result;
        }

        #endregion

        #region Xml File Parsing

        public bool CheckUsersRoleFromXml(string username)
        {
            //loading xml file to xml dosument object
            XDocument xml = XDocument.Load(HttpContext.Server.MapPath("/Utilities/Users.xml"));
            //looping  first for parent node 
            foreach (var users in xml.Descendants("users"))
            {
                //second loop for child 
                foreach (var user in users.Descendants("user"))
                {
                    //getting values from node
                    var name = user.Element("name").Value.ToString();
                    var age = user.Element("role").Value.ToString();

                    //matching 
                    if (name == username)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}