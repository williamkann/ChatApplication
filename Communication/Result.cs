using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class Result : Message
    {
        private User user;
        private bool error;

        public Result(User user, bool err)
        {
            this.user = user;
            error = err;
        }

        public User User
        {
            get { return user; }
        }

        public bool Error
        {
            get { return error; }
        }

        public override string ToString()
        {
            if (error == false)
                return "You are connected as : " + user.Login;
            else
                return "Fail to connect";
        }

    }
}
