using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Helper
{
    public class HttpHelper
    {
        private static readonly HttpClient _httpClient;

        static HttpHelper()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        }

        public static async Task<string> Get(string url, string encoding = "UTF-8")
        {
            var html = "";
            int tryCount = 3;
            GetHtml:
            bool isError = false;
            try
            {
                var stream = await _httpClient.GetStreamAsync(url);
                using (var sr = new StreamReader(stream, Encoding.GetEncoding(encoding)))
                {
                    html = sr.ReadToEnd();
                }
                isError = string.IsNullOrWhiteSpace(html);
            }
            catch (Exception ex)
            {
                isError = true;
                //Logger.Warn("{0}请求失败：{1}", url, ex.Message);
            }
            if (isError)
            {
                if (tryCount > 0)
                {
                    tryCount--;
                    goto GetHtml;
                }
            }
            return html;
        }


        //public static string Get(string url, int timeOut = 15, string cookie = null, string encoding = "UTF-8")
        //{
        //    var html = "";
        //    try
        //    {
        //        var request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "GET";
        //        if (cookie != null)
        //            request.Headers[HttpRequestHeader.Cookie] = cookie;
        //        request.AllowAutoRedirect = true;
        //        request.Timeout = timeOut;
        //        var response = (HttpWebResponse)request.GetResponse();
        //        using (var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
        //        {
        //            html = sr.ReadToEnd();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log.Error(url, ex);
        //    }
        //    return html;
        //}
    }
}
