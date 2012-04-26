/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */

namespace KalikoCMS.Social.Tags {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using KalikoCMS.Core;
    using KalikoCMS.Core.Collections;

    public static class Tag {
        // Caches all tags for each root separately..
        private static readonly Hashtable Roots = new Hashtable();



        /// <summary>
        /// Clear cached tags so that they are re-read from the database on next access.
        /// </summary>
        internal static void ClearCache() {
            Roots.Clear();
        }

        /// <summary>
        /// Fetch all pages under a certain root page that has a specific tag set.
        /// </summary>
        /// <param name="rootPage">PageId of the root to fetch pages from</param>
        /// <param name="tag">Single tag to match with</param>
        /// <returns>A page source containing pages matching the parameters sent</returns>
        public static PageCollection GetPagesForTag(Guid rootPage, string tag) {
            PageCollection pageCollection = new PageCollection();
            foreach(TagInfo t in GetTagsForRoot(rootPage)) {
                if(t.Tag == tag) {
                    foreach(Guid p in t.PageId) {
                        pageCollection.Add(p);
                    }
                }
            }
            return pageCollection;
        }

        /// <summary>
        /// Returns a comma separated string with all tags for all pages below a certain root page.
        /// </summary>
        /// <param name="rootPage">The page root</param>
        /// <returns>Tags in a comma separated string</returns>
        public static string GetTagStringForRoot(Guid rootPage) {
            ReadOnlyCollection<TagInfo> tags = GetTagsForRoot(rootPage);

            // Convert tags to comma separated string and return..
            return string.Join(",", tags.Select(t => t.Tag).ToArray());
        }


        /// <summary>
        /// Returns a list of tag info with all tags for all pages below a certain root page.
        /// </summary>
        /// <param name="rootPage">The page root</param>
        /// <returns>List with tag info</returns>
        public static ReadOnlyCollection<TagInfo> GetTagsForRoot(Guid rootPage) {
            // Is the tags already in the table?
            string key = GetCacheName(rootPage);
/*            if(!Roots.ContainsKey(key)) {
                Roots.Add(key, TagData.GetTagsForRoot(rootpage));
            }*/

            return new ReadOnlyCollection<TagInfo>((List<TagInfo>)Roots[key]);
        }


        private static string GetCacheName(Guid pageId) {
            return "Page_" + pageId + "_" + Language.CurrentLanguage;
        }
        /// <summary>
        /// Returns a list of tags for a specific page.
        /// </summary>
        /// <param name="pageId">PageId to return tags for</param>
        /// <param name="rootPage">The page root</param>
        /// <returns>Tags in a string list</returns>
        public static ReadOnlyCollection<string> GetTagsForPage(Guid pageId, Guid rootPage) {
            ReadOnlyCollection<TagInfo> tags = GetTagsForRoot(rootPage);

            return new ReadOnlyCollection<string>((tags.Where(tag => tag.PageId.Contains(pageId)).Select(tag => tag.Tag)).ToList());
        }

        /// <summary>
        /// Returns a list of tags for a specific page.
        /// </summary>
        /// <param name="pageId">PageId to return tags for</param>
        /// <param name="rootPage">The page root</param>
        /// <returns>Tags in a comma separated string</returns>
        public static string GetTagStringForPage(Guid pageId, Guid rootPage) {
            return string.Join(",", GetTagsForPage(pageId, rootPage).ToArray());
        }

        /// <summary>
        /// Saves tags for a specific page.
        /// </summary>
        /// <param name="pageid">PageId to save tags for</param>
        /// <param name="rootpage">The page root (usually parent id)</param>
        /// <param name="tags">Comma separated string with tags to save</param>
        public static void SaveTagsForPage(Guid pageId, Guid rootPage, string tags) {
            //TODO:
            throw new NotImplementedException("TODO");

/*            string[] newtags = tags.Split(',');
            Hashtable all = TagData.GetAllTags();
            DataQuery dq = TagData.GetTagPageInstanceForUpdate(pageid);
            int piid = PageData.GetPageInstanceForPage(pageid);
            // Ta bort alla gamla kopplingar..
            dq.Clear();

            // Loopa igenom alla nya taggar och koppla mot rätt id..
            foreach(string t in newtags) {
                // Kolla ifall taggen redan finns, annars skapa..
                if(t.Trim() != string.Empty) {
                    int tagid;
                    if(all.Contains(t.Trim())) {
                        tagid = (int)all[t.Trim()];
                    }
                    else {
                        tagid = TagData.CreateNewTag(t.Trim());
                    }

                    DataRow dr = dq.GetNewRow();
                    dr["PageInstanceId"] = piid;
                    dr["TagId"] = tagid;
                    dq.AddRow(dr);
                }

            }

            dq.Save();

            // Ta bort cache:ade värdena..
            Roots.Remove(GetCacheName(rootpage));*/
        }
    }
}
