using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UjyTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://u-jy.cn/");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10000);
            IWebElement inputAccount = driver.FindElement(By.Id("login_string"));
            inputAccount.Click();
            inputAccount.Clear();
            inputAccount.SendKeys(username.Text);
            IWebElement inputPassword = driver.FindElement(By.Id("login_pass"));
            inputPassword.Click();
            inputPassword.Clear();
            inputPassword.SendKeys(passwd.Text);
            IWebElement submitButton = driver.FindElement(By.Id("login"));
            submitButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("part7")));
            string sess = driver.ExecuteJavaScript<string>("return getcookie(\"ujy_session\")");
            session.Text = sess;
            driver.Quit();
        }
    }
}
