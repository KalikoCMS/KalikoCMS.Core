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

namespace KalikoCMS.Core {
    using System;
    using System.Web;
    using KalikoCMS.Extensions;
    using KalikoCMS.Serialization;

    public abstract class PropertyData {

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

        public virtual string Preview {
            get {
                var preview = StringValue.LimitCharacters(64);
                return HttpUtility.HtmlEncode(preview);
            }
        }

        public override abstract int GetHashCode();
    }
}
