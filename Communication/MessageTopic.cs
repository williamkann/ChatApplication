using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class MessageTopic : Message
    {
        private Topic topic;
        private string message;
        private User userSender;

        public MessageTopic(Topic topic)
        {
            this.topic = topic;
        }

        public MessageTopic(Topic topic, string message, User userSender)
        {
            this.topic = topic;
            this.message = message;
            this.userSender = userSender;
        }
        public Topic Topic
        {
            get { return topic; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public User UserSender
        {
            get { return userSender; }
        }

        public override string ToString()
        {
            return topic.Title;
        }
    }
}
