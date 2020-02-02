using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class MessageTopics : Message
    {
        private List<Topic> topics;


        public MessageTopics(List<Topic> topics)
        {
            this.topics = topics;
        }
        public List<Topic> Topics
        {
            get { return topics; }
        }


        public override string ToString()
        {
            string result = "Here is the list of the Topics : ";
            foreach (Topic t in topics)
                result += t;

            return result;
        }
    }
}
