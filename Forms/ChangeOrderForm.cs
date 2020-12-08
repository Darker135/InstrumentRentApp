using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstrumentRentApp.Forms
{
    public partial class ChangeOrderForm : Form
    {
        public ChangeOrderForm()
        {
            InitializeComponent();
        }

        private void ChangeOrderForm_Load(object sender, EventArgs e)
        {
            instrumentTextBox.Clear();
            clientTextBox.Clear();
            amountNumericUpDown.ResetText();
            startDateTimePicker.MinDate = DateTime.Now;
            startDateTimePicker.Value = DateTime.Now;
            finishDateTimePicker.Value = startDateTimePicker.Value.AddDays(1);
            priceNumericUpDown.ResetText();
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            ChangeNote();
            MainScreenForm.TxtToDgv();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChangeNote()
        {
            DateTime start = startDateTimePicker.Value;
            DateTime finish = finishDateTimePicker.Value;
            int difference = (int)finish.Subtract(start).TotalDays;

            string Instrument = instrumentTextBox.Text;
            string Client = clientTextBox.Text;
            int Amount = (int)amountNumericUpDown.Value;
            string StartingDate = startDateTimePicker.Value.ToString("d");
            string EndingDate = finishDateTimePicker.Value.ToString("d");
            decimal DailyPrice = priceNumericUpDown.Value;
            string Profit = (amountNumericUpDown.Value * priceNumericUpDown.Value * difference).ToString();

            string StrToSearch = $@"{Client}&{Instrument}&{Amount}&{StartingDate}&{EndingDate}&{DailyPrice}&{Profit}";

            List<string> quotelist = File.ReadAllLines(MainScreenForm.fullPathToDataFile).ToList();
            var newList = quotelist.Select(s => s.Replace($"{MainScreenForm.SelectedRowInfo}", $"{StrToSearch}")).ToList();

            File.WriteAllLines(MainScreenForm.fullPathToDataFile, newList);
            MessageBox.Show("Note has successfully changed", "OK");
        }

        private void startDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            finishDateTimePicker.MinDate = startDateTimePicker.Value;
            finishDateTimePicker.Value = startDateTimePicker.Value.AddDays(1);
        }
    }
}
