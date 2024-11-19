using System.Net;
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
                handler.UseCookies = true;
                //var cookieContainer = new CookieContainer();
                //cookieContainer.Add(new Cookie("cookieName", "cookieValue") { Domain = "example.com" });
                //handler.CookieContainer = cookieContainer;

                if (Config.useProxy)
                {
                    handler.Proxy = new WebProxy(Config.proxyUrl, Config.proxyPort);
                    handler.UseProxy = true;
                    handler.PreAuthenticate = true;
                    handler.UseDefaultCredentials = false;
                    handler.Credentials = new NetworkCredential(Config.proxyUsername, Config.proxyPassword);
                }
                using (HttpClient client = new HttpClient(handler))
                {
                    // 设置 Accept 头，以指示客户端可以接受重定向
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));

                    // 设置用户代理
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

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
