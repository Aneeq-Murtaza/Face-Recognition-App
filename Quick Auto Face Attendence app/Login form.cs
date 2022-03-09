using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Quick_Auto_Face_Attendence_app
{
    public partial class Login_form : Form
    {
        SqlConnection con;
        public Login_form()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=DESKTOP-SPB0LN9\SQLEXPRESS01;Initial Catalog=Facedetectiondatabse;Integrated Security=True");
        }

        private void Login_form_Load(object sender, EventArgs e)
        {
            textBox4.PasswordChar = '*';
            con.Open();
            string query = "select * from login";

            SqlCommand cmd = new SqlCommand(query, con);


            SqlDataReader myreader2;
            myreader2 = cmd.ExecuteReader();
            while (myreader2.Read())
            {
                comboBox1.Items.Add(myreader2.GetValue(0));

            }
            con.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                

                string query = "Select * from login where username='" + comboBox1.Text + "' and password='" + textBox4.Text + "'";
                

                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dta = new DataTable();
                sda.Fill(dta);
                
                if (dta.Rows.Count == 1 )
                {


                    MainWindow main = new MainWindow();
                    this.Hide();
                    main.ShowDialog();
                    this.Close();


                }
                
                else
                {
                    MessageBox.Show("INVALID USER NAME OR PASSWORD");
                }

            }

            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }

        }
    }
}
