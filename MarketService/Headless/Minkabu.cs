using HtmlAgilityPack;
using Market.Models;
using Market.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Market.Headless
{

    class Minkabu : DataIntegrator
    {

        protected override string[] Url
        {
            get
            {
                return new string[] {
                    $"https://minkabu.jp/stock/{Symbol}",              // 上場市場 
                    $"https://minkabu.jp/stock/{Symbol}/fundamental"   // 企業情報 
                };
            }
        }

        public Minkabu(string symbol)
        {
            Symbol = symbol;
            EquityProfile = new EquityProfile { Symbol = symbol };

            // 添加回调函数
            Callbacks.Add(Url[0], ExchangeClassificationAnalysis);
            Callbacks.Add(Url[1], CompanyStatisticsAnalysis);
        }


        private static readonly Dictionary<string, string> mapping = new Dictionary<string, string>
        {
            // 株式（上場市場）の状況
            { "上場市場", "Exchange" },
            { "上場年月日", "ListingDate" },
            { "単元株数", "PerUnit" },

            // 銘柄基本情報
            { "住所", "Address" },
            { "電話番号(IR)", "Tel" },
            { "資本金", "CapitalStock" },
        };


        // 上場市場 https://minkabu.jp/stock/3470
        public void ExchangeClassificationAnalysis(string html)
        {
            // 使用HTMLAgilityPack解析HTML文档
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            //市場区分   3470&nbsp;&nbsp;東証REIT
            var element = document.DocumentNode.SelectNodes("//div[@id='stock_header_contents']//div[@class='stock_label']")
                .Select(node => node.InnerText.Trim()).First();
            // 将 HTML 实体转换为对应的字符
            EquityProfile.Exchange = WebUtility.HtmlDecode(element.Replace(Symbol, string.Empty)).Trim();

            // 業種 REIT
        //    element = document.DocumentNode.SelectNodes("//div[@id='sh_field_body']//div[@class='ly_content_wrapper size_ss']//a")
            element = document.DocumentNode.SelectNodes("//div[@id='sh_field_body']//div[contains(@class, 'ly_content_wrapper size_ss')]//a[1]")
                .Select(node => node.InnerText.Trim()).First();

            EquityProfile.Sector = WebUtility.HtmlDecode(element).Trim();

        }

        /// <summary>
        /// Web内容の分析
        /// </summary>
        /// <param name="html">网页内容</param>
        // 企業情報 https://minkabu.jp/stock/7966/fundamental
        public void CompanyStatisticsAnalysis(string html)
        {
            // 使用HTMLAgilityPack解析HTML文档
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);


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
                        SetPropertyValue(EquityProfile, mapping[key], value);
                    }
                }
            }

        }
    }

}
