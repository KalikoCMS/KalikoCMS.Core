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

namespace KalikoCMS.Admin.Content {
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.Data.EntityProvider;
    using KalikoCMS.Extensions;
    using KalikoCMS.PropertyType;

    public partial class EditPage : Page {
        private List<PropertyEditorBase> _controls;
        private Guid _pageId;
        private string _pageName;
        private Guid _parentId;
        private int _pageTypeId;

        protected void Page_Init(object sender, EventArgs e) {
            GetQueryStringValues();

            SaveButton.Click += SaveButtonEventHandler;

            _controls = new List<PropertyEditorBase>();

            MainForm.Action = Request.Url.PathAndQuery;

            LoadControls();
        }

        private void GetQueryStringValues() {
            Request.QueryString["id"].TryParseGuid(out _pageId);
            Request.QueryString["parentId"].TryParseGuid(out _parentId);
            int.TryParse(Request.QueryString["pageTypeId"], out _pageTypeId);
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
        }

        private void LoadFormForNewPage() {
            PageHeader.Text = "Create new page";
            SetStandardFieldLabels();

            List<PropertyEntity> propertyDefinitions = PageType.GetPropertyDefinitions(_pageTypeId);

            foreach (PropertyEntity propertyDefinition in propertyDefinitions) {
                AddControl(propertyDefinition.Name, null, propertyDefinition.PropertyTypeId, propertyDefinition.Header, propertyDefinition.Parameters);
            }
        }

        private void SetStandardFieldLabels() {
            PageName.PropertyLabel = "Pagename";
            StartPublishDate.PropertyLabel = "Start publish";
            StopPublishDate.PropertyLabel = "Stop publish";
            VisibleInMenu.PropertyLabel = "Visible in menus";
        }

        private void LoadFormForExistingPage() {
            CmsPage cmsPage = PageFactory.GetPage(_pageId);

            _pageName = cmsPage.PageName;
            PageHeader.Text = _pageName;

            SetStandardFieldLabels(); 

            PageName.PropertyValue = new StringProperty(cmsPage.PageName);
            StartPublishDate.PropertyValue = new DateTimeProperty(cmsPage.StartPublish);
            StopPublishDate.PropertyValue = new DateTimeProperty(cmsPage.StopPublish);
            VisibleInMenu.PropertyValue = new BooleanProperty(cmsPage.VisibleInMenu);

            List<PropertyEntity> propertyDefinitions = PageType.GetPropertyDefinitions(cmsPage.PageTypeId);

            foreach (PropertyEntity propertyDefinition in propertyDefinitions) {
                string propertyName = propertyDefinition.Name;
                PropertyData propertyData = cmsPage.Property[propertyName];

                AddControl(propertyName, propertyData, propertyDefinition.PropertyTypeId, propertyDefinition.Header, propertyDefinition.Parameters);
            }
        }

        private void SaveButtonEventHandler(object sender, EventArgs e) {
            if(IsDataValid) {
                SaveData();
                MessageBox.Text = string.Format("<script>parent.$('.notifications.top-right').notify({{ type: 'info', message: \"<i class=\\\"icon-flag\\\"></i> Page <b>{0}</b> saved!!\", fadeOut: {{ enabled: true, delay: 5000 }}}}).show();parent.refreshTreeNode('{1}');</script>", _pageName, _parentId);
                MessageBox.Visible = true;
            }
            else {
                ErrorMessage.Text = "One or more errors occured!";
            }
        }

        protected bool IsDataValid {
            get {
                bool valid = true;

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

        private void SaveData() {
            if(_pageId == Guid.Empty) {
                SaveDataForNewPage();
            }
            else {
                SaveDataForExistingPage();
            }
        }

        private void SaveDataForNewPage() {
            CmsPage parent = PageFactory.GetPage(_parentId);
            EditablePage editablePage = parent.CreateChildPage(_pageTypeId);
            SavePropertiesForPage(editablePage);
        }

        private void SaveDataForExistingPage() {
            CmsPage cmsPage = PageFactory.GetPage(_pageId);
            _parentId = cmsPage.ParentId;

            EditablePage editablePage = cmsPage.MakeEditable();

            SavePropertiesForPage(editablePage);
        }

        private void SavePropertiesForPage(EditablePage editablePage) {
            editablePage.PageName = ((StringProperty)PageName.PropertyValue).Value;
            editablePage.SetStartPublish(((DateTimeProperty)StartPublishDate.PropertyValue).Value);
            editablePage.SetStopPublish(((DateTimeProperty)StopPublishDate.PropertyValue).Value);
            editablePage.SetVisibleInMenu(((BooleanProperty) VisibleInMenu.PropertyValue).Value);

            foreach (PropertyEditorBase propertyControl in _controls) {
                string propertyName = propertyControl.PropertyName;
                PropertyData propertyValue = propertyControl.PropertyValue;

                editablePage.SetProperty(propertyName, propertyValue);
            }

            editablePage.Save();
        }
    }
}