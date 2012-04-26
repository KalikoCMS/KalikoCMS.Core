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

namespace KalikoCMS.Social.Comments {
    using System;
    using System.Collections.ObjectModel;
    using KalikoCMS.Caching;
    using KalikoCMS.Social.Data;

    public class Comment {

        public int CommentId { get; set; }
        public Guid PageId { get; set; }
        public int LanguageId { get; set; }
        public string UserName { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsReported { get; set; }
        public bool IsRemoved { get; set; }
        public string IP { get; set; }
        public bool IsValid { get; set; }
        public string Email { get; set; }

        public static ReadOnlyCollection<Comment> GetComments(Guid pageId, int languageId) {
            string cacheKey = GetCacheKey(pageId, languageId);

            ReadOnlyCollection<Comment> comments = CacheManager.Get<ReadOnlyCollection<Comment>>(cacheKey);

            if (comments == null) {
                comments = new ReadOnlyCollection<Comment>(CommentData.GetComments(pageId, languageId));
                CacheManager.Add(cacheKey, comments, CachePriority.Low, 15);
            }

            return comments;
        }

        private static string GetCacheKey(Guid pageId, int languageId) {
            return string.Format("Comments:{0}:{1}", pageId, languageId);
        }

        public static void AddComment(Guid pageId, int languageId, string userName, string commentText, string userHostAddress, string email) {
            Comment comment = new Comment {
                                              PageId = pageId,
                                              LanguageId = languageId,
                                              UserName = userName,
                                              CommentText = commentText,
                                              IP = userHostAddress,
                                              Email = email,
                                              CommentDate = DateTime.Now
                                          };

            CommentData.AddComment(comment);

            RemoveFromCache(pageId, languageId);
        }

        private static void RemoveFromCache(Guid pageId, int languageId) {
            string cacheKey = GetCacheKey(pageId, languageId);
            CacheManager.Remove(cacheKey);
        }
    }
}
