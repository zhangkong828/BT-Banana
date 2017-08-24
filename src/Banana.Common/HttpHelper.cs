using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Banana.Common
{
    public class HttpHelper
    {
        public static string Get(string url, string cookie = null, string encoding = "UTF-8")
        {
            var html = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                if (cookie != null)
                    request.Headers[HttpRequestHeader.Cookie] = cookie;
                var response = (HttpWebResponse)request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
                {
                    html = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
               // Log.Error(url, ex);
            }
            return html;
        }
    }
}
