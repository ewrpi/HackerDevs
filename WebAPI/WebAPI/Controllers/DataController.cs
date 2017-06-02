using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class DataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Inquiry()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string key in HttpContext.Request.Form.Keys)
                dictionary.Add(key, HttpContext.Request.Form[key]);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(dictionary);

            Core.AddInquiry(data, Request.HttpContext.Connection.RemoteIpAddress.ToString());

            return Redirect("http://www.hackerdevs.com/inquiry.html");
        }
        [HttpGet]
        public string Test()
        {
            return "Hello";
        }
    }
}