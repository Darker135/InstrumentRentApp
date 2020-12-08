using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using InstrumentRentApp;

namespace InstrumentRentApp.Forms
{
    public partial class AddOrderForm : Form
    {
        
        public AddOrderForm()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddOrderForm_Load(object sender, EventArgs e)
        {
            instrumentTextBox.Clear();
            clientTextBox.Clear();
            amountNumericUpDown.ResetText();
            startDateTimePicker.MinDate = DateTime.Now;
            startDateTimePicker.Value = DateTime.Now;
            finishDateTimePicker.Value = startDateTimePicker.Value.AddDays(1);
            priceNumericUpDown.ResetText();
        }
        private void StartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            finishDateTimePicker.MinDate = startDateTimePicker.Value;
            finishDateTimePicker.Value = startDateTimePicker.Value.AddDays(1);
        }

        public void AddNote()
        {
            DateTime start = startDateTimePicker.Value;
            DateTime finish = finishDateTimePicker.Value;
            int difference = (int)finish.Subtract(start).TotalDays;

            string[] data = new string[7];
            data[0] = instrumentTextBox.Text;
            data[1] = clientTextBox.Text;
            data[2] = amountNumericUpDown.Value.ToString();
            data[3] = startDateTimePicker.Value.ToString("d");
            data[4] = finishDateTimePicker.Value.ToString("d");
            data[5] = priceNumericUpDown.Value.ToString();
            data[6] = (amountNumericUpDown.Value * priceNumericUpDown.Value * difference).ToString();

            StreamReader reader = new StreamReader(MainScreenForm.fullPathToDataFile);
            string oldData = reader.ReadToEnd();
            reader.Close();
            StreamWriter writer = new StreamWriter(MainScreenForm.fullPathToDataFile);
            writer.Write($@"{data[0]}&{data[1]}&{data[2]}&{data[3]}&{data[4]}&{data[5]}&{data[6]}"
                            +Environment.NewLine + oldData);
            writer.Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (instrumentTextBox.Text == "" || clientTextBox.Text == "")
                MessageBox.Show("Fill all the blanks, please", "OK");
            else if(amountNumericUpDown.Value <= 0 || priceNumericUpDown.Value <= 0)           
                MessageBox.Show("Amount and price can not be lesser or equal to zero", "OK");            
            else
            {
                AddNote();
                MainScreenForm.TxtToDgv();
                MessageBox.Show("Successfully added", "OK");                
                Close();
            }
                
        }
    }
}
