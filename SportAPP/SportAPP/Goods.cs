using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportAPP
{
    public partial class Goods : Form
    {
        DataBase db = new DataBase();
        int _userRole;
        public Goods(int role = 1)
        {
            InitializeComponent();

            if (role == 2)
            {
                button2.Visible = false;
            }
            if (role == 1)
            {
                button2.Visible = false;
                button3.Visible = false;
            }
            _userRole = role;
            CreateColumn();
            RefreshDataGrid(dataGridView1);
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
                dgw.Rows.Add(record.GetString(1), record.GetDecimal(6));
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from [Product]";
            SqlCommand command = new SqlCommand(queryString, db.GetConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
            db.closeConnection();
        }
        private void CreateColumn()
        {
            dataGridView1.Columns.Add("ProductName","ProductName");
            dataGridView1.Columns.Add("ProductCost","ProductCost");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            listBox1.Items.Add(row.Cells[0].Value.ToString()); // Предполагается, что товары находятся в первом столбце
            }
            catch
            {
                MessageBox.Show("Выберите название товара");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddGood f2 = new AddGood();
            f2.ShowDialog();
            this.Show();
        }

        private void SortButton_Click(object sender, EventArgs e)
        {
            
            dataTable.DefaultView.RowFilter = string.Format("Login LIKE '%{0}%'", SortTB.Text);
            dataGridViewDatesOfEnter.DataSource = dataTable;

        }
    }
}
