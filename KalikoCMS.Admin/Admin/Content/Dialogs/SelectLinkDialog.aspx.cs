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

namespace KalikoCMS.Admin.Content.Dialogs {
    using System;
    using KalikoCMS.Extensions;
    using KalikoCMS.PropertyType;

    public partial class SelectLinkDialog : System.Web.UI.Page {
        private LinkProperty.LinkType _currentType;
        private string _url;

        protected string ActiveTab {
            get {
                switch (_currentType) {
                    case LinkProperty.LinkType.File:
                        return "#file";
                    case LinkProperty.LinkType.Page:
                        return "#page";
                    default:
                        return "#external";
                }
            }
        }

        protected override void OnLoad(System.EventArgs e) {
            base.OnLoad(e);

            if (!IsPostBack) {
                ParsePostedValues();
                LoadValueFields();
            }

            SaveButton.ServerClick += SaveButtonHandler;
        }

        private void LoadValueFields() {
            switch (_currentType) {
                case LinkProperty.LinkType.Unknown:
                case LinkProperty.LinkType.External:
                    ExternalUrl.Text = _url;
                    break;
                case LinkProperty.LinkType.File:
                    FileUrl.Value = _url;
                    FileDisplayField.Text = _url;
                    break;
                case LinkProperty.LinkType.Page:
                    break;
            }
        }

        private void ParsePostedValues() {
            _url = Request.QueryString["url"];
            var typeString = Request.QueryString["type"];
            int type;

            if (int.TryParse(typeString, out type)) {
                _currentType = (LinkProperty.LinkType)type;
            }
            else {
                _currentType = LinkProperty.LinkType.Unknown;
            }
        }

        void SaveButtonHandler(object sender, System.EventArgs e) {
            string url;
            LinkProperty.LinkType linkType;

            switch (SelectedTab.Value) {
                case "#external":
                    linkType = LinkProperty.LinkType.External;
                    url = ExternalUrl.Text;
                    break;
                case "#file":
                    linkType = LinkProperty.LinkType.File;
                    url = FileUrl.Value;
                    break;
                case "#page":
                    linkType = LinkProperty.LinkType.Page;
                    url = GetUrlFromPageFields();
                    break;
                default:
                    linkType = LinkProperty.LinkType.Unknown;
                    url = string.Empty;
                    break;
            }

            CreateCallback(url, linkType);
        }

        private string GetUrlFromPageFields() {
            Guid pageId;

            if (!PageId.Value.TryParseGuid(out pageId)) {
                return string.Empty;
            }

            // TODO: Add language field when system goes multilanguage
            var page = PageFactory.GetPage(pageId);

            if (page == null) {
                return string.Empty;
            }

            return page.PageUrl.AbsolutePath;
        }

        private void CreateCallback(string url, LinkProperty.LinkType linkType) {
            PostbackResult.Text = string.Format("<script> top.executeCallback('{0}', '{1}'); top.closeModal(); </script>", url, (int)linkType);
        }
    }
}