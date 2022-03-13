using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NotesUsers.DatabaseRoot;
using NotesUsers.Models;
using System.Collections.Generic;

namespace NotesUsers.Controllers
{
    public class Registration : Controller
    {
        // GET: Registration

        IWebHostEnvironment _appEnvironment;

        public Registration(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public ActionResult IndexReg(string login, string password, string password1, short HasError = 0)
        {
            if (password != password1) return Error(login, 2);
            if ((password != null) && (login != null))
            {
                ApplicationContext ac = new ApplicationContext(_appEnvironment);
                List<User> clients = ac.users();
                foreach (var client in clients)
                {
                    if (login == client.Login)
                    {
                        return Error(login, 1);
                    }
                }
                Create(login, password, _appEnvironment);
            }
            return View(new AutorizationViewModel() { Login = login, Password = password, HasError = 0 });
        }

        public void Create(string login, string password, IWebHostEnvironment appEnvironment)
        {
            ApplicationContext ac = new ApplicationContext(appEnvironment);
            ac.addUser(new User() { Login = login, password = password });
            Response.Redirect("/Note/IndexNotes/?login="+login + "&password=" + password);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error(string login, short HasError)
        {
            return View(new AutorizationViewModel() { Login = login, Password = "", HasError = HasError});
        }
    }
}
