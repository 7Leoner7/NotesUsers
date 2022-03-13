using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesUsers.DatabaseRoot;
using NotesUsers.Models;
using System;

namespace NotesUsers.Controllers
{
    public class Autorization : Controller
    {
        // GET: Autorization

        IWebHostEnvironment _appEnvironment;

        public Autorization(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public ActionResult IndexAuth(string login, string password, short HasError)
        {
            return View(new AutorizationViewModel() { Login = login, Password = password, HasError = HasError });
        }


        public void RequestAuth(string login, string password)
        {
            ApplicationContext ac = new ApplicationContext(_appEnvironment);
            if (ac.AuthUser(new User() { Login = login, password = password }))
                Response.Redirect("/Note/IndexNotes/?login=" + login + "&password=" + password);
            else Response.Redirect("/Autorization/IndexAuth/?login=" + login + "&HasError=1"); ;
        }
    }
}
