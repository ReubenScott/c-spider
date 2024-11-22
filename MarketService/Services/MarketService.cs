using Market.Headless;
using Market.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Data.Linq.Mapping;

namespace Market.Services
{
    class MarketService
    {
        //データベースのファイルパスを指定
        //string connectionString = $"Data Source={db_file};Journal Mode=Memory;Synchronous=Full";
        private readonly string connectionString = $"Data Source={Config.db_file};Journal Mode=Memory;Synchronous=Full";

        /// <summary>
        /// 値上がり率 / 値下がり率ランキング
        /// </summary>
        /// <returns></returns>
        public List<Tuple<string, string, string>> InitializeSymbol(string tradingDate)
        {
            var profiles = new List<Tuple<string, string, string>>();
            List<string> symbols = new Kabuka().GetRankingList().Result;

            using (var connection = new SQLiteConnection(connectionString))
            {
                //データベースへ接続
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    //データ抽出
                    cmd.CommandText = @"SELECT symbol, name, update_date FROM company_statistics
                            WHERE delisting_date IS NULL
                            AND (update_date<> @tradingDate OR update_date IS NULL) 
                            AND symbol IN ( " + string.Join(",", symbols) + " )";

                    cmd.Parameters.AddWithValue("@tradingDate", tradingDate);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.WriteLine($"{reader["symbol"]} | {reader["name"]} | {reader["update_date"]}");
                            var line = reader.GetValues();
                            var profile = Tuple.Create(line.Get("symbol"), line.Get("name"), line.Get("update_date"));
                            profiles.Add(profile);
                        }
                    }
                }

