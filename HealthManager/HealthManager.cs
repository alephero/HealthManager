using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthManager
{
    public class HealthManager
    {
        private const string VERSION = "1.0";

        private Dictionary<string, decimal> activityTracking = new Dictionary<string, decimal>();
        private Dictionary<string, decimal> nutritionTracking = new Dictionary<string, decimal>();
        private Dictionary<string, decimal> sleepTracking = new Dictionary<string, decimal>();
        public void TrackActivity(string activityType, decimal duration)
        {
            if (activityTracking.ContainsKey(activityType))
            {
                activityTracking[activityType] += duration;
            }
            else
            {
                activityTracking.Add(activityType, duration);
            }
            MessageBox.Show($"Активность '{activityType}' отслежена на {duration} минут.");
        }
        public void TrackNutrition(string foodItem, decimal calories)
        {
            if (nutritionTracking.ContainsKey(foodItem))
            {
                nutritionTracking[foodItem] += calories;
            }
            else
            {
                nutritionTracking.Add(foodItem, calories);
            }
            MessageBox.Show($"Пища '{foodItem}' отслежена: {calories} калорий.");
        }
        public void TrackSleep(string date, decimal hours)
        {
            if (sleepTracking.ContainsKey(date))
            {
                sleepTracking[date] = hours;
            }
            else
            {
                sleepTracking.Add(date, hours);
            }
            MessageBox.Show($"Сон на {date} отслежен: {hours} часов.");
        }
        public void DisplayActivityReport()
        {
            var reportForm = new ReportForm();
            reportForm.ActivityTracking = activityTracking;
            reportForm.NutritionTracking = nutritionTracking;
            reportForm.SleepTracking = sleepTracking;
            reportForm.ShowDialog();
        }
        public Dictionary<string, decimal> GetActivityTracking()
        {
            return activityTracking;
        }

        public Dictionary<string, decimal> GetNutritionTracking()
        {
            return nutritionTracking;
        }

        public Dictionary<string, decimal> GetSleepTracking()
        {
            return sleepTracking;
        }
    }
}
