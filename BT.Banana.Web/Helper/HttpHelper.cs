using BT.Banana.Web.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BT.Banana.Web.Helper
{
    public class HttpHelper
    {
        public static string Get(string url, string cookie = null)
        {
            var html = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                if (cookie != null)
                    request.Headers[HttpRequestHeader.Cookie] = cookie;
                var response = (HttpWebResponse)request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    html = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Log.Error(url, ex);
            }
            return html;
        }
    }
}