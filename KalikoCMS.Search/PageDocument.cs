namespace KalikoCMS.Search {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using KalikoSearch.Core;

    public class PageDocument : IndexDocument {
        public PageDocument(string key, IndexItem item) {
            Key = key;
            Category = item.Category;
            Content = item.Content;
            Created = item.Created;
            LanguageId = item.LanguageId;
            MetaData = item.MetaData;
            Modified = item.Modified;
            PageId = item.PageId;
            Path = item.Path;
            Summary = item.Summary;
            Title = item.Title;
            PublishStart = item.PublishStart ?? DateTime.MaxValue;
            PublishStop = item.PublishStop ?? DateTime.MaxValue;
        }

        protected DateTime PublishStop {
            get {
                return ConvertLongToDate(GetNumericFieldValue("publishStop"));
            }
            set {
                AddNumericField("publishStop", ConvertDateToLong(value));
            }
        }

        protected DateTime PublishStart {
            get {
                return ConvertLongToDate(GetNumericFieldValue("publishStart"));
            }
            set {
                AddNumericField("publishStart", ConvertDateToLong(value));
            }
        }

        protected string Summary {
            get {
                return GetFieldValue("summary");
            }
            set {
                AddField("summary", value, FieldStore.Store, FieldIndex.Analyzed);
            }
        }

        protected Guid PageId {
            get {
                return new Guid(GetFieldValue("pageId"));
            }
            set {
                AddField("pageId", value.ToString(), FieldStore.Store, FieldIndex.DontIndex);
            }
        }

        protected DateTime Modified {
            get {
                return ConvertStringToDate(GetFieldValue("modified"));
            }
            set {
                AddField("modified", ConvertDateToString(value), FieldStore.Store, FieldIndex.DontIndex);
            }
        }

        // TODO: Replace dictionary with list containing store and index flags
        protected Dictionary<string, string> MetaData {
            set {
                foreach (var keyValuePair in value) {
                    AddField(keyValuePair.Key, keyValuePair.Value, FieldStore.Store, FieldIndex.Analyzed);
                }
            }
        }

        protected int LanguageId {
            get {
                return int.Parse(GetFieldValue("languageId"));
            }
            set {
                AddField("languageId", value.ToString(CultureInfo.InvariantCulture), FieldStore.Store, FieldIndex.DontIndex);
            }
        }

        protected DateTime Created {
            get {
                return ConvertStringToDate(GetFieldValue("created"));
            }
            set {
                AddField("created", ConvertDateToString(value), FieldStore.Store, FieldIndex.DontIndex);
            }
        }

        protected string Content {
            get {
                return GetFieldValue("content");
            }
            set {
                AddField("content", value, FieldStore.Store, FieldIndex.Analyzed);
            }
        }

        protected string Category {
            get {
                return GetFieldValue("category");
            }
            set {
                AddField("category", value, FieldStore.Store, FieldIndex.IndexOnly);
            }
        }
    }
}
