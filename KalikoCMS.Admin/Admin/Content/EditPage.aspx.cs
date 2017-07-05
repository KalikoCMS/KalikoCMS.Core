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

namespace KalikoCMS.Admin.Content {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Configuration;
    using Data;
    using Core;
    using Extensions;
    using KalikoCMS.PropertyType;

    public partial class EditPage : AdminPage {
        private List<PropertyEditorBase> _controls;
        private Guid _pageId;
        private string _pageName;
        private Guid _parentId;
        private int _pageTypeId;
        private bool _hasVersionSpecified;
        private int _version;
        private bool _useTabs;
        private Dictionary<string, Panel> _tabs;
        private List<string> _errors = new List<string>();

        protected void Page_Init(object sender, EventArgs e) {
            GetQueryStringValues();

            if (!IsPostBack) {
                RedirectIfSite();
            }

            SaveButton.Click += SaveButtonEventHandler;
            PublishButton.Click += PublishButtonEventHandler;

            _controls = new List<PropertyEditorBase>();

            MainForm.Action = Request.Url.PathAndQuery;

            LoadControls();

            Session["CmsAdminMode"] = "yes";
        }

        private void RedirectIfSite() {
            if (_pageTypeId == 0 && SiteFactory.IsSite(_pageId)) {
                Response.Redirect(string.Format("EditSite.aspx?id={0}", _pageId));
            }
        }

        private void GetQueryStringValues() {
            Request.QueryString["id"].TryParseGuid(out _pageId);
            Request.QueryString["parentId"].TryParseGuid(out _parentId);
            int.TryParse(Request.QueryString["pageTypeId"], out _pageTypeId);
            _hasVersionSpecified = int.TryParse(Request.QueryString["version"], out _version);
        }

        private void AddTabs(List<PropertyDefinition> propertyDefinitions) {
            var tabGroups = propertyDefinitions.Select(x => x.TabGroup).Distinct();

            if (tabGroups.Count() <= 1) {
                var header = new HtmlGenericControl("legend") {
                    InnerText = "Content"
                };
                EditControls.Controls.Add(header);
                _useTabs = false;
                return;
            }

            var tabs = new HtmlGenericControl("ul");
            tabs.Attributes.Add("class", "nav nav-tabs editor-tabs");
            tabs.Attributes.Add("role", "tablist");
            EditControls.Controls.Add(tabs);

            var tabContainer = new Panel {
                CssClass = "tab-content"
            };
            EditControls.Controls.Add(tabContainer);

            _tabs = new Dictionary<string, Panel>();
            var isFirst = true;
            var count = 0;

            foreach (var tabGroup in tabGroups) {
                var tab = new HtmlGenericControl("li") {
                    InnerHtml = string.Format("<a href=\"#tab{1}\" id=\"tab{1}-tab\" role=\"tab\" data-toggle=\"tab\">{0}</a>", tabGroup, count)
                };
                if (isFirst) {
                    tab.Attributes.Add("class", "active");
                }

                tab.Attributes.Add("role", "presentation");
                tabs.Controls.Add(tab);

                var tabTarget = new Panel {
                    CssClass = "tab-pane",
                    ClientIDMode = ClientIDMode.Static,
                    ID = string.Format("tab{0}", count)
                };
                if (isFirst) {
                    tabTarget.CssClass += " active";
                }
                tabTarget.Attributes.Add("role", "tabpanel");
                tabContainer.Controls.Add(tabTarget);
                _tabs.Add(tabGroup, tabTarget);

                isFirst = false;
                count++;
            }

            _useTabs = true;
        }

        private void AddControl(string propertyName, PropertyData propertyValue, Guid propertyTypeId, string headerText, string parameters, bool required, string tabGroup) {
            var propertyType = Core.PropertyType.GetPropertyType(propertyTypeId);
            var editControl = propertyType.EditControl;

            var loadControl = (PropertyEditorBase)LoadControl(editControl);
            loadControl.PropertyName = propertyName;
            loadControl.PropertyLabel = headerText;
            loadControl.Required = required;

            if (propertyValue != null) {
                loadControl.PropertyValue = propertyValue;
            }
            else if (propertyType.ClassInstance is CompositeProperty) {
                loadControl.PropertyValue = propertyType.CreateNewClassInstance();
            }

            if (!string.IsNullOrEmpty(parameters)) {
                loadControl.Parameters = parameters;
            }

            if (_useTabs) {
                var container = _tabs[tabGroup];
                container.Controls.Add(loadControl);
            }
            else {
                EditControls.Controls.Add(loadControl);
            }


            _controls.Add(loadControl);
        }

