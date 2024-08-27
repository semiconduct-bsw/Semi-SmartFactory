using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20240809_ServerMain
{
    public partial class FormSearchInventory : Form
    {
        private string connectionString = "User Id=scott;Password=tiger;" +
                                          "Data Source=(DESCRIPTION=" +
                                          "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                                          "(HOST=192.168.0.2)(PORT=1521)))" +
                                          "(CONNECT_DATA=(SERVER=DEDICATED)" +
                                          "(SERVICE_NAME=xe)));";
        public FormSearchInventory()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonShowOrder_Click(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM ORDERCAR ORDER BY 주문날짜";
                OracleDataAdapter adapter = new OracleDataAdapter(query, connection);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void buttonShowInventory_Click(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM PART";
                OracleDataAdapter adapter = new OracleDataAdapter(query, connection);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView2.DataSource = dataTable;
            }
        }
    }
}
