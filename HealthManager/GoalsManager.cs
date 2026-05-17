using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager
{
    public class GoalsManager
    {
        public decimal ActivityGoal { get; private set; } = 30;//минут/день
        public decimal NutritionGoal { get; private set; } = 2000;//ккал/день
        public decimal SleepGoal { get; private set; } = 8;//часов/день

        //установка целей
        public void SetActivityGoal(decimal minutesPerDay)
        {
            if (minutesPerDay <= 0)
                throw new ArgumentException("Цель по активности должна быть положительной.");
            ActivityGoal = minutesPerDay;
        }

        public void SetNutritionGoal(decimal caloriesPerDay)
        {
            if (caloriesPerDay <= 0)
                throw new ArgumentException("Цель по питанию должна быть положительной.");
            NutritionGoal = caloriesPerDay;
        }

        public void SetSleepGoal(decimal hoursPerDay)
        {
            if (hoursPerDay <= 0)
                throw new ArgumentException("Цель по сну должна быть положительной.");
            SleepGoal = hoursPerDay;
        }

        //прогресс(% от цели)
        public decimal GetActivityProgress(Dictionary<string, decimal> activityData)
        {
            if (activityData == null || activityData.Count == 0) return 0;
            decimal total = activityData.Values.Sum();
            return Math.Round(total / ActivityGoal * 100, 1);
        }

        public decimal GetNutritionProgress(Dictionary<string, decimal> nutritionData)
        {
            if (nutritionData == null || nutritionData.Count == 0) return 0;
            decimal total = nutritionData.Values.Sum();
            return Math.Round(total / NutritionGoal * 100, 1);
        }
        public decimal GetSleepProgress(Dictionary<string, decimal> sleepData)
        {
            if (sleepData == null || sleepData.Count == 0) return 0;
            decimal avg = sleepData.Values.Sum() / sleepData.Count;
            return Math.Round(avg / SleepGoal * 100, 1);
        }

        public decimal GetTotalActivity(Dictionary<string, decimal> activityData)
            => activityData == null ? 0 : activityData.Values.Sum();

        public decimal GetTotalNutrition(Dictionary<string, decimal> nutritionData)
            => nutritionData == null ? 0 : nutritionData.Values.Sum();

        public decimal GetAverageSleep(Dictionary<string, decimal> sleepData)
            => (sleepData == null || sleepData.Count == 0) ? 0
               : sleepData.Values.Sum() / sleepData.Count;

        public List<string> GetRecommendations(
            Dictionary<string, decimal> activityData,
            Dictionary<string, decimal> nutritionData,
            Dictionary<string, decimal> sleepData)
        {
            var result = new List<string>();

            decimal actPct = GetActivityProgress(activityData);
            decimal nutPct = GetNutritionProgress(nutritionData);
            decimal slpPct = GetSleepProgress(sleepData);
            //активность
            if (activityData == null || activityData.Count == 0)
                result.Add("Нет данных по активности. Начните отслеживать физическую активность.");
            else if (actPct < 50)
                result.Add($"Активность выполнена на {actPct}% от цели. Рекомендуется добавить больше физической нагрузки.");
            else if (actPct < 100)
                result.Add($"Активность выполнена на {actPct}%. Совсем немного до цели — продолжайте!");
            else
                result.Add($"Цель по активности достигнута ({actPct}%). Отличная работа!");

            //аитание
            if (nutritionData == null || nutritionData.Count == 0)
                result.Add("📋 Нет данных по питанию. Начните отслеживать потребляемые калории.");
            else if (nutPct < 80)
                result.Add($"⚠️ Потребление калорий — {nutPct}% от нормы. Возможно, вы едите недостаточно.");
            else if (nutPct > 120)
                result.Add($"⚠️ Потребление калорий превышает норму ({nutPct}%). Рекомендуется скорректировать рацион.");
            else
                result.Add($"✅ Потребление калорий в норме ({nutPct}%). Хорошо сбалансированное питание!");

            //сон
            if (sleepData == null || sleepData.Count == 0)
                result.Add("📋 Нет данных по сну. Начните отслеживать продолжительность сна.");
            else if (slpPct < 75)
                result.Add($"⚠️ Средний сон составляет {slpPct}% от нормы. Рекомендуется увеличить продолжительность сна.");
            else if (slpPct > 115)
                result.Add($"ℹ️ Вы спите больше нормы ({slpPct}%). Это может сигнализировать об усталости.");
            else
                result.Add($"✅ Продолжительность сна в норме ({slpPct}%). Хороший режим!");

            return result;
        }
    }
}
