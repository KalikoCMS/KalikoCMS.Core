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

namespace KalikoCMS.Admin.Content.PropertyType {
    using System;
    using System.Linq;
    using System.Web.UI;
    using Configuration;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class TagPropertyEditor : PropertyEditorBase {
        private TagPropertyAttribute.TagPropertyAttributeValues _attributeValues;

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            set {
                var tagProperty = (TagProperty)value;
                ValueField.Text = string.Join(", ", tagProperty.Tags);
            }
            get {
                var tags = ValueField.Text.Split(new[] {','});
                var tagList = tags.Select(t => t.Trim()).Where(t => t.Length > 0).ToList();
                var tagContext = TagContext.Value;
                
                return new TagProperty(tagContext, tagList);
            }
        }

        public override string Parameters {
            set {
                _attributeValues = Serialization.JsonSerialization.DeserializeJson<TagPropertyAttribute.TagPropertyAttributeValues>(value);
            }
        }

        public override bool Validate() {
            return true;
        }

        public override bool Validate(bool required) {
            if (required && string.IsNullOrEmpty(ValueField.Text)) {
                ErrorText.Text = "* Required";
                ErrorText.Visible = true;
                return false;
            }

            return true;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof(TagPropertyEditor), "assets/js/kalikocms.admin.taginput.min.js", SiteSettings.Instance.AdminPath + "assets/js/kalikocms.admin.taginput.min.js?v=" + Utils.VersionHash);
        }

        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);

            var tagContext = string.Empty;

            if (_attributeValues != null) {
                tagContext = _attributeValues.TagContext;
            }

            TagContext.Value = tagContext;
            ValueField.Attributes.Add("autocomplete", "off");

            var tags = TagManager.GetTags(tagContext);
            var tagList = string.Join(",", tags.Tags.Select(t => "'" + t.Value.TagName + "'"));

            Script.Text = @"<script> $(document).ready(function() { var tags = [" + tagList + "]; var tags = new Bloodhound({ datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'), queryTokenizer: Bloodhound.tokenizers.whitespace, local: $.map(tags, function(state) { return { value: state }; }) }); tags.initialize(); $('#" + ValueField.ClientID + "').tagsinput({ tagClass: 'label label-primary', typeaheadjs: {  name: 'tags', displayKey: 'value', valueKey: 'value', source: tags.ttAdapter() } }); });</script>";
        }
    }
}