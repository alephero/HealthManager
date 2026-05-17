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
            var reportTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(415, 240),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };

            var sb = new System.Text.StringBuilder();

            sb.AppendLine("Отчёт по активностям:");
            if (activityTracking != null && activityTracking.Count > 0)
            {
                foreach (var activity in activityTracking)
                {
                    sb.AppendLine($"  {activity.Key}: {activity.Value} минут.");
                }
            }
            else
            {
                sb.AppendLine("  (нет данных)");
            }

            sb.AppendLine();
            sb.AppendLine("Отчёт по питанию:");
            if (nutritionTracking != null && nutritionTracking.Count > 0)
            {
                foreach (var food in nutritionTracking)
                {
                    sb.AppendLine($"  {food.Key}: {food.Value} калорий.");
                }
            }
            else
            {
                sb.AppendLine("  (нет данных)");
            }

            sb.AppendLine();
            sb.AppendLine("Отчёт по сну:");
            if (sleepTracking != null && sleepTracking.Count > 0)
            {
                foreach (var sleep in sleepTracking)
                {
                    sb.AppendLine($"  {sleep.Key}: {sleep.Value} часов.");
                }
            }
            else
            {
                sb.AppendLine("  (нет данных)");
            }

            reportTextBox.Text = sb.ToString();
            this.Controls.Add(reportTextBox);
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
