using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using System.IO;
using System.Data.SqlClient;
using Microsoft.Office;
using System.Threading;
using DirectShowLib;
namespace Quick_Auto_Face_Attendence_app
{
    public partial class Attendence_form : Form
    {
        public VideoCapture Webcam { get; set; }
        public LBPHFaceRecognizer FaceRecognition { get; set; }
        public CascadeClassifier FaceDetection { get; set; }
        public CascadeClassifier EyeDetection { get; set; }
        public Mat Frame { get; set; }
        public List<Image<Gray, Byte>> Faces { get; set; }
        public List<int> IDs { get; set; }
        public int Processedimagewigth { get; set; } = 640;
        public int ProcessedimageHeight { get; set; } = 480;
        public int TimerCounter { get; set; } = 0;
        public int TimerLimit { get; set; } = 30;
        public int ScanCounter { get; set; } = 0;
        public string YMLPath { get; set; } = @"C:\Users\Hp\Documents\Trainingproymlfile\an.yml";
       
        public bool Facequare { get; set; } = true;
        public bool cam123 { get; set; } = true;
        public bool Eyesquare { get; set; } = false;
        public bool istraining { get; set; } = false;
        public int result { get; set; }
        public string namestd { get; set; }
        public string nameperson { get; set; }
        List<string> listnames123 { get; set; }
        List<string> distinct { get; set; }
        double FrameRate=100;
        public string[] aa;
        SqlConnection con;

        

        public Attendence_form()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            con = new SqlConnection(@"Data Source=DESKTOP-SPB0LN9\SQLEXPRESS01;Initial Catalog=Facedetectiondatabse;Integrated Security=True");
            FaceRecognition = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
            FaceDetection = new CascadeClassifier(@"C:\Users\Hp\Documents\detectionfile.xml");
            EyeDetection = new CascadeClassifier(@"C:\Users\Hp\Downloads\haarcascade_eye.xml");
            Frame = new Mat();
            Faces = new List<Image<Gray, Byte>>();
            IDs = new List<int>();
            pictureBox1.Image = null;
            listnames123 = new List<string> { };
            distinct = new List<string> { };
            FaceRecognition.Read(@"C:\Users\Hp\Documents\Trainingproymlfile\an.yml");
            MessageBox.Show("Document read has been completed");
            istraining = true;
        }
        
       
        private void Beginwebcam()
        {
            if (cam123)
            {
                if (Webcam == null)
                    Webcam = new VideoCapture();
                Webcam.ImageGrabbed += Webcam_ImageGrabbed;
                FrameRate = Webcam.GetCaptureProperty(CapProp.Fps);
                Webcam.Start();
            }

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

                        //imageframe.ROI = face;
                        //Rectangle newFaceRect = new Rectangle();
                        //newFaceRect.Location = face.Location;
                        //newFaceRect.Y = (int)(face.Y - face.Height / 4);
                        //newFaceRect.X = (int)(face.X - face.Width / 4);
                        //newFaceRect.Height = (int)(face.Height * 1.5);
                        //newFaceRect.Width = (int)(face.Width * 1.5);

                        //imageframe.Draw(face, new Bgr(Color.Orange), 2);
                        //CvInvoke.Rectangle(imageframe, face, new Bgr(Color.Black).MCvScalar, 2);
                        //var processedimage = imageframe.Copy(face).Resize(Processedimagewigth, ProcessedimageHeight, Emgu.CV.CvEnum.Inter.Cubic);
                        //var result = FaceRecognition.Predict(processedimage);
                        if (istraining)
                        {
                            Image<Gray, Byte> grayFaceResult = imageframe.Convert<Gray, Byte>();
                            var processedimage = grayFaceResult.Copy(face).Resize(Processedimagewigth, ProcessedimageHeight, Emgu.CV.CvEnum.Inter.Cubic);
                            
                            var result = FaceRecognition.Predict(processedimage);

                            if (result.Distance < 27)
                            {
                                textBox1.Text = result.Distance.ToString();
                                CvInvoke.PutText(imageframe, result.Label.ToString(), new Point(face.Location.X, face.Location.Y), Emgu.CV.CvEnum.FontFace.HersheyTriplex, 1.0, new Bgr(Color.IndianRed).MCvScalar);

                                imageframe.Draw(face, new Bgr(Color.Green), 2);
                                nameperson = result.Label.ToString();

                                listnames123.Add(nameperson.ToString());

                            }
                            else
                            {
                                textBox1.Text = result.Distance.ToString();
                                CvInvoke.PutText(imageframe, "Unknown", new Point(face.Location.X, face.Location.Y), Emgu.CV.CvEnum.FontFace.HersheyTriplex, 1.0, new Bgr(Color.IndianRed).MCvScalar);
                                imageframe.Draw(face, new Bgr(Color.Green), 2);
                                nameperson = "Unknown";
                            }
                        }
                    }
                    
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
                

                //imageframe.Bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
                pictureBox1.Image = imageframe.ToBitmap();//.Flip(Emgu.CV.CvEnum.FlipType.Horizontal).ToBitmap();


