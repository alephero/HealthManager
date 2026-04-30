using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProjectHealthManager
{
    [TestClass]
    public class HealthManagerTests
    {
        [TestMethod]
        public void TrackActivity_AddsNewActivity()
        {
            // Arrange
            var manager = new HealthManager.HealthManager();
            // Act
            manager.TrackActivity("Running", 30);
            var activities = manager.GetActivityTracking();
            Assert.IsTrue(activities.ContainsKey("Running"));
            Assert.AreEqual(30, activities["Running"]);
        }

        [TestMethod]
        public void TrackActivity_WhenDuplicate_SumDuration()
        {
            var manager = new HealthManager.HealthManager();

            manager.TrackActivity("Walking", 20);
            manager.TrackActivity("Walking", 15);

            var activities = manager.GetActivityTracking();
            Assert.AreEqual(35, activities["Walking"]);
        }

        [TestMethod]
        public void TrackNutrition_AddsFoodItem()
        {
            var manager = new HealthManager.HealthManager();

            manager.TrackNutrition("Apple", 95);

            var nutrition = manager.GetNutritionTracking();
            Assert.IsTrue(nutrition.ContainsKey("Apple"));
            Assert.AreEqual(95, nutrition["Apple"]);
        }

        [TestMethod]
        public void TrackSleep_AddsSleepRecord()
        {
            var manager = new HealthManager.HealthManager();

            manager.TrackSleep("2024-01-15", 8);

            var sleep = manager.GetSleepTracking();
            Assert.IsTrue(sleep.ContainsKey("2024-01-15"));
            Assert.AreEqual(8, sleep["2024-01-15"]);
        }

        [TestMethod]
        public void DisplayReport_WhenCalled_DoesNotThrow()
        {
            var manager = new HealthManager.HealthManager();
            manager.TrackActivity("Test", 10);

            try
            {
                manager.DisplayActivityReport();
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("DisplayActivityReport выбросил исключение");
            }
        }
        [TestMethod]
        public void TrackActivity_EmptyActivityType_DoesNotCrash()
        {
            var manager = new HealthManager.HealthManager();

            manager.TrackActivity("", 10);

            var activities = manager.GetActivityTracking();
            Assert.IsTrue(activities.ContainsKey(""));
        }

        [TestMethod]
        public void TrackNutrition_EmptyFoodItem_DoesNotCrash()
        {
            var manager = new HealthManager.HealthManager();

            manager.TrackNutrition("", 50);

            var nutrition = manager.GetNutritionTracking();
            Assert.IsTrue(nutrition.ContainsKey(""));
        }

        [TestMethod]
        public void TrackSleep_SameDate_UpdatesHours()
        {
            var manager = new HealthManager.HealthManager();

            manager.TrackSleep("2024-01-15", 8);
            manager.TrackSleep("2024-01-15", 2);

            var sleep = manager.GetSleepTracking();
            Assert.AreEqual(2, sleep["2024-01-15"]);
        }

        [TestMethod]
        public void TrackActivity_MultipleDifferentActivities()
        {
            var manager = new HealthManager.HealthManager();

            manager.TrackActivity("Running", 30);
            manager.TrackActivity("Swimming", 45);
            manager.TrackActivity("Walking", 20);

            var activities = manager.GetActivityTracking();
            Assert.AreEqual(3, activities.Count);
            Assert.AreEqual(30, activities["Running"]);
            Assert.AreEqual(45, activities["Swimming"]);
            Assert.AreEqual(20, activities["Walking"]);
        }

        [TestMethod]
        public void TrackNutrition_MultipleDifferentFoods()
        {
            var manager = new HealthManager.HealthManager();

            manager.TrackNutrition("Apple", 95);
            manager.TrackNutrition("Banana", 105);
            manager.TrackNutrition("Orange", 62);

            var nutrition = manager.GetNutritionTracking();
            Assert.AreEqual(3, nutrition.Count);
            Assert.AreEqual(95, nutrition["Apple"]);
            Assert.AreEqual(105, nutrition["Banana"]);
            Assert.AreEqual(62, nutrition["Orange"]);
        }
    }
}
