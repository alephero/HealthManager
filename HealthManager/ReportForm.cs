using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthManager
{
    public class ReportForm:Form
    {
        private Dictionary<string, decimal> activityTracking;
        private Dictionary<string, decimal> nutritionTracking;
        private Dictionary<string, decimal> sleepTracking;
        public Dictionary<string, decimal> ActivityTracking
        {
            set { activityTracking = value; }
        }
        public Dictionary<string, decimal> NutritionTracking
        {
            set { nutritionTracking = value; }
        }
        public Dictionary<string, decimal> SleepTracking
        {
            set { sleepTracking = value; }
        }
        public ReportForm(
    Dictionary<string, decimal> activityTracking,
    Dictionary<string, decimal> nutritionTracking,
    Dictionary<string, decimal> sleepTracking)
        {
            this.activityTracking = activityTracking;
            this.nutritionTracking = nutritionTracking;
            this.sleepTracking = sleepTracking;

            InitializeComponent();
            this.Text = "Отчёт по здоровью";
            this.Width = 450;
            this.Height = 300;
            CreateControls();
        }
        private void CreateControls()
        {
            var reportRichTextBox = new RichTextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(380, 280)
            };
            reportRichTextBox.AppendText("Отчёт по активностям:\n");
            if (activityTracking != null)
            {
                foreach (var activity in activityTracking)
                {
                    reportRichTextBox.AppendText($" {activity.Key}: {activity.Value} минут.\n");
                }
            }
            reportRichTextBox.AppendText("\nОтчёт по питанию:\n");
            if (nutritionTracking != null)
            {
                foreach (var food in nutritionTracking)
                {
                    reportRichTextBox.AppendText($" {food.Key}: {food.Value} калорий.\n");
                }
            }
            reportRichTextBox.AppendText("\nОтчёт по сну:\n");
            if (sleepTracking != null)
            {
                foreach (var sleep in sleepTracking)
                {
                    reportRichTextBox.AppendText($" {sleep.Key}: {sleep.Value} часов.\n");
                }
            }
            this.Controls.Add(reportRichTextBox);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.SuspendLayout();
            // 
            // ReportForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }
    }
}
