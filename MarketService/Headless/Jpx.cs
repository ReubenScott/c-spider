using HtmlAgilityPack;
using Market.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Market.Headless
{
    class Jpx : BaseFrame
    {
        /// <summary>
        /// 上場廃止銘柄一覧の取得
        /// https://www.jpx.co.jp/listing/stocks/delisted/index.html
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tuple<string, string>>> GetDelisted()
        {
            var profiles = new List<Tuple<string, string>>();

            List<string> urls = new List<string>
            {
                "https://www.jpx.co.jp/listing/stocks/delisted/index.html",
                "https://www.jpx.co.jp/listing/stocks/delisted/archives-01.html"
            };

            foreach (var url in urls)
            {
                string html = await GetHttpContent(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

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

                        Debug.WriteLine($"上場廃止日：{delistDate} 銘柄名：{name} コード：{symbol} 市場区分：{exchange} 上場廃止理由：{delistReason}");

                        var profile = Tuple.Create(symbol, delistDate);
                        profiles.Add(profile);
                    }
                }
            }

            return profiles;
        }



        /// <summary>
        /// 休業日一覧の取得
        /// https://www.jpx.co.jp/corporate/about-jpx/calendar/index.html
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetHolidayList()
        {
            var url = "https://www.jpx.co.jp/corporate/about-jpx/calendar/index.html";
            string html = await GetHttpContent(url);
           
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var elements = htmlDocument.DocumentNode.SelectNodes("//div[@class='component-normal-table']//table[@class='overtable']//tr/td")
                .Select(node => node.InnerText.Trim())
                .ToList();

            List<string> holidays = new List<string>();
            for (int i = 0; i < elements.Count; i += 2)
            {
                var day = elements[i];  // 2024/01/01（月）
                var value = elements[i + 1];  // 元日

                string pattern = @"\（[月火水木金土日]）";
                day = Regex.Replace(day, pattern, "");

                holidays.Add(day);
                Debug.WriteLine($"日付：{day} 名称：{value}");
            }

            return holidays;
        }

    }

}
