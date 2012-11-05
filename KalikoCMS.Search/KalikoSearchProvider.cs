
namespace KalikoCMS.Search {
    using System;
    using KalikoCMS.Events;
    using KalikoSearch.Core;

    public class KalikoSearchProvider : SearchProviderBase {
        private Collection _collection;
        private static string[] _searchFields = new[] { "title", "summary", "content" };

        public KalikoSearchProvider() {
            _collection = new Collection("KalikCMS");
        }

        public override void AddToIndex(IndexItem item) {
            string key = GetKey(item.PageId, item.LanguageId);
            PageDocument pageDocument = new PageDocument(key, item);
            _collection.AddDocument(pageDocument);
            
            DoOptimizations();
        }

        public override void RemoveFromIndex(Guid pageId, int languageId) {
            string key = GetKey(pageId, languageId);
            _collection.RemoveDocument(key);
        }

        public override void DoOptimizations() {
            _collection.OptimizeIndex();
        }

        private string GetKey(Guid pageId, int languageId) {
            string key = string.Format("{0}:{1}", pageId, languageId);
            return key;
        }

        public override void Init() {
            PageFactory.PageSaved += OnPageSaved;
        }

        void OnPageSaved(object sender, PageEventArgs e) {
            IndexPage(e.Page);
        }

        public override SearchResult Search(SearchQuery query) {
            KalikoSearch.Core.SearchResult searchResult = _collection.Search(query.SearchString, _searchFields, query.MetaData, query.ReturnFromPosition, query.NumberOfHitsToReturn);

            SearchResult result = ConvertResult(searchResult);

            return result;
        }

        private SearchResult ConvertResult(KalikoSearch.Core.SearchResult searchResult) {
            SearchResult result = new SearchResult();
            result.NumberOfHits = searchResult.NumberOfHits;
            result.SecondsTaken = searchResult.SecondsTaken;

            foreach (var hit in searchResult.Hits) {
                result.Hits.Add(new SearchHit() { Excerpt = hit.Excerpt, Path = hit.Path, Title = hit.Title, MetaData = hit.MetaData });
            }

            return result;
        }

    }
}
