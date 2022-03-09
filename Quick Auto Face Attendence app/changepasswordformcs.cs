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
    public partial class changepasswordformcs : Form
    {
        SqlConnection con;
        public changepasswordformcs()
        {
          
            InitializeComponent();
            con = new SqlConnection(@"Data Source=DESKTOP-SPB0LN9\SQLEXPRESS01;Initial Catalog=Facedetectiondatabse;Integrated Security=True");
            Displaydata();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                MessageBox.Show("PLEASE FILL BOTH BOXES");
            }
            else if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("PLEASE FILL THE BOX");
            }
            else
            {
                
                string query = "Select * from login where username='" + textBox1.Text + "'";

                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dta = new DataTable();
                sda.Fill(dta);


                if (dta.Rows.Count == 1)
                {
                    List<String> stringArr = new List<String>();

                    for (int a = 0; a < dta.Rows.Count; a++)
                    {
                        stringArr.Add(dta.Rows[a]["username"].ToString());
                        if (stringArr[a] != textBox1.Text)
                        {
                            con.Open();
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "insert into login values('" + textBox1.Text + "','" + textBox2.Text + "')";
                            cmd.ExecuteNonQuery();
                            con.Close();
                            Displaydata();
                            MessageBox.Show("NEW PASSWORD ADDED", "THANK YOU!");

                        }
                        else
                        {
                            MessageBox.Show("THIS ID ALREADY EXITS");
                        }
                    }



                }
                else
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into login values('" + textBox1.Text + "','" + textBox2.Text + "')";
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Displaydata();
                    MessageBox.Show("NEW PASSWORD ADDED", "THANK YOU!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from login where username='" + textBox1.Text + "'";
            cmd.ExecuteNonQuery();
            con.Close();
            Displaydata();
            MessageBox.Show("PASSWORD DELETED", "THANK YOU!");
        }
        public void Displaydata()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from login";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();




        }
    }
}
