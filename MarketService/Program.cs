using Market.Headless;
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

            //marketService.FormatData();
            // 9941, 5596, 1773, 3895, 6905, 9257
            string symbol = "4755";
            await marketService.UpdateCompanyProfile(symbol, "2024/11/09");
            
            HashSet<string> symbols = new HashSet<string>();
            symbols.Add("2651");
            symbols.Add("2427");
            symbols.Add("9783");
            symbols.Add("4185");
            //marketService.GetBatchSymbol("2024/10/08", 2);
            //marketService.UpdateDelistedStatus(symbols);
        }
    }
}
