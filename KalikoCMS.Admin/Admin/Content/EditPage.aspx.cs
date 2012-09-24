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
            Request.QueryString["id"].TryParseGuid(out _pageId);
            Request.QueryString["parentId"].TryParseGuid(out _parentId);
            int.TryParse(Request.QueryString["pageTypeId"], out _pageTypeId);

            SaveButton.Click += SaveButtonEventHandler;

            _controls = new List<PropertyEditorBase>();

            LoadControls();
        }

        private void AddControl(string propertyName, PropertyData propertyValue, Guid propertyTypeId, string headerText) {
            Core.PropertyType propertyType = Core.PropertyType.GetPropertyType(propertyTypeId);
            string editControl = propertyType.EditControl;

            var loadControl = (PropertyEditorBase)LoadControl(editControl);
            loadControl.PropertyName = propertyName;
            loadControl.PropertyLabel = headerText;
            
            if (propertyValue != null) {
                loadControl.PropertyValue = propertyValue;
            }

            EditControls.Controls.Add((Control)loadControl);
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
            SaveButton.Visible = false;
        }

        private void LoadFormForNewPage() {
            PageHeader.Text = "Create new page";
            PageName.PropertyLabel = "Pagename";
            StartPublishDate.PropertyLabel = "Start publish";
            StopPublishDate.PropertyLabel = "Stop publish";

            List<PropertyEntity> propertyDefinitions = PageFactory.GetPropertyDefinitionsForPagetype(_pageTypeId);

            foreach (PropertyEntity propertyDefinition in propertyDefinitions) {
                string propertyName = propertyDefinition.Name;
                
                AddControl(propertyName, null, propertyDefinition.PropertyTypeId, propertyDefinition.Header);
            }
        }

        private void LoadFormForExistingPage() {
            CmsPage cmsPage = PageFactory.GetPage(_pageId);

            _pageName = cmsPage.PageName;
            PageHeader.Text = _pageName;

            PageName.PropertyLabel = "Pagename";
            PageName.PropertyValue = new StringProperty(cmsPage.PageName);

            StartPublishDate.PropertyLabel = "Start publish";
            StartPublishDate.PropertyValue = new DateTimeProperty(cmsPage.StartPublish);

            StopPublishDate.PropertyLabel = "Stop publish";
            StopPublishDate.PropertyValue = new DateTimeProperty(cmsPage.StopPublish);

            List<PropertyEntity> propertyDefinitions = PageFactory.GetPropertyDefinitionsForPagetype(cmsPage.PageTypeId);

            foreach (PropertyEntity propertyDefinition in propertyDefinitions) {
                string propertyName = propertyDefinition.Name;
                PropertyData propertyData = cmsPage.Property[propertyName];

                AddControl(propertyName, propertyData, propertyDefinition.PropertyTypeId, propertyDefinition.Header);
            }
        }

        private void SaveButtonEventHandler(object sender, EventArgs e) {
            
            if(IsDataValid) {
                SaveData();
                MessageBox.Text = "<script>parent.$.jGrowl(\"Page <b>" + _pageName
                                  + "</b> saved!!\", { themeState: '' });parent.refreshTreeNode('" + _parentId
                                  + "');</script>";
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

            foreach (PropertyEditorBase propertyControl in _controls) {
                string propertyName = propertyControl.PropertyName;
                PropertyData propertyValue = propertyControl.PropertyValue;

                editablePage.SetProperty(propertyName, propertyValue);
            }

            editablePage.Save();
        }
    }
}