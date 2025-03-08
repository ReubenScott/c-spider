using HtmlAgilityPack;
using Market.Models;
using Market.Services;
using System.Collections.Generic;
using System.Linq;

namespace Market.Headless
{

    class Kabuyoho : DataIntegrator
    {
        /// <summary>
        /// 企業情報 https://kabuyoho.jp/reportTop?bcode=5020
        /// </summary>
        protected override string[] Url
        {
            get
            {

                return new string[] { $"https://kabuyoho.jp/reportTop?bcode={Symbol}" };
            }
        }


        public Kabuyoho(string symbol)
        {
            Symbol = symbol;
            EquityProfile = new EquityProfile { Symbol = symbol };

            // 添加回调函数
            Callbacks.Add(Url[0], CompanyStatisticsAnalysis);
        }


        private static readonly Dictionary<string, string> mapping = new Dictionary<string, string>
        {
            { "レーティング", "GradeRating" },
            { "PER(予)", "PER" },
            { "PBR(実)", "PBR" },
            { "ROA(実)", "ROA" },
            { "ROE(実)", "ROE" },
            { "配当利回り(予)", "DividendYield" },
            { "自己資本比率", "OwnCapitalRatio" },
            { "事業内容", "BusinessScope" },
            { "取扱い商品", "ProductRange" },
        };


        /// <summary>
        /// Web内容の分析
        /// </summary>
        /// <param name="html">网页内容</param>
        public void CompanyStatisticsAnalysis(string html)
        {
            // 使用HTMLAgilityPack解析HTML文档
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var elements = document.DocumentNode.SelectNodes("//div[@class='smary_box']//dl//dt| //div[@class='smary_box']//dl//dd")?.ToList();

            // 格納した銘柄が足りないのため
            if (elements != null)
            {
                for (int i = 0; i < elements.Count; i += 2)
                {
                    var key = elements[i].SelectNodes("./p")?.Select(p => p.InnerText.Trim()).First();
                    var value = elements[i + 1].SelectNodes("./p")?.Select(
                        p =>
                        {
                            // 移除P标签中的<span> 标签
                            foreach (var spanNode in p.Descendants("span").ToList())
                            {
                                spanNode.Remove();
                            }
                            return p.InnerText.Trim();
                        }
                    ).First();


                    if (key != null && mapping.ContainsKey(key))
                    {
                        SetPropertyValue(EquityProfile, mapping[key], value);
                    }
                }

                //  事業内容
                var businessElements = document.DocumentNode.SelectNodes("//section[@class='info_box info_box_contents']");
                if (businessElements.Count() > 0)
                {
                    var businessTitel = businessElements.Select(node => node.SelectNodes("./h2")).First().Select(p => p.InnerText.Trim()).First();
                    var businessScope = string.Join("\n", businessElements.Select(node => node.SelectNodes("./p")).First().Select(p => p.InnerText));

                    if (mapping.ContainsKey(businessTitel))
                    {
                        SetPropertyValue(EquityProfile, mapping[businessTitel], businessScope);
                    }
                }

                //  取扱い商品
                var productElements = document.DocumentNode.SelectNodes("//section[@class='info_box info_box_product']");
                if (productElements.Count > 0)
                {
                    var productTitel = productElements.Select(node => node.SelectNodes("./h2")).First().Select(p => p.InnerText.Trim()).First();
                    var productRange = string.Join("\n", productElements.Select(node => node.SelectNodes("./p")).First().Select(p => p.InnerText));

                    if (mapping.ContainsKey(productTitel))
                    {
                        SetPropertyValue(EquityProfile, mapping[productTitel], productRange);
                    }
                }
            }

        }
    }

}