        private void LoadControls() {
            LoadChildSortOrderLists();

            if (_pageId != Guid.Empty) {
                LoadFormForExistingPage();
            }
            else if(_pageTypeId > 0) {
                LoadFormForNewPage();
            }
        }

        private void LoadChildSortOrderLists() {
            ChildSortOrder.Items.Add(new ListItem("Created date", ((int)Core.Collections.SortOrder.CreatedDate).ToString()));
            ChildSortOrder.Items.Add(new ListItem("Page name", ((int)Core.Collections.SortOrder.PageName).ToString()));
            ChildSortOrder.Items.Add(new ListItem("Sort index", ((int)Core.Collections.SortOrder.SortIndex).ToString()));
            ChildSortOrder.Items.Add(new ListItem("Start publish date", ((int)Core.Collections.SortOrder.StartPublishDate).ToString()));
            ChildSortOrder.Items.Add(new ListItem("Updated date", ((int)Core.Collections.SortOrder.UpdateDate).ToString()));

            ChildSortDirection.Items.Add(new ListItem("Ascending", ((int)Core.Collections.SortDirection.Ascending).ToString()));
            ChildSortDirection.Items.Add(new ListItem("Descending", ((int)Core.Collections.SortDirection.Descending).ToString()));
        }

        private void LoadFormForNewPage() {
            PageHeader.Text = "Create new page";
            SetStandardFieldLabels();

            var propertyDefinitions = PageType.GetPropertyDefinitions(_pageTypeId);
            var pageType = PageType.GetPageType(_pageTypeId);
            var parent = PageFactory.GetPage(_parentId);
            var editablePage = parent.CreateChildPage(_pageTypeId);


            AddTabs(propertyDefinitions);
            foreach (var propertyDefinition in propertyDefinitions) {
                var propertyValue = editablePage.Property[propertyDefinition.Name];
                AddControl(propertyDefinition.Name, propertyValue, propertyDefinition.PropertyTypeId, propertyDefinition.Header, propertyDefinition.Parameters, propertyDefinition.Required, propertyDefinition.TabGroup);
            }

            PageTypeName.Text = PageType.GetPageType(_pageTypeId).DisplayName;

            ChildSortDirection.SelectedValue = ((int)pageType.DefaultChildSortDirection).ToString();
            ChildSortOrder.SelectedValue = ((int)pageType.DefaultChildSortOrder).ToString();
        }
        
        private void SetStandardFieldLabels() {
            PageName.PropertyLabel = "Pagename";
            StartPublishDate.PropertyLabel = "Start publish";
            StopPublishDate.PropertyLabel = "Stop publish";
            VisibleInMenu.PropertyLabel = "Visible in menus";
            VisibleInSitemap.PropertyLabel = "Visible in sitemaps";
        }

