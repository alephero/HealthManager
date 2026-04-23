using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HealthManager
{
    public class ActivityForm:Form
    {
        private TextBox activityTypeTextBox;
        private TextBox durationTextBox;
        private Label activityTypeLabel;
        private Label durationLabel;
        public string ActivityType { get; private set; }
        public decimal Duration { get; private set; }
        public ActivityForm()
        {
            InitializeComponent();
            this.Text = "Добавить активность";
            this.Width = 250;
            this.Height = 200;
            CreateControls();
        }
        private void CreateControls()
        {
            activityTypeLabel = new Label
            {
                Text = "Тип активности:",
                Location = new System.Drawing.Point(10, 10)
            };
            activityTypeTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 30),
                Size = new System.Drawing.Size(200, 20)
            };
            durationLabel = new Label
            {
                Text = "Продолжительность (минут):",
                Location = new System.Drawing.Point(10, 55)
            };
            durationTextBox = new TextBox
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
                if (decimal.TryParse(durationTextBox.Text, out decimal duration))
                {
                    ActivityType = activityTypeTextBox.Text;
                    Duration = duration;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное значениепродолжительности.");
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
            this.Controls.Add(activityTypeLabel);
            this.Controls.Add(activityTypeTextBox);
            this.Controls.Add(durationLabel);
            this.Controls.Add(durationTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActivityForm));
            this.SuspendLayout();
            // 
            // ActivityForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ActivityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }
    }
}
