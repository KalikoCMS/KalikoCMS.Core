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

namespace KalikoCMS.PropertyType {
    using System.Web.UI;
    using KalikoCMS.Core;

    public abstract class PropertyEditorBase : UserControl {
        public string PropertyName { get; set; }
        
        public abstract string PropertyLabel { set; }
        
        public abstract PropertyData PropertyValue { get; set; }

        public abstract bool Validate();

        public abstract bool Validate(bool required);
    }
}