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

namespace KalikoCMS.Social.Data.EntityProvider {
    using IQToolkit;
    using IQToolkit.Data.Mapping;
    using KalikoCMS.Core;
    using KalikoCMS.Data;
    using KalikoCMS.Social.Comments;

    public class ContentDatabaseWithAttributes : ContentDatabase {

        public ContentDatabaseWithAttributes(IEntityProvider provider)
            : base(provider) {
        }

        public ContentDatabaseWithAttributes()
            : base(DataManager.Provider) {
        }

        [Table]
        [Column(Member = "CommentId", IsPrimaryKey = true, IsGenerated = true)]
        [Column(Member = "PageId")]
        [Column(Member = "LanguageId")]
        [Column(Member = "UserName")]
        [Column(Member = "CommentText")]
        [Column(Member = "CommentDate")]
        [Column(Member = "IsReported")]
        [Column(Member = "IsRemoved")]
        [Column(Member = "IP")]
        [Column(Member = "IsValid")]
        [Column(Member = "Email")]
        public override IEntityTable<Comment> Comment {
            get { return base.Comment; }
        }
    }
}