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

namespace KalikoCMS.Core {
    using System;

    public class RootPage : CmsPage {
        internal RootPage(int languageId) {
            PageId = Guid.Empty;
            LanguageId = languageId;
            PageName = "Root";
            //TODO: FIXA TILL NEDAN!! (så att virtual path kommer med)
            PageUrl = new Uri("/", UriKind.Relative);
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