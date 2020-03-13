using ComplainPortal.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
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
            bool flag = false;
            //calling ldap validation function to validate user
           // returns true if exist 
            flag = validateUserByBind(userlogin.email, userlogin.password);
            //checking xml
            CheckUsersRoleFromXml(userlogin.email);
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

                Getuserinfo(username);
            }
            catch (Exception)
            {
                result = false;
            }

            conn.Dispose();

            return result;
        }


        public void Getuserinfo(string username)
        {
            using (var context = new PrincipalContext(ContextType.Domain, "akudc14.aku.edu"))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                if (user != null)
                {
                    ViewBag.UserName = user.Name;
                    ViewBag.EmailAddress = user.HomeDirectory;
                }


            }

        }
        #endregion

        #region Xml File Parsing

        public void CheckUsersRoleFromXml(string username)
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
                    //matching user name with xml users 
                    if (name == username)
                    {
                        //array list for storing roles
                        ArrayList roles = new ArrayList();
                        foreach(var userroles in user.Descendants("role"))
                        {
                            roles.Add(userroles.Value.ToString());
                        }
                        Session["userroles"] = roles;
                    }

                }
                
            }
            
        }

        #endregion
    }
}