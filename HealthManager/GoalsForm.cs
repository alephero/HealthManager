using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HealthManager
{
    public class GoalsForm : Form  // ← Добавлено : Form
    {
        public Dictionary<string, decimal> ActivityData { get; set; }
        public Dictionary<string, decimal> NutritionData { get; set; }
        public Dictionary<string, decimal> SleepData { get; set; }

        private GoalsManager _goalsManager = new GoalsManager();

        private NumericUpDown activityGoalInput;
        private NumericUpDown nutritionGoalInput;
        private NumericUpDown sleepGoalInput;
        private Button calculateButton;
        private Chart progressChart;
        private RichTextBox recommendationsBox;

        // Конструктор с данными из HealthManager
        public GoalsForm(Dictionary<string, decimal> activityData,
                         Dictionary<string, decimal> nutritionData,
                         Dictionary<string, decimal> sleepData)
        {
            ActivityData = activityData;
            NutritionData = nutritionData;
            SleepData = sleepData;

            InitializeComponent();
            this.Text = "Цели и рекомендации";
            this.Width = 680;
            this.Height = 580;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Times New Roman", 9f);
            CreateControls();
        }

        // Конструктор без параметров (для совместимости)
        public GoalsForm() : this(new Dictionary<string, decimal>(),
                                   new Dictionary<string, decimal>(),
                                   new Dictionary<string, decimal>())
        {
        }

        private void CreateControls()
        {
            int lx = 12, ix = 180, w = 100;

            var title = new Label
            {
                Text = "Установка целей",
                Font = new Font("Times New Roman", 11f, FontStyle.Bold),
                Location = new Point(lx, 10),
                Size = new Size(300, 22)
            };

            var actLabel = new Label { Text = "Активность (мин/день):", Location = new Point(lx, 42), Size = new Size(165, 20) };
            activityGoalInput = new NumericUpDown
            {
                Location = new Point(ix, 40),
                Size = new Size(w, 22),
                Minimum = 1,
                Maximum = 600,
                Value = 30
            };

            var nutLabel = new Label { Text = "Питание (ккал/день):", Location = new Point(lx, 72), Size = new Size(165, 20) };
            nutritionGoalInput = new NumericUpDown
            {
                Location = new Point(ix, 70),
                Size = new Size(w, 22),
                Minimum = 1,
                Maximum = 10000,
                Value = 2000
            };

            var slpLabel = new Label { Text = "Сон (часов/день):", Location = new Point(lx, 102), Size = new Size(165, 20) };
            sleepGoalInput = new NumericUpDown
            {
                Location = new Point(ix, 100),
                Size = new Size(w, 22),
                Minimum = 1,
                Maximum = 24,
                Value = 8,
                DecimalPlaces = 1,
                Increment = 0.5m
            };

            calculateButton = new Button
            {
                Text = "Рассчитать прогресс",
                Location = new Point(lx, 132),
                Size = new Size(160, 28)
            };
            calculateButton.Click += OnCalculateClick;

            // Chart
            progressChart = new Chart
            {
                Location = new Point(lx, 170),
                Size = new Size(640, 210)
            };

            var chartArea = new ChartArea("main");
            chartArea.AxisX.Title = "Категория";
            chartArea.AxisY.Title = "% выполнения";
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = 150;
            chartArea.AxisY.Interval = 25;
            chartArea.AxisX.MajorGrid.Enabled = false;
            progressChart.ChartAreas.Add(chartArea);

            var seriesProgress = new Series("Прогресс")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.SteelBlue,
                IsValueShownAsLabel = true,
                LabelFormat = "0.#'%'"
            };
            seriesProgress.Points.AddXY("Активность", 0);
            seriesProgress.Points.AddXY("Питание", 0);
            seriesProgress.Points.AddXY("Сон", 0);
            progressChart.Series.Add(seriesProgress);

            var seriesGoal = new Series("Цель (100%)")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.OrangeRed,
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dash
            };
            seriesGoal.Points.AddXY("Активность", 100);
            seriesGoal.Points.AddXY("Питание", 100);
            seriesGoal.Points.AddXY("Сон", 100);
            progressChart.Series.Add(seriesGoal);
            progressChart.Legends.Add(new Legend("main"));

            var recLabel = new Label
            {
                Text = "Рекомендации:",
                Font = new Font("Times New Roman", 10f, FontStyle.Bold),
                Location = new Point(lx, 388),
                Size = new Size(200, 20)
            };

            recommendationsBox = new RichTextBox
            {
                Location = new Point(lx, 410),
                Size = new Size(640, 115),
                ReadOnly = true,
                BackColor = Color.WhiteSmoke,
                Font = new Font("Times New Roman", 9.5f),
                Text = "Нажмите «Рассчитать прогресс» для получения рекомендаций."
            };

            this.Controls.AddRange(new Control[]
            {
                title, actLabel, activityGoalInput, nutLabel, nutritionGoalInput,
                slpLabel, sleepGoalInput, calculateButton, progressChart, recLabel, recommendationsBox
            });
        }

        private void OnCalculateClick(object sender, EventArgs e)
        {
            try
            {
                _goalsManager.SetActivityGoal((decimal)activityGoalInput.Value);
                _goalsManager.SetNutritionGoal((decimal)nutritionGoalInput.Value);
                _goalsManager.SetSleepGoal((decimal)sleepGoalInput.Value);

                decimal actPct = _goalsManager.GetActivityProgress(ActivityData);
                decimal nutPct = _goalsManager.GetNutritionProgress(NutritionData);
                decimal slpPct = _goalsManager.GetSleepProgress(SleepData);

                var series = progressChart.Series["Прогресс"];
                series.Points[0].SetValueXY("Активность", (double)actPct);
                series.Points[1].SetValueXY("Питание", (double)nutPct);
                series.Points[2].SetValueXY("Сон", (double)slpPct);

                ColorColumn(series.Points[0], actPct);
                ColorColumn(series.Points[1], nutPct);
                ColorColumn(series.Points[2], slpPct);

                progressChart.Invalidate();

                var recs = _goalsManager.GetRecommendations(ActivityData, NutritionData, SleepData);
                recommendationsBox.Text = string.Join(Environment.NewLine + Environment.NewLine, recs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ColorColumn(DataPoint point, decimal pct)
        {
            if (pct >= 100)
                point.Color = Color.SeaGreen;
            else if (pct >= 60)
                point.Color = Color.SteelBlue;
            else
                point.Color = Color.Tomato;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(676, 540);
            this.Name = "GoalsForm";
            this.ResumeLayout(false);
        }
    }
}