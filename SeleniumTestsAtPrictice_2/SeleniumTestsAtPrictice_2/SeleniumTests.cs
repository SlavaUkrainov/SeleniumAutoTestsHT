using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTestsAtPrictice_2
{
    public class SeleniumTests
    {
        private ChromeDriver driver;
        private WebDriverWait wait;

        private By emailInputLocator;
        private By buttonLocator;
        private By resultEmailLocator;
        private By emailLinkLocator;
        private By maleRadioButtonLocator;
        private By femaleRadioButtonLocator;
        private By sexLocator;
        private By formErrorLocator;


        private string testMail;
        private string otherLinkText;
        private string maleResultText;
        private string femaleResultText;
        private string inputErrorText;
        private string incorrectErrorText;
        private string incorrectEmail;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized"); // окно браузера во весь экран
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); // явные ожидания (?)
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); // неявные ожидания (?)

            emailInputLocator = By.Name("email");
            buttonLocator = By.Id("sendMe");
            resultEmailLocator = By.ClassName("your-email");
            emailLinkLocator = By.LinkText("указать другой e-mail");
            maleRadioButtonLocator = By.Id("boy");
            femaleRadioButtonLocator = By.Id("girl");
            sexLocator = By.ClassName("result-text");
            formErrorLocator = By.ClassName("form-error");

            testMail = "test_mail@gmai.com";
            otherLinkText = "указать другой e-mail";
            maleResultText = "Хорошо, мы пришлём имя для вашего мальчика на e-mail:";
            femaleResultText = "Хорошо, мы пришлём имя для вашей девочки на e-mail:";
            inputErrorText = "Введите email";
            incorrectErrorText = "Некорректный email";
            incorrectEmail = "incorrectEmail";
        }

        [Test]
        public void ParrotSite_FillFormWithEmail_Success()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual(testMail, driver.FindElement(resultEmailLocator).Text, "Заявка создана не на тот e-mail");
        }

        [Test]
        public void ParrotSite_ClickAnotherEmail_LastEmailExists()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(emailLinkLocator).Click();

            Assert.IsTrue(driver.FindElements(resultEmailLocator).Count == 0, "Предыдущий e-mail не был удален со страницы");
        }

        // Саша, может не надо..?
        [Test]
        public void ParrotSite_FillFormWithEmail_EmailLinkExists()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();

            Assert.IsTrue(driver.FindElements(emailLinkLocator).Count == 1, "Ссылка для ввода другого e-mail не появилась");
        }

        [Test]
        public void ParrotSite_ClickAnotherEmail_EmailInputIsEmpty()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(emailLinkLocator).Click();

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "После клика по ссылке поле ввода не отчистилось");
        }

        [Test]
        public void ParrotSite_ClickAnotherEmail_AnotherEmailIsEmpty()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(emailLinkLocator).Click();

            Assert.IsTrue(driver.FindElements(emailLinkLocator).Count == 0, "Не исчезла ссылка для ввода другого e-mail");
        }

        [Test]
        public void ParrotSite_SelectMale_Success()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(maleRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual(maleResultText, driver.FindElement(sexLocator).Text, "Заявка создана не на тот пол попугайчика");
        }

        [Test]
        public void ParrotSite_SelectFemale_Success()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(femaleRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual(femaleResultText, driver.FindElement(sexLocator).Text, "Заявка создана не на тот пол попугайчика");
        }

        [Test]
        public void ParrotSite_SelectFemaleClickOtherEmail_FemaleSelected()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            
            driver.FindElement(femaleRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(emailLinkLocator).Click();

            //Находим радиобаттон "girl" и проверяем, выбран ли он
            var flagIsChecked = driver.FindElement(femaleRadioButtonLocator).GetAttribute("checked");

            Assert.IsTrue(flagIsChecked == "true", "При выборе другой почты выбранный пол попугайчика не сохраняется");
        }

        [Test]
        public void ParrotSite_ClickAnotherEmail_ResultTextExists()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(maleRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(emailLinkLocator).Click();

            Assert.IsTrue(driver.FindElements(sexLocator).Count == 0, "Не исчез результирующий текст");
        }

        [Test]
        public void ParrotSite_InputEmptyEmail_InputError()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys("");
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual(inputErrorText, driver.FindElement(formErrorLocator).Text, "Не сработало валидационне уведомление о пустом поле ввода");
        }

        [Test]
        public void ParrotSite_InputIncorrectEmail_IncorrectEmailError()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(incorrectEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual(incorrectErrorText, driver.FindElement(formErrorLocator).Text, "Не сработало валидационне уведомление о некорректном e-mail");
        }

        [Test]
        public void ParrotSite_InputEmailAndRefresh_InputFieldIsEmpty()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(testMail);
           
            driver.Navigate().Refresh();

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "После обновления страници поле ввода email не очистилось");
        }

        [Test]
        public void ParrotSite_ChooseFemaleAndRefresh_MaleIsChosen()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.Navigate().Refresh();
            //Находим радиобаттон "girl" и проверяем, выбран ли он
            var flagIsChecked = driver.FindElement(femaleRadioButtonLocator).GetAttribute("checked");

            Assert.IsFalse(flagIsChecked == "true", "После обновления страницы радиобаттон girl не сбрасывается");
        }

        [Test]
        public void ParrotSite_SendFormAndRefresh_FormIsEmpty()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(femaleRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(testMail);
            driver.FindElement(buttonLocator).Click();

            driver.Navigate().Refresh();

            Assert.IsTrue(driver.FindElements(emailLinkLocator).Count == 0, "После обновления страницы данные отправленной формы не сбросились");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
