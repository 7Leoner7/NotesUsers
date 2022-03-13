using Microsoft.AspNetCore.Mvc;
using NotesUsers.Models;
using NotesUsers.DatabaseRoot;
using Microsoft.AspNetCore.Hosting;

namespace NotesUsers.Controllers
{
    public class Note : Controller
    {
        // GET: Note

        IWebHostEnvironment _appEnvironment;

        public Note(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public ActionResult IndexNote(User user, string head, string text, int id, short HasError = 0, bool IsIndexNote=false)
        {
            ApplicationContext ac = new ApplicationContext(_appEnvironment);
            if (!ac.AuthUser(user))
            {
                Response.Redirect("/Home/Index");
            }
            NoteViewModel nvm = new NoteViewModel { head = head, text = text, id = id, user = user, HasError = HasError };
            if(IsIndexNote)nvm.HasError = OperationNote(nvm);
            return View(nvm);
        }

        public short OperationNote(NoteViewModel nvm)
        {
            if (nvm.id > 0)
            {
               return RewriteNote(nvm.user.Login, nvm.user.password, nvm.head, nvm.text, nvm.id);
            }
            else return CreateNote(nvm.user.Login, nvm.user.password, nvm.head, nvm.text);
        }

        private short CreateNote(string login, string password, string head, string text)
        {
            ApplicationContext ac = new ApplicationContext(_appEnvironment);
            var test = ac.addNote(new NoteViewModel() { head = head, text = text, user = new User() { Login = login, password = password } });
            if (test == true)
            {
                Response.Redirect("/Note/IndexNotes/?login=" + login + "&password=" + password);
                return 0;
            }
            return -1;
        }

        private short RewriteNote(string login, string password, string head, string text, int id)
        {
            ApplicationContext ac = new ApplicationContext(_appEnvironment);
            var test = ac.ReNote(new NoteViewModel() { head = head, text = text, user = new User() { Login = login, password = password }, id = id });
            if (test == true)
            {
                Response.Redirect("/Note/IndexNotes/?login=" + login + "&password=" + password);
                return 0;
            }
            return -1;
        }

        public ActionResult IndexNotes(string login, string password)
        {
            ApplicationContext ac = new ApplicationContext(_appEnvironment);
            if (!ac.AuthUser(new User { Login = login, password = password })) Response.Redirect("/Home/Index");
            var Lnvm = ac.notes(new User() { Login = login, password = password });
            foreach (var nvm in Lnvm)
                nvm.user.password = password;
            if (Lnvm.Count == 0) Lnvm.Add(new NoteViewModel() { user = new User() { Login = login, password = password } }); //Костыль! не трогать!!!
            return View(Lnvm);
        }

        public void DeleteNoteID(User user, int id){
            new ApplicationContext(_appEnvironment).DeleteNote(user, id);
            Response.Redirect("/Note/IndexNotes/?login=" + user.Login + "&password=" + user.password);
        }
    }
}
