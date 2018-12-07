using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Services
{
    public interface IVideoRankingService
    {
        bool AccessStatistics(string id, string classify);
        
        List<KeyValuePair<string, double>> GetDayRankingByType(string type, int pageindex, int pagesize);
        

        List<KeyValuePair<string, double>> GetWeekRankingByType(string type, int pageindex, int pagesize);

        int GetAccessCount(string id, string classify);
    }
}
