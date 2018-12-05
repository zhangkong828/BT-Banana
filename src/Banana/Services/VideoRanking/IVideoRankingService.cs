using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Services
{
    public interface IVideoRankingService
    {
        bool AccessStatistics(string id, string classify);

        List<KeyValuePair<string, double>> GetDayRanking(string classify, int pageindex, int pagesize);

        List<KeyValuePair<string, double>> GetWeekRanking(string classify, int pageindex, int pagesize);
    }
}
