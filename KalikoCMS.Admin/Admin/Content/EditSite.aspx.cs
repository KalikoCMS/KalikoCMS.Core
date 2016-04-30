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
    using Core;
    using Extensions;
    using KalikoCMS.PropertyType;

    public partial class EditSite : AdminPage {
        private List<PropertyEditorBase> _controls;
        private Guid _siteId;
        private string _siteName;
        private bool _useTabs;
        private Dictionary<string, Panel> _tabs;

        protected void Page_Init(object sender, EventArgs e) {
            GetQueryStringValues();

            PublishButton.Click += PublishButtonEventHandler;

            _controls = new List<PropertyEditorBase>();

            MainForm.Action = Request.Url.PathAndQuery;

            LoadControls();

            Session["CmsAdminMode"] = "yes";
        }

        private void GetQueryStringValues() {
            Request.QueryString["id"].TryParseGuid(out _siteId);
        }

        private void LoadControls() {
            LoadChildSortOrderLists();
            LoadFormForSite();
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


        private void LoadFormForSite() {
            var cmsSite = SiteFactory.Get(_siteId);

            _siteName = cmsSite.Name;
            PageHeader.Text = _siteName;

            SetStandardFieldLabels();

            // Standard fields
            SiteName.PropertyValue = new StringProperty(cmsSite.Name);

            // Advanced fields
            ChildSortDirection.SelectedValue = ((int)cmsSite.ChildSortDirection).ToString();
            ChildSortOrder.SelectedValue = ((int)cmsSite.ChildSortOrder).ToString();

            var propertyDefinitions = CmsSite.PropertyDefinitions;

            AddTabs(propertyDefinitions);

            foreach (var propertyDefinition in propertyDefinitions) {
                var propertyName = propertyDefinition.Name;
                var propertyData = cmsSite.Property[propertyName];

                AddControl(propertyName, propertyData, propertyDefinition.PropertyTypeId, propertyDefinition.Header, propertyDefinition.Parameters, propertyDefinition.Required, propertyDefinition.TabGroup);
            }
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


        private void SetStandardFieldLabels() {
            SiteName.PropertyLabel = "Sitename";
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

        private void PublishButtonEventHandler(object sender, EventArgs e) {
            if (IsDataValid) {
                var site = SaveData();

                ScriptArea.Text = "top.refreshNode('" + _siteId + "');";

                Feedback.Text = string.Format("<script>parent.$(\'.notifications.top-right\').notify({{ type: \'info\', message: \"<i class=\\\"icon-flag\\\"></i> Changes to site '{0}' has been published!!\", fadeOut: {{ enabled: true, delay: 5000 }}}}).show();</script>", _siteName);
                Feedback.Visible = true;
            }
            else {
                ShowError(Feedback, "One or more errors occured!");
            }
        }

        private object SaveData() {
            var cmsSite = SiteFactory.Get(_siteId);
            var editableSite = cmsSite.MakeEditable();
            SavePropertiesForPage(editableSite);
            _siteName = editableSite.Name;

            return editableSite;
        }

        private void SavePropertiesForPage(EditableSite editableSite) {
            editableSite.Name = ((StringProperty)SiteName.PropertyValue).Value;
            editableSite.SetChildSortDirection(int.Parse(ChildSortDirection.SelectedValue));
            editableSite.SetChildSortOrder(int.Parse(ChildSortOrder.SelectedValue));

            foreach (var propertyControl in _controls) {
                var propertyName = propertyControl.PropertyName;
                var propertyValue = propertyControl.PropertyValue;

                editableSite.SetProperty(propertyName, propertyValue);
            }

            editableSite.SaveAndPublish();
        }

        protected bool IsDataValid {
            get {
                if (!SiteName.Validate(true)) {
                    return false;
                }

                return _controls.All(control => control.Validate(control.Required));
            }
        }
    }
}