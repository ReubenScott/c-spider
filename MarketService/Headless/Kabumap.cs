using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Market.Models;
using Market.Services;

namespace Market.Headless
{

    class Kabumap : DataIntegrator
    {
        protected override string Url
        {
            get
            {
                return $"https://dt.kabumap.com/servlets/dt/Action?SRC=basic/base&codetext={Symbol}";
            }
        }


        public Kabumap(string symbol)
        {
            Symbol = symbol;
        }


        private static readonly Dictionary<string, string> mapping = new Dictionary<string, string>
        {
            { "時価総額", "MarketCap" },
            { "配当利回り", "DividendYield" },
            { "PBR", "PBR" },
            { "PER", "PER" },
            { "出来高", "Volume" },
            { "年初来高値", "YearHigh" },
            { "年初来安値", "YearLow" },
            { "信用倍率", "CreditMultiplier" },
        };

        public override CompanyStatistics WebAnalysis(string html)
        {

            // 使用HTMLAgilityPack解析HTML文档
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            string price = document.DocumentNode.SelectNodes("//div[@class='priceArea']/span[@class='price']").First().InnerText;

            var elements = document.DocumentNode.SelectNodes(@"
             //div[contains(@class, 'upperArea') or contains(@class, 'lowerArea')]//dl/dt 
             | //div[contains(@class, 'upperArea') or contains(@class, 'lowerArea')]//dl/dd")
                .Select(node => node.InnerText.Trim())
                .ToList();

            var profile = new CompanyStatistics {Symbol = Symbol, PresentPrice = decimal.Parse(price) };

            for (int i = 0; i < elements.Count; i += 2)
            {
                var key = Regex.Replace(elements[i], @"\(.*?\)", "");
                var value = elements[i + 1];

                if (mapping.ContainsKey(key))
                {
                    SetPropertyValue(profile, mapping[key], value);
                }
            }

            return profile;
        }


        public static async Task UpdateCompanyProfile(List<string> symbols)
        {
            var start_time = DateTime.Now;
            var httpClient = new HttpClient();

            foreach (var symbol in symbols)
            {
                var url = $"https://dt.kabumap.com/servlets/dt/Action?SRC=basic/base&codetext={symbol}";
                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var elements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'upperArea') or contains(@class, 'lowerArea')]//dl/dt | //div[contains(@class, 'upperArea') or contains(@class, 'lowerArea')]//dl/dd")
                    .Select(node => node.InnerText.Trim())
                    .ToList();

                var row = new CompanyStatistics { Symbol = symbol };

                for (int i = 0; i < elements.Count; i += 2)
                {
                    var key = Regex.Replace(elements[i], @"\(.*?\)", "");
                    var value = elements[i + 1];

                    if (mapping.ContainsKey(key))
                    {
                        var field = mapping[key];
                        typeof(CompanyStatistics).GetProperty(field).SetValue(row, value);
                    }
                }

                // Update database logic here
            }

            var end_time = DateTime.Now;
            var elapsed_time = (end_time - start_time).TotalSeconds;

            Console.WriteLine($"耗时: {elapsed_time} 秒");
        }

        public async Task<CompanyStatistics> GetCompanyProfile1(string symbol)
        {
            var start_time = DateTime.Now;
            var httpClient = new HttpClient();

            var url = $"https://dt.kabumap.com/servlets/dt/Action?SRC=basic/base&codetext={symbol}";
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var elements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'upperArea') or contains(@class, 'lowerArea')]//dl/dt | //div[contains(@class, 'upperArea') or contains(@class, 'lowerArea')]//dl/dd")
                .Select(node => node.InnerText.Trim())
                .ToList();

            var row = new CompanyStatistics { Symbol = symbol };

            if (elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i += 2)
                {
                    var key = Regex.Replace(elements[i], @"\(.*?\)", "");
                    var value = elements[i + 1];

                    if (mapping.ContainsKey(key))
                    {
                        var field = mapping[key];
                        typeof(CompanyStatistics).GetProperty(field).SetValue(row, value);
                    }
                }
            }
            else
            {
                Console.WriteLine($"ウェブサイト「株マップ」に、銘柄「{symbol}」は登録されていません");
            }

            var end_time = DateTime.Now;
            var elapsed_time = (end_time - start_time).TotalSeconds;

            Console.WriteLine($"耗时: {elapsed_time} 秒");

            return row;
        }
    }

}