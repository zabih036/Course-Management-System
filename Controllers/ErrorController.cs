using Microsoft.AspNetCore.Mvc;

namespace JahanInstitute.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult ErrorHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.Message = "نوموړی صفحه شتون نه لرې";
                    ViewBag.Code = "404";
                    break;
                case 500:
                    ViewBag.Message = "نوموړی صفحه شتون نه لرې";
                    ViewBag.Code = "500";
                    break;
            }
            return View();
        }
    }
}