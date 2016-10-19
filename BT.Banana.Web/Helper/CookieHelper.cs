using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.Banana.Web.Helper
{
    public class CookieHelper
    {

        public static string Domain
        {
            get
            {

                var dom = ".btbanana.com";
#if DEBUG
                dom = "";
#endif
                ; return dom;
            }
        }

        
        /// <summary>
        /// 获取Cookie
        /// </summary>
        public static string GetValue(string keyName)
        {
            var cookie = HttpContext.Current.Request.Cookies[keyName];

            if (cookie != null)
            {
                return cookie.Value;
            }

            return "";
        }

        /// <summary>
        /// 设置Cookie，默认30天过期
        /// 如果val为空或null，则清空Cookie
        /// </summary>
        public static void SetValue(string keyName, string val)
        {
            SetValue(keyName, val, 30);
        }


        public static void SetValue(string keyName, string val, int expiresDay)
        {
            if (string.IsNullOrEmpty(val))
            {
                //设置过期，清空cookie
                var oldRes = HttpContext.Current.Response.Cookies[keyName];

                if (oldRes != null)
                {
                    oldRes.Value = val;
                    oldRes.Path = "/";
                    oldRes.Domain = Domain;
                    oldRes.Expires = DateTime.Now;
                }

                var oldReq = HttpContext.Current.Request.Cookies[keyName];

                if (oldReq != null)
                {
                    oldReq.Value = val;
                    oldReq.Path = "/";
                    oldReq.Domain = Domain;
                    oldReq.Expires = DateTime.Now;
                }

                return;
            }

            var cookieResponse = HttpContext.Current.Response.Cookies[keyName];

            if (cookieResponse != null)
            {
                cookieResponse.Value = val;
                cookieResponse.Path = "/";
                cookieResponse.Domain = Domain;
            }
            else
            {
                cookieResponse = new HttpCookie(keyName, val);

                cookieResponse.Path = "/";
                cookieResponse.Domain = Domain;

                HttpContext.Current.Response.AppendCookie(cookieResponse);
            }

            if (expiresDay > 0)
            {
                cookieResponse.Expires = DateTime.Now.AddDays(expiresDay);
            }

            var cookieRequest = HttpContext.Current.Request.Cookies[keyName];

            if (cookieRequest != null)
            {
                cookieRequest.Value = val;
            }
            else
            {
                cookieRequest = new HttpCookie(keyName, val);

                HttpContext.Current.Request.Cookies.Add(cookieRequest);
            }
        }
    }
}