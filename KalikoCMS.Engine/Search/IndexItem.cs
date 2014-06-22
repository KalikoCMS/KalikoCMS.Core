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

namespace KalikoCMS.Search {
    using System;
    using System.Collections.Generic;

    public class IndexItem {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        public string Tags { get; set; }
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
            MetaData = new Dictionary<string, string>();
        }

        public void EnsureCorrectValues() {
            Title = Title ?? string.Empty;
            Summary = Summary ?? string.Empty;
        }
    }
}
