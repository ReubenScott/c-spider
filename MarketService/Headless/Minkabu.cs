using HtmlAgilityPack;
using Market.Models;
using Market.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.Headless
{

    class Minkabu : DataIntegrator
    {
        protected override string Url
        {
            get
            {
                return $"https://minkabu.jp/stock/{Symbol}/fundamental";
            }
        }


        public Minkabu(string symbol)
        {
            Symbol = symbol;
        }


        private static readonly Dictionary<string, string> mapping = new Dictionary<string, string>
        {
            { "上場市場", "Exchange" },
            { "上場年月日", "ListingDate" },
            { "単元株数", "PerUnit" },
            { "住所", "Address" },
            { "電話番号(IR)", "Tel" },
            { "資本金", "CapitalStock" },
        };


        public override CompanyStatistics WebAnalysis(string html)
        {
            // 使用HTMLAgilityPack解析HTML文档
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var profile = new CompanyStatistics{Symbol = Symbol};

            var elements = document.DocumentNode.SelectNodes("//div[@class='ly_content_wrapper']//dl[@class='md_dataList']")
                .Select(node => node.InnerText.Trim())
                .ToList();

            foreach (var element in elements)
            {
                //var matches = Regex.Matches(element, @"(\S+.*?)\n+").Cast<Match>().Select(m => m.Value).ToList();
                string[] matches =  element.Split(new[] { "\n" }, StringSplitOptions.None);
                for (int i = 0; i < matches.Length; i += 2)
                {
                    var key = matches[i].Trim();
                    var value = matches[i + 1].Trim();

                    if (mapping.ContainsKey(key))
                    {
                        SetPropertyValue(profile, mapping[key], value);
                    }
                }
            }

            return profile;
        }
    }

}
