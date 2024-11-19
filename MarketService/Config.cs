
using Market.Services;

namespace Market
{
    static class Config
    {
        //db_file = "sample.db";
        //        db_file = ":memory:";
        //private static string db_file = "C:/temp/market.db";
        public const string db_file = "E:/Data/SQLite/market.db";

        //public static string logfile = "C:/temp/market.log";
        public const string logfile = "E:/Data/SQLite/market.log";

        public const int LogLevel = (int)Logger.Level.Debug;

        public const bool useProxy = false; // 根据需要设置是否使用代理

        // 亿牛云（动态转发隧道代理）
        //爬虫代理加强版 代理服务器的认证信息
        public const string proxyUrl = "www.16yun.cn";
        public const int proxyPort = 3100;
        public const string proxyUsername = "16YUN";
        public const string proxyPassword = "16IP";
    }
}
