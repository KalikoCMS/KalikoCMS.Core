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

namespace KalikoCMS.PropertyType {
    using System.Web;
    using KalikoCMS.Core;
    using KalikoCMS.Attributes;
    using KalikoCMS.Extensions;
    using KalikoCMS.Serialization;
    using MarkdownSharp;

    [PropertyType("B039C131-5F16-4C03-8ABE-1D053D1751EC", "Markdown", "HTML String", "%AdminPath%Content/PropertyType/MarkdownPropertyEditor.ascx")]
    public class MarkdownProperty : PropertyData {
        private static readonly int EmptyHashCode = string.Empty.GetHashCode();
        private static readonly Markdown Parser = new Markdown();
        private int? _cachedHashCode;

        public MarkdownProperty() {}

        public MarkdownProperty(string markdown) {
            Markdown = markdown;
            Html = Parser.Transform(markdown);
        }

        public string Markdown { get; set; }

        public string Html { get; set; }

        protected override string StringValue {
            get { return Html; }
        }

        public override string Preview {
            get {
                var preview = StringValue.StripHtml().LimitCharacters(32);
                return HttpUtility.HtmlEncode(preview);
            }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<MarkdownProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            if (Markdown == null) {
                return EmptyHashCode;
            }

            return Markdown.GetHashCode();
        }
    }
}