using Microsoft.AspNetCore.Mvc;
using SORAPC.Models;
using System.Diagnostics;

namespace SORAPC.Controllers
{
    public class HomeController : Controller
    {
        private SorapcContext _soradb;

        public HomeController(SorapcContext context)
        {
            _soradb = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}