using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProjectHealthManager
{
    [TestClass]
    public class HealthManagerBoundaryTests
    {
        [TestMethod]
        public void TrackActivity_ZeroDuration_DoesNotCrash()
        {
            var manager = new HealthManager.HealthManager();
            manager.TrackActivity("Rest", 0);
        }

        [TestMethod]
        public void TrackActivity_NegativeDuration_DoesNotCrash()
        {
            var manager = new HealthManager.HealthManager();
            manager.TrackActivity("Invalid", -10);
        }

        [TestMethod]
        public void TrackNutrition_ZeroCalories_DoesNotCrash()
        {
            var manager = new HealthManager.HealthManager();
            manager.TrackNutrition("Water", 0);
        }

        [TestMethod]
        public void TrackNutrition_NegativeCalories_DoesNotCrash()
        {
            var manager = new HealthManager.HealthManager();
            manager.TrackNutrition("BadFood", -100);
        }

        [TestMethod]
        public void TrackSleep_EmptyDate_DoesNotCrash()
        {
            var manager = new HealthManager.HealthManager();
            manager.TrackSleep("", 8);
        }

        [TestMethod]
        public void TrackSleep_ZeroHours_DoesNotCrash()
        {
            var manager = new HealthManager.HealthManager();
            manager.TrackSleep("2024-01-15", 0);
        }
    }
}