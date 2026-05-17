using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace HealthManager
{
    public class NutritionForm:Form
    {

        private TextBox foodItemTextBox;
        private TextBox caloriesTextBox;
        private Label foodItemLabel;
        private Label caloriesLabel;
        public string FoodItem { get; private set; }
        public decimal Calories { get; private set; }
        public NutritionForm()
        {
            InitializeComponent();
            this.Text = "Добавить питание";
            this.Width = 250;
            this.Height = 200;
            CreateControls();
        }
        private void CreateControls()
        {
            foodItemLabel = new Label
            {
                Text = "Название пищи:",
                Location = new System.Drawing.Point(10, 10)
            };
            foodItemTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 30),
                Size = new System.Drawing.Size(200, 20)
            };
            caloriesLabel = new Label
            {
                Text = "Калорийность:",
                Location = new System.Drawing.Point(10, 55)
            };
            caloriesTextBox = new TextBox
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
                if (string.IsNullOrWhiteSpace(foodItemTextBox.Text))
                {
                    MessageBox.Show("Введите название продукта.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!decimal.TryParse(caloriesTextBox.Text, out decimal calories))
                {
                    MessageBox.Show("Пожалуйста, введите корректное значение калорийности (число).", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (calories < 0)
                {
                    MessageBox.Show("Калорийность не может быть отрицательной.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                FoodItem = foodItemTextBox.Text;
                Calories = calories;
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
            this.Controls.Add(foodItemLabel);
            this.Controls.Add(foodItemTextBox);
            this.Controls.Add(caloriesLabel);
            this.Controls.Add(caloriesTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NutritionForm));
            this.SuspendLayout();
            // 
            // NutritionForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NutritionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }
    }
}
