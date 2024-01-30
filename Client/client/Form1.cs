using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace client
{
    public partial class ip_text : Form
    {
        // declertions and initalizations of nececary components 
        private Socket client;
        private Thread receiveThread;
        bool terminating = false;
        bool connected = false;
        bool if_ch = false;
        bool sps_ch = false;    
        public ip_text()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(ip_text_ip_textClosing);
            InitializeComponent();
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            // creating the socket 
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // checking username 
            if (string.IsNullOrEmpty(textBoxUsername.Text))
            {
                logs.AppendText("Error: Please enter a username before connecting.\n");
                return;
            }

            // getting ip adress and port 
            string ip = iptext.Text;
            int port;

            if (!int.TryParse(porttext.Text, out port))
            {
                logs.AppendText("Error: Check the port number.\n");
                return;
            }

            try
            {
                // trying to connect 
                client.Connect(ip, port);
                // Updating UI elements based on successful connection. 
                button_connect.Enabled = false;
                text_message.Enabled = true;
                button_subscribe.Enabled = true;
                channell.Enabled = true;

                // Sending username to the server
                string username = textBoxUsername.Text;
                username = "USERNAME|no channel|" + username;
                try
                {
                    Byte[] usernameData = Encoding.UTF8.GetBytes(username);
                    client.Send(usernameData);
                }
                catch { }
                

                // Starting the thread to receive data
                receiveThread = new Thread(new ThreadStart(ReceiveData));
                receiveThread.Start();
                connected = true;

                logs.AppendText($"Connected to the server. Username: {textBoxUsername.Text}\n");
            }
            catch (Exception ex)
            {
                logs.AppendText($"Error connecting to server: {ex.Message}\n");
                
            }
        }

        private void ReceiveData()
        {
            // Continuously listen for incoming data while connected. 
            while (connected)
            {
                try
                {
                    // getting the data 
                    Byte[] buffer = new byte[1024];
                    int bytesRead = client.Receive(buffer);
                    if (bytesRead > 0)

                    {
                        // Processing received data 
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        ProcessIncomingMessage(receivedData);
                    }
                    else
                    {
                        throw new SocketException(); // Throwing exception if bytesRead is 0
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        // disconnection from the server 
                        logs.AppendText("The connection to the server is lost\n");
                    }
                    if_ch = false;
                    sps_ch = false; 
                    connected = false;
                    break;
                }
            }

            // DUZELT !!!!!!!!!!!!!!!!!!!!!! 
            Invoke(new Action(() => {
                button_connect.Enabled = true;
                text_message.Enabled = false;
                button_send.Enabled = false;
            }));
        }

        private void button_subscribe_Click(object sender, EventArgs e)


        {
             
            // check channel box 
            if (string.IsNullOrEmpty(channell.Text))
            {
                logs.Invoke(new Action(() =>
                {
                    logs.AppendText("Error: Please enter a channel before subscribing.\n");
                }));
                return;
            }
            // make the channel to uppercase if any case 
            channell.Text.ToUpper();
            // if invalid channel 
            if (channell.Text != "SPS 101" && channell.Text != "IF 100" && channell.Text != "SPS 101 IF 100" && channell.Text != "IF 100 SPS 101") {
                logs.AppendText("Error: Invlaid channel name. \n ");
                SendData($"INVALID|{textBoxUsername.Text}| user tried to connect to invalid channel \n"); 
                return;
            }
            // UI components 
            button_send.Enabled = true;
            text_message.Enabled = true;
            unsubscribe_button.Enabled = true;
            // Send subscription  to the server 
            bool f = SendData($"SUBSCRIBE|{textBoxUsername.Text}|{channell.Text}");
            // if subscription was successfu 
            if (f) {
                //  if already subscribed to the channel  
                if ((channell.Text.Contains("IF 100") && if_ch) || (channell.Text.Contains("SPS 101") && sps_ch))
                {
                    logs.AppendText("Already subscibed to the channel \n ");
                    SendData($"INVALID|{textBoxUsername.Text}| user tried to connect already subscribed channel \n"); 
                    return;
                }

                //  successful subscription
                //logs.AppendText(textBoxUsername.Text + " has subscribed to " + channell.Text + "\n");
                // Update flags  
                if (channell.Text.Contains("IF 100"))
                {
                    if_ch = true;
                }
                if (channell.Text.Contains("SPS 101"))
                {
                    sps_ch  = true; 
                } 

            } 

        }
        private void unsubscribe_button_Click(object sender, EventArgs e)
        {
            // NOT BURDA CHANNEL NAME ILE USERNAME AYIR 
            // checks 
            if (string.IsNullOrEmpty(textBoxUsername.Text) || string.IsNullOrEmpty(channell.Text))
            {
                logs.Invoke(new Action(() =>
                {
                    logs.AppendText("Error: Please enter both username and channel before unsubscribing.\n");
                    SendData($"INVALID|{textBoxUsername.Text}|user tried to connect without channel name  \n"); 
                }));
                return;
            }

            channell.Text.ToUpper();
            // if valid 
            if (channell.Text != "SPS 101" && channell.Text != "IF 100" && channell.Text != "SPS 101 IF 100" && channell.Text != "IF 100 SPS 101")
            {
                logs.AppendText("Error: Invlaid channel name. \n ");
                // Send invalid channel invalid message to  server 
                SendData($"INVALID|{textBoxUsername.Text}| user tried to connect to invalid channel \n"); 
                return;
            }
            //  unsubscribe request to the server 
            bool c = SendData($"UNSUBSCRIBE|{textBoxUsername.Text}|{channell.Text}");

            if (c)
            {
                // check   not subscribed to the channel and return without action  
                if ((channell.Text.Contains("IF 100") && !if_ch) || (channell.Text.Contains("SPS 101") && !sps_ch))
                {
                    SendData($"INVALID|{textBoxUsername.Text}|{channell.Text}| user tried to connect to not yet subscribed channel \n"); 
                    logs.AppendText( "Not yet subscibed to the channel \n ");
                    return;
                }
                logs.AppendText(textBoxUsername.Text + " has unsubscribed to " + channell.Text + "\n");
                // Update flags  
                if (channell.Text.Contains("IF 100") )
                {
                    if_ch = false;

                }
                if(channell.Text.Contains("SPS 101"))
                {
                    sps_ch = false; 
                }
            }
                
  
            

        }
        private void button_send_Click(object sender, EventArgs e)
        {
            // controls username channel and message box to not be epty 
            if (string.IsNullOrEmpty(textBoxUsername.Text)  )
            {
                logs.Invoke(new Action(() =>
                {
                    logs.AppendText("Error: Please enter a username before sending.\n");
                    SendData($"INVALID|{textBoxUsername.Text}|user tried to connect without username \n");  
                }));
                return;
            }
            if (string.IsNullOrEmpty(channell.Text))
            {
                logs.Invoke(new Action(() =>
                {
                    logs.AppendText("Error: Please enter a channel before sending.\n");
                    SendData($"INVALID|{textBoxUsername.Text}|user tried to connect without channel \n");
                }));
                return; 
            }
            if(string.IsNullOrEmpty(text_message.Text))
            {
                logs.Invoke(new Action(() =>
                {
                    logs.AppendText("Error: Please enter a message before sending.\n");
                    SendData($"INVALID|{textBoxUsername.Text}|user tried to connect without message \n");
                }));
                return; 
            }
            //  channel subscription before sending a message. 
            if ((channell.Text == "IF 100" && !if_ch) || (channell.Text == "SPS 101" && !sps_ch))
            {
                logs.AppendText("Error: Not subscribed to channel: \n"); 
                SendData($"INVALID|{textBoxUsername.Text}| user tried to connect not subscribed channel \n");
                return; 
            }

            // Validate the channel name and flags 
            if (channell.Text != "SPS 101" && channell.Text != "IF 100" && channell.Text != "SPS 101 IF 100" && channell.Text != "IF 100 SPS 101") {
                logs.AppendText("Error: Invlaid channel name. \n ");
                SendData($"INVALID|{textBoxUsername.Text}| user tried to connect to invalid channel \n");
                return;
            }
            if ((channell.Text.Contains("IF 100") && !if_ch) || (channell.Text.Contains("SPS 101") && !sps_ch))
            {
                logs.AppendText("Not subscibed to the channel \n ");
                SendData($"INVALID|{textBoxUsername.Text}| user tried to connect not subscribed channel \n");
                
            }
            bool flag = false; // to check wheter data can be send sucessfully or nor 
            if(sps_ch && if_ch)// both subscribed to sps and if channel 
            {
                // checks for text box as well 
                if((channell.Text == "IF 100"))
                {
                    flag = SendData($"MESSAGE|{textBoxUsername.Text}|IF 100|{text_message.Text}"); 
                }
                else if(channell.Text == "SPS 101")
                {
                    flag = SendData($"MESSAGE|{textBoxUsername.Text}|SPS 101|{text_message.Text}");
                }
                else if ( channell.Text == "SPS 101 IF 100" || channell.Text == "IF 100 SPS 101")
                {
                    flag = SendData($"MESSAGE|{textBoxUsername.Text}|SPS 101 IF 100|{text_message.Text}"); 
                }
                   
            }
                
                
            else if (sps_ch)
                flag = SendData($"MESSAGE|{textBoxUsername.Text}|SPS 101|{text_message.Text}");
            else if(if_ch)
                flag= SendData($"MESSAGE|{textBoxUsername.Text}|IF 100|{text_message.Text}");

            // Send the message to the server based on  the subscribtion 
            if (flag)
                logs.AppendText(textBoxUsername.Text + " has send message to " + channell.Text + "\n"); 
            if(!flag)
                logs.AppendText(textBoxUsername.Text + " could not send message to " + channell.Text + "\n");
        }
        private void ProcessIncomingMessage(string incomingMessage)
        {
            // Split incoming message 
            string[] parts = incomingMessage.Split(':');

            //  if the message has three parts: channel, sender, and message 
            if (parts.Length == 3)
            {
                string channel = parts[0];
                string sender = parts[1];
                string message = parts[2];
                /// SYSYEM: CHANEL: MESSAGE 
                // if  message is a system message 
                if (incomingMessage.StartsWith("SYSTEM"))
                {
                    // Extract system message 
                    string systemMessage = incomingMessage.Substring("SYSTEM:".Length);
                    logs.Invoke(new Action(() =>
                    {
                        logs.AppendText($" {systemMessage} \n");
                    }));
                }
                else
                {
                  
                    logs.Invoke(new Action(() =>
                    {
                        logs.AppendText($"{channel} - {sender}: {message}\n");
                    }));
                }
               
                
            }
            else
            {
                // invalid message 
                logs.Invoke(new Action(() =>
                {
                    logs.AppendText($"Received invalid message : {incomingMessage}\n");

                }));
                // handling error case 
                if (incomingMessage == "ERROR|This username already exists. ")
                {
                    
                    text_message.Enabled = false;

                    button_subscribe.Enabled = false;
                    button_connect.Enabled = true;
                    channell.Enabled = false; 
                }
            }
        } 

       
        private void ip_text_ip_textClosing(object sender, FormClosingEventArgs e)
        {
            // set flags for termination and disconnecetion 
            terminating = true;
            connected = false;
 

            // Gracefully terminating the thread
            /* if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Join();
            }*/
           // exit 
            Environment.Exit(0);
        }

        private bool SendData(string data)
        {
            // check connetion 
            if (connected && client != null)
            {
                try
                {
                    // convert the data  
                    Byte[] buffer = Encoding.UTF8.GetBytes(data);
                    client.Send(buffer);
                    return true; 
                }
                catch   (Exception ex) {
                    logs.AppendText("Error: sending data to the server.\n"); 
                    // return false in case of exception 
                    return false; 
                } 
                
            }
            else
            { 
                // again false and error message 
                logs.AppendText("Error: Not connected to the server.\n");
                return false; 
            }
        }
    }
}
 