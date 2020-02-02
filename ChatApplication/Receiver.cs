using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;

namespace ChatApplication
{
    
    public class Receiver
    {
        private TcpClient comm;
        private List<User> users;
        private User user;
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
            //Get the choice on the menu from the user 

            MessageMenu choice = (MessageMenu)Net.receiveMessage(comm.GetStream());

            if (choice.MessageSend == 1)
            {
                //Launch the thread to create a new user
                MessageMenu mn = new MessageMenu(1);
                Net.sendMessage(comm.GetStream(), mn);
                createUser();
            }
            else if (choice.MessageSend == 2)
            {
                //Launch the connection thread
                MessageMenu mn = new MessageMenu(2);
                Net.sendMessage(comm.GetStream(), mn);
                doConnection();

            }
            else if (choice.MessageSend == 3)
            {
                //Exit the app
                MessageMenu mn = new MessageMenu(3);
                Net.sendMessage(comm.GetStream(), mn);
            }
        }

        public void doConnection()
        {

            Console.WriteLine("Connecting to the chat app");

            //Used to check if the user is valid
            bool valid = false;

            // read expression
            MessageUser testUser = (MessageUser)Net.receiveMessage(comm.GetStream());

            Console.WriteLine("Login and password received");
            // send result 

            foreach (User u in users)
                if (testUser.Login.Equals(u.Login) && testUser.Password.Equals(u.Password))
                    valid = true;


            if (valid)
            {
                //Send the user with a boolean to display the result (if success or not)
                Net.sendMessage(comm.GetStream(), new MessageUser(testUser.Login, testUser.Password, false));

                //Make the connection by putting the user in the object connectionUser
                User connectionUser = null;
                foreach (User u in users)
                    if (u.Login.Equals(testUser.Login) && u.Password.Equals(testUser.Password))
                        connectionUser = u;

                //Add the user to the list of connected people and send back the user to the client
                user = connectionUser;
                connectedUsers.Add(connectionUser);
                Net.sendMessage(comm.GetStream(), new MessageUser(connectionUser.Id_user, connectionUser.Login, connectionUser.Password, true));
                Console.WriteLine("You are connected as " + connectionUser.Login);



                //Store users with their tcpclient
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


                //Create the thread for 1 client
                Thread connectedActionThread = new Thread(new ThreadStart(connectedActions));
                connectedActionThread.Start();

            }
            else
            {
                //Same as before
                Net.sendMessage(comm.GetStream(), new MessageUser(testUser.Login, testUser.Password, true));
                doConnection();
            }

        }

