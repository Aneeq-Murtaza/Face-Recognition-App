using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
namespace Quick_Auto_Face_Attendence_app
{
    public partial class Registerform : Form
    {
        public VideoCapture Webcam { get; set; }
        public EigenFaceRecognizer FaceRecognition { get; set; }
        public CascadeClassifier FaceDetection { get; set; }
        public CascadeClassifier EyeDetection { get; set; }
        public Mat Frame { get; set; }
        public List<Image<Gray, Byte>> Faces { get; set; }
        public List<int> IDs { get; set; }
        public int Processedimagewigth { get; set; } = 640;
        public int ProcessedimageHeight { get; set; } = 480;
        
        public string YMLPath { get; set; } = @"C:\Users\Hp\Documents\Trainingproymlfile\an.yml";
        //public Timer Timer { get; set; }
        public bool Facequare { get; set; } = false;
        public bool Eyesquare { get; set; } = false;
        public int result { get; set; }
        public string namestd { get; set; }
        SqlConnection con;
        public Registerform()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=DESKTOP-SPB0LN9\SQLEXPRESS01;Initial Catalog=Facedetectiondatabse;Integrated Security=True");
            FaceRecognition = new EigenFaceRecognizer(80, double.PositiveInfinity);
            FaceDetection = new CascadeClassifier(@"C:\Users\Hp\Documents\detectionfile.xml");
            EyeDetection = new CascadeClassifier(@"C:\Users\Hp\Downloads\haarcascade_eye.xml");
            Frame = new Mat();
            Faces = new List<Image<Gray, Byte>>();
            IDs = new List<int>();
            pictureBox1.Image = null;
            Beginwebcam();
            func();
        }
        private void Beginwebcam()
        {
            if (Webcam == null)
                Webcam = new VideoCapture();
            Webcam.ImageGrabbed += Webcam_ImageGrabbed;
            Webcam.Start();



        }
        public void func()
        {
            string query = "select * from Register";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);


            SqlDataReader myreader;
            myreader = cmd.ExecuteReader();
            while (myreader.Read())
            {
                textBox1.Text = (Convert.ToInt32(myreader[0]) + 1).ToString();
            }
            con.Close();
        }
        private void Webcam_ImageGrabbed(object sender, EventArgs e)
        {
            Webcam.Retrieve(Frame);
            var imageframe = Frame.ToImage<Bgr, byte>();
            if (imageframe != null)
            {
                var grayframe = imageframe.Convert<Gray, byte>();
                Rectangle[] faces = FaceDetection.DetectMultiScale(grayframe, 1.2, 1, Size.Empty);
                var eyes = EyeDetection.DetectMultiScale(grayframe, 1.3, 1);

                if (Facequare)
                {
                    foreach (var face in faces)
                    {
                        // Rectangle newFaceRect = new Rectangle();
                        //newFaceRect.Location = face.Location;
                        //newFaceRect.Y = (int)(face.Y - face.Height / 4);
                        //newFaceRect.X = (int)(face.X - face.Width / 4);
                        //newFaceRect.Height = (int)(face.Height * 1.5);
                        //newFaceRect.Width = (int)(face.Width * 1.5);
                        imageframe.Draw(face, new Bgr(Color.Green), 2);
                        //imageframe.Draw(face, new Bgr(Color.Orange), 2);
                        //CvInvoke.Rectangle(imageframe, face, new Bgr(Color.Black).MCvScalar, 2);
                        Image<Gray, Byte> resultimg = imageframe.Convert<Gray, Byte>();

                        resultimg.ROI = face;
                        pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox3.Image = resultimg.Bitmap;
                    }
                    //CvInvoke.PutText(imageframe,result.ToString(), new Point(face.Location.X, face.Location.Y), Emgu.CV.CvEnum.FontFace.HersheyComplex, 1.0, new Bgr(Color.Red).MCvScalar);
                    //for (int i = 0; i < faces.Length; i++)
                    //{
                    //    try
                    //    {
                    //        CircleF circle = new CircleF();
                    //        float x = (int)(faces[i].X + (faces[i].Width / 2));
                    //        float y = (int)(faces[i].Y + (faces[i].Height / 2));
                    //        circle.Radius = faces[i].Width / 2;
                    //        imageframe.Draw(new CircleF(new PointF(x, y), circle.Radius), new Bgr(Color.Yellow), 1);
                    //    }
                    //    catch (Exception)
                    //    {
                    //        throw;
                    //    }
                    //}
                }
                if (Eyesquare)
                    foreach (var eye in eyes)
                        //imageframe.Draw(eye, new Bgr(Color.Yellow), 2);

                        for (int i = 0; i < eyes.Length; i++)
                        {
                            try
                            {
                                CircleF circle = new CircleF();
                                float x = (int)(eyes[i].X + (eyes[i].Width / 2));
                                float y = (int)(eyes[i].Y + (eyes[i].Height / 2));
                                circle.Radius = eyes[i].Width / 2;
                                imageframe.Draw(new CircleF(new PointF(x, y), circle.Radius), new Bgr(Color.Red), 2);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                else
                {

                }
                pictureBox2.Image = imageframe.ToBitmap();

                Thread.Sleep(1);

            }
        }

        private void Registerform_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Webcam.Stop();
            Webcam.Dispose();
            this.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into Register values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + "0" + "')";
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("NEW PERSON ADDED", "THANK YOU!");
            func();
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            richTextBox1.Text = "";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                namestd = textBox1.Text.ToString();
                string currentPath = Directory.GetCurrentDirectory();
                if (Directory.Exists(Path.Combine(currentPath, @"Trainedimages\" + textBox1.Text + "")) == true)
                {
                    richTextBox1.AppendText("Same ID Exits" + "\n");

                }
                else
                {
                    Directory.CreateDirectory(Path.Combine(currentPath, @"Trainedimages\" + textBox1.Text + ""));
                    timer1.Start();


                        





                }


            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }





        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (i < 200)
            {
                i++;
                string path = @"C:\Users\Hp\source\repos\Quick Auto Face Attendence app\Quick Auto Face Attendence app\bin\Debug\Trainedimages\%name%\".Replace("%name%", namestd);
                string path1 = path + "%value%.bmp".Replace("%value%", i.ToString());

                pictureBox3.Image.Save(path1, System.Drawing.Imaging.ImageFormat.Bmp);
                label5.Text = i.ToString();
                richTextBox1.AppendText(i.ToString() + "\n");
            }
            else
            {
                timer1.Stop();
                MessageBox.Show("Completed image saving successfully");
            }
        }
    

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(Facequare==true)
            {
                Facequare = false;
            }
            else if(Facequare == false)
            {
                Facequare = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Register where ID="+textBox1.Text+"";
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("DELETED", "THANK YOU!");
            func();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
