using Banana.Web.Models;
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

        public string IndexName = "dht";
        public string TypeName = "infos";

        public ElasticSearchService(ElasticClient client)
        {
            _client = client;
        }

        public void Search(string key)
        {
            var response = _client.Search<MagnetUrl>(s => s
                            .Index(IndexName)
                            .Type(TypeName)
                            .From(0)
                            .Size(10)
                            //.Query(q => q.
                            //    MultiMatch(mm => mm.Fields(fs => fs.Fields(f => f.Name, f => f.InfoHash)).Query(key)
                            //    ))
                            //.Query(q => q.
                            //    Match(m => m.Field(f => f.Name).Query(key)
                            //))
                            .Query(q => q
                                .MatchAll()
                            )
                            //.Query(q => q
                            //    .Bool(b => b
                            //        .Should(sd => sd
                            //            .Term(t => t.Field(f => f.InfoHash).Value(key)),
                            //            sd => sd
                            //            .Match(m => m
                            //                .Field(f => f.Name)
                            //                .Query(key)
                            //    )
                            //)))
                            .Sort(st => st.Descending(d => d.CreateTime))
                            .Source(sc => sc.IncludeAll())
                            );
            var a = 1;
        }

    }
}
