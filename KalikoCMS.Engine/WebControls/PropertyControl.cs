/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */

using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using KalikoCMS.Core;
using KalikoCMS.Framework;
using KalikoCMS.PropertyType;

namespace KalikoCMS.WebControls {
    public class PropertyControl : AutoBindableBase, IAttributeAccessor {

        #region Private Methods

        private string StyleTags {
            get {
                string o = string.Empty;
                if (!string.IsNullOrEmpty(CssClass)) {
                    o = " class=\"" + CssClass + "\"";
                }
                if (Style != null && Style.Count > 0) {
                    o += " style=\"" + HtmlStyle + "\"";
                }

                return o;
            }
        }

        private string HtmlStyle {
            get {
                return Style.Keys.Cast<string>().Aggregate((current, key) => current + (key + ":" + Style[key] + ";"));
            }
        }


        protected override void Render(HtmlTextWriter writer) {

            ID = ID ?? ClientID;
            Core.CmsPage page;
            //If we set 
            page = PageId == Guid.Empty ? ((PageTemplate)Page).CurrentPage : PageFactory.GetPage(PageId);

            //if(DataType == PageEditDataType.Property) {

            PropertyItem property = page.Property.GetItem(Name); //This will cause a exception if there is no element with the key "Name"

            //if(page.IsPageEditableByCurrentUser) {
            //}
            //else {
            //writer.Write(Core.PropertyType.GetPropertyType(property.PropertyTypeId).ClassInstance.Render(ref property));

//            Core.PropertyType propertyType = Core.PropertyType.GetPropertyType(property.PropertyTypeId);

            writer.Write("View:<br />");
            writer.Write(property.PropertyData);
            //writer.Write(propertyType.ClassInstance.Render(property, StyleTags));
            //writer.Write("Editable:<br />");
            //writer.Write(propertyType.ClassInstance.RenderEditable(property, StyleTags, page.PageId));


//TODO:            Page.ClientScript.RegisterStartupScript(typeof(Page), propertyType.Class, propertyType.ClassInstance.ScriptResource, true);

            //writer.Write(Core.PropertyTypeHandler.RenderEditable(ref property, page.PageID));
            //writer.Write(Core.PropertyTypeHandler.RenderEditable(ref property, page.PageID));
            //}


            /*            }
                        else //Not a property
                        {
                            //This will cause a exception if we have a faulty Name
                            StringBuilder sb;
                            if(page.IsPageEditableByCurrentUser)
                                sb = new StringBuilder("<span id=\"" + this.ID + "\"" + StyleTags + ">@datavalue</span>");
                            else
                                sb = new StringBuilder("@datavalue");

                            switch(DataType) {
                                case PageEditDataType.MetaDescription:
                                    sb.Replace("@datavalue", page.Description);
                                    break;
                                case PageEditDataType.MetaKeyWords:
                                    sb.Replace("@datavalue", page.MetaKeyWords);
                                    break;
                                case PageEditDataType.PageName:
                                    sb.Replace("@datavalue", page.PageName);
                                    break;
                                case PageEditDataType.SortOrder:
                                    sb.Replace("@datavalue", page.SortOrder.ToString());
                                    break;
                                case PageEditDataType.StartPublish:
                                    sb.Replace("@datavalue", StringFormat != string.Empty ? page.StartPublish.ToString(StringFormat) : page.StartPublish.ToString());
                                    break;
                                case PageEditDataType.StopPublish:
                                    sb.Replace("@datavalue", StringFormat != string.Empty ? page.StopPublish.ToString(StringFormat) : page.StopPublish.ToString());
                                    break;
                                case PageEditDataType.VisibleInMenus:
                                    sb.Replace("@datavalue", page.VisibleInLists.ToString());
                                    break;
                            }

                            writer.Write(sb.ToString());

                            //                if(page.IsPageEditableByCurrentUser) {
                            //                    ((PageTemplate)Page).RegisterScript(this.ID, page.PageID, DataType);
                            //                }
                        }*/
        }

        #endregion


        #region Public Properties
        /*
        [Bindable(true),
        Category("Data"),
        DefaultValue(PageEditDataType.Property)]
        public PageEditDataType DataType {
            get { return _datatype; }
            set { _datatype = value; }
        }*/

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string Name { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string StringFormat { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string Alt { get; set; }
        
        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string Text { get; set; }

        /// <summary>
        /// If we set PageId we use that page, else CurrentPage
        /// </summary>
        [Bindable(true),
         Category("Data"),
         DefaultValue(0)]
        public Guid PageId { get; set; }

        //====================================================================

        [CssClassProperty]
        [DefaultValue("")]
        public virtual string CssClass { get; set; }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CssStyleCollection Style {
            get {
                return Attributes.CssStyle;
            }
        }

        #endregion

    }
}
