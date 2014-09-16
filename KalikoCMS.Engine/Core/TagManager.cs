#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace KalikoCMS.Core {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web.Management;
    using AutoMapper;
    using Collections;
    using Data;
    using Data.Entities;
    using Telerik.OpenAccess;

    public class TagManager : IStartupSequence {
        private static Dictionary<string, TagContext> _tagContexts;

        private static Dictionary<string, TagContext> TagContexts {
            get { return _tagContexts ?? (_tagContexts = GetTagContexts()); }
        }

        public static void ClearCache() {
            _tagContexts = null;
        }

        private static Dictionary<string, TagContext> GetTagContexts() {
            var contexts = new Dictionary<string, TagContext>();

            // TODO: Replace with single query if possible
            using (var context = new DataContext()) {
                var tagContexts = Mapper.Map<List<TagContextEntity>, List<TagContext>>(context.TagContexts.ToList());
                var tags = Mapper.Map<List<TagEntity>, List<Tag>>(context.Tags.ToList());
                var pageTags = (from t in context.Tags
                    from p in context.Pages
                    where t.Pages.Select(x => x.PageId).Contains(p.PageId)
                    select new PageTag() { PageId = p.PageId, TagId = t.TagId }).ToList();

                foreach (var tagContext in tagContexts) {
                    AddTagsToContext(tagContext, tags, pageTags);
                    contexts.Add(tagContext.ContextName.ToLowerInvariant(), tagContext);
                }
            }

            return contexts;
        }

        private static void AddTagsToContext(TagContext tagContext, List<Tag> tags, List<PageTag> pageTags) {
            tagContext.Tags = new Dictionary<string, Tag>();
            
            var tagsInContext = tags.Where(t => t.TagContextId == tagContext.TagContextId);

            foreach (var tag in tagsInContext) {
                var tagId = tag.TagId;
                var pages = pageTags.Where(p => p.TagId == tagId).Select(p => p.PageId);
                tag.Pages = new Collection<Guid>(pages.ToList());
                tagContext.Tags.Add(tag.TagName.ToLowerInvariant(), tag);
            }
        }

        public static void TagPage(Guid pageId, string contextName, IEnumerable<string> tags) {
            var context = GetTagContext(contextName);
            var tagContextId = context.TagContextId;
            tags = tags.Distinct();

            RemoveAllTagsForPage(pageId, context);

            foreach (var tagName in tags) {
                Tag tag;

                if (context.Tags.TryGetValue(tagName.ToLowerInvariant(), out tag) == false) { 
                    tag = new Tag {
                        TagContextId = tagContextId,
                        TagName = tagName
                    };
                    var tagEntity = Mapper.Map<Tag, TagEntity>(tag);
                    DataManager.InsertOrUpdate(tagEntity);
                    context.Tags.Add(tag.TagName.ToLowerInvariant(), tag);
                }

                tag.Pages.Add(pageId);
                DataManager.Insert(new PageTag { PageId = pageId, TagId = tag.TagId });
            }
        }

        public static PageCollection GetPagesForTag(string contextName, string tagName) {
            TagContext tagContext;
            if (TagContexts.TryGetValue(contextName.ToLowerInvariant(), out tagContext) == false) {
                return new PageCollection();
            }

            Tag tag;
            if (tagContext.Tags.TryGetValue(tagName.ToLowerInvariant(), out tag) == false) {
                return new PageCollection();
            }

            return new PageCollection(tag.Pages);
        }

        private static void RemoveAllTagsForPage(Guid pageId, TagContext context) {
            var tagContextId = context.TagContextId;
            var tags = DataManager.Select<TagEntity>(t => t.TagContextId == tagContextId);
            
            if (tags.Count == 0) {
                return;
            }

            DataManager.Delete<PageTagEntity>(p => p.PageId == pageId && tags.Exists(t => t.TagId == p.TagId));

            foreach (var tag in context.Tags) {
                tag.Value.Pages.Remove(pageId);
            }
        }

        private static void RemoveAllTagsForPage(Guid pageId) {
            DataManager.Delete<PageTagEntity>(p => p.PageId == pageId);

            foreach (var context in TagContexts.Values) {
                foreach (var tag in context.Tags) {
                    tag.Value.Pages.Remove(pageId);
                }
            }
        }

        private static TagContext GetTagContext(string contextName) {
            TagContext context;

            if (TagContexts.TryGetValue(contextName, out context)) {
                return context;
            }
            
            context = new TagContext {
                ContextName = contextName
            };
            
            DataManager.InsertOrUpdate(context);

            TagContexts.Add(context.ContextName.ToLowerInvariant(), context);

            return context;
        }

        public static TagContext GetTags(string contextName) {
            if (TagContexts.ContainsKey(contextName)) {
                return TagContexts[contextName];
            }
            
            return new TagContext { ContextName = contextName };
        }

        public void Startup() {
            PageFactory.PageDeleted += PageDeletedHandler;
        }

        void PageDeletedHandler(object sender, Events.PageEventArgs e) {
            RemoveAllTagsForPage(e.PageId);
        }
    }
}
