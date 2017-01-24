using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace voiceChatServer
{
    public partial class Form1 : Form
    {
        TcpListener tcpListener;
        Thread listenThread;

        List<Thread> clientThread = new List<Thread>();

        //TcpClient[] tcpClient;
        //NetworkStream[] clientStream;

        List<TcpClient> tcpClient = new List<TcpClient>();
        List<NetworkStream> clientStream = new List<NetworkStream>();
        
        int numberofconnectedclients=0;
        //int[] incll;
        //int[] callto;
        List<int> incll = new List<int>();
        List<int> callto = new List<int>();

        int clioentnumber = 100000;
        private byte[][] bytes;


        //webcamm
        TcpListener tcpListener1;
        Thread listenThread1;
        List<Thread> clientThread1 = new List<Thread>();

        //Thread[] clientThread1;

        //TcpClient[] tcpClient1;
        //NetworkStream[] clientStream1;

        List<TcpClient> tcpClient1 = new List<TcpClient>();
        List<NetworkStream> clientStream1 = new List<NetworkStream>();


        int numberofconnectedclients1 = 0;
        //int[] incll1;
        //int[] callto1;
        List<int> incll1 = new List<int>();
        List<int> callto1 = new List<int>();

        int clioentnumber1 = 100000;
        //private byte[][] bytes1;


        //text transmit
        TcpListener tcpListener2;
        Thread listenThread2;

        //Thread[] clientThread2;

        List<Thread> clientThread2 = new List<Thread>();

        //TcpClient[] tcpClient2;
        //NetworkStream[] clientStream2;

        List<TcpClient> tcpClient2 = new List<TcpClient>();
        List<NetworkStream> clientStream2 = new List<NetworkStream>();


        int numberofconnectedclients2 = 0;


        DBConnect mysqldb;

        //friend
        string[] online;
        int online_count=0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mysqldb = new DBConnect();
            if (mysqldb.OpenConnection() == true) { label1.Text = "Database Connected"; }
            else { label1.Text = "Could not connect to mysql database"; }

            //tcpClient = new TcpClient[clioentnumber];
            //clientStream = new NetworkStream[clioentnumber];
            //clientThread = new Thread[clioentnumber]; //Thread(new ParameterizedThreadStart(HandleClientComm));

            

            //webcam
            //tcpClient1 = new TcpClient[clioentnumber];
            //clientStream1 = new NetworkStream[clioentnumber];
            //clientThread1 = new Thread[clioentnumber];
            //incll1 = new int[clioentnumber];
            //callto1 = new int[clioentnumber];
            //wc f

            //text transmit
            //tcpClient2 = new TcpClient[clioentnumber];
            //clientStream2 = new NetworkStream[clioentnumber];
            //clientThread2 = new Thread[clioentnumber];
            //tt f

            //incll = new int[clioentnumber];
            //callto=new int[clioentnumber];

            online = new string[clioentnumber];

            bytes = new byte[clioentnumber][];
            int i=0;
            while (i < clioentnumber)
            {
                bytes[i]=new byte[1600];
                i++;
            }
            this.tcpListener = new TcpListener(IPAddress.Any, 3000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();

            webcam_con();
            text_trans_con();
        }
        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client


                //clientThread[numberofconnectedclients] = new Thread(new ParameterizedThreadStart(HandleClientComm));
                //clientThread[numberofconnectedclients].Start(client);
                clientThread.Add(new Thread(new ParameterizedThreadStart(HandleClientComm)));
                clientThread[numberofconnectedclients].Start(client);
            }
        }
        private void HandleClientComm(object client)
        {
            //tcpClient[numberofconnectedclients] = (TcpClient)client;
            //clientStream[numberofconnectedclients] = tcpClient[numberofconnectedclients].GetStream();

            tcpClient.Add((TcpClient)client);
            clientStream.Add(tcpClient[numberofconnectedclients].GetStream());

            //incll[numberofconnectedclients] = 0;
            //callto[numberofconnectedclients] = 0;
            incll.Add(0);
            callto.Add(0);
            int id = numberofconnectedclients;

            int csss = 0;

            numberofconnectedclients++;
            while(true) {
                if (tcpClient[id].Connected == false) break;
                //label1.Text = "Aloop" + id;
            try
            {
            while (incll[id] == 0)
            {
                
                if (tcpClient[id].Connected == false) break;
                System.Threading.Thread.Sleep(5000);
                
            }


            while (incll[id] == 1 && incll[callto[id]] == 1)
                {
                    if (tcpClient[id].Connected == false) break;
                    clientStream[id].ReadTimeout=3000;
                        byte[] message = new byte[1600];
                        clientStream[id].Read(message, 0, 1600);
                        clientStream[callto[id]].Write(message, 0, 1600);
                    
                }
                
            }
            catch (Exception ee)
            {
                label1.Text = ee.Message;
            }
        }
            
            
        }
        //webcam-----------------------
        void webcam_con()
        {
            this.tcpListener1 = new TcpListener(IPAddress.Any, 3001);
            this.listenThread1 = new Thread(new ThreadStart(ListenForClients1));
            this.listenThread1.Start();
        }
        private void ListenForClients1()
        {
            this.tcpListener1.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client1 = this.tcpListener1.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                //clientThread1[numberofconnectedclients1] = new Thread(new ParameterizedThreadStart(HandleClientComm1));
                //clientThread1[numberofconnectedclients1].Start(client1);

                clientThread1.Add(new Thread(new ParameterizedThreadStart(HandleClientComm1)));
                clientThread1[numberofconnectedclients1].Start(client1);
            }
        }
        private void HandleClientComm1(object client)
        {
            //tcpClient1[numberofconnectedclients1] = (TcpClient)client;
            //clientStream1[numberofconnectedclients1] = tcpClient1[numberofconnectedclients1].GetStream();

            tcpClient1.Add((TcpClient)client);
            clientStream1.Add(tcpClient1[numberofconnectedclients1].GetStream());

            //incll1[numberofconnectedclients1] = 0;
            //callto1[numberofconnectedclients1] = 0;

            incll1.Add(0);
            callto1.Add(0);

            int id = numberofconnectedclients1;
            numberofconnectedclients1++;
            while (true)
            {
                if (tcpClient1[id].Connected == false) break;
                try
                {
                    while (incll[id] == 0)
                    {
                        if (tcpClient1[id].Connected == false) break;
                        System.Threading.Thread.Sleep(5000);
                        
                    }
                    while (incll[id] == 1 && incll[callto[id]] == 1)
                    {
                        if (tcpClient1[id].Connected == false) break;
                        byte[] message = new byte[2500];
                        clientStream1[id].Read(message, 0, 2500);
                        clientStream1[callto1[id]].Write(message, 0, 2500);
                    }

                }
                catch (Exception ee)
                {
                    label1.Text = ee.Message;
                   
                }
            }

        }


        //text transmission-----------------------
        void text_trans_con()
        {
            this.tcpListener2 = new TcpListener(IPAddress.Any, 3002);
            this.listenThread2 = new Thread(new ThreadStart(ListenForClients2));
            this.listenThread2.Start();
        }
        private void ListenForClients2()
        {
            this.tcpListener2.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client2 = this.tcpListener2.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                //clientThread2[numberofconnectedclients2] = new Thread(new ParameterizedThreadStart(HandleClientComm2));
                //clientThread2[numberofconnectedclients2].Start(client2);

                clientThread2.Add(new Thread(new ParameterizedThreadStart(HandleClientComm2)));
                clientThread2[numberofconnectedclients2].Start(client2);
            }
        }
        private void HandleClientComm2(object client)
        {
            //tcpClient2[numberofconnectedclients2] = (TcpClient)client;
            //clientStream2[numberofconnectedclients2] = tcpClient2[numberofconnectedclients2].GetStream();

            tcpClient2.Add((TcpClient)client);
            clientStream2.Add(tcpClient2[numberofconnectedclients2].GetStream());

            int id = numberofconnectedclients2;
            numberofconnectedclients2++;
            try
            {
                label1.Text = "connected " + numberofconnectedclients2;
                while (true)
                {
                    if (tcpClient2[id].Connected == false) break;
                    try
                        {
                    byte[] message=new byte[1000];
                    //label1.Text = "REading";
                    /*using (MemoryStream ms = new MemoryStream())
                        {
                            label1.Text = "REading";
                            //while (clientStream2[id].DataAvailable == false) { }
                            //clientStream2[id].CopyTo(ms);
                            clientStream2[id].Read(message, 0, 1000);
                            label1.Text = ms.Length.ToString();
                            message=ms.ToArray();
                        }
                     */
                    clientStream2[id].Read(message,0,1000);
                    
                    label1.Text = System.Text.Encoding.UTF8.GetString(message);
                    String s = System.Text.Encoding.UTF8.GetString(message);
                    label1.Text = s;
                    String[] s1 = s.Split(new string[] { "+-+" }, StringSplitOptions.None);
                    
                    if (s1[0]=="signup")
                    {
                        if (mysqldb.signup(s1[1], s1[2]) == true)
                        {
                            //label1.Text = "ok";
                            int size = "signupok+-+".Length;
                            int bsize=size*4;
                            byte[] b = new byte[bsize];
                            ASCIIEncoding.ASCII.GetBytes("signupok+-+", 0, size, b,0);
                            clientStream2[id].Write(b,0,b.Length);
                        }
                        else
                        {
                            //label1.Text = "error";
                            int size = "signuperror+-+".Length;
                            int bsize = size * 4;
                            byte[] b = new byte[bsize];
                            ASCIIEncoding.ASCII.GetBytes("signuperror+-+", 0, size, b, 0);
                            clientStream2[id].Write(b, 0, b.Length);
                        }
                    }
                        //login
                    else if (s1[0] == "login")
                    {
                        if (mysqldb.login(s1[1], s1[2]) == true)
                        {
                            label1.Text = "ok"+s1[0];
                            int size = "loginok+-+".Length;
                            int bsize = size * 4;
                            byte[] b = new byte[bsize];
                            ASCIIEncoding.ASCII.GetBytes("loginok+-+", 0, size, b, 0);
                            clientStream2[id].Write(b, 0, b.Length);

                            for (int i = 0; i < online_count; i++)
                            {
                                string[] un = online[i].Split(new string[] { "+-+" }, StringSplitOptions.None);
                                if (un[0] == s1[1])
                                {
                                    online[i] = "###+-+###+-+";
                                    break;
                                }
                                else
                                {

                                }
                            }
                            online[online_count]=s1[1] + "+-+" + id+"+-+";
                            label2.Text = online[online_count].ToString();
                            online_count++;
                        }
                        else
                        {
                            label1.Text = "error";
                            int size = "loginerror+-+".Length;
                            int bsize = size * 4;
                            byte[] b = new byte[bsize];
                            ASCIIEncoding.ASCII.GetBytes("loginerror+-+", 0, size, b, 0);
                            clientStream2[id].Write(b, 0, b.Length);
                        }
                    }
                        //drop call
                    else if (s1[0] == "dropcall")
                    {
                        incll[id] = 0;
                        incll[callto[id]] = 0;
                        incll1[id] = 0;
                        incll1[callto[id]] = 0;
                        int size = "dropcall+-+".Length;
                        int bsize = size * 4;
                        byte[] b = new byte[bsize];
                        ASCIIEncoding.ASCII.GetBytes("dropcall+-+", 0, size, b, 0);
                        clientStream2[callto[id]].Write(b, 0, b.Length);
                        clientStream2[id].Write(b, 0, b.Length);
                    }
                        //add friend
                    else if (s1[0] == "addfriend")
                    {
                        if (mysqldb.addfriend(s1[1], s1[2]) == true)
                        {
                            label1.Text = "ok";
                            int size = "addfriendok+-+".Length;
                            int bsize = size * 4;
                            byte[] b = new byte[bsize];
                            ASCIIEncoding.ASCII.GetBytes("addfriendok+-+", 0, size, b, 0);
                            clientStream2[id].Write(b, 0, b.Length);
                        }
                        else
                        {
                            label1.Text = "error";
                            int size = "addfrienderror+-+".Length;
                            int bsize = size * 4;
                            byte[] b = new byte[bsize];
                            ASCIIEncoding.ASCII.GetBytes("addfrienderror+-+", 0, size, b, 0);
                            clientStream2[id].Write(b, 0, b.Length);
                        }
                    }
                        //friendlist 
                    else if (s1[0] == "friendlist")
                    {
                        string flist=null;
                        label2.Text = online[id];
                        for (int i = 0; i < online_count; i++)
                        {
                            string[] un = online[i].Split(new string[] { "+-+" }, StringSplitOptions.None);
                            label2.Text = un[1];
                            if (un[1] == id.ToString())
                            {
                                flist=mysqldb.flist(un[0]);
                                label1.Text = flist;
                            }
                        }
                        int size = ("friendlist+-+"+flist+"+-+").Length;
                        int bsize = size * 4;
                        byte[] b = new byte[bsize];
                        ASCIIEncoding.ASCII.GetBytes(("friendlist+-+" + flist + "+-+"), 0, size, b, 0);
                        clientStream2[id].Write(b, 0, b.Length);
                    }

                    else if (s1[0] == "voicecall")
                {
                        label3.Text="";
                        for(int i=0;i<online_count;i++) {
                            label3.Text = label3.Text + online[i] + " ";
                        }
                    int csss=0;
                    for (int i = 0; i < online_count; i++)
                    {
                        string[] un = online[i].Split(new string[] { "+-+" }, StringSplitOptions.None);
                        label1.Text = un[0].ToString() + " " + s1[1].ToString() + " ";

                        if (un[0] == "###") continue;
                        int check_receiver=Int32.Parse(un[1]);
                        if (un[0].ToString() == s1[1].ToString() && incll[check_receiver] == 0 && un[0]!="###")
                        {
                            //video part
                            callto1[id] = Int32.Parse(un[1]);
                            //label1.Text = callto1[id].ToString();
                            incll1[id] = 1;

                            callto[id] = Int32.Parse(un[1]);
                            label2.Text = un[0] + " " + un[1].ToString() + " css:" + csss;
                            incll[id] = 1;


                            csss = 1;
                            label2.Text = label2.Text+" css:" + csss;
                            int size = "voicecallok+-+".Length;
                            int bsize = size * 4;
                            byte[] b = new byte[bsize];
                            ASCIIEncoding.ASCII.GetBytes("voicecallok+-+", 0, size, b, 0);
                            clientStream2[id].Write(b, 0, b.Length);




                            for (int inr = 0; inr < online_count; inr++)
                            {
                                string[] una = online[inr].Split(new string[] { "+-+" }, StringSplitOptions.None);

                                if (una[0] == "###") continue;
                                //label1.Text = una[0].Length.ToString() + " " + s1[1].Length.ToString() + " ";
                                if (una[1] == id.ToString())
                                {
                                    int size1 = ("voicecallaccpt+-+" + una[0] + "+-+").Length;
                                    int bsize1 = size1 * 4;
                                    byte[] b1 = new byte[bsize1];
                                    ASCIIEncoding.ASCII.GetBytes(("voicecallaccpt+-+" + una[0] + "+-+"), 0, size1, b1, 0);
                                    clientStream2[callto[id]].Write(b1, 0, b1.Length);
                                }
                            }
                            break;
                            
                        }
                        else
                        {

                        }
                    }
                    if (csss == 0)
                    {
                        int size = "voicecallerror+-+".Length;
                        int bsize = size * 4;
                        byte[] b = new byte[bsize];
                        ASCIIEncoding.ASCII.GetBytes("voicecallerror+-+", 0, size, b, 0);
                        clientStream2[id].Write(b, 0, b.Length);
                    }
                   
                    label2.Text = label2.Text + "   " + csss;
                }
                        //voice accpt
                    else if (s1[0] == "voicecallaccpt")
                    {
                        int csss = 0;
                        for (int i = 0; i < online_count; i++)
                        {
                            string[] un = online[i].Split(new string[] { "+-+" }, StringSplitOptions.None);
                            label1.Text = un[0].Length.ToString() + " " + s1[1].Length.ToString() + " ";
                            if (un[0] == "###") continue;
                            if (un[0] == s1[1])
                            {
                                //video part
                                callto1[id] = Int32.Parse(un[1]);
                                //label1.Text = callto1[id].ToString();
                                incll1[id] = 1;

                                callto[id] = Int32.Parse(un[1]);
                                label2.Text = un[0] + " " + un[1].ToString() + " css:" + csss;
                                incll[id] = 1;

                                

                                csss = 1;
                                label2.Text = label2.Text + " css:" + csss;
                                int size = "voicecallaccepted+-+".Length;
                                int bsize = size * 4;
                                byte[] b = new byte[bsize];
                                ASCIIEncoding.ASCII.GetBytes("voicecallaccepted+-+", 0, size, b, 0);
                                clientStream2[id].Write(b, 0, b.Length);

                                //notify user if call accepted
                                
                                clientStream2[callto[id]].Write(b, 0, b.Length);

                            }
                            else
                            {

                            }
                        }
                        if (csss == 0)
                        {
                            int size = "voicecallerror+-+".Length;
                            int bsize = size * 4;
                            byte[] b = new byte[bsize];
                            ASCIIEncoding.ASCII.GetBytes("voicecallerror+-+", 0, size, b, 0);
                            clientStream2[id].Write(b, 0, b.Length);
                        }

                        label2.Text = label2.Text + "   " + csss;
                    }
                        //drop incoming call
                    else if (s1[0] == "cancelcall")
                    {
                        int csss = 0;
                        for (int i = 0; i < online_count; i++)
                        {
                            string[] un = online[i].Split(new string[] { "+-+" }, StringSplitOptions.None);
                            label1.Text = un[0].Length.ToString() + " " + s1[1].Length.ToString() + " ";
                            if (un[0] == s1[1])
                            {
                                //calceller part
                                //video part
                                callto1[id] = Int32.Parse(un[1]);
                                //label1.Text = callto1[id].ToString();
                                incll1[id] = 0;

                                callto[id] = Int32.Parse(un[1]);
                                label2.Text = un[0] + " " + un[1].ToString() + " css:" + csss;
                                incll[id] = 0;
                                //calceller part end




                                //caller part
                                //video part
                                
                                incll1[callto1[id]] = 0;
                                //voice part
                                incll[callto1[id]] = 0;

                                //caller part end



                                csss = 1;
                                label2.Text = label2.Text + " css:" + csss;
                                int size = "cancelcall+-+".Length;
                                int bsize = size * 4;
                                byte[] b = new byte[bsize];
                                ASCIIEncoding.ASCII.GetBytes("cancelcall+-+", 0, size, b, 0);
                                

                                //notify user if call accepted

                                clientStream2[callto[id]].Write(b, 0, b.Length);

                            }
                            else
                            {

                            }
                        }
                        if (csss == 0)
                        {
                            int size = "voicecallerror+-+".Length;
                            int bsize = size * 4;
                            byte[] b = new byte[bsize];
                            ASCIIEncoding.ASCII.GetBytes("voicecallerror+-+", 0, size, b, 0);
                            clientStream2[id].Write(b, 0, b.Length);
                        }
                    }
                /*callto[id] = Int32.Parse(s);
                label1.Text = callto[id].ToString();
                incll[id] = 1;
                 */
                    
                        }
                    catch (Exception ee)
                    {
                        //label1.Text = ee.Message;
                        if (tcpClient2[id].Connected == false) break;
                    }
                }

            }
            catch (Exception ee)
            {
                label1.Text = ee.Message;
            }


        }
        //text transmission ok



        private void button1_Click(object sender, EventArgs e)
        {
            mysqldb.CloseConnection();
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string>[] gu=new List<string>[3];
            gu = mysqldb.getusers();
            int i = 0;
            label2.Text = gu[0].Count().ToString();
            while (i < gu[1].Count())
            {
                label2.Text = label2.Text + " " + gu[1][i];
                i++;
            }
        }
    }
}
