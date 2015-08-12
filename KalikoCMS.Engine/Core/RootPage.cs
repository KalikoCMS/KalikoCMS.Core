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

    public class RootPage : CmsPage {
        internal RootPage(int languageId) {
            PageId = Guid.Empty;
            LanguageId = languageId;
            PageName = "Root";
            PageUrl = new Uri(Utils.ApplicationPath, UriKind.Relative);
            PageTypeId = 0;

            FirstChild = 0;
            NextPage = -1;
            ParentId = Guid.Empty;
            RootId = Guid.Empty;
            TreeLevel = 0;

            StartPublish = DateTime.MinValue;
            StopPublish = DateTime.MaxValue;

            CreatedDate = DateTime.MinValue;
            UpdateDate = DateTime.MinValue;
        }

        public override EditablePage MakeEditable() {
            throw new Exception("Root page cannot be made editable!");
        }
    }
}