using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{

    [Serializable]
    public class Conversation
    {
        private string message;
        private User user;

        public Conversation(string message, User user)
        {
            this.message = message;
            this.user = user;
        }

        public string Message { get => message; set => message = value; }
        public User User { get => user; set => user = value; }

        public override string ToString()
        {
            return "From : " + user.Login + "\t" + Message;
        }
    }
    
}
