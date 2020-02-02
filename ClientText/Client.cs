using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;

namespace ClientText
{
    class Client
    {

        private string hostname;
        private int port;
        private TcpClient comm;
        private User user;
        private Mutex mutex;

        public Client(string h, int p)
        {
            hostname = h;
            port = p;
            mutex = new Mutex();
        }

        public void start()
        {
            comm = new TcpClient(hostname, port);
            Console.WriteLine(comm.Connected + " with " + comm.Client.RemoteEndPoint);

            //Create a message for the selection of 1 client
            Thread printMainMenuThread = new Thread(new ThreadStart(application));
            printMainMenuThread.Start();
        }

        public void application()
        {
            //Create a message for the selection of the menu
            MessageMenu mn = new MessageMenu(0);
            bool connected = false;
            do
            {
                Console.WriteLine("\t\t*********************************************");
                Console.WriteLine("\t\t********** WELCOME TO THE CHAT APP **********");
                Console.WriteLine("\t\t*********************************************");
                Console.WriteLine("\t\t1. Don't have an account ? Sign up !");
                Console.WriteLine("\t\t2. Log in");
                Console.WriteLine("\t\t3. Close app");

                //Read the user's choice and send it

                mn = new MessageMenu(0);


                mn.MessageSend = Int32.Parse(Console.ReadLine());
                Net.sendMessage(comm.GetStream(), mn);

                //Take the result of the choice print the next steps (create user form or topics form)
                mn = (MessageMenu)Net.receiveMessage(comm.GetStream());

                if (mn.MessageSend == 1)
                {
                    createClient();
                }
                else if (mn.MessageSend == 2)
                {
                    logClient();
                    connected = true;
                }
                else if (mn.MessageSend == 3)
                {
                    comm.Close();
                    System.Environment.Exit(1);
                }
            } while (connected != true);

            if (connected == true)
            {
                Console.WriteLine("You are logged in as " + user.Login);
                connectedActions();
            }
        }

        public void connectedActions()
        {
            //Create a message for the selection of the menu
            MessageMenu mn = new MessageMenu(0);

            Console.WriteLine("\t\t*********************************************");
            Console.WriteLine("\t\t********** WELCOME " + user.Login + " **********");
            Console.WriteLine("\t\t*********************************************");
            Console.WriteLine("\t\t1. Create a topic");
            Console.WriteLine("\t\t2. Display topics");
            Console.WriteLine("\t\t3. Send a private messsage");
            Console.WriteLine("\t\t4. Log out");


            //Read the user's choice and send it
            mn.MessageSend = Int32.Parse(Console.ReadLine());
            Net.sendMessage(comm.GetStream(), mn);

            mn = (MessageMenu)Net.receiveMessage(comm.GetStream());

            if (mn.MessageSend == 1)
                createTopic();
            else if (mn.MessageSend == 2)
                displayTopics();
            else if (mn.MessageSend == 3)
                displayUsers();
            else if (mn.MessageSend == 4)
            {
                Net.sendMessage(comm.GetStream(), new MessageUser(user.Id_user, user.Login, user.Password, true));
                application();
            }
        }
        public void createTopic()
        {
            mutex.WaitOne();
            Console.WriteLine("\tTitle : ");
            string title = Console.ReadLine();

            Net.sendMessage(comm.GetStream(), new MessageTopic(new Topic(0, title, user, 0, null, null)));
            mutex.ReleaseMutex();
            connectedActions();
        }

