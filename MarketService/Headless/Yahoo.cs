using HtmlAgilityPack;
using Market.Models;
using Market.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
                return $"https://finance.yahoo.com/quote/{Symbol}.{ex}/key-statistics/";
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
            //{ "Beta", "BETA" },
            { "Price/Book", "PBR" },
            { "Trailing P/E", "PER" },
            { "Return on Assets", "ROA" },
            { "Return on Equity", "ROE" },
            { "Diluted EPS", "EPS" },
            { "52 Week Low", "YearLow" },
            { "52 Week High", "YearHigh" },
            { "52 Week Range", "YearChangeRatio" },
            { "Forward Annual Dividend Yield", "DividendYield" },
            { "Ex-Dividend Date", "ExDividendDate" },
            { "Book Value Per Share", "BookValuePerShare" },
            { "Total Debt/Equity", "DebtEquityRatio" },
        };


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

                if (mapping.ContainsKey(key))
                {
                    SetPropertyValue(profile, mapping[key], value);
                }
            }

            return profile;
        }
    }

}
