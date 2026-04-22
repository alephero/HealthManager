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
            this.Text = "Добавить питание";
            this.Width = 250;
            this.Height = 150;
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
                if (decimal.TryParse(caloriesTextBox.Text, out decimal calories))
                {
                    FoodItem = foodItemTextBox.Text;
                    Calories = calories;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное значениекалорийности.");
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
            this.Controls.Add(foodItemLabel);
            this.Controls.Add(foodItemTextBox);
            this.Controls.Add(caloriesLabel);
            this.Controls.Add(caloriesTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }
    }
}
