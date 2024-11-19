using HtmlAgilityPack;
using Market.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Market.Headless
{
    class Kabuka : BaseFrame
    {
        public async Task<List<string>> GetRankingList()
        {
            // https://www.kabuka.jp.net/nensyorai-takane-yasune.html
            var url = "https://www.kabuka.jp.net/neagari-nesagari.html";
            string html = await GetHttpContent(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            List<string> symbols = new List<string>(); 

            var elements = htmlDocument.DocumentNode.SelectNodes("//div[@class='readmoretable']//div[@class='readmoretable_line']")
                .Select(node => node.InnerText.Trim())
                .ToList();

            foreach (var element in elements)
            {
                //var element = "マネーパートナーズグループ (8732) 東証スタンダード +80 +37.38%";
                string pattern = @"(\S+.*?)\s+\((\d*?)\)\s+(.*?)\s+(.*?)\s+(\S+.*?)\s*";
                Match match = Regex.Match(element, pattern);

                if (match.Success)
                {
                    var name = match.Groups[1].Value;
                    var symbol = match.Groups[2].Value;
                    var exchange = match.Groups[3].Value;
                    var change = match.Groups[4].Value;
                    var percentage = match.Groups[5].Value;

                    symbols.Add(symbol);
                    Console.WriteLine($"交易所：{exchange} 代码：{symbol} 名称：{name} 涨幅：{change} 百分比：{percentage}");
                }
            }

            return symbols;
        }
    }

}
