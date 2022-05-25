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
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["X-Store"].ConnectionString);
        DataSet ds = new DataSet();
        BindingList<Urun> fatura = new BindingList<Urun>();
        decimal toplamTutar;
        int silinecekUrunIndex;
        private void HomePage_Load(object sender, EventArgs e)
        {
            SqlDataAdapter dap = new SqlDataAdapter("SELECT * FROM Products where UnitsInStock != 0", con);
            dap.Fill(ds);
            ds.Tables[0].TableName = "Urunler";
            dataGridView1.DataSource = ds.Tables["Urunler"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            Urun urun = new Urun
            {
                ProductID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value),
                ProductName = dataGridView1.SelectedRows[0].Cells[1].Value.ToString(),
                UnitPrice = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells[2].Value)
            };
            fatura.Add(urun);
            FaturaGetir();           
        }

        private void FaturaGetir()
        {
            dataGridView2.DataSource = fatura;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.Columns[0].Visible = false;
            toplamTutar = 0;
            foreach (var item in fatura)
            {
                toplamTutar += item.UnitPrice;
            }
            label5.Text = toplamTutar.ToString() + " TL";
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            int urunID = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
            foreach (var item in fatura)
            {
                if (item.ProductID == urunID)
                {
                    silinecekUrunIndex=fatura.IndexOf(item);
                }
            }
            fatura.RemoveAt(silinecekUrunIndex);
            FaturaGetir();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter("SELECT * FROM Customers where PhoneNumber=@gsm", con);
            dap.SelectCommand.Parameters.AddWithValue("@gsm", textBox3.Text);
            dap.Fill(dt);
            if (dt.Rows.Count==0)
            {
                MessageBox.Show("Böyle bir müşteri yok! Müşteriyi kaydedin!");
            }
            else
            {
                SqlCommand orders = new SqlCommand("INSERT Orders (CustomerID,OrderDate) VALUES (@customerID,@date)", con);
                orders.Parameters.AddWithValue("@customerID", Convert.ToInt32(dt.Rows[0][0]));
                orders.Parameters.AddWithValue("@date", DateTime.Now);
                con.Open();
                orders.ExecuteNonQuery();
                con.Close();

                SqlDataAdapter orderID = new SqlDataAdapter("SELECT IDENT_CURRENT('dbo.Orders')", con);
                orderID.Fill(dt2);

                foreach (var item in fatura)
                {
                    SqlCommand orderDetails = new SqlCommand("INSERT OrderDetails (OrderID,ProductID,Quantity) VALUES (@orderID,@pID,1)", con);
                    orderDetails.Parameters.AddWithValue("@orderID", Convert.ToInt32(dt2.Rows[0][0]));
                    orderDetails.Parameters.AddWithValue("@pID", item.ProductID);
                    con.Open();
                    orderDetails.ExecuteNonQuery();
                    con.Close();
                }
                fatura.Clear();
                label5.Text = "-";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomerRegister frm = new CustomerRegister();
            frm.Show();
        }
    }
}
