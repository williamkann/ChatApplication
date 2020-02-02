using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;

namespace ChatApplicationGUI
{
    public class Receiver
    {
        private TcpClient comm;
        private List<User> users;
        private List<User> connectedUsers;
        private List<Topic> topics;
        private List<PrivateChat> privateChats;
        private List<TcpClient> tcps;
        private Dictionary<User, List<TcpClient>> userTcps;

        public Receiver(TcpClient s, List<User> users, List<Topic> topics, List<PrivateChat> privateChats, List<User> connectedUsers, Dictionary<User, List<TcpClient>> userTcps)
        {
            comm = s;
            this.users = users;
            this.connectedUsers = connectedUsers;
            this.topics = topics;
            this.privateChats = privateChats;
            this.userTcps = userTcps;
            this.tcps = new List<TcpClient>();
        }


        public void launchApp()
        {

            Console.WriteLine("Connecting to the chat app");

            // read expression
            Message testUser = Net.receiveMessage(comm.GetStream());

            if (testUser.GetType().Name.Equals("MessageUser"))
            { 
                //Used to check if the user is valid
                bool valid = false;

                // read expression
                MessageUser testUserConnect = (MessageUser)testUser;

                Console.WriteLine("Login and password received");
                // send result 

                foreach (User u in users)
                    if (testUserConnect.Login.Equals(u.Login) && testUserConnect.Password.Equals(u.Password))
                        valid = true;

                if (valid)
                {
                    //Send the user with a boolean to display the result (if success or not)
                    Net.sendMessage(comm.GetStream(), new MessageUser(testUserConnect.Login, testUserConnect.Password, false));

                    //Make the connection by putting the user in the object connectionUser
                    User connectionUser = null;
                    foreach (User u in users)
                        if (u.Login.Equals(testUserConnect.Login) && u.Password.Equals(testUserConnect.Password))
                            connectionUser = u;

                    //Add the user to the list of connected people and send back the user to the client
                    connectedUsers.Add(connectionUser);
                    Net.sendMessage(comm.GetStream(), new MessageUser(connectionUser.Id_user, connectionUser.Login, connectionUser.Password, true));
                    Console.WriteLine("You are connected as " + connectionUser.Login);


                    //Test to store users with their tcpclient
                    bool found = false;
                    foreach (KeyValuePair<User, List<TcpClient>> myClients in userTcps)
                    {
                        if (myClients.Key.Equals(connectionUser))
                        {
                            myClients.Value.Add(comm);
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        List<TcpClient> newClient = new List<TcpClient>();
                        newClient.Add(comm);
                        userTcps.Add(connectionUser, newClient);
                    }
                    Console.WriteLine("Added " + connectionUser.Login + " with the comm : " + comm.Client.RemoteEndPoint);


                    Console.WriteLine("Here are the connected users");
                    foreach (User u in connectedUsers)
                        Console.WriteLine("<" + u.Login + ">");


                    Thread connectedActionThread = new Thread(new ParameterizedThreadStart(connectedActions));
                    connectedActionThread.Start(true);

                }
                else
                {
                    //Same as before
                    Net.sendMessage(comm.GetStream(), new MessageUser(testUserConnect.Login, testUserConnect.Password, true));
                    launchApp();
                }
               
            }
            else if(testUser.GetType().Name.Equals("CreateUserMessage"))
            {
                //Receive the datas from the client side
                CreateUserMessage user = (CreateUserMessage)testUser;


                //Add the client to the tab
                users.Add(new User(users.Count + 1, user.Login, user.Password));

                //Print the users created
                Console.WriteLine("Table users updated : ");

                foreach (User u in users)
                    Console.WriteLine(u.Login + " " + u.Password);
                Console.WriteLine("\n");

                //Relaunche the menu when finish creation
                launchApp();
            }

        }

        public void connectedActions(object firstTime)
        {
            bool firstTimeApp = (bool)firstTime;
            //Send all the info about the topics and convs...
            if (firstTimeApp == true)
            {
                firstTime = false;
                Net.sendMessage(comm.GetStream(), new MessageTopics(topics));
            }

            
            //Receive an action
            Message messageMainPage = Net.receiveMessage(comm.GetStream());

            if (messageMainPage.GetType().Name.Equals("MessageTopic"))
            {
                MessageTopic messageTopic = (MessageTopic)messageMainPage;


                foreach (Topic t in topics)
                {
                    if (t.Id_topic == messageTopic.Topic.Id_topic)
                    {
                        //Create the thread to display messages
                        Thread displayTopicThread = new Thread(new ParameterizedThreadStart(displayTopic));
                        displayTopicThread.Start(t);
                    }
                }
            }
        }

        public void displayUsers()
        {
            //Send the users to the client
            Net.sendMessage(comm.GetStream(), new MessageUsers(users));

            //Create a thread to join a topic
            Thread joinPrivateChatThread = new Thread(new ThreadStart(joinPrivateChat));
            joinPrivateChatThread.Start();
        }

        public void joinPrivateChat()
        {
            //Receive the user destination chosen and the user

            //Id of the receiver
            MessageMenu choice = (MessageMenu)Net.receiveMessage(comm.GetStream());
            User userReceiver = new User(0, null, null);

            //Sender 
            MessageUser mu = (MessageUser)Net.receiveMessage(comm.GetStream());
            User userSender = mu.getUser();



            //Select the receiver :
            foreach (User u in users)
            {
                if (choice.MessageSend == u.Id_user)
                {
                    userReceiver.Id_user = choice.MessageSend;
                    userReceiver.Login = u.Login;
                    userReceiver.Password = u.Password;
                }
            }

            List<User> usersChat = new List<User>();

            Console.WriteLine("The sender is " + userSender.Login);
            usersChat.Add(userSender);
            usersChat.Add(userReceiver);

            //Search for the conv in the list of private messages
            bool valid = false;
            PrivateChat oldPrivateChat = new PrivateChat();
            foreach (PrivateChat pv in privateChats)
            {

                if ((usersChat[0].Id_user == pv.UsersChat[0].Id_user && usersChat[1].Id_user == pv.UsersChat[1].Id_user) || (usersChat[1].Id_user == pv.UsersChat[0].Id_user && usersChat[0].Id_user == pv.UsersChat[1].Id_user))
                {
                    valid = true;
                    oldPrivateChat = pv;
                    break;
                }
            }

            Thread displayUserChatThread = new Thread(new ParameterizedThreadStart(displayUserChat));

            if (valid == true)
            {


                //Print the chatters : 
                foreach (User u in oldPrivateChat.UsersChat)
                    Console.WriteLine(u.Login);


                Net.sendMessage(comm.GetStream(), new MessagePrivate(oldPrivateChat));
                displayUserChatThread.Start(oldPrivateChat);
            }
            else if (valid == false)
            {
                //Creation of the private conversation
                string titleChat = "Private Chat with " + userReceiver.Login + " and " + userSender.Login;
                Conversation privateMessage = new Conversation("test private message", userReceiver);
                List<Conversation> privateConversation = new List<Conversation>();
                PrivateChat newPrivateChat = new PrivateChat(1, titleChat, 0, privateConversation, usersChat);

                privateChats.Add(newPrivateChat);


                //Print the chatters : 
                foreach (User u in newPrivateChat.UsersChat)
                    Console.WriteLine(u.Login);


                Net.sendMessage(comm.GetStream(), new MessagePrivate(newPrivateChat));
                displayUserChatThread.Start(newPrivateChat);

            }
        }


        public void displayUserChat(object newPrivateChat)
        {
            PrivateChat messageReceived = (PrivateChat)newPrivateChat;

            messageReceived.printTheConversations();

            while (true)
            {
                MessagePrivate clientMessage = (MessagePrivate)Net.receiveMessage(comm.GetStream());
                Conversation conv = new Conversation(clientMessage.Message, clientMessage.UserSender);

                messageReceived.Conversation.Add(conv);
                messageReceived.Number_messages += 1;

                //Add the client associated with tcpClient
                foreach (KeyValuePair<User, List<TcpClient>> myClients in userTcps)
                {
                    foreach (User u in messageReceived.UsersChat)
                    {
                        if (myClients.Key.Login.Equals(u.Login))
                        {
                            foreach (TcpClient tcp in myClients.Value)
                            {
                                Net.sendMessage(tcp.GetStream(), new MessagePrivate(messageReceived));
                                Console.WriteLine("\n *** Sending to " + tcp.Client.RemoteEndPoint + " ***");
                                Console.WriteLine("Message received : " + clientMessage.Message + "\nfrom : " + clientMessage.UserSender.Login);
                            }
                        }
                    }
                }
            }
        }

        public void createTopic()
        {
            MessageTopic topic = (MessageTopic)Net.receiveMessage(comm.GetStream());

            Topic newTopic = topic.Topic;

            newTopic.Id_topic = topics.Count + 1;
            newTopic.Conversation = new List<Conversation>();
            newTopic.Chatters = new List<User>();

            topics.Add(newTopic);

            //Print the topics created
            Console.WriteLine("Table topics updated : ");

            foreach (Topic t in topics)
                Console.WriteLine(t.Title);
            Console.WriteLine("\n");

            connectedActions(false);
        }

        public void createUser()
        {
            //Receive the datas from the client side
            MessageUser user = (MessageUser)Net.receiveMessage(comm.GetStream());


            //Add the client to the tab
            users.Add(new User(users.Count + 1, user.Login, user.Password));

            //Print the users created
            Console.WriteLine("Table users updated : ");

            foreach (User u in users)
                Console.WriteLine(u.Login + " " + u.Password);
            Console.WriteLine("\n");

            //Relaunche the menu when finish creation
            launchApp();
        }


        public void displayTopics()
        {
            //Send the topics to the client 
            Net.sendMessage(comm.GetStream(), new MessageTopics(topics));
        }

        public void joinTopic(object messageJoin)
        {
            //Receive the topic chosen and the user
            MessageJoin mj = (MessageJoin)messageJoin;
            //Get the topic
            //Topic topic = topics[choice.MessageSend - 1];


            //Add the client to the topic
            bool searchClient = false;
            
            foreach (User u in mj.Topic.Chatters)
                if (u.Id_user.Equals(mj.User.Id_user))
                    searchClient = true;

            if (searchClient == false)
                mj.Topic.Chatters.Add(mj.User.getUser());
            Console.WriteLine("User added " + mj.User.Login);
            /*Net.sendMessage(comm.GetStream(), new MessageTopic(mj.Topic));*/
        }

        public void displayTopic(object topicObject)
        {
            Topic topic = (Topic)topicObject;

            MessageTopic clientMessage = (MessageTopic)Net.receiveMessage(comm.GetStream());

            Conversation conv = new Conversation(clientMessage.Message, clientMessage.UserSender);

            topic.Conversation.Add(conv);
            topic.Number_messages += 1;

            Console.WriteLine("Yoyoyoyoyo");

            
            //Add the client associated with tcpClient
            foreach (KeyValuePair<User, List<TcpClient>> myClients in userTcps)
            {
                foreach (User u in topic.Chatters)
                {
                        
                    if (myClients.Key.Login.Equals(u.Login))
                    {
                            
                        foreach (TcpClient tcp in myClients.Value)
                        {
                            Net.sendMessage(tcp.GetStream(), new MessageTopic(topic));
                            Net.sendMessage(tcp.GetStream(), new MessageTopics(topics));
                            Console.WriteLine("\n *** Sending to " + tcp.Client.RemoteEndPoint + " ***");
                            Console.WriteLine("Message received : " + clientMessage.Message + "\nFor the topic : " + clientMessage.Topic.Title + " from : " + clientMessage.UserSender.Login);
                        }
                    }
                }
            }
            topic.printTheConversations();
  
            Thread connectedActionThread = new Thread(new ParameterizedThreadStart(connectedActions));
            connectedActionThread.Start(false);

        }
    }
}
