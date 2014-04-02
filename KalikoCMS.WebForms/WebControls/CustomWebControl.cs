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

namespace KalikoCMS.WebForms.WebControls {
    using System.ComponentModel;
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.WebForms.Framework;
    using AttributeCollection = System.Web.UI.AttributeCollection;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class CustomWebControl : Control, INamingContainer {

        private AttributeCollection _attributeCollection;
        private StateBag _stateBag;
        private CmsPage _currentPage;


        protected CmsPage CurrentPage {
            get {
                return _currentPage ?? (_currentPage = ((PageTemplate)Page).CurrentPage);
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AttributeCollection Attributes {
            get {
                if(_attributeCollection == null) {
                    if(_stateBag == null) {
                        _stateBag = new StateBag(true);
                        if(IsTrackingViewState) {
                            ((IStateManager)_stateBag).TrackViewState();
                        }
                    }
                    _attributeCollection = new AttributeCollection(_stateBag);
                }
                return _attributeCollection;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool HasAttributes {
            get {
                return (_attributeCollection != null && _attributeCollection.Count > 0) || (_stateBag != null && _stateBag.Count > 0);
            }
        }


        public string GetAttribute(string key) {
            if(_stateBag == null) {
                return null;
            }
            return (string)_stateBag[key];
        }


        public void SetAttribute(string key, string value) {
            Attributes[key] = value;
        }
    }
}