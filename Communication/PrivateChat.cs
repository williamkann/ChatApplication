using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class PrivateChat
    {
        private int id_chat;
        private string title;
        private int number_messages;
        private List<Conversation> conversation;
        private List<User> usersChat;

        public int Id_chat { get => id_chat; set => id_chat = value; }
        public string Title { get => title; set => title = value; }
        public int Number_messages { get => number_messages; set => number_messages = value; }
        public List<Conversation> Conversation { get => conversation; set => conversation = value; }
        public List<User> UsersChat { get => usersChat; set => usersChat = value; }

        public PrivateChat() { }
        public PrivateChat(int id_chat, string title,
            int number_messages, List<Conversation> conversation, List<User> usersChat)
        {
            this.id_chat = id_chat;
            this.title = title;
            this.number_messages = number_messages;
            this.conversation = conversation;
            this.usersChat = usersChat;
            usersChat.Capacity = 2;
        }

        public override string ToString()
        {
            return "ID chat : " + Id_chat
                + "\nTitle : " + Title
                + "\nNumber messages : " + Number_messages
                + "\nConversation : " + Conversation
                + "\nUser chat : " + usersChat;
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
