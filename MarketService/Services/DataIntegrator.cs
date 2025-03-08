using Market.Models;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;

namespace Market.Services
{
    abstract class DataIntegrator : BaseFrame
    {
        /// <summary>
        /// 目标网站的URL
        /// </summary>
        protected abstract string[] Url { get; }

        /// <summary>
        /// 銘柄 股票代码 
        /// </summary>
        protected string Symbol { get; set; }


        /// <summary>
        /// 株情報
        /// </summary>
        protected EquityProfile EquityProfile;


        /// <summary>
        /// 回调函数名和对应的值
        /// 键的类型是string，值的类型是Action<string>
        /// Action<string>是一个委托类型
        /// </summary>
        protected Dictionary<string, Action<string>> Callbacks = new Dictionary<string, Action<string>>();


        /// <summary>
        /// Webからデータの取得
        /// </summary>
        public virtual async Task<EquityProfile> GetCompanyProfile()
        {

            foreach(string url in Url)
            {
                string html = await GetHttpContent(url);
                if (html == null)
                {
                    break;
                }

                // 调用回调函数
                Callbacks[url](html);
            }

            return EquityProfile;
        }


        protected void SetPropertyValue(object profile, string field , string value)
        {
            // 将 HTML 实体转换为对应的字符
            string decodedStr = WebUtility.HtmlDecode(value).Trim();
            // 字符- 至少出现两次  --   ---  ---倍
            decodedStr = Regex.Replace(decodedStr, "^-{2,}.*", "");

            // 如果为空退出
            if (string.IsNullOrWhiteSpace(decodedStr))
            {
                return;
            }

            // 获取字段类型
            PropertyInfo property = profile.GetType().GetProperty(field);
            string typeName = property.PropertyType.Name;
            Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
            if (underlyingType != null)
            {
                typeName = underlyingType.Name;
            }


            switch (typeName)
            {
                case "Int32":  // 出来高  264,400株
                    property.SetValue(profile, int.Parse(ConvertNumberUnit(decodedStr)));
                    break;
                case "Int64":  // 発行済み株式数	2,151,545,900(株)
                    property.SetValue(profile, long.Parse(ConvertNumberUnit(decodedStr)));
                    break;
                case "Single":
                    property.SetValue(profile, float.Parse(ConvertNumberUnit(decodedStr)));
                    break;
                case "Decimal":  // 年高値  196.0
                    property.SetValue(profile, decimal.Parse(ConvertNumberUnit(decodedStr)));
                    break;
                case "DateTime":  // "1971年8月5日"
                    string format1 = "yyyy年M月d日";
                    string format2 = "yyyy/M/d";

                    DateTime dateTime;
                    if (!DateTime.TryParseExact(decodedStr, format1, null, System.Globalization.DateTimeStyles.None, out dateTime))
                    {
                        if(!DateTime.TryParseExact(decodedStr, format2, null, System.Globalization.DateTimeStyles.None, out dateTime))
                        {
                            // TODO 
                            throw new Exception($"日期格式不正确: {decodedStr}");
                        }
                    }

                    if (dateTime != DateTime.MinValue)
                    {
                        property.SetValue(profile, dateTime);
                    }

                    break;
                case "String":
                default:
                    property.SetValue(profile, decodedStr);
                    break;
            }

        }


        /// <summary>
        /// 数字の単位を変換する
        /// </summary>
        public static string ConvertNumberUnit(string decodedStr)
        {

            // 基本単位：　%、株　倍　円
            // 金額単位（億円） 変更必要:  円、十円、百円、千円、万円、百万円、億円、M、B、T　　

            //　796億円 58,435百万円    1兆5百万円   1,658,600株  1,598.0
            // ([-+])?(\d+\.?(?:,*\d+)?(?:\.\d+)?)\s*([^\d\s]+)?
            string pattern = @"([-+])?(\d+(?:,\d+)*(?:\.?\d+)?)\s*([^\d\s]+)?";
            
            // 创建匹配器
            MatchCollection matches = Regex.Matches(decodedStr, pattern);

            int sign = 1;  // 正负符号 默认为正数
            decimal sum = 0;

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                // 提取符号
                if (i == 0 && match.Groups[1].Value == "-")
                {
                    sign = -1;
                }
                // 提取数值和单位
                string value = match.Groups[2].Value;
                string units = match.Groups[3].Value;

                // 转换数值为浮点数
                decimal number = decimal.Parse(value.Replace(",", ""));

                // 根据单位计算倍数
                long multipliers = 1;
                foreach (char unit in units)
                {
                    long multiple;
                    switch (unit)
                    {
                        case '十':
                            multiple = 10;
                            break;
                        case '百':
                            multiple = 100;
                            break;
                        case '千':
                            multiple = 1000;
                            break;
                        case '万':
                            multiple = 10000;
                            break;
                        case '億':
                            multiple = 100000000;
                            break;
                        case '兆':
                            multiple = 1000000000000;
                            break;
                        case 'K':    // 千
                            multiple = 1000;
                            break;
                        case 'M':    // 百万
                            multiple = 1000000;
                            break;
                        case 'B':    // 十億
                            multiple = 1000000000;
                            break;
                        case 'T':    // 万億
                            multiple = 1000000000000;
                            break;
                        default:
                            multiple = 1;  // 默认倍数
                            break;
                    }
                    // 计算结果
                    multipliers = multipliers * multiple;
                }

                sum = sum + (number * multipliers);
            }

            sum = sign * sum;

            return sum.ToString();
        }

    }
}
