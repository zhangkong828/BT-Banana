using Banana.Models;
using Banana.Models.ViewModels;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Services
{
    public class MagnetSearchService : IMagnetSearchService
    {
        private readonly ElasticClient _client;

        public string IndexName = "dht";
        public string TypeName = "infos";

        public MagnetSearchService(ElasticClient client)
        {
            _client = client;
        }

        public MagnetLinkSearchResultViewModel MagnetLinkSearch(string key, int pageIndex, int pageSize = 10)
        {
            var result = new MagnetLinkSearchResultViewModel()
            {
                SearchId = Guid.NewGuid().ToString(),
                SearchKey = key,
                PageIndex = pageIndex,
                PageSize = pageSize

            };

            var response = _client.Search<MagnetLink>(s => s
                            .Index(IndexName)
                            .Type(TypeName)
                            .From((pageIndex - 1) * pageSize)
                            .Size(pageSize)
                            //.Query(q => q.
                            //    MultiMatch(mm => mm.Fields(fs => fs.Fields(f => f.Name, f => f.InfoHash)).Query(key)
                            //    ))
                            //.Query(q => q.
                            //    Match(m => m.Field(f => f.Name).Query(key)
                            //))
                            //.Query(q => q
                            //    .MatchAll()
                            //)
                            .Query(q => q
                                .Bool(b => b
                                    .Should(sd => sd
                                        .Term(t => t.Field(f => f.InfoHash).Value(key)),
                                        sd => sd
                                        .Match(m => m
                                            .Field(f => f.Name)
                                            .Query(key)
                                )
                            )))
                            .Highlight(h => h
                                //.PreTags("<em>")
                                //.PostTags("</em>")
                                .Fields(
                                    fs => fs.Field(f => f.Name)
                                )

                            )
                            //.Sort(st => st.Descending(d => d.CreateTime))
                            .Source(sc => sc.IncludeAll())
                            );
            result.Totals = (int)response.Total;
            result.FindTime = response.Took.ToString();//毫秒
            foreach (var hit in response.Hits)
            {
                var item = hit.Source;
                //高亮
                var highlightValue = new HighlightHit();
                if (hit.Highlights.TryGetValue("name", out highlightValue))
                    item.Name = highlightValue.Highlights.FirstOrDefault() ?? item.Name;

                result.SearchResult.Add(item);
            }
            return result;
        }

        public MagnetLink MagnetLinkInfo(string infohash)
        {
            //根据唯一id获取
            var response = _client.Get(new DocumentPath<MagnetLink>(infohash), pd => pd.Index(IndexName).Type(TypeName));
            return response.Source;
        }
    }
}
