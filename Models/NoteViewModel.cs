using NotesUsers.DatabaseRoot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesUsers.Models
{
    public class NoteViewModel
    {
        public string head { get; set; }
        public string text { get; set; }
        public int id { get; set; }
        public User user { get; set; }
        public short HasError { get; set; } //Свойство HasError определяет вид ошибки -1 (Передача данных в дб), 0 (Всё нормально)

    }
}
