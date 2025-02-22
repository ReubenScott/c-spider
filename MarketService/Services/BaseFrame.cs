using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Market.Services
{
    class BaseFrame
    {
        /// <summary>
        /// Webからデータの取得
        /// </summary>
        /// <param name="url">目标网站的URL</param>
        /// <param name="totalPage">ページ総数</param>
        /// <returns></returns>
        public async Task<string> GetHttpContent(string Url)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                // 设置 cookies
                // handler.UseCookies = true;
                // 创建一个 CookieContainer 来存储 Cookie
                //var cookieContainer = new CookieContainer();
                //cookieContainer.Add(new Cookie("cookieName", "cookieValue") { Domain = "example.com" });
                //handler.CookieContainer = cookieContainer;

                /*
                // 获取所有的 Cookie // 替换为您的 Base URL
                CookieCollection cookies = handler.CookieContainer.GetCookies(new Uri(Url));
                // 打印所有的 Cookie
                foreach (Cookie cookie in cookies)
                {
                    Console.WriteLine($"Cookie Name: {cookie.Name}, Cookie Value: {cookie.Value}");
                }
                */

                // Proxy 根据需要设置是否使用代理
                //爬虫代理加强版 代理服务器的认证信息
                //if (Config.useProxy)
                //{
                //    handler.Proxy = new WebProxy(Config.proxyUrl, Config.proxyPort);
                //    handler.UseProxy = true;
                //    handler.PreAuthenticate = true;
                //    handler.UseDefaultCredentials = false;
                //    handler.Credentials = new NetworkCredential(Config.proxyUsername, Config.proxyPassword);
                //}
                using (HttpClient client = new HttpClient(handler))
                {
                    // 设置 Accept 头，以指示客户端可以接受重定向
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));

                    // 设置用户代理
                    client.DefaultRequestHeaders.Add("User-Agent", ConfigurationManager.AppSettings["UserAgent"]);

                    // 添加 API 密钥到请求头
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    using (HttpResponseMessage response = await client.GetAsync(Url))
                    {
                        // 确保响应状态码表示成功
                        response.EnsureSuccessStatusCode();

                        // 如果响应成功，继续处理响应内容
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }

        }

    }
}
