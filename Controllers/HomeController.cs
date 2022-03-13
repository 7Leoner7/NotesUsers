using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotesUsers.DatabaseRoot;
using NotesUsers.Models;
using System.Diagnostics;
using System.IO;

namespace NotesUsers.Controllers
{
    public class HomeController : Controller
    {
        IWebHostEnvironment _appEnvironment;

        public HomeController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            ApplicationContext ac = new ApplicationContext(_appEnvironment);
            if (ac.testDB() == false) return View(new IndexViewModel() { HasError = 1 });
            return View(new IndexViewModel() { HasError = 0 });
        }

        public void connector(string conn)
        {
            using (StreamWriter writer = new StreamWriter(_appEnvironment.WebRootPath + "/lib/ConnectDB.txt"))
            {
                writer.WriteLine(conn);
            }
            Response.Redirect("/Home/Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
