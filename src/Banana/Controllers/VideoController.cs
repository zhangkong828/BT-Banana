using Microsoft.AspNetCore.Mvc;

namespace Banana.Controllers
{
    public class VideoController : Controller
    {
        public VideoController()
        {

        }
        [Route("/video")]
        public IActionResult Video()
        {
            return View();
        }


        [Route("/s/video/{key}/{index?}")]
        public IActionResult VideoSearch(string key, string index)
        {
            //key = key.Trim();
            //if (string.IsNullOrEmpty(key))
            //    return RedirectToAction("index");
            //int.TryParse(index, out int currentIndex);
            //currentIndex = currentIndex < 1 ? 1 : currentIndex > 100 ? 100 : currentIndex; //最多100页
            //var pageSize = 10;
            //var result = new MagnetLinkSearchResultViewModel();
            ////只缓存前20页数据  1分钟
            //if (currentIndex > 20)
            //{
            //    result = _elasticSearchService.MagnetLinkSearch(key, currentIndex, pageSize);
            //}
            //else
            //{
            //    var cacheKey = $"s_m_{key}_{currentIndex}";
            //    if (!_memoryCache.TryGetValue(cacheKey, out result))
            //    {
            //        result = _elasticSearchService.MagnetLinkSearch(key, currentIndex, pageSize);
            //        _memoryCache.Set(cacheKey, result, new DateTimeOffset(DateTime.Now.AddMinutes(5)));
            //    }
            //}
            return View();
        }

        [Route("/d/video/{id}")]
        public IActionResult VideoDetail(string hash)
        {
            //if (string.IsNullOrEmpty(hash) || hash.Length != 40)
            //    return new RedirectResult("/error");
            //var model = new MagnetLink();
            //var cacheKey = $"d_m_{hash}";
            //if (!_memoryCache.TryGetValue(cacheKey, out model))
            //{
            //    model = _elasticSearchService.MagnetLinkInfo(hash);
            //    if (model == null)
            //        return new RedirectResult("/error");
            //    _memoryCache.Set(cacheKey, model, new TimeSpan(0, 30, 0));
            //}
            return View();
        }
    }
}