using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebMandel.Models;
using WebMandel.Infrastructure;
using WebMandel.Logic;

namespace WebMandel.Controllers
{
    public class HomeController : Controller
    {
        IMailService _mailService;
        IMandelbrot _mandelbrot;

        IMandelbrot MandelbrotData
        {
            get
            {
                var ret = Session["mandelbrot"] as IMandelbrot;
                if (ret == null)
                {
                    Session["mandelbrot"] = _mandelbrot;
                    return _mandelbrot;
                }
                return ret;
            }
        }

        public HomeController(IMailService mailService, IMandelbrot mandelbrot)
        {
            _mailService = mailService;
            _mandelbrot = mandelbrot;
        }

        void SetEntry()
        {
            Session["entry"] = "Index";
        }

        bool CheckEntry()
        {
            var entry = Session["entry"] as string;
            return entry == "Index";
        }

        public ActionResult Index()
        {
            SetEntry();
            return View(new MandelbrotViewModel(_mandelbrot));
        }
#if false
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(MandelbrotViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            return View(new MandelbrotViewModel(_mandelbrot));
        }
#endif
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View(new ContactModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                await _mailService.SendMailAsync(model.Name, model.Email,
                                                 "Nils Gösche", "cartan@cartan.de",
                                                 model.Subject, model.Message);
                return View("Sent");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Mandelbrot(double xMin, double yMax, double scale, int iterations)
        {
            if (!CheckEntry()) return View("Error");
            _mandelbrot.XMin = xMin;
            _mandelbrot.YMax = yMax;
            _mandelbrot.Scale = scale;
            _mandelbrot.Iterations = iterations;
            var file = _mandelbrot.GenerateMandelImage();
            return new FileContentResult(file, "image/png");
        }
    }
}