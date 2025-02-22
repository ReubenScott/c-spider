using HtmlAgilityPack;
using Market.Models;
using Market.Services;
using Microsoft.Playwright;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Market.Headless
{

    class Yahoo : DataIntegrator
    {
        protected override string Url
        {
            // 東証 https://finance.yahoo.com/quote/7615.T/key-statistics/
            // 福証 https://finance.yahoo.com/quote/3047.F/key-statistics/
            // 札証 https://finance.yahoo.com/quote/9085.S/key-statistics/
            get
            {
                string ex = "T";
                if (Exchange.Contains("東証"))
                {
                    ex = "T";
                }
                else if (Exchange.Contains("福証"))
                {
                    ex = "F";
                }
                else if (Exchange.Contains("札証"))
                {
                    ex = "S";
                }
                return $"https://finance.yahoo.com/quote/{Symbol}.{ex}/";
            }
        }

        public Yahoo(string symbol)
        {
            Symbol = symbol;
        }

        public Yahoo(string symbol, string exchange)
        {
            Symbol = symbol;
            Exchange = exchange;
        }


        private static readonly Dictionary<string, string> mapping = new Dictionary<string, string>
        {
            { "Market Cap", "MarketCap" },
            { "Enterprise Value", "EnterpriseValue" },
            { "Price/Book", "PBR" },
            { "Trailing P/E", "PER" },
            { "Enterprise Value/Revenue", "EVRevenue" },
            { "Enterprise Value/EBITDA", "EVEBITDA" },
            { "Return on Assets", "ROA" },
            { "Return on Equity", "ROE" },
            { "Diluted EPS", "EPS" },
            { "52 Week Low", "YearLow" },
            { "52 Week High", "YearHigh" },
            { "52 Week Range", "YearChangeRatio" },
            { "200-Day Moving Average", "MovingAverage" },
            { "Forward Annual Dividend Yield", "DividendYield" },
            { "Ex-Dividend Date", "ExDividendDate" },
            { "Book Value Per Share", "BookValuePerShare" },
            { "Total Debt/Equity", "DebtEquityRatio" },
        };

        /// <summary>
        /// Webからデータの取得
        /// </summary>
        public override async Task<CompanyStatistics> GetCompanyProfile()
        {
            //string html = await PlaywrightAsync();
            string html = await SeleniumAsync();
            return WebAnalysis(html);
        }

        public override CompanyStatistics WebAnalysis(string html)
        {
            // 使用HTMLAgilityPack解析HTML文档
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var profile = new CompanyStatistics { Symbol = Symbol };

            var elements = document.DocumentNode.SelectNodes("//div[contains(@class, 'table-container')]//table//tr")
                .Concat(document.DocumentNode.SelectNodes("//div[contains(@class, 'container')]//table//tr"))
                .Select(node => node.SelectNodes("td"))
                .Where(node => node != null && node.Count > 1)
                .ToList();

            foreach (var element in elements)
            {
                var key = element[0].SelectNodes("./.")?.Select(
                    td =>
                    {
                        // 移除<sup> 标签
                        foreach (var supNode in td.Descendants("sup").ToList())
                        {
                            supNode.Remove();
                        }
                        return td.InnerText.Trim();
                    }
                ).First();
                var value = element[1].InnerText.Trim();

                // 剔除掉 () 之间的内容
                key = Regex.Replace(key, @"\(.*?\)", "").Trim();

                if (value != "--" && key == "Ex-Dividend Date")
                {
                    // 将日期字符串转换为 datetime 对象
                    var date = DateTime.ParseExact(value, "M/d/yyyy", null);
                    // 将 datetime 对象转换为 yyyymmdd 格式
                    value = date.ToString("yyyy/MM/dd");
                }

                // 配当利回り%
                if (value == "0.00%" && key == "Trailing Annual Dividend Yield")
                {
                    continue;
                }

                if (mapping.ContainsKey(key))
                {
                    SetPropertyValue(profile, mapping[key], value);
                }
            }


            return profile;
        }


        /// <summary>
        /// Playwrightでデータの取得
        /// </summary>
        private async Task<string> PlaywrightAsync()
        {
            IPlaywright playwright = null;
            IBrowser browser = null;
            IPage page = null;
            try
            {
                playwright = await Playwright.CreateAsync();

                // 创建Chromium浏览器实例
                browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
                {
                    Headless = true, // 关闭无头模式(有界面)
                    Channel = "chrome", // 指定采用chrome浏览器类型
                    Devtools = true, // 启用开发者工具
                    ChromiumSandbox = false, // 关闭浏览器沙盒
                    ExecutablePath = ConfigurationManager.AppSettings["ChromePath"], // 指定浏览器可执行文件位置
                    Args = new[] { "--enable-automation=true", "--disable-blink-features=AutomationControlled" }, // 防止被检测
                });

                page = await browser.NewPageAsync();

                await page.GotoAsync(Url, new PageGotoOptions()
                {
                    Timeout = 100 * 1000 // 超时: 毫秒
                });

                // <a href="/quote/7615.T/key-statistics/"><span>Statistics</span></a>
                // 定位元素 CSS Selector 通常更快
                var locator = page.Locator("a[href*='/quote/'][href*='/key-statistics/'] > span:has-text('Statistics')");

                // 使用 XPath
                //var locator = page.Locator("//a[contains(@href, '/quote/') and contains(@href, '/key-statistics/')]/span[text()='Statistics']");

                // 获取匹配到元素的个数
                var count = await locator.CountAsync();
                Console.WriteLine($"Number of matching elements: {count}");

                // 点击 获取第一个匹配的元素
                await locator.First.ClickAsync();

                // 获取所有匹配的元素
                //var elements = await locator.AllInnerTextsAsync();
                //foreach (var element in elements)
                //{
                //    var innerText = await element.InnerTextAsync();
                //    Console.WriteLine($"Found element with text: {element}");
                //}

                // Wait for navigation to complete
                await page.WaitForNavigationAsync(new PageWaitForNavigationOptions()
                {
                    Timeout = 100 * 1000 // 超时: 毫秒
                });

                // Now you can get the HTML content of the new page
                var html = await page.ContentAsync();


                //var title = await page.InnerTextAsync("title");
                //Console.WriteLine(title);
                return html;
            }
            finally
            {
                // 关闭所有窗口，退出实例
                if (page != null ) await page.CloseAsync();
                if (browser != null) await browser.CloseAsync();
                if (playwright != null) playwright.Dispose();
            }

        }


        /// <summary>
        /// Seleniumで、データの取得
        /// </summary>
        private async Task<string> SeleniumAsync()
        {
            ChromeDriver webDriver = null;
            try
            {
                // 创建ChromeOptions对象并设置无头模式
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
                // 设置页面加载策略为Eager，即页面加载到可交互状态时即认为加载完成
                options.PageLoadStrategy = PageLoadStrategy.Eager;
                //# chrome.exe可執行檔的路徑
                options.BinaryLocation = ConfigurationManager.AppSettings["ChromePath"];

                //# 启用带插件的浏览器 設定檔路徑 设置成用户自己的数据目录
                //options.add_argument("--user-data-dir=" + r"C:/Users/tanoshi/AppData/Local/Chromium/User Data/")

                // ChromeDriver 的实际路径
                string driverPath = ConfigurationManager.AppSettings["DriverPath"];
                ChromeDriverService service = ChromeDriverService.CreateDefaultService(driverPath);
                service.HideCommandPromptWindow = true; // 隐藏命令行窗口

                // 创建WebDriver实例
                webDriver = new ChromeDriver(service, options);

                //timeout: 最长超时时间
                int timeout = 200;

                // 设置页面加载超时时间为 单位 秒
                webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeout);

                // 显式等待
                WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout));

                // 打开网页
                await Task.Run(() => webDriver.Navigate().GoToUrl(Url));

                // 等待特定元素出现
                wait.Until(d => d.FindElement(By.XPath("//a[contains(@href, '/quote/') and contains(@href, '/key-statistics/')]")));

                // 搜索并点击
                IWebElement element = webDriver.FindElement(By.XPath("//a[contains(@href, '/quote/') and contains(@href, '/key-statistics/')]"));
                element.Click();

                // 等待页面加载完成，这里使用显式等待
                //wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                // 等待特定元素出现
                wait.Until(d => d.FindElement(By.XPath("//div[contains(@class, 'table-container')]")));

                // 获取新页面的HTML
                string html = webDriver.PageSource;
                return html;
            }
            finally
            {
                // 关闭所有窗口，退出 WebDriver 实例
                if (webDriver != null) webDriver.Quit(); 
            }
        }
    }

}
