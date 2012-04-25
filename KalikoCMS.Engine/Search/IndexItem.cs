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

namespace KalikoCMS.Search {
    using System;
    using System.Collections.Generic;

    public class IndexItem {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        //public List<string> Tags { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
        public Guid PageId { get; set; }
        public int LanguageId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
        public DateTime? PublishStart { get; set; }
        public DateTime? PublishStop { get; set; }

        public IndexItem() {
            //Tags = new List<string>();
            MetaData = new Dictionary<string, string>();
        }

        public void EnsureCorrectValues() {
            Title = Title ?? string.Empty;
            Summary = Summary ?? string.Empty;
        }
    }
}
