using Npgsql;
using System.Collections.Generic;
using NotesUsers.Models;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NotesUsers.DatabaseRoot
{
    public class ApplicationContext
    {
        //"Host=localhost;Port=5432;Username=dbuser;Password=123456;Database=db;"
        public string ConnectionDB { get; set; }

        public ApplicationContext(IWebHostEnvironment appEnvironment)
        {
                using (StreamReader reader = new StreamReader(appEnvironment.WebRootPath+"/lib/ConnectDB.txt"))
                {
                    ConnectionDB = reader.ReadToEnd();
                }
        }

        public List<User> users()
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionDB);
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT login, password FROM client", conn);
                NpgsqlDataReader dr = command.ExecuteReader();

                List<User> lu = new List<User>();
                while (dr.Read())
                    lu.Add(new User { Login = dr[0].ToString(), password = dr[1].ToString() });

                conn.Close();
                return lu;
            }
            catch { return null; }
        }

        public List<NoteViewModel> notes(User user)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionDB);
                conn.Open();
                string comm = "SELECT id, note, login, head FROM notes WHERE login = " + '\'' + user.Login + '\''; //"SELECT id, note, user FROM notes WHERE user = '" + login + "'";
                NpgsqlCommand command = new NpgsqlCommand(comm, conn);
                NpgsqlDataReader dr = command.ExecuteReader();

                List<NoteViewModel> ln = new List<NoteViewModel>();
                while (dr.Read())
                    ln.Add(new NoteViewModel { id = (int)dr[0], head = dr[3].ToString(), text = dr[1].ToString(), user = new User() { Login = dr[2].ToString() } });

                conn.Close();
                return ln;
            }
            catch { return null; }
        }

        public bool addNote(NoteViewModel nvm)
        {
            try
            {
                if ((nvm.text == null) || (nvm.user.Login == null) || (nvm.head == null)) throw new Exception();
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionDB);
                conn.Open();
                string comm = "INSERT INTO notes VALUES(nextval('notes_Id_seq')+1," + '\'' + nvm.text + "'," + '\'' + nvm.user.Login + "'," + '\'' + nvm.head + "')";
                NpgsqlCommand command = new NpgsqlCommand(comm, conn);
                var n = command.ExecuteNonQuery();
                conn.Close();
                return n>0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ReNote(NoteViewModel nvm)
        {
            try
            {
                if ((nvm.text == null) || (nvm.user.Login == null) || (nvm.head == null)) throw new Exception();
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionDB);
                conn.Open();
                string comm = "UPDATE notes SET note = " + '\'' + nvm.text + "', head = " + '\'' + nvm.head + "' WHERE id = "+nvm.id;
                NpgsqlCommand command = new NpgsqlCommand(comm, conn);
                var n = command.ExecuteNonQuery();
                conn.Close();
                return n > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool addUser(User user)
        {
                try
                {
                    if ((user.Login == null) || (user.password == null) || (users().Contains(user))) throw new Exception();
                    NpgsqlConnection conn = new NpgsqlConnection(ConnectionDB);
                    conn.Open();
                    string comm = "INSERT INTO client VALUES(" + '\'' + user.Login + "'," + '\'' + user.password + "')";
                    NpgsqlCommand command = new NpgsqlCommand(comm, conn);
                    var n = command.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                catch (Exception)
            {
                    return false;
                }
            
        }

        public bool testDB()
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionDB);
                conn.Open();
                string comm = @"CREATE TABLE IF NOT EXISTS public.client (
    login text NOT NULL,
    password text NOT NULL
) WITH(oids = false);


        CREATE SEQUENCE IF NOT EXISTS notes_id_seq INCREMENT 1 MINVALUE 1;

CREATE TABLE IF NOT EXISTS public.notes (
    id integer DEFAULT nextval('notes_id_seq') NOT NULL,
    note text NOT NULL,
    login text NOT NULL,
    head text NOT NULL,
    CONSTRAINT notes_pkey PRIMARY KEY(id)
) WITH(oids = false); ";
                NpgsqlCommand command = new NpgsqlCommand(comm, conn);
                var dr = command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AuthUser(User user)
        {
            try
            {
                if ((user.Login == null) || (user.password == null)) throw new Exception();
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionDB);
                conn.Open();
                string comm = "SELECT login, password FROM client WHERE login = " + '\'' + user.Login + '\'';
                NpgsqlCommand command = new NpgsqlCommand(comm, conn);
                NpgsqlDataReader dr = command.ExecuteReader();

                User u1 = new User();
                while (dr.Read())
                    u1 = new User() { Login = dr[0].ToString(), password = dr[1].ToString()}; 
                conn.Close();
                return (u1.Login == user.Login)&&(u1.password == user.password);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void DeleteNote(User user, int id)
        {
            try
            {
                if ((user.Login == null) || (user.password == null) || (!AuthUser(user))) throw new Exception();

                NpgsqlConnection conn = new NpgsqlConnection(ConnectionDB);
                conn.Open();
                string comm = "DELETE FROM notes WHERE login = " + '\'' + user.Login + '\'' + " AND id = " + id;
                NpgsqlCommand command = new NpgsqlCommand(comm, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch{ }
        }
    }
}
