using Market.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace Market
{
    public partial class DaemonService : ServiceBase
    {
        private MarketService marketService;

        /// <summary>
        /// 業務基準日 yyyy/MM/dd")
        /// </summary>
        private string latestTradingDate;

        /// <summary>
        /// 间隔时间
        /// </summary>
        private const int IntervalTime = 5 * 60 * 1000 ; //  毫秒

        /// <summary>
        /// 任务队列
        /// </summary>
        static LinkedList<string> SymbolQueue = new LinkedList<string>();

        /// <summary>
        /// 出错列表
        /// </summary>
        private HashSet<string> blackList = new HashSet<string>();

        /// <summary>
        /// 节假日
        /// </summary>
        public static List<string> Holidays = new List<string>();


        public DaemonService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 服务启动：指示服务开始运行时应采取的操作。 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            Logger.Debug("OnStart...");
            marketService = new MarketService();
            marketService.InitHolidays();

            Timer myTimer = new Timer();
            myTimer.Elapsed += TimerTickHandler;
            myTimer.Interval = IntervalTime;  // 毫秒
            myTimer.Enabled = true;
            myTimer.Start();

            // 创建消费者任务
            Task consumerTask = Task.Run(async () =>
            {
                while (true)
                {
                    if (SymbolQueue.First == null)
                    {
                        // 等待一段时间
                        System.Threading.Thread.Sleep(IntervalTime);
                    }
                    else
                    {
                        // 获取链表的第一个元素
                        var sw = new Stopwatch();
                        sw.Start();

                        string symbol = SymbolQueue.First.Value;

                        try
                        {
                            await marketService.UpdateCompanyProfile(symbol, latestTradingDate);
                            // 等待一段时间
                            System.Threading.Thread.Sleep(4000);   //  毫秒
                        }
                        catch (Exception ex)
                        {
                            blackList.Add(symbol);
                            Logger.Error($"UpdateCompanyProfile {symbol} Error : {ex.Message}");
                            Logger.Error(ex.StackTrace);
                            Logger.Info($"[BlackList] :  {string.Join(", ", blackList)}");
                        }
                        finally
                        {
                            SymbolQueue.RemoveFirst();
                            sw.Stop();
                            Logger.Debug($"SymbolQueue Consumed: {symbol}  Working... {sw.ElapsedMilliseconds} msec");
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 暂停：指示在服务暂停时应发生什么情况。
        /// </summary>
        protected override void OnPause()
        {
            Logger.Debug("OnPause...");
        }

        /// <summary>
        /// 继续：指示服务在暂停后恢复正常运行时应发生什么情况。
        /// </summary>
        protected override void OnContinue()
        {
            Logger.Debug("OnContinue...");
        }

        /// <summary>
        /// 服务停止：指示在服务停止运行时应发生什么情况。
        /// </summary>
        protected override void OnStop()
        {
            Logger.Debug("OnStop...");
        }

        /// <summary>
        /// 停止前：指示在系统关闭之前应发生什么情况（如果此时服务正在运行）。
        /// </summary>
        protected override void OnShutdown()
        {
            Logger.Debug("OnShutdown...");
        }


        /// <summary>
        /// 在这里编写你的定时器触发逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTickHandler(object sender, ElapsedEventArgs e)
        {
            Logger.Debug("计时器触发: " + e.SignalTime);
            try
            {
                if (marketService.IsWorkingTime())
                {
                    AddTask();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"AddTask Error :  {ex.Message}");
                Logger.Error(ex.StackTrace);
            }
        }


        /// <summary>
        /// 创建生产者任务
        /// </summary>
        private void AddTask()
        {
            string lastBusDate = latestTradingDate;
            latestTradingDate = marketService.GetLastBusinessDate();

            List<Tuple<string, string, string>> batchSymbol = marketService.GetBatchSymbol(latestTradingDate, blackList.Count);

            // 业务日期切换时(每日执行一次)
            if (latestTradingDate != lastBusDate)
            {
                batchSymbol.AddRange(marketService.InitializeSymbol(latestTradingDate));
            }

            // 创建生产者任务 
            foreach (Tuple<string, string, string> item in batchSymbol.AsEnumerable().Reverse())
            {
                string symbol = item.Item1;
                string name = item.Item2;

                if (blackList.Contains(symbol))
                {
                    continue;
                }

                // 检查元素是否已经存在于集合中
                if (!SymbolQueue.Contains(symbol))
                {
                    // 添加元素到队列中
                    SymbolQueue.AddLast(symbol);
                    Logger.Debug($"SymbolQueue Enqueue : {symbol}  {name}");
                }
            }

            // 出错列表重跑
            if (SymbolQueue.Count == 0 && blackList.Count > 0)
            {
                // 更新上場状況
                foreach (string delisting in marketService.UpdateDelistedStatus(blackList))
                {
                    blackList.Remove(delisting);
                }

                foreach (string symbol in blackList.ToList())
                {
                    if (blackList.Remove(symbol))
                    {
                        SymbolQueue.AddLast(symbol);
                        Logger.Debug($"SymbolQueue Enqueue : {symbol}");
                    }
                }
            }

        }

    }
}
