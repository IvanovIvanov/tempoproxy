using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace TempoProxy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Developers()
        {
            ViewBag.Message = "Your application description page.";

            IFileWorker file = new FileWorker(null);
            String dataFromFile =  file.ReadFile();

            List<String> UserList = JsonConvert.DeserializeObject<List<String>>(dataFromFile);

            return View(UserList);
        }

        public JsonResult UpdateUserList (List<String> jsonUserList)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(jsonUserList);

                IFileWorker file = new FileWorker(null);
                file.UpdateFile(jsonString);

                return Json(new { result = "ok" });
            }
            catch
            {
                return Json(new { result = "fail" });
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}