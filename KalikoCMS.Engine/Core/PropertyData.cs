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

namespace KalikoCMS.Core {
    using System;
    using System.Web;
    using Extensions;
    using Serialization;

    public abstract class PropertyData : IHtmlString {

        protected abstract string StringValue { get; }

        protected abstract PropertyData DeserializeFromJson(string data);

        internal PropertyData Deserialize(string data) {
            PropertyData property = DeserializeFromJson(data);

            if (property == null) {
                var type = GetType();
                
                // TODO: Temporary fix to prevent wrongfully generic casts
                if (type.IsGenericType) {
                    return null;
                }

                property = (PropertyData)Activator.CreateInstance(type);
            }

            return property;
        }

        internal virtual string Serialize() {
            return JsonSerialization.SerializeJson(this);
        }

        public override string ToString() {
            return StringValue;
        }

        public string ToHtmlString() {
            return StringValue;
        }

        public virtual string Preview {
            get {
                var preview = StringValue.LimitCharacters(64);
                return HttpUtility.HtmlEncode(preview);
            }
        }

        public override abstract int GetHashCode();
    }
}
