using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class MessageNavigation : Message
    {
        private User user;
        private string message;

        public User User { get => user; set => user = value; }
        public string Message { get => message; set => message = value; }

        public MessageNavigation(User user, string message)
        {
            this.user = user;
            this.message = message;
        }
    }
}
