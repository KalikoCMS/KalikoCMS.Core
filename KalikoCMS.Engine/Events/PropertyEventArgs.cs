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