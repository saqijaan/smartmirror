using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using SmartMirror.Sqlite;
using System.IO;
using System.Drawing.Imaging;

using SmartMirror.Models;
using SmartMirror.ImageProcessing;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace SmartMirror
{
    public partial class Form1 : Form
    {
        private ImageProcessor processor;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFormData();
            //startCortanaInterface();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (users.SelectedIndex < 0){
                MessageBox.Show("Please Select a User First");return;
            }
            try
            {
                
                face face = new face();
                face.UserId = (users.SelectedItem as User).Id;
                using (MemoryStream stream = new MemoryStream())
                {
                    pictureBox2.Image.Save(stream, ImageFormat.Bmp);
                    face.Image = stream.ToArray();
                }
                if (!face.Save())
                {
                    MessageBox.Show(User.LastError);
                }
                else
                {
                    new ImageRecognizer().Train();   
                    MessageBox.Show("Face Saved");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            processor = new ImageProcessor();
            processor.StartProcessing(pictureBox1,pictureBox2,processUser);
        }

        private void processUser(int id)
        {
            
            User user =  User.find(id);

            if (user != null)
            {
                recognizedUser.Invoke((MethodInvoker) delegate {
                    recognizedUser.Text = String.Format(String.Format("Name : {0}\n Email: {1}\n Phone : {2}", user.Name,user.Email,user.Phone));
                });
                //Debug.Write(String.Format("Name : {0}\n Email: {1}\n Phone : {2}", user.Name,user.Email,user.Phone));
                //Debug.Write(String.Format(String.Format("Name : {0}\n Email: {1}\n Phone : {2}\n Password : {4}", user.Name, user.Email, user.Phone, user.Password)));
            }else{
                recognizedUser.Invoke((MethodInvoker)delegate {
                    recognizedUser.Text = ""; //String.Format(String.Format("Name : {0}\n Email: {1}\n Phone : {2}", user.Name, user.Email, user.Phone));
                });
            }

        }

        private void LoadFormData()
        {
            users.DataSource = User.get();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (processor != null)
            {
                processor.Stop();
            }
        }

        private void startCortanaInterface()
        {
            Task.Run(()=> {
                TcpListener _server = SocketInterface.start();
                while (true)
                {
                    if (_server.Pending())
                    {
                        TcpClient client = _server.AcceptTcpClient();
                        byte[] data = new byte[client.Available];
                        client.Client.Receive(data);
                        client.Client.Send(SocketInterface.encode("HTTP/1.1 \r\n200 Ok"));
                        client.Close();
                        Debug.WriteLine(SocketInterface.decode(data)); 
                        Thread.Sleep(100);
                    }
                }
            });
        }

        private void addUser_Click(object sender, EventArgs e)
        {
            new NewUser().ShowDialog();
            users.DataSource = User.get();
        }
    }
}
