#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz and Contributors
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */
#endregion

namespace KalikoCMS.Social.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KalikoCMS.Core;
    using KalikoCMS.Data;
    using KalikoCMS.Social.Comments;

    internal static class CommentData {
        internal static void AddComment(Comment comment) {
            DataManager.Insert(SocialDataManager.Instance.Comment, comment);
        }


        internal static Comment GetComment(int commentId) {
            return DataManager.GetById(SocialDataManager.Instance.Comment, commentId);
        }


        internal static List<Comment> GetComments(Guid pageId, int languageId) {
            return DataManager.Select(SocialDataManager.Instance.Comment, c => (c.PageId == pageId) && (c.LanguageId == languageId));
        }


        internal static void DeleteComment(int commentId) {
            DataManager.OpenConnection();

            try {
                IQueryable<Comment> comments = SocialDataManager.Instance.Comment.Where(c => c.CommentId == commentId);
                SocialDataManager.Instance.Comment.Delete(comments);
            }
            finally {
                DataManager.CloseConnection();
            }
        }
    }
}
