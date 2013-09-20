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

namespace KalikoCMS.Events {
    using System;
    using Core.Collections;

    public delegate void PropertyEventHandler(Object sender, PropertyEventArgs e);

    public class PropertyEventArgs : EventArgs {
        private readonly Guid _pageId;
        private readonly PropertyCollection _propertyCollection;

        public PropertyEventArgs(Guid pageId, PropertyCollection propertyCollection, string newValue, string oldValue) {
            _pageId = pageId;
            _propertyCollection = propertyCollection;
            NewValue = newValue;
            OldValue = oldValue;
        }

        public Guid PageId {
            get { return _pageId; }
        }

        public PropertyCollection PropertyCollection {
            get { return _propertyCollection; }
        }

        public string NewValue { get; set; }

        public string OldValue { get; set; }
    }
}