                connection.Close();
            }

            return profiles;
        }

        /// <summary>
        /// 按更新时间顺序，取最久没更新的
        /// </summary>
        /// <param name="growth"></param>
        /// <returns></returns>
        public List<Tuple<string, string, string>> GetBatchSymbol(string tradingDate, int growth)
        {
            var profiles = new List<Tuple<string, string, string>>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                //データベースへ接続
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    //データ抽出 
                    cmd.CommandText = @"SELECT symbol, name, update_date FROM company_statistics
                             WHERE (update_date <> ? OR update_date IS NULL)
                                 AND delisting_date IS NULL
                             ORDER BY update_date ASC, RANDOM() LIMIT ?";

                    cmd.Parameters.AddWithValue("@tradingDate", tradingDate);
                    cmd.Parameters.AddWithValue("@limit", 50 + growth);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.WriteLine($"{reader["symbol"]} | {reader["name"]} | {reader["update_date"]}");
                            var line = reader.GetValues();
                            var profile = Tuple.Create(line.Get("symbol"), line.Get("name"), line.Get("update_date"));
                            profiles.Add(profile);
                        }
                    }
                }

                connection.Close();
            }
            return profiles;
        }



        /// <summary>
        /// 更新銘柄基本情報
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public async Task UpdateCompanyProfile(string symbol, string dateString)
        {
            CompanyStatistics profile1 = await new Kabumap(symbol).GetCompanyProfile();
            CompanyStatistics profile2 = await new Nikkei(symbol).GetCompanyProfile();
            CompanyStatistics profile3 = await new Kabuyoho(symbol).GetCompanyProfile();
            CompanyStatistics profile4 = await new Minkabu(symbol).GetCompanyProfile();

            CompanyStatistics profile5 = profile4;  // await new Yahoo(symbol, profile4.Exchange).GetCompanyProfile();
            // 名証  https://finance.yahoo.com/ 不支持
            if (!profile4.Exchange.Contains("名証"))
            {
                profile5 = await new Yahoo(symbol, profile4.Exchange).GetCompanyProfile();
            }

            CompanyStatistics profile = MergeProfiles(profile1, profile2, profile3, profile4, profile5);

            // 获取CompanyStatistics对象的属性
            var properties = typeof(CompanyStatistics).GetProperties();

            int count = 0;
            string updateColumn = "";

            // SQL  参数化
            var updateValues = new List<SQLiteParameter>();

            foreach (var property in properties)
            {
                // 获取属性的值
                var value = property.GetValue(profile);

                // 如果属性有值，则更新数据库中的字段
                if (value != null)
                {
                    // 获取应用到属性上的自定义特性
                    var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();


                    if (columnAttribute != null)
                    {
                        // 日付フォーマット変換
                        string typeName = property.PropertyType.Name;
                        Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                        if (underlyingType != null)
                        {
                            typeName = underlyingType.Name;
                        }
                        switch (typeName)
                        {
                            case "DateTime":
                                DateTime? dateTime = (DateTime?)value;
                                value = dateTime.Value.ToString("yyyy/MM/dd");
                                break;
                        }

                        // 金額単位変換
                        switch (property.Name)
                        {
                            case "MarketCap":
                            case "EnterpriseValue":
                                // 時価総額  企業価値  	単位：億円
                                value = (decimal)value / (10000 * 10000);
                                break;
                        }

                        // 使用占位符方式SQL拼接,  避免SQL注入风险
                        var paramName = $"@{columnAttribute.Name}";
                        if (count++ == 0)
                        {
                            updateColumn = $"{columnAttribute.Name} = {paramName}";
                        }
                        else
                        {
                            updateColumn += $", {columnAttribute.Name} = {paramName}";
                        }

                        updateValues.Add(new SQLiteParameter(paramName, value));
                    }

                }

            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var sql = $"UPDATE company_statistics SET update_date = '{dateString}', {updateColumn} WHERE symbol = '{symbol}'";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddRange(updateValues.ToArray());
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

        }


        public static T MergeProfiles<T>(params T[] objects) where T : new()
        {
            // 获取第一个对象的类型
            Type type = typeof(T);

            // 创建一个新的对象
            T mergedObject = Activator.CreateInstance<T>();

            // 获取类型的所有属性
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                foreach (T obj in objects.AsEnumerable().Reverse())
                {
                    // 如果对象中有属性值，则使用该值 末位优先级最高
                    if (property.GetValue(obj) != null)
                    {
                        property.SetValue(mergedObject, property.GetValue(obj));
                        // 一旦找到非空值，跳出循环
                        break;
                    }
                }
            }

            return mergedObject;
        }


        /// <summary>
        /// 更新上場廃止数据
        /// </summary>
        public List<string> UpdateDelistedStatus(HashSet<string> blackList)
        {
            List<Tuple<string, string>> profiles = new Jpx().GetDelisted().Result;

            List<string> symbols = new List<string>();

            string sql1 = @"UPDATE company_statistics
                     SET delisting_date = @delistingDate
                    WHERE symbol = @symbol";

            string sql2 = @"INSERT INTO company_statistics_archive
                     SELECT * FROM company_statistics
                     WHERE symbol = @symbol";

            string sql3 = @"DELETE FROM company_statistics
                    WHERE symbol = @symbol";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SQLiteCommand(connection))
                {
                    foreach (Tuple<string, string> item in profiles)
                    {
                        string symbol = item.Item1;
                        string delistingDate = item.Item2;

                        if (blackList.Contains(symbol))
                        {
                            //  company_statistics の変更
                            command.CommandText = sql1;
                            command.Parameters.AddWithValue("@symbol", symbol);
                            command.Parameters.AddWithValue("@delistingDate", delistingDate);
                            command.ExecuteNonQuery();

                            //  company_statistics_archive への追加
                            command.CommandText = sql2;
                            command.Parameters.AddWithValue("@symbol", "");
                            command.Parameters["@symbol"].Value = symbol;
                            command.ExecuteNonQuery();


                            //  company_statistics の削除
                            command.CommandText = sql3;
                            command.Parameters.AddWithValue("@symbol", symbol);
                            command.ExecuteNonQuery();

                            symbols.Add(symbol);
                        }
                    }

                    transaction.Commit();
                }

                connection.Close();
            }

            return symbols;
        }

        /// <summary>
        /// 初始化节假日
        /// </summary>
        public void InitHolidays()
        {
            string url = "https://holidays-jp.github.io/api/v1/date.json";
            string json = new BaseFrame().GetHttpContent(url).Result;

            // 使用LINQ获取JSON对象的键，并添加到DaemonService.Holidays列表中
            DaemonService.Holidays.AddRange(JObject.Parse(json).Properties().Select(p => p.Name));
        }


        // (更新日期 < 当前日期) 并且 (更新日期 < 最终交易日期 + 1天)
        public bool IsHoliday(string dateString)
        {
            DateTime currentDate = DateTime.Today; // 当前日期
            DateTime lastBusDate = currentDate;

            // 更新日期
            //string dateString = "2025-07-21";
            DateTime updateDate = DateTime.Parse(dateString);

            Console.WriteLine($"日期: {updateDate.ToString("yyyy-MM-dd")}");
            Console.WriteLine($"現在日付: {currentDate.ToString("yyyy-MM-dd")}");

            // (更新日期 < 当前日期) 并且 (更新日期 < 最终交易日期)
            if (updateDate < currentDate)
            {
                Console.WriteLine("更新日期小于当前日期");

                // 最终交易日期
                while (lastBusDate.DayOfWeek == DayOfWeek.Saturday || lastBusDate.DayOfWeek == DayOfWeek.Sunday
                     || DaemonService.Holidays.Contains(lastBusDate.ToString("yyyy-MM-dd")))
                {
                    lastBusDate = lastBusDate.AddDays(-1);
                }

                //  更新日期 < 最终交易日期
                if (updateDate < lastBusDate)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 获取最终交易日期
        /// </summary>
        /// <returns></returns>
        public string GetLastBusinessDate()
        {
            // 最终交易日期
            DateTime currentTime = DateTime.Now;
            DateTime lastBusDate = currentTime.Date;

            // 当前日期 收盘时间 之前  (暂时预定1小时 16:00 之后 ， 网站数据更新完毕)
            if (currentTime.Hour < 16)
            {
                lastBusDate = lastBusDate.AddDays(-1);
            }

            while (lastBusDate.DayOfWeek == DayOfWeek.Saturday || lastBusDate.DayOfWeek == DayOfWeek.Sunday
                 || DaemonService.Holidays.Contains(lastBusDate.ToString("yyyy-MM-dd")))
            {
                lastBusDate = lastBusDate.AddDays(-1);
            }

            return lastBusDate.ToString("yyyy/MM/dd");
        }



        /// <summary>
        /// 判断是否下载时间段
        /// </summary>
        /// <returns></returns>
        public bool IsWorkingTime()
        {
            bool isWorkingTime = false;
            DateTime currentTime = DateTime.Now;

            if (currentTime.DayOfWeek == DayOfWeek.Saturday || currentTime.DayOfWeek == DayOfWeek.Sunday
                 || DaemonService.Holidays.Contains(currentTime.ToString("yyyy-MM-dd")))
            {
                isWorkingTime = true;
            } else if (currentTime.Hour >= 16 || currentTime.Hour <= 7)
            {
                //  开盘之前 08:00 ； 收盘之后(暂时预定1小时 16:00 ，网站数据更新完毕)
                isWorkingTime = true;
            }

            return isWorkingTime;
        }


        public void FormatData()
        {
            string sql1 = @"SELECT symbol, market_cap ,enterprise_value from company_statistics
                    WHERE delisting_date IS NOT NULL";

            string sql2 = @"UPDATE company_statistics
                     SET market_cap = @marketCap, enterprise_value = @enterpriseValue
                    WHERE symbol = @symbol";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();


                var profiles = new List<Tuple<string, float, float>>();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SQLiteCommand(connection))
                {
                    //データ抽出 
                    command.CommandText = sql1;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var line = reader.GetValues();

                            string symbol = line.Get("symbol");
                            string marketCap = line.Get("market_cap");
                            string enterpriseValue = line.Get("enterprise_value");

                            if (marketCap.Contains("B") || marketCap.Contains("M") || marketCap.Contains("T") 
                             || enterpriseValue.Contains("B") || enterpriseValue.Contains("M") || enterpriseValue.Contains("T"))
                            {
                                // 時価総額  企業価値  	単位：億円
                                float _marketCap = float.Parse(DataIntegrator.ConvertNumberUnit(marketCap)) / (10000 * 10000);
                                float _enterpriseValue = float.Parse(DataIntegrator.ConvertNumberUnit(enterpriseValue)) / (10000 * 10000);

                                var profile = Tuple.Create(symbol, _marketCap, _enterpriseValue);

                                profiles.Add(profile);
                            }
                        }
                    }

                    foreach (Tuple<string, float, float> item in profiles)
                    {
                        string symbol = item.Item1;
                        float marketCap = item.Item2;
                        float enterpriseValue = item.Item3;
                   
                            //  company_statistics の変更
                            command.CommandText = sql2;
                            command.Parameters.AddWithValue("@marketCap", marketCap.ToString());
                            command.Parameters.AddWithValue("@enterpriseValue", enterpriseValue.ToString());
                            command.Parameters.AddWithValue("@symbol", symbol);
                            command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    connection.Close();
                }
            }



        }
    }
}
