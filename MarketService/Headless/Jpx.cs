using HtmlAgilityPack;
using Market.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Market.Headless
{
    class Jpx : BaseFrame
    {
        public async Task<List<Tuple<string, string>>> GetDelisted()
        {
            var url = "https://www.jpx.co.jp/listing/stocks/delisted/index.html";
            string html = await GetHttpContent(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var profiles = new List<Tuple<string, string>>();

            var elements = htmlDocument.DocumentNode.SelectNodes("//div[@id='readArea']//div[@class='component-normal-table']//table//tbody//tr").ToList();

            foreach (var element in elements)
            {
                // 选择所有的td元素
                var tdElements = element.SelectNodes("./td");
                if (tdElements != null)
                {
                    var delistDate = tdElements[0].InnerText.Trim(); // 上場廃止日
                    var name = tdElements[1].InnerText.Trim();  // 銘柄名
                    var symbol = tdElements[2].InnerText.Trim();  // コード
                    var exchange = tdElements[3].InnerText.Trim();  // 市場区分
                    var delistReason = tdElements[4].InnerText.Trim();  // 上場廃止理由

                    Console.WriteLine($"上場廃止日：{delistDate} 銘柄名：{name} コード：{symbol} 市場区分：{exchange} 上場廃止理由：{delistReason}");

                    var profile = Tuple.Create(symbol, delistDate);
                    profiles.Add(profile);
                }
            }

            return profiles;
        }
    }

}