        public void createClient()
        {
            mutex.WaitOne();
            Console.WriteLine("\tNew login : ");
            string login = Console.ReadLine();
            Console.WriteLine("\tNew password : ");
            string password = Console.ReadLine();
            Console.WriteLine("\tConfirm your password : ");
            string confirmPass = Console.ReadLine();

            if (password.Equals(confirmPass))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Net.sendMessage(comm.GetStream(), new MessageUser(login, password, false));



                mutex.ReleaseMutex();
            }
            else if (!password.Equals(confirmPass))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t/!\\ ** Your password is different from the confirmed password ** /!\\ ");
                createClient();
            }
        }

        public void logClient()
        {

            Console.WriteLine("\t\tLogin : ");
            string login = Console.ReadLine();


            Console.WriteLine("\t\tPassword : ");
            string password = Console.ReadLine();

            Net.sendMessage(comm.GetStream(), new MessageUser(login, password, true));

            MessageUser resUser = (MessageUser)Net.receiveMessage(comm.GetStream());

            //Check the boolean received. If false, means you are connected
            if (resUser.Err == true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t/!\\ ** ERROR of client password / Login ** /!\\");
                logClient();
            }
            else
            {
                //Assign the user the session
                MessageUser mu = (MessageUser)Net.receiveMessage(comm.GetStream());
                user = mu.getUser();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You are connected as " + user.Login);
            }
        }

        public void displayUsers()
        {
            MessageUsers mus = (MessageUsers)Net.receiveMessage(comm.GetStream());

            foreach (User u in mus.Users)
                Console.WriteLine("\t\t" + u.Id_user + " <" + u.Login + ">");

            joinUser();
        }

        public void joinUser()
        {
            MessageMenu mn = new MessageMenu(0);
            MessageUser mu = new MessageUser(user.Id_user, user.Login, user.Password, false);

            Console.WriteLine("\t\tChoose to join a user : ");

            //Send the choice and the user
            mn.MessageSend = Int32.Parse(Console.ReadLine());

            //Send the user that we want to send to 
            Net.sendMessage(comm.GetStream(), mn);

            //Send our user
            Net.sendMessage(comm.GetStream(), mu);

            displayPrivateChat();

        }

        public void displayPrivateChat()
        {
            MessagePrivate mp = (MessagePrivate)Net.receiveMessage(comm.GetStream());
            PrivateChat newMp = mp.PvChat;

            Console.WriteLine("\t\t*****************************************");
            Console.WriteLine("\t\t********** " + newMp.Title + "***********");
            Console.WriteLine("\t\t*****************************************");

            Thread printPrivateConversationThread = new Thread(new ParameterizedThreadStart(printPrivateConversation));
            printPrivateConversationThread.Start(newMp);

            Thread sendPrivateMessageThread = new Thread(new ParameterizedThreadStart(sendPrivateMessage));
            sendPrivateMessageThread.Start(newMp);

        }

        public void printPrivateConversation(object privateConversation)
        {
            PrivateChat messageReceived = (PrivateChat)privateConversation;

            Console.WriteLine("\t\t*** Last conversation ***");
            messageReceived.printTheConversations();

            //Search for the client of PV Chat
            while (true)
            {
                Message msg = Net.receiveMessage(comm.GetStream());
                if (msg.GetType().Name.Equals("MessagePrivate"))
                {
                    MessagePrivate mp = (MessagePrivate)msg;

                    if (mp.PvChat.UsersChat[0].Id_user == messageReceived.UsersChat[0].Id_user || mp.PvChat.UsersChat[0].Id_user == messageReceived.UsersChat[1].Id_user)
                    {
                        if (mp.PvChat.UsersChat[1].Id_user == messageReceived.UsersChat[0].Id_user || mp.PvChat.UsersChat[1].Id_user == messageReceived.UsersChat[1].Id_user)
                        {
                            Console.WriteLine("------------------------------------------------------");
                            mp.PvChat.printTheConversations();
                            Console.WriteLine("Send a message : ");
                        }
                    }
                }
                else if(msg.GetType().Name.Equals("MessageNavigation"))
                    break;

            }

        }

        public void sendPrivateMessage(object privateMessage)
        {
            while (true)
            {
                PrivateChat messageReceived = (PrivateChat)privateMessage;
                string message = Console.ReadLine();

                if(message.Equals("-1"))
                {
                    Net.sendMessage(comm.GetStream(), new MessageNavigation(user, "-1"));
                    break;
                }
                else
                    Net.sendMessage(comm.GetStream(), new MessagePrivate(messageReceived, message, user));
            }

        }

        public void displayTopics()
        {

            MessageTopics topics = (MessageTopics)Net.receiveMessage(comm.GetStream());

            Console.WriteLine("\t\t" + topics.Topics.Count + " topics are online");

            //Display the topic's titles
            int i = 1;
            foreach (Topic t in topics.Topics)
            {
                Console.WriteLine("\t\t" + i + ". " + t.Title);
                i++;
            }

            joinTopic();
        }

        public void joinTopic()
        {

            MessageMenu mn = new MessageMenu(0);
            MessageUser mu = new MessageUser(user.Id_user, user.Login, user.Password, false);

            Console.WriteLine("\t\tChoose to join / update a topic : ");

            //Send the choice and the user
            mn.MessageSend = Int32.Parse(Console.ReadLine());
            Net.sendMessage(comm.GetStream(), mn);
            Net.sendMessage(comm.GetStream(), mu);

            displayTopic();

        }

        public void displayTopic()
        {
            //Get the topic to display
            MessageTopic receivedTopic = (MessageTopic)Net.receiveMessage(comm.GetStream());
            Topic topic = receivedTopic.Topic;

            Console.WriteLine("\t\t*****************************************");
            Console.WriteLine("\t\t********** " + topic.Title + "***********");
            Console.WriteLine("\t\t*****************************************");

            printChatters(topic);
            printAuthor(topic);
            printNumber_message(topic);

            Thread printConversationThread = new Thread(new ParameterizedThreadStart(printConversation));
            printConversationThread.Start(topic);

            Thread sendMessageThread = new Thread(new ParameterizedThreadStart(sendMessage));
            sendMessageThread.Start(topic);
        }

        public void printChatters(Topic topic)
        {
            Console.WriteLine("\t\tHere are the list of chatters : ");
            foreach (User u in topic.Chatters)
                Console.WriteLine("\t\t<" + u.Login + ">");
        }

        public void printAuthor(Topic topic)
        {
            Console.WriteLine("\t\tAuthor of the topic : <" + topic.Author.Login + ">");
        }

        public void printNumber_message(Topic topic)
        {
            Console.WriteLine("\t\tTot. Number of messages : " + topic.Number_messages);
        }

        //Threads to manage the conversation
        public void printConversation(object topic)
        {
            Topic topicReceived = (Topic)topic;

            Console.WriteLine("\t\t*** Last conversation ***");
            topicReceived.printTheConversations();

            while (true)
            {
                Message msg = Net.receiveMessage(comm.GetStream());
                if (msg.GetType().Name.Equals("MessageTopic"))
                {
                    MessageTopic receivedTopic = (MessageTopic)msg;

                    Console.WriteLine("Updated topic " + receivedTopic.Topic.Title);
                    Console.WriteLine("------------------------------------------------------");
                    receivedTopic.Topic.printTheConversations();

                    Console.WriteLine("Send a message : ");
                }
                else if (msg.GetType().Name.Equals("MessageNavigation"))
                    break;
            }
        }

        public void sendMessage(object topic)
        {

            while (true)
            {

                Topic topicReceived = (Topic)topic;
                string message = Console.ReadLine();
                if (message.Equals("-1"))
                {
                    Net.sendMessage(comm.GetStream(), new MessageNavigation(user, "-1"));
                    break;
                }
                else
                    Net.sendMessage(comm.GetStream(), new MessageTopic(topicReceived, message, user));
            }
            connectedActions();

        }
    }
}
