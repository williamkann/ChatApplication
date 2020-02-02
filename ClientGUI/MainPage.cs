using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communication;

namespace ClientGUI
{
    public partial class MainPage : Form
    {
        private TcpClient comm;
        private User user;

        public delegate void addConversation(Topic topic);

        public MainPage(TcpClient comm, User user)
        {
            InitializeComponent();
            this.comm = comm;
            this.user = user;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void MainPage_Load(object sender, EventArgs e)
        {
            usernameLabel.Text = LoginForm.user.Login;
            this.Text = "Welcome to the chat application " + usernameLabel.Text;

            //Ask for the list of topics
            MessageTopics topics = (MessageTopics)Net.receiveMessage(comm.GetStream());
            
            foreach(Topic t in topics.Topics)
                this.topicList.Items.Add(t);

            
        }

        private void logoutLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            comm.Close();
            LoginForm loginForm = new LoginForm("127.0.0.1", 8976);
            loginForm.Show();
            this.Hide();
        }

        private void topicList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Topic topic = (Topic) this.topicList.SelectedItem;


            Thread printConversationThread = new Thread(new ParameterizedThreadStart(printConversation));
            printConversationThread.Start(topic);
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            Topic topic = (Topic)this.topicList.SelectedItem;
            
            //Notify that it's a messageTopic that we are sending
            Net.sendMessage(comm.GetStream(), new MessageTopic(topic, messageTextBox.Text, user));

            //Send the actual message Topic
            Net.sendMessage(comm.GetStream(), new MessageTopic(topic, messageTextBox.Text, user));

            MessageTopic topicReceived = (MessageTopic)Net.receiveMessage(comm.GetStream());

            messageTextBox.Text = string.Empty;

            //Update objects of the listBox of topics
            MessageTopics topics = (MessageTopics)Net.receiveMessage(comm.GetStream());

            this.topicList.Items.Clear();


            int i = 0;
            int j = 0;
            foreach (Topic t in topics.Topics)
            {
                this.topicList.Items.Add(t);
                if (t.Id_topic == topicReceived.Topic.Id_topic)
                    i = j;
                j++;
            }


            this.topicList.SelectedIndex = i;
        }


        private void printConversation(object topic)
        {
            addConversation reset = new addConversation(resetTextBox);
            addConversation ac = new addConversation(addSafe);
            Invoke(reset, (Topic)topic);
            Invoke(ac, (Topic)topic);
        }

        private void resetTextBox(Topic voidTopic)
        {
            conversationTextbox.Text = string.Empty;
        }

        private void addSafe(Topic topic)
        {
            foreach(Conversation c in topic.Conversation)
            {
                conversationTextbox.Text += c.User.Login + " : " + c.Message + Environment.NewLine;
            }
        }


        private void conversationTextbox_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void joinButton_Click(object sender, EventArgs e)
        {
/*            Topic topic = (Topic)this.topicList.SelectedItem;

            Net.sendMessage(comm.GetStream(), new MessageJoin(topic, user));*/
        }
    }
}
