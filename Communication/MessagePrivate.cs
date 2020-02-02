using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class MessagePrivate : Message
    {
        private PrivateChat pvChat;
        private string message;
        private User userSender;

        public MessagePrivate(PrivateChat pvChat)
        {
            this.pvChat = pvChat;
        }

        public MessagePrivate(PrivateChat pvChat, string message, User userSender)
        {
            this.pvChat = pvChat;
            this.message = message;
            this.userSender = userSender;
        }


        public string Message
        {
            get { return message; }
        }

        public User UserSender
        {
            get { return userSender; }
        }

        public PrivateChat PvChat { get => pvChat; set => pvChat = value; }

        public override string ToString()
        {
            return pvChat.Title;
        }
    }
}
