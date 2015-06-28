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

            if (!string.IsNullOrEmpty(parameters)) {
                loadControl.Parameters = parameters;
            }

            EditControls.Controls.Add(loadControl);
            _controls.Add(loadControl);
        }

        private void LoadControls() {
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

        private void LoadFormForRootPage() {
            PageHeader.Text = "Root";
            PageName.Visible = false;
            StartPublishDate.Visible = false;
            StopPublishDate.Visible = false;
            VisibleInMenu.Visible = false;
            SaveButton.Visible = false;
            AdvancedOptionButton.Visible = false;
        }

        private void LoadFormForNewPage() {
            PageHeader.Text = "Create new page";
            SetStandardFieldLabels();

            var propertyDefinitions = PageType.GetPropertyDefinitions(_pageTypeId);

            foreach (var propertyDefinition in propertyDefinitions) {
                AddControl(propertyDefinition.Name, null, propertyDefinition.PropertyTypeId, propertyDefinition.Header, propertyDefinition.Parameters);
            }
        }

        private void SetStandardFieldLabels() {
            PageName.PropertyLabel = "Pagename";
            StartPublishDate.PropertyLabel = "Start publish";
            StopPublishDate.PropertyLabel = "Stop publish";
            VisibleInMenu.PropertyLabel = "Visible in menus";
            VisibleInSitemap.PropertyLabel = "Visible in sitemaps";
            SortOrder.PropertyLabel = "Sort order";
        }

        private void LoadFormForExistingPage() {
            CmsPage cmsPage;

            if (_hasVersionSpecified) {
                cmsPage = PageFactory.GetSpecificVersion(_pageId, _version);
            }
            else {
                cmsPage = PageFactory.GetWorkingCopy(_pageId);
            }

            _pageName = cmsPage.PageName;
            PageHeader.Text = _pageName;

            SetStandardFieldLabels(); 

            // Standard fields
            PageName.PropertyValue = new StringProperty(cmsPage.PageName);
            StartPublishDate.PropertyValue = new DateTimeProperty(cmsPage.StartPublish);
            StopPublishDate.PropertyValue = new DateTimeProperty(cmsPage.StopPublish);
            VisibleInMenu.PropertyValue = new BooleanProperty(cmsPage.VisibleInMenu);

            // Advanced fields
            VisibleInSitemap.PropertyValue = new BooleanProperty(cmsPage.VisibleInSiteMap);
            PageUrlSegment.Text = cmsPage.UrlSegment;
            SortOrder.PropertyValue = new NumericProperty(cmsPage.SortOrder);
            OldPageUrlSegment.Value = cmsPage.UrlSegment;

            PageId.Text = cmsPage.PageId.ToString();

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
                    page.Publish(true);
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
                    ShowMessage(Feedback, String.Format("Page <b>{0}</b> has been published!", _pageName));
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
                else if (!SortOrder.Validate()) {
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
            editablePage.SetStartPublish(((DateTimeProperty)StartPublishDate.PropertyValue).Value);
            editablePage.SetStopPublish(((DateTimeProperty)StopPublishDate.PropertyValue).Value);
            editablePage.SetVisibleInMenu(((BooleanProperty) VisibleInMenu.PropertyValue).Value);
            editablePage.SetVisibleInSiteMap(((BooleanProperty)VisibleInSitemap.PropertyValue).Value);
            editablePage.SetSortOrder(((NumericProperty)SortOrder.PropertyValue).Value);

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