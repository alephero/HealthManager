using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace HealthManager
{
    public class SleepForm:Form
    {
        private TextBox dateTextBox;
        private TextBox hoursTextBox;
        private Label dateLabel;
        private Label hoursLabel;
        public string Date { get; private set; }
        public decimal Hours { get; private set; }
        public SleepForm()
        {
            this.Text = "Добавить сон";
            this.Width = 250;
            this.Height = 150;
            CreateControls();
        }
        private void CreateControls()
        {
            dateLabel = new Label
            {
                Text = "Дата:",
                Location = new System.Drawing.Point(10, 10)
            };
            dateTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 30),
                Size = new System.Drawing.Size(200, 20)
            };
            hoursLabel = new Label
            {
                Text = "Количество часов:",
                Location = new System.Drawing.Point(10, 55)
            };
            hoursTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 75),
                Size = new System.Drawing.Size(200, 20)
            };
            var okButton = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(10, 100),
                Size = new System.Drawing.Size(80, 25)
            };
            okButton.Click += (sender, e) =>
            {
                if (decimal.TryParse(hoursTextBox.Text, out decimal hours))
                {
                    Date = dateTextBox.Text;
                    Hours = hours;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное значение часов.");
                }
            };
            var cancelButton = new Button
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(130, 100),
                Size = new System.Drawing.Size(80, 25)
            };
            cancelButton.Click += (sender, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            this.Controls.Add(dateLabel);
            this.Controls.Add(dateTextBox);
            this.Controls.Add(hoursLabel);
            this.Controls.Add(hoursTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

    }
}
