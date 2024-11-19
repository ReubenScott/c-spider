using HtmlAgilityPack;
using Market.Models;
using Market.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Market.Headless
{

    class Nikkei : DataIntegrator
    {
        protected override string Url
        {
            get
            {
                return $"https://www.nikkei.com/nkd/company/gaiyo/?scode={Symbol}";
            }
        }


        public Nikkei(string symbol)
        {
            Symbol = symbol;
        }

        public Nikkei() { }

        private static readonly Dictionary<string, string> mapping = new Dictionary<string, string>
        {
            { "正式社名", "Name" },
            { "設立年月日", "EstablishedDate" },
            { "日経業種分類", "Industry" },
            { "東証業種名", "Sector" },
            { "指数採用", "IndexAdoption" },
            { "URL", "Url" },
            { "代表者氏名", "Representative" },
            { "売買単位", "PerUnit" },
            { "本社住所", "Address" },
            { "電話番号", "Tel" },
            { "資本金", "CapitalStock" },
        };


        public override CompanyStatistics WebAnalysis(string html)
        {
            // 使用HTMLAgilityPack解析HTML文档
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var profile = new CompanyStatistics { Symbol = Symbol };

            foreach (var tr in document.DocumentNode.SelectNodes("//div[@class='m-articleFrame_body']//table//tr")?.Take(22))
            {
                var matches = Regex.Matches(tr.InnerText, @"(\S+.*?)\n+");
                var key = matches[0].Value.Trim();
                var value = matches[1].Value.Trim();

                // 正式社名    （株）削除
                if (key == "正式社名")
                {
                    value = value.Replace("（株）","");
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
