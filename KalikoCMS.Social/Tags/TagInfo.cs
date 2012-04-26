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
    using System.Collections.ObjectModel;

    public class TagInfo {
        public string Tag { get; private set; }
        public int Count { get; private set; }
        public Collection<Guid> PageId { get; private set; }

        public TagInfo(string tag) {
            Tag = tag;
            Count = 0;
            PageId = new Collection<Guid>();
        }

        public override string ToString() {
            return Tag;
        }
    }
}
