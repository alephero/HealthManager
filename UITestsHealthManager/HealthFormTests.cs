using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UITestsHealthManager
{
    [TestClass]
    [DoNotParallelize]
    public class HealthFormTests
    {
        private FlaUI.Core.Application app;
        private AutomationBase automation;
        private Window mainWindow;

        [TestInitialize]
        public void Init()
        {
            string exePath = @"F:\тестирование\HealthManager1\HealthManager\HealthManager\bin\Debug\HealthManager.exe";
            automation = new UIA3Automation();
            app = FlaUI.Core.Application.Launch(exePath);

            mainWindow = null;
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(500);
                try
                {
                    mainWindow = app.GetAllTopLevelWindows(automation).FirstOrDefault();
                    if (mainWindow != null) break;
                }
                catch { }
            }

            Assert.IsNotNull(mainWindow, "Главное окно не открылось");
            Console.WriteLine($"Найдено главное окно: '{mainWindow.Title}'");
            Thread.Sleep(1000);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try { app?.Close(); } catch { }
            try { app?.Dispose(); } catch { }
            try { automation?.Dispose(); } catch { }
        }

        [TestMethod]
        public void TestMethod1_AllButtonsPresent()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));

            Console.WriteLine("Найденные кнопки:");
            foreach (var b in buttons)
                Console.WriteLine($"  '{b.Name}'");

            // Ожидаемые кнопки
            string[] expectedButtons = {
                "Отслеживать активность",
                "Отслеживать питание",
                "Отслеживать сон",
                "Показать отчёт"
            };

            foreach (var expectedName in expectedButtons)
            {
                var found = buttons.FirstOrDefault(b => b.Name == expectedName);
                Assert.IsNotNull(found, $"Кнопка '{expectedName}' не найдена на форме");
                Console.WriteLine($"  ✓ '{expectedName}' найдена");
            }
        }
    }
}
