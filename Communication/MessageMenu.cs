using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class MessageMenu : Message
    {

        private int messageSend;

        public MessageMenu(int messageSend)
        {
            this.messageSend = messageSend;
        }

        public int MessageSend
        {
            get { return messageSend; }
            set { messageSend = value; }
        }

        public override string ToString()
        {
            return "choice number " + messageSend;
        }
    }
}
