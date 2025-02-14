using HtmlAgilityPack;
using Market.Models;
using Market.Services;
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
                } else if (Exchange.Contains("福証"))
                {
                    ex = "F";
                } else if (Exchange.Contains("札証"))
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
            // 创建ChromeOptions对象并设置无头模式
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            // 设置页面加载策略为Eager，即页面加载到可交互状态时即认为加载完成
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            //# chrome.exe可執行檔的路徑，查看方法。chrome://version/
            options.BinaryLocation = ConfigurationManager.AppSettings["ChromePath"];

            //# 启用带插件的浏览器 設定檔路徑 设置成用户自己的数据目录
            //options.add_argument("--user-data-dir=" + r"C:/Users/tanoshi/AppData/Local/Chromium/User Data/")

            // ChromeDriver 的实际路径
            string driverPath = ConfigurationManager.AppSettings["DriverPath"];
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(driverPath);
            service.HideCommandPromptWindow = true; // 隐藏命令行窗口

            // 创建WebDriver实例
            var webDriver = new ChromeDriver(service, options);

            // 设置页面加载超时时间为 单位 秒
            webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(200);

            try
            {
                // 打开网页
                await Task.Run(() => webDriver.Navigate().GoToUrl(Url));

                // 搜索并点击
                IWebElement element = webDriver.FindElement(By.XPath("//a[contains(@href, '/quote/') and contains(@href, '/key-statistics/') ]"));
                element.Click();

                // 等待页面加载完成，这里使用显式等待
                WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                // 获取新页面的HTML
                string html = webDriver.PageSource;
                return WebAnalysis(html);
            }
            finally
            {
                // 关闭所有窗口，退出 WebDriver 实例
                webDriver.Quit();
            }
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
    }

}
