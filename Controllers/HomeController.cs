using EnsambladoraSV.Models.InventoryDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Web.Security;
using System.Web.WebPages;
using Antlr.Runtime.Tree;

namespace EnsambladoraSV.Controllers
{
    public class HomeController : Controller
    {


        private InventorySystemDBEntities repository = new InventorySystemDBEntities();

        public ActionResult Login()
        {
            return View();
        } 

        public ActionResult inicio()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Registro()
        {

            ViewBag.Message = "Your contact page.";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authenticate(string email, string password, string rememberSession)
        {

            bool isTrue = false;
            if (IsValidUser(email, password))
            {
                string userName = User.Identity.Name;

                if (!string.IsNullOrEmpty(userName))
                {
                    if (rememberSession == "1")
                    {
                        FormsAuthentication.SetAuthCookie(email, rememberSession == "1");
                        isTrue = true;
                    }
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                    isTrue = true;
                }
            }
            return Json(new { status = isTrue }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetUsers(string name, string email, string password)
        {
            string passwordHasher = BCrypt.Net.BCrypt.HashPassword(password);

            ObjectParameter ErrorMessage = new ObjectParameter("ErrorMessage", typeof(string));

            repository.SetUsers(name,email, passwordHasher, ErrorMessage);

            var result = ErrorMessage.Value.ToString();

            return Json(new {ErrorMessage = result}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult VerifyToken()
        {
            bool hasToken = false;
            string userName = User.Identity.Name;

            if (!string.IsNullOrEmpty(userName))
            {
                hasToken = true;
               return Json(new { status = hasToken }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = hasToken}, JsonRequestBehavior.AllowGet);
        }


        private bool IsValidUser(string email, string password)
        {

            ObjectParameter passwordHash = new ObjectParameter("passwordHash", typeof(string));

            var result = repository.GetPasswordHashByEmail(email, passwordHash);
            string hashedPassword = passwordHash.Value.ToString();

            if(hashedPassword == "")
            {
                return false;
            }
            else {
                bool isMatch = BCrypt.Net.BCrypt.Verify(password, hashedPassword); 

                if (isMatch == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}