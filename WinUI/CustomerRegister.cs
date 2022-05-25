using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinUI
{
    public partial class CustomerRegister : Form
    {
        public CustomerRegister()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["X-Store"].ConnectionString);
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT Customers (Name,LastName,PhoneNumber,Address) VALUES (@name,@lastname,@gsm,@address)", con);
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                cmd.Parameters.AddWithValue("@lastname", textBox2.Text);
                cmd.Parameters.AddWithValue("@gsm", textBox3.Text);
                cmd.Parameters.AddWithValue("@address", textBox4.Text);
                con.Open();
                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    MessageBox.Show("Bilgiler başarıyla kaydedildi.");
                }
                else
                {
                    MessageBox.Show("Bilgiler Kaydedilemedi");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State==ConnectionState.Open)
                {
                    con.Close();
                }
                MessageBox.Show("satır:43"+"\n"+ex.Message);
            }
        }
    }
}
