using SendGrid;
using SendGrid.Helpers.Mail;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Configuration;

namespace GatewayBelizeOn.Controllers
{
    public class OrdersController : Controller
    {
        // Shopify base URL, you should put your commerceURL API
        string apiShopifyURL = ConfigurationManager.AppSettings["shopifyURL"];

        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method to get Pending Orders from Shopify
        /// </summary>
        /// <returns>Json</returns>
        [HttpGet]
        public JsonResult getPendingOrders()
        {
            try
            {
                var restClient = new RestClient(apiShopifyURL);
                restClient.Authenticator = new HttpBasicAuthenticator("usernameFromShopify", "passwordFromShopify");
                var request = new RestRequest("orders.json", Method.GET);
                request.AddUrlSegment("status", "open");
                request.AddHeader("header", "Content-Type: application/json");
                IRestResponse<dynamic> response = restClient.Execute<dynamic>(request);
                return Json(response.Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error to get the pending transactions from Shopify", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Method to send an email notification that a transation was being paid.
        /// </summary>
        /// <param name="id_transaction">It is the id of the transaction i being report</param>
        /// <returns>Json</returns>
        [HttpGet]
        public async Task<JsonResult> sendMail(string id_transaction)
        {
            try
            {
                await Execute(id_transaction);
                return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.OK, "Correo Enviado exitosamente"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error at the time to send the email at the administrator", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Method to integrated the SendGrind API.
        /// </summary>
        /// <param name="idTransaction"></param>
        /// <returns></returns>
        static async Task Execute(string idTransaction)
        {
            var apiKey = "keyGivenFromSendGrid";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("manager@zitro.bz", "BelizeOn");
            var subject = "BelizeOn payment approved by Zitro";
            var to = new EmailAddress("Belizeon@karlmenzies.com", "");
            var plainTextContent = "Payment done by Zitro agent regarding Order #" + idTransaction + "";
            var htmlContent = "<strong>Payment done by Zitro agent regarding Order #" + idTransaction + "</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            var a = response.StatusCode;
        }
    }
}