        public void connectedActions()
        {
            //Get the choice on the menu from the user 
            MessageMenu choice = (MessageMenu)Net.receiveMessage(comm.GetStream());

            if (choice.MessageSend == 1)
            {
                //Launch the thread to create a new topic
                MessageMenu mn = new MessageMenu(1);
                Net.sendMessage(comm.GetStream(), mn);
                Thread newTopicThread = new Thread(new ThreadStart(createTopic));
                newTopicThread.Start();
            }
            else if (choice.MessageSend == 2)
            {
                //Launch the connection thread
                MessageMenu mn = new MessageMenu(2);
                Net.sendMessage(comm.GetStream(), mn);
                Thread displayTopicThread = new Thread(new ThreadStart(displayTopics));
                displayTopicThread.Start();
            }
            else if (choice.MessageSend == 3)
            {
                MessageMenu mn = new MessageMenu(3);
                Net.sendMessage(comm.GetStream(), mn);
                Thread displayUserThread = new Thread(new ThreadStart(displayUsers));
                displayUserThread.Start();
            }
            else if (choice.MessageSend == 4)
            {
                MessageMenu mn = new MessageMenu(4);
                Net.sendMessage(comm.GetStream(), mn);

                //Delete the log out user from the connected users 
                MessageUser mu = (MessageUser)Net.receiveMessage(comm.GetStream());

                Console.WriteLine(connectedUsers.Remove(mu.getUser()));

                launchApp();
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
                Message msg = Net.receiveMessage(comm.GetStream());
                
                if (msg.GetType().Name.Equals("MessagePrivate"))
                {
                    MessagePrivate clientMessage = (MessagePrivate)msg;
                    Conversation conv = new Conversation(clientMessage.Message, clientMessage.UserSender);

                    messageReceived.Conversation.Add(conv);
                    messageReceived.Number_messages += 1;

                    //search the client associated with tcpClient
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
                else if(msg.GetType().Name.Equals("MessageNavigation"))
                {
                    Console.WriteLine("Exit Private conv for " + user.Login);

                    //Check for the clients we want to remove
                    messageReceived.UsersChat.Remove(user);
                    Net.sendMessage(comm.GetStream(), new MessageNavigation(user, "-1"));
                    break;
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

            connectedActions();
        }

        public void createUser()
        {
            //Receive the datas from the client side
            MessageUser user = (MessageUser)Net.receiveMessage(comm.GetStream());


            //Add the client to the tab
            users.Add(new User(users.Count + 1, user.Login, user.Password));


            //Update the file of users
            UserSerialize us = new UserSerialize(users);
            us.SerializeNow();

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

            //Create a thread to join a topic
            Thread joinTopicThread = new Thread(new ThreadStart(joinTopic));
            joinTopicThread.Start();

        }

        public void joinTopic()
        {
            //Receive the topic chosen and the user
            MessageMenu choice = (MessageMenu)Net.receiveMessage(comm.GetStream());
            MessageUser mu = (MessageUser)Net.receiveMessage(comm.GetStream());

            //Get the topic
            Topic topic = topics[choice.MessageSend - 1];


            //Add the client to the topic
            bool searchClient = false;
            foreach (User u in topic.Chatters)
                if (u.Id_user.Equals(mu.Id_user))
                    searchClient = true;

            if (searchClient == false)
                topic.Chatters.Add(mu.getUser());

            Net.sendMessage(comm.GetStream(), new MessageTopic(topic));

            //Create the thread to display messages
            Thread displayTopicThread = new Thread(new ParameterizedThreadStart(displayTopic));
            displayTopicThread.Start(topic);

        }

        public void displayTopic(object topicObject)
        {
            Topic topic = (Topic)topicObject;

            while (true)
            {
                Message msg = Net.receiveMessage(comm.GetStream());

                if (msg.GetType().Name.Equals("MessageTopic"))
                {
                    if (msg == null)
                    {
                        Console.WriteLine("Client stopped responding");
                        comm.Dispose();
                        return;
                    }
                    MessageTopic clientMessage = (MessageTopic)msg;

                    Conversation conv = new Conversation(clientMessage.Message, clientMessage.UserSender);

                    topic.Conversation.Add(conv);
                    topic.Number_messages += 1;

                    //Check for the clients we want to send the message to
                    foreach (KeyValuePair<User, List<TcpClient>> myClients in userTcps)
                    {
                        foreach (User u in topic.Chatters)
                        {
                            if (myClients.Key.Login.Equals(u.Login))
                            {
                                foreach (TcpClient tcp in myClients.Value)
                                {
                                    Net.sendMessage(tcp.GetStream(), new MessageTopic(topic));
                                    Console.WriteLine("\n *** Sending to " + tcp.Client.RemoteEndPoint + " ***");
                                    Console.WriteLine("Message received : " + clientMessage.Message + "\nFor the topic : " + clientMessage.Topic.Title + " from : " + clientMessage.UserSender.Login);
                                }
                            }
                        }
                    }
                }
                else if (msg.GetType().Name.Equals("MessageNavigation"))
                {
                    Console.WriteLine("Exit Topic for " + user.Login);

                    //Check for the clients we want to send the message to
                    topic.Chatters.Remove(user);
                    Net.sendMessage(comm.GetStream(), new MessageNavigation(user, "-1"));
                    break;
                }
            }
            connectedActions();
        }
    }
}
