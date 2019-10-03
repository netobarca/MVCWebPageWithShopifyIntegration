using GatewayBelizeOn.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace GatewayBelizeOn.Controllers
{
    public class HomeController : Controller
    {
        // Shopify base URL, you should put your commerceURL API
        string apiShopifyURL = ConfigurationManager.AppSettings["shopifyURL"];

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Validate if the sessions for the current user does not expire yet.
        /// </summary>
        /// <returns>Json</returns>
        [HttpGet]
        public JsonResult verifySession()
        {
            try
            {
                DateTime originalExpire = (DateTime)(Session["expireZitroUser"]);
                var comparationDate = DateTime.Now;
                if (originalExpire > comparationDate)
                {
                    return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.OK,""), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Session["userZitro"] = "";
                    Session["expireZitroUser"] = "";
                    return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Your Session has expire, please login again"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                Session["userZitro"] = "";
                Session["expireZitroUser"] = "";
                return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError, "Your Session has expire, please login again"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Method to get an specific transaction with an id given
        /// </summary>
        /// <param name="id_transaction">It is the id of the transaction im looking for</param>
        /// <returns>Json</returns>
        [HttpGet]
        public JsonResult getTransactions(string id_transaction) {
            try
            {
                var restClient = new RestClient(apiShopifyURL);
                restClient.Authenticator = new HttpBasicAuthenticator("usernameFromShopify", "passwordFromShopify");
                var request = new RestRequest("orders/" + id_transaction + "/transactions.json", Method.GET);
                request.AddUrlSegment("status", "open");
                request.AddHeader("header", "Content-Type: application/json");
                IRestResponse<dynamic> response = restClient.Execute<dynamic>(request);
                return Json(response.Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error at the time to get a transation", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Method to mark as paid a shopify transaction.
        /// </summary>
        /// <param name="transaction">Object with the neccesaries fields to mark the transaction as paid</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult markToPaid(Transaction transaction)
        {
            try
            {
                var restClient = new RestClient(apiShopifyURL + "orders/" + transaction.order_id + "/transactions.json");
                var request = new RestRequest( Method.POST);

                restClient.Authenticator = new HttpBasicAuthenticator("usernameFromShopify", "passwordFromShopify");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                request.AddHeader("Host", "yourcommerceURL.myshopify.com");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("undefined", "{\r\n  \"transaction\": {\r\n    \"currency\": \"" 
                    + transaction.currency 
                    + "\",\r\n    \"amount\": \"" 
                    + transaction.amount + "\",\r\n    \"kind\": \"" 
                    + transaction.kind + "\",\r\n    \"parent_id\": \"" 
                    + transaction.parent_id + "\"\r\n  }\r\n}", 
                    ParameterType.RequestBody
                );

                IRestResponse<dynamic> response = restClient.Execute<dynamic>(request);

                return Json(response.Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error at the time to Mark the transation as paid", JsonRequestBehavior.AllowGet);
            }

        }
    }
}