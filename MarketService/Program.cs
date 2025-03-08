using Market.Services;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Market
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new DaemonService()
            };
            ServiceBase.Run(ServicesToRun);
        }

            
        static async Task Main1()
        {
            MarketService marketService = new MarketService();

            string tradingDate = "2025/03/07";  // 業務基準日 yyyy/MM/dd")

            HashSet<string> symbols = new HashSet<string>();
            //symbols.Add("7966");
            //symbols.Add("3358");
            //symbols.Add("3845");
            //symbols.Add("5446");
            symbols.Add("3470");

            foreach (string symbol in symbols)
            {
                await marketService.UpdateCompanyProfile(symbol, tradingDate);
            }
            //marketService.GetBatchSymbol("2024/10/08", 2);
            //marketService.UpdateDelistedStatus(symbols);
        }
    }
}
