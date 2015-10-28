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
    using System.Web.UI.WebControls;
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

        protected void Page_Init(object sender, EventArgs e) {
            GetQueryStringValues();

            SaveButton.Click += SaveButtonEventHandler;
            PublishButton.Click += PublishButtonEventHandler;

            _controls = new List<PropertyEditorBase>();

            MainForm.Action = Request.Url.PathAndQuery;

            LoadControls();
        }

        private void GetQueryStringValues() {
            Request.QueryString["id"].TryParseGuid(out _pageId);
            Request.QueryString["parentId"].TryParseGuid(out _parentId);
            int.TryParse(Request.QueryString["pageTypeId"], out _pageTypeId);
            _hasVersionSpecified = int.TryParse(Request.QueryString["version"], out _version);

        }

        private void AddControl(string propertyName, PropertyData propertyValue, Guid propertyTypeId, string headerText, string parameters) {
            var propertyType = Core.PropertyType.GetPropertyType(propertyTypeId);
            var editControl = propertyType.EditControl;

            var loadControl = (PropertyEditorBase)LoadControl(editControl);
            loadControl.PropertyName = propertyName;
            loadControl.PropertyLabel = headerText;
            
            if (propertyValue != null) {
                loadControl.PropertyValue = propertyValue;
            }
            else if (propertyType.ClassInstance is CompositeProperty) {
                loadControl.PropertyValue = propertyType.CreateNewClassInstance();
            }

            if (!string.IsNullOrEmpty(parameters)) {
                loadControl.Parameters = parameters;
            }

            EditControls.Controls.Add(loadControl);
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
            else {
                LoadFormForRootPage();
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

        private void LoadFormForRootPage() {
            PageHeader.Text = "Root";
            PageTypeName.Text = "Root";
            PageId.Text = Guid.Empty.ToString();
            PageName.Visible = false;
            StartPublishDate.Visible = false;
            StopPublishDate.Visible = false;
            VisibleInMenu.Visible = false;
            SaveButton.Visible = false;
            PublishButton.Visible = false;
            AdvancedOptionButton.Visible = false;
        }

        private void LoadFormForNewPage() {
            PageHeader.Text = "Create new page";
            SetStandardFieldLabels();

            var propertyDefinitions = PageType.GetPropertyDefinitions(_pageTypeId);
            foreach (var propertyDefinition in propertyDefinitions) {
                AddControl(propertyDefinition.Name, null, propertyDefinition.PropertyTypeId, propertyDefinition.Header, propertyDefinition.Parameters);
            }

            PageTypeName.Text = PageType.GetPageType(_pageTypeId).DisplayName;

            var pageType = PageType.GetPageType(_pageTypeId);
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
                    ShowMessage(Feedback, "This version has not yet been published", "warning");
                }
                else if (cmsPage.OriginalStatus == PageInstanceStatus.Archived)
                {
                    ShowMessage(Feedback, "This version has previously been published", "warning");
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

            var propertyDefinitions = PageType.GetPropertyDefinitions(cmsPage.PageTypeId);

            foreach (var propertyDefinition in propertyDefinitions) {
                var propertyName = propertyDefinition.Name;
                var propertyData = cmsPage.Property[propertyName];

                AddControl(propertyName, propertyData, propertyDefinition.PropertyTypeId, propertyDefinition.Header, propertyDefinition.Parameters);
            }
        }

        private void SaveButtonEventHandler(object sender, EventArgs e) {
            if(IsDataValid) {
                var page = SaveData();

                if (_pageTypeId > 0) {
                    Feedback.Text = string.Format("<script>parent.$('.notifications.top-right').notify({{ type: 'info', message: \"<i class=\\\"icon-flag\\\"></i> Page <b>{0}</b> has been created!!\", fadeOut: {{ enabled: true, delay: 5000 }}}}).show();parent.refreshTreeNode('{1}','{2}');document.location = '{3}?id={2}';</script>", _pageName, _parentId, _pageId, Request.Path);
                    Feedback.Visible = true;
                }
                else {
                    ShowMessage(Feedback, String.Format("A working copy of <b>{0}</b> has been saved! Remember to publish this version once it's complete.", _pageName));
                }
            }
            else {
                ShowError(Feedback, "One or more errors occured!");
            }
        }

        private void PublishButtonEventHandler(object sender, EventArgs e) {
            if(IsDataValid) {
                var page = SaveData();

                page.Publish();

                ScriptArea.Text = "top.refreshNode('" + CurrentPageId + "');";

                if (_pageTypeId > 0) {
                    Feedback.Text = string.Format("<script>parent.$('.notifications.top-right').notify({{ type: 'info', message: \"<i class=\\\"icon-flag\\\"></i> Page <b>{0}</b> has been created and published!!\", fadeOut: {{ enabled: true, delay: 5000 }}}}).show();parent.refreshTreeNode('{1}','{2}');document.location = '{3}?id={2}';</script>", _pageName, _parentId, _pageId, Request.Path);
                    Feedback.Visible = true;
                }
                else {
                    Feedback.Text = string.Format("<script>parent.$('.notifications.top-right').notify({{ type: 'info', message: \"<i class=\\\"icon-flag\\\"></i> Page <b>{0}</b> has been published!!\", fadeOut: {{ enabled: true, delay: 5000 }}}}).show();</script>", _pageName);
                    Feedback.Visible = true;
                }
            }
            else {
                ShowError(Feedback, "One or more errors occured!");
            }
        }

        protected bool IsDataValid {
            get {
                var valid = true;

                if (!PageName.Validate(true)) {
                    valid = false;
                }
                else if (!StartPublishDate.Validate()) {
                    valid = false;
                }
                else if (!StopPublishDate.Validate()) {
                    valid = false;
                }

                return valid;
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
            OldPageUrlSegment.Value = editablePage.UrlSegment;

            return editablePage;
        }

        private EditablePage SaveDataForExistingPage() {
            var cmsPage = PageFactory.GetWorkingCopy(_pageId);
            _parentId = cmsPage.ParentId;
            var editablePage = cmsPage.MakeEditable();
            SavePropertiesForPage(editablePage);
            OldPageUrlSegment.Value = editablePage.UrlSegment;

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