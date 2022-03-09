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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            timer1.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(i==6)
            {
                timer1.Stop();
                Login_form form11 = new Login_form();
                this.Hide();
                form11.ShowDialog();
                this.Close();
            }
            else
            {
                i++;
            }
        }
    }
}
