using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using xTool.Models;

namespace xTool.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int xLocal = 0, yLocal = 0;
        int index = 0;
        int total, pending, error, completed;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            string tokens = Clipboard.GetText(TextDataFormat.UnicodeText);
            string[] accounts = Regex.Split(tokens, "\r\n");
            foreach (string acc in accounts)
            {
                try
                {
                    MainData mainData = new MainData(acc);
                    mainData.Id = ++index;
                    dgAccount.Items.Add(mainData);
                }
                catch (Exception ex)
                {

                }
            }
            total = index;
            UpdateSummary();
        }
        private void UpdateSummary()
        {
            Dispatcher.Invoke(() =>
            {
                pending = total - (completed + error);
                tblTotal.Text = total.ToString();
                tblPending.Text = pending.ToString();
                tblError.Text = error.ToString();
                tblCompleted.Text = completed.ToString();
            }, System.Windows.Threading.DispatcherPriority.Background);
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            var accounts = dgAccount.Items;
            Task[] tasks = new Task[accounts.Count];
            int i = 0;
            foreach (var item in accounts)
            {
                try
                {
                    tasks[i++] = Task.Factory.StartNew(() =>
                    {
                        var acc = item as MainData;

                        ChromeDriver chromeDriver;
                        ChromeOptions options = new ChromeOptions();
                        options.AddArguments($"--disable-notifications", "--window-size=" + Convert.ToString(620) + "," + Convert.ToString(420), "--window-position=" + Convert.ToString(xLocal += 50) + "," + Convert.ToString(yLocal += 20), "--no-sandbox");
                        options.AddArgument("disable-infobars");
                        ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService();
                        defaultService.HideCommandPromptWindow = true;
                        Dispatcher.Invoke(() =>
                        {
                            if (this.cbHideBrowser.IsChecked == true)
                                options.AddArgument("headless");
                        });
                        try
                        {
                            chromeDriver = new ChromeDriver(defaultService, options);
                            DoWork(chromeDriver, acc);
                        }
                        catch (Exception ex)
                        {

                        }
                    });
                }
                catch (Exception ex)
                {
                }
                //finally
                //{
                //    i++; 
                //    xLocal += 50;
                //    yLocal += 20;
                //}
            }
            Task.WaitAll();
        }

        public void ChangeStatus(int rowIndex, string status, string color)
        {
            Dispatcher.Invoke(() =>
            {
                var row = (DataGridRow)dgAccount.ItemContainerGenerator.ContainerFromIndex(rowIndex);
                MainData mainData = row.Item as MainData;
                dgAccount.Items.RemoveAt(rowIndex);
                mainData.Status = status;
                mainData.Color = color;
                dgAccount.Items.Insert(rowIndex, mainData);
            }, System.Windows.Threading.DispatcherPriority.Background);
        }
        private void DoWork(ChromeDriver chromeDriver, MainData mainData)
        {
            ChangeStatus(mainData.Id - 1, "Đang khởi tạo...", "Orange");
            WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromMilliseconds(50000));
            chromeDriver.Url = "http://2fa.live";
            chromeDriver.Navigate();

            //var listToken = chromeDriver.FindElementById("listToken");
            var listToken = wait.Until(ExpectedConditions.ElementExists(By.Id("listToken")));
            listToken.SendKeys(mainData.TwoFactorAuthentication);

            var submit = chromeDriver.FindElementById("submit");
            submit.Click();

            ChangeStatus(mainData.Id - 1, "Chờ Token", "Orange");
            Thread.Sleep(3000);
            var output = chromeDriver.FindElementById("output");

            string oValue = output.GetAttribute("value");
            string[] outputs = oValue.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string oString = outputs[1];

            ChangeStatus(mainData.Id - 1, "Đăng nhập FB", "Orange");

            chromeDriver.Url = "https://m.facebook.com/";
            chromeDriver.Navigate();

            var email = chromeDriver.FindElementById("m_login_email");
            var pass = chromeDriver.FindElementById("m_login_password");

            email.SendKeys(mainData.UserName);
            pass.SendKeys(mainData.Password);

            ///html/body/div[1]/div/div[3]/div[1]/div/div[2]/div[2]/form/div[5]/div[1]/button
            var login = chromeDriver.FindElement(By.XPath("//*[@id='u_0_4']/button"));
            login.Click();

            //chromeDriver.SwitchTo().Window(chromeDriver.WindowHandles.Last());

            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("approvals_code")));

            var approvalCode = chromeDriver.FindElementById("approvals_code");
            //var approvalCode = chromeDriver.FindElement(By.XPath("//*[@id='approvals_code']"));
            element.SendKeys(oString);

            var sendCode = chromeDriver.FindElement(By.XPath("//*[@id='checkpointSubmitButton']/button"));
            sendCode.Click();

            Thread.Sleep(1000);

            var continueButton = chromeDriver.FindElementById("checkpointSubmitButton-actual-button");
            continueButton.Click();

            ChangeStatus(mainData.Id - 1, "Thêm thẻ", "Orange");
            chromeDriver.Url = "https://www.facebook.com/ads/manager/account_settings/account_billing";
            chromeDriver.Navigate();

            try
            {
                var addPaymentMethod = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='globalContainer']/div/div/div/div/div/div/div/div/div/div/div/div/div/button")));
                addPaymentMethod.Click();
            }
            catch (Exception ex)
            {
                ChangeStatus(mainData.Id - 1, "Thêm thẻ", "Red");
                error++;
                UpdateSummary();
            }

            ////*[@id="u_i_4"]/div/div[4]/div[1]/label/input
            var creditCardNumber = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='AdsPaymentsFlowForm']/div/div/div/div/div/div[4]/div[1]/label/input")));
            //var creditCardNumber = chromeDriver.FindElement(By.CssSelector("//*[@name='creditCardNumber'"));

            //chromeDriver.SwitchTo().ActiveElement();
            creditCardNumber.SendKeys(mainData.CardNumber);

            var month = chromeDriver.FindElement(By.XPath("//*[@id='AdsPaymentsFlowForm']/div/div/div/div/div/div[4]/div[2]/label/input"));
            month.SendKeys(mainData.Month);

            var year = chromeDriver.FindElement(By.XPath("//*[@id='AdsPaymentsFlowForm']/div/div/div/div/div/div[4]/div[3]/label/input"));
            year.SendKeys(mainData.Year);

            var csc = chromeDriver.FindElement(By.XPath("//*[@id='AdsPaymentsFlowForm']/div/div/div/div/div/div[6]/div[1]/label/input"));
            csc.SendKeys(mainData.CardCode);

            Thread.Sleep(1000);
            var continueConfirm = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='XPaymentDialogFooter']/table/tbody/tr/td/div/div/button")));
            continueConfirm.Click();

            // Open a new Tab
            //chromeDriver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "t");
            //chromeDriver.SwitchTo().Window(chromeDriver.WindowHandles.Last());
            //chromeDriver.Navigate().GoToUrl(mainData.AdsAddress);

            chromeDriver.ExecuteScript($"window.open('{mainData.AdsAddress}','_blank');");

            //Thread.Sleep(3000);
            //CheckAlert();

            ChangeStatus(mainData.Id - 1, "Kết bạn", "Orange");
            var m = chromeDriver.ExecuteScript($"console.log(document.title);");
            chromeDriver.SwitchTo().Window(chromeDriver.WindowHandles.Last());

            try
            {
                var addFriend = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='profileEscapeHatchContentID']/div/div/div/div/button[1]")));
                addFriend.Click();
            }
            catch (Exception ex)
            {
            }

            if (CheckBeingFriend(chromeDriver, mainData, wait))
            {
                ChangeStatus(mainData.Id - 1, "Thêm quyền", "Orange");
                chromeDriver.Url = "https://www.facebook.com/ads/manager/account_settings/information";
                chromeDriver.Navigate();
                wait.Until(d => ((IJavaScriptExecutor)chromeDriver).ExecuteScript("return document.readyState").Equals("complete"));

                var addPeople = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div/div[2]/div/div[1]/div/div/div[2]/div[5]/div/div[2]/div[2]/div/div/div/div[1]/div/a/button")));
                addPeople.Click();

                wait.Until(d => ((IJavaScriptExecutor)chromeDriver).ExecuteScript("return document.readyState").Equals("complete"));
                var person = wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[9]/div[2]/div/div/form/div[2]/div/table/tbody/tr/td[1]/div/div/div/div/input")));

                chromeDriver.SwitchTo().ActiveElement();
                person.SendKeys(mainData.AdsName);

                //wait.Until(d => ((IJavaScriptExecutor)chromeDriver).ExecuteScript("return document.readyState").Equals("complete"));

                Thread.Sleep(1500);
                chromeDriver.FindElement(By.XPath("/html/body/div[9]/div[2]/div/div/form/div[2]/div/table/tbody/tr/td[1]/div/div/div/div/input")).SendKeys(Keys.ArrowDown);

                //chromeDriver.SwitchTo().ActiveElement();
                //Actions builder = new Actions(chromeDriver);
                //builder.SendKeys(Keys.Enter);

                Thread.Sleep(500);
                chromeDriver.FindElement(By.XPath("/html/body/div[9]/div[2]/div/div/form/div[2]/div/table/tbody/tr/td[1]/div/div/div/div/input")).SendKeys(Keys.Enter);

                Thread.Sleep(500);
                // Confirm
                var confirmBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[9]/div[2]/div/div/form/div[3]/button")));
                confirmBtn.Click();

                wait.Until(d => ((IJavaScriptExecutor)chromeDriver).ExecuteScript("return document.readyState").Equals("complete"));
                Thread.Sleep(1500);
                var closeBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[11]/div[2]/div/div/div/div[3]/a")));
                closeBtn.Click();
                ChangeStatus(mainData.Id - 1, "Hoàn thành", "Green");
                completed++;
                UpdateSummary();
            }
        }

        private bool CheckBeingFriend(ChromeDriver chromeDriver, MainData mainData, WebDriverWait wait)
        {
            ChangeStatus(mainData.Id - 1, "Chờ chấp nhận", "Orange");
            Thread.Sleep(5000);
            chromeDriver.Navigate().Refresh();
            try
            {
                wait.Until(d => ((IJavaScriptExecutor)chromeDriver).ExecuteScript("return document.readyState").Equals("complete"));
                var isFriend = chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div[2]/div[2]/div[1]/div/div[1]/div/div[3]/div/div[2]/div[2]/div/div/div[1]/a"));
                if (isFriend != null)
                    return true;
                return CheckBeingFriend(chromeDriver, mainData, wait);
            }
            catch (Exception ex)
            {
                return CheckBeingFriend(chromeDriver, mainData, wait);
            }
        }
        public void CheckAlert(ChromeDriver chromeDriver)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromMilliseconds(2000));
                wait.Until(ExpectedConditions.AlertIsPresent());
                IAlert alert = chromeDriver.SwitchTo().Alert();
                alert.Dismiss();
            }
            catch (Exception e)
            {
                //exception handling
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var response = MessageBox.Show("Bạn chắc chắn muốn thoát chương trình?", "Xác nhận",
                                   MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                //Application.Current.Shutdown();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            dgAccount.Items.Clear();
            index = 0;
        }
    }
}
