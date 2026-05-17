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
    public class ReportFormTests
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

        private Window WaitForReportWindow()
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

        [TestMethod]
        public void TestMethod1_WillItOpen()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var reportButton = buttons.FirstOrDefault(b => b.Name == "Показать отчёт");
            Assert.IsNotNull(reportButton, "Кнопка 'Показать отчёт' не найдена");

            RunInStaThread(() => reportButton.AsButton().Click());
            Thread.Sleep(500);

            var reportWindow = WaitForReportWindow();
            Assert.IsNotNull(reportWindow, "Окно 'Отчёт по здоровью' не открылось");

            Console.WriteLine($"Заголовок окна: '{reportWindow.Title}'");
            Assert.AreEqual("Отчёт по здоровью", reportWindow.Title, "Заголовок окна не совпадает");

            reportWindow.Close();
            Thread.Sleep(500);
        }

        [TestMethod]
        public void TestMethod2_ReportHasTextBox()
        {
            var buttons = mainWindow.FindAllDescendants(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
            var reportButton = buttons.FirstOrDefault(b => b.Name == "Показать отчёт");
            Assert.IsNotNull(reportButton, "Кнопка 'Показать отчёт' не найдена");

            RunInStaThread(() => reportButton.AsButton().Click());
            Thread.Sleep(500);

            var reportWindow = WaitForReportWindow();
            Assert.IsNotNull(reportWindow, "Окно 'Отчёт по здоровью' не открылось");
            var richTextBox = reportWindow.FindFirstDescendant(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Document));
            Assert.IsNotNull(richTextBox, "RichTextBox не найден на форме");

            var text = richTextBox.AsTextBox().Text;
            Console.WriteLine($"Текст в отчёте:\n{text}");
            Assert.IsTrue(text.Contains("Отчёт по активностям"), "Секция активностей не найдена");
            Assert.IsTrue(text.Contains("Отчёт по питанию"), "Секция питания не найдена");
            Assert.IsTrue(text.Contains("Отчёт по сну"), "Секция сна не найдена");

            reportWindow.Close();
            Thread.Sleep(500);
        }
    }
}
