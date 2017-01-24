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

using NAudio.Wave;
using System.Media;
using System.IO;

using System.Drawing.Imaging;
using WinFormCharpWebCam;

using System.IO.Compression;

namespace VoiceChatAppClient
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream clientStream;
        IPEndPoint serverEndPoint;
        int incall = 0;

        WaveOut _waveOut;
        WaveIn waveIn;
        byte[] bytes = new byte[1600];
        BufferedWaveProvider bwp;

        //webcam
        TcpClient client1;
        NetworkStream clientStream1;
        WebCam webcam;
        int incall1 = 0;
        int camonoroff = 0;

        //text
        TcpClient client2;
        NetworkStream clientStream2;

        //loginstat
        int loginstatus = 0;


        int command;

        string accptcall = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox5.PasswordChar = '*';
            textBox4.PasswordChar = '*';

            groupBox1.Visible = false;
            groupBox2.Visible = true;
            groupBox3.Visible = false;
            client = new TcpClient();

            serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.116.1"), 3000);

            client.Connect(serverEndPoint);

            clientStream = client.GetStream();

            //webcam
            client1 = new TcpClient();

            IPEndPoint serverEndPoint1 = new IPEndPoint(IPAddress.Parse("192.168.116.1"), 3001);

            client1.Connect(serverEndPoint1);

            clientStream1 = client1.GetStream();

            //text tranmsmission
            client2 = new TcpClient();

            IPEndPoint serverEndPoint2 = new IPEndPoint(IPAddress.Parse("192.168.116.1"), 3002);

            client2.Connect(serverEndPoint2);

            clientStream2 = client2.GetStream();

            pictureBox1.Image = pictureBox1.InitialImage;


            try
            {
                _waveOut = new WaveOut();

                int sampleRate = 8000; // 8 kHz
                int channels = 1; // mono

                waveIn = new WaveIn(this.Handle);
                waveIn.BufferMilliseconds = 100;

                //waveIn.DataAvailable += waveIn_DataAvailable;
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker3.RunWorkerAsync();

                

                waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
                waveIn.StartRecording();

                
                bwp = new BufferedWaveProvider(waveIn.WaveFormat);

                _waveOut.Init(bwp);
                _waveOut.Play();





                webcam = new WebCam();
                webcam.InitializeWebCam(ref pictureBox1);
            }
            catch (Exception ee)
            {

                label1.Text = ee.Message;
            }
            backgroundWorker2.RunWorkerAsync();
            backgroundWorker5.RunWorkerAsync();
            //backgroundWorker3.RunWorkerAsync();

            //webcam
            backgroundWorker4.RunWorkerAsync();


        }
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if ( incall==1)
            {
                //System.Threading.Thread.Sleep(10);
                clientStream.Write(e.Buffer, 0, 1600);
                //bytes = e.Buffer;
                
               // bwp.AddSamples(e.Buffer, 0, 400);
                
            }
            else
            {
                bwp.ClearBuffer();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            waveIn.DataAvailable += waveIn_DataAvailable;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //backgroundWorker1.RunWorkerAsync();

            string text = "voicecall+-+" + listBox1.SelectedItem.ToString() + "+-+";
            int size = text.Length;
            string sss = listBox1.SelectedItem.ToString();
            label9.Text = sss.Length.ToString();
            int bsize = size * 4;
            byte[] b = new byte[bsize];
            ASCIIEncoding.ASCII.GetBytes(text, 0, size, b, 0);
            label8.Text = label8.Text + " " + System.Text.Encoding.UTF8.GetString(b);
            clientStream2.Write(b, 0, b.Length);

            //backgroundWorker6.RunWorkerAsync();

            
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true) {
                if (incall == 1)
                {
                    //System.Threading.Thread.Sleep(10);
                    
                        clientStream.Read(bytes, 0, 1600);
                        bwp.AddSamples(bytes, 0, 1600);
                        clientStream.Flush();
                        if (bwp.BufferedDuration.Milliseconds > 500)
                        {
                            bwp.ClearBuffer();
                            //label1.Text=bwp.BufferLength.ToString();
                        }
                    
                }
                else
                {
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true) {
                System.Threading.Thread.Sleep(300);
            //text transmission
           
            //signup
            if(command==1) {
                label8.Text = "Signing up....";
                string text = "signup+-+" + textBox3.Text + "+-+" + textBox4.Text+ "+-+";
                int size = text.Length;
                label8.Text = label8.Text + size;
                int bsize = size * 4;
                byte[] b = new byte[bsize];
                ASCIIEncoding.ASCII.GetBytes(text, 0, size, b, 0);
                label8.Text = label8.Text + " " + System.Text.Encoding.UTF8.GetString(b);
                clientStream2.Write(b, 0, b.Length);
                byte[] message=new byte[1000];
                clientStream2.Read(message, 0, 1000);
                String s = System.Text.Encoding.UTF8.GetString(message);
                String[] s1 = s.Split(new string[] { "+-+" }, StringSplitOptions.None);
                if (s1[0]=="signupok")
                    {
                    label8.Text = "Signup Successful. now you can login";
                }
                else
                {
                    label8.Text = "Username already exist. try different";
                }
                command = 0;
            }
            //login
            else if (command == 2)
            {
                label8.Text = "Logging you to the system....";
                string text = "login+-+" + textBox2.Text + "+-+" + textBox5.Text + "+-+";
                int size = text.Length;
                label8.Text = label8.Text + size;
                int bsize = size * 4;
                byte[] b = new byte[bsize];
                ASCIIEncoding.ASCII.GetBytes(text, 0, size, b, 0);
                label8.Text = label8.Text + " " + System.Text.Encoding.UTF8.GetString(b);
                clientStream2.Write(b, 0, b.Length);
                byte[] message = new byte[1000];
                clientStream2.Read(message, 0, 1000);
                String s = System.Text.Encoding.UTF8.GetString(message);
                String[] s1 = s.Split(new string[] { "+-+" }, StringSplitOptions.None);
                if (s1[0] == "loginok")
                {
                    //label8.Text = "Signup Successful. now you can login";
                    groupBox2.Visible = false;
                    groupBox1.Visible = true;
                    loginstatus = 1;
                    
                }
                else
                {
                    label8.Text = "Username or Password do not match";
                }
                command = 0;
            }
                //add friend
            else if (command == 4)
            {
                label9.Text = "Adding Friend....";
                string text = "addfriend+-+" + textBox2.Text + "+-+" + textBox6.Text + "+-+";
                int size = text.Length;
                label8.Text = label8.Text + size;
                int bsize = size * 4;
                byte[] b = new byte[bsize];
                ASCIIEncoding.ASCII.GetBytes(text, 0, size, b, 0);
                label8.Text = label8.Text + " " + System.Text.Encoding.UTF8.GetString(b);
                clientStream2.Write(b, 0, b.Length);
                byte[] message = new byte[1000];
                clientStream2.Read(message, 0, 1000);
                String s = System.Text.Encoding.UTF8.GetString(message);
                String[] s1 = s.Split(new string[] { "+-+" }, StringSplitOptions.None);
                if (s1[0] == "addfriendok")
                {
                    label9.Text = "Friend added....";
                    listBox1.Items.Add(textBox6.Text);
                }
                else
                {
                    label9.Text = "Error adding....";
                }
                command = 0;
            }
            //drop call
            else if (command == 3)
            {
                string text = "dropcall+-+";
                int size = text.Length;
                label8.Text = label8.Text + size;
                int bsize = size * 4;
                byte[] b = new byte[bsize];
                ASCIIEncoding.ASCII.GetBytes(text, 0, size, b, 0);
                label8.Text = label8.Text + " " + System.Text.Encoding.UTF8.GetString(b);
                clientStream2.Write(b, 0, b.Length);
                command = 0;
            }
            else
            {
                if (clientStream2.DataAvailable == true)
                {
                    byte[] message = new byte[1000];
                    clientStream2.Read(message, 0, 1000);
                    String s = System.Text.Encoding.UTF8.GetString(message);
                    String[] s1 = s.Split(new string[] { "+-+" }, StringSplitOptions.None);
                    if (s1[0] == "dropcall")
                    {
                        incall1 = 0;
                        incall = 0;
                        if (camonoroff != 0)
                        {
                 //webcam.Stop();
                        }
                        label9.Text = "call cut";
                    }
                    if (s1[0] == "friendlist")
                    {
                        string[] ss= s1[1].Split(new string[] { " " }, StringSplitOptions.None);
                        for (int i = 0; i < ss.Length; i++)
                        {
                            listBox1.Items.Add(ss[i]);
                        }
                    }
                    if (s1[0] == "voicecallok")
                    {
                        label9.Text = "Calling...";
                        //incall = 1;

                    }
                    if (s1[0] == "voicecallerror")
                    {
                        label9.Text = "Error Calling...";
                        //label8.Text = "Username or Password do not match";
                    }
                    if (s1[0] == "voicecallaccpt")
                    {
                        groupBox3.Visible = true;
                        label10.Text = "Accept incomint call from " + s1[1];
                        accptcall = s1[1];
                        //label8.Text = "Username or Password do not match";
                    }
                    if (s1[0] == "voicecallaccepted")
                    {
                        label9.Text = "Connected";
                        incall = 1;
                    }
                    if (s1[0] == "cancelcall")
                    {
                        label9.Text = "Call Rejected";
                        incall = 0;
                    }
                }
                else
                {
                    if (loginstatus == 1)
                    {
                        string text = "friendlist+-+";
                        int size = text.Length;
                        label8.Text = label8.Text + size;
                        int bsize = size * 4;
                        byte[] b = new byte[bsize];
                        ASCIIEncoding.ASCII.GetBytes(text, 0, size, b, 0);
                        label8.Text = label8.Text + " " + System.Text.Encoding.UTF8.GetString(b);
                        clientStream2.Write(b, 0, b.Length);
                        loginstatus = 2;
                    }
                }
            }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox1.InitialImage;
            


            if (camonoroff == 0)
            {
                webcam.Start();
                camonoroff = 1;
                incall1 = 1;
            }
            else
            {
                //WebCam webcam1 = new WebCam();
                //webcam1.InitializeWebCam(ref pictureBox1);
                //webcam = webcam1;
                //webcam.Continue();
                incall1 = 1;
            }


            
            
            
            //Thread t = new Thread(new ThreadStart(hwc));
            //t.Start();
            //backgroundWorker5.RunWorkerAsync();
        }
        public void hwc()
        {
            webcam = new WebCam();
            webcam.InitializeWebCam(ref pictureBox1);
            webcam.Start();
            backgroundWorker4.RunWorkerAsync(); 
                
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            pictureBox1.Visible = false;

            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID 
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 15L);

            myEncoderParameters.Param[0] = myEncoderParameter;
            while (true)
            {
                
                if (incall1 == 1)
                {
                    System.Threading.Thread.Sleep(120);
                    try
                    {
                        
                        //pictureBox2.Image=pictureBox2.InitialImage;




                        byte[] b = new byte[2500];

                        Image img = pictureBox1.Image;

                        Bitmap objBitmap = new Bitmap(img, new Size(200, 200));
                        MemoryStream mstream = new MemoryStream();
                        objBitmap.Save(mstream, jpgEncoder, myEncoderParameters);
                        byte[] buffer = mstream.ToArray();

                        //label1.Text = buffer.Length.ToString();
                        //                if (img.Width <= 0) { img = pictureBox1.InitialImage; }

                        ImageConverter converter = new ImageConverter();
                        b = (byte[])converter.ConvertTo(img, typeof(byte[]));
                        if (client.Connected == true)
                        {
                            this.Invoke((MethodInvoker)delegate
    {
        pictureBox3.Image = img;
    });

                            byte[] bb, b1,bb2,b2;
                            bb = buffer;
                            using (var compressIntoMs = new MemoryStream())
                            {
                                using (var gzs = new BufferedStream(new GZipStream(compressIntoMs,
                                 CompressionMode.Compress), 100))
                                {
                                    gzs.Write(bb, 0, bb.Length);
                                }
                                b1 = compressIntoMs.ToArray();
                            }

                            bb2 = b1;

                            using (var compressIntoMs = new MemoryStream())
                            {
                                using (var gzs = new BufferedStream(new GZipStream(compressIntoMs,
                                 CompressionMode.Compress), 100))
                                {
                                    gzs.Write(bb2, 0, bb2.Length);
                                }
                                bb2 = compressIntoMs.ToArray();
                            }

                            b2 = bb2;
                            label11.Text = b2.Length.ToString();


                            label1.Text = buffer.Length.ToString();
                            clientStream1.Write(b1, 0, b1.Length);
                            
                            //clientStream.Flush();
                        }
                        //b=mStream.ToArray();

                        if (b.Length >= 0)
                        {

                            using (MemoryStream mStream = new MemoryStream(b))
                            {

                                //pictureBox2.Image = Image.FromStream(mStream);
                            }
                        }

                    }


                    catch (Exception ee)
                    {
                        label1.Text = ee.Message;
                    }
                    GC.Collect();
                }
                else
                {
                    pictureBox1.Image = pictureBox1.InitialImage;
                    pictureBox3.Image = pictureBox1.Image;
                    System.Threading.Thread.Sleep(5000);
                    GC.Collect();
                }
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] message = new byte[2500];
            int bytesRead;


            while (true)
            {
               // if (clientStream1.DataAvailable == true)
               // {
                    bytesRead = 0;

                    try
                    {
                        //blocks until a client sends a message

                        bytesRead = clientStream1.Read(message, 0, 2500);
                        byte[] b1;

                        using (var compressedMs = new MemoryStream(message))
                        {
                            using (var decompressedMs = new MemoryStream())
                            {
                                using (var gzs = new BufferedStream(new GZipStream(compressedMs,
                                 CompressionMode.Decompress), 100))
                                {
                                    gzs.CopyTo(decompressedMs);
                                }
                                b1= decompressedMs.ToArray();
                            }
                        }

                        label1.Text = message.ToString();
                        using (MemoryStream mStream = new MemoryStream(b1))
                        {
                            pictureBox2.Image = Image.FromStream(mStream);
                        }
                    }
                    catch
                    {
                        //a socket error has occured
                        //break;
                    }

                    if (bytesRead == 0)
                    {
                        //the client has disconnected from the server
                        //break;
                    }

                    //message has successfully been received
                    //ASCIIEncoding encoder = new ASCIIEncoding();
                    //System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
                //}
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            command = 1;
            //backgroundWorker3.RunWorkerAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            command = 2;
            //backgroundWorker3.RunWorkerAsync();
        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] message = new byte[1000];
            clientStream.Read(message, 0, 1000);
            String s = System.Text.Encoding.UTF8.GetString(message);
            String[] s1 = s.Split(new string[] { "+-+" }, StringSplitOptions.None);
            if (s1[0] == "voicecallok")
            {
                //label8.Text = "Signup Successful. now you can login";
                incall = 1;

            }
            else
            {
                label9.Text = "Error Calling...";
                //label8.Text = "Username or Password do not match";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            incall1 = 0;
            incall = 0;
            if (camonoroff != 0)
            {
                //webcam.Stop();
            }
            command = 3;
            //backgroundWorker3.RunWorkerAsync();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            command = 4;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string text = "voicecallaccpt+-+" + accptcall + "+-+";
            int size = text.Length;
            //label9.Text = listBox1.SelectedItem.ToString();
            int bsize = size * 4;
            byte[] b = new byte[bsize];
            ASCIIEncoding.ASCII.GetBytes(text, 0, size, b, 0);
            label8.Text = label8.Text + " " + System.Text.Encoding.UTF8.GetString(b);
            clientStream2.Write(b, 0, b.Length);
            accptcall = null;
            groupBox3.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            incall1 = 0;
            //webcam.Stop();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string text = "cancelcall+-+" + accptcall + "+-+";
            int size = text.Length;
            //label9.Text = listBox1.SelectedItem.ToString();
            int bsize = size * 4;
            byte[] b = new byte[bsize];
            ASCIIEncoding.ASCII.GetBytes(text, 0, size, b, 0);
            label8.Text = label8.Text + " " + System.Text.Encoding.UTF8.GetString(b);
            clientStream2.Write(b, 0, b.Length);
            accptcall = null;
            groupBox3.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            webcam.AdvanceSetting();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}