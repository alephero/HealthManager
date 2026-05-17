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
            InitializeComponent();
            this.Text = "Добавить сон";
            this.Width = 250;
            this.Height = 200;
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
                if (string.IsNullOrWhiteSpace(dateTextBox.Text))
                {
                    MessageBox.Show("Введите дату.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DateTime parsedDate;
                if (!DateTime.TryParse(dateTextBox.Text, out parsedDate))
                {
                    MessageBox.Show("Пожалуйста, введите корректную дату (например, 17.05.2026 или 2026-05-17).", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!decimal.TryParse(hoursTextBox.Text, out decimal hours))
                {
                    MessageBox.Show("Пожалуйста, введите корректное значение часов (число).", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (hours < 0)
                {
                    MessageBox.Show("Количество часов не может быть отрицательным.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (hours > 24)
                {
                    DialogResult result = MessageBox.Show("Вы ввели больше 24 часов. Это корректно?", "Предупреждение",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                Date = dateTextBox.Text;
                Hours = hours;
                DialogResult = DialogResult.OK;
                Close();
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SleepForm));
            this.SuspendLayout();
            // 
            // SleepForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SleepForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }
    }
}