                Thread.Sleep(1);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cam123 == true)
            {
                
                this.Close();
            }
           else if(cam123==false)

            {
                Webcam.Stop();
                Webcam.Dispose();
                this.Close();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            List<string> listnames = new List<string> { }; 
            con.Open();
            string query = "select * from Register";

            SqlCommand cmd = new SqlCommand(query, con);
                

            SqlDataReader myreader2;
            myreader2 = cmd.ExecuteReader();
            while (myreader2.Read())
            {
                listnames.Add(myreader2.GetValue(0).ToString());
            }
            con.Close();

            for (int i = 0; i < listnames.Count; i++)
            {
                string path = @"C:\Users\Hp\source\repos\Quick Auto Face Attendence app\Quick Auto Face Attendence app\bin\Debug\Trainedimages\" + listnames[i] + "";
                string[] files = Directory.GetFiles(path, "*.bmp", SearchOption.AllDirectories);

                foreach (var file in files)
                {

                    Image<Gray, Byte> trainedimage = new Image<Gray, Byte>(file);
                    var processedimage = trainedimage.Resize(Processedimagewigth, ProcessedimageHeight, Emgu.CV.CvEnum.Inter.Cubic);
                    Faces.Add(processedimage);
                    IDs.Add(int.Parse(listnames[i]));


                }


                FaceRecognition.Train(Faces.ToArray(), IDs.ToArray());
                FaceRecognition.Write(@"C:\Users\Hp\Documents\Trainingproymlfile\an.yml");
                MessageBox.Show(listnames[i]);



                //}
            }
            MessageBox.Show("Training Complete");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (cam123 == true)
            {

                Beginwebcam();
                cam123 = false;
                
            }
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Facequare == true)
            {
                Facequare = false;
            }
            else if (Facequare == false)
            {
                Facequare = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            var imageframe = Frame.ToImage<Gray, byte>();
            if (imageframe != null)
            {

                var faces = FaceDetection.DetectMultiScale(imageframe, 1.2, 1);

                for (int i = 0; i < faces.Length; i++)
                {


                    if (faces.Count() != 0)
                    {
                        
                        var processedimage = imageframe.Copy(faces[i]).Resize(Processedimagewigth, ProcessedimageHeight, Emgu.CV.CvEnum.Inter.Cubic);
                        
                        var result = FaceRecognition.Predict(processedimage);
                       
                        if (result.Distance <27)
                        {
                            textBox1.Text = result.Distance.ToString();
                            
                            nameperson = result.Label.ToString();
                            
                            listnames123.Add(nameperson.ToString());
                            
                        }
                        else
                        {
                            textBox1.Text = result.Distance.ToString();
                            
                            nameperson = "Unknown";
                        }
                        //richTextBox1.AppendText(result.Label.ToString() + "\n");

                    }
                    else
                    {

                        

                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            
            if (cam123 == false)
            {

                Facequare = false;
                Webcam.Stop();
                Webcam.Dispose();
                pictureBox1.Image = null;
                cam123 = true;


            }
            List<string> listnames1 = new List<string> { };
            con.Open();
            string query = "select * from Register";

            SqlCommand cmd = new SqlCommand(query, con);


            SqlDataReader myreader2;
            myreader2 = cmd.ExecuteReader();
            while (myreader2.Read())
            {
                listnames1.Add(myreader2.GetValue(0).ToString());
                
            }
            con.Close();
            con.Open();
            SqlCommand cmd1 = con.CreateCommand();
            for (int i = 0; i < listnames1.Count; i++)
            {
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "Update Register set Attendence=" + 0.ToString() + " where ID=" + listnames1[i] + "";
                cmd1.ExecuteNonQuery();
                
            }

            con.Close();
            
            string[] dist = listnames123.Distinct().ToArray();
            for (int i = 0; i < dist.Length; i++)
            {
                //comboBox2.Items.Add(dist[i]);
            }
            try
            {
                
                
                con.Open();

                SqlCommand cmd4 = con.CreateCommand();


                for (int i = 0; i < dist.Length; i++)
                {
                    cmd4.CommandType = CommandType.Text;
                    cmd4.CommandText = "Update Register set Attendence='"+"P"+"' where ID=" + dist[i] + "";
                    cmd4.ExecuteNonQuery();
                }




                con.Close();
            }
            catch(Exception er)
            {
                MessageBox.Show(er.Message);
            }
            for (int i = 0; i <dist.Length; i++)
            {
                listnames1.Remove(dist[i]);
                
            }
            
            try
            {
                

                con.Open();

                SqlCommand cmd4 = con.CreateCommand();


                for (int i = 0; i < listnames1.Count; i++)
                {
                    cmd4.CommandType = CommandType.Text;
                    cmd4.CommandText = "Update Register set Attendence='" + "A" + "' where ID=" + listnames1[i] + "";
                    cmd4.ExecuteNonQuery();
                }




                con.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
            MessageBox.Show("Attendence has been saved Thank you");
            Displaydata();
        }

        private void button8_Click(object sender, EventArgs e)
        {
           
                
           
        }
        public void Displaydata()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Register";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();




        }

        private void button9_Click(object sender, EventArgs e)
        {
            DateTime d = new DateTime();

            string aa = d.ToString("dd,MM,yyyy"); 
            MessageBox.Show(aa);
            try
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
               
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
              
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                 
                worksheet.Name = "Exported from gridview";
                  
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }
                
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                 
                workbook.SaveAs(@"E:\'"+aa.ToString()+"'.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
             
                app.Quit();
                MessageBox.Show("Attendence has been saved to file", "Thank You!");
            }
            catch(Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Beginwebcam();
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            
        }
    }
}

    

