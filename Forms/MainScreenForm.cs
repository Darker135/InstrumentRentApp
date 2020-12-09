using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstrumentRentApp.Forms;

namespace InstrumentRentApp
{
    public partial class MainScreenForm : Form
    {
        public static string SelectedRowInfo;
        static readonly AddOrderForm orderForm = new AddOrderForm();
        static readonly ChangeOrderForm changeForm = new ChangeOrderForm();
        static DataTable table = new DataTable();
        static readonly string dataFileName = @"OrderInfo.txt";
        static readonly string pathToDataFile = Path.Combine(Environment.CurrentDirectory);
        public static readonly string fullPathToDataFile = Path.Combine(pathToDataFile, dataFileName);

        public MainScreenForm()
        {
            InitializeComponent();
        }

        

        private void MainScreenForm_Load(object sender, EventArgs e)
        {
            ManageDgv();
            CheckFileExistance();
            TxtToDgv();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            orderForm.ShowDialog();
        }

        private void CheckFileExistance()
        {
            if (!File.Exists(fullPathToDataFile))
                using (var stream = File.Create(fullPathToDataFile))
                {
                    stream.Dispose();
                }
        }

        private void ManageDgv()
        {
            table.Columns.Add("Client", typeof(string));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Amount", typeof(int));
            table.Columns.Add("Starting date", typeof(DateTime));
            table.Columns.Add("Ending date", typeof(DateTime));
            table.Columns.Add("Daily price", typeof(string));
            table.Columns.Add("Profit", typeof(string));

            dataGridView1.DataSource = table;
        }

        public static void TxtToDgv()
        {
            table.Rows.Clear();
            string[] lines = File.ReadAllLines(fullPathToDataFile);
            string[] values;

            for(int i = 0; i < lines.Length;i++)
            {
                values = lines[i].ToString().Split('&');
                string[] row = new string[values.Length];
                for(int j = 0; j < values.Length; j++)
                {
                    row[j] = values[j].Trim().TrimEnd('\n');
                }
                table.Rows.Add(row);
            }
        }

        private void DeleteNote()
        {
            DataGridViewRow row = this.dataGridView1.SelectedRows[0];
            string Client = row.Cells["Client"].Value.ToString();
            string Product = row.Cells["Product"].Value.ToString();
            string Amount = row.Cells["Amount"].Value.ToString();
            DateTime StartingDate = DateTime.Parse(row.Cells["Starting date"].Value.ToString());
            DateTime EndingDate = DateTime.Parse(row.Cells["Ending date"].Value.ToString());
            string DailyPrice = row.Cells["Daily price"].Value.ToString();
            string Profit = row.Cells["Profit"].Value.ToString();

            string StrToSearch = $@"{Client}&{Product}&{Amount}&{StartingDate.ToString("d")}&{EndingDate.ToString("d")}&{DailyPrice}&{Profit}";
            
            List<string> quotelist = File.ReadAllLines(fullPathToDataFile).ToList();
                quotelist.Remove(StrToSearch);
            File.WriteAllLines(fullPathToDataFile, quotelist.ToArray());
            TxtToDgv();
            MessageBox.Show("Note has successfully deleted", "OK");
        }

        public string SendSelectedInfo()
        {
            DataGridViewRow row = dataGridView1.SelectedRows[0];
            string Client = row.Cells["Client"].Value.ToString();
            string Product = row.Cells["Product"].Value.ToString();
            string Amount = row.Cells["Amount"].Value.ToString();
            DateTime StartingDate = DateTime.Parse(row.Cells["Starting date"].Value.ToString());
            DateTime EndingDate = DateTime.Parse(row.Cells["Ending date"].Value.ToString());
            string DailyPrice = row.Cells["Daily price"].Value.ToString();
            string Profit = row.Cells["Profit"].Value.ToString();

            string StrToSearch = $@"{Client}&{Product}&{Amount}&{StartingDate.ToString("d")}&{EndingDate.ToString("d")}&{DailyPrice}&{Profit}";
            return StrToSearch;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DeleteNote();
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            SelectedRowInfo = SendSelectedInfo();
            changeForm.ShowDialog();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainScreenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
