using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly ElasticClient _client;
        public ElasticSearchService(ElasticClient client)
        {
            _client = client;
        }


    }
}
