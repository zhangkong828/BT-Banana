using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace BT.Banana.Web.Core
{
    public class Log
    {
        //在网站根目录下创建日志目录
        public static string path = HttpContext.Current.Request.PhysicalApplicationPath + "logs";

        /// <summary>
        /// 写入调试信息
        /// </summary>
        public static void Debug(string info, Exception ex)
        {
            if (Config.LOG_LEVENL >= 3)
            {
                WriteLog("DEBUG", info, ex);
            }
        }

        /// <summary>
        /// 写入运行时信息
        /// </summary>
        public static void Info(string info, Exception ex)
        {
            if (Config.LOG_LEVENL >= 2)
            {
                WriteLog("INFO", info, ex);
            }
        }

        /// <summary>
        /// 写入错误信息
        /// </summary>
        public static void Error(string info, Exception ex)
        {
            if (Config.LOG_LEVENL >= 1)
            {
                WriteLog("ERROR", info, ex);
            }
        }
       

        private static void WriteLog(string type, string info, Exception ex)
        {
            //如果日志目录不存在就创建
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

            var exception = GetInnerException(ex);

            //创建或打开日志文件，向日志文件末尾追加记录
            using (StreamWriter mySw = File.AppendText(filename))
            {
                var sb = new StringBuilder();
                sb.AppendLine("===================================================");
                sb.AppendLine($"CreateOn：{time}     Type：{type}");
                sb.AppendLine("---------------------------------------------------");
                sb.AppendLine("Info：");
                sb.AppendLine(info);
                sb.AppendLine("---------------------------------------------------");
                sb.AppendLine("Message：");
                sb.AppendLine(exception.Message);
                sb.AppendLine("---------------------------------------------------");
                sb.AppendLine("StackTrace：");
                sb.AppendLine(exception.StackTrace);
                sb.AppendLine("---------------------------------------------------");
                sb.AppendLine("TargetSite：");
                sb.AppendLine(exception.TargetSite.ToString());
                sb.AppendLine("===================================================");
                sb.AppendLine("");
                sb.AppendLine("");
                mySw.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// 获取最底层的exception
        /// </summary>
        private static Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return GetInnerException(ex.InnerException);
            }
            return ex;

        }

    }
}