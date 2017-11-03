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

        public void Search<T>(string key) where T : class
        {
            var response = _client.Search<T>(s=>s
                            .From(0)
                            .Size(10)
                            //.Query(q=>q.Match(m=>m.Field(f=>f.)))
                            .Query(q=>q.)
                            );
        }

    }
}
