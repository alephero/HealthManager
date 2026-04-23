using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HealthManager
{
    public class HealthForm:Form
    {
        private HealthManager healthManager;
        private Button trackActivityButton;
        private Button trackNutritionButton;
        private Button trackSleepButton;
        private Button displayReportButton;
        public HealthForm()
        {
            InitializeComponent();
            this.Text = "Управление здоровьем";
            this.Width = 450;
            this.Height = 130;
            CreateControls();
            healthManager = new HealthManager();
        }
        private void CreateControls()
        {
            trackActivityButton = new Button
            {
                Location = new System.Drawing.Point(10, 20),
                Text = "Отслеживать активность",
                Size = new System.Drawing.Size(180, 25)
            };
            trackActivityButton.Click += (sender, e) =>
            {
                var activityForm = new ActivityForm();
                activityForm.ShowDialog();
                if (activityForm.DialogResult == DialogResult.OK)
                {
                    healthManager.TrackActivity(activityForm.ActivityType, activityForm.Duration);
                }
            };
            trackNutritionButton = new Button
            {
                Location = new System.Drawing.Point(200, 20),
                Text = "Отслеживать питание",
                Size = new System.Drawing.Size(180, 25)
            };
            trackNutritionButton.Click += (sender, e) =>
            {
                var nutritionForm = new NutritionForm();
                nutritionForm.ShowDialog();
                if (nutritionForm.DialogResult == DialogResult.OK)
                {
                    healthManager.TrackNutrition(nutritionForm.FoodItem, nutritionForm.Calories);
                }
            };
            trackSleepButton = new Button
            {
                Location = new System.Drawing.Point(10, 50),
                Text = "Отслеживать сон",
                Size = new System.Drawing.Size(180, 25)
            };
            trackSleepButton.Click += (sender, e) =>
            {
                var sleepForm = new SleepForm();
                sleepForm.ShowDialog();
                if (sleepForm.DialogResult == DialogResult.OK)
                {
                    healthManager.TrackSleep(sleepForm.Date, sleepForm.Hours);
                }
            };
            displayReportButton = new Button
            {
                Location = new System.Drawing.Point(200, 50),
                Text = "Показать отчёт",
                Size = new System.Drawing.Size(180, 25)
            };
            displayReportButton.Click += (sender, e) => healthManager.DisplayActivityReport();
            this.Controls.Add(trackActivityButton);
            this.Controls.Add(trackNutritionButton);
            this.Controls.Add(trackSleepButton);
            this.Controls.Add(displayReportButton);
        }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HealthForm());
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HealthForm));
            this.SuspendLayout();
            // 
            // HealthForm
            // 
            this.ClientSize = new System.Drawing.Size(558, 353);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HealthForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }
    }
}
