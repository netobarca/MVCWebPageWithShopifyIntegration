using GatewayBelizeOn.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GatewayBelizeOn.Controllers
{
    public class ZitroController : Controller
    {
        //Variable to manage the instance of the EntityFramework.
        private belizeonEntities _belizeonEntities = new belizeonEntities();
        
        /// <summary>
        /// Method to validate if the sessions is active.
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Index()
        {
            String userName = (string)(Session["userZitro"]);
            if (userName == "" || userName == null)
            {
                return View();
            }
            else
            {
                return View("../Orders/Index");
            }            
        }

        /// <summary>
        /// Method to verify if the user exist in our databases with the given user y password
        /// </summary>
        /// <param name="loginData">Object with the neccesaries fields to login the user</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult verifyUSer(Account loginData)
        {
            try
            {
                string crypt = Encrypter.EncryptSHA256(loginData.password);
                var expireTime = DateTime.Now.AddMinutes(30);

                BON_USR_USERS user = this.getUserValidate(loginData.userName, crypt);
                if (user != null)
                {
                    Session["userZitro"] = loginData.userName;
                    Session["expireZitroUser"] = expireTime;
                    return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.OK, user.userName), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized,"User or Password Invalid please try again"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError, "We have some problems with our site, please try again later"), JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Method to consume the Databases with entity framework
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="password">password</param>
        /// <returns>BON_USR_USERS</returns>
        private BON_USR_USERS getUserValidate(string userName, string password)
        {
            return _belizeonEntities.BON_USR_USERS.Where(x => x.userName == userName && x.password == password).FirstOrDefault();
        }

        /// <summary>
        /// Method to clean sessions and redirct to the login View
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Logout()
        {
            Session["userZitro"] = "";
            Session["expireZitroUser"] = "";
            return View("../Zitro/Index");
        }
    }
}