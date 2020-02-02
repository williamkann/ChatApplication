using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class Topic
    {
        private int id_topic;
        private string title;
        private User author;
        private int number_messages;
        private List<Conversation> conversation;
        private List<User> chatters;

        public Topic(int id_topic, string title, User author,
            int number_messages, List<Conversation> conversation, List<User> chatters)
        {
            this.id_topic = id_topic;
            this.title = title;
            this.author = author;
            this.number_messages = number_messages;
            this.conversation = conversation;
            this.chatters = chatters;
        }


        public string Title { get => title; set => title = value; }
        public User Author { get => author; set => author = value; }
        public int Number_messages { get => number_messages; set => number_messages = value; }
        public List<Conversation> Conversation { get => conversation; set => conversation = value; }
        public List<User> Chatters { get => chatters; set => chatters = value; }
        public int Id_topic { get => id_topic; set => id_topic = value; }

        public override string ToString()
        {
            return //"ID topic : " + Id_topic
                 "\nTitle : " + Title
   /*             + "\nAuthor " + Author
                + "\nNumber messages : " + Number_messages
                + "\nConversation : " + Conversation
                + "\nChatters : " + Chatters*/;
        }

        public void printTheConversations()
        {
            foreach (Conversation c in Conversation)
            {
                Console.WriteLine(" - " + c.User.Login + " : " + c.Message);
            }
        }
    }
}
