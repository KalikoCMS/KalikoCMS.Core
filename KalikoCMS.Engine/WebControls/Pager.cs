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
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class Pager : AutoBindableBase {
        private int _activeIndex;
        private IPageable _targetControl;

        #region Public Properties

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public IPageable TargetControl {
            get { return _targetControl; }
            set {
                _targetControl = value;
                if (_activeIndex > 0) {
                    _targetControl.PageIndex = _activeIndex;
                }
            }
        }

        [Bindable(true), Category("Data"), DefaultValue(true)]
        public bool RenderAsList { get; set; }

        [Bindable(true), Category("Data"), DefaultValue(true)]
        public string SelectedPageClass { get; set; }
        
        [Bindable(true), Category("Data"), DefaultValue(true)]
        public string DisabledLinkClass { get; set; }

        [Bindable(true), Category("Data"), DefaultValue(true)]
        public string PreviousLinkText { get; set; }

        [Bindable(true), Category("Data"), DefaultValue(true)]
        public string NextLinkText { get; set; }

        #endregion

        public Pager() {
            RenderAsList = true;
            SelectedPageClass = "active";
            DisabledLinkClass = "disabled";
            NextLinkText = "&raquo;";
            PreviousLinkText = "&laquo;";
        }

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);

            int.TryParse(HttpContext.Current.Request.QueryString["p"], out _activeIndex);
        }

        public override void DataBind() {
            base.DataBind();

            EnsureChildControls();
            CreateControlHierarchy();
            ChildControlsCreated = true;
        }

        private void CreateControlHierarchy() {
            if (TargetControl == null) {
                Utils.Throw<ArgumentException>("TargetControl not found!");
            }

            if (_activeIndex == 0) {
                _activeIndex = TargetControl.PageIndex;
            }
            if (_activeIndex == 0) {
                _activeIndex = 1;
            }

            var pageCount = TargetControl.PageCount;
            var linkUrl = LinkUrlTemplate;
            Control container;

            if (RenderAsList) {
                var listContainer = new HtmlGenericControl("ul");
                listContainer.Attributes["class"] = "pagination";
                container = listContainer;
                Controls.Add(container);
            }
            else {
                container = this;
            }

            container.Controls.Add(CreatePreviousLink(linkUrl));

            for (int i = 1; i <= pageCount; i++) {
                container.Controls.Add(CreateLink(i, i.ToString(CultureInfo.InvariantCulture), linkUrl));
            }

            container.Controls.Add(CreateNextLink(linkUrl, pageCount));
        }

        private Control CreatePreviousLink(string linkUrl) {
            if (_activeIndex > 1) {
                return CreateLink(_activeIndex - 1, PreviousLinkText, linkUrl, "");
            }
            else {
                return CreateLink(1, PreviousLinkText, "javascript:", DisabledLinkClass);
            }
        }

        private Control CreateNextLink(string linkUrl, int pageCount) {
            if (_activeIndex < pageCount) {
                return CreateLink(_activeIndex + 1, NextLinkText, linkUrl, "");
            }
            else {
                return CreateLink(1, NextLinkText, "javascript:", DisabledLinkClass);
            }
        }

        private Control CreateLink(int pageIndex, string pageTitle, string linkUrl, string cssOverride = null) {
            HtmlControl control;
            var link = new HtmlAnchor { 
                InnerHtml = pageTitle,
                HRef = linkUrl.Replace("__page__", pageIndex.ToString(CultureInfo.InvariantCulture))
            };

            if (RenderAsList) {
                control = new HtmlGenericControl("li");
                control.Controls.Add(link);
            }
            else {
                control = link;
            }

            if (cssOverride != null) {
                control.Attributes["class"] = cssOverride;
            }
            else if (_activeIndex == pageIndex) {
                control.Attributes["class"] = SelectedPageClass;
            }

            return control;
        }

        private static string LinkUrlTemplate {
            get {
                var url = HttpContext.Current.Request.Url;

                var queryStrings = HttpUtility.ParseQueryString(url.Query);
                queryStrings.Set("p", "__page__");
                queryStrings.Remove("id");

                return string.Format("{0}?{1}", url.AbsolutePath, queryStrings);
            }
        }
    }
}
