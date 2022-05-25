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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["X-Store"].ConnectionString);
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter userValidation = new SqlDataAdapter("Select * from Users where UserName=@uName and Password=@pword",con);
            userValidation.SelectCommand.Parameters.AddWithValue("@uName", textBox1.Text);
            userValidation.SelectCommand.Parameters.AddWithValue("@pword", textBox2.Text);
            userValidation.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                HomePage frm = new HomePage();
                label3.Text = "Giriş yapılıyor";
                //System.Threading.Thread.Sleep(1500);
                frm.Show();
            }
            else
            {
                label3.Text = "Kullanıcı adı veya şifre hatalı!!";
            }
            
        }
    }
}
