using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class MessageUsers : Message
    {
        private List<User> users;

        public MessageUsers(List<User> users)
        {
            this.users = users;
        }

        public List<User> Users
        {
            get { return users; }
        }
    }
}
