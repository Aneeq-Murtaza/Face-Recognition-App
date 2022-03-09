using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quick_Auto_Face_Attendence_app
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            label2.Text = "AI has provided us the opportunities to start real time detection";
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Registerform register = new Registerform();
            register.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
          
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Attendence_form att = new Attendence_form();
            att.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            changepasswordformcs form = new changepasswordformcs();
            form.Show();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
