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
    using KalikoCMS.Core;
    using KalikoCMS.Data;
    using KalikoCMS.Social.Comments;

    public class ContentDatabase {
        private readonly IEntityProvider _provider;

        public ContentDatabase(IEntityProvider provider) {
            _provider = provider;
        }

        public ContentDatabase() {
            _provider = DataManager.Provider;
        }

        public IEntityProvider Provider {
            get { return _provider; }
        }

        #region Table definitions

        public virtual IEntityTable<Comment> Comment {
            get { return _provider.GetTable<Comment>("Comment"); }
        }

        #endregion
    }
}
