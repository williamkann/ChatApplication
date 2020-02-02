using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class User
    {
        private int id_user;
        private string login;
        private string password;


        public User(int id_user, string login, string password)
        {
            this.id_user = id_user;
            this.login = login;
            this.password = password;

        }




        public int Id_user
        {
            get { return id_user; }
            set { id_user = value; }
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public User getUser()
        {
            return new User(id_user, login, password);
        }
    }
}