        private void LoadFormForExistingPage() {
            CmsPage cmsPage;

            if (_hasVersionSpecified) {
                cmsPage = PageFactory.GetSpecificVersion(_pageId, _version);
            }
            else {
                cmsPage = PageFactory.GetWorkingCopy(_pageId);
            }

            if (!IsPostBack) {
                if (cmsPage.OriginalStatus == PageInstanceStatus.WorkingCopy)
                {
                    ShowMessage(Feedback, "This version has not yet been published. " + RenderPreviewButton(cmsPage), "warning");
                }
                else if (cmsPage.OriginalStatus == PageInstanceStatus.Archived)
                {
                    ShowMessage(Feedback, "This version has previously been published. " + RenderPreviewButton(cmsPage), "warning");
                }
            }

            _pageName = cmsPage.PageName;
            PageHeader.Text = _pageName;

            SetStandardFieldLabels(); 

            // Standard fields
            PageName.PropertyValue = new StringProperty(cmsPage.PageName);
            StartPublishDate.PropertyValue = new UniversalDateTimeProperty(cmsPage.StartPublish);
            StopPublishDate.PropertyValue = new UniversalDateTimeProperty(cmsPage.StopPublish);
            VisibleInMenu.PropertyValue = new BooleanProperty(cmsPage.VisibleInMenu);

            // Advanced fields
            VisibleInSitemap.PropertyValue = new BooleanProperty(cmsPage.VisibleInSiteMap);
            PageUrlSegment.Text = cmsPage.UrlSegment;
            OldPageUrlSegment.Value = cmsPage.UrlSegment;
            ChildSortDirection.SelectedValue = ((int)cmsPage.ChildSortDirection).ToString();
            ChildSortOrder.SelectedValue = ((int)cmsPage.ChildSortOrder).ToString();

            PageId.Text = cmsPage.PageId.ToString();

            PageTypeName.Text = PageType.GetPageType(cmsPage.PageTypeId).DisplayName;

            var basePage = PageFactory.GetPage(_pageId);
            if (basePage != null) {
                ShortUrl.Text = string.Format("<a href=\"{0}{1}\" target=\"_blank\">{0}{1}</a>", Utils.ApplicationPath, basePage.ShortUrl);
                PublishedUrl.Text = "<a href=\"" + basePage.PageUrl + "\" target=\"_blank\">" + basePage.PageUrl + "</a>";
            }

            var propertyDefinitions = PageType.GetPropertyDefinitions(cmsPage.PageTypeId);

            AddTabs(propertyDefinitions);

            foreach (var propertyDefinition in propertyDefinitions) {
                var propertyName = propertyDefinition.Name;
                var propertyData = cmsPage.Property[propertyName];

                AddControl(propertyName, propertyData, propertyDefinition.PropertyTypeId, propertyDefinition.Header, propertyDefinition.Parameters, propertyDefinition.Required, propertyDefinition.TabGroup);
            }
        }

        private void SaveButtonEventHandler(object sender, EventArgs e) {
            if(IsDataValid) {
                var page = SaveData();

                if (_pageTypeId > 0) {
                    Feedback.Text = string.Format("<script>parent.$('.notifications.top-right').notify({{ type: 'info', message: \"<i class=\\\"icon-flag\\\"></i> Page <b>{0}</b> has been created!!\", fadeOut: {{ enabled: true, delay: 5000 }}}}).show();parent.refreshTreeNode('{1}','{2}');document.location = '{3}?id={2}';</script>", _pageName.Replace("\"", "\\\""), _parentId, _pageId, Request.Path);
                    Feedback.Visible = true;
                }
                else {
                    ShowMessage(Feedback, String.Format("A working copy of <b>{0}</b> has been saved! Remember to publish this version once it's complete. {1}", _pageName, RenderPreviewButton(page)));
                }
            }
            else {
                ShowError(Feedback, "One or more errors occured!<br/>" + string.Join("<br/>", _errors));
            }
        }

        private void PublishButtonEventHandler(object sender, EventArgs e) {
            if(IsDataValid) {
                var page = SaveData();

                page.Publish();

                ScriptArea.Text = "top.refreshNode('" + CurrentPageId + "');";

                if (_pageTypeId > 0) {
                    Feedback.Text = string.Format("<script>parent.$('.notifications.top-right').notify({{ type: 'info', message: \"<i class=\\\"icon-flag\\\"></i> Page <b>{0}</b> has been created and published!!\", fadeOut: {{ enabled: true, delay: 5000 }}}}).show();parent.refreshTreeNode('{1}','{2}');document.location = '{3}?id={2}';</script>", _pageName.Replace("\"", "\\\""), _parentId, _pageId, Request.Path);
                    Feedback.Visible = true;
                }
                else {
                    Feedback.Text = string.Format("<script>parent.$('.notifications.top-right').notify({{ type: 'info', message: \"<i class=\\\"icon-flag\\\"></i> Page <b>{0}</b> has been published!!\", fadeOut: {{ enabled: true, delay: 5000 }}}}).show();</script>", _pageName.Replace("\"", "\\\""));
                    Feedback.Visible = true;
                }
            }
            else {
                ShowError(Feedback, "One or more errors occured!<br/>" + string.Join("<br/>", _errors));
            }
        }

