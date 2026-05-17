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
    public class ActivityFormTests
    {
        private FlaUI.Core.Application app;
        private AutomationBase automation;
        private Window mainWindow;

        [TestInitialize]
        public void Init()
        {
            string exePath = @"F:\тестирование\HealthManager1\HealthManager\HealthManager\bin\Debug\HealthManager.exe";
            app = FlaUI.Core.Application.Launch(exePath);
            automation = new UIA3Automation();
            mainWindow = null;
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(500);
                try
                {
                    var windows = app.GetAllTopLevelWindows(automation);
                    mainWindow = windows.FirstOrDefault();
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
        private Window WaitForActivityWindow()
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
        // Хелпер — запускает действие в отдельном STA-потоке
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

            Console.WriteLine("Найденные кнопки:");
            foreach (var b in buttons)
                Console.WriteLine($"  '{b.Name}'");

            var trackActivityButton = buttons.FirstOrDefault(b => b.Name == "Отслеживать активность");
            Assert.IsNotNull(trackActivityButton, "Кнопка 'Отслеживать активность' не найдена");
            RunInStaThread(() => trackActivityButton.AsButton().Click());
            Thread.Sleep(500);
            var activityWindow = WaitForActivityWindow();
            Assert.IsNotNull(activityWindow, "Окно 'Добавить активность' не открылось");
            var cancelButtons = activityWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var cancelButton = cancelButtons.FirstOrDefault(b => b.Name == "Отмена");
            Assert.IsNotNull(cancelButton, "Кнопка 'Отмена' не найдена");
            cancelButton.AsButton().Click();
            Thread.Sleep(500);
        }

        [TestMethod]
        public void TestMethod2_aVvodDannih()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var trackActivityButton = buttons.FirstOrDefault(b => b.Name == "Отслеживать активность");
            Assert.IsNotNull(trackActivityButton, "Кнопка 'Отслеживать активность' не найдена");
            RunInStaThread(() => trackActivityButton.AsButton().Click());
            Thread.Sleep(500);
            var activityWindow = WaitForActivityWindow();
            Assert.IsNotNull(activityWindow, "Окно 'Добавить активность' не открылось");
            var textBoxes = activityWindow
                .FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit))
                .ToList();
            Assert.IsTrue(textBoxes.Count >= 2, $"Ожидалось 2 поля, найдено: {textBoxes.Count}");

            textBoxes[0].AsTextBox().Text = "Тестовая активность";
            Thread.Sleep(200);
            textBoxes[1].AsTextBox().Text = "30";
            Thread.Sleep(200);
            var okButtons = activityWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var okButton = okButtons.FirstOrDefault(b => b.Name == "OK");
            Assert.IsNotNull(okButton, "Кнопка OK не найдена");
            okButton.AsButton().Click();
            Thread.Sleep(1000);
            var activityWindowAfter = app.GetAllTopLevelWindows(automation)
                .FirstOrDefault(w => w.Title == "Добавить активность");
            Assert.IsNull(activityWindowAfter, "Окно должно было закрыться");
        }

        [TestMethod]
        public void TestMethod3_EmptyFields()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var trackActivityButton = buttons.FirstOrDefault(b => b.Name == "Отслеживать активность");
            Assert.IsNotNull(trackActivityButton, "Кнопка 'Отслеживать активность' не найдена");
            RunInStaThread(() => trackActivityButton.AsButton().Click());
            Thread.Sleep(500);
            var activityWindow = WaitForActivityWindow();
            Assert.IsNotNull(activityWindow, "Окно 'Добавить активность' не открылось");
            var okButtons = activityWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var okButton = okButtons.FirstOrDefault(b => b.Name == "OK");
            Assert.IsNotNull(okButton, "Кнопка OK не найдена");
            okButton.AsButton().Click();
            Thread.Sleep(500);
            var activityWindowAfter = mainWindow.FindFirstDescendant(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            Assert.IsNotNull(activityWindowAfter, "Окно должно было остаться открытым при пустых полях");
            var cancelButtons = activityWindowAfter.AsWindow().FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var cancelButton = cancelButtons.FirstOrDefault(b => b.Name == "Отмена");
            cancelButton?.AsButton().Click();
            Thread.Sleep(500);
        }
    }
    }
