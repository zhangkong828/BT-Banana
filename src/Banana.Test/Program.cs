using Banana.Common;
using System;

namespace Banana.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://api.47ks.com/webcloud/?v=https://v.qq.com/x/cover/yoz60y87rdgl1vp/e0024dxa4jv.html";
            HttpHelper.Get(url);

            Console.ReadKey();
        }
    }
}
