using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communication;

namespace ClientGUI
{
    public partial class LoginForm : Form
    {
        public static User user;
        public static TcpClient comm;

        public LoginForm(string h, int p)
        {
            InitializeComponent();
            comm = new TcpClient(h, p);
        }

        private void ok_Click(object sender, EventArgs e)
        {
            Net.sendMessage(comm.GetStream(), new MessageUser(loginText.Text, passwordText.Text, true));

            MessageUser resUser = (MessageUser)Net.receiveMessage(comm.GetStream());

            //Check the boolean received. If false, means you are connected
            if (resUser.Err == true)
            {
                MessageBox.Show("** ERROR of client password / Login ** ");
            }
            else
            {
                //Assign the user the session
                MessageUser mu = (MessageUser)Net.receiveMessage(comm.GetStream());
                user = mu.getUser();
                
                MainPage mainPage = new MainPage(comm, user);
                mainPage.Show();
                this.Hide();

                MessageBox.Show("You are connected as " + user.Login);
            }
            loginText.Text = "";
            passwordText.Text = "";

        }

        private void createAccountButton_Click(object sender, EventArgs e)
        {
            //Net.sendMessage(comm.GetStream(), new CreateUserMessage(loginText.Text, passwordText.Text, true));

        }
    }
}
