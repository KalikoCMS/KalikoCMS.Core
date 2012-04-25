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
    public class SearchQuery {
        public SearchQuery(string searchString) {
            SearchString = searchString;
            ReturnFromPosition = 0;
            NumberOfHitsToReturn = 100;
            MetaData = new string[] {};
        }

        public int ReturnFromPosition { get; set; }
        public int NumberOfHitsToReturn { get; set; }
        public string SearchString { get; set; }
        public string[] MetaData { get; set; }
    }
}
