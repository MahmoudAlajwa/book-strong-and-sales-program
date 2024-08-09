using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Book_Shop
{
    public partial class BookShop : Form
    {
        public BookShop()
        {
            InitializeComponent();
            populate_user();
            populate();
        }

        private void reset_panels() 
        {
            Books.Hide();
            Users.Hide();
            Dashboard.Hide();
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HP\Desktop\Book Shop\Book Shop\BookShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        int key = 0;
        private void populate()
        {
            con.Open();
            string query = "select * from BookTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookDGV.DataSource = ds.Tables[0];
            con.Close();
        }
        private void Filter()
        {
            con.Open();
            string query = "select * from BookTbl where BCat='" + CatCbSearchCb.SelectedItem.ToString() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void Reset()
        {
            BTitleTb.Text = "";
            BautTb.Text = "";
            BcatCb.SelectedIndex = -1;
            QtyTb.Text = "";
            PriceTb.Text = "";
            YPubTb.Text = "";
            PubTb.Text = "";
            NuPagTb.Text = "";
            ShPoTb.Text = "";
            EditNumTb.Text = "";
        }

        private void BookShop_Load(object sender, EventArgs e)
        {
            Color_Reset();
            button4.BackColor = Color.White;
            pictureBox2.BackColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Color_Reset()
        {
            button4.BackColor = Color.FromArgb(64, 64, 64);
            button5.BackColor = Color.FromArgb(64, 64, 64);
            button6.BackColor = Color.FromArgb(64, 64, 64);
            button7.BackColor = Color.FromArgb(64, 64, 64);
            pictureBox2.BackColor = Color.FromArgb(64, 64, 64);
            pictureBox3.BackColor = Color.FromArgb(64, 64, 64);
            pictureBox4.BackColor = Color.FromArgb(64, 64, 64);
            pictureBox5.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            reset_panels();
            Color_Reset();
            button4.BackColor = Color.White;
            pictureBox2.BackColor = Color.White;
            Books.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            reset_panels();
            Color_Reset();
            button6.BackColor = Color.White;
            pictureBox3.BackColor = Color.White;
            Users.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            reset_panels();
            Color_Reset();
            button5.BackColor = Color.White;
            pictureBox4.BackColor = Color.White;
            Dashboard.Show();

            // loade in dashboard panel
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select sum(BQty) from BookTbl", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            BooksStockLbl.Text = dt.Rows[0][0].ToString();
            SqlDataAdapter sda1 = new SqlDataAdapter("select sum(Amount) from BillTbl", con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            AmountLbl.Text = dt1.Rows[0][0].ToString();
            SqlDataAdapter sda2 = new SqlDataAdapter("select count(*) from UserTbl", con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            UserTotalLbl.Text = dt2.Rows[0][0].ToString();
            con.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Color_Reset();
            button7.BackColor = Color.White;
            pictureBox5.BackColor = Color.White;
            this.Hide();
            Login_admin login = new Login_admin();
            login.Show();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (BTitleTb.Text == "" || BautTb.Text == "" || QtyTb.Text == "" || PriceTb.Text == "" || BcatCb.SelectedIndex == -1 || YPubTb.Text == "" || PubTb.Text == "" || NuPagTb.Text == "" || ShPoTb.Text == "" || EditNumTb.Text == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "insert into BookTbl Values('" + BTitleTb.Text + "','" + BautTb.Text + "','" + BcatCb.SelectedItem.ToString() + "','" + QtyTb.Text + "','" + PriceTb.Text + "','" + YPubTb.Text + "','" + PubTb.Text + "','" + NuPagTb.Text + "','" + ShPoTb.Text + "','" + EditNumTb.Text + "','" + openFileDialog1.FileName + "')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Book Saved successfully");
                    con.Close();
                    populate();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (BTitleTb.Text == "" || BautTb.Text == "" || QtyTb.Text == "" || PriceTb.Text == "" || BcatCb.SelectedIndex == -1 || YPubTb.Text == "" || PubTb.Text == "" || NuPagTb.Text == "" || ShPoTb.Text == "" || EditNumTb.Text == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "Update BookTbl set BTitle='" + BTitleTb.Text + "',BAuthor='" + BautTb.Text + "',BCat='" + BcatCb.SelectedItem.ToString() + "',BQty=" + QtyTb.Text + ",BPrice=" + PriceTb.Text + ",BYearPub=" + YPubTb.Text + ",BPublisher=" + PubTb.Text + ",BNuPages=" + NuPagTb.Text + ",BShelfPos=" + ShPoTb.Text + ",BEditNum=" + EditNumTb.Text + ",BImagePath=" + path + "where BId=" + key + ";";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Book Updated successfully");
                    con.Close();
                    populate();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "delete from BookTbl where BId=" + key + ";";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Book Deleted successfully");
                    con.Close();
                    populate();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
            pictureBox6.Image = Image.FromFile("C:\\Users\\HP\\Desktop\\Book Shop\\Book Shop\\img\\book-outline.png");
        }

        private void CatCbSearchCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter();
        }

        private void bunifuThinButton25_Click(object sender, EventArgs e)
        {
            populate();
            CatCbSearchCb.SelectedIndex = -1;
        }
        string path = "C:\\Users\\HP\\Desktop\\Book Shop\\Book Shop\\img\\book-outline.png";
        private void BookDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                BTitleTb.Text = BookDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                BautTb.Text = BookDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                BcatCb.SelectedItem = BookDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
                QtyTb.Text = BookDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
                PriceTb.Text = BookDGV.Rows[e.RowIndex].Cells[5].Value.ToString();
                YPubTb.Text = BookDGV.Rows[e.RowIndex].Cells[6].Value.ToString();
                PubTb.Text = BookDGV.Rows[e.RowIndex].Cells[7].Value.ToString();
                NuPagTb.Text = BookDGV.Rows[e.RowIndex].Cells[8].Value.ToString();
                ShPoTb.Text = BookDGV.Rows[e.RowIndex].Cells[9].Value.ToString();
                EditNumTb.Text = BookDGV.Rows[e.RowIndex].Cells[10].Value.ToString();
                path = BookDGV.Rows[e.RowIndex].Cells[11].Value.ToString();

                if (BTitleTb.Text == "")
                {
                    key = 0;
                }
                else
                {
                    key = Convert.ToInt32(BookDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please Click On a Used Cell");
            }
        }

        private void populate_user()
        {
            con.Open();
            string query = "select * from UserTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            UserDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void Reset_user()
        {
            UnameTb.Text = "";
            PassTb.Text = "";
            AddTb.Text = "";
            PhoneTb.Text = "";
            NoteTbl.Text = "";
            RoleTbl.Text = "";
        }

        private void bunifuThinButton28_Click(object sender, EventArgs e)
        {
            if (UnameTb.Text == "" || AddTb.Text == "" || PassTb.Text == "" || PhoneTb.Text == "" || RoleTbl.Text == "" || NoteTbl.Text == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    con.Open();
                    string date = $"{datetime.Value:yyyy-MM-dd}";
                    string query = "insert into UserTbl Values('" + UnameTb.Text + "','" + PhoneTb.Text + "','" + AddTb.Text + "','" + PassTb.Text + "','" + RoleTbl.Text + "','" + NoteTbl.Text + "','" + date + "')";//
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Saved successfully");
                    con.Close();
                    populate_user();
                    Reset_user();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void bunifuThinButton27_Click(object sender, EventArgs e)
        {
            if (UnameTb.Text == "" || AddTb.Text == "" || PassTb.Text == "" || PhoneTb.Text == "" || RoleTbl.Text == "" || NoteTbl.Text == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    con.Open();
                    string date = $"{datetime.Value:yyyy-MM-dd}";
                    string query = "Update UserTbl set UName='" + UnameTb.Text + "',UPhone='" + PhoneTb.Text + "',UAdd='" + AddTb.Text + "',UPass='" + PassTb.Text + "',URole='" + RoleTbl.Text + "',UNote='" + NoteTbl.Text + "',DateTime='" + date + "'where UId=" + key + ";";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Updated successfully");
                    con.Close();
                    populate_user();
                    Reset_user();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    con.Open();
                    string date = $"{datetime.Value:yyyy-MM-dd}";
                    string query = "delete from UserTbl where UId=" + key + ";";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Book Deleted successfully");
                    con.Close();
                    populate_user();
                    Reset_user();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            Reset_user();
        }

        private void UserDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UnameTb.Text = UserDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
            PhoneTb.Text = UserDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
            AddTb.Text = UserDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
            PassTb.Text = UserDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
            RoleTbl.Text = UserDGV.Rows[e.RowIndex].Cells[5].Value.ToString();
            NoteTbl.Text = UserDGV.Rows[e.RowIndex].Cells[6].Value.ToString();


            if (UnameTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(UserDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
        }

        private void bunifuThinButton26_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                pictureBox6.Image = Image.FromFile(openFileDialog1.FileName);
                path = openFileDialog1.FileName;
            }
        }
    }
}
