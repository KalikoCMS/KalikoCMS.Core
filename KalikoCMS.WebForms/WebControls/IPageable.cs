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

namespace KalikoCMS.WebForms.WebControls {
    using System.ComponentModel;

    public interface IPageable {
        [Bindable(true), Category("Data"), DefaultValue(null)]
        int PageSize { get; set; }

        [Bindable(true), Category("Data"), DefaultValue(null)]
        int PageIndex { get; set; }

        int PageCount { get; }
        bool PagerOnFirstPage { get; }
        bool PagerOnLastPage { get; }
        
        void Rebind();
    }
}