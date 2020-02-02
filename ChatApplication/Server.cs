using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;

namespace ChatApplication
{
    class Server
    {
        private int port;
        private List<User> users;
        private List<User> connectedUsers;
        private List<Topic> topics;
        private List<PrivateChat> privateChats;
        private Dictionary<User, List<TcpClient>> userTcps;

        public Server(int port)
        {
            this.port = port;
            userTcps = new Dictionary<User, List<TcpClient>>();

            //Users of our app
            users = new List<User>();

            //Object to serialize users.txt
            UserSerialize us = new UserSerialize(users);

            //Deserialize users from the file users.txt
            users = us.DeSerializeNow();

            //User that are connected
            connectedUsers = new List<User>();


            /* Creation of private chats */
            List<Conversation> conversationsPrivate1 = new List<Conversation>();

            conversationsPrivate1.Add(new Conversation("Salut", users[1]));
            conversationsPrivate1.Add(new Conversation("Bonjour", users[2]));


            List<User> privateChatParticipants = new List<User>();
            privateChatParticipants.Add(users[1]);
            privateChatParticipants.Add(users[2]);

            PrivateChat privateChat1 = new PrivateChat(1, "Private Chat with " + users[1].Login + " and " + users[2].Login, 2, conversationsPrivate1, privateChatParticipants);


            privateChats = new List<PrivateChat>();
            privateChats.Add(privateChat1);



            /* Creation of topics */
            List<Conversation> conversationsTopic1 = new List<Conversation>();

            conversationsTopic1.Add(new Conversation("Salut message 1", users[1]));
            conversationsTopic1.Add(new Conversation("Bonjour", users[2]));

            List<Conversation> conversationsTopic2 = new List<Conversation>();
            conversationsTopic2.Add(new Conversation("Hello hello", users[1]));
            conversationsTopic2.Add(new Conversation("Test test", users[2]));


            List<User> participants = new List<User>();
            participants.Add(users[1]);
            participants.Add(users[2]);


            topics = new List<Topic>();
            Topic topic1 = new Topic(1, "Le topic du PC !", users[1], 2, conversationsTopic1, participants);
            Topic topic2 = new Topic(2, "Le topic du Console", users[2], 2, conversationsTopic2, participants);

            /* End of creation topics */


            topics.Add(topic1);
            topics.Add(topic2);

        }

        public void start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();

            while (true)
            {
                TcpClient comm = l.AcceptTcpClient();
                Console.WriteLine("Connection established @" + comm.Client.RemoteEndPoint);


                new Thread(new Receiver(comm, users, topics, privateChats, connectedUsers, userTcps).launchApp).Start();

            }

        }
    }
}
