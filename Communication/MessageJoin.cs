using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class MessageJoin : Message
    {
        private Topic topic;
        private User user;


        public MessageJoin(Topic topic, User user)
        {
            this.topic = topic;
            this.user = user;
        }
        public Topic Topic
        {
            get { return topic; }
        }


        public User User
        {
            get { return user; }
        }

        public override string ToString()
        {
            return topic.Title;
        }
    }
}