        private string RenderPreviewButton(CmsPage page) {
            return string.Format("<a href=\"{2}?id={0}&version={1} \" target=\"_blank\">Preview</a>", page.PageId, page.CurrentVersion, SiteSettings.Instance.PreviewPath);
        }

        protected bool IsDataValid {
            get {
                if (!PageName.Validate(true)) {
                    _errors.Add("Page name is not valid");
                    return false;
                }
                if (!IsPublishDatesValid) {
                    return false;
                }

                return _controls.All(control => control.Validate(control.Required));
            }
        }

        public bool IsPublishDatesValid {
            get {
                if (!StartPublishDate.Validate()) {
                    _errors.Add("Start publish date is not valid");
                    return false;
                }
                if (!StopPublishDate.Validate()) {
                    _errors.Add("Stop publish date is not valid");
                    return false;
                }

                var startPublishDate = ((UniversalDateTimeProperty)StartPublishDate.PropertyValue).Value;
                var stopPublishDate = ((UniversalDateTimeProperty)StopPublishDate.PropertyValue).Value;

                if (startPublishDate != null && stopPublishDate != null && startPublishDate > stopPublishDate) {
                    _errors.Add("Start publish date is later than stop publish date");
                    return false;
                }

                return true;
            }
        }

        private EditablePage SaveData() {
            if(_pageId == Guid.Empty) {
                return SaveDataForNewPage();
            }
            else {
                return SaveDataForExistingPage();
            }
        }

        private EditablePage SaveDataForNewPage() {
            var parent = PageFactory.GetPage(_parentId);
            var editablePage = parent.CreateChildPage(_pageTypeId);
            SavePropertiesForPage(editablePage);
            _pageId = editablePage.PageId;
            _pageName = editablePage.PageName;
            OldPageUrlSegment.Value = editablePage.UrlSegment;

            return editablePage;
        }

        private EditablePage SaveDataForExistingPage() {
            var cmsPage = PageFactory.GetWorkingCopy(_pageId);
            _parentId = cmsPage.ParentId;
            var editablePage = cmsPage.MakeEditable();
            SavePropertiesForPage(editablePage);
            OldPageUrlSegment.Value = editablePage.UrlSegment;
            _pageName = editablePage.PageName;

            return editablePage;
        }

        private void SavePropertiesForPage(EditablePage editablePage) {
            editablePage.PageName = ((StringProperty)PageName.PropertyValue).Value;
            editablePage.SetStartPublish(((UniversalDateTimeProperty)StartPublishDate.PropertyValue).Value);
            editablePage.SetStopPublish(((UniversalDateTimeProperty)StopPublishDate.PropertyValue).Value);
            editablePage.SetVisibleInMenu(((BooleanProperty) VisibleInMenu.PropertyValue).Value);
            editablePage.SetVisibleInSiteMap(((BooleanProperty)VisibleInSitemap.PropertyValue).Value);
            editablePage.SetChildSortDirection(int.Parse(ChildSortDirection.SelectedValue));
            editablePage.SetChildSortOrder(int.Parse(ChildSortOrder.SelectedValue)); 

            HandlePageUrlSegment(editablePage);

            foreach (var propertyControl in _controls) {
                var propertyName = propertyControl.PropertyName;
                var propertyValue = propertyControl.PropertyValue;

                editablePage.SetProperty(propertyName, propertyValue);
            }

            editablePage.Save();
        }

        public string CurrentPageId { get { return _pageId.ToString(); } }

        private void HandlePageUrlSegment(EditablePage editablePage) {
            var oldSegment = OldPageUrlSegment.Value;
            var newSegment = PageUrlSegment.Text;

            if (newSegment == oldSegment) {
                return;
            }

            editablePage.SetPageUrl(newSegment);
        }
    }
}