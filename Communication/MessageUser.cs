using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class MessageUser : Message
    {

        private string login;
        private string password;
        private int id_user;
        private bool err;

        public MessageUser(string login, string password, bool err)
        {
            this.login = login;
            this.password = password;
            this.err = err;
        }

        public MessageUser(int id_user, string login, string password, bool err)
        {
            this.id_user = id_user;
            this.login = login;
            this.password = password;
            this.err = err;
        }

        public int Id_user
        {
            get { return id_user; }
        }
        public string Login
        {
            get { return login; }
        }

        public string Password
        {
            get { return password; }
        }

        public User getUser()
        {
            return new User(id_user, login, password);
        }
        public bool Err
        {
            get { return err; }
        }


        public override string ToString()
        {
            return "The user is : " + login;
        }
    }
}
