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
    public class SLeepFormTests
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

        private void RunInStaThread(Action action)
        {
            var thread = new Thread(() => action());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private Window WaitForSleepWindow()
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

        // Тест 1: проверяем что окно "Добавить сон" открывается
        [TestMethod]
        public void TestMethod1_WillItOpen()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var trackSleepButton = buttons.FirstOrDefault(b => b.Name == "Отслеживать сон");
            Assert.IsNotNull(trackSleepButton, "Кнопка 'Отслеживать сон' не найдена");

            RunInStaThread(() => trackSleepButton.AsButton().Click());
            Thread.Sleep(500);

            var sleepWindow = WaitForSleepWindow();
            Assert.IsNotNull(sleepWindow, "Окно 'Добавить сон' не открылось");

            var cancelButtons = sleepWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var cancelButton = cancelButtons.FirstOrDefault(b => b.Name == "Отмена");
            Assert.IsNotNull(cancelButton, "Кнопка 'Отмена' не найдена");
            cancelButton.AsButton().Click();

            Thread.Sleep(500);
        }

        // Тест 2: вводим корректные данные и проверяем что окно закрывается
        [TestMethod]
        public void TestMethod2_ValidInput()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var trackSleepButton = buttons.FirstOrDefault(b => b.Name == "Отслеживать сон");
            Assert.IsNotNull(trackSleepButton, "Кнопка 'Отслеживать сон' не найдена");

            RunInStaThread(() => trackSleepButton.AsButton().Click());
            Thread.Sleep(500);

            var sleepWindow = WaitForSleepWindow();
            Assert.IsNotNull(sleepWindow, "Окно 'Добавить сон' не открылось");

            var textBoxes = sleepWindow
                .FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit))
                .ToList();
            Assert.IsTrue(textBoxes.Count >= 2, $"Ожидалось 2 поля, найдено: {textBoxes.Count}");

            textBoxes[0].AsTextBox().Text = "17.05.2026";
            Thread.Sleep(200);
            textBoxes[1].AsTextBox().Text = "8";
            Thread.Sleep(200);

            var okButtons = sleepWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var okButton = okButtons.FirstOrDefault(b => b.Name == "OK");
            Assert.IsNotNull(okButton, "Кнопка OK не найдена");
            okButton.AsButton().Click();

            Thread.Sleep(1000);

            var sleepWindowAfter = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window))
                .FirstOrDefault(w => w.Name == "Добавить сон");
            Assert.IsNull(sleepWindowAfter, "Окно должно было закрыться после нажатия OK");
        }

        // Тест 3: пустые поля — окно не должно закрываться
        [TestMethod]
        public void TestMethod3_EmptyFields()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var trackSleepButton = buttons.FirstOrDefault(b => b.Name == "Отслеживать сон");
            Assert.IsNotNull(trackSleepButton, "Кнопка 'Отслеживать сон' не найдена");

            RunInStaThread(() => trackSleepButton.AsButton().Click());
            Thread.Sleep(500);

            var sleepWindow = WaitForSleepWindow();
            Assert.IsNotNull(sleepWindow, "Окно 'Добавить сон' не открылось");

            var okButtons = sleepWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var okButton = okButtons.FirstOrDefault(b => b.Name == "OK");
            Assert.IsNotNull(okButton, "Кнопка OK не найдена");
            okButton.AsButton().Click();

            Thread.Sleep(500);

            var sleepWindowAfter = mainWindow.FindFirstDescendant(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            Assert.IsNotNull(sleepWindowAfter, "Окно должно было остаться открытым при пустых полях");

            var cancelButtons = sleepWindowAfter.AsWindow().FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var cancelButton = cancelButtons.FirstOrDefault(b => b.Name == "Отмена");
            cancelButton?.AsButton().Click();

            Thread.Sleep(500);
        }
    }
}
