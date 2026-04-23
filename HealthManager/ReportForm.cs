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
        public ReportForm()
        {
            this.Text = "Отчёт по здоровью";
            this.Width = 400;
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

    }
}
