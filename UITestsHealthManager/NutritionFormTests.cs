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
    public class NutritionFormTests
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

        private Window WaitForNutritionWindow()
        {
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(500);
                try
                {
                    var modalWindow = mainWindow.FindFirstDescendant(
                        cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
                    if (modalWindow != null)
                    {
                        Console.WriteLine($"Найдено дочернее окно: '{modalWindow.Name}'");
                        return modalWindow.AsWindow();
                    }
                }
                catch { }
            }
            return null;
        }

        private void RunInStaThread(Action action)
        {
            var thread = new Thread(() => action());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [TestMethod]
        public void TestMethod1_WillItOpen()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var trackNutritionButton = buttons.FirstOrDefault(b => b.Name == "Отслеживать питание");
            Assert.IsNotNull(trackNutritionButton, "Кнопка 'Отслеживать питание' не найдена");

            RunInStaThread(() => trackNutritionButton.AsButton().Click());
            Thread.Sleep(500);

            var nutritionWindow = WaitForNutritionWindow();
            Assert.IsNotNull(nutritionWindow, "Окно 'Добавить питание' не открылось");

            var cancelButtons = nutritionWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var cancelButton = cancelButtons.FirstOrDefault(b => b.Name == "Отмена");
            Assert.IsNotNull(cancelButton, "Кнопка 'Отмена' не найдена");
            cancelButton.AsButton().Click();

            Thread.Sleep(500);
        }

        [TestMethod]
        public void TestMethod2_ValidInput()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var trackNutritionButton = buttons.FirstOrDefault(b => b.Name == "Отслеживать питание");
            Assert.IsNotNull(trackNutritionButton, "Кнопка 'Отслеживать питание' не найдена");

            RunInStaThread(() => trackNutritionButton.AsButton().Click());
            Thread.Sleep(500);

            var nutritionWindow = WaitForNutritionWindow();
            Assert.IsNotNull(nutritionWindow, "Окно 'Добавить питание' не открылось");

            var textBoxes = nutritionWindow
                .FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit))
                .ToList();
            Assert.IsTrue(textBoxes.Count >= 2, $"Ожидалось 2 поля, найдено: {textBoxes.Count}");

            textBoxes[0].AsTextBox().Text = "Гречка";
            Thread.Sleep(200);
            textBoxes[1].AsTextBox().Text = "350";
            Thread.Sleep(200);

            var okButtons = nutritionWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var okButton = okButtons.FirstOrDefault(b => b.Name == "OK");
            Assert.IsNotNull(okButton, "Кнопка OK не найдена");
            okButton.AsButton().Click();

            Thread.Sleep(1000);
            var nutritionWindowAfter = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window))
                .FirstOrDefault(w => w.Name == "Добавить питание");
            Assert.IsNull(nutritionWindowAfter, "Окно должно было закрыться после нажатия OK");
        }

        [TestMethod]
        public void TestMethod3_EmptyFields()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var trackNutritionButton = buttons.FirstOrDefault(b => b.Name == "Отслеживать питание");
            Assert.IsNotNull(trackNutritionButton, "Кнопка 'Отслеживать питание' не найдена");

            RunInStaThread(() => trackNutritionButton.AsButton().Click());
            Thread.Sleep(500);

            var nutritionWindow = WaitForNutritionWindow();
            Assert.IsNotNull(nutritionWindow, "Окно 'Добавить питание' не открылось");
            var okButtons = nutritionWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var okButton = okButtons.FirstOrDefault(b => b.Name == "OK");
            Assert.IsNotNull(okButton, "Кнопка OK не найдена");
            okButton.AsButton().Click();

            Thread.Sleep(500);
            var nutritionWindowAfter = mainWindow.FindFirstDescendant(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            Assert.IsNotNull(nutritionWindowAfter, "Окно должно было остаться открытым при пустых полях");
            var cancelButtons = nutritionWindowAfter.AsWindow().FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var cancelButton = cancelButtons.FirstOrDefault(b => b.Name == "Отмена");
            cancelButton?.AsButton().Click();

            Thread.Sleep(500);
        }
    }
}
