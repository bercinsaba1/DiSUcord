using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace server_project
{
    public partial class Form1 : Form
    {
        // initalizations 
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Dictionary<string, Socket> clients = new Dictionary<string, Socket>();
        private List<Socket> IF100_clients = new List<Socket>();
        private List<Socket> SPS101_clients = new List<Socket>();
        // flags 
        bool terminating = false;
        bool listening = false;

        public Form1()
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ensure all connections socets are closed 
            terminating = true;
            listening = false;

            foreach (var client in clients.Values)
            {
                if (client.Connected)
                {
                    client.Close();
                }
            }

            if (serverSocket.Connected)
            {
                serverSocket.Close();
            }

            Environment.Exit(0);
        }

        private void button_listen_Click(object sender, EventArgs e)
        {
            // Starting the server and listening for incoming connections. 
            int serverPort;
            if (int.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(0); // 0 means no upper limit 

                listening = true;
                button_listen.Enabled = false;

                Thread acceptThread = new Thread(new ThreadStart(AcceptClients));
                acceptThread.Start();

                all_clients.AppendText("Server started listening on port: " + serverPort + "\n");
            }
            else
            {
                all_clients.AppendText("Please check port number.\n");
            }
        }

        private void AcceptClients()
        {
            // Continuously accepting clients while the server is listening. 
            while (listening)
            {
                try
                {
                    var clientSocket = serverSocket.Accept();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(clientSocket);
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        all_clients.AppendText("The socket stopped working.\n");


                    }
                }
            }
        }

        private void HandleClient(object client)
        {
            // handle client in a diffrent thread
            Socket clientSocket = (Socket)client;
            bool connected = true;

            try
            {
                // continously listen for messages from the client 
                while (connected && !terminating)
                {
                    Byte[] buffer = new Byte[1024];
                    int bytesRead = clientSocket.Receive(buffer);
                    // if bytes are recevied 
                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        ProcessMessage(message, clientSocket);
                    }
                    else
                    {
                        // if no bytes are read, socket migh have been closed 
                        throw new SocketException(); // Socket closed
                    }
                }
            }
            catch
            {
                // exeption handling and remove client if terminating is false 
                if (!terminating)
                {
                    // Remove client from all lists
                    RemoveClient(clientSocket);
                }
                connected = false;
            }
        }

        private void RemoveClient(Socket clientSocket)
        {
            // find username by sockets search and find speific username 
            string usernameToRemove = FindUsernameBySocket(clientSocket);
            all_clients.AppendText($"{usernameToRemove} has disconnected.\n");
            // if username is not null  
            if (usernameToRemove != null)
                clients.Remove(usernameToRemove);


            if (clientSocket != null)
            {
                //  clients  is a if 100 client 
                if (IF100_clients.Contains(clientSocket))
                {
                    string subscribedUsers = "Currently users in IF 100 are; ";
                    foreach (Socket cli in IF100_clients)
                    {
                        foreach (var pair in clients)
                        {
                            if (pair.Value == cli)   // both looking if 100 clients and all soceket client if they have same username 
                            {
                                subscribedUsers += " " + pair.Key;  // shows the currently subsribed clients at that particular channel 
                            }
                        }
                    }
                    IF100_clients.Remove(clientSocket); 
                    all_clients.AppendText(subscribedUsers + "\n");
                    SendServerMessage(subscribedUsers, "IF 100");
                }

                // smilar logic where the client is a sps 101 client 
                if (SPS101_clients.Contains(clientSocket))
                {
                    string subscribedUsers = "Currently users in SPS 101 are;";
                    foreach (Socket cli in SPS101_clients)
                    {
                        foreach (var pair in clients)
                        {
                            if (pair.Value == cli)
                            {
                                subscribedUsers += " " + pair.Key;
                            }
                        }
                    }
                    SPS101_clients.Remove(clientSocket);
                    all_clients.AppendText(subscribedUsers + "\n");
                    SendServerMessage(subscribedUsers, "SPS 101");
                }


                
            }



            if (clientSocket != null)
            {
                clientSocket.Close();
            }
        }

        private string FindUsernameBySocket(Socket clientSocket)
        {
            // find the username  
            foreach (var pair in clients)
            {
                if (pair.Value == clientSocket)
                {
                    return pair.Key;
                }
            }

            all_clients.AppendText("a client ");
            return null; // or handle this case as you see fit
        }

        private void ProcessMessage(string message, Socket sender)
        {
            //  process incoming messages  actions based on the message content from client 
            string[] parts = message.Split('|');
            //if the message format is valid not less than 3 
            if (parts.Length < 3)
            {
                SendErrorMessage(sender, "Invalid message format.");
                return;
            }

            // Extract information from the message and act accordingly 
            string action = parts[0];
            string username = parts[1];
            string channel = parts[2];

            //  different actions like SUBSCRIBE UNSUBSCRIBE 
            switch (action.ToUpper())
            {
                case "SUBSCRIBE":
                    SubscribeUser(username, channel, sender);
                    //SendMessage(username, channel, string.Join("|", parts, 3, parts.Length - 3)); 
                    break;

                case "UNSUBSCRIBE":
                    UnsubscribeUser(username, channel);
                    //SendMessage(username, channel, string.Join("|", parts, 3, parts.Length - 3)); 
                    break;
                case "USERNAME":
                    if (clients.ContainsKey(channel))
                    {

                        SendErrorMessage(sender, "This username already exists. ");
                        sender.Close();
                        return;
                    }
                    all_clients.AppendText($"{channel} has been connected \n");
                    clients.Add(channel, sender);
                    send_if.Enabled = true;
                    send_sps.Enabled = true;
                    break;

                case "MESSAGE":

                    // both channel check in that case only enters this link 
                    if ((channel == "IF 100 SPS 101" || channel == "SPS 101 IF 100"))

                    {
                        if_clients.AppendText($"{username} has send a message to IF 100 : {parts[3]} \n");
                        sps_clients.AppendText($"{username} has send a message to SPS 101 : {parts[3]} \n");

                        SendMessage(username, channel, string.Join("|", parts, 3, parts.Length - 3));
                    }
                    else if (channel.Contains("IF 100"))
                    {
                        if_clients.AppendText($"{username} has send a message to IF 100 : {parts[3]} \n");
                        SendMessage(username, channel, string.Join("|", parts, 3, parts.Length - 3));
                    }
                    else if (channel.Contains("SPS 101"))
                    {
                        sps_clients.AppendText($"{username} has send a message to SPS 101 : {parts[3]} \n");

                        SendMessage(username, channel, string.Join("|", parts, 3, parts.Length - 3));
                    }



                    break;

                case "INVALID":
                    all_clients.AppendText($"{username} {channel}   \n");
                    break;

                default:
                    SendErrorMessage(sender, "Unknown action.");
                    break;
            }
        }

        private void SendErrorMessage(Socket client, string errorMessage)
        {
            all_clients.AppendText("Error Message: " + errorMessage+"\n");
            try
            {
                Byte[] errorData = Encoding.UTF8.GetBytes($"ERROR|{errorMessage}");
                client.Send(errorData, 0, errorData.Length, SocketFlags.None);
            }
            catch (Exception e)
            {
                //all_clients.AppendText("Client has disconented \n");

            }

        }

        private void SubscribeUser(string username, string channel, Socket client)
        {
            // determine user channel 
            if (!clients.ContainsKey(username))
            {
                clients[username] = client;
            }
            // handle subscrition situation based on channel 

            if (channel == "IF 100" && !IF100_clients.Contains(client))
            {
                IF100_clients.Add(client);
                string msg = $"{username} subscribed to {channel}\n";
                //all_clients.AppendText(msg);
                SendServerMessage(msg, "IF 100");

            }
            else if (channel == "SPS 101" && !SPS101_clients.Contains(client))
            {
                SPS101_clients.Add(client);
                string msg = $"{username} subscribed to {channel}\n";
                all_clients.AppendText(msg);
                SendServerMessage(msg, "SPS 101");
            }
            // error handling 
            else if (channel == "SPS 101" && SPS101_clients.Contains(client))
            {
                SendErrorMessage(client, $"Client already subscribed: {channel}\n");
                return;

            }
            // error handling 
            else if (channel == "IF 100" && IF100_clients.Contains(client))
            {
                SendErrorMessage(client, $"Client already subscribed: {channel}\n");
                return;

            }
            // error handling 
            else if ((channel == "IF 100 SPS 101" || channel == "SPS 101 IF 100") && IF100_clients.Contains(client) && !SPS101_clients.Contains(client))
            {
                SendErrorMessage(client, $"Client already subscribed: IF100 ");
                return;
            }
            // error hanfling 
            else if ((channel == "IF 100 SPS 101" || channel == "SPS 101 IF 100") && !IF100_clients.Contains(client) && SPS101_clients.Contains(client))
            {
                SendErrorMessage(client, $"Client already subscribed: SPS 101");
                return;
            }
            // error handlling 
            else if ((channel == "IF 100 SPS 101" || channel == "SPS 101 IF 100") && IF100_clients.Contains(client) && SPS101_clients.Contains(client))
            {
                SendErrorMessage(client, $"Client already subscribed: SPS 101 and IF 100");
                return;
            }
            // add lists and subscribe 
            else if ((channel == "IF 100 SPS 101" || channel == "SPS 101 IF 100") && !IF100_clients.Contains(client) && !SPS101_clients.Contains(client))
            {
                IF100_clients.Add(client);
                SPS101_clients.Add(client);
                string msg = $"{username} subscribed to SPS 101 and IF 100.\n";
                all_clients.AppendText(msg);

            }
            // currently users 
            if (IF100_clients.Contains(client))
            {
                string subscribedUsers = "Currently users in IF 100 are; ";
                foreach (Socket cli in IF100_clients)
                {
                    foreach (var pair in clients)
                    {
                        if (pair.Value == cli)
                        {
                            subscribedUsers += " " + pair.Key;
                        }
                    }
                }
                all_clients.AppendText(subscribedUsers + "\n");
                SendServerMessage(subscribedUsers, "IF 100");
            }

            // curently users 
            if (SPS101_clients.Contains(client))
            {
                string subscribedUsers = "Currently users in SPS 101 are ";
                foreach (Socket cli in SPS101_clients)
                {
                    foreach (var pair in clients)
                    {
                        if (pair.Value == cli)
                        {
                            subscribedUsers += " " + pair.Key;
                        }
                    }
                }
                all_clients.AppendText(subscribedUsers + "\n");
                SendServerMessage(subscribedUsers, "SPS 101");
            }


        }

        private void UnsubscribeUser(string username, string channel)
        {
            // channel check 2 channel at the same time 
            if ((channel == "IF 100 SPS 101" || channel == "SPS 101 IF 100") && SPS101_clients.Contains(clients[username]) && IF100_clients.Contains(clients[username]))
            {
                IF100_clients.Remove(clients[username]);
                SPS101_clients.Remove(clients[username]);
                string msg = $"{username} unsubscribed to {channel}\n";
                all_clients.AppendText(msg);
                SendServerMessage(msg, "SPS 101 IF 100");



            }
            // if sps 101 channel only 
            else if (channel.Contains("SPS 101") && SPS101_clients.Contains(clients[username]))
            {
                SPS101_clients.Remove(clients[username]);
                SPS101_clients.Remove(clients[username]);
                string msg = $"{username} unsubscribed to SPS 101 \n";
                all_clients.AppendText(msg);
                SendServerMessage(msg, "SPS 101");
            }
            // if if 100 channel only 
            else if (channel.Contains("IF 100") && IF100_clients.Contains(clients[username]))
            {
                IF100_clients.Remove(clients[username]);
                string msg = $"{username} unsubscribed to IF 100 \n";
                all_clients.AppendText(msg);
                SendServerMessage(msg, "IF 100");


            }
            else
            {
                SendErrorMessage(clients[username], $"Invalid channel or not subscribed: {channel}");
                return;
            }
            // show the current users in if 100 
            if (channel.Contains("IF 100"))
            {
                string subscribedUsers = "Currently users in IF 100 are; ";
                foreach (Socket cli in IF100_clients)
                {
                    foreach (var pair in clients)
                    {
                        if (pair.Value == cli)
                        {
                            subscribedUsers += " " + pair.Key;
                        }
                    }
                }
                all_clients.AppendText(subscribedUsers + "\n");
                SendServerMessage(subscribedUsers, "IF 100");
            }
            // current users in sps 101 
            if (channel.Contains("SPS 101"))
            {
                string subscribedUsers = "Currently users in SPS 101 are; ";
                foreach (Socket cli in SPS101_clients)
                {
                    foreach (var pair in clients)
                    {
                        if (pair.Value == cli)
                        {
                            subscribedUsers += " " + pair.Key;
                        }
                    }
                }
                all_clients.AppendText(subscribedUsers + "\n");
                SendServerMessage(subscribedUsers, "SPS 101");
            }


        }

        private void SendMessage(string username, string channel, string message)
        {
            // string formattedMessage = $"{channel}:{username}:{message}";
            // send both of the channel 
            if ((channel == "IF 100 SPS 101" || channel == "SPS 101 IF 100"))
            {
                foreach (var clientSocket in SPS101_clients)
                {
                    try
                    {
                        string formattedMessage = $"SPS 101:{username}:{message}";
                        Byte[] messageData = Encoding.UTF8.GetBytes(formattedMessage);
                        clientSocket.Send(messageData, 0, messageData.Length, SocketFlags.None);

                        // sending to sps client  

                        //all_clients.AppendText($"The message has been send to {(IF100_clients.Count() + SPS100_clients.Count())} user. \n");
                    }
                    catch
                    {
                        // error handling 
                        //all_clients.AppendText("Client has disconented \n");
                    }

                }
                foreach (var clientSocket in IF100_clients)
                {
                    try
                    {
                        string formattedMessage = $"IF 100:{username}:{message}";
                        Byte[] messageData = Encoding.UTF8.GetBytes(formattedMessage);
                        clientSocket.Send(messageData, 0, messageData.Length, SocketFlags.None);

                        // sending if clients 

                    }
                    catch
                    {
                        // error handling 
                        // all_clients.AppendText("Client has disconented \n");
                    }

                }
               all_clients.AppendText($"The message has been send to {(IF100_clients.Count() + SPS101_clients.Count())} SPS 101 and IF 100 user. \n"); 
            }

            else if (channel == "IF 100")
            {
                // sending if client 
                foreach (var clientSocket in IF100_clients)
                {
                    try
                    {
                        string formattedMessage = $"{channel}:{username}:{message}";
                        Byte[] messageData = Encoding.UTF8.GetBytes(formattedMessage);
                        clientSocket.Send(messageData, 0, messageData.Length, SocketFlags.None);
                        
                    }
                    catch { }
                }
                all_clients.AppendText($"The message has been send to IF100 clients {IF100_clients.Count()} IF 100 user. \n\n");
            }
            else if (channel == "SPS 101")
            {
                // sps client send 
                foreach (var clientSocket in SPS101_clients)
                {
                    try
                    {

                        string formattedMessage = $"{channel}:{username}:{message}";
                        Byte[] messageData = Encoding.UTF8.GetBytes(formattedMessage);
                        clientSocket.Send(messageData, 0, messageData.Length, SocketFlags.None);
                        

                    }
                    catch
                    {
                        //all_clients.AppendText("Client has disconented \n");           
                    }

                }
                all_clients.AppendText($"The message has been send to  SPS101 clients {SPS101_clients.Count()} user. \n");
            }

            else
            {
                SendErrorMessage(clients[username], "Invalid channel or already subscribed. ");
            }
        }

        private void SendServerMessage(string serverMessage, string channel)
        {
            // sennding system message when : invalid stuation happens, server send message, sending a client message to other clients message  and subrcitions in current conditions 
            if (channel == "IF 100")
            {
                foreach (var clientSocket in IF100_clients)
                {
                    try
                    {
                        // formating and sending the message 
                        Byte[] messageData = Encoding.UTF8.GetBytes($"SYSTEM:{channel}:{serverMessage}");
                        clientSocket.Send(messageData, 0, messageData.Length, SocketFlags.None);
                       
                       
                    }
                    catch
                    {
                        // all_clients.AppendText("Client has disconented \n");
                    }
                }
                if_clients.AppendText("System has send a message to IF 100: " + serverMessage + "\n");
                all_clients.AppendText($"The message has been send to {(IF100_clients.Count())} IF 100 user.\n\n"); 
            }
            else if (channel == "SPS 101")
            {
                foreach (var clientSocket in SPS101_clients)
                {
                    try
                    {
                        // formatting and sending the message 
                        Byte[] messageData = Encoding.UTF8.GetBytes($"SYSTEM:{channel}:{serverMessage}");
                        clientSocket.Send(messageData, 0, messageData.Length, SocketFlags.None);
                        


                    }
                    catch
                    {
                        //all_clients.AppendText("Client has disconented \n");

                    }

                }
                sps_clients.AppendText("System has send a message to SPS 101: " + serverMessage + "\n");
                all_clients.AppendText($"The message has been send to {(SPS101_clients.Count())} SPS 101 user.\n\n"); 
            }
            else if ((channel == "IF 100 SPS 101" || channel == "SPS 101 IF 100"))
            {
                // sending both if and sps 
                foreach (var clientSocket in SPS101_clients)
                {
                    try
                    {
                        Byte[] messageData = Encoding.UTF8.GetBytes($"SYSTEM: SPS 101 :{serverMessage}");
                        clientSocket.Send(messageData, 0, messageData.Length, SocketFlags.None);


                    }

                    catch
                    {

                        //all_clients.AppendText("Client has disconented \n");
                    }
                }

                foreach (var clientSocket in IF100_clients)
                {
                    try
                    {
                        Byte[] messageData = Encoding.UTF8.GetBytes($"SYSTEM: IF100 :{serverMessage}");
                        clientSocket.Send(messageData, 0, messageData.Length, SocketFlags.None);
                        
                    }
                    catch
                    {
                        //all_clients.AppendText("Client has disconented \n"); 
                    }

                }
                all_clients.AppendText($"The message has been send to {(IF100_clients.Count() + SPS101_clients.Count())} IF 100  and SPS 101 user.\n \n"); 
            }
        }

        private void send_if_Click(object sender, EventArgs e)
        {
            // send server side messge when button is cliked to if 100 clients 
            string serverMessage = textBox_message.Text;
            SendServerMessage(serverMessage, "IF 100");

        }

        private void send_sps_Click(object sender, EventArgs e)
        {
            // smilary send to sps clients 
            string serverMessage = textBox_message.Text;
            SendServerMessage(serverMessage, "SPS 101");
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        
    }
}