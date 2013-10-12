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

namespace KalikoCMS.WebControls {
    using System.ComponentModel;
    using System.Web.UI;
    using Core;
    using Framework;
    using AttributeCollection = System.Web.UI.AttributeCollection;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class CustomWebControl : Control, INamingContainer {

        private AttributeCollection _attributeCollection;
        private StateBag _stateBag;
        private CmsPage _currentPage;


        protected CmsPage CurrentPage {
            get { return _currentPage ?? (_currentPage = ((PageTemplate)Page).CurrentPage); }
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